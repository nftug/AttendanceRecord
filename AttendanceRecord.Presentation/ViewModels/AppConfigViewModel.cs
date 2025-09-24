using AttendanceRecord.Application.Dtos.Requests;
using AttendanceRecord.Application.UseCases.AppConfig;
using AttendanceRecord.Presentation.Constants;
using AttendanceRecord.Presentation.Dtos.AppConfig;
using BrowserBridge;
using Mediator.Switch;

namespace AttendanceRecord.Presentation.ViewModels;

public sealed class AppConfigViewModel(IEventDispatcher eventDispatcher, ISender mediator)
    : ViewModelBase<AppConfigCommandType>(eventDispatcher)
{
    protected override ValueTask HandleActionAsync(AppConfigCommandType action, CommandMessage message)
        => (action, message.Payload, message.CommandId) switch
        {
            (AppConfigCommandType.GetAppConfig, _, var commandId)
                => HandleGetAppConfigAsync(commandId),
            (AppConfigCommandType.SaveAppConfig, var payload, var commandId)
                when payload.TryParse(AppJsonContext.Default.AppConfigSaveRequestDto, out var request)
                    => HandleSaveAppConfigAsync(request, commandId),
            _ => throw new NotImplementedException($"Action {action} is not implemented or payload is missing."),
        };

    private async ValueTask HandleGetAppConfigAsync(Guid? commandId)
    {
        var response = await mediator.Send(new GetAppConfig());
        Dispatch(new(response, commandId), AppJsonContext.Default.GetAppConfigResultEvent);
    }

    private async ValueTask HandleSaveAppConfigAsync(AppConfigSaveRequestDto request, Guid? commandId)
    {
        await mediator.Send(new SaveAppConfig(request));
        Dispatch(new(commandId), AppJsonContext.Default.SaveAppConfigResultEvent);
    }
}
