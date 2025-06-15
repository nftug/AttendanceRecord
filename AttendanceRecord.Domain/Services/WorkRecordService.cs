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
        var existingRecord = await workRecordFactory.FindByDateAsync(workRecord.RecordedDate);
        if (existingRecord != null && existingRecord.Id != workRecord.Id)
            throw new DomainException("同じ日に複数の勤務記録を保存することはできません。");

        await workRecordRepository.SaveAsync(workRecord);
        return workRecord;
    }

    public async Task DeleteAsync(Guid id)
    {
        await workRecordRepository.DeleteAsync(id);
    }
}
