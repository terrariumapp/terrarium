//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.
//------------------------------------------------------------------------------

// asmcheck.cpp : Defines the entry point for the DLL application.
//


#include "stdafx.h"
#include "stdio.h"

#pragma warning(disable: 4127 4063 4100 4189 4245 4244)
#include <corHlpr.cpp>
#pragma warning(default: 4127 4063 4100 4189 4245 4244)

#include <corerror.h>
#include <StrongName.h>
#include "asmcheck.h"
#include <memory>

// this is file location sensitive and may fail without compiler warnings
// if you relocate, please verify with sn -Tp that the assembly
// is really getting signed
#pragma managed
#using <mscorlib.dll>
#using <system.dll>

//#define RETAIL_ZOO_BUILD

#ifdef RETAIL_ZOO_BUILD
//[assembly:System::Reflection::AssemblyDelaySignAttribute(true)];
[assembly:System::Reflection::AssemblyKeyFileAttribute("..\\..\\Keys\\production.snk")];
#else
//[assembly:System::Reflection::AssemblyKeyFileAttribute("..\\..\\Keys\\development.snk")];
#endif
#pragma unmanaged

[assembly:System::Reflection::AssemblyVersionAttribute("2.0.50522.7")];

CRITICAL_SECTION osCritSec;
OSVERSIONINFOA g_osVersion;

BOOL APIENTRY DllMain( HANDLE /* hModule */, DWORD  ul_reason_for_call, LPVOID /* lpReserved */)
{
	switch (ul_reason_for_call) {
		case DLL_PROCESS_ATTACH:
			InitializeCriticalSection(&osCritSec);
			break;
	}

	return TRUE;
}


#undef OPDEF

typedef enum opcode_t {
#define OPDEF(c,s,pop,push,args,type,l,s1,s2,ctrl) c,
#include "opcode.def"
#undef OPDEF
	CEE_COUNT,			/* number of instructions and macros pre-defined */
} OPCODE;

#define IsBadInstr(n)   ((n) >= CEE_COUNT) ? 1 : _badInstrTable[(n)]

typedef enum opcode_format_t {
	InlineNone      = 0,  // no inline args
	InlineVar       = 1,  // local variable       (U2 (U1 if Short on))
	InlineI         = 2,  // an signed integer    (I4 (I1 if Short on))
	InlineR         = 3,  // a real number        (R8 (R4 if Short on))
	InlineBrTarget  = 4,  // branch target        (I4 (I1 if Short on))
	InlineI8        = 5,
	InlineMethod    = 6,   // method token (U4)
	InlineField     = 7,   // field token  (U4)
	InlineType      = 8,   // type token   (U4)
	InlineString    = 9,   // string TOKEN (U4)
	InlineSig       = 10,  // signature tok (U4)
	InlineRVA       = 11,  // ldptr token  (U4)
	InlineTok       = 12,  // a metadata token of unknown type (U4)
	InlineSwitch    = 13,  // count (U4), pcrel1 (U4) .... pcrelN (U4)
	InlinePhi       = 14,  // count (U1), var1 (U2) ... varN (U2)
	ShortInline     = 16,					      // if this bit is set, the format is the 'short' format
	PrimaryMask     = (ShortInline-1),			  // mask these off to get primary enumeration above
	ShortInlineVar  = (ShortInline + InlineVar),
	ShortInlineI    = (ShortInline + InlineI),
	ShortInlineR    = (ShortInline + InlineR),
	ShortInlineBrTarget = (ShortInline + InlineBrTarget),
} OPCODE_FORMAT;



typedef struct {
	char *  pszName;
	USHORT   Ref; // reference codes
	BYTE    Type; // Inline0 etc.
	BYTE    Len;  // std mapping
	BYTE    Std1;
	BYTE    Std2;
} opcodeinfo_t;

opcodeinfo_t OpcodeInfo[] =
{
#define OPDEF(c,s,pop,push,args,type,l,s1,s2,ctrl) s,c,args,l,s1,s2,
#include "opcode.def"
#undef OPDEF
};


OSVERSIONINFOA* InternalGetOSVersion()
{
	if ( g_osVersion.dwOSVersionInfoSize == 0 )
    {
		__try
        {
			EnterCriticalSection(&osCritSec);

			if ( g_osVersion.dwOSVersionInfoSize == 0 )
            {
				g_osVersion.dwOSVersionInfoSize = sizeof(OSVERSIONINFOA);
				GetVersionExA(&g_osVersion);
			}
		}

		__finally
        {
			LeaveCriticalSection(&osCritSec);
		}
	}

	return &g_osVersion;
}



bool IsWin9x() {
	OSVERSIONINFOA* os = InternalGetOSVersion();

	if ( NULL == os )
		return true;

	// VER_PLATFORM_WIN32_WINDOWS indicates:
	// Win95, Win98, or WinME (all ANSI)
	// VER_PLATFORM_WIN32_NT indicates:
	// NT 3.5, NT 4, Win2K, WinXP, or Windows.NET Server (all Unicode)
	return(os->dwPlatformId == VER_PLATFORM_WIN32_WINDOWS);
}


BOOL ValidateCanonicalizedName(LPCWSTR path, BOOL fForce) {
	BOOL result = FALSE;
	BOOLEAN verified = FALSE;
	BOOLEAN force = (BOOLEAN)fForce;

	BOOL success = StrongNameSignatureVerificationEx(path,
													 force,		// force verification, even if registry disallows
													 &verified);

	// if we fail, assume that it's not verifiable,
	// and therefore not loadable
	if ( success && verified )
		result = TRUE;

	return result;
}

