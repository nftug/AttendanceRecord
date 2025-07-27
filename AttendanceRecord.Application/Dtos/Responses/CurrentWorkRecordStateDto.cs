using AttendanceRecord.Domain.Entities;
using AttendanceRecord.Domain.ValueObjects;

namespace AttendanceRecord.Application.Dtos.Responses;

public record CurrentWorkRecordStateDto(
    TimeSpan WorkTime,
    TimeSpan RestTime,
    TimeSpan Overtime,
    TimeSpan OverTimeMonthly,
    bool IsResting,
    bool CanToggleRest
)
{
    public static CurrentWorkRecordStateDto FromDomain(WorkRecord workRecord, WorkRecordTally monthlyTally)
        => new(
                WorkTime: workRecord.TotalWorkTime,
                RestTime: workRecord.TotalRestTime,
                Overtime: workRecord.Overtime,
                OverTimeMonthly: monthlyTally.OvertimeTotal,
                IsResting: workRecord.IsResting,
                CanToggleRest: workRecord.IsTodaysOngoing
            );

    public static CurrentWorkRecordStateDto Empty => FromDomain(WorkRecord.Empty, WorkRecordTally.Empty);
}
