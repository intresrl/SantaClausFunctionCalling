using Microsoft.AspNetCore.Mvc;
using SantaClausFunctionCalling.Models;
using SantaClausFunctionCalling.Services.Abstract;

namespace SantaClausFunctionCalling.Endpoints;

public static class SantaClausEndpoints
{
    public static WebApplication MapSantaClausEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("v1/santaclaus")
            .WithTags("SantaClaus");

        group.MapGet("/hello", () => "Hello World!")
            .WithSummary("Say Hello")
            .WithDescription("Santa Claus AI says hello to you.");
        
        group.MapPost("/agent", 
            async ([FromServices] IAgentService agentService, [FromBody] AgentRequest request) =>
            {
                var response = await agentService.Agent(request);
                return Results.Ok(response);
            })
        .WithSummary("Send a message to the AI Agent")
        .WithDescription("Interact with the AI-powered Santa Claus agent.");
        

        return app;
    }
}