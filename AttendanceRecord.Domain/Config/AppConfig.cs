namespace AttendanceRecord.Domain.Config;

public record AppConfig(
    int StandardWorkMinutes,
    bool ResidentNotificationEnabled,
    AppConfig.WorkEndAlarmConfig WorkEndAlarm,
    AppConfig.RestStartAlarmConfig RestStartAlarm,
    AppConfig.StatusFormatConfig StatusFormat
)
{
    public TimeSpan StandardWorkTimeSpan => TimeSpan.FromMinutes(StandardWorkMinutes);

    public record WorkEndAlarmConfig(
        bool IsEnabled,
        int RemainingMinutes,
        int SnoozeMinutes
    )
    {
        public TimeSpan RemainingTime => TimeSpan.FromMinutes(RemainingMinutes);
        public TimeSpan SnoozeTime => TimeSpan.FromMinutes(SnoozeMinutes);
    }

    public record RestStartAlarmConfig(
        bool IsEnabled,
        int ElapsedMinutes,
        int SnoozeMinutes
    )
    {
        public TimeSpan ElapsedTime => TimeSpan.FromMinutes(ElapsedMinutes);
        public TimeSpan SnoozeTime => TimeSpan.FromMinutes(SnoozeMinutes);
    }

    public record StatusFormatConfig(string StatusFormat, string TimeSpanFormat);

    public static readonly AppConfig Default = new(
        StandardWorkMinutes: 480,
        ResidentNotificationEnabled: true,
        WorkEndAlarm: new(
            IsEnabled: true,
            RemainingMinutes: 15,
            SnoozeMinutes: 5
        ),
        RestStartAlarm: new(
            IsEnabled: true,
            ElapsedMinutes: 240,
            SnoozeMinutes: 5
        ),
        StatusFormat: new(
            StatusFormat: @"- 勤務時間: {daily_work}
- 休憩時間: {daily_rest}
- 本日の残業時間: {daily_over}
- 今月の残業時間: {monthly_over}",
            TimeSpanFormat: "h'時間'm'分'"
            )
    );
}
