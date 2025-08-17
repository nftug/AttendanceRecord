namespace BrowserBridge;

public class ViewModelException(Guid viewId, Guid? commandId, string? message, Exception? innerException = null)
    : Exception(message, innerException)
{
    public Guid ViewId { get; } = viewId;
    public Guid? CommandId { get; } = commandId;
}
