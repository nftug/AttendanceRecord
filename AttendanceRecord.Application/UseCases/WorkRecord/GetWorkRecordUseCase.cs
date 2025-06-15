using AttendanceRecord.Application.Dtos.Responses;
using AttendanceRecord.Domain.Services;

namespace AttendanceRecord.Application.UseCases.WorkRecord;

public class GetWorkRecordUseCase(WorkRecordFactory workRecordFactory)
{
    public async Task<WorkRecordResponseDto> ExecuteAsync(Guid id)
    {
        var workRecord = await workRecordFactory.FindByIdAsync(id)
            ?? throw new KeyNotFoundException($"WorkRecordが見つかりません: {id}");
        return WorkRecordResponseDto.FromDomain(workRecord);
    }
}
