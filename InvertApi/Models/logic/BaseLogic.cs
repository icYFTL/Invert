namespace InvertApi.Models.logic;

public abstract class BaseLogic
{
    protected readonly IServiceScope Scope;
    protected readonly ILogger? Logger;
    protected BaseLogic(IServiceScopeFactory scopeFactory)
    {
        Scope = scopeFactory.CreateScope();
        Logger = Scope.ServiceProvider.GetService<ILogger>();
    }
}