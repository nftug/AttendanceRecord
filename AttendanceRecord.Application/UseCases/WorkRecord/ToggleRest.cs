using AttendanceRecord.Application.Services;
using AttendanceRecord.Domain.Services;
using Mediator.Switch;

namespace AttendanceRecord.Application.UseCases.WorkRecord;

[RequestHandler(typeof(ToggleRestHandler))]
public sealed record ToggleRest : IRequest<Unit>;

public sealed class ToggleRestHandler(
    CurrentWorkRecordStateStore workRecordStore, WorkRecordService workRecordService)
    : IRequestHandler<ToggleRest, Unit>
{
    public async Task<Unit> Handle(ToggleRest request, CancellationToken cancellationToken)
    {
        await workRecordService.ToggleRestAsync();
        await workRecordStore.ReloadAsync();
        return Unit.Value;
    }
}
