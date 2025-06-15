using System.Reflection;

namespace AttendanceRecord.Infrastructure.Services;

public class AppDataDirectoryService
{
    public string DirectoryPath { get; }

    public AppDataDirectoryService()
    {
        var assembly = Assembly.GetEntryAssembly()
            ?? throw new InvalidOperationException("Entry assembly is not available.");

        var appName = assembly.GetName().Name ?? "AppData";
        var baseDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        DirectoryPath = Path.Combine(baseDir, appName);
        Directory.CreateDirectory(DirectoryPath);
    }

    public string GetFilePath(string fileName)
    {
        if (string.IsNullOrWhiteSpace(fileName))
            throw new ArgumentException("File name cannot be null or empty.", nameof(fileName));

        return Path.Combine(DirectoryPath, fileName);
    }
}
