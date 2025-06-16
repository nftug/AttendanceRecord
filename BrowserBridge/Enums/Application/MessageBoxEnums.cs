using System.Text.Json.Serialization;

namespace BrowserBridge;

[JsonConverter(typeof(JsonStringEnumConverter<ButtonsType>))]
public enum ButtonsType
{
    Ok,
    OkCancel,
    YesNo,
    YesNoCancel
}

[JsonConverter(typeof(JsonStringEnumConverter<IconType>))]
public enum IconType
{
    Info,
    Warning,
    Error,
    Question
}

[JsonConverter(typeof(JsonStringEnumConverter<MessageBoxResultType>))]
public enum MessageBoxResultType
{
    Ok,
    Cancel,
    Yes,
    No
}
