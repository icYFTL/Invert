using System.Net;

namespace InvertApi.Models.logic;

public class FailedLogicResult : GenericLogicResult
{
    public override bool Status => false;
    public override HttpStatusCode StatusCode { get; init; } = HttpStatusCode.InternalServerError;
}