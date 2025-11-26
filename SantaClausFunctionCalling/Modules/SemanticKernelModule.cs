using Microsoft.SemanticKernel;
using SantaClausFunctionCalling.Plugin;
using SantaClausFunctionCalling.Services.Abstract;
using SantaClausFunctionCalling.Services.Concrete;

namespace SantaClausFunctionCalling.Modules;

public class SemanticKernelModule : IModule
{
    public bool IsEnabled => true;
    public int Order => 0;
    
    public IServiceCollection Register(WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IAgentService, AgentService>();
        
        builder.Services.AddScoped<SantaClausPlugin>();
        
        var openAiApiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY") ?? 
                           throw new NullReferenceException("'OPENAI_API_KEY' not found.");

        var openAiModel = Environment.GetEnvironmentVariable("OPENAI_MODEL") ?? "gpt-4o-mini";
        
        builder.Services.AddKeyedScoped<Kernel>("Kernel", (serviceProvider, _) => 
        {
            var kernelBuilder = Kernel.CreateBuilder()
                .AddOpenAIChatCompletion(
                    modelId: openAiModel,
                    apiKey: openAiApiKey
                );
            
            var kernel = kernelBuilder.Build();
            
            var santaClausPlugin = serviceProvider.GetRequiredService<SantaClausPlugin>();
            
            kernel.Plugins.AddFromObject(santaClausPlugin, "SantaClausPlugin");
            
            return kernel;
        });
        
        return builder.Services;
    }

    public WebApplication Configure(WebApplication app)
    {
        return app;
    }
}
