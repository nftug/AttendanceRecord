using AttendanceRecord.Application.Services;
using AttendanceRecord.Domain.Interfaces;
using AttendanceRecord.Domain.Services;
using AttendanceRecord.Infrastructure.Repositories;
using AttendanceRecord.Infrastructure.Services;
using AttendanceRecord.Presentation.ViewModels;
using BrowserBridge;
using BrowserBridge.Photino;
using StrongInject;

namespace AttendanceRecord.Presentation.Composition;

#region Modules
[RegisterModule(typeof(PhotinoContainerModule))]
[RegisterModule(typeof(MinimalLoggerModule))]
[Register(typeof(AppService), Scope.SingleInstance)]
public class AppBaseModule;

[Register(typeof(IntervalService), Scope.SingleInstance)]
[Register(typeof(WorkRecordService), Scope.SingleInstance)]
[Register(typeof(CurrentWorkRecordStateStore), Scope.SingleInstance)]
[Register(typeof(WorkRecordAlarmService), Scope.SingleInstance)]
[Register(typeof(WorkRecordRepository), Scope.SingleInstance, typeof(IWorkRecordRepository))]
[Register(typeof(WorkRecordTallyFactory), Scope.SingleInstance)]
public class WorkRecordModule;

[Register(typeof(AppDataDirectoryService), Scope.SingleInstance)]
[Register(typeof(AppConfigStore), Scope.SingleInstance)]
[Register(typeof(AppConfigRepository), Scope.SingleInstance, typeof(IAppConfigRepository))]
public class AppConfigModule;

[Register(typeof(HomePageViewModel))]
[Register(typeof(ViewModelResolver<HomePageViewModel>), typeof(IViewModelResolver))]
[Register(typeof(HistoryPageViewModel))]
[Register(typeof(ViewModelResolver<HistoryPageViewModel>), typeof(IViewModelResolver))]
[Register(typeof(AlarmViewModel))]
[Register(typeof(ViewModelResolver<AlarmViewModel>), typeof(IViewModelResolver))]
public class ViewModelModule;
#endregion

#region ViewModel Container
public interface IViewModelContainer : IViewModelContainerBase,
                                       IContainer<HomePageViewModel>,
                                       IContainer<HistoryPageViewModel>,
                                       IContainer<AlarmViewModel>;
#endregion

#region Container
[RegisterModule(typeof(AppBaseModule))]
[RegisterModule(typeof(MediatorModule))]
[RegisterModule(typeof(WorkRecordModule))]
[RegisterModule(typeof(AppConfigModule))]
[RegisterModule(typeof(ViewModelModule))]
public partial class AppContainer : IContainer<AppService>, IViewModelContainer, IMediatorContainer;
#endregion
