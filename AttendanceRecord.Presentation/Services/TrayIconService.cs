using System.Reflection;
using System.Runtime.Versioning;
using AttendanceRecord.Presentation.Utils;
using BrowserBridge.Photino;
using H.NotifyIcon.Core;

namespace AttendanceRecord.Presentation.Services;

public interface ITrayIconService : IDisposable
{
    void CreateNotifyIcon();
    void ToggleShowWindow();
}

[SupportedOSPlatform("windows5.1.2600")]
public sealed class WindowsTrayIconService(PhotinoWindowInstance windowInstance) : ITrayIconService
{
    private TrayIconWithContextMenu? _trayIcon;
    private bool _isVisible = true;

    public void CreateNotifyIcon()
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
                    new PopupMenuItem("Toggle show window", (_, _) => ToggleShowWindow()),
                    new PopupMenuItem("Exit", (_, _) => window.Close())
                }
            }
        };

        _trayIcon.Create();

        windowInstance.OnWindowClosing += () => _trayIcon?.Dispose();
    }

    public void ToggleShowWindow()
    {
        if (windowInstance.Value is not { } window) return;

        if (_isVisible) Win32WindowHelper.Hide(window.WindowHandle);
        else Win32WindowHelper.Restore(window.WindowHandle);
        _isVisible = !_isVisible;
    }

    public void Dispose() => _trayIcon?.Dispose();
}

public class NoopTrayIconService : ITrayIconService
{
    public void CreateNotifyIcon()
    {
    }

    public void ToggleShowWindow()
    {
    }

    public void Dispose()
    {
    }
}
