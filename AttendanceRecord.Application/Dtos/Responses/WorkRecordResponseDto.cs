using AttendanceRecord.Domain.ValueObjects;

namespace AttendanceRecord.Application.Dtos.Responses;

public record WorkRecordResponseDto(
    Guid Id,
    DateTime RecordedDate,
    TimeDurationResponseDto Duration,
    RestRecordResponseDto[] RestRecords,
    TimeSpan TotalWorkTime,
    TimeSpan TotalRestTime,
    TimeSpan Overtime,
    bool IsTodaysOngoing,
    bool IsWorking,
    bool IsResting
)
{
    public static WorkRecordResponseDto FromDomain(Domain.Entities.WorkRecord workRecord)
        => new(
            workRecord.Id,
            workRecord.RecordedDate,
            TimeDurationResponseDto.FromDomain(workRecord.Duration),
            [.. workRecord.RestRecords.Select(RestRecordResponseDto.FromDomain)],
            workRecord.TotalWorkTime,
            workRecord.TotalRestTime,
            workRecord.Overtime,
            workRecord.IsTodaysOngoing,
            workRecord.IsWorking,
            workRecord.IsResting
        );

    public static WorkRecordResponseDto Empty => FromDomain(Domain.Entities.WorkRecord.Empty);

    public bool IsEmpty => Id == Guid.Empty;
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
}
