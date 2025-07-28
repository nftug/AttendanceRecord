using AttendanceRecord.Application.Dtos.Responses;
using BrowserBridge;

namespace AttendanceRecord.Dtos.HomePage;

public record CurrentWorkRecordStateEvent(CurrentWorkRecordStateDto State)
    : EventMessage<CurrentWorkRecordStateDto>("workRecordState", State);

public record WorkRecordTallyStateEvent(WorkRecordTallyResponseDto State)
    : EventMessage<WorkRecordTallyResponseDto>("workRecordTallyState", State);

public record ToggleWorkResultEvent : EventMessage<DummyEventPayload>
{
    public ToggleWorkResultEvent(Guid commandId) : base(new(), commandId, "toggleWork") { }
}

public record ToggleRestResultEvent : EventMessage<DummyEventPayload>
{
    public ToggleRestResultEvent(Guid commandId) : base(new(), commandId, "toggleRest") { }
}
