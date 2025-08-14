namespace BrowserBridge;

public record ViewModelErrorEvent(ViewModelErrorEvent.ViewModelError Payload)
    : EventMessage<ViewModelErrorEvent.ViewModelError>(Payload)
{
    public override string Event => "Error";

    public record ViewModelError(string Message, string Details);
}