// the return value has been new'ed, if it's non-null it must be deleted []
WCHAR* CanonicalizePath(LPCWSTR asmName) {
	LPWSTR path = NULL;

	if ( !IsWin9x() )
    {
		// canonicalize path
		LPWSTR filePart = NULL;
		DWORD dwSize = GetFullPathNameW(asmName, NULL, 0, NULL);
		if ( dwSize ) {
			path = new WCHAR[ dwSize + 1];
			dwSize = GetFullPathNameW(asmName, dwSize, path, &filePart);

			if ( dwSize == 0 ) {
				delete [] path;
				path = NULL;
			}
		}
	}
    else
    {
		size_t len = wcslen(asmName);

		LPSTR ansiAsmName = new char[ len + 1 ];
		wcstombs(ansiAsmName, asmName, len);
		ansiAsmName[ len ] = '\0';

		LPSTR filePart = NULL;
		DWORD dwSize = GetFullPathNameA(ansiAsmName, 0, NULL, NULL);
		_ASSERTE(dwSize > 0);

		if ( dwSize )
        {
			path = new WCHAR[ dwSize + 1 ];
			LPSTR ansiPath = new char[ dwSize + 1];
			dwSize = GetFullPathNameA(ansiAsmName, dwSize, ansiPath, &filePart);
			_ASSERTE(dwSize > 0);

			if ( dwSize == 0 )
            {
				delete [] path;
				path = NULL;
			}
            else
            {
				// return allocated path populated
				mbstowcs(path, ansiPath, dwSize);
				path[dwSize] = L'\0';
			}

			if ( NULL != ansiPath )
				delete [] ansiPath;
		}

		if ( NULL != ansiAsmName )
			delete [] ansiAsmName;
	}

	return path;
}

extern "C" BOOL _declspec(dllexport)ValidateStrongName(LPCWSTR asmName)
{
	BOOL result = FALSE;
	LPWSTR path = CanonicalizePath(asmName);

	if (  NULL != path )
    {
		result = ValidateCanonicalizedName(path, TRUE);
		delete [] path;
	}

	return result;
}

extern "C" BOOL _declspec(dllexport)ValidateStrongNameEx(LPCWSTR asmName, BOOLEAN fForce)
{
	BOOL result = FALSE;
	LPWSTR path = CanonicalizePath(asmName);

	if (  NULL != path ) {
		result = ValidateCanonicalizedName(path, fForce);
		delete [] path;
	}

	return result;
}

extern "C" BOOL _declspec(dllexport) CheckAssemblyEx(LPCWSTR asmName, unsigned int flags)
{
	return CheckAssemblyInternal(asmName, flags);
}

// main entry point for managed code wrapper
// returns TRUE if assembly is valid (passes all tests)
// otherwise returns false
extern "C" BOOL _declspec(dllexport) CheckAssembly(LPCWSTR asmName)
{
	return CheckAssemblyInternal(asmName, REPORT_FLAGS_NONE);
}

// main entry point for managed code wrapper when using reporting
// returns TRUE if assembly is valid (passes all tests)
// otherwise returns false
// Additionally saves an XML file at the designated file location
extern "C" BOOL _declspec(dllexport) CheckAssemblyWithReporting(LPCWSTR asmName, LPCWSTR xmlFile) {
	return CheckAssemblyInternal(asmName, xmlFile, REPORT_FLAGS_XML);
}

BOOL CheckAssemblyInternal(LPCWSTR asmName, unsigned int flags) {
	return CheckAssemblyInternal(asmName, NULL, flags);
}

BOOL CheckAssemblyInternal(LPCWSTR asmName, LPCWSTR xmlFile, unsigned int flags) {
	BOOL result = FALSE;

	LPCWSTR path = CanonicalizePath(asmName);
	_ASSERT(NULL != path);

	if ( NULL != path )
    {
		ManagedAssembly a(flags, xmlFile);

		if ( a.Validate(path) ) {
			result = TRUE;
		}
		delete [] path;

	}

    return result;
}

OPCODE DecodeOpcode(const BYTE *pCode, DWORD *pdwLen) {
	OPCODE opcode;

	*pdwLen = 1;
	opcode = OPCODE(pCode[0]);
	switch (opcode) {
		case CEE_PREFIX1:
			opcode = OPCODE(pCode[1] + 256);
			if (opcode < 0 || opcode >= CEE_COUNT)
				opcode = CEE_COUNT;
			*pdwLen = 2;
			break;
		case CEE_PREFIXREF:
		case CEE_PREFIX2:
		case CEE_PREFIX3:
		case CEE_PREFIX4:
		case CEE_PREFIX5:
		case CEE_PREFIX6:
		case CEE_PREFIX7:
			*pdwLen = 3;
			return CEE_COUNT;
	}
	return opcode;
}

const WCHAR* ManagedAssembly::_ErrorFormatStr = L"%s.%s [%s]\n";

ManagedAssembly::ManagedAssembly()
{
	ZeroInit();
	CreateBadInstructionTable();

	FinalInitialize();
}

ManagedAssembly::ManagedAssembly(unsigned int reportFlags)
{
	ZeroInit();
	CreateBadInstructionTable();

	_reportFlags = reportFlags;
	FinalInitialize();
}

ManagedAssembly::ManagedAssembly(unsigned int reportFlags, LPCWSTR xmlFile)
{
	ZeroInit();
	CreateBadInstructionTable();

	_reportFlags  = reportFlags;
	_saveFile = xmlFile;
	FinalInitialize();
}

void ManagedAssembly::FinalInitialize()
{
	// UsingXml relies on _xmlInited being true,
	// so check flags manually here and use UsingXml hereafter
	if ( _reportFlags & REPORT_FLAGS_XML ) {
		HRESULT hr = _xmlDom.CoCreateInstance(__uuidof(DOMDocument));

		if ( SUCCEEDED(hr) )
        {
			_xmlInited = true;

			if ( _saveFile == NULL ) {
				_saveFile = L"output.xml";
			}

			VARIANT_BOOL retVal = false;
			CComBSTR doc = L"<asmCheck></asmCheck>";
			_xmlDom->loadXML(doc, &retVal);

			CComBSTR bstrSS(L"asmCheck");
			_xmlDom->selectSingleNode(bstrSS, &_rootDomNode);
		}
	}
}

ManagedAssembly::~ManagedAssembly()
{
	if ( _xmlInited ) {
		CComVariant fname(_saveFile);
		_xmlDom->save(fname);
	}

	Unload();
}

void ManagedAssembly::ZeroInit()
{
	_module =  NULL;
	_file = _map = NULL;
	_dispenser = NULL;
	_import = NULL;
	_reportFlags = 0;
	_badInstrTable = NULL;
	_xmlInited = false;
}

