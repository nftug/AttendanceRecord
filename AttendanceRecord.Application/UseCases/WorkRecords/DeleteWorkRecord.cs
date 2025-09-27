using AttendanceRecord.Application.Services;
using AttendanceRecord.Domain.Interfaces;
using Mediator.Switch;

namespace AttendanceRecord.Application.UseCases.WorkRecords;

public sealed record DeleteWorkRecord(Guid Id) : IRequest<Unit>;

public sealed class DeleteWorkRecordHandler(
    CurrentWorkRecordStateStore workRecordStore, IWorkRecordRepository repository)
    : IRequestHandler<DeleteWorkRecord, Unit>
{
    public async Task<Unit> Handle(DeleteWorkRecord request, CancellationToken cancellationToken)
    {
        await repository.DeleteAsync(request.Id);
        await workRecordStore.ReloadAsync();

        return Unit.Value;
    }
}
