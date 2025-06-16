using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

namespace BrowserBridge;

public static class JsonElementExtensions
{
    public static T? ParsePayload<T>(this JsonElement element)
    {
        if (element.ValueKind is JsonValueKind.Null or JsonValueKind.Undefined)
            return default;

        return element.Deserialize<T>();
    }

    public static T? ParsePayload<T>(this JsonElement element, JsonTypeInfo<T> jsonTypeInfo)
    {
        if (element.ValueKind is JsonValueKind.Null or JsonValueKind.Undefined)
            return default;

        return element.Deserialize(jsonTypeInfo);
    }

    public static ValueTask HandlePayloadAsync<T>(
        this JsonElement payload, Func<T, ValueTask> callback)
    {
        var parsed = payload.ParsePayload<T>();
        return parsed != null ? callback(parsed) : ValueTask.CompletedTask;
    }

    public static ValueTask HandlePayloadSync<T>(
        this JsonElement payload, Action<T> callback)
    {
        return payload.HandlePayloadAsync<T>((parsed) =>
        {
            callback(parsed);
            return ValueTask.CompletedTask;
        });
    }

    public static ValueTask HandlePayloadAsync<T>(
        this JsonElement payload, JsonTypeInfo<T> jsonTypeInfo, Func<T, ValueTask> callback)
    {
        var parsed = payload.ParsePayload(jsonTypeInfo);
        return parsed != null ? callback(parsed) : ValueTask.CompletedTask;
    }

    public static ValueTask HandlePayloadSync<T>(
        this JsonElement payload, JsonTypeInfo<T> jsonTypeInfo, Action<T> callback)
    {
        return payload.HandlePayloadAsync(jsonTypeInfo, (parsed) =>
        {
            callback(parsed);
            return ValueTask.CompletedTask;
        });
    }
}
