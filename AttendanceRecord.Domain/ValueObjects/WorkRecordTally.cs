using AttendanceRecord.Domain.Entities;

namespace AttendanceRecord.Domain.ValueObjects;

public class WorkRecordTally(IEnumerable<WorkRecord> workRecords, TimeSpan monthlyTotalBusinessTime)
{
    private readonly IReadOnlyList<WorkRecord> _workRecords
        = [.. workRecords.OrderBy(wr => wr.RecordedDate)];

    public TimeSpan WorkTimeTotal => new(workRecords.Sum(wr => wr.TotalWorkTime.Ticks));
    public TimeSpan RestTimeTotal => new(workRecords.Sum(wr => wr.TotalRestTime.Ticks));
    public TimeSpan OvertimeTotal =>
        new(_workRecords.Sum(wr => wr.TotalWorkTime.Ticks) - monthlyTotalBusinessTime.Ticks);

    public (Guid Id, DateOnly Date)[] WorkRecords => [.. _workRecords.Select(wr => (wr.Id, wr.RecordedDate))];

    public int? RecordedMonth => workRecords.FirstOrDefault()?.RecordedDate.Month;

    public static WorkRecordTally Empty => new([], TimeSpan.Zero);
}
