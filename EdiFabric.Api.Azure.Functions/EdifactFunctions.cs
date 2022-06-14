using System.Threading.Tasks;
using EdiFabric.Api;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Configuration;

namespace EdiFabric.Api.Azure.Functions
{
    public class EdifactFunctions
    {
        EdiFunctions _ediFunctions;

        public EdifactFunctions(IEdifactService edifactService, IConfiguration configuration)
        {
            _ediFunctions = new EdiFunctions(edifactService);
        }

        [Function("edifact/read")]
        public async Task<HttpResponseData> Read([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req, FunctionContext executionContext)
        {
            return await _ediFunctions.Read(req, executionContext.GetLogger<IEdifactService>());
        }

        [Function("edifact/write")]
        public async Task<HttpResponseData> Write([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req, FunctionContext executionContext)
        {
            return await _ediFunctions.Write(req, executionContext.GetLogger<IEdifactService>());
        }      

        [Function("edifact/validate")]
        public async Task<HttpResponseData> Validate([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req, FunctionContext executionContext)
        {
            return await _ediFunctions.Validate(req, executionContext.GetLogger<IEdifactService>());
        }

        [Function("edifact/ack")]
        public async Task<HttpResponseData> Ack([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req, FunctionContext executionContext)
        {
            return await _ediFunctions.Ack(req, executionContext.GetLogger<IEdifactService>());
        }        
    }
}
