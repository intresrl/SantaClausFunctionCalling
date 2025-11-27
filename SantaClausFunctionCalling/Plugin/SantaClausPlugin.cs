using System.ComponentModel;
using System.Text.Json;
using Microsoft.SemanticKernel;
using SantaClausFunctionCalling.Models;

namespace SantaClausFunctionCalling.Plugin;

public class SantaClausPlugin
{
    private readonly ILogger<SantaClausPlugin> _logger;
    private const string NaughtyListFilePath = "naughtyList.json";
    private const string NiceListFilePath = "niceList.json";
    private const string SantaLetterFilePath = "santaLetter.json";

    public SantaClausPlugin(ILogger<SantaClausPlugin> logger)
    {
        _logger = logger;
        InitializeJsonFiles();
    }
    
    private void InitializeJsonFiles()
    {
        if (!File.Exists(NaughtyListFilePath))
        {
            var naughtyListData = new NaughtyListData();
            File.WriteAllText(NaughtyListFilePath, JsonSerializer.Serialize(naughtyListData, new JsonSerializerOptions { WriteIndented = true }));
        }
        
        if (!File.Exists(NiceListFilePath))
        {
            var niceListData = new NiceListData();
            File.WriteAllText(NiceListFilePath, JsonSerializer.Serialize(niceListData, new JsonSerializerOptions { WriteIndented = true }));
        }
        
        if (!File.Exists(SantaLetterFilePath))
        {
            var santaLetterData = new SantaLetterData();
            File.WriteAllText(SantaLetterFilePath, JsonSerializer.Serialize(santaLetterData, new JsonSerializerOptions { WriteIndented = true }));
        }
    }
    
    [KernelFunction("AddPersonToNaughtyList")]
    [Description("Aggiunge una persona alla lista dei cattivi di Babbo Natale.")]
    public async Task<string> AddPersonToNaughtyList([Description("il nome della persona da aggiungere alla lista dei cattivi")] string personName)
    {
        _logger.LogInformation("CHIAMATO: AddPersonToNaughtyList con personName = {PersonName}", personName);
        
        try
        {
            var jsonContent = await File.ReadAllTextAsync(NaughtyListFilePath);
            var naughtyListData = JsonSerializer.Deserialize<NaughtyListData>(jsonContent) ?? new NaughtyListData();
            
            if (!naughtyListData.NaughtyList.Contains(personName))
            {
                naughtyListData.NaughtyList.Add(personName);
                await File.WriteAllTextAsync(NaughtyListFilePath, JsonSerializer.Serialize(naughtyListData, new JsonSerializerOptions { WriteIndented = true }));
                _logger.LogInformation("RESULT AddPersonToNaughtyList: {PersonName} aggiunto con successo", personName);
                return $"{personName} è stato aggiunto con successo alla lista dei cattivi.";
            }
            
            _logger.LogInformation("RESULT AddPersonToNaughtyList: {PersonName} già presente", personName);
            return $"{personName} è già presente nella lista dei cattivi.";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore durante l'aggiunta di {PersonName} alla lista dei cattivi", personName);
            return $"Errore durante l'aggiunta di {personName} alla lista dei cattivi.";
        }
    }
    
    [KernelFunction("AddPersonToNiceList")]
    [Description("Aggiunge una persona alla lista dei buoni di Babbo Natale.")]
    public async Task<string> AddPersonToNiceList([Description("il nome della persona da aggiungere alla lista dei buoni")] string personName)
    {
        _logger.LogInformation("CHIAMATO: AddPersonToNiceList con personName = {PersonName}", personName);
        
        try
        {
            var jsonContent = await File.ReadAllTextAsync(NiceListFilePath);
            var niceListData = JsonSerializer.Deserialize<NiceListData>(jsonContent) ?? new NiceListData();
            
            if (!niceListData.NiceList.Contains(personName))
            {
                niceListData.NiceList.Add(personName);
                await File.WriteAllTextAsync(NiceListFilePath, JsonSerializer.Serialize(niceListData, new JsonSerializerOptions { WriteIndented = true }));
                _logger.LogInformation("RESULT AddPersonToNiceList: {PersonName} aggiunto con successo", personName);
                return $"{personName} è stato aggiunto con successo alla lista dei buoni.";
            }
            
            _logger.LogInformation("RESULT AddPersonToNiceList: {PersonName} già presente", personName);
            return $"{personName} è già presente nella lista dei buoni.";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore durante l'aggiunta di {PersonName} alla lista dei buoni", personName);
            return $"Errore durante l'aggiunta di {personName} alla lista dei buoni.";
        }
    }
    
    [KernelFunction("AddGiftToSantaLetter")]
    [Description("Aggiunge un regalo alla letterina di Babbo Natale con la quantità desiderata e il destinatario.")]
    public async Task<string> AddGiftToSantaLetter(
        [Description("la quantità di regali da aggiungere")] int quantity,
        [Description("il nome del regalo da aggiungere")] string gift,
        [Description("il nome del destinatario del regalo")] string recipient)
    {
        _logger.LogInformation("CHIAMATO: AddGiftToSantaLetter con quantity = {Quantity}, gift = {Gift}, recipient = {Recipient}", quantity, gift, recipient);
        
        try
        {
            var jsonContent = await File.ReadAllTextAsync(SantaLetterFilePath);
            var santaLetterData = JsonSerializer.Deserialize<SantaLetterData>(jsonContent) ?? new SantaLetterData();
            
            var newItem = new SantaLetterItem
            {
                Quantity = quantity,
                Gift = gift,
                Recipient = recipient
            };
            
            santaLetterData.SantaLetter.Add(newItem);
            await File.WriteAllTextAsync(SantaLetterFilePath, JsonSerializer.Serialize(santaLetterData, new JsonSerializerOptions { WriteIndented = true }));
            
            _logger.LogInformation("RESULT AddGiftToSantaLetter: {Quantity} {Gift} per {Recipient} aggiunto con successo", quantity, gift, recipient);
            return $"{quantity} {gift} per {recipient} è stato aggiunto con successo alla letterina di Babbo Natale.";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore durante l'aggiunta del regalo alla letterina");
            return $"Errore durante l'aggiunta del regalo alla letterina di Babbo Natale.";
        }
    }
}