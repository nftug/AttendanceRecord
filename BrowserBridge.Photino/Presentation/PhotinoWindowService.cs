using System.Diagnostics;
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

    public void SendNotification(string title, string message)
    {
        if (windowInstance.Value is not { } window)
            throw new InvalidOperationException("Window instance is not ready");

        if (OperatingSystem.IsMacOS())
        {
            var script = $"display notification \"{message}\" with title \"{title}\"";
            var process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "osascript",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };
            process.StartInfo.ArgumentList.Add("-e");
            process.StartInfo.ArgumentList.Add(script);

            process.Start();
            process.WaitForExit();
        }
        else
        {
            window.SendNotification(title, message);
        }
    }
}
