using System.Text.Json;
using System.Text.Json.Serialization;
using AttendanceRecord.Domain.Config;
using AttendanceRecord.Domain.Interfaces;
using AttendanceRecord.Infrastructure.Constants;
using AttendanceRecord.Infrastructure.Dtos;
using AttendanceRecord.Infrastructure.Services;

namespace AttendanceRecord.Infrastructure.Repositories;

public class AppConfigRepository(AppDataDirectoryService appDataDirectory) : IAppConfigRepository
{
    private readonly string _filePath = appDataDirectory.GetFilePath("config.json");

    private readonly JsonContext _jsonContext = new(new()
    {
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    });

    public async Task<AppConfig> LoadAsync()
    {
        if (!File.Exists(_filePath)) return AppConfig.Default;

        using var stream = File.OpenRead(_filePath);
        var dto = await JsonSerializer.DeserializeAsync(stream, _jsonContext.AppConfigFileDto);
        return dto?.ToDomain() ?? AppConfig.Default;
    }

    public async Task SaveAsync(AppConfig appConfig)
    {
        var dto = AppConfigFileDto.FromDomain(appConfig);
        using var stream = File.Create(_filePath);
        await JsonSerializer.SerializeAsync(stream, dto, _jsonContext.AppConfigFileDto);
        await stream.FlushAsync();
    }
}
