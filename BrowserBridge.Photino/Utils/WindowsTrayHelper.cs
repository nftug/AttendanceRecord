using System.Runtime.InteropServices;

namespace BrowserBridge.Photino.Utils;

public static class WindowsTrayHelper
{
    private static readonly string _appName = EnvironmentConstants.AppName;

    private static IntPtr _oldWndProc = IntPtr.Zero;
    private static WndProcDelegate? _wndProcDelegate;
    private static bool _trayAdded;

    private const uint WM_USER = 0x0400;
    private static readonly uint _trayMessage = WM_USER + 1;
    private const int GWL_WNDPROC = -4;
    private const uint NIF_MESSAGE = 0x00000001;
    private const uint NIF_ICON = 0x00000002;
    private const uint NIF_TIP = 0x00000004;
    private const uint NIM_ADD = 0x00000000;
    private const uint NIM_DELETE = 0x00000002;
    private const int WM_LBUTTONUP = 0x0202;
    private const int SW_HIDE = 0;
    private const int SW_RESTORE = 9;

    private delegate IntPtr WndProcDelegate(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

    public static void Initialize(IntPtr hwnd)
    {
        if (hwnd == IntPtr.Zero || _wndProcDelegate != null) return;
        _wndProcDelegate = new WndProcDelegate(NativeWndProc);
        var procPtr = Marshal.GetFunctionPointerForDelegate(_wndProcDelegate);
        _oldWndProc = SetWindowLongPtr(hwnd, GWL_WNDPROC, procPtr);
        AppDomain.CurrentDomain.ProcessExit += (_, _) => Cleanup(hwnd);
    }

    public static void HideToTray(IntPtr hwnd)
    {
        if (hwnd == IntPtr.Zero) return;
        if (!_trayAdded) TryAddTrayIcon(hwnd);
        ShowWindow(hwnd, SW_HIDE);
    }

    private static void Cleanup(IntPtr hwnd)
    {
        TryRemoveTrayIcon(hwnd);
        if (_oldWndProc == IntPtr.Zero) return;

        SetWindowLongPtr(hwnd, GWL_WNDPROC, _oldWndProc);
        _oldWndProc = IntPtr.Zero;
    }

    private static IntPtr NativeWndProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam)
    {
        if (msg == _trayMessage)
        {
            int action = lParam.ToInt32();
            if (action == WM_LBUTTONUP) RestoreWindowFromTray(hWnd);
            return IntPtr.Zero;
        }
        return CallWindowProc(_oldWndProc, hWnd, msg, wParam, lParam);
    }

    private static void RestoreWindowFromTray(IntPtr hWnd)
    {
        TryRemoveTrayIcon(hWnd);
        ShowWindow(hWnd, SW_RESTORE);
        SetForegroundWindow(hWnd);
    }

    private static void TryAddTrayIcon(IntPtr hWnd)
    {
        var nid = new NOTIFYICONDATA
        {
            cbSize = (uint)Marshal.SizeOf<NOTIFYICONDATA>(),
            hWnd = hWnd,
            uID = 1,
            uFlags = NIF_MESSAGE | NIF_ICON | NIF_TIP,
            uCallbackMessage = _trayMessage,
            hIcon = LoadIcon(IntPtr.Zero, new IntPtr(32512)),
            szTip = _appName
        };
        Shell_NotifyIcon(NIM_ADD, ref nid);
        _trayAdded = true;
    }

    private static void TryRemoveTrayIcon(IntPtr hWnd)
    {
        if (!_trayAdded) return;
        var nid = new NOTIFYICONDATA
        {
            cbSize = (uint)Marshal.SizeOf<NOTIFYICONDATA>(),
            hWnd = hWnd,
            uID = 1
        };
        Shell_NotifyIcon(NIM_DELETE, ref nid);
        _trayAdded = false;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    private struct NOTIFYICONDATA
    {
        public uint cbSize;
        public IntPtr hWnd;
        public uint uID;
        public uint uFlags;
        public uint uCallbackMessage;
        public IntPtr hIcon;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string szTip;
    }

    [DllImport("user32.dll", EntryPoint = "SetWindowLongPtrW", SetLastError = true)]
    private static extern IntPtr SetWindowLongPtr64(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

    [DllImport("user32.dll", EntryPoint = "SetWindowLongW", SetLastError = true)]
    private static extern IntPtr SetWindowLong32(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

    private static IntPtr SetWindowLongPtr(IntPtr hWnd, int nIndex, IntPtr newProc)
    {
        return IntPtr.Size == 8 ? SetWindowLongPtr64(hWnd, nIndex, newProc) : SetWindowLong32(hWnd, nIndex, newProc);
    }

    [DllImport("user32.dll")]
    private static extern IntPtr CallWindowProc(IntPtr lpPrevWndFunc, IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

    [DllImport("user32.dll")]
    private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

    [DllImport("user32.dll")]
    private static extern bool SetForegroundWindow(IntPtr hWnd);

    [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    private static extern IntPtr LoadIcon(IntPtr hInstance, IntPtr lpIconName);

    [DllImport("shell32.dll", CharSet = CharSet.Unicode)]
    private static extern bool Shell_NotifyIcon(uint dwMessage, ref NOTIFYICONDATA lpdata);
}
