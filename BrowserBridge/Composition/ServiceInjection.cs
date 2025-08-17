using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using StrongInject;

namespace BrowserBridge;

#region StrongInject Modules
[RegisterFactory(typeof(CommandDispatcherFactory), Scope.SingleInstance)]
[Register(typeof(ErrorHandler), Scope.SingleInstance, typeof(IErrorHandler))]
[Register(typeof(StrongInjectContainerInstance), Scope.SingleInstance, typeof(IContainerInstance))]
[Register(typeof(WindowViewModel))]
[Register(typeof(ViewModelResolver<WindowViewModel>), typeof(IViewModelResolver))]
public class BridgeContainerModule;

[Register(typeof(ConsoleLogWriter), typeof(ILogWriter))]
[Register(typeof(MinimalLogger<>), typeof(ILogger<>))]
public class MinimalLoggerModule;

public interface IViewModelContainerBase : IContainer<WindowViewModel>;

public class CommandDispatcherFactory(
    IViewModelResolver[] viewModelResolvers,
    ILogger<CommandDispatcher> logger,
    IErrorHandler errorHandler,
    IEventDispatcher eventDispatcher) : IFactory<CommandDispatcher>
{
    public CommandDispatcher Create() => new(viewModelResolvers, logger, errorHandler, eventDispatcher);
}
#endregion

#region MsDependencyInjection Modules
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBrowserBridge(this IServiceCollection services)
    {
        services.AddSingleton<CommandDispatcher>();
        services.AddSingleton<IErrorHandler, ErrorHandler>();
        services.AddSingleton<IContainerInstance, MsDependencyInjectionContainerInstance>();

        services.AddScoped<WindowViewModel>();
        services.AddScoped<IViewModelResolver, ViewModelResolver<WindowViewModel>>();

        return services;
    }

    public static IServiceCollection AddMinimalLogger(this IServiceCollection services)
    {
        services.AddSingleton<ILogWriter, ConsoleLogWriter>();
        services.AddTransient(typeof(ILogger<>), typeof(MinimalLogger<>));

        return services;
    }
}
#endregion