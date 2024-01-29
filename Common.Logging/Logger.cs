using BepInEx.Logging;
using Microsoft.Extensions.Logging;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace BoosterRaise.Common.Logging;

public class Logger : ILogger
{
    private readonly string name;
    private readonly ManualLogSource degelate;

    public Logger(string name, ManualLogSource degelate)
    {
        this.name = name;
        this.degelate = degelate;
    }

    public IDisposable BeginScope<TState>(TState state) => default!;

    public bool IsEnabled(LogLevel logLevel) => LogLevel.None != logLevel;

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        degelate.Log(mapLevel(logLevel), $"{name} - {formatter(state, exception)}");
    }

    private BepInEx.Logging.LogLevel mapLevel(LogLevel logLevel) => logLevel switch
    {
        LogLevel.Trace => BepInEx.Logging.LogLevel.Debug,
        LogLevel.Debug => BepInEx.Logging.LogLevel.Info,
        LogLevel.Information => BepInEx.Logging.LogLevel.Message,
        LogLevel.Warning => BepInEx.Logging.LogLevel.Warning,
        LogLevel.Error => BepInEx.Logging.LogLevel.Error,
        LogLevel.Critical => BepInEx.Logging.LogLevel.Fatal,
        LogLevel.None => BepInEx.Logging.LogLevel.None,
        _ => throw new ArgumentOutOfRangeException(nameof(logLevel), $"Not expected logLevel value: {logLevel}"),
    };
}
