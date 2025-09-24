namespace AttendanceRecord.Application.Dtos.Requests;

public record AppConfigSaveRequestDto(
    int StandardWorkMinutes,
    bool ResidentNotificationEnabled,
    AppConfigSaveRequestDto.WorkEndAlarmConfigRequest WorkEndAlarm,
    AppConfigSaveRequestDto.RestStartAlarmConfigRequest RestStartAlarm,
    AppConfigSaveRequestDto.StatusFormatConfigRequest StatusFormat)
{
    public record WorkEndAlarmConfigRequest(bool IsEnabled, int RemainingMinutes, int SnoozeMinutes);

    public record RestStartAlarmConfigRequest(bool IsEnabled, int ElapsedMinutes, int SnoozeMinutes);

    public record StatusFormatConfigRequest(string StatusFormat, string TimeSpanFormat);

    public Domain.Config.AppConfig ToDomain() => new(
        StandardWorkMinutes,
        ResidentNotificationEnabled,
        new Domain.Config.AppConfig.WorkEndAlarmConfig(
            WorkEndAlarm.IsEnabled,
            WorkEndAlarm.RemainingMinutes,
            WorkEndAlarm.SnoozeMinutes
        ),
        new Domain.Config.AppConfig.RestStartAlarmConfig(
            RestStartAlarm.IsEnabled,
            RestStartAlarm.ElapsedMinutes,
            RestStartAlarm.SnoozeMinutes
        ),
        new Domain.Config.AppConfig.StatusFormatConfig(
            StatusFormat.StatusFormat,
            StatusFormat.TimeSpanFormat
        )
    );
}
