using AttendanceRecord.Application.Dtos.Responses;
using BrowserBridge;

namespace AttendanceRecord.Presentation.Dtos.Alarm;

public record TriggeredEvent(AlarmResponseDto Payload)
    : EventMessage<AlarmResponseDto>(Payload)
{
    public override string Event => "Triggered";
}

public record SnoozeResultEvent(Guid? CommandId) : CommandResultEventVoidMessage(CommandId)
{
    public override string CommandName => nameof(AlarmCommandType.Snooze);
}
