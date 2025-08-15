using AttendanceRecord.Application.Dtos.Requests;
using AttendanceRecord.Application.UseCases.WorkRecords;
using AttendanceRecord.Constants;
using AttendanceRecord.Dtos.HistoryPage;
using BrowserBridge;
using Mediator.Switch;

namespace AttendanceRecord.ViewModels;

public sealed class HistoryPageViewModel(IEventDispatcher eventDispatcher, ISender mediator)
    : ViewModelBase<HistoryPageCommandType>(eventDispatcher)
{
    protected override ValueTask HandleActionAsync(HistoryPageCommandType action, CommandMessage message)
        => (action, message.Payload, message.CommandId) switch
        {
            (HistoryPageCommandType.GetWorkRecordList, var payload, { } commandId)
                when payload.TryParse(AppJsonContext.Default.WorkRecordTallyGetRequestDto, out var req) =>
                    HandleGetWorkRecordListAsync(req, commandId),
            (HistoryPageCommandType.GetWorkRecord, var payload, { } commandId)
                when payload.TryGetGuid(out var itemId) =>
                    HandleGetWorkRecordAsync(itemId, commandId),
            (HistoryPageCommandType.SaveWorkRecord, var payload, var commandId)
                when payload.TryParse(AppJsonContext.Default.WorkRecordSaveRequestDto, out var req) =>
                    HandleSaveWorkRecordAsync(req, commandId),
            (HistoryPageCommandType.DeleteWorkRecord, var payload, var commandId)
                when payload.TryGetGuid(out var itemId) =>
                    HandleDeleteWorkRecordAsync(itemId, commandId),
            _ => throw new NotImplementedException($"Action {action} is not implemented or payload is missing.")
        };

    private async ValueTask HandleGetWorkRecordListAsync(WorkRecordTallyGetRequestDto request, Guid commandId)
    {
        var result = await mediator.Send(new GetWorkRecordTally(request));
        Dispatch(new(result, commandId), AppJsonContext.Default.GetWorkRecordListResultEvent);
    }

    private async ValueTask HandleGetWorkRecordAsync(Guid itemId, Guid commandId)
    {
        var result = await mediator.Send(new GetWorkRecord(itemId));
        Dispatch(new(result, commandId), AppJsonContext.Default.GetWorkRecordResultEvent);
    }

    private async ValueTask HandleSaveWorkRecordAsync(WorkRecordSaveRequestDto request, Guid? commandId)
    {
        await mediator.Send(new SaveWorkRecord(request));
        Dispatch(new(commandId), AppJsonContext.Default.SaveWorkRecordResultEvent);
    }

    private async ValueTask HandleDeleteWorkRecordAsync(Guid itemId, Guid? commandId)
    {
        await mediator.Send(new DeleteWorkRecord(itemId));
        Dispatch(new(commandId), AppJsonContext.Default.DeleteWorkRecordResultEvent);
    }
}
