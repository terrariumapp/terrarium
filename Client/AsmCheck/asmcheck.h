//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.
//------------------------------------------------------------------------------
#pragma once
#pragma unmanaged
#pragma warning(disable: 4701) // Disable warnings in STL xstring header


#include <set>
#include <map>
#include <corerror.h>

#include <string>
#include <xstring>
#include <hash_set>
#include <hash_map>
#include <xhash>

#define BZERO(buff, size) ZeroMemory(buff, size)

unsigned long StringHash ( const wchar_t *name )
{
    unsigned long   h = 0, g;
    while ( *name )
    {
        h = ( h << 4 ) + *name++;
        if ( (g = (h & 0xF0000000)) != 0 )
            h ^= g >> 24;
        h &= ~g;
    }
    return h;
}

typedef std::basic_string<wchar_t> wideString;

class HashHelper : public stdext::hash_compare<wideString> {
public:
	size_t operator()(const wchar_t* hash) const {
		return StringHash(hash);
	}

	size_t operator()(const wideString& hash) const {
		return StringHash(hash.c_str());
	}

	bool operator()(const wchar_t* lhs, const wchar_t* rhs) const {
		return wcscmp(lhs, rhs) == 0;
	}

	bool operator()(const wideString& lhs, const wideString& rhs) const {
		return lhs == rhs;
	}

};

typedef std::set<wideString> typeDefSet;
typedef std::map<mdToken, bool> metaTokenMap;
typedef std::map<mdToken, wideString> metaNameMap;

#ifdef DEBUG
// tracing, debugging helpers
void OutputDebugStringFmt( LPCWSTR lpszFormat, ... );

#define ASMTRACE(fmt, arg) OutputDebugStringFmt(fmt, arg)
#define ASMTRACE2(fmt, arg1, arg2) OutputDebugStringFmt(fmt, arg1, arg2)
#define ASMTRACE3(fmt, arg1, arg2, arg3) OutputDebugStringFmt(fmt, arg1, arg2, arg3)
#define ASMTRACE4(fmt, arg1, arg2, arg3, arg4) OutputDebugStringFmt(fmt, arg1, arg2, arg3, arg4)
#else
#define ASMTRACE(fmt, arg) ((void)0)
#define ASMTRACE2(fmt, arg1, arg2) ((void)0)
#define ASMTRACE3(fmt, arg1, arg2, arg3) ((void)0)
#define ASMTRACE4(fmt, arg1, arg2, arg3, arg4) ((void)0)
#endif


#define UNUSED(v)  ((void)v)

// enable use of metaTokenMap based token cache
// to cache the results of invalid type lookups
#define META_TOKEN_CACHE

// enable use of token to name caching at the app layer
#define META_TOKEN_NAME_CACHE

#define REPORT_FLAGS_NONE    0x00000000
#define REPORT_FLAGS_CONSOLE 0x00000001
#define REPORT_FLAGS_XML     0x00000002

#define DECLARE_STR_BUFFER(nm) WCHAR nm[STRING_BUFFER_LEN]

// forward defs
BOOL CheckAssemblyInternal(LPCWSTR asmName, unsigned int flags);
BOOL CheckAssemblyInternal(LPCWSTR asmName, LPCWSTR xmlFile, unsigned int flags);

// helper class to prevent against enum handle leaks
class enumMgr {
private:
	IMetaDataImport* _mdImport;
	HCORENUM* _corEnum;

public:
	enumMgr(IMetaDataImport* metaDataDisp, HCORENUM* corEnum) {
		_mdImport = metaDataDisp;
		_corEnum = corEnum;
	}

	~enumMgr() {
		if ( NULL != _corEnum && *_corEnum ) {
			_mdImport->CloseEnum(*_corEnum);
			*_corEnum = NULL;
		}
	}
};


// uncomment to emit IL dumps for testing
//#define _EMIT_DIAGNOSTICS

//#define _EMIT_INSTRUCTIONS

#define ArraySize(s) (sizeof(s) / sizeof(s[0]))
#define ENUM_BUFFER_SIZE 10
#define STRING_BUFFER_LEN 1024
#define	NEW_TRY_BLOCK	0x80000000
#define PUT_INTO_CODE	0x40000000
#define ERR_OUT_OF_CODE	0x20000000
#define SEH_NEW_PUT_MASK	(NEW_TRY_BLOCK | PUT_INTO_CODE | ERR_OUT_OF_CODE)

enum ErrorContext {
	UnknownContext = 0,
	InvalidCall,
	InvalidField,
	InvalidBaseClass,
	StaticMethod,
	PinvokeMethod,
	HasSecurityMethod,
	RequiresSecObjectMethod,
	ClassConstructor,
	StaticField,
	ExceptionHandlers,
	BadInstruction,
	UnmanagedAssembly,
    InternalClass,
	MisalignedMethodHeader
};

class AssemblyErrorInfo {
private:
	int _errorCount;

public:

	static const WCHAR* GetErrorString(ErrorContext ctx);

	AssemblyErrorInfo();
	void FoundError() {
		_errorCount++;
	}
	int GetErrorCount() {
		return _errorCount;
	}
};


class ManagedAssembly {
private:
	HMODULE _module;
	HANDLE  _file;
	HANDLE  _map;
	IMetaDataDispenserEx* _dispenser;
	IMetaDataImport *_import;
	typeDefSet _bannedTypes;

