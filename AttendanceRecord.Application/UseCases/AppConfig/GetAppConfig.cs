using AttendanceRecord.Application.Dtos.Responses;
using AttendanceRecord.Domain.Services;
using Mediator.Switch;

namespace AttendanceRecord.Application.UseCases.AppConfig;

public sealed record GetAppConfig : IRequest<AppConfigResponseDto>;

public sealed class GetAppConfigHandler(AppConfigStore appConfigStore)
    : IRequestHandler<GetAppConfig, AppConfigResponseDto>
{
    public Task<AppConfigResponseDto> Handle(GetAppConfig request, CancellationToken ct)
        => Task.FromResult(AppConfigResponseDto.FromDomain(appConfigStore.Config));
}
