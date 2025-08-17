namespace AttendanceRecord.Domain.Utils;

public static class DateTimeProvider
{
    private static Func<DateTime> _now = () => DateTime.Now;

    public static DateTime Now => _now();

    public static DateTime Today => Now.Date;

    public static void SetNow(Func<DateTime> nowFunc) => _now = nowFunc;

    public static void SetNowDate(DateOnly date, Func<DateTime> nowFunc) =>
        SetNow(() => date.ToDateTime(TimeOnly.MinValue).Add(nowFunc().TimeOfDay));

    public static void Reset() => _now = () => DateTime.Now;
}
