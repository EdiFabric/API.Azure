using EdiFabric.Api;
using EdiFabric.Api.Azure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
                .ConfigureFunctionsWorkerDefaults()
                .ConfigureServices(s =>
                {
                    s.AddEdiFabricApi();
                    //  Uncomment if you wish to use distributed cache for models
                    //  s.AddHostedService<LocalModelsService>();
                })
                
                .Build();

host.Run();