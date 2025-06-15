using AttendanceRecord.Application.Services;
using AttendanceRecord.Domain.Services;

namespace AttendanceRecord.Application.UseCases.WorkRecord;

public class DeleteWorkRecordUseCase(WorkRecordStore workRecordStore, WorkRecordService workRecordService)
{
    public async Task ExecuteAsync(Guid id)
    {
        await workRecordService.DeleteAsync(id);

        // 今月分の記録を削除した場合のため、ストアをリセットする
        await workRecordStore.ResetAsync();
    }
}
