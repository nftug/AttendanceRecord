using Microsoft.Extensions.Logging;

namespace BrowserBridge;

public interface IErrorHandler
{
    void HandleError(Exception exception);
}

public class ErrorHandler(
    IDialogService dialogService, IEventDispatcher eventDispatcher, ILogger<ErrorHandler> logger)
    : IErrorHandler
{
    public void HandleError(Exception exception)
    {
        logger.LogError(exception, "Error!: {Message}", exception.Message);

        if (exception is ViewModelException vmException)
        {
            var errorEvent = new ViewModelErrorEvent(new(exception.Message, exception.ToString()))
            {
                ViewId = vmException.ViewId
            };
            eventDispatcher.Dispatch(errorEvent, BridgeJsonContext.Default.EventMessageViewModelError);
        }

        dialogService.ShowMessageBox(exception.Message, EnvironmentConstants.AppName, icon: IconType.Error);
    }
}
