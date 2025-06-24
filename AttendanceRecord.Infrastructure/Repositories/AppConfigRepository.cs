using System.Text.Json;
using AttendanceRecord.Domain.Config;
using AttendanceRecord.Domain.Interfaces;
using AttendanceRecord.Infrastructure.Constants;
using AttendanceRecord.Infrastructure.Dtos;
using AttendanceRecord.Infrastructure.Services;

namespace AttendanceRecord.Infrastructure.Repositories;

public class AppConfigRepository : IAppConfigRepository, IDisposable
{
    private readonly string _filePath;
    private AppConfig _appConfig;
    private readonly FileStream _lockStream;
    private readonly Lock _syncRoot = new();

    public AppConfigRepository(AppDataDirectoryService appDataDirectory)
    {
        _filePath = appDataDirectory.GetFilePath("config.json");
        _lockStream = new FileStream(_filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
        _appConfig = LoadFile(_lockStream);
    }

    private static AppConfig LoadFile(FileStream stream)
    {
        if (stream.Length == 0) return AppConfig.Default;
        stream.Position = 0;
        var dto = JsonSerializer.Deserialize(stream, JsonContext.Default.AppConfigFileDto);
        return dto?.ToDomain() ?? AppConfig.Default;
    }

    public void Dispose() => _lockStream?.Dispose();

    public ValueTask<AppConfig> LoadAsync() => new(_appConfig);

    public ValueTask SaveAsync(AppConfig appConfig)
    {
        lock (_syncRoot)
        {
            _appConfig = appConfig;
            _lockStream.SetLength(0);
            _lockStream.Position = 0;
            var dto = AppConfigFileDto.FromDomain(appConfig);
            JsonSerializer.Serialize(_lockStream, dto, JsonContext.Default.AppConfigFileDto);
            _lockStream.Flush();
        }
        return ValueTask.CompletedTask;
    }
}
