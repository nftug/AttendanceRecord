using AttendanceRecord.Domain.Interfaces;
using AttendanceRecord.Presentation.Services;
using AttendanceRecord.Presentation.Utils;
using BrowserBridge;
using BrowserBridge.Photino;
using Photino.NET;

namespace AttendanceRecord.Presentation;

public sealed class AppService(
    IContainerInstance containerInstance,
    PhotinoWindowInstance windowInstance,
    CommandDispatcher dispatcher,
    IErrorHandler errorHandler,
    ISingleInstanceGuard singleInstanceGuard,
    ITrayIconService trayIconService
) : PhotinoAppServiceBase(containerInstance, windowInstance, dispatcher, errorHandler)
{
    protected override string LocalDebugUrl => "http://localhost:5173";

    protected override PhotinoWindow SetupWindow(PhotinoWindow window)
    {
        return window
                .SetUseOsDefaultSize(false)
                .SetSize(new(1200, 820))
                .Center()
                .SetContextMenuEnabled(EnvironmentConstants.IsDebugMode)
                .SetDevToolsEnabled(EnvironmentConstants.IsDebugMode);
    }

    protected override void HandleWindowCreated(object? sender, EventArgs e)
    {
        base.HandleWindowCreated(sender, e);

        trayIconService.CreateTrayIcon();
        Window.WindowMinimized += (_, _) => trayIconService.SetShowWindow(true);

        Task.Run(async () =>
        {
            if (!singleInstanceGuard.TryAcquireLock())
            {
                // 別のインスタンスが起動している場合、既に起動しているアプリを表示して終了
                await NamedPipeClient.SendMessageAsync("ShowWindow");
                Environment.Exit(0);
            }

            await NamedPipeServer.ReceiveMessageAsync(message =>
            {
                if (message?.Content == "ShowWindow") trayIconService.SetShowWindow(true);
            });
        });
    }

    protected override ValueTask<bool> ShouldCloseAsync()
    {
        if (trayIconService.IsTrayIconAvailable)
        {
            trayIconService.SetShowWindow(false);
            return ValueTask.FromResult(false);
        }
        else
        {
            var result = Window.ShowMessage(
                "確認", "アプリケーションを終了しますか？", PhotinoDialogButtons.YesNo, PhotinoDialogIcon.Question);
            return ValueTask.FromResult(result == PhotinoDialogResult.Yes);
        }
    }
}
