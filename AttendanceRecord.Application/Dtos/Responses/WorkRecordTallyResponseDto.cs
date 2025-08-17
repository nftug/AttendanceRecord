using AttendanceRecord.Domain.Config;
using AttendanceRecord.Domain.ValueObjects;

namespace AttendanceRecord.Application.Dtos.Responses;

public record WorkRecordTallyResponseDto(
    DateTime? RecordedDate,
    WorkRecordItemResponseDto[] WorkRecords,
    TimeSpan WorkTimeTotal,
    TimeSpan RestTimeTotal,
    TimeSpan OvertimeTotal
)
{
    public static WorkRecordTallyResponseDto FromDomain(WorkRecordTally tally, AppConfig appConfig)
        => new(
            tally.RecordedDate,
            [.. tally.WorkRecords.Select(WorkRecordItemResponseDto.FromDomain)],
            tally.GetWorkTimeTotal(),
            tally.GetRestTimeTotal(),
            tally.GetOvertimeTotal(appConfig)
        );

    public static WorkRecordTallyResponseDto Empty => FromDomain(WorkRecordTally.Empty, AppConfig.Default);
}
