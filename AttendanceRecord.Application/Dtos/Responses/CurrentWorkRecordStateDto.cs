using AttendanceRecord.Domain.Config;
using AttendanceRecord.Domain.Entities;
using AttendanceRecord.Domain.Extensions;
using AttendanceRecord.Domain.Utils;
using AttendanceRecord.Domain.ValueObjects;

namespace AttendanceRecord.Application.Dtos.Responses;

public record CurrentWorkRecordStateDto(
    DateTime CurrentDateTime,
    TimeSpan WorkTime,
    TimeSpan RestTime,
    TimeSpan Overtime,
    TimeSpan OvertimeMonthly,
    bool IsActive,
    bool IsWorking,
    bool IsResting
)
{
    public static CurrentWorkRecordStateDto FromDomain(
        WorkRecord workRecord, WorkRecordTally monthlyTally, AppConfig appConfig) =>
            new(
                CurrentDateTime: DateTimeProvider.Now.TruncateMs(),
                WorkTime: workRecord.GetWorkTime(),
                RestTime: workRecord.GetRestTime(),
                Overtime: workRecord.GetOvertime(appConfig),
                OvertimeMonthly: monthlyTally.GetOvertimeTotal(appConfig),
                IsActive: workRecord.IsTodaysOngoing,
                IsWorking: workRecord.IsWorking,
                IsResting: workRecord.IsResting
            );

    public static CurrentWorkRecordStateDto Empty =>
        FromDomain(WorkRecord.Empty, WorkRecordTally.Empty, AppConfig.Default);
}
