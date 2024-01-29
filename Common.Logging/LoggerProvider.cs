using System.Collections.Concurrent;
using BepInEx.Logging;
using Microsoft.Extensions.Logging;

namespace BoosterRaise.Common.Logging;

public sealed class LoggerProvider : ILoggerProvider
{
    private readonly ConcurrentDictionary<string, ILogger> loggers = new ();
    private readonly ManualLogSource logSource;

    public LoggerProvider(ManualLogSource logSource) {
        this.logSource = logSource;
    }

    public ILogger CreateLogger(string categoryName)
    {
        return loggers.GetOrAdd(categoryName, name => new Logger(name, logSource));
    }

    public void Dispose()
    {
    }
}
