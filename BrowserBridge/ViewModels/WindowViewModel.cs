namespace BrowserBridge;

public sealed class WindowViewModel(IEventDispatcher eventDispatcher, IWindowService windowService)
    : ViewModelBase<WindowCommandType>(eventDispatcher)
{
    protected override void OnFirstRender() { }

    protected override ValueTask HandleActionAsync(WindowCommandType action, CommandMessage message)
        => (action, message.Payload, message.CommandId) switch
        {
            (WindowCommandType.MessageBox, var payload, { } commandId)
                when payload.TryParse(BridgeJsonContext.Default.MessageBoxCommandPayload, out var msgBoxPayload) =>
                    ShowMessageBox(msgBoxPayload, commandId),
            (WindowCommandType.SetMinimized, var payload, _)
                when bool.TryParse(payload.ToString(), out var minimized) =>
                    SetMinimized(minimized),
            _ => ValueTask.CompletedTask
        };

    private ValueTask ShowMessageBox(MessageBoxCommandPayload command, Guid? commandId)
    {
        string title = command.Title ?? EnvironmentConstants.AppName;
        var dialogResult = windowService.ShowMessageBox(command.Message, title, command.Buttons, command.Icon);

        if (commandId != null)
            Dispatch(new(dialogResult, commandId.Value), BridgeJsonContext.Default.MessageBoxResultEvent);

        return ValueTask.CompletedTask;
    }

    private ValueTask SetMinimized(bool minimized)
    {
        windowService.SetMinimized(minimized);
        return ValueTask.CompletedTask;
    }
}
