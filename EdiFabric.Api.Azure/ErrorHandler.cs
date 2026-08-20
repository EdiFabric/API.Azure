using System.Net;
using EdiFabric.Native.X12;
using Microsoft.Azure.Functions.Worker.Http;

static class ErrorHandler
{
    public static async Task<HttpResponseData> BuildErrorResponse(this HttpRequestData req, HttpStatusCode statusCode, string message)
    {
        var response = req.CreateResponse(statusCode);
        await response.WriteAsJsonAsync(new
        {
            Code = (int)statusCode,
            Details = new List<string> { message }
        });
        return response;
    }

    public static async Task<HttpResponseData> BuildErrorResponse(this HttpRequestData req, Exception ex)
    {
        return await req.BuildErrorResponse(StatusCodeFor(ex), ex.Message);
    }

    private static HttpStatusCode StatusCodeFor(Exception ex)
    {
        if (ex is InvalidDataException)
            return HttpStatusCode.BadRequest;

        if (ex is EdiFabricException ediEx)
        {
            return ediEx.Code is
                (int)EdiFabricErrorCode.IncorrectInput or
                (int)EdiFabricErrorCode.MapNotSet or
                (int)EdiFabricErrorCode.IncorrectMode or
                (int)EdiFabricErrorCode.ConfigDeserialization or
                (int)EdiFabricErrorCode.SplitSegmentIdMissing
                ? HttpStatusCode.BadRequest
                : HttpStatusCode.InternalServerError;
        }

        return HttpStatusCode.InternalServerError;
    }
}
