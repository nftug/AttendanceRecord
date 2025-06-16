using System.Text.Json;

namespace BrowserBridge;

public class WindowViewModel(IEventDispatcher eventDispatcher, IDialogService dialogService)
    : ViewModelBase<WindowCommandType>(eventDispatcher)
{
    protected override void OnFirstRender() { }

    protected override ValueTask HandleActionAsync(WindowCommandType action, JsonElement? payload, Guid? commandId)
        => (action, payload) switch
        {
            (WindowCommandType.MessageBox, { } p) =>
                p.HandlePayloadSync(BridgeJsonContext.Default.MessageBoxCommandPayload, p => ShowMessageBox(p, commandId)),
            _ => ValueTask.CompletedTask
        };

    private void ShowMessageBox(MessageBoxCommandPayload command, Guid? commandId)
    {
        string title = command.Title ?? EnvironmentConstants.AppName;
        var dialogResult = dialogService.ShowMessageBox(command.Message, title, command.Buttons, command.Icon);

        if (commandId != null)
        {
            var eventPayload = new MessageBoxResultEvent(dialogResult, commandId.Value);
            Dispatch(eventPayload, BridgeJsonContext.Default.EventMessageMessageBoxResultType);
        }
    }
}
