using System.Runtime.InteropServices;
using System.Runtime.Versioning;

namespace AttendanceRecord.Presentation.Utils;

[SupportedOSPlatform("windows")]
public static class Win32WindowHelper
{
    private const int SW_HIDE = 0;
    private const int SW_RESTORE = 9;

    [DllImport("user32.dll", SetLastError = true)]
    private static extern bool ShowWindow(nint hWnd, int nCmdShow);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern bool SetForegroundWindow(nint hWnd);

    [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    private static extern IntPtr LoadIcon(IntPtr hInstance, IntPtr lpIconName);

    public static void Hide(nint hWnd)
    {
        ShowWindow(hWnd, SW_HIDE);
    }

    public static void Restore(nint hWnd, bool activate = true)
    {
        if (hWnd == 0) return;
        ShowWindow(hWnd, SW_RESTORE);
        if (activate) SetForegroundWindow(hWnd);
    }

    public static IntPtr LoadDefaultIcon()
    {
        // 32512 is the ID for the default application icon
        return LoadIcon(IntPtr.Zero, new IntPtr(32512));
    }
}
