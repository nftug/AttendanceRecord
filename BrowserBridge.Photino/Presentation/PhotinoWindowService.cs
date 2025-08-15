using Photino.NET;

namespace BrowserBridge.Photino;

public class PhotinoWindowService(PhotinoWindowInstance windowInstance) : IWindowService
{
    public MessageBoxResultType ShowMessageBox(string message, string title, ButtonsType buttons, IconType icon)
    {
        if (windowInstance.Value is not { } window)
            throw new InvalidOperationException("Window instance is not ready");

        var actualButtons = buttons switch
        {
            ButtonsType.Ok => PhotinoDialogButtons.Ok,
            ButtonsType.OkCancel => PhotinoDialogButtons.OkCancel,
            ButtonsType.YesNo => PhotinoDialogButtons.YesNo,
            ButtonsType.YesNoCancel => PhotinoDialogButtons.YesNoCancel,
            _ => PhotinoDialogButtons.Ok,
        };

        var actualIcon = icon switch
        {
            IconType.Info => PhotinoDialogIcon.Info,
            IconType.Warning => PhotinoDialogIcon.Warning,
            IconType.Error => PhotinoDialogIcon.Error,
            IconType.Question => PhotinoDialogIcon.Question,
            _ => PhotinoDialogIcon.Info
        };

        var dialogResult = window.ShowMessage(title, message, actualButtons, actualIcon);

        return dialogResult switch
        {
            PhotinoDialogResult.Ok => MessageBoxResultType.Ok,
            PhotinoDialogResult.Cancel => MessageBoxResultType.Cancel,
            PhotinoDialogResult.Yes => MessageBoxResultType.Yes,
            PhotinoDialogResult.No => MessageBoxResultType.No,
            _ => MessageBoxResultType.Ok
        };
    }

    public void SetMinimized(bool minimized)
    {
        if (windowInstance.Value is not { } window)
            throw new InvalidOperationException("Window instance is not ready");

        window.SetMinimized(minimized);
    }
}
