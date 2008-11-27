//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                               
//------------------------------------------------------------------------------

using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace Terrarium.Client.SplashScreen
{
    /// <summary>
    ///  Represents a native window that can be used as a splash
    ///  screen.  This enables display of UI before the Windows.Forms
    ///  libraries have been *warmed* up.
    /// </summary>
    /// <remarks>
    ///  WARNING! Even though ReSharper and other tools will show values here
    ///  as not being used or referenced, DO NOT REMOVE or otherwise refactor
    ///  them away. We're in Win32 COM land people and this stuff breaks easily.
    /// </remarks>
    internal class SplashWindow : MarshalByRefObject
    {
        private const int COLOR_WINDOW = 5;
        private const int CS_DROPSHADOW = 0x00020000;
        private const int CW_USEDEFAULT = (unchecked((int) 0x80000000));
        private const int LWA_ALPHA = 0x00000002;
        private const int LWA_COLORKEY = 0x00000001;
        private const int PM_REMOVE = 0x0001;
        private const int SW_SHOWNORMAL = 1;
        private const string ThreadName = "SplashThread";
        private const string WindowClassName = "SplashWindow";
        private const int WM_CLOSE = 0x0010;
        private const int WM_CREATE = 0x0001;
        private const int WM_DESTROY = 0x0002;
        private const int WM_ERASEBKGND = 0x0014;
        private const int WM_PAINT = 0x000F;
        private const int WM_TIMER = 0x0113;
        private const int WS_EX_LAYERED = 0x00080000;
        private const int WS_EX_TOOLWINDOW = 0x00000080;
        private const int WS_EX_TOPMOST = 0x00000008;

        private const int WS_OVERLAPPEDWINDOW =
            (0x00000000 | 0x00C00000 | 0x00080000 | 0x00040000 | 0x00020000 | 0x00010000);

        private const int WS_POPUP = (unchecked((int) 0x80000000));
        private const int WS_VISIBLE = 0x10000000;

        private static SplashWindow current;
        private static WNDPROC WindowProcedure;
        private SplashScreenCustomizerEventHandler _customizer;
        private Form _formToActivate;
        private int _height;
        private IntPtr _hwnd;
        private Image _image;
        private int _minimumDuration;
        private bool _minimumDurationComplete;
        private bool _showShadow;
        private int _timer;
        private Color _transparencyKey;
        private bool _waitingForTimer;
        private int _width;

        /// <summary>
        ///  Can't create a window this way.  Use the Current property.
        /// </summary>
        private SplashWindow()
        {
        }

        /// <summary>
        ///  Returns the current splash window.  If one doesn't exist
        ///  then one is created on the fly.
        /// </summary>
        internal static SplashWindow Current
        {
            get
            {
                if (current == null)
                {
                    current = new SplashWindow();
                }
                return current;
            }
        }

        /// <summary>
        ///  Set the image used as the background of the splash window.
        /// </summary>
        internal Image Image
        {
            get { return _image; }

            set
            {
                _image = value;
                _width = _image.Width;
                _height = _image.Height;
            }
        }

        /// <summary>
        ///  This is the minimum amount of time the window will display for.
        /// </summary>
        internal int MinimumDuration
        {
            get { return _minimumDuration; }

            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException();
                }

                if (_hwnd != IntPtr.Zero)
                {
                    throw new InvalidOperationException();
                }
                _minimumDuration = value;
            }
        }

        /// <summary>
        ///  Should a drop shadow be displayed?
        /// </summary>
        internal bool ShowShadow
        {
            get { return _showShadow; }

            set
            {
                if (_hwnd != IntPtr.Zero)
                {
                    throw new InvalidOperationException();
                }
                _showShadow = value;
            }
        }

        /// <summary>
        ///  Enabled a transparency key color for the image.  This will
        ///  generate a *cool* splash screen with transparent regions.
        /// </summary>
        internal Color TransparencyKey
        {
            get { return _transparencyKey; }

            set
            {
                if (_hwnd != IntPtr.Zero)
                {
                    throw new InvalidOperationException();
                }
                _transparencyKey = value;
            }
        }

        /// <summary>
        ///  Creates a native Windows OS, Window to represent the
        ///  splash screen.
        /// </summary>
        /// <returns>True if the windows was created.</returns>
        private bool CreateNativeWindow()
        {
            var result = false;

            var style = WS_VISIBLE | WS_POPUP;
            var exStyle = WS_EX_TOOLWINDOW;

            if ((_transparencyKey.IsEmpty == false) && IsLayeringSupported())
            {
                exStyle |= WS_EX_LAYERED;
            }

            var desktop = Screen.FromPoint(Control.MousePosition);
            var screenRect = desktop.WorkingArea;

            int left = Math.Max(screenRect.X, screenRect.X + (screenRect.Width - _width)/2);
            int top = Math.Max(screenRect.Y, screenRect.Y + (screenRect.Height - _height)/2);

            _hwnd = CreateWindowEx(exStyle, WindowClassName, "", style, left, top, _width, _height, IntPtr.Zero,
                                   IntPtr.Zero, GetModuleHandle(null), null);
            if (_hwnd != IntPtr.Zero)
            {
                ShowWindow(_hwnd, SW_SHOWNORMAL);
                UpdateWindow(_hwnd);
                result = true;
            }

            return result;
        }

        /// <summary>
        ///  Hides the window and activates the given form after the
        ///  splash screen is hidden.  This listens for the minimum
        ///  display duration to make sure it doesn't get hidden too
        ///  soon.
        /// </summary>
        /// <param name="formToActivate">The Windows Forms Form to activate when the splash screen is hidden.</param>
        internal void Hide(Form formToActivate)
        {
            _formToActivate = formToActivate;
            if (_minimumDuration > 0)
            {
                _waitingForTimer = true;
                if (_minimumDurationComplete == false)
                {
                    return;
                }
            }
            if (_hwnd != IntPtr.Zero)
            {
                PostMessage(_hwnd, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
            }
        }

        /// <summary>
        ///  Registers the native window class associated with the window
        ///  generated to display the splash window.
        /// </summary>
        /// <returns>True if the registration succeeded, false otherwise.</returns>
        private bool RegisterWindowClass()
        {
            var result = false;

            var wc = new WNDCLASS();
            wc.style = 0;
            wc.lpfnWndProc = WindowProcedure = WndProc;
            wc.hInstance = GetModuleHandle(null);
            wc.hbrBackground = (IntPtr) (COLOR_WINDOW + 1);
            wc.lpszClassName = WindowClassName;
            wc.cbClsExtra = 0;
            wc.cbWndExtra = 0;
            wc.hIcon = IntPtr.Zero;
            wc.hCursor = IntPtr.Zero;
            wc.lpszMenuName = null;

            if (_showShadow && IsDropShadowSupported())
            {
                wc.style |= CS_DROPSHADOW;
            }

            if (RegisterClass(wc) != IntPtr.Zero)
            {
                result = true;
            }

            return result;
        }

        /// <summary>
        ///  Enables the use of a special customization callback handler.  Examine
        ///  the SplashScreenCustomizerEventHandler.cs file for more information.
        /// </summary>
        /// <param name="customizer">Set the customizer callback for this splash screen.</param>
        internal void SetCustomizer(SplashScreenCustomizerEventHandler customizer)
        {
            _customizer = customizer;
        }

        /// <summary>
        ///  Start a new thread and launch the splash screen window.
        /// </summary>
        internal void Show()
        {
            if (_hwnd != IntPtr.Zero) return;
            var thread = new Thread(ThreadFunction) {Name = ThreadName};
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.IsBackground = true;
        }

        /// <summary>
        ///  This method is called to run the splash screen thread.  This
        ///  is responsible for handling the windows message pump and allowing
        ///  the splash screen to run while the application continues to load
        ///  by itself.
        /// </summary>
        private static void ThreadFunction()
        {
            var result = current.RegisterWindowClass();
            if (result)
            {
                result = current.CreateNativeWindow();
            }

            if (!result) return;

            var msg = new MSG();
            while (GetMessage(ref msg, IntPtr.Zero, 0, 0))
            {
                TranslateMessage(ref msg);
                DispatchMessage(ref msg);
            }

            current._hwnd = IntPtr.Zero;
            if (current._formToActivate == null) return;

            current._formToActivate.Invoke(new MethodInvoker(current._formToActivate.Activate));
            current._formToActivate = null;
        }

        /// <summary>
        ///  A custom windows procedure that processes events for the splash screen
        ///  window.  the WM_CREATE, WM_DESTROY, WM_TIMER, and WM_PAINT messages are handled
        ///  specifically.
        /// </summary>
        /// <param name="hwnd">The handle of the window.</param>
        /// <param name="msg">The windows message identifier.</param>
        /// <param name="wParam">The *Word* Message parameters.</param>
        /// <param name="lParam">The *Long* Message parameters.</param>
        /// <returns>A return value adequate to the message sent.</returns>
        protected virtual int WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam)
        {
            switch (msg)
            {
                case WM_CREATE:
                    if ((_transparencyKey.IsEmpty == false) && IsLayeringSupported())
                    {
                        SetLayeredWindowAttributes(hwnd, ColorTranslator.ToWin32(_transparencyKey), 0, LWA_COLORKEY);
                    }

                    if (_minimumDuration > 0)
                    {
                        _timer = SetTimer(hwnd, 1, _minimumDuration, IntPtr.Zero);
                    }
                    break;
                case WM_DESTROY:
                    PostQuitMessage(0);
                    break;
                case WM_ERASEBKGND:
                    return 1;
                case WM_PAINT:
                    {
                        var ps = new PAINTSTRUCT();
                        var hdc = BeginPaint(hwnd, ref ps);

                        if (hdc != IntPtr.Zero)
                        {
                            var g = Graphics.FromHdcInternal(hdc);
                            g.DrawImage(_image, 0, 0, _width, _height);
                            if (_customizer != null)
                            {
                                _customizer(new SplashScreenSurface(g, new Rectangle(0, 0, _width - 1, _height - 1)));
                            }
                            g.Dispose();
                        }

                        EndPaint(hwnd, ref ps);
                    }
                    return 0;
                case WM_TIMER:
                    KillTimer(hwnd, _timer);
                    _timer = 0;
                    _minimumDurationComplete = true;

                    if (_waitingForTimer)
                    {
                        PostMessage(hwnd, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
                    }
                    return 0;
            }
            return DefWindowProc(hwnd, msg, wParam, lParam);
        }

        /// <summary>
        ///  Examines OS support for the drop shadow feature.
        /// </summary>
        /// <returns>True if the OS supports drop shadows, false otherwise.</returns>
        private static bool IsDropShadowSupported()
        {
            return (Environment.OSVersion.Version.CompareTo(new Version(5, 1, 0, 0)) >= 0);
        }

        /// <summary>
        ///  Examines OS support for the transparency color key through
        ///  layering support.
        /// </summary>
        /// <returns>True if the OS supports transparency and layering, false otherwise.</returns>
        private static bool IsLayeringSupported()
        {
            return (Environment.OSVersion.Version.CompareTo(new Version(5, 0, 0, 0)) >= 0);
        }

        [DllImport("user32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
        private static extern IntPtr BeginPaint(IntPtr hWnd, [In, Out] ref PAINTSTRUCT lpPaint);

        [DllImport("user32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
        private static extern bool EndPaint(IntPtr hWnd, ref PAINTSTRUCT lpPaint);

        [DllImport("user32.dll", EntryPoint = "CreateWindowEx", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CreateWindowEx(int dwExStyle, string lpszClassName, string lpszWindowName,
                                                    int style, int x, int y, int width, int height, IntPtr hWndParent,
                                                    IntPtr hMenu, IntPtr hInst,
                                                    [MarshalAs(UnmanagedType.AsAny)] object pvParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int DefWindowProc(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
        private static extern bool DestroyWindow(IntPtr hWnd);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int DispatchMessage(ref MSG msg);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool GetMessage(ref MSG msg, IntPtr hwnd, int minFilter, int maxFilter);

        [DllImport("kernel32", CharSet = CharSet.Auto)]
        private static extern IntPtr GetModuleHandle(string modName);

        [DllImport("user32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
        private static extern bool KillTimer(IntPtr hwnd, int idEvent);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern bool PeekMessage([In, Out] ref MSG msg, IntPtr hwnd, int msgMin, int msgMax, int remove);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int PostMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
        private static extern void PostQuitMessage(int nExitCode);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr RegisterClass(WNDCLASS wc);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int SetLayeredWindowAttributes(IntPtr hwnd, int color, byte alpha, int flags);

        [DllImport("user32.dll")]
        private static extern int SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        [DllImport("user32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
        private static extern int SetTimer(IntPtr hWnd, int nIDEvent, int uElapse, IntPtr lpTimerFunc);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
        private static extern bool TranslateMessage(ref MSG msg);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern bool UpdateWindow(IntPtr hWnd);

        #region Nested type: MSG

        [ComVisible(true), StructLayout(LayoutKind.Sequential)]
        private struct MSG
        {
            public IntPtr hwnd;
            public int message;
            public IntPtr wParam;
            public IntPtr lParam;
            public int time;
            // pt was a by-value POINT structure
            public int pt_x;
            public int pt_y;
        }

        #endregion

        #region Nested type: PAINTSTRUCT

        [StructLayout(LayoutKind.Sequential)]
        private struct PAINTSTRUCT
        {
            public IntPtr hdc;
            public bool fErase;
            // rcPaint was a by-value RECT structure
            public int rcPaint_left;
            public int rcPaint_top;
            public int rcPaint_right;
            public int rcPaint_bottom;
            public bool fRestore;
            public bool fIncUpdate;
            public int reserved1;
            public int reserved2;
            public int reserved3;
            public int reserved4;
            public int reserved5;
            public int reserved6;
            public int reserved7;
            public int reserved8;
        }

        #endregion

        #region Nested type: RECT

        [StructLayout(LayoutKind.Sequential)]
        private struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;

            internal RECT(int left, int top, int right, int bottom)
            {
                this.left = left;
                this.top = top;
                this.right = right;
                this.bottom = bottom;
            }

            internal static RECT FromXYWH(int x, int y, int width, int height)
            {
                return new RECT(x, y, x + width, y + height);
            }
        }

        #endregion

        #region Nested type: WNDCLASS

        [ComVisible(false), StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        private class WNDCLASS
        {
            public int style;
            public WNDPROC lpfnWndProc;
            public int cbClsExtra;
            public int cbWndExtra;
            public IntPtr hInstance;
            public IntPtr hIcon;
            public IntPtr hCursor;
            public IntPtr hbrBackground;
            public string lpszMenuName;
            public string lpszClassName;
        }

        #endregion

        #region Nested type: WNDPROC

        private delegate int WNDPROC(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        #endregion
    }
}