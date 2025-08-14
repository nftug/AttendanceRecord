namespace BrowserBridge;

public abstract record EventMessageBase
{
    public Guid ViewId { get; init; }
    public abstract string Event { get; }
}

public abstract record EventMessage<TPayload>(TPayload? Payload) : EventMessageBase;

public abstract record CommandResultEventMessage<TPayload>(TPayload? Payload, Guid CommandId)
    : EventMessage<TPayload>(Payload)
{
    public abstract string CommandName { get; }
    public override string Event => $"Receive:{CommandName}";
}

public abstract record DummyEventMessage : EventMessageBase;

public abstract record CommandResultDummyEventMessage(Guid CommandId) : EventMessageBase
{
    public abstract string CommandName { get; }
    public override string Event => $"Receive:{CommandName}";
}
