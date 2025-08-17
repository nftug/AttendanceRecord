using AttendanceRecord.Domain.Config;
using AttendanceRecord.Domain.Entities;
using AttendanceRecord.Domain.ValueObjects;

namespace AttendanceRecord.Application.Dtos.Responses;

public record WorkRecordResponseDto(
    Guid Id,
    DateTime RecordedDate,
    TimeDurationResponseDto Duration,
    RestRecordResponseDto[] RestRecords,
    TimeSpan WorkTime,
    TimeSpan RestTime,
    TimeSpan Overtime,
    bool IsTodaysOngoing,
    bool IsWorking,
    bool IsResting
)
{
    public static WorkRecordResponseDto FromDomain(WorkRecord workRecord, AppConfig appConfig)
        => new(
            workRecord.Id,
            workRecord.RecordedDate,
            TimeDurationResponseDto.FromDomain(workRecord.Duration),
            [.. workRecord.RestRecords.Select(RestRecordResponseDto.FromDomain)],
            workRecord.GetWorkTime(),
            workRecord.GetRestTime(),
            workRecord.GetOvertime(appConfig),
            workRecord.IsTodaysOngoing,
            workRecord.IsWorking,
            workRecord.IsResting
        );

    public static WorkRecordResponseDto Empty => FromDomain(WorkRecord.Empty, AppConfig.Default);
}

public record RestRecordResponseDto(
    Guid Id,
    DateTime RecordedDate,
    TimeDurationResponseDto Duration,
    bool IsActive
)
{
    public static RestRecordResponseDto FromDomain(RestRecord restRecord)
        => new(
            restRecord.Id,
            restRecord.RecordedDate,
            TimeDurationResponseDto.FromDomain(restRecord.Duration),
            restRecord.IsActive
        );
}

public record TimeDurationResponseDto(DateTime StartedOn, DateTime? FinishedOn)
{
    public static TimeDurationResponseDto FromDomain(TimeDuration duration)
        => new(duration.StartedOn, duration.FinishedOn);
}
