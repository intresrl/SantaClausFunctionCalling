using SantaClausFunctionCalling.Models;

namespace SantaClausFunctionCalling.Services.Abstract;

public interface IAgentService
{
    Task<AgentResponse> Agent(AgentRequest agentRequest);
    Task<ListSummaryResponse> GetSantaListsAsync();
}