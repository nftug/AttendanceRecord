using StrongInject;

namespace BrowserBridge;

public interface IContainerInstance
{
    void Inject(object container);
    IOwnedService<T> Resolve<T>() where T : notnull;
}

public class StrongInjectContainerInstance : IContainerInstance
{
    private object? _container;

    public void Inject(object container) => _container = container;

    public IOwnedService<T> Resolve<T>() where T : notnull
    {
        if (_container == null)
            throw new InvalidOperationException("AppContainer is not injected");

        var owned = ((IContainer<T>)_container).Resolve();

        return new StrongInjectOwnedService<T>(owned);
    }
}

public class MsDependencyInjectionContainerInstance : IContainerInstance
{
    private IServiceProvider? _serviceProvider;

    public void Inject(object container) => _serviceProvider = (IServiceProvider)container;

    public IOwnedService<T> Resolve<T>() where T : notnull
    {
        if (_serviceProvider == null)
            throw new InvalidOperationException("AppContainer is not injected");

        return new MsDependencyInjectionOwnedService<T>(_serviceProvider);
    }
}
