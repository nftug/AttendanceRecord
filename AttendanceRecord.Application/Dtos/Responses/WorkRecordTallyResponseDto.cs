namespace AttendanceRecord.Application.Dtos.Responses;

public record WorkRecordTallyResponseDto(
    WorkRecordItemResponseDto[] WorkRecords,
    TimeSpan WorkTimeTotal,
    TimeSpan RestTimeTotal,
    TimeSpan OvertimeTotal
)
{
    public static WorkRecordTallyResponseDto FromDomain(Domain.ValueObjects.WorkRecordTally tally)
        => new(
            [.. tally.WorkRecords.Select(WorkRecordItemResponseDto.FromDomain)],
            tally.WorkTimeTotal,
            tally.RestTimeTotal,
            tally.OvertimeTotal
        );

    public static WorkRecordTallyResponseDto Empty => FromDomain(new([]));
}
