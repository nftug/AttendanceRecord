using AttendanceRecord.Application.Dtos.Requests;
using AttendanceRecord.Application.Services;
using AttendanceRecord.Domain.Services;

namespace AttendanceRecord.Application.UseCases.WorkRecord;

public class SaveWorkRecordUseCase(
    CurrentWorkRecordStateStore workRecordStore, WorkRecordService workRecordService, WorkRecordFactory workRecordFactory)
{
    public async Task ExecuteAsync(WorkRecordSaveRequestDto request)
    {
        Domain.Entities.WorkRecord workRecord;

        if (request.Id != null)
        {
            workRecord =
                await workRecordFactory.FindByIdAsync(request.Id.Value)
                ?? throw new KeyNotFoundException($"WorkRecordが見つかりません: {request.Id}");
            workRecord.Update(request.Duration.ToDomain(), request.RestRecords.Select(r => r.ToDomain()));
        }
        else
        {
            workRecord = workRecordFactory.Create(
                request.Duration.ToDomain(),
                [.. request.RestRecords.Select(r => r.ToDomain())]
            );
        }

        await workRecordService.SaveAsync(workRecord);
        await workRecordStore.ReloadAsync();
    }
}
