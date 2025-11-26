using SantaClausFunctionCalling.Models;

namespace SantaClausFunctionCalling.Services.Abstract;

public interface IAgentService
{
    Task<string> Agent(AgentRequest agentRequest);
}