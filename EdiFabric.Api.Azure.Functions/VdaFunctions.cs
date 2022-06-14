using System.Threading.Tasks;
using EdiFabric.Api;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;

namespace EdiFabric.Api.Azure.Functions
{
    public class VdaFunctions
    {
        EdiFunctions _ediFunctions;

        public VdaFunctions(IVdaService vdaService)
        {
            _ediFunctions = new EdiFunctions(vdaService);
        }

        [Function("vda/read")]
        public async Task<HttpResponseData> Read([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req, FunctionContext executionContext)
        {
            return await _ediFunctions.Read(req, executionContext.GetLogger<IVdaService>());
        }

        [Function("vda/write")]
        public async Task<HttpResponseData> Write([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req, FunctionContext executionContext)
        {
            return await _ediFunctions.Write(req, executionContext.GetLogger<IVdaService>());
        }

        [Function("vda/validate")]
        public async Task<HttpResponseData> Validate([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req, FunctionContext executionContext)
        {
            return await _ediFunctions.Validate(req, executionContext.GetLogger<IVdaService>());
        }
    }
}
