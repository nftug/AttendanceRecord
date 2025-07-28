using AttendanceRecord.Application.Services;
using AttendanceRecord.Domain.Interfaces;
using AttendanceRecord.Domain.Services;
using AttendanceRecord.Infrastructure.Repositories;
using AttendanceRecord.Infrastructure.Services;
using AttendanceRecord.ViewModels;
using BrowserBridge;
using BrowserBridge.Photino;
using StrongInject;

namespace AttendanceRecord.Composition;

#region Modules
[RegisterModule(typeof(PhotinoContainerModule))]
[RegisterModule(typeof(MinimalLoggerModule))]
[Register(typeof(AppService), Scope.SingleInstance)]
public class AppBaseModule;

[Register(typeof(IntervalService), Scope.SingleInstance)]
[Register(typeof(WorkRecordService), Scope.SingleInstance)]
[Register(typeof(WorkRecordFactory), Scope.SingleInstance)]
[Register(typeof(CurrentWorkRecordStateStore), Scope.SingleInstance)]
[Register(typeof(WorkRecordRepository), Scope.SingleInstance, typeof(IWorkRecordRepository))]
public class WorkRecordModule;

[Register(typeof(AppDataDirectoryService), Scope.SingleInstance)]
[Register(typeof(AppConfigStore), Scope.SingleInstance)]
[Register(typeof(AppConfigRepository), Scope.SingleInstance, typeof(IAppConfigRepository))]
public class AppConfigModule;

[Register(typeof(HomePageViewModel))]
[Register(typeof(ViewModelResolver<HomePageViewModel>), typeof(IViewModelResolver))]
public class ViewModelModule;
#endregion

#region ViewModel Container
public interface IViewModelContainer : IViewModelContainerBase, IContainer<HomePageViewModel>;
#endregion

#region Container
[RegisterModule(typeof(AppBaseModule))]
[RegisterModule(typeof(MediatorModule))]
[RegisterModule(typeof(WorkRecordModule))]
[RegisterModule(typeof(AppConfigModule))]
[RegisterModule(typeof(ViewModelModule))]
public partial class AppContainer : IContainer<AppService>, IViewModelContainer, IMediatorContainer;
#endregion
