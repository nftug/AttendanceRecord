using AttendanceRecord.Application.Services;
using AttendanceRecord.Domain.Services;
using Mediator.Switch;

namespace AttendanceRecord.Application.UseCases.WorkRecords;

public sealed record DeleteWorkRecord(Guid Id) : IRequest<Unit>;

public sealed class DeleteWorkRecordHandler(
    CurrentWorkRecordStateStore workRecordStore, WorkRecordService workRecordService)
    : IRequestHandler<DeleteWorkRecord, Unit>
{
    public async Task<Unit> Handle(DeleteWorkRecord request, CancellationToken cancellationToken)
    {
        await workRecordService.DeleteAsync(request.Id);
        await workRecordStore.ReloadAsync();

        return Unit.Value;
    }
}
