using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Azure.Functions.Worker.Http;
using EdiFabric.Api.Azure.Models;

internal static class Extensions
{
    public static ReadParameters GetReadParams(this HttpRequestData req)
    {
        var result = new ReadParameters();
        var query = req.ParseQuery();
        if (query is null)
            return result;

        result.CharSet = query.GetString("charSet");

        return result;
    }

    public static WriteParameters GetWriteParams(this HttpRequestData req)
    {
        var result = new WriteParameters();
        var query = req.ParseQuery();
        if (query is null)
            return result;

        result.ContentType = query.GetString("contentType");
        result.CharSet = query.GetString("charSet");
        result.Postfix = query.GetString("postfix");

        return result;
    }

    public static ValidateParameters GetValidateParams(this HttpRequestData req)
    {
        var result = new ValidateParameters();
        var query = req.ParseQuery();
        if (query is null)
            return result;

        BindValidate(result, query);
        return result;
    }

    public static AckParameters GetAckParams(this HttpRequestData req)
    {
        var result = new AckParameters();
        var query = req.ParseQuery();
        if (query is null)
            return result;

        BindValidate(result, query);

        if (query.TryGetBool("suppressTa1", out var suppressTa1))
            result.SuppressTa1 = suppressTa1;
        if (query.TryGetBool("ak901p", out var ak901p))
            result.Ak901p = ak901p;
        if (query.TryGetBool("genForValid", out var genForValid))
            result.GenForValid = genForValid;
        if (query.TryGetBool("gen997", out var gen997))
            result.Gen997 = gen997;

        return result;
    }

    public static string? GetContentType(this HttpRequestData req)
    {
        if (req.Headers.TryGetValues("Content-Type", out var values))
            return values.FirstOrDefault();

        return null;
    }

    private static void BindValidate(ValidateParameters result, Dictionary<string, Microsoft.Extensions.Primitives.StringValues> query)
    {
        result.Regex = query.GetString("regex");
        result.DateFormat = query.GetString("dateFormat");
        result.TimeFormat = query.GetString("timeFormat");
        if (query.TryGetBool("skipSeqCount", out var skipSeqCount))
            result.SkipSeqCount = skipSeqCount;
        if (query.TryGetBool("skipHlSeq", out var skipHlSeq))
            result.SkipHlSeq = skipHlSeq;
        if (query.TryGetInt("snipLevel", out var snipLevel))
            result.SnipLevel = snipLevel;
        if (query.TryGetInt("maxErrors", out var maxErrors))
            result.MaxErrors = maxErrors;
    }

    private static Dictionary<string, Microsoft.Extensions.Primitives.StringValues>? ParseQuery(this HttpRequestData req)
    {
        if (req.Url == null || string.IsNullOrEmpty(req.Url.Query))
            return null;

        return QueryHelpers.ParseQuery(req.Url.Query)
            .ToDictionary(pair => pair.Key, pair => pair.Value, StringComparer.OrdinalIgnoreCase);
    }

    private static string? GetString(this Dictionary<string, Microsoft.Extensions.Primitives.StringValues> query, string key)
    {
        var value = query.GetValueOrDefault(key).ToString();
        return string.IsNullOrEmpty(value) ? null : value;
    }

    private static bool TryGetBool(this Dictionary<string, Microsoft.Extensions.Primitives.StringValues> query, string key, out bool value)
    {
        value = default;
        var raw = query.GetString(key);
        return !string.IsNullOrEmpty(raw) && bool.TryParse(raw, out value);
    }

    private static bool TryGetInt(this Dictionary<string, Microsoft.Extensions.Primitives.StringValues> query, string key, out int value)
    {
        value = default;
        var raw = query.GetString(key);
        return !string.IsNullOrEmpty(raw) && int.TryParse(raw, out value);
    }
}
