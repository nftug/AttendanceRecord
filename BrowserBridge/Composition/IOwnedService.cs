using Microsoft.Extensions.DependencyInjection;
using StrongInject;

namespace BrowserBridge;

public interface IOwnedService<out T> : IDisposable
    where T : notnull
{
    T Value { get; }
}

public sealed class StrongInjectOwnedService<T>(IOwned<T> owned) : IOwnedService<T>
    where T : notnull
{
    public T Value => owned.Value;

    public void Dispose() => owned.Dispose();
}

public sealed class MsDependencyInjectionOwnedService<T>(IServiceProvider serviceProvider) : IOwnedService<T>
    where T : notnull
{
    private readonly IServiceScope scope = serviceProvider.CreateScope();

    public T Value => scope.ServiceProvider.GetRequiredService<T>();

    public void Dispose() => scope.Dispose();
}
