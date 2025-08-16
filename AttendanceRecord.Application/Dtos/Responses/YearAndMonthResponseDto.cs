using AttendanceRecord.Domain.ValueObjects;

namespace AttendanceRecord.Application.Dtos.Responses;

public record YearAndMonthResponseDto(int Year, int Month)
{
    public static YearAndMonthResponseDto FromDomain(YearAndMonth yearAndMonth) =>
        new(yearAndMonth.Year, yearAndMonth.Month);
}
