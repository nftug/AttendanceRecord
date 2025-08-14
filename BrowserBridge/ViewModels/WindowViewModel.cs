namespace BrowserBridge;

public sealed class WindowViewModel(IEventDispatcher eventDispatcher, IDialogService dialogService)
    : ViewModelBase<WindowCommandType>(eventDispatcher)
{
    protected override void OnFirstRender() { }

    protected override ValueTask HandleActionAsync(WindowCommandType action, CommandMessage message)
        => (action, message.Payload, message.CommandId) switch
        {
            (WindowCommandType.MessageBox, var payload, { } commandId)
                when payload.TryParse(BridgeJsonContext.Default.MessageBoxCommandPayload, out var msgBoxPayload) =>
                    ShowMessageBox(msgBoxPayload, commandId),
            _ => ValueTask.CompletedTask
        };

    private ValueTask ShowMessageBox(MessageBoxCommandPayload command, Guid? commandId)
    {
        string title = command.Title ?? EnvironmentConstants.AppName;
        var dialogResult = dialogService.ShowMessageBox(command.Message, title, command.Buttons, command.Icon);

        if (commandId != null)
            Dispatch(new(dialogResult, commandId.Value), BridgeJsonContext.Default.MessageBoxResultEvent);

        return ValueTask.CompletedTask;
    }
}
