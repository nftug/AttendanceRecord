using Microsoft.Extensions.Logging;

namespace BrowserBridge;

public interface IErrorHandler
{
    void HandleError(Exception exception);
}

public class ErrorHandler(
    IWindowService dialogService, IEventDispatcher eventDispatcher, ILogger<ErrorHandler> logger)
    : IErrorHandler
{
    public void HandleError(Exception exception)
    {
        logger.LogError(exception, "Error!: {Message}", exception.Message);

        if (exception is ViewModelException vmException)
        {
            var errorContent = new ViewModelError(vmException.CommandId, exception.Message, exception.ToString());
            var errorEvent = new ViewModelErrorEvent(errorContent)
            {
                ViewId = vmException.ViewId
            };
            eventDispatcher.Dispatch(errorEvent, BridgeJsonContext.Default.ViewModelErrorEvent);
        }

        dialogService.ShowMessageBox(exception.Message, EnvironmentConstants.AppName, icon: IconType.Error);
    }
}
