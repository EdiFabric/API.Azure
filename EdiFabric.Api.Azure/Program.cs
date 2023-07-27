using EdiFabric.Api;
using EdiFabric.Api.Azure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
                .ConfigureFunctionsWorkerDefaults()
                .ConfigureServices(s =>
                {
                    s.AddEdiFabricApi();
                    s.AddHostedService<LocalModelsService>();
                })
                
                .Build();

host.Run();