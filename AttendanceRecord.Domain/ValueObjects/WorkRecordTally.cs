using AttendanceRecord.Domain.Entities;

namespace AttendanceRecord.Domain.ValueObjects;

public class WorkRecordTally(IEnumerable<WorkRecord> workRecords)
{
    private readonly IReadOnlyList<WorkRecord> _workRecords
        = workRecords.OrderBy(wr => wr.RecordedDate).ToList();

    public TimeSpan WorkTimeTotal => new(workRecords.Sum(wr => wr.TotalWorkTime.Ticks));
    public TimeSpan RestTimeTotal => new(workRecords.Sum(wr => wr.TotalRestTime.Ticks));
    public (Guid Id, DateTime Date)[] WorkRecords => [.. workRecords.Select(wr => (wr.Id, wr.RecordedDate))];

    public int RecordedMonth
        => workRecords.FirstOrDefault()?.RecordedDate.Month ?? DateTime.MinValue.Month;

    public WorkRecordTally Recreate(WorkRecord workRecord)
    {
        var workRecords = _workRecords.ToList();

        var idx = workRecords.ToList().FindIndex(wr => wr.Id == workRecord.Id);
        if (idx < 0) return this;

        workRecords[idx] = workRecord;

        return new(workRecords);
    }
}
