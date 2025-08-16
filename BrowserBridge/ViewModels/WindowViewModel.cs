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
            (WindowCommandType.SetMinimized, var payload, var commandId)
                when bool.TryParse(payload.ToString(), out var minimized) =>
                    SetMinimizedAsync(minimized, commandId),
            _ => throw new NotImplementedException($"Action {action} is not implemented or payload is missing.")
        };

    private ValueTask ShowMessageBox(MessageBoxCommandPayload command, Guid? commandId)
    {
        string title = command.Title ?? EnvironmentConstants.AppName;
        var dialogResult = windowService.ShowMessageBox(command.Message, title, command.Buttons, command.Icon);

        if (commandId != null)
            Dispatch(new(dialogResult, commandId.Value), BridgeJsonContext.Default.MessageBoxResultEvent);

        return ValueTask.CompletedTask;
    }

    private async ValueTask SetMinimizedAsync(bool minimized, Guid? commandId)
    {
        windowService.SetMinimized(minimized);

        await Task.Delay(100); // Allow time for the window state to change
        Dispatch(new(commandId), BridgeJsonContext.Default.SetMinimizedResultEvent);
    }
}
