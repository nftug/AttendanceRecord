using AttendanceRecord.Application.Dtos.Responses;
using AttendanceRecord.Application.Services;
using AttendanceRecord.Domain.Exceptions;
using AttendanceRecord.Domain.Interfaces;
using AttendanceRecord.Domain.Utils;
using Mediator.Switch;

namespace AttendanceRecord.Application.UseCases.WorkRecords;

public sealed record ToggleRest : IRequest<CurrentWorkRecordStateDto>;

public sealed class ToggleRestHandler(
    CurrentWorkRecordStateStore workRecordStore, IWorkRecordRepository repository)
    : IRequestHandler<ToggleRest, CurrentWorkRecordStateDto>
{
    public async Task<CurrentWorkRecordStateDto> Handle(ToggleRest request, CancellationToken cancellationToken)
    {
        var workToday = await repository.FindByDateAsync(DateTimeProvider.Today)
            ?? throw new DomainException("本日の勤務記録が存在しません。");

        workToday = workToday.ToggleRest();
        await repository.SaveAsync(workToday);

        await workRecordStore.ReloadAsync();
        return workRecordStore.CurrentWorkRecordState.CurrentValue;
    }
}
