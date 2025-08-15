using AttendanceRecord.Application.Dtos.Responses;
using AttendanceRecord.Domain.Interfaces;
using AttendanceRecord.Domain.Services;
using Mediator.Switch;

namespace AttendanceRecord.Application.UseCases.WorkRecords;

public sealed record GetWorkRecord(Guid Id) : IRequest<WorkRecordResponseDto>;

public sealed class GetWorkRecordHandler(
    IWorkRecordRepository repository, AppConfigStore appConfigStore)
    : IRequestHandler<GetWorkRecord, WorkRecordResponseDto>
{
    public async Task<WorkRecordResponseDto> Handle(
        GetWorkRecord request, CancellationToken cancellationToken)
    {
        var workRecord = await repository.FindByIdAsync(request.Id)
            ?? throw new KeyNotFoundException($"WorkRecordが見つかりません: {request.Id}");
        return WorkRecordResponseDto.FromDomain(workRecord, appConfigStore.Config);
    }
}
