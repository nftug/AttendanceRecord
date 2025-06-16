namespace BrowserBridge;

public interface IDialogService
{
    MessageBoxResultType ShowMessageBox(
        string message,
        string title,
        ButtonsType buttons = ButtonsType.Ok,
        IconType icon = IconType.Info
    );
}
