using AttendanceRecord.Domain.Enums;

namespace AttendanceRecord.Dtos.Alarm;

public enum AlarmCommandType
{
    Snooze
}

public record SnoozeCommandPayload(AlarmType Type);
