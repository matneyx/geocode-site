using System.Threading.Tasks;
using Cake.Common;
using Cake.Common.IO;
using Cake.Common.Tools.DotNet;
using Cake.Common.Tools.DotNet.Build;
using Cake.Common.Tools.DotNet.Test;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Frosting;
using Cake.Npm;

public static class Program
{
    public static int Main(string[] args)
    {
        return new CakeHost()
            .UseContext<BuildContext>()
            .Run(args);
    }
}

public class BuildContext : FrostingContext
{
    public string MsBuildConfiguration { get; set; }

    public BuildContext(ICakeContext context)
        : base(context)
    {
        MsBuildConfiguration = context.Argument("configuration", "Release");
    }
}

[TaskName("Clean")]
public sealed class CleanTask : FrostingTask<BuildContext>
{
    public override void Run(BuildContext context)
    {
        context.CleanDirectory($"../src/Geocod.io.Demo/bin/{context.MsBuildConfiguration}");
        context.CleanDirectory($"../src/Geocod.io.Demo.Tests/bin/{context.MsBuildConfiguration}");
    }
}

[TaskName("Build")]
[IsDependentOn(typeof(CleanTask))]
public sealed class BuildTask : FrostingTask<BuildContext>
{
    public override void Run(BuildContext context)
    {
        context.DotNetBuild("../src/Geocod.io.Demo.sln", new DotNetBuildSettings
        {
            Configuration = context.MsBuildConfiguration,
        });
    }
}

[TaskName("DotnetTest")]
[IsDependentOn(typeof(BuildTask))]
public sealed class DotnetTestTask : FrostingTask<BuildContext>
{
    public override void Run(BuildContext context)
    {
        context.DotNetTest("../src/Geocod.io.Demo.sln", new DotNetTestSettings
        {
            Configuration = context.MsBuildConfiguration,
            NoBuild = true,
        });
    }
}

[TaskName("ComponentTest")]
[IsDependentOn(typeof(BuildTask))]
public sealed class ComponentTestTask : FrostingTask<BuildContext>
{
    public override void Run(BuildContext context)
    {
        context.Environment.WorkingDirectory = context.Environment.WorkingDirectory.Combine(DirectoryPath.FromString("../src/Geocod.io.Demo/FrontEnd"));
        context.NpmRunScript("cypress:run");
    }
}

[TaskName("Default")]
[IsDependentOn(typeof(BuildTask))]
[IsDependentOn(typeof(DotnetTestTask))]
[IsDependentOn(typeof(ComponentTestTask))]
public class DefaultTask : FrostingTask
{
}
