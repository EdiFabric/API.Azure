using EdiFabric.Api;
using Microsoft.Extensions.Hosting;

namespace EdiFabric.Api.Azure.Functions
{
    public class Program
    {
        public static void Main()
        {
            var host = new HostBuilder()
                .ConfigureFunctionsWorkerDefaults()
                .ConfigureServices(s => {
                    s.AddEdiNation();
                })
                .Build();

            host.Run();
        }
    }
}