void ManagedAssembly::Dispose()
{
	if ( NULL != _module )
    {
		UnmapViewOfFile(_module);
		_module = NULL;
	}
	if ( NULL != _map )
    {
		CloseHandle(_map);
		_map = NULL;
	}
	if ( NULL != _file )
    {
		CloseHandle(_file);
		_file = NULL;
	}

	if ( NULL != _import )
    {
		_import->Release();
		_import = NULL;
	}


	if ( NULL != _dispenser )
    {
		_dispenser->Release();
		_dispenser = NULL;
	}

	if ( NULL != _badInstrTable )
    {
		delete [] _badInstrTable;
		_badInstrTable = NULL;
	}

}

bool ManagedAssembly::LoadFile(LPCWSTR name)
{
	bool success = false;

	if ( !IsWin9x() )
    {
		_file = CreateFileW(name, GENERIC_READ, FILE_SHARE_READ,
							0, OPEN_EXISTING, 0, 0);
		_ASSERTE(_file != INVALID_HANDLE_VALUE);
		if (_file != INVALID_HANDLE_VALUE)
        {
			_map = CreateFileMappingW(_file, NULL, PAGE_READONLY, 0, 0, NULL);
			_ASSERTE(_map != NULL);
			if (_map != NULL)
            {
				_module = (HMODULE) MapViewOfFile(_map, FILE_MAP_READ, 0, 0, 0);
				_ASSERTE(_module != NULL);
				success = (_module != NULL);
			}
		}
	}
    else
    {
		size_t len = wcslen(name);
		LPSTR ansiName = new char[ len + 1 ];
		wcstombs(ansiName, name, len);
		ansiName[len] = '\0';

		_file = CreateFileA(ansiName, GENERIC_READ, FILE_SHARE_READ,
							0, OPEN_EXISTING, 0, 0);
		_ASSERTE(_file != INVALID_HANDLE_VALUE);

		if ( _file != INVALID_HANDLE_VALUE )
        {
			_map = CreateFileMappingA(_file, NULL, PAGE_READONLY, 0, 0, NULL);
			_ASSERTE(_map != NULL);

			if ( _map != NULL )
            {
				_module = (HMODULE) MapViewOfFile(_map, FILE_MAP_READ, 0, 0, 0);
				_ASSERTE(_module != NULL);
				success = (_module != NULL);
			}
		}

		if ( NULL != ansiName )
			delete [] ansiName;
	}

	return success;
}

void ManagedAssembly::CreateBadInstructionTable()
{
	if ( NULL == _badInstrTable )
		_badInstrTable = new unsigned int[ CEE_COUNT ];

	BZERO(_badInstrTable, sizeof(unsigned int) * CEE_COUNT);
	_badInstrTable[CEE_STSFLD] = 1;
}

// Insert the list of types organisms must not use into a list
// These are unauthorized because they can be used by a malicious
// (or poorly written) organism to deadlock, starve resources,
// or otherwise mess with the state of the Terrarium game.
// We just check against direct calls to banned types
// and classes derived from banned types
bool ManagedAssembly::ResolveUnauthorizedTypes()
{
	int errors = 0;

    WCHAR* bannedTypes[] = {
        L"System.Threading.Thread",
        L"System.Threading.ThreadPool",
        L"System.Activator",
        L"System.Threading.Timer",
        L"System.Threading.Mutex",
        L"System.Threading.Monitor",
        L"System.AppDomain",
        L"System.Threading.WaitHandle",
        L"System.GC",
        L"System.IntPtr",
        L"System.LocalDataStoreSlot",
        L"System.Security.SecurityManager",
        L"System.Windows.Forms.MessageBox",
        L"System.Reflection.Assembly",
        L"System.Runtime.Remoting.CallContext",
        L"System.Security.Principal",
        L"System.Drawing.Graphics",
        L"System.Drawing.Bitmap",
        L"System.Drawing.Image",
        L"System.Reflection.Binder",
        L"System.Reflection.MemberInfo",
        L"System.Reflection.MethodInfo",
        L"System.Reflection.FieldInfo",
        L"System.Security.Cryptography.SymmetricAlgorithm",
        L"System.Security.Cryptography.AsymmetricAlgorithm",
        L"System.Console",
        L"System.Diagnostics.Process",
        L"System.Diagnostics.Debug",
        L"System.Diagnostics.Debugger",
        L"System.Diagnostics.Trace",
        L"System.Diagnostics.StackTrace",
        L"System.Diagnostics.StackFrame",
        L"System.Diagnostics.ProcessThread",
        L"System.Diagnostics.ProcessModule",
        L"System.Diagnostics.TraceListener",
        L"System.Diagnostics.TraceListenerCollection",
        L"JScript 0",
        L"System.IO.Path",

    };


	for (int j = 0; j < ArraySize(bannedTypes); j++ )
    {
		_bannedTypes.insert(bannedTypes[j]);
	}

	return(0 == errors);
}

#define CHECK_HEADER(p, Struct)  {                                                      \
	if (p == NULL)                                                                      \
					  {                                                                 \
					  return false;                                                     \
					  }                                                                 \
					                                                                    \
					  if (((PBYTE)p + sizeof(Struct)) > (_base + m_dwLength))           \
					  {                                                                 \
					  return false;                                                     \
					  }                                                                 \
}

bool ManagedAssembly::CheckDosHeader()
{
	bool result = true;
	IMAGE_DOS_HEADER* dosHeader = (IMAGE_DOS_HEADER*)_module;

	if ( dosHeader->e_magic != IMAGE_DOS_SIGNATURE &&
		 0 < dosHeader->e_lfanew &&
		 dosHeader->e_lfanew < 0xFF0 )
		result = false;

	return result;
}

