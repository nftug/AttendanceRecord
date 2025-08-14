using AttendanceRecord.Application.Dtos.Responses;
using BrowserBridge;

namespace AttendanceRecord.Dtos.HistoryPage;

public record GetWorkRecordListResultEvent(WorkRecordTallyResponseDto Payload, Guid CommandId)
    : CommandResultEventMessage<WorkRecordTallyResponseDto>(Payload, CommandId)
{
    public override string CommandName => nameof(HistoryPageCommandType.GetWorkRecordList);
}

public record GetWorkRecordResultEvent(WorkRecordResponseDto Payload, Guid CommandId)
    : CommandResultEventMessage<WorkRecordResponseDto>(Payload, CommandId)
{
    public override string CommandName => nameof(HistoryPageCommandType.GetWorkRecord);
}

public record SaveWorkRecordResultEvent(Guid? CommandId) : CommandResultEventVoidMessage(CommandId)
{
    public override string CommandName => nameof(HistoryPageCommandType.SaveWorkRecord);
}

public record DeleteWorkRecordResultEvent(Guid? CommandId) : CommandResultEventVoidMessage(CommandId)
{
    public override string CommandName => nameof(HistoryPageCommandType.DeleteWorkRecord);
}
