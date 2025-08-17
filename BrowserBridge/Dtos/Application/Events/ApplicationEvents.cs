namespace BrowserBridge;

public record ViewModelInitResultEvent(InitCommandPayload Payload, Guid? CommandId)
    : CommandResultEventMessage<InitCommandPayload>(Payload, CommandId)
{
    public override string CommandName => nameof(AppActionType.Init);
}

public record ViewModelErrorEvent(ViewModelErrorEvent.ViewModelError Payload)
    : EventMessage<ViewModelErrorEvent.ViewModelError>(Payload)
{
    public override string Event => "Error";

    public record ViewModelError(string Message, string Details);
}
