namespace AttendanceRecord.Domain.Config;

public record AppConfig(
    int StandardWorkMinutes,
    bool ResidentNotificationEnabled,
    AppConfig.WorkRecordAlarmConfig WorkRecordAlarm,
    AppConfig.RestRecordAlarmConfig RestRecordAlarm,
    AppConfig.StatusFormatConfig StatusFormat
)
{
    public record WorkRecordAlarmConfig(
        bool IsEnabled,
        int RemainingMinutes,
        int SnoozeMinutes
    );

    public record RestRecordAlarmConfig(
        bool IsEnabled,
        int ElapsedMinutes,
        int SnoozeMinutes
    );

    public record StatusFormatConfig(string StatusFormat, string TimeSpanFormat);

    public static readonly AppConfig Default = new(
        StandardWorkMinutes: 480,
        ResidentNotificationEnabled: true,
        WorkRecordAlarm: new(
            IsEnabled: true,
            RemainingMinutes: 15,
            SnoozeMinutes: 5
        ),
        RestRecordAlarm: new(
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
