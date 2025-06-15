using AttendanceRecord.Application.Dtos.Responses;
using AttendanceRecord.Domain.Entities;
using AttendanceRecord.Domain.ValueObjects;

namespace AttendanceRecord.Application.Dtos.Requests;

public record WorkRecordSaveRequestDto(
    Guid? Id, TimeDurationRequestDto Duration, RestRecordSaveRequestDto[] RestRecords);

public record RestRecordSaveRequestDto(Guid? RestRecordId, TimeDurationRequestDto Duration)
{
    public RestRecord ToDomain() => new(RestRecordId ?? Guid.NewGuid(), Duration.ToDomain());
}

public record TimeDurationRequestDto(DateTime StartedOn, DateTime? FinishedOn)
{
    public TimeDuration ToDomain() => TimeDuration.Create(StartedOn, FinishedOn);
}
