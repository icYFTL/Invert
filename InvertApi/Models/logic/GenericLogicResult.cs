using System.Net;

namespace InvertApi.Models.logic;

public class GenericLogicResult
{
    public virtual bool Status { get; init; }
    public virtual HttpStatusCode StatusCode { get; init; }
    public virtual object? Result { get; init; }
    public virtual Exception? Exception { get; init; }
}