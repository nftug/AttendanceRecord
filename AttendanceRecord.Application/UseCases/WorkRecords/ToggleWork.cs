using AttendanceRecord.Application.Dtos.Responses;
using AttendanceRecord.Application.Services;
using AttendanceRecord.Domain.Entities;
using AttendanceRecord.Domain.Interfaces;
using AttendanceRecord.Domain.Utils;
using Mediator.Switch;

namespace AttendanceRecord.Application.UseCases.WorkRecords;

public sealed record ToggleWork : IRequest<CurrentWorkRecordStateDto>;

public sealed class ToggleWorkHandler(
    CurrentWorkRecordStateStore workRecordStore, IWorkRecordRepository repository)
    : IRequestHandler<ToggleWork, CurrentWorkRecordStateDto>
{
    public async Task<CurrentWorkRecordStateDto> Handle(ToggleWork request, CancellationToken cancellationToken)
    {
        var workToday = await repository.FindByDateAsync(DateTimeProvider.Today);
        workToday = workToday?.ToggleWork() ?? WorkRecord.Start();
        await repository.SaveAsync(workToday);

        await workRecordStore.ReloadAsync();
        return workRecordStore.CurrentWorkRecordState.CurrentValue;
    }
}
