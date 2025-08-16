using AttendanceRecord.Domain.Config;
using AttendanceRecord.Domain.Entities;
using AttendanceRecord.Domain.Enums;

namespace AttendanceRecord.Domain.ValueObjects;

public record RestStartAlarm : AlarmBase<RestStartAlarm>
{
    public override AlarmType Type => AlarmType.RestStart;

    protected override TimeSpan GetSnoozeDuration(AppConfig appConfig) =>
        appConfig.RestStartAlarm.SnoozeTime;

    protected override bool ShouldTrigger(WorkRecord workRecord, AppConfig appConfig) =>
        appConfig.RestStartAlarm.IsEnabled
            && workRecord.IsTodaysOngoing
            && workRecord.GetTotalRestTime() == TimeSpan.Zero
            && workRecord.GetTotalWorkTime() >= appConfig.RestStartAlarm.ElapsedTime;
}
