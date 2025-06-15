using AttendanceRecord.Application.Services;
using AttendanceRecord.Domain.Services;

namespace AttendanceRecord.Application.UseCases.WorkRecord;

public class ToggleRestUseCase(WorkRecordStore workRecordStore, WorkRecordService workRecordService)
{
    public async Task ExecuteAsync()
    {
        var workRecord = await workRecordService.ToggleRestAsync();
        await workRecordStore.ResetAsync();
    }
}
