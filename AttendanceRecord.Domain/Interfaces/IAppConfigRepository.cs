using AttendanceRecord.Domain.Config;

namespace AttendanceRecord.Domain.Interfaces;

public interface IAppConfigRepository
{
    Task<AppConfig> LoadAsync();
    Task SaveAsync(AppConfig appConfig);
}
