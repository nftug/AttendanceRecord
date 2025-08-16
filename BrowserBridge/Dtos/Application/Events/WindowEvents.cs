namespace BrowserBridge;

public record MessageBoxResultEvent(MessageBoxResultType Payload, Guid CommandId)
    : CommandResultEventMessage<MessageBoxResultType>(Payload, CommandId)
{
    public override string CommandName => nameof(WindowCommandType.MessageBox);
}

public record SetMinimizedResultEvent(Guid? CommandId)
    : CommandResultEventVoidMessage(CommandId)
{
    public override string CommandName => nameof(WindowCommandType.SetMinimized);
}
