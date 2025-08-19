using System.Text.Json.Serialization;

namespace BrowserBridge;

#region Message
[JsonSerializable(typeof(ViewModelInitResultEvent))]
[JsonSerializable(typeof(ViewModelErrorEvent))]
[JsonSerializable(typeof(CommandMessage))]
[JsonSerializable(typeof(InitCommandPayload))]
#endregion

#region
[JsonSerializable(typeof(MessageBoxResultEvent))]
[JsonSerializable(typeof(MessageBoxCommandPayload))]
[JsonSerializable(typeof(SetMinimizedResultEvent))]
[JsonSerializable(typeof(SendNotificationResultEvent))]
[JsonSerializable(typeof(SendNotificationCommandPayload))]
#endregion

[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
public partial class BridgeJsonContext : JsonSerializerContext;
