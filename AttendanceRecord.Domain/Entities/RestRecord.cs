using AttendanceRecord.Domain.Exceptions;
using AttendanceRecord.Domain.Utils;
using AttendanceRecord.Domain.ValueObjects;

namespace AttendanceRecord.Domain.Entities;

public class RestRecord(Guid id, TimeDuration duration)
{
    public Guid Id { get; } = id;
    public TimeDuration Duration { get; private set; } = duration;

    public DateTime RecordedDate => Duration.RecordedDate;
    public TimeSpan GetTotalTime() => Duration.GetTotalTime();
    public bool IsActive => Duration.IsActive && RecordedDate == DateTimeProvider.Today;

    public static RestRecord Create(DateTime startedOn, DateTime? finishedOn)
        => new(Guid.NewGuid(), TimeDuration.Create(startedOn, finishedOn));

    internal static RestRecord Start() => new(Guid.NewGuid(), TimeDuration.GetStart());

    internal RestRecord Finish()
    {
        if (!IsActive)
            throw new DomainException("進行中ではない休憩記録は完了できません。");

        Duration = Duration.GetFinished();
        return this;
    }
}
