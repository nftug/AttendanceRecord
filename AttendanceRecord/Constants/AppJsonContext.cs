using System.Text.Json.Serialization;
using AttendanceRecord.Application.Dtos.Requests;
using AttendanceRecord.Dtos.HomePage;
using AttendanceRecord.Dtos.WorkRecordList;

namespace AttendanceRecord.Constants;

[JsonSerializable(typeof(HomePageCommandType))]
[JsonSerializable(typeof(HomePageStateEvent))]
[JsonSerializable(typeof(ToggleWorkResultEvent))]
[JsonSerializable(typeof(ToggleRestResultEvent))]
[JsonSerializable(typeof(GetWorkRecordListResultEvent))]
[JsonSerializable(typeof(WorkRecordTallyGetRequestDto))]
[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
public partial class AppJsonContext : JsonSerializerContext;
