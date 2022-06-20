using EdiFabric.Api;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
                .ConfigureFunctionsWorkerDefaults()
                .ConfigureServices(s =>
                {
                    s.AddEdiFabricApi();
                })
                .Build();

host.Run();