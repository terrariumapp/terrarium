//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                               
//------------------------------------------------------------------------------

namespace Terrarium.Net
{
    public class NativeSocketErrors
    {
        /*
        * All Windows Sockets error constants are biased by WSABASEERR from
        * the "normal"
        */
        const int WSABASEERR             = 10000;

        /*
        * Windows Sockets definitions of regular Microsoft C error constants
        */
        // A blocking socket call was canceled.
        const int WSAEINTR               = (WSABASEERR+4);
        const int WSAEBADF               = (WSABASEERR+9);
        // Permission denied.
        const int WSAEACCES              = (WSABASEERR+13);
        // Bad address.
        const int WSAEFAULT              = (WSABASEERR+14);
        // Invalid argument.
        const int WSAEINVAL              = (WSABASEERR+22);
        // Too many open files.
        const int WSAEMFILE              = (WSABASEERR+24);
    
        /*
        * Windows Sockets definitions of regular Berkeley error constants
        */

        // Resource temporarily unavailable.
        const int WSAEWOULDBLOCK         = (WSABASEERR+35);
        // Operation now in progress.
        const int WSAEINPROGRESS         = (WSABASEERR+36);
        // Operation already in progress.
        const int WSAEALREADY            = (WSABASEERR+37);
        // Socket operation on nonsocket.
        const int WSAENOTSOCK            = (WSABASEERR+38);
        // Destination address required.
        const int WSAEDESTADDRREQ        = (WSABASEERR+39);
        // Message too long.
        const int WSAEMSGSIZE            = (WSABASEERR+40);
        // Protocol wrong type for socket.
        const int WSAEPROTOTYPE          = (WSABASEERR+41);
        // Bad protocol option.
        const int WSAENOPROTOOPT         = (WSABASEERR+42);
        // Protocol not supported.
        const int WSAEPROTONOSUPPORT     = (WSABASEERR+43);
        // Socket type not supported.
        const int WSAESOCKTNOSUPPORT     = (WSABASEERR+44);
        // Operation not supported.
        const int WSAEOPNOTSUPP          = (WSABASEERR+45);
        // Protocol family not supported.
        const int WSAEPFNOSUPPORT        = (WSABASEERR+46);
        // Address family not supported by protocol family.
        const int WSAEAFNOSUPPORT        = (WSABASEERR+47);
        // Address already in use.
        const int WSAEADDRINUSE          = (WSABASEERR+48);
        // Cannot assign requested address.
        const int WSAEADDRNOTAVAIL       = (WSABASEERR+49);
        // Network is down.
        const int WSAENETDOWN            = (WSABASEERR+50);
        // Network is unreachable.
        const int WSAENETUNREACH         = (WSABASEERR+51);
        // Network dropped connection on reset.
        const int WSAENETRESET           = (WSABASEERR+52);
        // Software caused connection to abort.
        const int WSAECONNABORTED        = (WSABASEERR+53);
        // Connection reset by peer.
        const int WSAECONNRESET          = (WSABASEERR+54);
        // No buffer space available.
        const int WSAENOBUFS             = (WSABASEERR+55);
        // Socket is already connected.
        const int WSAEISCONN             = (WSABASEERR+56);
        // Socket is not connected.
        const int WSAENOTCONN            = (WSABASEERR+57);
        // Cannot send after socket shutdown.
        const int WSAESHUTDOWN           = (WSABASEERR+58);
        const int WSAETOOMANYREFS        = (WSABASEERR+59);
        // Connection timed out.
        const int WSAETIMEDOUT           = (WSABASEERR+60);
        // Connection refused.
        const int WSAECONNREFUSED        = (WSABASEERR+61);
        const int WSAELOOP               = (WSABASEERR+62);
        const int WSAENAMETOOLONG        = (WSABASEERR+63);
        // Host is down.
        const int WSAEHOSTDOWN           = (WSABASEERR+64);
        // No route to host.
        const int WSAEHOSTUNREACH        = (WSABASEERR+65);
        const int WSAENOTEMPTY           = (WSABASEERR+66);
        // Too many processes.
        const int WSAEPROCLIM            = (WSABASEERR+67);
        const int WSAEUSERS              = (WSABASEERR+68);
        const int WSAEDQUOT              = (WSABASEERR+69);
        const int WSAESTALE              = (WSABASEERR+70);
        const int WSAEREMOTE             = (WSABASEERR+71);

        // Graceful shutdown in progress.
        const int WSAEDISCON             = (WSABASEERR+101);

        /*
        * Extended Windows Sockets error constant definitions
        */

        // Network subsystem is unavailable.
        const int WSASYSNOTREADY         = (WSABASEERR+91);
        // Winsock.dll out of range.
        const int WSAVERNOTSUPPORTED     = (WSABASEERR+92);
        // Successful startup not yet performed.
        const int WSANOTINITIALISED      = (WSABASEERR+93);

        /*
        * Winsock 2 Defines this value just to be ERROR_IO_PENDING or 997
        */
        // Overlapped operations will complete later.
        const int WSA_IO_PENDING         = (997);

        /*
        * Error return codes from gethostbyname() and gethostbyaddr()
        *              = (when using the resolver). Note that these errors are
        * retrieved via WSAGetLastError() and must therefore follow
        * the rules for avoiding clashes with error numbers from
        * specific implementations or language run-time systems.
        * For this reason the codes are based at WSABASEERR+1001.
        * Note also that [WSA]NO_ADDRESS is defined only for
        * compatibility purposes.
        */

