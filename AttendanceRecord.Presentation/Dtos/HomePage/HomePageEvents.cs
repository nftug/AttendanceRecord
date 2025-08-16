using AttendanceRecord.Application.Dtos.Responses;
using BrowserBridge;

namespace AttendanceRecord.Presentation.Dtos.HomePage;

public record HomePageStateEvent(CurrentWorkRecordStateDto Payload)
    : EventMessage<CurrentWorkRecordStateDto>(Payload)
{
    public override string Event => "State";
}

public record ToggleWorkResultEvent(CurrentWorkRecordStateDto Payload, Guid? CommandId)
    : CommandResultEventMessage<CurrentWorkRecordStateDto>(Payload, CommandId)
{
    public override string CommandName => nameof(HomePageCommandType.ToggleWork);
}

public record ToggleRestResultEvent(CurrentWorkRecordStateDto Payload, Guid? CommandId)
    : CommandResultEventMessage<CurrentWorkRecordStateDto>(Payload, CommandId)
{
    public override string CommandName => nameof(HomePageCommandType.ToggleRest);
}
