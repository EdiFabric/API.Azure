using System.Runtime.InteropServices;
using EdiFabric.Api.Azure;
using EdiFabric.Native.X12;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var workerDirectory = Path.GetDirectoryName(typeof(Program).Assembly.Location);
if (string.IsNullOrEmpty(workerDirectory))
    workerDirectory = AppContext.BaseDirectory;

PrependNativeLibrarySearchPath(workerDirectory);

var host = new HostBuilder()
                .ConfigureFunctionsWorkerDefaults()
                .ConfigureServices(s =>
                {
                    s.AddSingleton<ILocalModelsService, LocalModelsService>();
                })
                .Build();

if (string.IsNullOrEmpty(Configuration.ApiKey))
    throw new Exception("No ApiKey configuration.");

var libraryPath = string.IsNullOrWhiteSpace(Configuration.LibraryPath) ? workerDirectory : Configuration.LibraryPath;
try
{
    EdiFabricX12.Load(libraryPath);
}
catch (DllNotFoundException ex)
{
    throw new InvalidOperationException(
        $"Unable to load '{NativeLibraryFileName()}' from '{workerDirectory}'. " +
        "Linux Azure Function Apps need edifabric-x12-tools.so in the published output " +
        "(place it in the project or repository root before publishing). " +
        "Windows Function Apps need edifabric-x12-tools.dll.",
        ex);
}

EdiFabricX12.SetSerial(Configuration.ApiKey);

var localModels = host.Services.GetRequiredService<ILocalModelsService>();
var mapPath = Path.Combine(workerDirectory, "map", "map.json");
if (File.Exists(mapPath))
    localModels.Load(Configuration.ApiKey, mapPath);
else
    localModels.LoadOnline(Configuration.ApiKey);

host.Run();

static void PrependNativeLibrarySearchPath(string directory)
{
    if (string.IsNullOrEmpty(directory))
        return;

    var variable = OperatingSystem.IsWindows() ? "PATH" : "LD_LIBRARY_PATH";
    var current = Environment.GetEnvironmentVariable(variable);
    var combined = string.IsNullOrEmpty(current) ? directory : directory + Path.PathSeparator + current;
    Environment.SetEnvironmentVariable(variable, combined);
}

static string NativeLibraryFileName()
{
    if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        return "edifabric-x12-tools.dll";
    if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        return "edifabric-x12-tools.dylib";
    return "edifabric-x12-tools.so";
}
