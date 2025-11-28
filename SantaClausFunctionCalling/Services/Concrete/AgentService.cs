using System.Text.Json;
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
    
    private const string NaughtyListFilePath = "naughtyList.json";
    private const string NiceListFilePath = "niceList.json";
    private const string SantaLetterFilePath = "santaLetter.json";
    
    private readonly OpenAIPromptExecutionSettings _openAiPromptExecutionSettings = new()
    {
        FunctionChoiceBehavior = FunctionChoiceBehavior.Auto(),
        ChatSystemPrompt = "Sei l'assistente virtuale di Babbo Natale e devi rispondere sempre in modo cordiale e festoso. " +
                           "Ricorda che puoi: " +
                           "1. Aggiungere persone alla lista dei cattivi " +
                           "2. Aggiungere persone alla lista dei buoni " +
                           "3. Aggiungere regali alla letterina di Babbo Natale specificando quantità, nome del regalo e destinatario " +
                           "Rispondi alla richiesta utilizzando i plugin disponibili quando necessario. " +
                           "Utilizza solo i plugin pertinenti alla domanda e nulla di più. " +
                           "Non utilizzare i plugin se i dati che restituiscono non sono pertinenti alla domanda. In questo caso, rispondi dicendo che non puoi fornire informazioni pertinenti. " +
                           "Rispondi sempre in italiano con un tono allegro e festoso tipico di Babbo Natale."
    };
    
    public AgentService([FromKeyedServices("Kernel")] Kernel kernel)
    {
        _kernel = kernel;
        _chatCompletion = kernel.GetRequiredService<IChatCompletionService>();
    }
    
    public async Task<AgentResponse> Agent(AgentRequest agentRequest)
    {
        ChatHistory userChat = [];
        userChat.AddUserMessage(agentRequest.Request);
        
        ChatMessageContent response = await _chatCompletion.GetChatMessageContentAsync(userChat,
            executionSettings: _openAiPromptExecutionSettings, kernel: _kernel);
        
        return new AgentResponse
        {
            AgentResponseText = response.Content ?? string.Empty
        };
    }
    
    public async Task<ListSummaryResponse> GetSantaListsAsync()
    {
        var naughtyList = await ReadNaughtyListAsync();
        var niceList = await ReadNiceListAsync();
        var santaLetter = await ReadSantaLetterAsync();
        return new ListSummaryResponse
        {
            NaughtyList = naughtyList,
            NiceList = niceList,
            SantaLetter = santaLetter
        };
    }
    
    private async Task<List<string>> ReadNaughtyListAsync()
    {
        try
        {
            if (!File.Exists(NaughtyListFilePath))
                return new List<string>();
                
            var jsonContent = await File.ReadAllTextAsync(NaughtyListFilePath);
            var data = JsonSerializer.Deserialize<NaughtyListData>(jsonContent);
            return data?.NaughtyList ?? new List<string>();
        }
        catch
        {
            return new List<string>();
        }
    }
    
    private async Task<List<string>> ReadNiceListAsync()
    {
        try
        {
            if (!File.Exists(NiceListFilePath))
                return new List<string>();
                
            var jsonContent = await File.ReadAllTextAsync(NiceListFilePath);
            var data = JsonSerializer.Deserialize<NiceListData>(jsonContent);
            return data?.NiceList ?? new List<string>();
        }
        catch
        {
            return new List<string>();
        }
    }
    
    private async Task<List<SantaLetterItem>> ReadSantaLetterAsync()
    {
        try
        {
            if (!File.Exists(SantaLetterFilePath))
                return new List<SantaLetterItem>();
                
            var jsonContent = await File.ReadAllTextAsync(SantaLetterFilePath);
            var data = JsonSerializer.Deserialize<SantaLetterData>(jsonContent);
            return data?.SantaLetter ?? new List<SantaLetterItem>();
        }
        catch
        {
            return new List<SantaLetterItem>();
        }
    }
}