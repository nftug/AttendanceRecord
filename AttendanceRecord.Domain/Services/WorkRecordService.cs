using AttendanceRecord.Domain.Entities;
using AttendanceRecord.Domain.Exceptions;
using AttendanceRecord.Domain.Interfaces;

namespace AttendanceRecord.Domain.Services;

public class WorkRecordService(WorkRecordFactory workRecordFactory, IWorkRecordRepository workRecordRepository)
{
    public async Task<WorkRecord> ToggleWorkAsync()
    {
        var workToday = await workRecordFactory.FindByDateAsync(DateTime.Today);

        workToday = workToday?.ToggleWork() ?? workRecordFactory.CreateAndStart();
        await workRecordRepository.SaveAsync(workToday);

        return workToday;
    }

    public async Task<WorkRecord> ToggleRestAsync()
    {
        var workToday = await workRecordFactory.FindByDateAsync(DateTime.Today)
            ?? throw new DomainException("本日の勤務記録が存在しません。");

        workToday = workToday.ToggleRest();
        await workRecordRepository.SaveAsync(workToday);

        return workToday;
    }

    public async Task<WorkRecord> SaveAsync(WorkRecord workRecord)
    {
        await workRecordRepository.SaveAsync(workRecord);
        return workRecord;
    }

    public async Task DeleteAsync(Guid id)
    {
        await workRecordRepository.DeleteAsync(id);
    }
}
