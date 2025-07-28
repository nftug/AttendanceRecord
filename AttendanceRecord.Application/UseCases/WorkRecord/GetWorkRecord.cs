using AttendanceRecord.Application.Dtos.Responses;
using AttendanceRecord.Domain.Services;
using Mediator.Switch;

namespace AttendanceRecord.Application.UseCases.WorkRecord;

[RequestHandler(typeof(GetWorkRecordHandler))]
public sealed record GetWorkRecord(Guid Id) : IRequest<WorkRecordResponseDto>;

public sealed class GetWorkRecordHandler(WorkRecordFactory workRecordFactory)
    : IRequestHandler<GetWorkRecord, WorkRecordResponseDto>
{
    public async Task<WorkRecordResponseDto> Handle(
        GetWorkRecord request, CancellationToken cancellationToken)
    {
        var workRecord = await workRecordFactory.FindByIdAsync(request.Id)
            ?? throw new KeyNotFoundException($"WorkRecordが見つかりません: {request.Id}");
        return WorkRecordResponseDto.FromDomain(workRecord);
    }
}
