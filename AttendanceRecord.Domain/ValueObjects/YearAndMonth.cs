namespace AttendanceRecord.Domain.ValueObjects;

public record YearAndMonth(int Year, int Month)
{
    public static YearAndMonth FromDate(DateOnly date)
        => new(date.Year, date.Month);

    public static YearAndMonth Empty => new(0, 0);
}
