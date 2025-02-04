using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using EdiFabric.Api;

public class NcpdpFunctions
{
    EdiFunctions _ediFunctions;

    public NcpdpFunctions(INcpdpService ncpdpService)
    {
        _ediFunctions = new EdiFunctions(ncpdpService);
    }

    [Function("ncpdpread")]
    public async Task<HttpResponseData> Read([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "ncpdp/read")] HttpRequestData req, FunctionContext executionContext)
    {
        return await _ediFunctions.Read(req, executionContext.GetLogger<INcpdpService>());
    }

    [Function("ncpdpwrite")]
    public async Task<HttpResponseData> Write([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "ncpdp/write")] HttpRequestData req, FunctionContext executionContext)
    {
        return await _ediFunctions.Write(req, executionContext.GetLogger<INcpdpService>());
    }

    [Function("ncpdpvalidate")]
    public async Task<HttpResponseData> Validate([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "ncpdp/validate")] HttpRequestData req, FunctionContext executionContext)
    {
        return await _ediFunctions.Validate(req, executionContext.GetLogger<INcpdpService>());
    }
    /// <summary>
    /// This is a system operation used only for the in-house web translator.
    /// </summary>
    [Function("ncpdpanalyze")]
    public async Task<HttpResponseData> Analyze([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "ncpdp/analyze")] HttpRequestData req, FunctionContext executionContext)
    {
        return await _ediFunctions.Analyze(req, executionContext.GetLogger<INcpdpService>());
    }
}
