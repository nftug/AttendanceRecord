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
            (WindowCommandType.SendNotification, var payload, var commandId)
                when payload.TryParse(
                    BridgeJsonContext.Default.SendNotificationCommandPayload, out var notificationPayload) =>
                    SendNotificationAsync(notificationPayload, commandId),
            _ => throw new NotImplementedException($"Action {action} is not implemented or payload is missing.")
        };

    private ValueTask ShowMessageBox(MessageBoxCommandPayload command, Guid? commandId)
    {
        string title = command.Title ?? EnvironmentConstants.AppName;
        var dialogResult = windowService.ShowMessageBox(command.Message, title, command.Buttons, command.Icon);

        Dispatch(new(dialogResult, commandId), BridgeJsonContext.Default.MessageBoxResultEvent);

        return ValueTask.CompletedTask;
    }

    private async ValueTask SetMinimizedAsync(bool minimized, Guid? commandId)
    {
        windowService.SetMinimized(minimized);

        await Task.Delay(100); // Allow time for the window state to change
        Dispatch(new(commandId), BridgeJsonContext.Default.SetMinimizedResultEvent);
    }

    private ValueTask SendNotificationAsync(SendNotificationCommandPayload payload, Guid? commandId)
    {
        windowService.SendNotification(payload.Title, payload.Message);
        Dispatch(new(commandId), BridgeJsonContext.Default.SendNotificationResultEvent);
        return ValueTask.CompletedTask;
    }
}
