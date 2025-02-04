using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using EdiFabric.Api;

public class Hl7Functions
{
    EdiFunctions _ediFunctions;

    public Hl7Functions(IHl7Service hl7Service)
    {
        _ediFunctions = new EdiFunctions(hl7Service);
    }

    [Function("hl7read")]
    public async Task<HttpResponseData> Read([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "hl7/read")] HttpRequestData req, FunctionContext executionContext)
    {
        return await _ediFunctions.Read(req, executionContext.GetLogger<IHl7Service>());
    }

    [Function("hl7write")]
    public async Task<HttpResponseData> Write([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "hl7/write")] HttpRequestData req, FunctionContext executionContext)
    {
        return await _ediFunctions.Write(req, executionContext.GetLogger<IHl7Service>());
    }

    [Function("hl7validate")]
    public async Task<HttpResponseData> Validate([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "hl7/validate")] HttpRequestData req, FunctionContext executionContext)
    {
        return await _ediFunctions.Validate(req, executionContext.GetLogger<IHl7Service>());
    }
    /// <summary>
    /// This is a system operation used only for the in-house web translator.
    /// </summary>
    [Function("hl7analyze")]
    public async Task<HttpResponseData> Analyze([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "hl7/analyze")] HttpRequestData req, FunctionContext executionContext)
    {
        return await _ediFunctions.Analyze(req, executionContext.GetLogger<IHl7Service>());
    }
}
