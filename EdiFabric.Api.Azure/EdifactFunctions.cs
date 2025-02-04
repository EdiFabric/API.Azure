using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Configuration;
using EdiFabric.Api;

public class EdifactFunctions
{
    EdiFunctions _ediFunctions;

    public EdifactFunctions(IEdifactService edifactService)
    {
        _ediFunctions = new EdiFunctions(edifactService);
    }

    [Function("edifactread")]
    public async Task<HttpResponseData> Read([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "edifact/read")] HttpRequestData req, FunctionContext executionContext)
    {
        return await _ediFunctions.Read(req, executionContext.GetLogger<IEdifactService>());
    }

    [Function("edifactwrite")]
    public async Task<HttpResponseData> Write([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "edifact/write")] HttpRequestData req, FunctionContext executionContext)
    {
        return await _ediFunctions.Write(req, executionContext.GetLogger<IEdifactService>());
    }

    [Function("edifactvalidate")]
    public async Task<HttpResponseData> Validate([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "edifact/validate")] HttpRequestData req, FunctionContext executionContext)
    {
        return await _ediFunctions.Validate(req, executionContext.GetLogger<IEdifactService>());
    }

    [Function("edifactack")]
    public async Task<HttpResponseData> Ack([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "edifact/ack")] HttpRequestData req, FunctionContext executionContext)
    {
        return await _ediFunctions.Ack(req, executionContext.GetLogger<IEdifactService>());
    }
    /// <summary>
    /// This is a system operation used only for the in-house web translator.
    /// </summary>
    [Function("edifactanalyze")]
    public async Task<HttpResponseData> Analyze([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "edifact/analyze")] HttpRequestData req, FunctionContext executionContext)
    {
        return await _ediFunctions.Analyze(req, executionContext.GetLogger<IEdifactService>());
    }
}
