using AttendanceRecord.Application.Services;
using AttendanceRecord.Domain.Services;
using Mediator.Switch;

namespace AttendanceRecord.Application.UseCases.WorkRecord;

[RequestHandler(typeof(DeleteWorkRecordHandler))]
public sealed record DeleteWorkRecordUseCase(Guid Id) : IRequest<Unit>;

public sealed class DeleteWorkRecordHandler(
    CurrentWorkRecordStateStore workRecordStore, WorkRecordService workRecordService)
    : IRequestHandler<DeleteWorkRecordUseCase, Unit>
{
    public async Task<Unit> Handle(DeleteWorkRecordUseCase request, CancellationToken cancellationToken)
    {
        await workRecordService.DeleteAsync(request.Id);
        await workRecordStore.ReloadAsync();

        return Unit.Value;
    }
}
