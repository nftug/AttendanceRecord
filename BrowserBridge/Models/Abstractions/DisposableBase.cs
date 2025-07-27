using R3;

namespace BrowserBridge;

public abstract class DisposableBase : IDisposable
{
    protected readonly CompositeDisposable Disposable = [];

    protected bool disposedValue;

    protected virtual void Dispose(bool disposing) { }

    public void Dispose()
    {
        Disposable.Dispose();

        Dispose(disposing: true);
        disposedValue = true;
        GC.SuppressFinalize(this);
    }
}
