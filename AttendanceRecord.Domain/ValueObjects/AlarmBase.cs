using AttendanceRecord.Domain.Config;
using AttendanceRecord.Domain.Entities;

namespace AttendanceRecord.Domain.ValueObjects;

public abstract record AlarmBase<TSelf>
    where TSelf : AlarmBase<TSelf>
{
    public bool IsTriggered { get; private set; }
    public bool IsSnoozed => SnoozedUntil != null && SnoozedUntil > DateTime.Now;
    public DateTime? SnoozedUntil { get; private set; }

    public TSelf TryTrigger(WorkRecord workRecord, AppConfig appConfig) =>
        ShouldTrigger(workRecord, appConfig) && !IsSnoozed
            ? (TSelf)this with { IsTriggered = true, SnoozedUntil = null }
            : (TSelf)this with { IsTriggered = false };

    public TSelf MarkSnooze(AppConfig appConfig) =>
        IsSnoozed
            ? (TSelf)this
            : (TSelf)this with { SnoozedUntil = DateTime.Now.Add(GetSnoozeDuration(appConfig)) };

    protected abstract bool ShouldTrigger(WorkRecord workRecord, AppConfig appConfig);

    protected abstract TimeSpan GetSnoozeDuration(AppConfig appConfig);
}
