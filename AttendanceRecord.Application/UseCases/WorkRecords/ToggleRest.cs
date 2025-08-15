using AttendanceRecord.Application.Dtos.Responses;
using AttendanceRecord.Application.Services;
using AttendanceRecord.Domain.Services;
using Mediator.Switch;

namespace AttendanceRecord.Application.UseCases.WorkRecords;

public sealed record ToggleRest : IRequest<CurrentWorkRecordStateDto>;

public sealed class ToggleRestHandler(
    CurrentWorkRecordStateStore workRecordStore, WorkRecordService workRecordService)
    : IRequestHandler<ToggleRest, CurrentWorkRecordStateDto>
{
    public async Task<CurrentWorkRecordStateDto> Handle(ToggleRest request, CancellationToken cancellationToken)
    {
        await workRecordService.ToggleRestAsync();
        await workRecordStore.ReloadAsync();
        return workRecordStore.CurrentWorkRecordState.CurrentValue;
    }
}
