using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using EdiFabric.Api;

public class X12Functions
{
    EdiFunctions _ediFunctions;

    public X12Functions(IX12Service x12Service)
    {
        _ediFunctions = new EdiFunctions(x12Service);
    }

    [Function("x12read")]
    public async Task<HttpResponseData> Read([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "x12/read")] HttpRequestData req, FunctionContext executionContext)
    {
        return await _ediFunctions.Read(req, executionContext.GetLogger<IX12Service>());
    }

    [Function("x12write")]
    public async Task<HttpResponseData> Write([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "x12/write")] HttpRequestData req, FunctionContext executionContext)
    {
        return await _ediFunctions.Write(req, executionContext.GetLogger<IX12Service>());
    }

    [Function("x12validate")]
    public async Task<HttpResponseData> Validate([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "x12/validate")] HttpRequestData req, FunctionContext executionContext)
    {
        return await _ediFunctions.Validate(req, executionContext.GetLogger<IX12Service>());
    }

    [Function("x12ack")]
    public async Task<HttpResponseData> Ack([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "x12/ack")] HttpRequestData req, FunctionContext executionContext)
    {
        return await _ediFunctions.Ack(req, executionContext.GetLogger<IX12Service>());
    }
    /// <summary>
    /// This is a system operation used only for the in-house web translator.
    /// </summary>
    [Function("x12analyze")]
    public async Task<HttpResponseData> Analyze([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "x12/analyze")] HttpRequestData req, FunctionContext executionContext)
    {
        return await _ediFunctions.Analyze(req, executionContext.GetLogger<IX12Service>());
    }
}
