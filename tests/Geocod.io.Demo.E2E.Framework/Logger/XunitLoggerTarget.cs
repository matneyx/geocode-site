using NLog;
using NLog.Targets;
using Xunit.Abstractions;

namespace Geocod.io.Demo.E2E.Framework.Logger;

public class XunitLoggerTarget : TargetWithLayout
{
    private readonly ITestOutputHelper helper;

    public XunitLoggerTarget(ITestOutputHelper helper)
    {
        this.helper = helper;
    }

    protected override void Write(LogEventInfo logEvent)
    {
        var logMessage = Layout.Render(logEvent);
        helper.WriteLine(logMessage);
    }
}
