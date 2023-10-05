using System.Net;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using EdiFabric.Api;
using EdiFabric.Api.Azure;
using EdiFabric;

public class EdiFunctions
{
    IEdiService _ediService;
    private readonly string _noData = "No data in request body.";

    public EdiFunctions(IEdiService ediService)
    {
        _ediService = ediService;
    }

    public async Task<HttpResponseData> Read(HttpRequestData req, ILogger logger)
    {
        if (req.Body == null || req.Body.Length == 0)
        {
            logger.LogError(_noData);
            return await req.BuildErrorResponse(HttpStatusCode.BadRequest, _noData);
        }

        try
        {
            var apiKey = GetApiKey(req);
            SerialKey.Set(apiKey);
            //  Uncomment and then comment the line above if you wish to use distributed cache for tokens
            //  BlobCache.Set(apiKey);
            var res = req.CreateResponse(HttpStatusCode.OK);
            res.Headers.Add("Content-Type", "application/json; charset=utf-8");
            await _ediService.ReadAsync(req.Body, res.Body, apiKey, req.GetReadParams());
            return res;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.ToString());
            return await req.BuildErrorResponse(ex);
        }
    }

    public async Task<HttpResponseData> Write(HttpRequestData req, ILogger logger)
    {
        if (req.Body == null || req.Body.Length == 0)
        {
            logger.LogError(_noData);
            return await req.BuildErrorResponse(HttpStatusCode.BadRequest, _noData);
        }

        try
        {
            var apiKey = GetApiKey(req);
            SerialKey.Set(apiKey);
            //  Uncomment and then comment the line above if you wish to use distributed cache for tokens
            //  BlobCache.Set(apiKey);
            var res = req.CreateResponse(HttpStatusCode.OK);
            var writeParams = req.GetWriteParams();
            res.Headers.Add("Content-Type", writeParams.ContentType);
            await _ediService.WriteAsync(req.Body, res.Body, apiKey, writeParams);
            return res;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.ToString());
            return await req.BuildErrorResponse(ex);
        }
    }

    public async Task<HttpResponseData> Validate(HttpRequestData req, ILogger logger)
    {
        if (req.Body == null || req.Body.Length == 0)
        {
            logger.LogError(_noData);
            return await req.BuildErrorResponse(HttpStatusCode.BadRequest, _noData);
        }

        try
        {
            var apiKey = GetApiKey(req);
            SerialKey.Set(apiKey);
            //  Uncomment and then comment the line above if you wish to use distributed cache for tokens
            //  BlobCache.Set(apiKey);
            var res = req.CreateResponse(HttpStatusCode.OK);
            res.Headers.Add("Content-Type", "application/json; charset=utf-8");
            await _ediService.ValidateAsync(req.Body, res.Body, apiKey, req.GetValidateParams());
            return res;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.ToString());
            return await req.BuildErrorResponse(ex);
        }
    }

    public async Task<HttpResponseData> Ack(HttpRequestData req, ILogger logger)
    {
        if (req.Body == null || req.Body.Length == 0)
        {
            logger.LogError(_noData);
            return await req.BuildErrorResponse(HttpStatusCode.BadRequest, _noData);
        }

        try
        {
            var apiKey = GetApiKey(req);
            SerialKey.Set(apiKey);
            //  Uncomment and then comment the line above if you wish to use distributed cache for tokens
            //  BlobCache.Set(apiKey);
            var res = req.CreateResponse(HttpStatusCode.OK);
            res.Headers.Add("Content-Type", "application/json; charset=utf-8");
            await _ediService.GenerateAckAsync(req.Body, res.Body, apiKey, req.GetAckParams());
            return res;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.ToString());
            return await req.BuildErrorResponse(ex);
        }
    }
    /// <summary>
    /// This is a system operation used only for the in-house web translator.
    /// </summary>
    public async Task<HttpResponseData> Analyze(HttpRequestData req, ILogger logger)
    {
        if (req.Body == null || req.Body.Length == 0)
        {
            logger.LogError(_noData);
            return await req.BuildErrorResponse(HttpStatusCode.BadRequest, _noData);
        }

        try
        {
            var apiKey = GetApiKey(req);
            SerialKey.Set(apiKey);
            //  Uncomment and then comment the line above if you wish to use distributed cache for tokens
            //  BlobCache.Set(apiKey);
            var res = req.CreateResponse(HttpStatusCode.OK);
            res.Headers.Add("Content-Type", "application/json; charset=utf-8");
            await _ediService.AnalyzeAsync(req.Body, res.Body, apiKey, req.GetAnalyzeParams());
            return res;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.ToString());
            return await req.BuildErrorResponse(ex);
        }
    }

    private string GetApiKey(HttpRequestData req)
    {
        if (req.Headers.TryGetValues("Ocp-Apim-Subscription-Key", out var apiKeys) && apiKeys.FirstOrDefault() != null)
            return apiKeys.First();

        return Configuration.ApiKey;
    }
}
