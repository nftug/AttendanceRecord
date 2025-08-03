namespace AttendanceRecord.Domain.Config;

public record AppConfig(
    int StandardWorkMinutes,
    bool ResidentNotificationEnabled,
    AppConfig.WorkTimeAlarmConfig WorkTimeAlarm,
    AppConfig.RestTimeAlarmConfig RestTimeAlarm,
    AppConfig.StatusFormatConfig StatusFormat
)
{
    public record WorkTimeAlarmConfig(
        bool IsEnabled,
        int RemainingMinutes,
        int SnoozeMinutes
    )
    {
        public TimeSpan RemainingTime => TimeSpan.FromMinutes(RemainingMinutes);
        public TimeSpan SnoozeTime => TimeSpan.FromMinutes(SnoozeMinutes);

        /// <summary>
        /// 退勤前アラームを鳴らすべきか判定
        /// </summary>
        public bool ShouldTrigger(Entities.WorkRecord workRecord)
            => IsEnabled
                && workRecord.IsTodaysOngoing
                && workRecord.Overtime >= -RemainingTime;
    }

    public record RestTimeAlarmConfig(
        bool IsEnabled,
        int ElapsedMinutes,
        int SnoozeMinutes
    )
    {
        public TimeSpan ElapsedTime => TimeSpan.FromMinutes(ElapsedMinutes);
        public TimeSpan SnoozeTime => TimeSpan.FromMinutes(SnoozeMinutes);

        /// <summary>
        /// 休憩前アラームを鳴らすべきか判定
        /// </summary>
        public bool ShouldTrigger(Entities.WorkRecord workRecord)
            => IsEnabled
                && workRecord.IsTodaysOngoing
                && workRecord.TotalRestTime == TimeSpan.Zero
                && workRecord.TotalWorkTime >= ElapsedTime;
    }

    public record StatusFormatConfig(string StatusFormat, string TimeSpanFormat);

    public static readonly AppConfig Default = new(
        StandardWorkMinutes: 480,
        ResidentNotificationEnabled: true,
        WorkTimeAlarm: new(
            IsEnabled: true,
            RemainingMinutes: 15,
            SnoozeMinutes: 5
        ),
        RestTimeAlarm: new(
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
