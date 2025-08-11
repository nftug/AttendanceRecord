using AttendanceRecord.Application.Dtos.Responses;
using AttendanceRecord.Application.Services;
using AttendanceRecord.Domain.Services;
using Mediator.Switch;

namespace AttendanceRecord.Application.UseCases.WorkRecord;

public sealed record ToggleWork : IRequest<CurrentWorkRecordStateDto>;

public sealed class ToggleWorkHandler(
    CurrentWorkRecordStateStore workRecordStore, WorkRecordService workRecordService)
    : IRequestHandler<ToggleWork, CurrentWorkRecordStateDto>
{
    public async Task<CurrentWorkRecordStateDto> Handle(ToggleWork request, CancellationToken cancellationToken)
    {
        await workRecordService.ToggleWorkAsync();
        await workRecordStore.ReloadAsync();
        return workRecordStore.CurrentWorkRecordState.CurrentValue;
    }
}
