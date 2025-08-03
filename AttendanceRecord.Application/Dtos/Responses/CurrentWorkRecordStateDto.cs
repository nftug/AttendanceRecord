using AttendanceRecord.Domain.Entities;
using AttendanceRecord.Domain.ValueObjects;

namespace AttendanceRecord.Application.Dtos.Responses;

public record CurrentWorkRecordStateDto(
    TimeSpan WorkTime,
    TimeSpan RestTime,
    TimeSpan Overtime,
    TimeSpan OverTimeMonthly,
    bool IsWorking,
    bool IsResting
)
{
    public static CurrentWorkRecordStateDto FromDomain(WorkRecord workRecord, WorkRecordTally monthlyTally)
        => new(
                WorkTime: workRecord.TotalWorkTime,
                RestTime: workRecord.TotalRestTime,
                Overtime: workRecord.Overtime,
                OverTimeMonthly: monthlyTally.OvertimeTotal,
                IsWorking: workRecord.IsWorking,
                IsResting: workRecord.IsResting
            );

    public static CurrentWorkRecordStateDto Empty => FromDomain(WorkRecord.Empty, WorkRecordTally.Empty);
}
