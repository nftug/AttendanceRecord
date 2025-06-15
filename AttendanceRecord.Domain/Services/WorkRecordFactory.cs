using AttendanceRecord.Domain.Entities;
using AttendanceRecord.Domain.Interfaces;
using AttendanceRecord.Domain.ValueObjects;

namespace AttendanceRecord.Domain.Services;

public class WorkRecordFactory(AppConfigStore configStore, IWorkRecordRepository workRecordRepository)
{
    public async Task<WorkRecord?> FindByIdAsync(Guid id)
    {
        var workRecord = await workRecordRepository.FindByIdAsync(id);
        return workRecord?.SetStandardWorkMinutes(configStore.Config.StandardWorkMinutes);
    }

    public async Task<WorkRecord?> FindByDateAsync(DateTime date)
    {
        var workRecord = await workRecordRepository.FindByDateAsync(date);
        return workRecord?.SetStandardWorkMinutes(configStore.Config.StandardWorkMinutes);
    }

    public async Task<WorkRecordTally> GetMonthlyTallyAsync(DateTime month)
    {
        var workRecords = (await workRecordRepository.FindByMonthAsync(month))
            .Select(x => x.SetStandardWorkMinutes(configStore.Config.StandardWorkMinutes));
        return new(workRecords);
    }

    public WorkRecord CreateAndStart()
    {
        var workRecord = WorkRecord.Start();
        return workRecord.SetStandardWorkMinutes(configStore.Config.StandardWorkMinutes);
    }

    public WorkRecord Create(DateTime startedOn, DateTime? finishedOn, IReadOnlyList<RestRecord> restTimes)
    {
        var workRecord = WorkRecord.Create(startedOn, finishedOn, restTimes);
        return workRecord.SetStandardWorkMinutes(configStore.Config.StandardWorkMinutes);
    }
}
