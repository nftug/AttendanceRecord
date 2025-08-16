using BrowserBridge;
using BrowserBridge.Photino;
using Photino.NET;

namespace AttendanceRecord.Presentation;

public sealed class AppService(
    IContainerInstance containerInstance,
    PhotinoWindowInstance windowInstance,
    CommandDispatcher dispatcher,
    IErrorHandler errorHandler
) : PhotinoAppServiceBase(containerInstance, windowInstance, dispatcher, errorHandler)
{
    protected override string LocalDebugUrl => "http://localhost:5173";

    protected override PhotinoWindow SetupWindow(PhotinoWindow window)
        => window
            .SetUseOsDefaultSize(false)
            .SetSize(new(1200, 800))
            .Center()
            .SetContextMenuEnabled(EnvironmentConstants.IsDebugMode)
            .SetDevToolsEnabled(EnvironmentConstants.IsDebugMode);
}
