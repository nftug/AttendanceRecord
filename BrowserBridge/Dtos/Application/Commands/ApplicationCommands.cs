using System.Text.Json.Serialization;

namespace BrowserBridge;

[JsonConverter(typeof(JsonStringEnumConverter<AppActionType>))]
public enum AppActionType
{
    Init,
    Leave
}

public record InitCommandPayload(string Type);
