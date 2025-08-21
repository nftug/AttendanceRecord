using System.Reflection;
using System.Runtime.Versioning;
using AttendanceRecord.Presentation.Utils;
using BrowserBridge.Photino;
using H.NotifyIcon.Core;

namespace AttendanceRecord.Presentation.Services;

public interface ITrayIconService : IDisposable
{
    void CreateTrayIcon();
    void SetShowWindow(bool isVisible);
}

[SupportedOSPlatform("windows5.1.2600")]
public sealed class WindowsTrayIconService(PhotinoWindowInstance windowInstance) : ITrayIconService
{
    private TrayIconWithContextMenu? _trayIcon;

    public void CreateTrayIcon()
    {
        if (!OperatingSystem.IsWindowsVersionAtLeast(5, 1, 2600)) return;
        if (windowInstance.Value is not { } window) return;

        _trayIcon = new()
        {
            Icon = Win32WindowHelper.LoadDefaultIcon(),
            ToolTip = Assembly.GetEntryAssembly()?.GetName().Name ?? "Application",
            ContextMenu = new()
            {
                Items =
                {
                    new PopupMenuItem("メインウィンドウの表示", (_, _) => SetShowWindow(true)),
                    new PopupMenuItem("終了", (_, _) => window.Close())
                }
            }
        };

        _trayIcon.MessageWindow.MouseEventReceived += (_, args) =>
        {
            if (args.MouseEvent == MouseEvent.IconLeftMouseUp)
                SetShowWindow(true);
        };

        _trayIcon.Create();

        windowInstance.OnWindowClosing += () => _trayIcon?.Dispose();
    }

    public void SetShowWindow(bool isVisible)
    {
        if (windowInstance.Value is not { } window) return;

        if (isVisible) Win32WindowHelper.Restore(window.WindowHandle);
        else Win32WindowHelper.Hide(window.WindowHandle);
    }

    public void Dispose() => _trayIcon?.Dispose();
}

public class NoopTrayIconService : ITrayIconService
{
    public void CreateTrayIcon()
    {
    }

    public void SetShowWindow(bool isVisible)
    {
    }

    public void Dispose()
    {
    }
}
