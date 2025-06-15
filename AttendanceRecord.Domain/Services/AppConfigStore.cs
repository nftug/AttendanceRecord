using AttendanceRecord.Domain.Config;
using AttendanceRecord.Domain.Interfaces;

namespace AttendanceRecord.Domain.Services;

public class AppConfigStore(IAppConfigRepository appConfigRepository)
{
    public AppConfig Config { get; private set; } = AppConfig.Default;

    public async Task LoadAsync()
    {
        Config = await appConfigRepository.LoadAsync();
        if (Config.StandardWorkMinutes <= 0)
        {
            Config = Config with { StandardWorkMinutes = AppConfig.Default.StandardWorkMinutes };
        }
    }

    public async Task SaveAsync(AppConfig appConfig)
    {
        Config = appConfig;
        await appConfigRepository.SaveAsync(appConfig);
    }
}
