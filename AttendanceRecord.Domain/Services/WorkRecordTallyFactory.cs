using AttendanceRecord.Domain.Interfaces;
using AttendanceRecord.Domain.ValueObjects;
using Nager.Date;

namespace AttendanceRecord.Domain.Services;

public class WorkRecordTallyFactory(IWorkRecordRepository workRecordRepository, AppConfigStore appConfigStore)
{
    public async Task<WorkRecordTally> GetMonthlyAsync(int year, int month)
    {
        var workRecords = await workRecordRepository.FindByMonthAsync(year, month);
        var monthlyTotalBusinessTime = GetTotalBusinessTime(year, month);
        return new WorkRecordTally(workRecords, monthlyTotalBusinessTime);
    }

    private TimeSpan GetTotalBusinessTime(int year, int month)
    {
        var businessDays = GetBusinessDaysInMonth(year, month);
        return TimeSpan.FromMinutes(appConfigStore.Config.StandardWorkMinutes * businessDays.Count());
    }

    private static IEnumerable<DateOnly> GetBusinessDaysInMonth(int year, int month)
    {
        var daysInMonth = DateTime.DaysInMonth(year, month);

        HashSet<DateTime> jpHolidays =
            [.. HolidaySystem.GetHolidays(year, CountryCode.JP).Select(h => h.Date.Date)];

        return Enumerable.Range(1, daysInMonth)
            .Select(day => new DateTime(year, month, day))
            .Where(d => d.DayOfWeek is not DayOfWeek.Saturday and not DayOfWeek.Sunday)
            .Where(d => !jpHolidays.Contains(d.Date))
            .Distinct()
            .Select(DateOnly.FromDateTime);
    }
}