// Note: This method makes a best-effort attempt to validate the creature,
// make sure that its assembly is well-formed, and doesn't use any types
// that the creature should not use.
bool ManagedAssembly::Validate(LPCWSTR name) 
{
	_currentAssembly = name;
	bool success = false;
	HRESULT hr;

	if ( LoadFile(name) )
    {
		// First validate that the native OS headers haven't been modified to
        // prevent any viruses that could have been inserted there
		_base = (PVOID)_module;
		if ( CheckDosHeader() )
        {
			_headers = RtlpImageNtHeader(_module);

			hr = CoCreateInstance(CLSID_CorMetaDataDispenser, NULL,
								  CLSCTX_INPROC_SERVER,
								  IID_IMetaDataDispenser,
								  (void **) &_dispenser);
			_ASSERTE(SUCCEEDED(hr));

			if (SUCCEEDED(hr)) {
				LPCWSTR szScope = name;
				hr = _dispenser->OpenScope(szScope, 0, IID_IMetaDataImport, (IUnknown**)&_import);

                // Now walk through and make sure the animal isn't using types that are
                // banned.
				if (SUCCEEDED(hr))
                {
					ResolveUnauthorizedTypes();

					// start with globals, then walk types
					ProcessType(mdTokenNil);

					HCORENUM typeDefEnum = NULL;
					enumMgr eMgr(_import, &typeDefEnum);

					mdTypeDef typeDefs[ENUM_BUFFER_SIZE];
					ULONG count, totalCount = 1;
					HRESULT hr;

					while (SUCCEEDED(hr = _import->EnumTypeDefs( &typeDefEnum, typeDefs,
                                                                 ArraySize(typeDefs), &count)) && count > 0)
                    {
						for (ULONG i = 0; i < count; i++, totalCount++)
                        {
							ProcessType(typeDefs[i]);
						}
					}
				}
                else
                {
					_errors.FoundError(); // make sure it fails...
					ASMTRACE(L"asmcheck: Can't open scope: %s\n", name);
				}
			}

			success = _errors.GetErrorCount() == 0;
		}
	}

	ASMTRACE2(L"asmcheck: %d errors found in %s\n", _errors.GetErrorCount(), name);
	return success;
}

void ManagedAssembly::ProcessType(mdTypeDef tok) {
	if ( tok != mdTokenNil )
    {
		GetTypeName(tok, _currentType, ArraySize(_currentType));

		if ( _xmlInited )
        {
			CComBSTR rootNode = L"type";
			CComVariant elemNode(NODE_ELEMENT);
			CComPtr<IXMLDOMNode> rootDomElem;

			_xmlDom->createNode(elemNode, rootNode, NULL, &rootDomElem);

			if ( _currTypeNode.p != NULL )
            {
				_currTypeNode.Release();
			}
			_rootDomNode->appendChild(rootDomElem, &_currTypeNode);

			CComQIPtr<IXMLDOMElement> childTypeElement;
			childTypeElement = _currTypeNode;
			_currReportNode = _currTypeNode;
			childTypeElement->setAttribute(CComBSTR(L"name"), CComVariant(_currentType));

		}

		TypeCheckTree(tok, InvalidBaseClass, L"Type Definition");
		DisplayTypeDefProps(tok);
	}

	ValidateMemberTypes(tok);
}

HRESULT ManagedAssembly::GetTypeName(mdTypeDef inTypeDef, WCHAR* buffer, int len)
{
	_ASSERTE(SUCCEEDED(_import->IsValidToken(inTypeDef)));
	HRESULT hr = S_FALSE;

#ifdef META_TOKEN_NAME_CACHE
	metaNameMap::iterator it = _metaNameMap.find(inTypeDef);
	if ( it != _metaNameMap.end()) {
		wcscpy(buffer, ((*it).second).c_str());
		return NO_ERROR;
	}
#endif

	if ( !RidFromToken(inTypeDef) )
		return E_FAIL;

	switch (TypeFromToken(inTypeDef))
    {
		case mdtMemberRef:
			hr = GetMemberRefName(inTypeDef, buffer, len);
			break;

		case mdtTypeRef:
			hr = GetTypeRefName(inTypeDef, buffer, len);
			break;

		case mdtTypeDef:
			hr = GetTypeDefName(inTypeDef, buffer, len);
			break;

		default:
		case mdtTypeSpec:
		case mdtAssembly:
		case mdtAssemblyRef:
		case mdtModuleRef:
#ifdef _EMIT_DIAGNOSTICS
			wprintf(L"Just blew something off in GetTypeName\n");
#endif
			break;
	}

#ifdef META_TOKEN_NAME_CACHE
	if (SUCCEEDED(hr) && buffer[0]) {
		_metaNameMap[inTypeDef] = wideString(buffer);
	}
#endif

	return hr;
}

HRESULT ManagedAssembly::GetTypeDefBase(mdTypeDef inTypeDef, mdTypeDef& outTypeDef)
{
	mdTypeDef baseClass = mdTypeDefNil;
	HRESULT hr = E_FAIL;

	if ( TypeFromToken(inTypeDef) == mdtTypeDef )
    {
		hr = _import->GetTypeDefProps(
									 inTypeDef, NULL, 0, NULL, NULL, &baseClass);
		_ASSERTE(SUCCEEDED(hr));
	}

	if (SUCCEEDED(hr))
    {
		outTypeDef = baseClass;
	}

	return hr;
}

HRESULT ManagedAssembly::GetMemberRefName(mdTypeDef inTypeDef, WCHAR* buffer, int len )
{
	ULONG memCount = 0;

	HRESULT hr = _import->GetMemberRefProps(
										   inTypeDef,					  // [IN] given memberref
										   NULL,				   // [OUT] Put classref or classdef here.
										   buffer,				 // [OUT] buffer to fill for member's name
										   len,				 // [IN] the count of char of szMember
										   &memCount,			  // [OUT] actual count of char in member name
										   NULL,		// [OUT] point to meta data blob value
										   NULL);			// [OUT] actual size of signature blob

	_ASSERTE(SUCCEEDED(hr));
	return hr;
}

HRESULT ManagedAssembly::GetTypeDefName(mdTypeDef inTypeDef, WCHAR* buffer, int len)
{
	HRESULT hr = _import->GetTypeDefProps(
										 // [IN] The import scope.
										 inTypeDef,					 // [IN] TypeDef token for inquiry.
										 buffer,			 // [OUT] Put name here.
										 len,				  // [IN] size of name buffer in wide chars.
										 NULL,					 // [OUT] put size of name (wide chars) here.
										 NULL,					 // [OUT] Put flags here.
										 NULL);						 // [OUT] Put base class TypeDef/TypeRef here.

	_ASSERTE(SUCCEEDED(hr));
	return hr;
}

