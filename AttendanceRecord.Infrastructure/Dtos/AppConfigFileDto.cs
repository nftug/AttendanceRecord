using AttendanceRecord.Domain.Config;

namespace AttendanceRecord.Infrastructure.Dtos;

public record AppConfigFileDto(
    int StandardWorkMinutes,
    bool ResidentNotificationEnabled,
    AppConfigFileDto.WorkTimeAlarmConfigFileDto WorkTimeAlarm,
    AppConfigFileDto.RestTimeAlarmConfigFileDto RestTimeAlarm,
    AppConfigFileDto.StatusFormatConfigFileDto StatusFormat
)
{
    public static AppConfigFileDto FromDomain(AppConfig config)
        => new(
            config.StandardWorkMinutes,
            config.ResidentNotificationEnabled,
            new(
                config.WorkEndAlarm.IsEnabled,
                config.WorkEndAlarm.RemainingMinutes,
                config.WorkEndAlarm.SnoozeMinutes
            ),
            new(
                config.RestStartAlarm.IsEnabled,
                config.RestStartAlarm.ElapsedMinutes,
                config.RestStartAlarm.SnoozeMinutes
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
                WorkTimeAlarm.IsEnabled,
                WorkTimeAlarm.RemainingMinutes,
                WorkTimeAlarm.SnoozeMinutes
            ),
            new(
                RestTimeAlarm.IsEnabled,
                RestTimeAlarm.ElapsedMinutes,
                RestTimeAlarm.SnoozeMinutes
            ),
            new(StatusFormat.StatusFormat, StatusFormat.TimeSpanFormat)
        );

    public record WorkTimeAlarmConfigFileDto(
        bool IsEnabled,
        int RemainingMinutes,
        int SnoozeMinutes
    );

    public record RestTimeAlarmConfigFileDto(
        bool IsEnabled,
        int ElapsedMinutes,
        int SnoozeMinutes
    );

    public record StatusFormatConfigFileDto(string StatusFormat, string TimeSpanFormat);
}