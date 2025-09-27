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
            var message = await JsonSerializer.DeserializeAsync(stream, AppJsonContext.Default.NamedPipeMessage);
            action(message);
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
            var message = new NamedPipeMessage(Environment.ProcessId.ToString(), content);
            await JsonSerializer.SerializeAsync(stream, message, AppJsonContext.Default.NamedPipeMessage);
            return true;
        }
        catch (TimeoutException)
        {
            Console.WriteLine("Failed to send message: Timeout");
            return false;
        }
    }
}

public record NamedPipeMessage(string Sender, string Content);
