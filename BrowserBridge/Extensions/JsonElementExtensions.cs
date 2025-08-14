using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

namespace BrowserBridge;

public static class JsonElementExtensions
{
    public static bool TryParse<T>(this JsonElement element, out T value)
    {
        if (element.ValueKind is JsonValueKind.Null or JsonValueKind.Undefined)
        {
            value = default!;
            return false;
        }

        try
        {
            value = element.Deserialize<T>()!;
            return value != null;
        }
        catch
        {
            value = default!;
            return false;
        }
    }

    public static bool TryParse<T>(this JsonElement element, JsonTypeInfo<T> jsonTypeInfo, out T value)
    {
        if (element.ValueKind is JsonValueKind.Null or JsonValueKind.Undefined)
        {
            value = default!;
            return false;
        }

        try
        {
            value = element.Deserialize(jsonTypeInfo)!;
            return value != null;
        }
        catch
        {
            value = default!;
            return false;
        }
    }
}
