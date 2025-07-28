using BrowserBridge;
using Mediator.Switch;

namespace AttendanceRecord.Composition;

public class SwitchMediatorServiceProvider(IContainerInstance containerInstance) : ISwitchMediatorServiceProvider
{
    public T Get<T>() where T : notnull => containerInstance.Resolve<T>().Value;
}
