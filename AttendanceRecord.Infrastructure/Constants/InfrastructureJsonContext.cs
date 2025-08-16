using System.Text.Json;
using System.Text.Json.Serialization;
using AttendanceRecord.Infrastructure.Dtos;

namespace AttendanceRecord.Infrastructure.Constants;

[JsonSerializable(typeof(WorkRecordFileDto[]))]
[JsonSerializable(typeof(AppConfigFileDto))]
[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
public partial class InfrastructureJsonContext : JsonSerializerContext
{
    public static InfrastructureJsonContext Indented { get; } =
        new(new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });
}
