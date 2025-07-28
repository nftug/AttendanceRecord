using System.Text.Json.Serialization;
using AttendanceRecord.Application.Dtos.Responses;
using AttendanceRecord.Dtos.HomePage;
using BrowserBridge;

namespace AttendanceRecord.Constants;

[JsonSerializable(typeof(HomePageCommandType))]
[JsonSerializable(typeof(EventMessage<CurrentWorkRecordStateDto>))]
[JsonSerializable(typeof(EventMessage<WorkRecordTallyResponseDto>))]
[JsonSerializable(typeof(EventMessage<WorkRecordResponseDto>))]
[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
public partial class AppJsonContext : JsonSerializerContext;
