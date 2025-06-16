namespace BrowserBridge;

public record ViewModelErrorEvent(ViewModelErrorEvent.ViewModelError Payload)
    : EventMessage<ViewModelErrorEvent.ViewModelError>("error", Payload)
{
    public record ViewModelError(string Message, string Details);
}
