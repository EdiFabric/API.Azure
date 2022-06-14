﻿using Microsoft.Azure.Functions.Worker.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace EdiFabric.Api.Azure.Functions
{
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
}
