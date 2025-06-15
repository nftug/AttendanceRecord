using AttendanceRecord.Domain.Config;

namespace AttendanceRecord.Domain.Interfaces;

public interface IAppConfigRepository
{
    AppConfig Config { get; }
    ValueTask LoadAsync();
    ValueTask SaveAsync(AppConfig appConfig);
}
