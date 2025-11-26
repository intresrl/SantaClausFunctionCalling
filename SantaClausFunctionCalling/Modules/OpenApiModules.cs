using Microsoft.OpenApi.Models;
using Scalar.AspNetCore;

namespace SantaClausFunctionCalling.Modules;

public class OpenApiModule : IModule
{
    public bool IsEnabled => true;
    public int Order => 0;

    public IServiceCollection Register(WebApplicationBuilder builder)
    {
        builder.Services.AddOpenApi(options =>
        {
            options.AddDocumentTransformer((document, _, _) =>
            {
                document.Servers = [new OpenApiServer {Url = "/"}];
                document.Info = new OpenApiInfo
                {
                    Title = "Santa Claus AI by Memoria",
                    Version = "v1.0",
                    Description = "Santa Claus AI - API REST",
                    Contact = new OpenApiContact
                    {
                        Name = "SantaClausAI"
                    }
                };

                return Task.CompletedTask;
            });
        });

        return builder.Services;
    }

    public WebApplication Configure(WebApplication app)
    {
        app.MapOpenApi();
        app.MapScalarApiReference(options =>
        {
            options.WithTitle("Santa Claus AI API")
                .WithTheme(ScalarTheme.None);
        });

        return app;
    }
}