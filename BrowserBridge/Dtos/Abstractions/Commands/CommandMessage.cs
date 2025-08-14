using System.Text.Json;

namespace BrowserBridge;

public record CommandMessage(Guid ViewId, Guid? CommandId, string Command, JsonElement Payload);
