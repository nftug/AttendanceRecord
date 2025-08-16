using System.IO.Pipes;
using System.Text.Json;
using AttendanceRecord.Presentation.Constants;

namespace AttendanceRecord.Presentation.Utils;

public abstract class NamedPipeBase
{
    public static readonly string PipeName = "attendance-record";
}

public class NamedPipeServer : NamedPipeBase
{
    public static async Task ReceiveMessageAsync(Action<NamedPipeMessage?> action)
    {
        while (true)
        {
            using var stream = new NamedPipeServerStream(PipeName);
            await stream.WaitForConnectionAsync();
            using var reader = new StreamReader(stream);
            var json = await reader.ReadToEndAsync();
            action(NamedPipeMessage.Deserialize(json));
        }
    }
}

public class NamedPipeClient : NamedPipeBase
{
    public static async Task<bool> SendMessageAsync(string content)
    {
        try
        {
            using var stream = new NamedPipeClientStream(PipeName);
            await stream.ConnectAsync(3000);

            using var writer = new StreamWriter(stream);

            var message = new NamedPipeMessage(Environment.ProcessId.ToString(), content);
            var json = message.Serialize();
            await writer.WriteAsync(json);

            return true;
        }
        catch (TimeoutException)
        {
            Console.WriteLine("Failed to send message: Timeout");
            return false;
        }
    }
}

public record NamedPipeMessage(string Sender, string Content)
{
    public string Serialize() => JsonSerializer.Serialize(this, AppJsonContext.Default.NamedPipeMessage);

    public static NamedPipeMessage? Deserialize(string? serialized)
    {
        try
        {
            if (serialized is not { Length: > 0 })
                return null;
            return JsonSerializer.Deserialize<NamedPipeMessage>(serialized, AppJsonContext.Default.NamedPipeMessage);
        }
        catch (JsonException e)
        {
            System.Diagnostics.Debug.WriteLine($"Failed to deserialize NamedPipeMessage: {e.Message}");
        }
        return null;
    }
}
