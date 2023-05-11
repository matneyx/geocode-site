using Boa.Constrictor.Screenplay;
using NLog;
using NLog.Config;
using Xunit.Abstractions;

namespace Geocod.io.Demo.E2E.Framework.Logger;

public class NunitBoaConstrictorLogger : AbstractLogger
{
    public NunitBoaConstrictorLogger(ITestOutputHelper testOutputHelper, LogSeverity lowestSeverity = LogSeverity.Trace)
        : base(lowestSeverity)
    {
        var config = new LoggingConfiguration();
        var target = new XunitLoggerTarget(testOutputHelper);
        config.AddTarget("Xunit", target);

        config.LoggingRules.Add(new LoggingRule("*", lowestSeverity.ToLogLevel(), target));
        LogManager.Configuration = config;
    }

    public override void Close()
    {
        LogManager.Shutdown();
    }

    protected override void LogRaw(string message, LogSeverity severity = LogSeverity.Info)
    {
        LogManager.GetLogger("Boa.Constrictor").Log(severity.ToLogLevel(), message);
    }
}
