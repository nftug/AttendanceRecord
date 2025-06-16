using System.Text.Json.Serialization.Metadata;

namespace BrowserBridge;

public interface IEventDispatcher
{
    void Dispatch<TPayload>(
        EventMessage<TPayload> message,
        JsonTypeInfo<EventMessage<TPayload>> jsonTypeInfo
    );

    void Dispatch<TPayload>(EventMessage<TPayload> message);
}
