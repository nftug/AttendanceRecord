using AttendanceRecord.Domain.Config;

namespace AttendanceRecord.Domain.Interfaces;

public interface IAppConfigRepository
{
    ValueTask<AppConfig> LoadAsync();
    ValueTask SaveAsync(AppConfig appConfig);
}
