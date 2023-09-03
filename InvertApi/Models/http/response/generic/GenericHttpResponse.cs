using InvertApi.Models.logic;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace InvertApi.Models.http.response.generic;

public class GenericHttpResponse : ObjectResult
{
    public GenericHttpResponse(bool status, dynamic? response, int statusCode) : base(
        JsonConvert.SerializeObject(
            new
            {
                Status = status, Response = response is string ? new { Message = response } : (object?)response
            }, Formatting.Indented))
    {
        StatusCode = statusCode;
    }

    public static GenericHttpResponse FromLogicResult(GenericLogicResult result)
    {
        return new GenericHttpResponse(result.Status, result.Result, (int)result.StatusCode);
    }
}