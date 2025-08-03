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
                config.WorkTimeAlarm.IsEnabled,
                config.WorkTimeAlarm.RemainingMinutes,
                config.WorkTimeAlarm.SnoozeMinutes
            ),
            new(
                config.RestTimeAlarm.IsEnabled,
                config.RestTimeAlarm.ElapsedMinutes,
                config.RestTimeAlarm.SnoozeMinutes
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