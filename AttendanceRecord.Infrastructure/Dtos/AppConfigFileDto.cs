using AttendanceRecord.Domain.Config;

namespace AttendanceRecord.Infrastructure.Dtos;

public record AppConfigFileDto(
    int StandardWorkMinutes,
    bool ResidentNotificationEnabled,
    AppConfigFileDto.WorkRecordAlarmConfigFileDto WorkRecordAlarm,
    AppConfigFileDto.RestRecordAlarmConfigFileDto RestRecordAlarm,
    AppConfigFileDto.StatusFormatConfigFileDto StatusFormat
)
{
    public static AppConfigFileDto FromDomain(AppConfig config)
        => new(
            config.StandardWorkMinutes,
            config.ResidentNotificationEnabled,
            new(
                config.WorkRecordAlarm.IsEnabled,
                config.WorkRecordAlarm.RemainingMinutes,
                config.WorkRecordAlarm.SnoozeMinutes
            ),
            new(
                config.RestRecordAlarm.IsEnabled,
                config.RestRecordAlarm.ElapsedMinutes,
                config.RestRecordAlarm.SnoozeMinutes
            ),
            new(
                config.StatusFormat.StatusFormat,
                config.StatusFormat.TimeSpanFormat
            )
        );

    public AppConfig ToDomain() =>
        new(
            StandardWorkMinutes,
            ResidentNotificationEnabled,
            new(
                WorkRecordAlarm.IsEnabled,
                WorkRecordAlarm.RemainingMinutes,
                WorkRecordAlarm.SnoozeMinutes
            ),
            new(
                RestRecordAlarm.IsEnabled,
                RestRecordAlarm.ElapsedMinutes,
                RestRecordAlarm.SnoozeMinutes
            ),
            new(StatusFormat.StatusFormat, StatusFormat.TimeSpanFormat)
        );

    public record WorkRecordAlarmConfigFileDto(
        bool IsEnabled,
        int RemainingMinutes,
        int SnoozeMinutes
    );

    public record RestRecordAlarmConfigFileDto(
        bool IsEnabled,
        int ElapsedMinutes,
        int SnoozeMinutes
    );

    public record StatusFormatConfigFileDto(string StatusFormat, string TimeSpanFormat);
}