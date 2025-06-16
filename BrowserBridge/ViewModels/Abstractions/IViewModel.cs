namespace BrowserBridge;

public interface IViewModel : IDisposable
{
    ValueTask HandleAsync(CommandMessage message);
    void SetViewId(Guid viewId);
}
