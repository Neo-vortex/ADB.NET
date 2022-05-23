var target = Argument("target", "Test");
var configuration = Argument("configuration", "Release");

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Clean")
    .WithCriteria(c => HasArgument("rebuild"))
    .Does(() =>
{
    CleanDirectory($"./src/ADB.NET/bin/{configuration}");
    CleanDirectory($"./src/Playground/bin/{configuration}");
    CleanDirectory($"./src/UnitTest/bin/{configuration}");
    CleanDirectory($"./src/USBAndroidDebuggingBridge/bin/{configuration}");
    CleanDirectory($"./src/WirelessAndroidDebuggingBridge/bin/{configuration}");
    CleanDirectory($"./src/AndroidDebuggingBridgeKeyrin/bin/{configuration}");
    

});

Task("Build")
    .IsDependentOn("Clean")
    .Does(() =>
{
    DotNetBuild("ADB.NET.sln", new DotNetBuildSettings
    {
        Configuration = configuration,
    });
});

Task("Test")
    .IsDependentOn("Build")
    .Does(() =>
{
    DotNetTest("ADB.NET.sln", new DotNetTestSettings
    {
        Configuration = configuration,
        NoBuild = true,
    });
});

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);
