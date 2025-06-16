namespace BrowserBridge;

public class ViewModelException : Exception
{
    public Guid ViewId { get; }

    public ViewModelException(Guid viewId, string? message, Exception? innerException = null)
        : base(message, innerException)
    {
        ViewId = viewId;
    }
}
