using System.Reflection;
using System.Runtime.Versioning;
using AttendanceRecord.Presentation.Utils;
using BrowserBridge.Photino;
using H.NotifyIcon.Core;
using Photino.NET;

namespace AttendanceRecord.Presentation.Services;

public interface ITrayIconService : IDisposable
{
    bool IsTrayIconAvailable { get; }
    void CreateTrayIcon();
    void SetShowWindow(bool isVisible);
}

[SupportedOSPlatform("windows5.1.2600")]
public sealed class WindowsTrayIconService(PhotinoWindowInstance windowInstance) : ITrayIconService
{
    private TrayIconWithContextMenu? _trayIcon;

    public bool IsTrayIconAvailable => _trayIcon is not null;

    public void CreateTrayIcon()
    {
        if (!OperatingSystem.IsWindowsVersionAtLeast(5, 1, 2600)) return;

        _trayIcon = new()
        {
            Icon = Win32WindowHelper.LoadDefaultIcon(),
            ToolTip = Assembly.GetEntryAssembly()?.GetName().Name ?? "Application",
            ContextMenu = new()
            {
                Items =
                {
                    new PopupMenuItem("表示", (_, _) => SetShowWindow(true)),
                    new PopupMenuItem("終了", (_, _) => HandleClickExit())
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

    private void HandleClickExit()
    {
        if (windowInstance.Value is not { } window) return;
        var result = window.ShowMessage(
            "確認", "アプリケーションを終了しますか？", PhotinoDialogButtons.YesNo, PhotinoDialogIcon.Question);
        if (result == PhotinoDialogResult.Yes) Environment.Exit(0);
    }

    public void Dispose() => _trayIcon?.Dispose();
}

public sealed class NoopTrayIconService(PhotinoWindowInstance windowInstance) : ITrayIconService
{
    public bool IsTrayIconAvailable => false;

    public void CreateTrayIcon()
    {
    }

    public void SetShowWindow(bool isVisible)
    {
        windowInstance.Value?.SetMinimized(!isVisible);
    }

    public void Dispose()
    {
    }
}
