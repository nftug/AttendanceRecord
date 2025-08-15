using AttendanceRecord.Domain.Config;
using AttendanceRecord.Domain.Entities;

namespace AttendanceRecord.Domain.ValueObjects;

public class WorkRecordTally(IEnumerable<WorkRecord> workRecords)
{
    private readonly IReadOnlyList<WorkRecord> _workRecords
        = [.. workRecords.OrderBy(wr => wr.RecordedDate)];

    public TimeSpan WorkTimeTotal => new(workRecords.Sum(wr => wr.TotalWorkTime.Ticks));
    public TimeSpan RestTimeTotal => new(workRecords.Sum(wr => wr.TotalRestTime.Ticks));
    public TimeSpan GetOvertimeTotal(AppConfig appConfig) =>
        new(_workRecords.Sum(wr => wr.GetOvertime(appConfig).Ticks));

    public (Guid Id, DateTime Date)[] WorkRecords => [.. _workRecords.Select(wr => (wr.Id, wr.RecordedDate))];

    public int? RecordedMonth => workRecords.FirstOrDefault()?.RecordedDate.Month;

    public static WorkRecordTally Empty => new([]);
}
