namespace BrowserBridge;

public class ViewModelException(Guid viewId, string? message, Exception? innerException = null)
    : Exception(message, innerException)
{
    public Guid ViewId { get; } = viewId;
}
