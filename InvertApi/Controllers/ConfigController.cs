using InvertApi.Commands;
using InvertApi.Extensions;
using InvertApi.Models.http.response.generic;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace InvertApi.Controllers;

[ApiController]
[Route("/config")]
public class ConfigController
{
    #region SpecialData

    public class ParseRequest
    {
        public string Data { get; init; }
    }
    
    public class RemoveDuplicatesRequest : ParseRequest
    {
    }
    
    public class SaveRequest : ParseRequest
    {
    }
    
    public class FixRequest : ParseRequest
    {
        public int Level { get; init; }
    }

    #endregion
    private readonly ConfigLogic _configLogic;

    public ConfigController(IServiceScopeFactory scopeFactory)
    {
        var scope = scopeFactory.CreateScope();
        _configLogic = scope.ServiceProvider.GetRequiredService<ConfigLogic>();
    }
    
    [HttpGet]
    [Route("")]
    public async Task<IActionResult> OnRootAsync()
    {
        return new SuccessHttpResponse();
    }

    [HttpPost]
    [Route("parse")]
    public async Task<IActionResult> OnParseAsync([FromBody] ParseRequest request)
    {
        try
        {
            var data = request.Data.FromBase64();
            var result = await _configLogic.ParseAsync(data);
            return GenericHttpResponse.FromLogicResult(result);
        }
        catch
        {
            return new FailedHttpResponse
            {
                StatusCode = 400
            };
        }
    }
    
    [HttpPost]
    [Route("fix")]
    public async Task<IActionResult> OnFixAsync([FromBody] FixRequest request)
    {
        try
        {
            var data = JsonConvert.DeserializeObject<List<BaseCommand>>(request.Data.FromBase64());
            if (data is null)
            {
                return new FailedHttpResponse
                {
                    StatusCode = 400
                };
            }
            var result = await _configLogic.FixAsync(request.Level, data);
            return GenericHttpResponse.FromLogicResult(result);
        }
        catch
        {
            return new FailedHttpResponse
            {
                StatusCode = 400
            };
        }
    }
    
    [HttpPost]
    [Route("remove_duplicates")]
    public async Task<IActionResult> OnRemoveDuplicatesAsync([FromBody] RemoveDuplicatesRequest request)
    {
        try
        {
            var data = JsonConvert.DeserializeObject<List<BaseCommand>>(request.Data.FromBase64());
            if (data is null)
            {
                return new FailedHttpResponse
                {
                    StatusCode = 400
                };
            }
            var result = await _configLogic.RemoveDuplicatesAsync(data);
            return GenericHttpResponse.FromLogicResult(result);
        }
        catch
        {
            return new FailedHttpResponse
            {
                StatusCode = 400
            };
        }
    }
}