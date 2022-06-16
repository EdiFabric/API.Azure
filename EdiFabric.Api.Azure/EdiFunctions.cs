using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace EdiFabric.Api.Azure
{
    public class EdiFunctions
    {
        IEdiService _ediService;
        private readonly string _apiKey = "Ocp-Apim-Subscription-Key";
        private readonly string _noApiKey = "No Ocp-Apim-Subscription-Key in header.";
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

            if (!req.Headers.TryGetValues(_apiKey, out IEnumerable<string> apiKeys) || apiKeys.FirstOrDefault() == null)
            {
                logger.LogError(_noApiKey);
                return await req.BuildErrorResponse(HttpStatusCode.BadRequest, _noApiKey);
            }

            try
            {
                var res = req.CreateResponse(HttpStatusCode.OK);
                res.Headers.Add("Content-Type", "application/json; charset=utf-8");
                await _ediService.ReadAsync(req.Body, res.Body, apiKeys.First(), req.GetReadParams());
                return res;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
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

            if (!req.Headers.TryGetValues(_apiKey, out IEnumerable<string> apiKeys) || apiKeys.FirstOrDefault() == null)
            {
                logger.LogError(_noApiKey);
                return await req.BuildErrorResponse(HttpStatusCode.BadRequest, _noApiKey);
            }

            try
            {
                var res = req.CreateResponse(HttpStatusCode.OK);
                var writeParams = req.GetWriteParams();
                res.Headers.Add("Content-Type", writeParams.ContentType);
                await _ediService.WriteAsync(req.Body, res.Body, apiKeys.First(), writeParams);
                return res;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
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

            if (!req.Headers.TryGetValues(_apiKey, out IEnumerable<string> apiKeys) || apiKeys.FirstOrDefault() == null)
            {
                logger.LogError(_noApiKey);
                return await req.BuildErrorResponse(HttpStatusCode.BadRequest, _noApiKey);
            }

            try
            {
                var res = req.CreateResponse(HttpStatusCode.OK);
                res.Headers.Add("Content-Type", "application/json; charset=utf-8");
                await _ediService.ValidateAsync(req.Body, res.Body, apiKeys.First(), req.GetValidateParams());
                return res;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
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

            if (!req.Headers.TryGetValues(_apiKey, out IEnumerable<string> apiKeys) || apiKeys.FirstOrDefault() == null)
            {
                logger.LogError(_noApiKey);
                return await req.BuildErrorResponse(HttpStatusCode.BadRequest, _noApiKey);
            }

            try
            {
                var res = req.CreateResponse(HttpStatusCode.OK);
                res.Headers.Add("Content-Type", "application/json; charset=utf-8");
                await _ediService.GenerateAckAsync(req.Body, res.Body, apiKeys.First(), req.GetAckParams());
                return res;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return await req.BuildErrorResponse(ex);
            }
        }        
    }
}
