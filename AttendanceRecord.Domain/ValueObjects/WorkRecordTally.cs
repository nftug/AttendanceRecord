using AttendanceRecord.Domain.Config;
using AttendanceRecord.Domain.Entities;

namespace AttendanceRecord.Domain.ValueObjects;

public class WorkRecordTally(IEnumerable<WorkRecord> workRecords)
{
    private readonly IReadOnlyList<WorkRecord> _workRecords
        = [.. workRecords.OrderBy(wr => wr.RecordedDate)];

    public TimeSpan GetWorkTimeTotal() =>
        new(_workRecords.Sum(wr => wr.GetWorkTime().Ticks));

    public TimeSpan GetRestTimeTotal() =>
        new(_workRecords.Sum(wr => wr.GetRestTime().Ticks));

    public TimeSpan GetOvertimeTotal(AppConfig appConfig) =>
        new(_workRecords.Sum(wr => wr.GetOvertime(appConfig).Ticks));

    public (Guid Id, DateOnly Date)[] WorkRecords => [.. _workRecords.Select(wr => (wr.Id, wr.RecordedDate))];

    public YearAndMonth? RecordedYearAndMonth =>
        _workRecords.FirstOrDefault()?.RecordedDate is { } date ? YearAndMonth.FromDate(date) : null;

    public static WorkRecordTally Empty => new([]);
}