HRESULT ManagedAssembly::GetTypeDefFlags(mdTypeDef inTypeDef, DWORD* flags)
{
	HRESULT hr = _import->GetTypeDefProps(
										 // [IN] The import scope.
										 inTypeDef,					 // [IN] TypeDef token for inquiry.
										 NULL,			 // [OUT] Put name here.
										 0,				  // [IN] size of name buffer in wide chars.
										 NULL,					 // [OUT] put size of name (wide chars) here.
										 flags,					 // [OUT] Put flags here.
										 NULL);						 // [OUT] Put base class TypeDef/TypeRef here.

	_ASSERTE(SUCCEEDED(hr));
	return hr;
}

HRESULT ManagedAssembly::GetTypeRefName(mdTypeDef inTypeDef, WCHAR* buffer, int len)
{
	HRESULT hr = E_FAIL;

	hr = _import->GetTypeRefProps(
								 inTypeDef,					 // [IN] TypeDef token for inquiry.
								 NULL,
								 buffer,			 // [OUT] Put name here.
								 len,				  // [IN] size of name buffer in wide chars.
								 NULL);						 // [OUT] Put base class TypeDef/TypeRef here.

	_ASSERTE(SUCCEEDED(hr));
	return hr;
}

void ManagedAssembly::TypeCheckTree(mdToken tok, ErrorContext ctx, WCHAR* container)
{
	_typeCheckFailed = true;
	DECLARE_STR_BUFFER(className);

	if ( SUCCEEDED(GetTypeName(tok, className, ArraySize(className))))
    {
		TypeCheck(className, ctx, container);

#ifdef META_TOKEN_CACHE

		// try to see if it's in the cache of known tokens and results
		// if we don't find it, do the work
		metaTokenMap::iterator it = _tokenCache.find(tok);
		if ( it == _tokenCache.end() ) {
#endif

			// check base class types...
			mdTypeDef parentTok;
			mdTypeDef currTok = tok;
			while (SUCCEEDED(GetTypeDefBase(currTok, parentTok)))
            {
				if ( parentTok != mdTokenNil )
                {
					if (SUCCEEDED(GetTypeName(parentTok, className, ArraySize(className))))
                    {
                        if ( className != NULL && *className != L'\0' )
                        {
                            if ( !wcscmp(className, L"Animal") || !wcscmp(className, L"Plant") )
                            {
                                DWORD flags = 0;
                                GetTypeDefFlags(tok, &flags);
                                if ( !IsTdPublic(flags) )
                                {
                                    ReportError(InternalClass, _ErrorFormatStr, _currentType);
                                    _errors.FoundError();
                                }
                            }
                        }

						TypeCheck(className, ctx, container);
						currTok = parentTok;
					}
                    else
						break;
				}
			}


#ifdef META_TOKEN_CACHE
			// cache it
			_tokenCache[tok] = _typeCheckFailed;
		}
		// it was in the cache, check the result
		else
        {
			bool result = (*it).second;
			if ( !result )
            {
				_errors.FoundError();
				ReportError(ctx, _ErrorFormatStr, _currentType, container, className);
			}
		}
#endif

	}
}

void ManagedAssembly::TypeCheck(LPCWSTR className, ErrorContext ctx, WCHAR* container)
{
	if ( NULL != className && *className!= L'\0' ) {
		typeDefSet::iterator it = _bannedTypes.find(className);
		if ( it != _bannedTypes.end() )
        {
			_typeCheckFailed = true;
			_errors.FoundError();
			ReportError(ctx, _ErrorFormatStr, _currentType, container, className);
#ifdef _DEBUG
			ASMTRACE(L"Invalid type found: %s\n", className);
#endif
		}
	}
}

void ManagedAssembly::DisplayTypeDefProps(mdTypeDef inTypeDef)
{
	HRESULT hr;
	DECLARE_STR_BUFFER(buffer);
	int bufLen = sizeof(buffer) / sizeof(WCHAR);
	hr = GetTypeDefName(inTypeDef, buffer, bufLen);
}


bool ManagedAssembly::IsEmptyMethod(PBYTE pCode, DWORD dwCodeSize)
{
	bool isEmpty = false;

	if( dwCodeSize == 1 && NULL != pCode )
    {
		DWORD len = 0;
		OPCODE instr = DecodeOpcode(pCode, &len);
		if( instr == CEE_RET ) {
			isEmpty = true;
		}
	}

	return isEmpty;
}


