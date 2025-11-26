using SantaClausFunctionCalling.Endpoints;

namespace SantaClausFunctionCalling.Modules;

public class SantaClausModule : IModule
{
    public bool IsEnabled => true;
    public int Order => 0;

    public IServiceCollection Register(WebApplicationBuilder builder)
    {
        return builder.Services;
    }

    public WebApplication Configure(WebApplication app)
    {
        app.MapSantaClausEndpoints();
        return app;
    }
}