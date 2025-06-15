using AttendanceRecord.Domain.Entities;

namespace AttendanceRecord.Domain.ValueObjects;

public class WorkRecordTally(IEnumerable<WorkRecord> workRecords)
{
    public TimeSpan WorkTimeTotal => new(workRecords.Sum(wr => wr.TotalWorkTime.Ticks));
    public TimeSpan RestTimeTotal => new(workRecords.Sum(wr => wr.TotalRestTime.Ticks));

    public IReadOnlyList<WorkRecord> WorkRecords { get; }
        = workRecords.OrderBy(wr => wr.RecordedDate).ToList();

    public WorkRecordTally Recreate(WorkRecord workRecord)
    {
        var workRecords = WorkRecords.ToList();

        var idx = workRecords.ToList().FindIndex(wr => wr.Id == workRecord.Id);
        if (idx < 0) return this;

        workRecords[idx] = workRecord;

        return new(workRecords);
    }
}
