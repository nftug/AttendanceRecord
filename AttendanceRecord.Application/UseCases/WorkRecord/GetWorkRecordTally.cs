using AttendanceRecord.Application.Dtos.Requests;
using AttendanceRecord.Application.Dtos.Responses;
using AttendanceRecord.Domain.Services;
using Mediator.Switch;

namespace AttendanceRecord.Application.UseCases.WorkRecord;

public sealed record GetWorkRecordTally(WorkRecordTallyGetRequestDto Request) : IRequest<WorkRecordTallyResponseDto>;

public sealed class GetWorkRecordTallyHandler(WorkRecordFactory workRecordFactory)
    : IRequestHandler<GetWorkRecordTally, WorkRecordTallyResponseDto>
{
    public async Task<WorkRecordTallyResponseDto> Handle(
        GetWorkRecordTally request, CancellationToken cancellationToken)
    {
        var workRecord = await workRecordFactory.GetMonthlyTallyAsync(request.Request.Year, request.Request.Month);
        return WorkRecordTallyResponseDto.FromDomain(workRecord);
    }
}