void ManagedAssembly::CheckMethodCode(PBYTE pCode, DWORD dwCodeSize, DWORD	/* codeRVA */)
{
	unsigned int instrPtr = 0;

	while (instrPtr < dwCodeSize)
    {
		DWORD   Len;
		OPCODE  instr;

#ifdef _EMIT_DIAGNOSTICS
#endif
		DECLARE_STR_BUFFER(instrBuff);
		DECLARE_STR_BUFFER(buffer);
		int bufLen = ArraySize(buffer);
		size_t cnt, maxCnt;
		const WCHAR wideNull = L'\0';

		instr = DecodeOpcode(&pCode[instrPtr], &Len);

#ifdef _EMIT_INSTRUCTIONS
#define OUTPUT_INSTR(v)
#define OUTPUT_REF(r)

#else
#define OUTPUT_INSTR(v)
#define OUTPUT_REF(r)
#endif

		instrBuff[0] = wideNull;
		maxCnt = strlen(OpcodeInfo[instr].pszName);
		cnt = ArraySize(instrBuff);
		maxCnt = min(cnt, maxCnt);
		cnt = mbstowcs(instrBuff, OpcodeInfo[instr].pszName, maxCnt );
		if ( cnt == maxCnt )
			instrBuff[ cnt - 1] = wideNull;

		if ( IsBadInstr(instr) )
        {
			_errors.FoundError();
			ReportError(BadInstruction, _ErrorFormatStr, _currentType, _currentMember, instrBuff);
		}

		instrPtr += Len;
		switch (OpcodeInfo[instr].Type) {
			default:
				break;

			case InlineNone:
				OUTPUT_INSTR(L"InlineNone");
				break;

			case ShortInlineI:
			case ShortInlineVar:
				OUTPUT_INSTR(L"ShortInline");
				instrPtr++;
				break;

			case InlineVar:
				OUTPUT_INSTR(L"InlineVar");
				instrPtr += 2;
				break;

			case InlineI:
			case InlineRVA:
				OUTPUT_INSTR(L"InlineRVA");
				instrPtr += 4;
				break;

			case InlineI8:
				OUTPUT_INSTR(L"InlineI8");
				instrPtr += 8;
				break;

			case ShortInlineR:
				OUTPUT_INSTR(L"ShortInlineR");
				instrPtr += 4;
				break;

			case InlineR:
				OUTPUT_INSTR(L"InlineR");
				instrPtr += 8;
				break;

			case ShortInlineBrTarget:
				OUTPUT_INSTR(L"ShortInlineBrTarget");
				instrPtr++;
				break;

			case InlineBrTarget:
				OUTPUT_INSTR(L"InlineBrTarget");
				instrPtr+=4;
				break;

			case InlineSwitch: {
					OUTPUT_INSTR(L"InlineSwitch");
					DWORD numCases = pCode[instrPtr] + (pCode[instrPtr+1] << 8) +
									 (pCode[instrPtr+2] << 16) + (pCode[instrPtr+3] << 24);
					instrPtr+=4;
					for ( unsigned i = 0; i < numCases; i++ ) {
						instrPtr += 4;
					}
				}
				break;

			case InlinePhi: {
					DWORD cases = pCode[instrPtr];
					OUTPUT_INSTR(L"InlinePhi");
					instrPtr += 2 * cases + 1;
					break;
				}

			case InlineString:
			case InlineField:
			case InlineType:
			case InlineTok:
			case InlineMethod:
				{
					DWORD tk;
					DWORD tkType;
					tk = pCode[instrPtr] + (pCode[instrPtr+1] << 8) +
						 (pCode[instrPtr+2] << 16) + (pCode[instrPtr+3] << 24);
					tkType = TypeFromToken(tk);
					OUTPUT_INSTR(L"InlineSwitch");

					if (OpcodeInfo[instr].Type== InlineTok) {

						switch (tkType) {
							case mdtMemberRef:
								break;

							case mdtFieldDef:
								break;

							case mdtMethodDef:
								break;
						}
					} else {
						switch (tkType) {
							case mdtTypeDef:
							case mdtTypeRef:
							case mdtTypeSpec: {
								}
								break;

							case mdtMemberRef:
								{
									mdToken classTok = mdTokenNil;
									HRESULT hr = _import->GetMemberRefProps(
																		   tk,			  // [IN] TypeDef token for inquiry.
																		   &classTok,		 // [OUT] Put classref or classdef here.
																		   buffer,
																		   bufLen,
																		   NULL,
																		   NULL,
																		   NULL);

									_ASSERTE(SUCCEEDED(hr));
									if ( SUCCEEDED(hr)) {

										hr = GetTypeName(classTok, instrBuff, sizeof(instrBuff)/sizeof(WCHAR));
										_ASSERTE(SUCCEEDED(hr));
										OUTPUT_REF(instrBuff);
										OUTPUT_REF(L"::");
										OUTPUT_REF(buffer);
										TypeCheckTree(classTok, InvalidCall, buffer);
									}
								}
								break;

							case mdtString: {
								}
								break;


							case mdtFieldDef: {
									mdTypeDef classTok = mdTokenNil;
									HRESULT hr = _import->GetFieldProps(tk, &classTok, buffer, bufLen,
																		NULL, NULL, NULL, NULL, NULL, NULL, NULL);
									hr = GetTypeName(classTok, instrBuff, sizeof(instrBuff)/sizeof(WCHAR));
									_ASSERTE(SUCCEEDED(hr));
									OUTPUT_REF(instrBuff);
									OUTPUT_REF(L" ");
									OUTPUT_REF(buffer);
									TypeCheckTree(classTok, InvalidField, buffer);
								}

								break;

							case mdtMethodDef:{
									mdTypeDef classTok = mdTokenNil;
									// do method code here
									HRESULT hr = _import->GetMemberProps(tk, &classTok, buffer, bufLen,
																		 NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL);
									_ASSERTE(SUCCEEDED(hr));

									if (SUCCEEDED(hr)) {
										hr = GetTypeName(classTok, instrBuff, sizeof(instrBuff)/sizeof(WCHAR));
										if (SUCCEEDED(hr)) {
											OUTPUT_REF(instrBuff);
											OUTPUT_REF(L"::");
											OUTPUT_REF(buffer);
											TypeCheckTree(classTok, InvalidCall, buffer);
										}
									}
								}

								break;

						}
					}


					instrPtr+=4;
					break;
				}

			case InlineSig:
				OUTPUT_INSTR(L"InlineSig");
				instrPtr+=4;
				break;
		}

#ifdef _EMIT_DIAGNOSTICS
#endif

	}
}

void ManagedAssembly::CheckFieldAttrs(DWORD dwAttrs, WCHAR* buffer)
{
	// constants/literals are OK, other static fields aren't
	if (IsFdStatic(dwAttrs) && !IsFdLiteral(dwAttrs) ) {
		ReportError(StaticField, _ErrorFormatStr, _currentType, buffer, buffer);
		_errors.FoundError();
	}
}


void ManagedAssembly::CheckFieldType(mdTypeDef	/* classType */, WCHAR* fieldName, PCCOR_SIGNATURE pCorSig, ULONG /* sigSize */) {
	mdToken sigTok;

	if ( SigHasClassType(pCorSig, &sigTok) ) {
		TypeCheckTree(sigTok, InvalidField, fieldName);
	}
}

