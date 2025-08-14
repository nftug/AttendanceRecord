using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

namespace BrowserBridge.Photino;

public class EventDispatcher(PhotinoWindowInstance _window) : IEventDispatcher
{
    public void Dispatch<TEventMessage>(TEventMessage message, JsonTypeInfo<TEventMessage> jsonTypeInfo)
        where TEventMessage : EventMessageBase
    {
        if (_window.Value is not { } window) return;
        if (message.ViewId == default) return;

        window.SendWebMessage(JsonSerializer.Serialize(message, jsonTypeInfo));
    }

    public void Dispatch<TEventMessage>(TEventMessage message)
        where TEventMessage : EventMessageBase
    {
        if (_window.Value is not { } window) return;
        if (message.ViewId == default) return;

        window.SendWebMessage(JsonSerializer.Serialize(message));
    }
}
