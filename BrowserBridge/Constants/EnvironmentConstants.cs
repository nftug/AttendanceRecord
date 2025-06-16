using System.Reflection;

namespace BrowserBridge;

public static class EnvironmentConstants
{

#if DEBUG

    public static readonly bool IsDebugMode = true;

#else

    public static readonly bool IsDebugMode = false;

#endif

    public static readonly string AppName =
        Assembly.GetEntryAssembly()?.GetCustomAttribute<AssemblyTitleAttribute>()?.Title ?? "";
}
