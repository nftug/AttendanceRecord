using System.Text.Json.Serialization;

namespace BrowserBridge;

#region Message
[JsonSerializable(typeof(EventMessage<DummyEventPayload>))]
[JsonSerializable(typeof(EventMessage<ViewModelErrorEvent.ViewModelError>))]
[JsonSerializable(typeof(CommandMessage))]
[JsonSerializable(typeof(InitCommandPayload))]
#endregion

#region
[JsonSerializable(typeof(EventMessage<MessageBoxResultType>))]
[JsonSerializable(typeof(MessageBoxCommandPayload))]
#endregion

[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
public partial class BridgeJsonContext : JsonSerializerContext;
