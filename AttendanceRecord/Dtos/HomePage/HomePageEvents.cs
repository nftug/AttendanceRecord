using AttendanceRecord.Application.Dtos.Responses;
using BrowserBridge;

namespace AttendanceRecord.Dtos.HomePage;

public record CurrentWorkRecordStateEvent(CurrentWorkRecordStateDto State)
    : EventMessage<CurrentWorkRecordStateDto>("workRecordState", State);

public record WorkRecordTallyStateEvent(WorkRecordTallyResponseDto State)
    : EventMessage<WorkRecordTallyResponseDto>("workRecordTallyState", State);

public record ToggleWorkEvent : EventMessage<DummyEventPayload>
{
    public ToggleWorkEvent(Guid commandId) : base(new(), commandId, "toggleWork") { }
}

public record ToggleRestEvent : EventMessage<DummyEventPayload>
{
    public ToggleRestEvent(Guid commandId) : base(new(), commandId, "toggleRest") { }
}
