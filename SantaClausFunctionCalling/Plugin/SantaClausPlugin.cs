using System.ComponentModel;
using Microsoft.SemanticKernel;

namespace SantaClausFunctionCalling.Plugin;

public class SantaClausPlugin
{
    private readonly ILogger<SantaClausPlugin> _logger;

    public SantaClausPlugin(ILogger<SantaClausPlugin> logger)
    {
        _logger = logger;
    }

    [KernelFunction("IsOnTheNaughtyList")]
    [Description("Restituisce se una persona è nella lista dei cattivi di Babbo Natale.")]
    public async Task<string> IsOnTheNaughtyList([Description("il nome della persona da verificare")] string personName)
    {
        _logger.LogInformation("CHIAMATO: is_on_the_naughty_list con personName = {PersonName}", personName);
        
        if(personName.ToLower().Equals("andrea"))
        {
            _logger.LogInformation("RESULT is_on_the_naughty_list: true");
            return $"{personName} è nella lista dei cattivi.";
        }
        _logger.LogInformation("RESULT is_on_the_naughty_list: false");
        return $"{personName} non è nella lista dei cattivi.";
    }
}