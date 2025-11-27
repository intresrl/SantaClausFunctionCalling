namespace SantaClausFunctionCalling.Models;

public class AgentResponse
{
    public string AgentResponseText { get; set; } = string.Empty;
    public List<string> NaughtyList { get; set; } = new();
    public List<string> NiceList { get; set; } = new();
    public List<SantaLetterItem> SantaLetter { get; set; } = new();
}


