using AttendanceRecord.Application.Dtos.Responses;
using BrowserBridge;

namespace AttendanceRecord.Presentation.Dtos.AppConfig;

public record GetAppConfigResultEvent(AppConfigResponseDto Payload, Guid? CommandId)
    : CommandResultEventMessage<AppConfigResponseDto>(Payload, CommandId)
{
    public override string CommandName => nameof(AppConfigCommandType.GetAppConfig);
}

public record SaveAppConfigResultEvent(Guid? CommandId) : CommandResultEventVoidMessage(CommandId)
{
    public override string CommandName => nameof(AppConfigCommandType.SaveAppConfig);
}
