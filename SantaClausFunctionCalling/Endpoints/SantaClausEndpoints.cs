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
                return Results.Ok(new
                {
                    agentResponse = response.AgentResponseText
                });
            })
        .WithSummary("Send a message to the AI Agent")
        .WithDescription("Interact with the AI-powered Santa Claus agent.");

        group.MapGet("/lists", async ([FromServices] IAgentService agentService) =>
        {
            var lists = await agentService.GetSantaListsAsync();
            return Results.Ok(lists);
        })
        .WithSummary("Ottieni gli elenchi di Babbo Natale")
        .WithDescription("Recupera lista dei cattivi, dei buoni e la letterina attuale.");
        

        return app;
    }
}