using AttendanceRecord.Domain.Config;
using AttendanceRecord.Domain.Interfaces;

namespace AttendanceRecord.Domain.Services;

public class AppConfigStore
{
    private readonly IAppConfigRepository _appConfigRepository;

    public AppConfig Config { get; private set; } = AppConfig.Default;

    public AppConfigStore(IAppConfigRepository appConfigRepository)
    {
        _appConfigRepository = appConfigRepository;
        Task.Run(LoadAsync).ConfigureAwait(false);
    }

    public async Task LoadAsync()
    {
        Config = await _appConfigRepository.LoadAsync();
    }

    public async Task SaveAsync(AppConfig appConfig)
    {
        Config = appConfig with { };
        await _appConfigRepository.SaveAsync(Config);
    }
}
