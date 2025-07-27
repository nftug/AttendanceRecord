using AttendanceRecord.Application.Services;
using AttendanceRecord.Domain.Services;

namespace AttendanceRecord.Application.UseCases.WorkRecord;

public class ToggleWorkUseCase(CurrentWorkRecordStateStore workRecordStore, WorkRecordService workRecordService)
{
    public async Task ExecuteAsync()
    {
        await workRecordService.ToggleWorkAsync();
        await workRecordStore.ReloadAsync();
    }
}
