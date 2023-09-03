using System.Reflection;
using InvertApi.Models.http.response.generic;
using Microsoft.AspNetCore.Mvc;

namespace InvertApi.Controllers;

[ApiController]
[Route("/general")]
public class GeneralController
{
    [HttpGet]
    [Route("version")]
    public async Task<IActionResult> OnGetVersionAsync()
    {
        var version = Assembly.GetExecutingAssembly()
            .GetCustomAttributes<AssemblyInformationalVersionAttribute>()
        .Select(x => x.InformationalVersion)
        .FirstOrDefault();

        return new SuccessHttpResponse(version);
    }
}