// Don't allow pinvokes, methods with security attributes or static constructors
void ManagedAssembly::CheckMethodAttrs(DWORD dwAttrs, WCHAR* buffer, bool isEmpty) {
	if (IsMdPinvokeImpl(dwAttrs)) {
		ReportError(PinvokeMethod, _ErrorFormatStr, _currentType, buffer, buffer);
		_errors.FoundError();
	}

	if (IsMdHasSecurity(dwAttrs)) {
		ReportError(HasSecurityMethod, _ErrorFormatStr, _currentType, buffer, buffer);
		_errors.FoundError();
	}
	if (IsMdRequireSecObject(dwAttrs)) {
		ReportError(RequiresSecObjectMethod, _ErrorFormatStr, _currentType, buffer, buffer);
		_errors.FoundError();
	}

	if (IsMdClassConstructorW(dwAttrs, buffer) && !isEmpty) {
		ReportError(ClassConstructor, _ErrorFormatStr, _currentType, buffer, buffer);
		_errors.FoundError();
	}
}

bool ManagedAssembly::SigHasClassType(PCCOR_SIGNATURE typeSig, mdToken* tok, bool stripCallConv) {
	bool repeatLoop;
	bool result = false;
	int typ;
	unsigned callConv;

	if (stripCallConv)
		callConv = CorSigUncompressData(typeSig);

	if ( NULL != tok )
		*tok = mdTokenNil;

	do {
		repeatLoop = false;
		switch (typ = *typeSig++) {
			case ELEMENT_TYPE_VALUETYPE:
			case ELEMENT_TYPE_CLASS:
				result = true;
				if ( NULL != tok ) {
					*tok = CorSigUncompressToken(typeSig);
				}
				break;
		}
	}while (repeatLoop);

	return result;
}

void ManagedAssembly::SigToString(PCCOR_SIGNATURE sig, ULONG /* sigSize */, WCHAR* buffer, int maxLen) {
	BZERO(buffer, maxLen);
	int typ;
	WCHAR* str;
	mdToken  tk;
	DECLARE_STR_BUFFER(className);

	// we're intentionally not using this variable...
	unsigned callConv = CorSigUncompressData(sig);
	UNUSED(callConv);

	HRESULT hr;

	bool repeatLoop = true;
	do
    {
		repeatLoop = false;

		switch (typ = *sig++)
        {
			case ELEMENT_TYPE_VOID          :
				str = L"void"; goto APPEND;
			case ELEMENT_TYPE_BOOLEAN       :
				str = L"bool"; goto APPEND;
			case ELEMENT_TYPE_CHAR          :
				str = L"char"; goto APPEND;
			case ELEMENT_TYPE_I1            :
				str = L"int8"; goto APPEND;
			case ELEMENT_TYPE_U1            :
				str = L"unsigned int8"; goto APPEND;
			case ELEMENT_TYPE_I2            :
				str = L"int16"; goto APPEND;
			case ELEMENT_TYPE_U2            :
				str = L"unsigned int16"; goto APPEND;
			case ELEMENT_TYPE_I4            :
				str = L"int32"; goto APPEND;
			case ELEMENT_TYPE_U4            :
				str = L"unsigned int32"; goto APPEND;
			case ELEMENT_TYPE_I8            :
				str = L"int64"; goto APPEND;
			case ELEMENT_TYPE_U8            :
				str = L"unsigned int64"; goto APPEND;
			case ELEMENT_TYPE_R4            :
				str = L"float32"; goto APPEND;
			case ELEMENT_TYPE_R8            :
				str = L"float64"; goto APPEND;
			case ELEMENT_TYPE_U             :
				str = L"native unsigned int"; goto APPEND;
			case ELEMENT_TYPE_I             :
				str = L"native int"; goto APPEND;
			case ELEMENT_TYPE_OBJECT        :
				str = L"object"; goto APPEND;
			case ELEMENT_TYPE_STRING        :
				str = L"string"; goto APPEND;
			case ELEMENT_TYPE_TYPEDBYREF        :
				str = L"typedref"; goto APPEND;
				APPEND:
				wcscat(buffer, str);
				break;

			case ELEMENT_TYPE_VALUETYPE    :
			case ELEMENT_TYPE_CLASS         :
				sig += CorSigUncompressToken(sig, &tk);
				hr = GetTypeName(tk, className, ArraySize(className));
				_ASSERTE(SUCCEEDED(hr));

				if (SUCCEEDED(hr) )
                {
					wcscat(buffer, className);
				}
				break;

			default:
			case ELEMENT_TYPE_SENTINEL      :
			case ELEMENT_TYPE_END           :
				break;
		}
	}
    while (repeatLoop);
}

void ManagedAssembly::ValidateMemberTypes(mdToken tkType)
{
	HRESULT hr;
	HCORENUM memEnum = NULL;
	ULONG count = 0;
	mdMemberRef memRefs[ENUM_BUFFER_SIZE];
	mdTypeDef classType;
	DWORD bufLen = ArraySize(_currentMember);
	DWORD implFlags = 0;
	DWORD dwAttrs = 0;
	mdMemberRef currRef;
	PCCOR_SIGNATURE pCorSig = NULL;
	ULONG sigSize = 0;
	PIMAGE_SECTION_HEADER   pSectionHeader;
	COR_ILMETHOD*       pimHeader = NULL;
	DWORD codeRVA = 0;
	DWORD dwCodeSize;
	PBYTE pbCode;

	enumMgr eMgr(_import, &memEnum);

	while (SUCCEEDED(hr = _import->EnumMembers(&memEnum, tkType,
											   memRefs, ArraySize(memRefs), &count)) && count > 0 )
    {
		for (ULONG i = 0; i < count; i++ )
        {
			currRef = memRefs[i];
			hr = _import->GetMemberProps(currRef, &classType, _currentMember,
										 bufLen, NULL, &dwAttrs, &pCorSig, &sigSize, &codeRVA, &implFlags, NULL, NULL, NULL);

			if ( SUCCEEDED(hr) )
            {
				if ( _xmlInited )
                {
					OutputDebugString(L"Processing Member\n");
					CComBSTR rootNode = L"member";
					CComVariant elemNode(NODE_ELEMENT);
					CComPtr<IXMLDOMNode> rootDomElem;

					_xmlDom->createNode(elemNode, rootNode, NULL, &rootDomElem);
					if ( _currMemberNode.p != NULL ) {
						_currMemberNode.Release();
					}
					_currTypeNode->appendChild(rootDomElem, &_currMemberNode);

					CComQIPtr<IXMLDOMElement> childMemberElement;
					childMemberElement = _currMemberNode;
					_currReportNode = _currMemberNode;
					childMemberElement->setAttribute(CComBSTR(L"name"), CComVariant(_currentMember));
				}

				switch ( TypeFromToken(currRef))
                {
					case mdtFieldDef:
						CheckFieldAttrs(dwAttrs, _currentMember);
						CheckFieldType(currRef, _currentMember, pCorSig, sigSize);
						break;

					case mdtProperty:
					case mdtMethodDef:
                        {
							bool isEmpty = false;
							pSectionHeader = (PIMAGE_SECTION_HEADER) RtlImageRvaToVa(_headers, _module, codeRVA, NULL);
							if ( NULL != pSectionHeader)
                            {
								pimHeader = (COR_ILMETHOD*) (pSectionHeader);
								if(( ((size_t)pimHeader) & 3) != 0)
                                {
                                    ReportError(MisalignedMethodHeader, _ErrorFormatStr, _currentType);
									break;
								}

								COR_ILMETHOD_DECODER imdHeader( (const COR_ILMETHOD*)pimHeader );

								dwCodeSize = (DWORD)imdHeader.CodeSize;
								pbCode = (PUCHAR)imdHeader.Code;
								CheckMethodCode(pbCode, dwCodeSize, codeRVA);
								isEmpty = IsEmptyMethod(pbCode, dwCodeSize);
							}

							CheckMethodAttrs(dwAttrs, _currentMember, isEmpty);
						}
						break;

					case mdtEvent:
						break;
				}
			}
            else
            {
				// handle GetMemberProps failure
				_errors.FoundError();
			}
		}
	}


}

