using AttendanceRecord.Application.Dtos.Requests;
using AttendanceRecord.Application.Dtos.Responses;
using AttendanceRecord.Domain.Interfaces;
using AttendanceRecord.Domain.Services;
using Mediator.Switch;

namespace AttendanceRecord.Application.UseCases.WorkRecords;

public sealed record GetWorkRecordTally(WorkRecordTallyGetRequestDto Request)
    : IRequest<WorkRecordTallyResponseDto>;

public sealed class GetWorkRecordTallyHandler(
    IWorkRecordRepository repository, AppConfigStore appConfigStore)
    : IRequestHandler<GetWorkRecordTally, WorkRecordTallyResponseDto>
{
    public async Task<WorkRecordTallyResponseDto> Handle(
        GetWorkRecordTally request, CancellationToken cancellationToken)
    {
        var monthlyItems = await repository.FindByMonthAsync(request.Request.Year, request.Request.Month);
        return WorkRecordTallyResponseDto.FromDomain(new(monthlyItems), appConfigStore.Config);
    }
}
