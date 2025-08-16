using AttendanceRecord.Application.Dtos.Requests;
using AttendanceRecord.Application.Dtos.Responses;
using AttendanceRecord.Domain.Services;
using Mediator.Switch;

namespace AttendanceRecord.Application.UseCases.WorkRecords;

public sealed record GetWorkRecordTally(WorkRecordTallyGetRequestDto Request)
    : IRequest<WorkRecordTallyResponseDto>;

public sealed class GetWorkRecordTallyHandler(WorkRecordTallyFactory workRecordTallyFactory)
    : IRequestHandler<GetWorkRecordTally, WorkRecordTallyResponseDto>
{
    public async Task<WorkRecordTallyResponseDto> Handle(
        GetWorkRecordTally request, CancellationToken cancellationToken)
    {
        var workRecordTally = await workRecordTallyFactory.GetMonthlyAsync(request.Request.Year, request.Request.Month);
        return WorkRecordTallyResponseDto.FromDomain(workRecordTally);
    }
}
