using AttendanceRecord.Domain.Exceptions;
using AttendanceRecord.Domain.ValueObjects;

namespace AttendanceRecord.Domain.Entities;

public class RestRecord(Guid id, TimeDuration duration)
{
    public Guid Id { get; } = id;
    public TimeDuration Duration { get; private set; } = duration;

    public DateOnly RecordedDate => Duration.RecordedDate;
    public TimeSpan TotalTime => Duration.TotalTime;
    public bool IsActive => Duration.IsActive && RecordedDate == DateOnly.FromDateTime(DateTime.Today);

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
