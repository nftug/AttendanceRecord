using AttendanceRecord.Application.Dtos.Responses;
using AttendanceRecord.Domain.Services;
using Mediator.Switch;

namespace AttendanceRecord.Application.UseCases.WorkRecord;

[RequestHandler(typeof(GetWorkRecordTallyHandler))]
public sealed record GetWorkRecordTally(DateTime MonthDate) : IRequest<WorkRecordTallyResponseDto>;

public sealed class GetWorkRecordTallyHandler(WorkRecordFactory workRecordFactory)
    : IRequestHandler<GetWorkRecordTally, WorkRecordTallyResponseDto>
{
    public async Task<WorkRecordTallyResponseDto> Handle(
        GetWorkRecordTally request, CancellationToken cancellationToken)
    {
        var workRecord = await workRecordFactory.GetMonthlyTallyAsync(request.MonthDate);
        return WorkRecordTallyResponseDto.FromDomain(workRecord);
    }
}
