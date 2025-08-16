using System.Text.Json.Serialization;

namespace AttendanceRecord.Domain.Enums;

[JsonConverter(typeof(JsonStringEnumConverter<AlarmType>))]
public enum AlarmType
{
    WorkEnd,
    RestStart
}
