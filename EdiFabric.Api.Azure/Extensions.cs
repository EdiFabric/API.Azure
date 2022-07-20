using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Azure.Functions.Worker.Http;
using EdiFabric.Api;

internal static class Extensions
{
    public static ReadParams GetReadParams(this HttpRequestData req)
    {        
        var result = new ReadParams();

        if (req.Url != null && !string.IsNullOrEmpty(req.Url.Query))
        {
            var queryDictionary = QueryHelpers.ParseQuery(req.Url.Query);

            var coe = queryDictionary.GetValueOrDefault("continueOnError").ToString();
            if (!string.IsNullOrEmpty(coe) && bool.TryParse(coe, out bool continueOnError))
            {
                result.ContinueOnError = continueOnError;
            }

            var charSet = queryDictionary.GetValueOrDefault("charSet").ToString();
            if (!string.IsNullOrEmpty(charSet))
            {
                result.CharSet = charSet;
            }

            var es3 = queryDictionary.GetValueOrDefault("eancomS3").ToString();
            if (!string.IsNullOrEmpty(es3) && bool.TryParse(es3, out bool eancomS3))
            {
                result.EancomS3IsDefault = eancomS3;
            }

            var inv = queryDictionary.GetValueOrDefault("ignoreNullValues").ToString();
            if (!string.IsNullOrEmpty(inv) && bool.TryParse(inv, out bool ignoreNullValues))
            {
                result.IgnoreNullValues = ignoreNullValues;
            }

            var model = queryDictionary.GetValueOrDefault("model").ToString();
            if (!string.IsNullOrEmpty(model))
            {
                result.Model = model;
            }
        }

        return result;
    }

    public static WriteParams GetWriteParams(this HttpRequestData req)
    {
        var result = new WriteParams();

        if (req.Url != null && !string.IsNullOrEmpty(req.Url.Query))
        {
            var queryDictionary = QueryHelpers.ParseQuery(req.Url.Query);

            var pw = queryDictionary.GetValueOrDefault("preserveWhitespace").ToString();
            if (!string.IsNullOrEmpty(pw) && bool.TryParse(pw, out bool preserveWhitespace))
            {
                result.PreserveWhitespace = preserveWhitespace;
            }

            result.ContentType = "application/octet-stream; charset=utf-8";
            var charSet = queryDictionary.GetValueOrDefault("charSet").ToString();
            if (!string.IsNullOrEmpty(charSet))
            {
                result.CharSet = charSet;
                result.ContentType = $"application/octet-stream; charset={charSet}";
            }

            var postFix = queryDictionary.GetValueOrDefault("postfix").ToString();
            if (!string.IsNullOrEmpty(postFix))
            {
                result.Postfix = postFix;
            }

            var es3 = queryDictionary.GetValueOrDefault("eancomS3").ToString();
            if (!string.IsNullOrEmpty(es3) && bool.TryParse(es3, out bool eancomS3))
            {
                result.EancomS3IsDefault = eancomS3;
            }

            var ng = queryDictionary.GetValueOrDefault("noG1").ToString();
            if (!string.IsNullOrEmpty(ng) && bool.TryParse(ng, out bool noG1))
            {
                result.NoG1 = noG1;
            }

            var trailerMessage = queryDictionary.GetValueOrDefault("trailerMessage").ToString();
            if (!string.IsNullOrEmpty(trailerMessage))
            {
                result.NcpdpTrailerMessage = trailerMessage;
            }
        }

        return result;
    }

