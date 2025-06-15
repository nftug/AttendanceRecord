using AttendanceRecord.Domain.ValueObjects;

namespace AttendanceRecord.Application.Dtos.Responses;

public record WorkRecordTallyResponseDto(
    (Guid Id, DateTime Date)[] WorkRecords, TimeSpan WorkTimeTotal, TimeSpan RestTimeTotal)
{
    public static WorkRecordTallyResponseDto FromDomain(WorkRecordTally tally)
        => new(tally.WorkRecords, tally.WorkTimeTotal, tally.RestTimeTotal);
}
