using System.Text.Json.Serialization;

namespace BrowserBridge;

#region Message
[JsonSerializable(typeof(ViewModelErrorEvent))]
[JsonSerializable(typeof(CommandMessage))]
[JsonSerializable(typeof(InitCommandPayload))]
#endregion

#region
[JsonSerializable(typeof(MessageBoxResultEvent))]
[JsonSerializable(typeof(MessageBoxCommandPayload))]
#endregion

[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
public partial class BridgeJsonContext : JsonSerializerContext;
