using AttendanceRecord.Application.Dtos.Requests;
using AttendanceRecord.Application.Dtos.Responses;
using AttendanceRecord.Domain.Interfaces;
using AttendanceRecord.Domain.Services;
using AttendanceRecord.Domain.ValueObjects;
using Mediator.Switch;

namespace AttendanceRecord.Application.UseCases.WorkRecords;

public sealed record GetWorkRecordTally(WorkRecordTallyGetRequestDto Request)
    : IRequest<WorkRecordTallyResponseDto>;

public sealed class GetWorkRecordTallyHandler(
    IWorkRecordRepository workRecordRepository, AppConfigStore appConfigStore)
    : IRequestHandler<GetWorkRecordTally, WorkRecordTallyResponseDto>
{
    public async Task<WorkRecordTallyResponseDto> Handle(
        GetWorkRecordTally request, CancellationToken cancellationToken)
    {
        var yearAndMonth = new YearAndMonth(request.Request.Year, request.Request.Month);
        var records = await workRecordRepository.FindByMonthAsync(yearAndMonth);
        return WorkRecordTallyResponseDto.FromDomain(new(records), appConfigStore.Config);
    }
}
