using AttendanceRecord.Application.Dtos.Responses;
using AttendanceRecord.Domain.Services;

namespace AttendanceRecord.Application.UseCases.WorkRecord;

public class GetWorkRecordTallyUseCase(WorkRecordFactory workRecordFactory)
{
    public async Task<WorkRecordTallyResponseDto> ExecuteAsync(DateTime monthDate)
    {
        var workRecord = await workRecordFactory.GetMonthlyTallyAsync(monthDate);
        return WorkRecordTallyResponseDto.FromDomain(workRecord);
    }
}
