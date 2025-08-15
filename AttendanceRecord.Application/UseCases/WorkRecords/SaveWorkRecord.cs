using AttendanceRecord.Application.Dtos.Requests;
using AttendanceRecord.Application.Services;
using AttendanceRecord.Domain.Entities;
using AttendanceRecord.Domain.Interfaces;
using AttendanceRecord.Domain.Services;
using Mediator.Switch;

namespace AttendanceRecord.Application.UseCases.WorkRecords;

public sealed record SaveWorkRecord(WorkRecordSaveRequestDto Body) : IRequest<Unit>;

public sealed class SaveWorkRecordHandler(
    CurrentWorkRecordStateStore workRecordStore,
    WorkRecordService workRecordService,
    IWorkRecordRepository workRecordRepository
) : IRequestHandler<SaveWorkRecord, Unit>
{
    public async Task<Unit> Handle(SaveWorkRecord request, CancellationToken cancellationToken)
    {
        WorkRecord workRecord;

        if (request.Body.Id != null)
        {
            workRecord =
                await workRecordRepository.FindByIdAsync(request.Body.Id.Value)
                ?? throw new KeyNotFoundException($"WorkRecordが見つかりません: {request.Body.Id}");
            workRecord.Update(
                request.Body.Duration.ToDomain(), request.Body.RestRecords.Select(r => r.ToDomain()));
        }
        else
        {
            workRecord = WorkRecord.Create(
                request.Body.Duration.ToDomain(),
                request.Body.RestRecords.Select(r => r.ToDomain())
            );
        }

        await workRecordService.SaveAsync(workRecord);
        await workRecordStore.ReloadAsync();
        return Unit.Value;
    }
}
