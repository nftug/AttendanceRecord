namespace BrowserBridge;

public interface IWindowService
{
    MessageBoxResultType ShowMessageBox(
        string message,
        string title,
        ButtonsType buttons = ButtonsType.Ok,
        IconType icon = IconType.Info
    );

    void SetMinimized(bool minimized);
}
