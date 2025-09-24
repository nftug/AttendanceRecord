namespace AttendanceRecord.Application.Dtos.Responses;

public record AppConfigResponseDto(
    int StandardWorkMinutes,
    bool ResidentNotificationEnabled,
    AppConfigResponseDto.WorkEndAlarmConfigResponse WorkEndAlarm,
    AppConfigResponseDto.RestStartAlarmConfigResponse RestStartAlarm,
    AppConfigResponseDto.StatusFormatConfigResponse StatusFormat)
{
    public record WorkEndAlarmConfigResponse(bool IsEnabled, int RemainingMinutes, int SnoozeMinutes);

    public record RestStartAlarmConfigResponse(bool IsEnabled, int ElapsedMinutes, int SnoozeMinutes);

    public record StatusFormatConfigResponse(string StatusFormat, string TimeSpanFormat);

    public static AppConfigResponseDto FromDomain(Domain.Config.AppConfig config)
        => new(
            config.StandardWorkMinutes,
            config.ResidentNotificationEnabled,
            new WorkEndAlarmConfigResponse(
                config.WorkEndAlarm.IsEnabled,
                config.WorkEndAlarm.RemainingMinutes,
                config.WorkEndAlarm.SnoozeMinutes
            ),
            new RestStartAlarmConfigResponse(
                config.RestStartAlarm.IsEnabled,
                config.RestStartAlarm.ElapsedMinutes,
                config.RestStartAlarm.SnoozeMinutes
            ),
            new StatusFormatConfigResponse(
                config.StatusFormat.StatusFormat,
                config.StatusFormat.TimeSpanFormat
            )
        );
}
