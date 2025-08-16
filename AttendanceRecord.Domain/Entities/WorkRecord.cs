using AttendanceRecord.Domain.Config;
using AttendanceRecord.Domain.Exceptions;
using AttendanceRecord.Domain.Extensions;
using AttendanceRecord.Domain.Utils;
using AttendanceRecord.Domain.ValueObjects;

namespace AttendanceRecord.Domain.Entities;

public class WorkRecord(Guid id, TimeDuration duration, IEnumerable<RestRecord> restRecords)
{
    public Guid Id { get; } = id;
    public TimeDuration Duration { get; private set; } = duration;
    public IReadOnlyList<RestRecord> RestRecords => [.. _restRecords];

    private readonly List<RestRecord> _restRecords = [.. restRecords.OrderBy(x => x.Duration.StartedOn)];

    public DateOnly RecordedDate => Duration.RecordedDate;
    public TimeSpan GetTotalWorkTime() => Duration.GetTotalTime() - GetTotalRestTime();
    public TimeSpan GetTotalRestTime() => new(RestRecords.Sum(x => x.GetTotalTime().Ticks));
    public TimeSpan GetOvertime(AppConfig appConfig) =>
        GetTotalWorkTime() - TimeSpan.FromMinutes(appConfig.StandardWorkMinutes);

    public bool IsTodays => RecordedDate == DateTimeProvider.Today;
    public bool IsTodaysOngoing => Duration.IsActive && IsTodays;
    public bool IsResting => IsTodaysOngoing && RestRecords.LastOrDefault()?.IsActive == true;
    public bool IsWorking => IsTodaysOngoing && !IsResting;

    public static WorkRecord Empty => new(Guid.Empty, TimeDuration.Empty, []);

    public WorkRecord Recreate() => new(Id, Duration, RestRecords);

    public WorkRecord Update(TimeDuration duration, IEnumerable<RestRecord> restRecords)
    {
        Duration = duration;
        _restRecords.Clear();
        _restRecords.AddRange(restRecords.OrderBy(x => x.Duration.StartedOn));

        return this;
    }

    public static WorkRecord Start() => new(Guid.NewGuid(), TimeDuration.GetStart(), []);

    public static WorkRecord Create(TimeDuration duration, IEnumerable<RestRecord> restRecords) =>
        new(Guid.NewGuid(), duration, restRecords);

    internal WorkRecord ToggleRest()
    {
        if (!IsTodaysOngoing)
            throw new DomainException("進行中ではない勤務記録は一時停止できません。");

        if (!IsResting) StartRest();
        else FinishRest();

        return this;
    }

    internal WorkRecord ToggleWork()
    {
        if (!IsTodays)
            throw new DomainException("本日付以外の出勤状態の切り替えはできません。");

        if (IsTodaysOngoing)
        {
            // 退勤操作
            if (IsResting) FinishRest();
            Duration = Duration.GetFinished();
        }
        else
        {
            // 再開操作
            _restRecords.Add(RestRecord.Create(Duration.FinishedOn!.Value, DateTimeProvider.Now.TruncateMs()));
            Duration = Duration.GetRestart();
        }

        return this;
    }

    private void StartRest() => _restRecords.Add(RestRecord.Start());

    private void FinishRest() => _restRecords[^1].Finish();
}