	unsigned int _reportFlags;
	unsigned int* _badInstrTable;



	////////////////////////////////////
	// XML support, enabled when REPORTS_FLAGS_XML is set
	bool _xmlInited;
	CComPtr<IXMLDOMDocument> _xmlDom;
	CComPtr<IXMLDOMNode> _rootDomNode;

	CComPtr<IXMLDOMNode> _currTypeNode;
	CComPtr<IXMLDOMNode> _currMemberNode;
	CComPtr<IXMLDOMNode> _currReportNode;

	inline bool UsingXml() {
		return (_reportFlags & REPORT_FLAGS_XML) && _xmlInited;
	}
	////////////////////////////////////

	inline bool Reporting() {
		return _reportFlags & REPORT_FLAGS_CONSOLE;
	}

	AssemblyErrorInfo _errors;

	// pointer to arg passed to Validate: do not delete
	LPCWSTR _currentAssembly;
	LPCWSTR _saveFile;
	WCHAR _currentType[STRING_BUFFER_LEN];
	WCHAR _currentMember[STRING_BUFFER_LEN];
	metaTokenMap _tokenCache;
	bool _typeCheckFailed;


#ifdef META_TOKEN_NAME_CACHE
	metaNameMap _metaNameMap;
#endif

	static const WCHAR* _ErrorFormatStr;

	PVOID _base;
	PIMAGE_NT_HEADERS _headers;
	void DisplayTypeDefProps(mdTypeDef inTypeDef);
	void CheckMethodCode(PBYTE pbCode, DWORD dwCodeSize, DWORD codeRVA);
	void ProcessType(mdToken tok);
	void Unload();
	void ZeroInit();
	void FinalInitialize();
	void Dispose();
	bool LoadFile(LPCWSTR name);
	void* RtlImageRvaToVa(PIMAGE_NT_HEADERS NtHeaders, void* Base, ULONG Rva, PIMAGE_SECTION_HEADER *LastRvaSection);
	HRESULT GetTypeDefName(mdTypeDef, WCHAR* buffer, int len);
	HRESULT GetTypeDefFlags(mdTypeDef inTypeDef, DWORD* flags);
	HRESULT GetTypeRefName(mdTypeDef inTypeDef, WCHAR* buffer, int len);
	HRESULT GetTypeName(mdTypeDef inTypeDef, WCHAR* buffer, int len);
	HRESULT GetMemberRefName(mdTypeDef inTypeDef, WCHAR* buffer, int len );
	bool ResolveUnauthorizedTypes();
	void TypeCheck(LPCWSTR className, ErrorContext ctx = UnknownContext, WCHAR* container = NULL);
	void TypeCheckTree(mdToken tok, ErrorContext ctx = UnknownContext, WCHAR* container = NULL);
	bool CheckDosHeader();
	void ValidateMemberTypes(mdToken tkType);
	void CheckMethodAttrs(DWORD dwAttrs, WCHAR* buffer, bool isEmpty);
	void CheckFieldAttrs(DWORD dwAttrs, WCHAR* buffer);
	void CheckFieldType(mdTypeDef classType, WCHAR* buffer, PCCOR_SIGNATURE pCorSig, ULONG sigSize);
	HRESULT GetTypeDefBase(mdTypeDef inTypeDef, mdTypeDef& outTypeDef);
	void SigToString(PCCOR_SIGNATURE sig, ULONG sigSize, WCHAR* buff, int maxLen);
	bool SigHasClassType(PCCOR_SIGNATURE sig, mdToken* tok = NULL, bool stripCallConv = true);
	bool IsEmptyMethod(PBYTE pCode, DWORD dwCodeSize);

	void ReportError(ErrorContext ctx, LPCWSTR formatStr, ...);
	void CreateBadInstructionTable();

	PIMAGE_SECTION_HEADER RtlImageRvaToSection(PIMAGE_NT_HEADERS NtHeaders, PVOID Base, ULONG Rva);
public:
	ManagedAssembly();
	ManagedAssembly(unsigned int reportFlags);
	ManagedAssembly(unsigned int reportFlags, LPCWSTR xmlFile);
	~ManagedAssembly();

	bool Validate(LPCWSTR name);
};

__inline
PIMAGE_NT_HEADERS
NTAPI RtlpImageNtHeader (IN PVOID Base) {
	PIMAGE_NT_HEADERS NtHeaders = NULL;
#ifndef _MAC
	if (Base != NULL && Base != (PVOID)-1) {
		__try {
			if ((((PIMAGE_DOS_HEADER)Base)->e_magic == IMAGE_DOS_SIGNATURE) &&
				((ULONG)((PIMAGE_DOS_HEADER)Base)->e_lfanew < 0x10000000)) { // 256 MB
				NtHeaders = (PIMAGE_NT_HEADERS)((PCHAR)Base + ((PIMAGE_DOS_HEADER)Base)->e_lfanew);
				if (NtHeaders->Signature != IMAGE_NT_SIGNATURE) {
					NtHeaders = NULL;
				}
			}
		}
		__except(EXCEPTION_EXECUTE_HANDLER) {
			NtHeaders = NULL;
		}
	}
#endif //_MAC
	return NtHeaders;
}

