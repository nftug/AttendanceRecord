namespace BrowserBridge;

public record ViewModelInitResultEvent(InitCommandPayload Payload, Guid? CommandId)
    : CommandResultEventMessage<InitCommandPayload>(Payload, CommandId)
{
    public override string CommandName => nameof(AppActionType.Init);
}

public record ViewModelErrorEvent(ViewModelError Payload)
    : EventMessage<ViewModelError>(Payload)
{
    public override string Event => "Error";
}

public record ViewModelError(Guid? CommandId, string Message, string Details);