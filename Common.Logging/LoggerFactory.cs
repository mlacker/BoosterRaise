using BepInEx.Logging;
using Microsoft.Extensions.Logging;

namespace BoosterRaise.Common.Logging;

public static class LoggerFactory
{
    private static ILoggerProvider? provider;

    public static void InitialProvider(ManualLogSource logSource)
    {
        provider = new LoggerProvider(logSource);
    }

    public static ILogger CreateLogger<T>()
    {
        return provider!.CreateLogger(typeof(T).Name);
    }

    public static ILogger CreateLogger(string categoryName)
    {
        return provider!.CreateLogger(categoryName);
    }
}
