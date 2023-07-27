using Microsoft.Azure.Functions.Worker.Http;
using System.Net;

static class ErrorHandler
{
    public static async Task<HttpResponseData> BuildErrorResponse(this HttpRequestData req, HttpStatusCode statusCode, string message)
    {
        var response = req.CreateResponse();
        await response.WriteAsJsonAsync(new
        {
            Code = (int)statusCode,
            Details = new List<string> { message }
        }, statusCode);
        return response;
    }

    public static async Task<HttpResponseData> BuildErrorResponse(this HttpRequestData req, Exception ex)
    {
        var statusCode = ex is InvalidDataException ? HttpStatusCode.BadRequest : HttpStatusCode.InternalServerError;
        var response = req.CreateResponse();
        await response.WriteAsJsonAsync(new
        {
            Code = (int)statusCode,
            Details = new List<string> { ex.Message }
        }, statusCode);
        return response;
    }
}

