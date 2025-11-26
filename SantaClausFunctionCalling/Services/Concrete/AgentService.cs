using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using SantaClausFunctionCalling.Models;
using SantaClausFunctionCalling.Services.Abstract;
using ChatMessageContent = Microsoft.SemanticKernel.ChatMessageContent;

namespace SantaClausFunctionCalling.Services.Concrete;

public class AgentService : IAgentService
{
    private readonly Kernel _kernel;
    private readonly IChatCompletionService _chatCompletion;
    
    private readonly OpenAIPromptExecutionSettings _openAiPromptExecutionSettings = new()
    {
        FunctionChoiceBehavior = FunctionChoiceBehavior.Auto(),
        ChatSystemPrompt = "Sei l'assistente virtuale di Babbo Natale rispondi sempre cordialmente a ciò che ti viene chiesto"+
                           "Rispondi alla richiesta di Babbo Natale utilizzando i plugin disponibili" +
                           "Utilizza solo i plugin pertinenti alla domanda e nulla di più. " +
                           "Non utilizzare i plugin se i dati che restituiscono non sono pertinenti alla domanda. In questo caso, rispondi dicendo che non puoi fornire informazioni pertinenti." +
                           "Rispondi in italiano."
    };
    
    public AgentService([FromKeyedServices("Kernel")] Kernel kernel)
    {
        _kernel = kernel;
        _chatCompletion = kernel.GetRequiredService<IChatCompletionService>();
    }
    
    public async Task<string> Agent(AgentRequest agentRequest)
    {
        ChatHistory userChat = [];
        userChat.AddUserMessage(agentRequest.Request);
        
        ChatMessageContent response = await _chatCompletion.GetChatMessageContentAsync(userChat,
            executionSettings: _openAiPromptExecutionSettings, kernel: _kernel);
        
        return response.Content ?? "";
    }
}