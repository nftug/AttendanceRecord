using System.Text.Json;

namespace BrowserBridge;

public sealed class WindowViewModel(IEventDispatcher eventDispatcher, IDialogService dialogService)
    : ViewModelBase<WindowCommandType>(eventDispatcher)
{
    protected override void OnFirstRender() { }

    protected override ValueTask HandleActionAsync(WindowCommandType action, JsonElement? payload, Guid? commandId)
        => (action, payload) switch
        {
            (WindowCommandType.MessageBox, { }) =>
                payload.Value.HandlePayloadSync(
                    BridgeJsonContext.Default.MessageBoxCommandPayload, v => ShowMessageBox(v, commandId)),
            _ => ValueTask.CompletedTask
        };

    private void ShowMessageBox(MessageBoxCommandPayload command, Guid? commandId)
    {
        string title = command.Title ?? EnvironmentConstants.AppName;
        var dialogResult = dialogService.ShowMessageBox(command.Message, title, command.Buttons, command.Icon);

        if (commandId != null)
            Dispatch(new(dialogResult, commandId.Value), BridgeJsonContext.Default.MessageBoxResultEvent);
    }
}
