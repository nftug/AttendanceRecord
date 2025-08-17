namespace AttendanceRecord.Domain.Utils;

public static class DateTimeProvider
{
    private static Func<DateTime> _utcNow = () => DateTime.UtcNow;

    public static DateTime UtcNow => _utcNow();

    public static DateOnly UtcToday => DateOnly.FromDateTime(UtcNow);

    public static void SetUtcNow(Func<DateTime> nowFunc) => _utcNow = nowFunc;

    public static void SetNowDate(DateOnly date, Func<DateTime> nowFunc) =>
        SetUtcNow(() => date.ToDateTime(TimeOnly.MinValue).Add(nowFunc().TimeOfDay));

    public static void Reset() => _utcNow = () => DateTime.UtcNow;
}
