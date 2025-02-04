using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using EdiFabric.Api;

public class VdaFunctions
{
    EdiFunctions _ediFunctions;

    public VdaFunctions(IVdaService vdaService)
    {
        _ediFunctions = new EdiFunctions(vdaService);
    }

    [Function("vdaread")]
    public async Task<HttpResponseData> Read([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "vda/read")] HttpRequestData req, FunctionContext executionContext)
    {
        return await _ediFunctions.Read(req, executionContext.GetLogger<IVdaService>());
    }

    [Function("vdawrite")]
    public async Task<HttpResponseData> Write([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "vda/write")] HttpRequestData req, FunctionContext executionContext)
    {
        return await _ediFunctions.Write(req, executionContext.GetLogger<IVdaService>());
    }

    [Function("vdavalidate")]
    public async Task<HttpResponseData> Validate([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "vda/validate")] HttpRequestData req, FunctionContext executionContext)
    {
        return await _ediFunctions.Validate(req, executionContext.GetLogger<IVdaService>());
    }
    /// <summary>
    /// This is a system operation used only for the in-house web translator.
    /// </summary>
    [Function("vdaanalyze")]
    public async Task<HttpResponseData> Analyze([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "vda/analyze")] HttpRequestData req, FunctionContext executionContext)
    {
        return await _ediFunctions.Analyze(req, executionContext.GetLogger<IVdaService>());
    }
}
