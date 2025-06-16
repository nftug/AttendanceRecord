using Microsoft.Extensions.DependencyInjection;
using StrongInject;

namespace BrowserBridge;

public interface IOwnedService<out T> : IDisposable
    where T : class
{
    T Value { get; }
}

public class StrongInjectOwnedService<T>(IOwned<T> owned) : IOwnedService<T>
    where T : class
{
    public T Value => owned.Value;

    public void Dispose() => owned.Dispose();
}

public class MsDependencyInjectionOwnedService<T>(IServiceProvider serviceProvider) : IOwnedService<T>
    where T : class
{
    private readonly IServiceScope scope = serviceProvider.CreateAsyncScope();

    public T Value => scope.ServiceProvider.GetRequiredService<T>();

    public void Dispose() => scope.Dispose();
}
