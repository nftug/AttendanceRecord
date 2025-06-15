using AttendanceRecord.Domain.Entities;
using AttendanceRecord.Domain.ValueObjects;

namespace AttendanceRecord.Infrastructure.Dtos;

public record WorkRecordFileDto(Guid Id, TimeDurationFileDto Duration, RestRecordFileDto[] RestRecords)
{
    public static WorkRecordFileDto FromDomain(WorkRecord workRecord)
        => new(
            workRecord.Id,
            TimeDurationFileDto.FromDomain(workRecord.Duration),
            [.. workRecord.RestRecords.Select(RestRecordFileDto.FromDomain)]
        );

    public WorkRecord ToDomain() =>
        new(Id, Duration.ToDomain(), RestRecords.Select(r => r.ToDomain()));
}

public record RestRecordFileDto(Guid Id, TimeDurationFileDto Duration)
{
    public static RestRecordFileDto FromDomain(RestRecord restRecord)
        => new(restRecord.Id, TimeDurationFileDto.FromDomain(restRecord.Duration));

    public RestRecord ToDomain() => new(Id, Duration.ToDomain());
}

public record TimeDurationFileDto(DateTime StartedOn, DateTime? FinishedOn)
{
    public static TimeDurationFileDto FromDomain(TimeDuration duration)
        => new(duration.StartedOn, duration.FinishedOn);

    public TimeDuration ToDomain() => TimeDuration.Reconstruct(StartedOn, FinishedOn);
}
