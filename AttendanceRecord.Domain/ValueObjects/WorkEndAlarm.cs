using AttendanceRecord.Domain.Config;
using AttendanceRecord.Domain.Entities;
using AttendanceRecord.Domain.Enums;

namespace AttendanceRecord.Domain.ValueObjects;

public record WorkEndAlarm : AlarmBase<WorkEndAlarm>
{
    public override AlarmType Type => AlarmType.WorkEnd;

    protected override TimeSpan GetSnoozeDuration(AppConfig appConfig) =>
        appConfig.WorkEndAlarm.SnoozeTime;

    protected override bool ShouldTrigger(WorkRecord workRecord, AppConfig appConfig) =>
        appConfig.WorkEndAlarm.IsEnabled
            && workRecord.IsTodaysOngoing
            && workRecord.GetOvertime(appConfig) >= -appConfig.WorkEndAlarm.RemainingTime;
}