        // Authoritative Answer: Host not found
        const int WSAHOST_NOT_FOUND      = (WSABASEERR+1001);
        // Non-Authoritative: Host not found; or SERVERFAIL */
        const int WSATRY_AGAIN           = (WSABASEERR+1002);
        /* Non recoverable errors; FORMERR, REFUSED, NOTIMP */
        // This is a nonrecoverable error.
        const int WSANO_RECOVERY         = (WSABASEERR+1003);
        // Valid name, no data record of requested type.
        const int WSANO_DATA             = (WSABASEERR+1004);
        // Host not found.
        const int HOST_NOT_FOUND         = WSAHOST_NOT_FOUND;
        // Nonauthoritative host not found.
        const int TRY_AGAIN              = WSATRY_AGAIN;
        // This is a nonrecoverable error.
        const int NO_RECOVERY            = WSANO_RECOVERY;
        // Valid name, no data record of requested type.
        const int NO_DATA                = WSANO_DATA;
        /* no address; look for MX record */
        const int WSANO_ADDRESS          = WSANO_DATA;
        const int NO_ADDRESS             = WSANO_ADDRESS;


        public const int Success                                = 0;

        //The socket is invalid.</para>
        public const int InvalidSocket                          = (~0);
    
        // The socket has an error.
        public const int SocketError                            = (-1);

        public const int HostNotFound                           = HOST_NOT_FOUND;
        public const int NoAddress                              = NO_ADDRESS;
        public const int NoData                                 = NO_DATA;
        public const int NoRecovery                             = NO_RECOVERY;
        public const int TryAgain                               = TRY_AGAIN;
        public const int WsaIOPending                           = WSA_IO_PENDING;
        public const int WsaBaseError                           = WSABASEERR;
        public const int WsaErrorAccess                         = WSAEACCES;
        public const int WsaErrorAddressInUse                   = WSAEADDRINUSE;
        public const int WsaErrorAddressNotAvailable            = WSAEADDRNOTAVAIL;
        public const int WsaErrorAddressFamilyNoSupported       = WSAEAFNOSUPPORT;
        public const int WsaErrorAlready                        = WSAEALREADY;
        public const int WsaErrorBadFileHandle                  = WSAEBADF;
        public const int WsaErrorConnectionAborted              = WSAECONNABORTED;
        public const int WsaErrorConnectionRefused              = WSAECONNREFUSED;
        public const int WsaErrorConnectionReset                = WSAECONNRESET;
        public const int WsaErrorDestinationAddressRequested    = WSAEDESTADDRREQ;
        public const int WsaErrorDisconnected                   = WSAEDISCON;
        public const int WsaErrorOutOfDiskQuota                 = WSAEDQUOT;
        public const int WsaErrorFault                          = WSAEFAULT;
        public const int WsaErrorHostDown                       = WSAEHOSTDOWN;
        public const int WsaErrorHostUnreachable                = WSAEHOSTUNREACH;
        public const int WsaErrorInProgress                     = WSAEINPROGRESS;
        public const int WsaErrorInterrupted                    = WSAEINTR;
        public const int WsaErrorInvalid                        = WSAEINVAL;
        public const int WsaErrorIsConnection                   = WSAEISCONN;
        public const int WsaErrorLoop                           = WSAELOOP;
        public const int WsaErrorTooManyOpenSockets             = WSAEMFILE;
        public const int WsaErrorMessageSize                    = WSAEMSGSIZE;
        public const int WsaErrorNameTooLong                    = WSAENAMETOOLONG;
        public const int WsaErrorNetDown                        = WSAENETDOWN;
        public const int WsaErrorNetReset                       = WSAENETRESET;
        public const int WsaErrorNetUnreachable                 = WSAENETUNREACH;
        public const int WsaErrorNoBuffers                      = WSAENOBUFS;
        public const int WsaErrorNoProtocolOption               = WSAENOPROTOOPT;
        public const int WsaErrorNotConnected                   = WSAENOTCONN;
        public const int WsaErrorNotEmpty                       = WSAENOTEMPTY;
        public const int WsaErrorNotSocket                      = WSAENOTSOCK;
        public const int WsaErrorOperationNotSupported          = WSAEOPNOTSUPP;
        public const int WsaErrorProtocolFamilyNoSupported      = WSAEPFNOSUPPORT;
        public const int WsaErrorProcessLimit                   = WSAEPROCLIM;
        public const int WsaErrorPotocolNotSupported            = WSAEPROTONOSUPPORT;
        public const int WsaErrorProtocolType                   = WSAEPROTOTYPE;
        public const int WsaErrorRemote                         = WSAEREMOTE;
        public const int WsaErrorShutdown                       = WSAESHUTDOWN;
        public const int WsaErrorSocketNotSupported             = WSAESOCKTNOSUPPORT;
        public const int WsaErrorStale                          = WSAESTALE;
        public const int WsaErrorTimedOut                       = WSAETIMEDOUT;
        public const int WsaErrorTooManyRefuses                 = WSAETOOMANYREFS;
        public const int WsaErrorUsers                          = WSAEUSERS;
        public const int WsaErrorWouldBlock                     = WSAEWOULDBLOCK;
        public const int WsaHostNotFound                        = WSAHOST_NOT_FOUND;
        public const int WsaNoAddress                           = WSANO_ADDRESS;
        public const int WsaNoData                              = WSANO_DATA;
        public const int WsaNoRecovery                          = WSANO_RECOVERY;
        public const int WsaNotInitialized                      = WSANOTINITIALISED;
        public const int WsaSystemNotReady                      = WSASYSNOTREADY;
        public const int WsaTryAgain                            = WSATRY_AGAIN;
        public const int WsaVersionNotSupported                 = WSAVERNOTSUPPORTED;
    }
}