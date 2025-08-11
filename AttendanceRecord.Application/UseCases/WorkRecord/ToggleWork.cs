using AttendanceRecord.Application.Services;
using AttendanceRecord.Domain.Services;
using Mediator.Switch;

namespace AttendanceRecord.Application.UseCases.WorkRecord;

public sealed record ToggleWork : IRequest<Unit>;

public sealed class ToggleWorkHandler(
    CurrentWorkRecordStateStore workRecordStore, WorkRecordService workRecordService)
    : IRequestHandler<ToggleWork, Unit>
{
    public async Task<Unit> Handle(ToggleWork request, CancellationToken cancellationToken)
    {
        await workRecordService.ToggleWorkAsync();
        await workRecordStore.ReloadAsync();
        return Unit.Value;
    }
}
