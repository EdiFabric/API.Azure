using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;

namespace EdiFabric.Api.Azure.Functions
{
    public class X12Functions
    {
        EdiFunctions _ediFunctions;

        public X12Functions(IX12Service x12Service)
        {
            _ediFunctions = new EdiFunctions(x12Service);
        }

        [Function("x12/read")]
        public async Task<HttpResponseData> Read([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req, FunctionContext executionContext)
        {
            return await _ediFunctions.Read(req, executionContext.GetLogger<IX12Service>());
        }

        [Function("x12/write")]
        public async Task<HttpResponseData> Write([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req, FunctionContext executionContext)
        {
            return await _ediFunctions.Write(req, executionContext.GetLogger<IX12Service>());
        }

        [Function("x12/validate")]
        public async Task<HttpResponseData> Validate([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req, FunctionContext executionContext)
        {
            return await _ediFunctions.Validate(req, executionContext.GetLogger<IX12Service>());
        }

        [Function("x12/ack")]
        public async Task<HttpResponseData> Ack([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req, FunctionContext executionContext)
        {
            return await _ediFunctions.Ack(req, executionContext.GetLogger<IX12Service>());
        }
    }
}
