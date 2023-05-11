using System;
using Boa.Constrictor.Screenplay;
using NLog;

namespace Geocod.io.Demo.E2E.Framework.Logger;

public static class LogSeverityExtensions
{
    public static LogLevel ToLogLevel(this LogSeverity severity)
    {
        return severity switch
        {
            LogSeverity.Trace => LogLevel.Trace,
            LogSeverity.Debug => LogLevel.Debug,
            LogSeverity.Info => LogLevel.Info,
            LogSeverity.Warning => LogLevel.Warn,
            LogSeverity.Error => LogLevel.Error,
            LogSeverity.Fatal => LogLevel.Fatal,
            _ => throw new ArgumentOutOfRangeException(nameof(severity), severity, null)
        };
    }
}
