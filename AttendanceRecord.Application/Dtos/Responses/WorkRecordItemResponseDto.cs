namespace AttendanceRecord.Application.Dtos.Responses;

public record WorkRecordItemResponseDto(Guid Id, DateTime Date)
{
    public static WorkRecordItemResponseDto FromDomain(Domain.Entities.WorkRecord record)
        => new(record.Id, record.RecordedDate);

    public static WorkRecordItemResponseDto FromDomain((Guid Id, DateTime Date) record)
        => new(record.Id, record.Date);
}
