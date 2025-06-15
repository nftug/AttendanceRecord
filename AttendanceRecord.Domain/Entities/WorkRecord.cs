using AttendanceRecord.Domain.Exceptions;
using AttendanceRecord.Domain.Extensions;
using AttendanceRecord.Domain.ValueObjects;

namespace AttendanceRecord.Domain.Entities;

public class WorkRecord(Guid id, TimeDuration duration, IReadOnlyList<RestRecord> restRecords)
{
    public Guid Id { get; } = id;
    public TimeDuration Duration { get; private set; } = duration;
    public IReadOnlyList<RestRecord> RestRecords => _restRecords.ToList();

    private readonly List<RestRecord> _restRecords = [.. restRecords.OrderBy(x => x.Duration.StartedOn)];
    private int _standardWorkMinutes;

    public DateTime RecordedDate => Duration.RecordedDate;
    public TimeSpan TotalRestTime => new(RestRecords.Sum(x => x.TotalTime.Ticks));
    public TimeSpan TotalWorkTime => Duration.TotalTime - TotalRestTime;
    public TimeSpan Overtime =>
        TotalWorkTime - new TimeSpan(_standardWorkMinutes / 60, _standardWorkMinutes % 60, 0);

    public bool IsTodays => RecordedDate == DateTime.Today;
    public bool IsTodaysOngoing => Duration.IsActive && IsTodays;
    public bool IsResting => IsTodaysOngoing && RestRecords.LastOrDefault()?.IsActive == true;
    public bool IsWorking => IsTodaysOngoing && !IsResting;

    internal WorkRecord SetStandardWorkMinutes(int standardWorkMinutes)
    {
        if (standardWorkMinutes <= 0)
            throw new DomainException("標準勤務時間は正の整数でなければなりません。");
        _standardWorkMinutes = standardWorkMinutes;
        return this;
    }

    public static WorkRecord Create(
        DateTime startedOn, DateTime? finishedOn, IReadOnlyList<RestRecord> restTimes)
        => new(Guid.NewGuid(), TimeDuration.Create(startedOn, finishedOn), restTimes);

    public WorkRecord EditWorkDuration(DateTime startedOn, DateTime? finishedOn)
    {
        Duration = TimeDuration.Create(startedOn, finishedOn);
        return this;
    }

    public WorkRecord EditRestDuration(Guid restId, DateTime startedOn, DateTime? finishedOn)
    {
        var rest = _restRecords.FirstOrDefault(x => x.Id == restId)
            ?? throw new DomainException("指定された休憩記録が見つかりません。");

        rest.EditDuration(startedOn, finishedOn);
        return this;
    }

    internal static WorkRecord Start() => new(Guid.NewGuid(), TimeDuration.GetStart(), []);

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
            var restDuration = TimeDuration.Create(Duration.FinishedOn!.Value, DateTime.Now.TruncateMs());
            _restRecords.Add(new(Guid.NewGuid(), restDuration));

            Duration = Duration.GetRestart();
        }

        return this;
    }

    private void StartRest() => _restRecords.Add(RestRecord.Start());

    private void FinishRest() => _restRecords[^1].Finish();
}
