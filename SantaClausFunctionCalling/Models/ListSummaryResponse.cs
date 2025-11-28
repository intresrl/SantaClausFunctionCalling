namespace SantaClausFunctionCalling.Models;

public class ListSummaryResponse
{
    public List<string> NaughtyList { get; set; } = new();
    public List<string> NiceList { get; set; } = new();
    public List<SantaLetterItem> SantaLetter { get; set; } = new();
}

