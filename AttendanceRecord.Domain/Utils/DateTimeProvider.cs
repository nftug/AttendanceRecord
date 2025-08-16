namespace AttendanceRecord.Domain.Utils;

public static class DateTimeProvider
{
    private static Func<DateTime> _now = () => DateTime.Now;

    public static DateTime Now => _now();

    public static DateOnly Today => DateOnly.FromDateTime(Now);

    public static void SetNow(Func<DateTime> nowFunc) => _now = nowFunc;

    public static void SetNowDate(DateOnly date) =>
        SetNow(() => date.ToDateTime(TimeOnly.MinValue).Add(DateTime.Now.TimeOfDay));

    public static void Reset() => _now = () => DateTime.Now;
}
