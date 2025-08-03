using AttendanceRecord.Application.UseCases.WorkRecord;
using Mediator.Switch;
using StrongInject;

namespace AttendanceRecord.Composition;

[Register(typeof(ToggleWorkHandler), Scope.SingleInstance)]
[Register(typeof(ToggleRestHandler), Scope.SingleInstance)]
[Register(typeof(GetWorkRecordTallyHandler), Scope.SingleInstance)]
[Register(typeof(GetWorkRecordHandler), Scope.SingleInstance)]
[Register(typeof(SaveWorkRecordHandler), Scope.SingleInstance)]
[Register(typeof(DeleteWorkRecordHandler), Scope.SingleInstance)]
[Register(typeof(Application.Mediator), Scope.SingleInstance, typeof(ISender), typeof(IPublisher))]
[Register(typeof(SwitchMediatorServiceProvider), Scope.SingleInstance, typeof(ISwitchMediatorServiceProvider))]
public class MediatorModule;

public interface IMediatorContainer : IContainer<ToggleWorkHandler>,
                                      IContainer<ToggleRestHandler>,
                                      IContainer<GetWorkRecordTallyHandler>,
                                      IContainer<GetWorkRecordHandler>,
                                      IContainer<SaveWorkRecordHandler>,
                                      IContainer<DeleteWorkRecordHandler>;
