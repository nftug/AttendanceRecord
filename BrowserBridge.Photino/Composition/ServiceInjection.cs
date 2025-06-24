using Microsoft.Extensions.DependencyInjection;
using StrongInject;

namespace BrowserBridge.Photino;

#region StrongInject Modules
[RegisterModule(typeof(BridgeContainerModule))]
[Register(typeof(PhotinoWindowInstance), Scope.SingleInstance)]
[Register(typeof(PhotinoDialogService), Scope.SingleInstance, typeof(IDialogService))]
[Register(typeof(EventDispatcher), Scope.SingleInstance, typeof(IEventDispatcher))]
public class PhotinoContainerModule;
#endregion

#region MsDependencyInjection Modules
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPhotinoBrowserBridge(this IServiceCollection services)
    {
        services.AddBrowserBridge();
        services.AddSingleton<PhotinoWindowInstance>();
        services.AddSingleton<IDialogService, PhotinoDialogService>();
        services.AddSingleton<IEventDispatcher, EventDispatcher>();
        return services;
    }
}
#endregion
