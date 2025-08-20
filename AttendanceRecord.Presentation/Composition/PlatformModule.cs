using AttendanceRecord.Presentation.Services;
using BrowserBridge.Photino;
using StrongInject;

namespace AttendanceRecord.Presentation.Composition;

[RegisterFactory(typeof(TrayIconServiceFactory), Scope.SingleInstance)]
public class PlatformModule;

public class TrayIconServiceFactory(PhotinoWindowInstance windowInstance) : IFactory<ITrayIconService>
{
    public ITrayIconService Create() =>
        OperatingSystem.IsWindows() ? new WindowsTrayIconService(windowInstance) : new NoopTrayIconService();
}
