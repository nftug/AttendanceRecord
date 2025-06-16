using Photino.NET;

namespace BrowserBridge.Photino;

public abstract class PhotinoAppServiceBase(
    IContainerInstance appContainerInstance,
    PhotinoWindowInstance windowInstance,
    CommandDispatcher dispatcher,
    IErrorHandler errorHandler
)
{
    protected bool? IsClosing { get; private set; }

    protected PhotinoWindow Window { get; } = new();

    protected virtual PhotinoWindow SetupWindow(PhotinoWindow window) => window;

    protected abstract string LocalDebugUrl { get; }

    public void Run(object container)
    {
        appContainerInstance.Inject(container);

        string appUrl =
            EnvironmentConstants.IsDebugMode ? LocalDebugUrl
            : OperatingSystem.IsWindows() ? "http://localhost" : "app://localhost/";

        Window
            .SetTitle(EnvironmentConstants.AppName)
            .LoadRawString($"""<meta http-equiv="refresh" content="0; URL='{appUrl}'" />""")
            .RegisterWebMessageReceivedHandler(HandleWebMessageReceived)
            .RegisterWindowCreatedHandler(HandleWindowCreated)
            .RegisterWindowClosingHandler(HandleWindowClosing);

        SetupWindow(Window);

        if (!EnvironmentConstants.IsDebugMode)
        {
            Window.RegisterCustomSchemeHandler(new Uri(appUrl).Scheme, PhotinoCustomSchemeHandler.HandleEmbeddedFile);
        }

        Window.WaitForClose();
    }

    protected virtual async void HandleWebMessageReceived(object? sender, string messageJson)
    {
        await dispatcher.DispatchAsync(messageJson);
    }

    protected virtual void HandleWindowCreated(object? sender, EventArgs e)
    {
        windowInstance.Inject(Window);

        AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
        {
            errorHandler.HandleError((e.ExceptionObject as Exception)!);
            Environment.Exit(1);
        };
        TaskScheduler.UnobservedTaskException += (sender, e) =>
        {
            errorHandler.HandleError(e.Exception);
            e.SetObserved();
        };
    }

    protected virtual bool HandleWindowClosing(object? sender, EventArgs e)
    {
        if (IsClosing is { } isClosing)
        {
            IsClosing = null;
            return !isClosing;
        }

        Task.Run(async () =>
        {
            IsClosing = await ShouldCloseAsync();
            Window.Close();
        });

        return true;
    }

    protected virtual ValueTask<bool> ShouldCloseAsync() => new(true);
}
