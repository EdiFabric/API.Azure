using System.Net;
using System.Text;
using EdiFabric.Api.Azure;
using EdiFabric.Native.X12;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

public class EdiFunctions
{
    private readonly string _noData = "No data in request body.";

    public async Task<HttpResponseData> Read(HttpRequestData req, ILogger logger)
    {
        if (req.Body == null || req.Body.Length == 0)
        {
            logger.LogError(_noData);
            return await req.BuildErrorResponse(HttpStatusCode.BadRequest, _noData);
        }

        try
        {
            Authorize(req);
            var edi = await ReadBodyAsync(req, req.GetReadParams().CharSet);
            var result = EdiFabricX12.Parse(edi, ParseMode.Json);

            var res = req.CreateResponse(HttpStatusCode.OK);
            res.Headers.Add("Content-Type", "application/json; charset=utf-8");
            await res.WriteStringAsync(result.Transactions);
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
            Authorize(req);
            var writeParams = req.GetWriteParams();
            using var reader = new StreamReader(req.Body, Encoding.UTF8);
            var json = await reader.ReadToEndAsync();
            var edi = EdiFabricX12.Build(json, writeParams.Postfix);

            var res = req.CreateResponse(HttpStatusCode.OK);
            res.Headers.Add("Content-Type", writeParams.ToContentType());
            await res.WriteStringAsync(edi);
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
            Authorize(req);
            var edi = await ReadEdiAsync(req);
            var result = EdiFabricX12.Parse(edi, ParseMode.JsonValidate, req.GetValidateParams().ToConfig());

            var res = req.CreateResponse(HttpStatusCode.OK);
            res.Headers.Add("Content-Type", "application/json; charset=utf-8");
            await res.WriteStringAsync(result.Report);
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
            Authorize(req);
            var edi = await ReadEdiAsync(req);
            var result = EdiFabricX12.Parse(edi, ParseMode.JsonValidateAck, req.GetAckParams().ToConfig());

            var res = req.CreateResponse(HttpStatusCode.OK);
            res.Headers.Add("Content-Type", "application/json; charset=utf-8");
            await res.WriteStringAsync(result.Report);
            return res;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.ToString());
            return await req.BuildErrorResponse(ex);
        }
    }

    private static void Authorize(HttpRequestData req)
    {
        var apiKey = GetApiKey(req);
        EdiFabricX12.SetSerial(apiKey);
        //  Uncomment and then comment the line above if you wish to use distributed cache for tokens
        //  BlobCache.Set(apiKey);
    }

    private static async Task<byte[]> ReadEdiAsync(HttpRequestData req)
    {
        var contentType = req.GetContentType();
        if (contentType != null && contentType.StartsWith("application/json", StringComparison.OrdinalIgnoreCase))
        {
            using var reader = new StreamReader(req.Body, Encoding.UTF8);
            var json = await reader.ReadToEndAsync();
            return Encoding.UTF8.GetBytes(EdiFabricX12.Build(json));
        }

        return await ReadBodyAsync(req);
    }

    private static async Task<byte[]> ReadBodyAsync(HttpRequestData req, string? charSet = null)
    {
        using var buffer = new MemoryStream();
        await req.Body.CopyToAsync(buffer);
        var bytes = buffer.ToArray();

        if (string.IsNullOrEmpty(charSet))
            return bytes;

        var text = Encoding.GetEncoding(charSet).GetString(bytes);
        return Encoding.UTF8.GetBytes(text);
    }

    private static string GetApiKey(HttpRequestData req)
    {
        if (req.Headers.TryGetValues("Ocp-Apim-Subscription-Key", out var apiKeys) && apiKeys.FirstOrDefault() != null)
            return apiKeys.First();

        return Configuration.ApiKey;
    }
}
