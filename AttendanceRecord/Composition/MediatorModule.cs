using AttendanceRecord.Application.UseCases.WorkRecord;
using Mediator.Switch;
using StrongInject;

namespace AttendanceRecord.Composition;

[Register(typeof(ToggleWorkHandler))]
[Register(typeof(ToggleRestHandler))]
[Register(typeof(GetWorkRecordTallyHandler))]
[Register(typeof(GetWorkRecordHandler))]
[Register(typeof(SaveWorkRecordHandler))]
[Register(typeof(DeleteWorkRecordHandler))]
[Register(typeof(Application.Mediator), Scope.SingleInstance, typeof(ISender), typeof(IPublisher))]
[Register(typeof(SwitchMediatorServiceProvider), Scope.SingleInstance, typeof(ISwitchMediatorServiceProvider))]
public class MediatorModule;

public interface IMediatorContainer : IContainer<ToggleWorkHandler>,
                                      IContainer<ToggleRestHandler>,
                                      IContainer<GetWorkRecordTallyHandler>,
                                      IContainer<GetWorkRecordHandler>,
                                      IContainer<SaveWorkRecordHandler>,
                                      IContainer<DeleteWorkRecordHandler>;
