using Microsoft.Extensions.Logging;

namespace BrowserBridge;

public interface ILogWriter
{
    void Write(string message);
    void WriteException(Exception exception);
}

public class ConsoleLogWriter : ILogWriter
{
    public void Write(string message) => Console.WriteLine(message);
    public void WriteException(Exception exception) => Console.WriteLine(exception);
}

public class FileLogWriter(string filePath) : ILogWriter
{
    public void Write(string message)
    {
        File.AppendAllText(filePath, message + Environment.NewLine);
    }

    public void WriteException(Exception exception)
    {
        File.AppendAllText(filePath, exception.ToString() + Environment.NewLine);
    }
}

public class MinimalLogger<T>(ILogWriter logWriter) : ILogger<T>
{
    private readonly string _categoryName = typeof(T).FullName ?? "Unknown";

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull
        => NullScope.Instance;

    public bool IsEnabled(LogLevel logLevel) => logLevel >= LogLevel.Information;

    public void Log<TState>(
        LogLevel logLevel,
        EventId eventId,
        TState state,
        Exception? exception,
        Func<TState, Exception?, string> formatter)
    {
        if (!IsEnabled(logLevel)) return;

        var message = formatter(state, exception);
        var time = DateTime.Now.ToString("HH:mm:ss");
        var level = logLevel.ToString();

        logWriter.Write($"[{time}] [{level}]: {_categoryName}\n\t{message}");

        if (exception != null) logWriter.WriteException(exception);
    }

    private class NullScope : IDisposable
    {
        public static NullScope Instance { get; } = new();
        public void Dispose() { }
    }
}
