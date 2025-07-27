using AttendanceRecord.Application.Services;
using AttendanceRecord.Domain.Services;

namespace AttendanceRecord.Application.UseCases.WorkRecord;

public class ToggleRestUseCase(CurrentWorkRecordStateStore workRecordStore, WorkRecordService workRecordService)
{
    public async Task ExecuteAsync()
    {
        await workRecordService.ToggleRestAsync();
        await workRecordStore.ReloadAsync();
    }
}
