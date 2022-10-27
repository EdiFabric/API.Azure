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

    [Function("ncpdp/read")]
    public async Task<HttpResponseData> Read([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req, FunctionContext executionContext)
    {
        return await _ediFunctions.Read(req, executionContext.GetLogger<INcpdpService>());
    }

    [Function("ncpdp/write")]
    public async Task<HttpResponseData> Write([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req, FunctionContext executionContext)
    {
        return await _ediFunctions.Write(req, executionContext.GetLogger<INcpdpService>());
    }

    [Function("ncpdp/validate")]
    public async Task<HttpResponseData> Validate([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req, FunctionContext executionContext)
    {
        return await _ediFunctions.Validate(req, executionContext.GetLogger<INcpdpService>());
    }

    [Function("ncpdp/analyze")]
    public async Task<HttpResponseData> Analyze([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req, FunctionContext executionContext)
    {
        return await _ediFunctions.Analyze(req, executionContext.GetLogger<INcpdpService>());
    }
}
