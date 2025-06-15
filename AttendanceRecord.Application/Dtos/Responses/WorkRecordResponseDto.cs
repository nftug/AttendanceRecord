using AttendanceRecord.Domain.ValueObjects;

namespace AttendanceRecord.Application.Dtos.Responses;

public record WorkRecordResponseDto(
    Guid Id,
    DateTime RecordedDate,
    TimeDurationResponseDto Duration,
    RestRecordResponseDto[] RestRecords,
    TimeSpan TotalRestTime,
    TimeSpan TotalWorkTime,
    TimeSpan Overtime,
    bool IsTodays,
    bool IsTodaysOngoing,
    bool IsResting,
    bool IsWorking
)
{
    public static WorkRecordResponseDto FromDomain(Domain.Entities.WorkRecord workRecord)
        => new(
            workRecord.Id,
            workRecord.RecordedDate,
            TimeDurationResponseDto.FromDomain(workRecord.Duration),
            [.. workRecord.RestRecords.Select(RestRecordResponseDto.FromDomain)],
            workRecord.TotalRestTime,
            workRecord.TotalWorkTime,
            workRecord.Overtime,
            workRecord.IsTodays,
            workRecord.IsTodaysOngoing,
            workRecord.IsResting,
            workRecord.IsWorking
        );
}

public record RestRecordResponseDto(
    Guid Id,
    DateTime RecordedDate,
    TimeDurationResponseDto Duration,
    bool IsActive
)
{
    public static RestRecordResponseDto FromDomain(Domain.Entities.RestRecord restRecord)
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

    public TimeDuration ToDomain() => TimeDuration.Reconstruct(StartedOn, FinishedOn);
}
