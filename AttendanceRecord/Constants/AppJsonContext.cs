using System.Text.Json.Serialization;
using AttendanceRecord.Application.Dtos.Requests;
using AttendanceRecord.Dtos.HistoryPage;
using AttendanceRecord.Dtos.HomePage;

namespace AttendanceRecord.Constants;

[JsonSerializable(typeof(HomePageStateEvent))]
[JsonSerializable(typeof(ToggleWorkResultEvent))]
[JsonSerializable(typeof(ToggleRestResultEvent))]
[JsonSerializable(typeof(GetWorkRecordListResultEvent))]
[JsonSerializable(typeof(GetWorkRecordResultEvent))]
[JsonSerializable(typeof(SaveWorkRecordResultEvent))]
[JsonSerializable(typeof(DeleteWorkRecordResultEvent))]
[JsonSerializable(typeof(WorkRecordTallyGetRequestDto))]
[JsonSerializable(typeof(WorkRecordSaveRequestDto))]
[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
public partial class AppJsonContext : JsonSerializerContext;
