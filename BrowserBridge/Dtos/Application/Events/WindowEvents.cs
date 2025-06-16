namespace BrowserBridge;

public record MessageBoxResultEvent : EventMessage<MessageBoxResultType>
{
    public MessageBoxResultEvent(MessageBoxResultType payload, Guid commandId)
        : base(payload, commandId, "messageBox") { }
}
