namespace BrowserBridge;

public enum WindowCommandType
{
    MessageBox,
    SetMinimized,
    SendNotification
}

public record MessageBoxCommandPayload(
    string? Title,
    string Message,
    ButtonsType Buttons = ButtonsType.Ok,
    IconType Icon = IconType.Info
);

public record SendNotificationCommandPayload(string Title, string Message);