void ManagedAssembly::Unload()
{
	Dispose();
}

void* ManagedAssembly::RtlImageRvaToVa(
							   PIMAGE_NT_HEADERS NtHeaders,
							   void* Base,
							   ULONG Rva,
							   PIMAGE_SECTION_HEADER *LastRvaSection
							   )
{
	PIMAGE_SECTION_HEADER NtSection;

	if (( !LastRvaSection ) ||
		(NtSection = *LastRvaSection) == NULL ||
		Rva < NtSection->VirtualAddress ||
		Rva >= NtSection->VirtualAddress + NtSection->SizeOfRawData
	   )
    {
		NtSection = RtlImageRvaToSection( NtHeaders, Base, Rva);
	}

	if (NtSection != NULL)
    {
		if (LastRvaSection != NULL)
        {
			*LastRvaSection = NtSection;
		}

		return(PVOID)((PCHAR)Base +
					  (Rva - NtSection->VirtualAddress) +
					  NtSection->PointerToRawData
					 );
	}
    else
    {
		return NULL;
	}
}




PIMAGE_SECTION_HEADER ManagedAssembly::RtlImageRvaToSection(PIMAGE_NT_HEADERS NtHeaders, PVOID	/* Base */, ULONG Rva) {
	ULONG i;
	PIMAGE_SECTION_HEADER NtSection;

	NtSection = IMAGE_FIRST_SECTION( NtHeaders );
	for (i=0; i<NtHeaders->FileHeader.NumberOfSections; i++)
    {
		if (Rva >= NtSection->VirtualAddress &&
			Rva < NtSection->VirtualAddress + NtSection->SizeOfRawData
		   )
        {
			return NtSection;
		}
		++NtSection;
	}
	return NULL;
}

void ManagedAssembly::ReportError(ErrorContext ctx, LPCWSTR formatStr, ...)
{
	ASMTRACE4(L"asmcheck: [Error] %s in %s.%s (%s)\n",
			  AssemblyErrorInfo::GetErrorString(ctx),
			  _currentType, _currentMember, _currentAssembly);

	if ( Reporting() )
    {
		va_list marker;
		va_start(marker, formatStr);
		wprintf(L"*%s: ", AssemblyErrorInfo::GetErrorString(ctx));
		vwprintf(formatStr, marker);
		va_end(marker);
	}

	if ( UsingXml() )
    {
		CComBSTR rootNode = L"error";
		CComVariant elemNode(NODE_ELEMENT);
		CComPtr<IXMLDOMNode> rootDomElem;

		_xmlDom->createNode(elemNode, rootNode, NULL, &rootDomElem);

		CComPtr<IXMLDOMNode> errorNode;
		_currReportNode->appendChild(rootDomElem, &errorNode);

		CComQIPtr<IXMLDOMElement> childElement;
		childElement = errorNode;
		childElement->setAttribute(CComBSTR(L"name"), CComVariant(AssemblyErrorInfo::GetErrorString(ctx)));
	}
}


// AssemblyErrorInfo methods
AssemblyErrorInfo::AssemblyErrorInfo()
{
	_errorCount = 0;
}

static WCHAR*
errorContexts[] = {
	L"An unexpected assembly validation error occurred",
	L"You are calling a class that isn't allowed",
	L"You are using a field that is of a type that isn't allowed",
	L"You are deriving from a base class that isn't allowed",
	L"Static methods aren't allowed and you have one",
	L"Pinvoke calls aren't allowed and you have one",
	L"Has Security Method",
	L"RequiresSecObject Method",
	L"Class constructors aren't allowed and you have one",
	L"Static fields aren't allowed and you have one",
	L"Exception handlers aren't allowed and you have one",
	L"You have IL instructions that aren't allowed",
	L"Your assembly is not a managed assembly",
    L"Class derived from Animal or Plant must be marked public"
	L"Your assembly has a misaligned method header within it"
};

const WCHAR* AssemblyErrorInfo::GetErrorString(ErrorContext ctx)
{
	int _ctx = (int)ctx;
	if ( _ctx < ArraySize(errorContexts))
    {
		return errorContexts[_ctx];
	}
	return errorContexts[0];
}


#ifdef _DEBUG
void OutputDebugStringFmt( LPCWSTR lpszFormat, ... ) {
	WCHAR    lpszBuffer[STRING_BUFFER_LEN*2];
	va_list  marker;

	va_start( marker, lpszFormat );
	_vsnwprintf( lpszBuffer, STRING_BUFFER_LEN*2, lpszFormat, marker );
	va_end( marker );
	OutputDebugString( lpszBuffer );
}
#endif

#pragma managed