    public static ValidateParams GetValidateParams(this HttpRequestData req)
    {
        var result = new ValidateParams();

        if (req.Url != null && !string.IsNullOrEmpty(req.Url.Query))
        {
            var queryDictionary = QueryHelpers.ParseQuery(req.Url.Query);

            var st = queryDictionary.GetValueOrDefault("skipTrailer").ToString();
            if (!string.IsNullOrEmpty(st) && bool.TryParse(st, out bool skipTrailer))
            {
                result.SkipTrailerValidation = skipTrailer;
            }

            var decimalPoint = queryDictionary.GetValueOrDefault("decimalPoint").ToString();
            if (!string.IsNullOrEmpty(decimalPoint))
            {
                result.DecimalPoint = decimalPoint == "." ? '.' : ',';
            }

            var syntaxSet = queryDictionary.GetValueOrDefault("syntaxSet").ToString();
            if (!string.IsNullOrEmpty(syntaxSet))
            {
                result.SyntaxSet = syntaxSet;
            }

            var so = queryDictionary.GetValueOrDefault("structureOnly").ToString();
            if (!string.IsNullOrEmpty(so) && bool.TryParse(so, out bool structureOnly))
            {
                result.StructureOnly = structureOnly;
            }

            var es3 = queryDictionary.GetValueOrDefault("eancomS3").ToString();
            if (!string.IsNullOrEmpty(es3) && bool.TryParse(es3, out bool eancomS3))
            {
                result.EancomS3IsDefault = eancomS3;
            }

            var bs = queryDictionary.GetValueOrDefault("basicSyntax").ToString();
            if (!string.IsNullOrEmpty(bs) && bool.TryParse(bs, out bool basicSyntax))
            {
                result.BasicSyntax = basicSyntax;
            }
        }

        return result;
    }

    public static AckParams GetAckParams(this HttpRequestData req)
    {
        var result = new AckParams();

        if (req.Url != null && !string.IsNullOrEmpty(req.Url.Query))
        {
            var queryDictionary = QueryHelpers.ParseQuery(req.Url.Query);

            var syntaxSet = queryDictionary.GetValueOrDefault("syntaxSet").ToString();
            if (!string.IsNullOrEmpty(syntaxSet))
            {
                result.SyntaxSet = syntaxSet;
            }

            var dd = queryDictionary.GetValueOrDefault("detectDuplicates").ToString();
            if (!string.IsNullOrEmpty(dd) && bool.TryParse(dd, out bool detectDuplicates))
            {
                result.DetectDuplicates = detectDuplicates;
            }

            var avm = queryDictionary.GetValueOrDefault("ackForValidTrans").ToString();
            if (!string.IsNullOrEmpty(avm) && bool.TryParse(avm, out bool ackForValidTrans))
            {
                result.GenerateForValidMessages = ackForValidTrans;
            }

            var mcn = queryDictionary.GetValueOrDefault("tranRefNumber").ToString();
            if (!string.IsNullOrEmpty(mcn) && int.TryParse(mcn, out int tranRefNumber))
            {
                result.MessageControlNumber = tranRefNumber;
            }

            var technicalAck = queryDictionary.GetValueOrDefault("technicalAck").ToString();
            if (!string.IsNullOrEmpty(technicalAck))
            {
                result.TechnicalAck = technicalAck;
            }

            var es3 = queryDictionary.GetValueOrDefault("eancomS3").ToString();
            if (!string.IsNullOrEmpty(es3) && bool.TryParse(es3, out bool eancomS3))
            {
                result.EancomS3IsDefault = eancomS3;
            }

            var ba = queryDictionary.GetValueOrDefault("batchAcks").ToString();
            if (!string.IsNullOrEmpty(ba) && bool.TryParse(ba, out bool batchAcks))
            {
                result.BatchAcks = batchAcks;
            }

            var irn = queryDictionary.GetValueOrDefault("interchangeRefNumber").ToString();
            if (!string.IsNullOrEmpty(irn) && int.TryParse(irn, out int interchangeRefNumber))
            {
                result.InterchangeControlNumber = interchangeRefNumber;
            }

            var ack = queryDictionary.GetValueOrDefault("ack").ToString();
            if (!string.IsNullOrEmpty(ack))
            {
                result.AckVersion = ack;
            }

            var ak9isP = queryDictionary.GetValueOrDefault("ak901isP").ToString();
            if (!string.IsNullOrEmpty(ak9isP) && bool.TryParse(ak9isP, out bool ak901isP))
            {
                result.Ak901ShouldBeP = ak901isP;
            }
        }

        return result;
    }
}
