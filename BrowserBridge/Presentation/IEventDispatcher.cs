using System.Text.Json.Serialization.Metadata;

namespace BrowserBridge;

public interface IEventDispatcher
{
    void Dispatch<TEventMessage>(TEventMessage message, JsonTypeInfo<TEventMessage> jsonTypeInfo)
        where TEventMessage : EventMessageBase;

    void Dispatch<TEventMessage>(TEventMessage message)
        where TEventMessage : EventMessageBase;
}
