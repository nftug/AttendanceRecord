using AttendanceRecord.Application.Dtos.Responses;
using BrowserBridge;

namespace AttendanceRecord.Dtos.HomePage;

public record HomePageStateEvent(CurrentWorkRecordStateDto State)
    : EventMessage<CurrentWorkRecordStateDto>("state", State);

public record ToggleWorkResultEvent : EventMessage<DummyEventPayload>
{
    public ToggleWorkResultEvent(Guid commandId) : base(new(), commandId, "toggleWork") { }
}

public record ToggleRestResultEvent : EventMessage<DummyEventPayload>
{
    public ToggleRestResultEvent(Guid commandId) : base(new(), commandId, "toggleRest") { }
}
