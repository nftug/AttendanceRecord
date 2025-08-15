using AttendanceRecord.Domain.Config;
using AttendanceRecord.Domain.ValueObjects;

namespace AttendanceRecord.Application.Dtos.Responses;

public record WorkRecordTallyResponseDto(
    WorkRecordItemResponseDto[] WorkRecords,
    TimeSpan WorkTimeTotal,
    TimeSpan RestTimeTotal,
    TimeSpan OvertimeTotal
)
{
    public static WorkRecordTallyResponseDto FromDomain(WorkRecordTally tally, AppConfig appConfig)
        => new(
            [.. tally.WorkRecords.Select(WorkRecordItemResponseDto.FromDomain)],
            tally.WorkTimeTotal,
            tally.RestTimeTotal,
            tally.GetOvertimeTotal(appConfig)
        );

    public static WorkRecordTallyResponseDto Empty => FromDomain(new([]), AppConfig.Default);
}
