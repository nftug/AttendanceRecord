using AttendanceRecord.Application.Dtos.Requests;
using AttendanceRecord.Domain.Services;
using Mediator.Switch;

namespace AttendanceRecord.Application.UseCases.AppConfig;

public sealed record SaveAppConfig(AppConfigSaveRequestDto Request) : IRequest<Unit>;

public sealed class SaveAppConfigHandler(AppConfigStore appConfigStore)
    : IRequestHandler<SaveAppConfig, Unit>
{
    public async Task<Unit> Handle(SaveAppConfig request, CancellationToken ct)
    {
        var config = request.Request.ToDomain();
        await appConfigStore.SaveAsync(config);
        return Unit.Value;
    }
}
