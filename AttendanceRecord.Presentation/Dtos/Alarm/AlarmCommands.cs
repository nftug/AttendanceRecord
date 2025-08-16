using AttendanceRecord.Domain.Enums;

namespace AttendanceRecord.Presentation.Dtos.Alarm;

public enum AlarmCommandType
{
    Snooze
}

public record SnoozeCommandPayload(AlarmType Type);
