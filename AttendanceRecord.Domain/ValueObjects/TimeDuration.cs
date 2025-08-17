using AttendanceRecord.Domain.Exceptions;
using AttendanceRecord.Domain.Extensions;
using AttendanceRecord.Domain.Utils;

namespace AttendanceRecord.Domain.ValueObjects;

public record TimeDuration
{
    public DateTime StartedOn { get; private init; }
    public DateTime? FinishedOn { get; private init; }

    public DateOnly RecordedDate => DateOnly.FromDateTime(StartedOn);
    public bool IsActive => !IsEmpty && FinishedOn == null;
    public bool IsEmpty => StartedOn == default;

    public TimeSpan GetTotalTime()
        => this switch
        {
            { IsEmpty: true } => TimeSpan.Zero,
            { IsActive: true } =>
                StartedOn.Date == DateTimeProvider.UtcNow.TruncateMs().Date
                    ? DateTimeProvider.UtcNow.TruncateMs() - StartedOn
                    : StartedOn.Date.AddDays(1) - StartedOn,
            { FinishedOn: { } } => FinishedOn.Value - StartedOn,
            _ => throw new DomainException("無効な時間の状態です。")
        };

    public static TimeDuration Reconstruct(DateTime startedOn, DateTime? finishedOn = null)
        => new() { StartedOn = startedOn.ToUniversalTime(), FinishedOn = finishedOn?.ToUniversalTime() };

    public static TimeDuration Create(DateTime startedOn, DateTime? finishedOn = null)
    {
        startedOn = startedOn.ToUniversalTime();
        finishedOn = finishedOn?.ToUniversalTime();

        if (finishedOn != null && startedOn > finishedOn)
            throw new DomainException("開始日が終了日よりも後に指定されています。");

        return new() { StartedOn = startedOn, FinishedOn = finishedOn };
    }

    internal static TimeDuration Empty => new();

    internal static TimeDuration GetStart() => Create(DateTimeProvider.UtcNow.TruncateMs());

    internal TimeDuration GetFinished() => Create(StartedOn, DateTimeProvider.UtcNow.TruncateMs());

    internal TimeDuration GetRestart() => Create(StartedOn);
}
