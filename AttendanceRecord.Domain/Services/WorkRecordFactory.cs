using AttendanceRecord.Domain.Entities;
using AttendanceRecord.Domain.Interfaces;

namespace AttendanceRecord.Domain.Services;

public class WorkRecordFactory(IAppConfigRepository configRepository, IWorkRecordRepository workRecordRepository)
{
    public async Task<WorkRecord?> FindByIdAsync(Guid id)
    {
        var workRecord = await workRecordRepository.FindByIdAsync(id);
        return workRecord?.SetStandardWorkMinutes(configRepository.Config.StandardWorkMinutes);
    }

    public async Task<WorkRecord?> FindByDateAsync(DateTime date)
    {
        var workRecord = await workRecordRepository.FindByDateAsync(date);
        return workRecord?.SetStandardWorkMinutes(configRepository.Config.StandardWorkMinutes);
    }

    public async Task<IReadOnlyList<WorkRecord>> FindByMonthAsync(DateTime month)
    {
        var workRecords = await workRecordRepository.FindByMonthAsync(month);
        return workRecords
            .Select(x => x.SetStandardWorkMinutes(configRepository.Config.StandardWorkMinutes))
            .ToList();
    }

    public WorkRecord CreateAndStart()
    {
        var workRecord = WorkRecord.Start();
        return workRecord.SetStandardWorkMinutes(configRepository.Config.StandardWorkMinutes);
    }

    public WorkRecord Create(DateTime startedOn, DateTime? finishedOn, IReadOnlyList<RestRecord> restTimes)
    {
        var workRecord = WorkRecord.Create(startedOn, finishedOn, restTimes);
        return workRecord.SetStandardWorkMinutes(configRepository.Config.StandardWorkMinutes);
    }
}
