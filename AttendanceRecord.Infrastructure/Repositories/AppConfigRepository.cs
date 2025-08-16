using System.Text.Json;
using AttendanceRecord.Domain.Config;
using AttendanceRecord.Domain.Interfaces;
using AttendanceRecord.Infrastructure.Constants;
using AttendanceRecord.Infrastructure.Dtos;
using AttendanceRecord.Infrastructure.Services;

namespace AttendanceRecord.Infrastructure.Repositories;

public class AppConfigRepository(AppDataDirectoryService appDataDirectory) : IAppConfigRepository
{
    private readonly string _filePath = appDataDirectory.GetFilePath("config.json");

    public async ValueTask<AppConfig> LoadAsync()
    {
        try
        {
            using var stream = new FileStream(_filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            if (stream.Length == 0) return AppConfig.Default;
            var content = await JsonSerializer.DeserializeAsync(stream, InfrastructureJsonContext.Indented.AppConfigFileDto);
            return content?.ToDomain() ?? AppConfig.Default;
        }
        catch (FileNotFoundException)
        {
            return AppConfig.Default;
        }
    }

    public async ValueTask SaveAsync(AppConfig appConfig)
    {
        using var stream = new FileStream(_filePath, FileMode.Create, FileAccess.Write, FileShare.None);
        var dto = AppConfigFileDto.FromDomain(appConfig);
        await JsonSerializer.SerializeAsync(stream, dto, InfrastructureJsonContext.Indented.AppConfigFileDto);
        await stream.FlushAsync();
    }
}
