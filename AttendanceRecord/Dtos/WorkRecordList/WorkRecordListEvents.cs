using AttendanceRecord.Application.Dtos.Responses;
using BrowserBridge;

namespace AttendanceRecord.Dtos.WorkRecordList;

public record GetWorkRecordListResultEvent : EventMessage<WorkRecordTallyResponseDto>
{
    public GetWorkRecordListResultEvent(Guid commandId, WorkRecordTallyResponseDto result)
        : base(result, commandId, "getWorkRecordList") { }
}
