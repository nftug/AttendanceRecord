using AttendanceRecord.Domain.Config;
using AttendanceRecord.Domain.Entities;

namespace AttendanceRecord.Domain.ValueObjects;

public class WorkRecordTally(IEnumerable<WorkRecord> workRecords)
{
    private readonly IReadOnlyList<WorkRecord> _workRecords
        = [.. workRecords.OrderBy(wr => wr.RecordedDate)];

    public TimeSpan GetWorkTimeTotal() =>
        new(_workRecords.Sum(wr => wr.GetTotalWorkTime().Ticks));
    public TimeSpan GetRestTimeTotal() =>
        new(_workRecords.Sum(wr => wr.GetTotalRestTime().Ticks));
    public TimeSpan GetOvertimeTotal(AppConfig appConfig) =>
        new(_workRecords.Sum(wr => wr.GetOvertime(appConfig).Ticks));

    public (Guid Id, DateOnly Date)[] WorkRecords => [.. _workRecords.Select(wr => (wr.Id, wr.RecordedDate))];

    public int? RecordedMonth => _workRecords.FirstOrDefault()?.RecordedDate.Month;

    public static WorkRecordTally Empty => new([]);
}
