using AttendanceRecord.Domain.Config;
using AttendanceRecord.Domain.Entities;

namespace AttendanceRecord.Domain.ValueObjects;

public record RestStartAlarm : AlarmBase<RestStartAlarm>
{
    protected override TimeSpan GetSnoozeDuration(AppConfig appConfig) =>
        appConfig.RestStartAlarm.SnoozeTime;

    protected override bool ShouldTrigger(WorkRecord workRecord, AppConfig appConfig) =>
        appConfig.RestStartAlarm.IsEnabled
            && workRecord.IsTodaysOngoing
            && workRecord.TotalRestTime == TimeSpan.Zero
            && workRecord.TotalWorkTime >= appConfig.RestStartAlarm.ElapsedTime;
}
