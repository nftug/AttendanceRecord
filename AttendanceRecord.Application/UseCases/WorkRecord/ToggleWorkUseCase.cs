using AttendanceRecord.Application.Services;
using AttendanceRecord.Domain.Services;

namespace AttendanceRecord.Application.UseCases.WorkRecord;

public class ToggleWorkUseCase(WorkRecordStore workRecordStore, WorkRecordService workRecordService)
{
    public async Task ExecuteAsync()
    {
        var workRecord = await workRecordService.ToggleWorkAsync();
        await workRecordStore.ReloadAsync();
    }
}
