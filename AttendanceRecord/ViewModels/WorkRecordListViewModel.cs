using System.Text.Json;
using AttendanceRecord.Application.Dtos.Requests;
using AttendanceRecord.Application.UseCases.WorkRecord;
using AttendanceRecord.Constants;
using AttendanceRecord.Dtos.WorkRecordList;
using BrowserBridge;
using Mediator.Switch;

namespace AttendanceRecord.ViewModels;

public sealed class WorkRecordListViewModel(IEventDispatcher eventDispatcher, ISender mediator)
    : ViewModelBase<WorkRecordListCommandType>(eventDispatcher)
{
    protected override void OnFirstRender() { }

    protected override ValueTask HandleActionAsync(WorkRecordListCommandType action, JsonElement? payload, Guid? commandId)
        => (action, payload, commandId) switch
        {
            (WorkRecordListCommandType.GetWorkRecordList, { }, { }) =>
                payload.Value.HandlePayloadAsync(
                    AppJsonContext.Default.WorkRecordTallyGetRequestDto,
                    request => HandleGetWorkRecordListAsync(request, commandId.Value)),
            _ => throw new NotImplementedException()
        };

    private async ValueTask HandleGetWorkRecordListAsync(WorkRecordTallyGetRequestDto request, Guid commandId)
    {
        var result = await mediator.Send(new GetWorkRecordTally(request));
        Dispatch(new GetWorkRecordListResultEvent(commandId, result), AppJsonContext.Default.EventMessageWorkRecordTallyResponseDto);
    }
}
