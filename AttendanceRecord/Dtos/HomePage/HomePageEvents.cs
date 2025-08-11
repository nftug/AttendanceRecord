using AttendanceRecord.Application.Dtos.Responses;
using BrowserBridge;

namespace AttendanceRecord.Dtos.HomePage;

public record HomePageStateEvent(CurrentWorkRecordStateDto State)
    : EventMessage<CurrentWorkRecordStateDto>("state", State);

public record ToggleWorkResultEvent : EventMessage<CurrentWorkRecordStateDto>
{
    public ToggleWorkResultEvent(Guid commandId, CurrentWorkRecordStateDto result)
        : base(result, commandId, "toggleWork") { }
}

public record ToggleRestResultEvent : EventMessage<CurrentWorkRecordStateDto>
{
    public ToggleRestResultEvent(Guid commandId, CurrentWorkRecordStateDto result)
        : base(result, commandId, "toggleRest") { }
}
