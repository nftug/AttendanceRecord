namespace BrowserBridge;

public enum WindowCommandType
{
    MessageBox,
    SetMinimized
}

public record MessageBoxCommandPayload(
    string? Title,
    string Message,
    ButtonsType Buttons = ButtonsType.Ok,
    IconType Icon = IconType.Info
);
