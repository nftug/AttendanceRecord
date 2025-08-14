using AttendanceRecord.Application.Dtos.Responses;
using BrowserBridge;

namespace AttendanceRecord.Dtos.WorkRecordList;

public record GetWorkRecordListResultEvent(WorkRecordTallyResponseDto Payload, Guid CommandId)
    : CommandResultEventMessage<WorkRecordTallyResponseDto>(Payload, CommandId)
{
    public override string CommandName => nameof(WorkRecordListCommandType.GetWorkRecordList);
}
