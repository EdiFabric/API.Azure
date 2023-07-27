using Microsoft.Extensions.Hosting;

namespace EdiFabric.Api.Azure
{
    public class LocalModelsService : IHostedService
    {
        private readonly IModelService _modelService;

        public LocalModelsService(IModelService modelService)
        {
            _modelService = modelService;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            //  Load local EDI models
            //  When models are local they won't be pulled from EdiNation API
            try
            {
                await BlobCache.LoadModels(_modelService);
            }
            catch(Exception ex) 
            {
                Console.WriteLine($"Can't load models from cache. {ex.Message}");
            }
        }
        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
