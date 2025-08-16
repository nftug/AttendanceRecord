using AttendanceRecord.Domain.Exceptions;
using AttendanceRecord.Domain.Extensions;

namespace AttendanceRecord.Domain.ValueObjects;

public record TimeDuration
{
    public DateTime StartedOn { get; private init; }
    public DateTime? FinishedOn { get; private init; }

    public DateOnly RecordedDate => DateOnly.FromDateTime(StartedOn);
    public bool IsActive => !IsEmpty && FinishedOn == null;
    public bool IsEmpty => StartedOn == default;

    public TimeSpan TotalTime
        => this switch
        {
            { IsEmpty: true } => TimeSpan.Zero,
            { IsActive: true } =>
                StartedOn.Date == DateTime.Now.TruncateMs().Date
                    ? DateTime.Now.TruncateMs() - StartedOn
                    : StartedOn.Date.AddDays(1) - StartedOn,
            { FinishedOn: { } } => FinishedOn.Value - StartedOn,
            _ => throw new DomainException("無効な時間の状態です。")
        };

    public static TimeDuration Reconstruct(DateTime startedOn, DateTime? finishedOn = null)
        => new() { StartedOn = startedOn, FinishedOn = finishedOn };

    public static TimeDuration Create(DateTime startedOn, DateTime? finishedOn = null)
    {
        if (finishedOn != null && startedOn > finishedOn)
            throw new DomainException("開始日が終了日よりも後に指定されています。");

        return new() { StartedOn = startedOn, FinishedOn = finishedOn };
    }

    internal static TimeDuration Empty => new();

    internal static TimeDuration GetStart() => Create(DateTime.Now.TruncateMs());

    internal TimeDuration GetFinished() => Create(StartedOn, DateTime.Now.TruncateMs());

    internal TimeDuration GetRestart() => Create(StartedOn);
}
