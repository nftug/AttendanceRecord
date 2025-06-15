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
        Guid id = Guid.NewGuid();
        var workRecord = WorkRecord.Start(id);
        return workRecord.SetStandardWorkMinutes(configStore.Config.StandardWorkMinutes);
    }

    public WorkRecord Create(TimeDuration duration, IReadOnlyList<RestRecord> restTimes)
    {
        Guid id = Guid.NewGuid();
        var workRecord = new WorkRecord(id, duration, restTimes);
        return workRecord.SetStandardWorkMinutes(configStore.Config.StandardWorkMinutes);
    }
}
