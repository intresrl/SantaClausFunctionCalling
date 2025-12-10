# 🎅 Assistente di Babbo Natale - Backend

Applicazione .NET Minimal API che funge da backend per un servizio di gestione delle liste di Babbo Natale, integrato con un agente AI tramite OpenAI e Semantic Kernel.

## 📋 Descrizione

Questo progetto è il backend di un servizio che permette di:
- Gestire la lista dei bambini cattivi
- Gestire la lista dei bambini buoni
- Gestire le letterine per Babbo Natale con regali e destinatari
- Interagire con un agente AI che utilizza **Function Calling** per modificare automaticamente le liste

L'agente AI è alimentato da **Azure OpenAI** tramite **Microsoft Semantic Kernel** e può eseguire operazioni sulle liste in modo autonomo interpretando richieste in linguaggio naturale.

## 🚀 Installazione

### Prerequisiti
- .NET 9.0 SDK
- Un account OpenAI con accesso all'API


## 💻 Avvio

```bash
dotnet run --project SantaClausFunctionCalling
```

L'API sarà disponibile su `http://localhost:5288`

## 📡 API Endpoints

### GET `/v1/santaclaus/lists`
Restituisce tutte le liste di Babbo Natale

**Risposta esempio:**
```json
{
  "naughtyList": ["Luca", "Fabio", "Roberto", "Giuseppe", "Luigi"],
  "niceList": ["Andrea", "Lorenzo", "Federico", "Mario"],
  "santaLetter": [
    {
      "quantity": 1,
      "gift": "macchinina",
      "recipient": "Andrea"
    },
    {
      "quantity": 3,
      "gift": "giochi in scatola",
      "recipient": "Federico"
    },
    {
      "quantity": 2,
      "gift": "videogiochi",
      "recipient": "Mario"
    }
  ]
}
```

### POST `/v1/santaclaus/agent`
Invia una richiesta all'agente AI che utilizza il Function Calling per eseguire operazioni

**Body richiesta:**
```json
{
  "request": "Aggiungi Mario alla lista dei buoni e Luigi alla lista dei cattivi"
}
```

**Risposta esempio:**
```json
{
  "agentResponse": "Ho! Ho! Ho! 🎅 Ho aggiunto con gioia Mario alla lista dei buoni! È sempre meraviglioso vedere bambini che si comportano bene! 🎄✨\n\nPurtroppo, Luigi è stato aggiunto alla lista dei cattivi. Spero che possa migliorare il suo comportamento prima di Natale! 🎁"
}
```

## 🎯 Function Calling & Plugin

Il progetto utilizza il **Function Calling** di OpenAI tramite Semantic Kernel. L'agente AI ha accesso a tre funzioni:

### SantaClausPlugin

#### `AddPersonToNaughtyList`
- **Descrizione:** Aggiunge una persona alla lista dei cattivi
- **Parametri:** 
  - `personName` (string): il nome della persona da aggiungere

#### `AddPersonToNiceList`
- **Descrizione:** Aggiunge una persona alla lista dei buoni
- **Parametri:**
  - `personName` (string): il nome della persona da aggiungere

#### `AddGiftToSantaLetter`
- **Descrizione:** Aggiunge un regalo alla letterina di Babbo Natale
- **Parametri:**
  - `quantity` (int): la quantità di regali da aggiungere
  - `gift` (string): il nome del regalo
  - `recipient` (string): il nome del destinatario

L'agente AI decide autonomamente quali funzioni chiamare in base alla richiesta dell'utente, grazie al **FunctionChoiceBehavior.Auto()**.

## 🎨 Caratteristiche

- **Minimal API** con .NET 9.0
- **Semantic Kernel** per l'integrazione con OpenAI
- **Function Calling automatico** per operazioni sulle liste
- **Persistenza su file JSON** per le liste
- **CORS configurato** per accettare richieste dal frontend
- **OpenAPI/Swagger** con Scalar per la documentazione
- **Modular architecture** con dependency injection

## 📁 Struttura del Progetto

```
SantaClausFunctionCalling/
  ├── Program.cs                          # Entry point dell'applicazione
  ├── appsettings.json                    # Configurazione
  ├── naughtyList.json                    # Lista dei cattivi (generata automaticamente)
  ├── niceList.json                       # Lista dei buoni (generata automaticamente)
  ├── santaLetter.json                    # Letterina di Babbo Natale (generata automaticamente)
  ├── Endpoints/
  │   └── SantaClausEndpoints.cs         # Definizione degli endpoint API
  ├── Models/
  │   ├── AgentRequest.cs                # Modello richiesta agente
  │   ├── AgentResponse.cs               # Modello risposta agente
  │   ├── ListSummaryResponse.cs         # Modello risposta liste
  │   ├── NaughtyListData.cs             # Modello lista cattivi
  │   ├── NiceListData.cs                # Modello lista buoni
  │   ├── SantaLetterData.cs             # Modello letterina
  │   └── SantaLetterItem.cs             # Modello singolo regalo
  ├── Modules/
  │   ├── CorsModule.cs                  # Configurazione CORS
  │   ├── IModule.cs                     # Interfaccia moduli
  │   ├── ModuleExtensions.cs            # Extension methods per i moduli
  │   ├── OpenApiModules.cs              # Configurazione OpenAPI
  │   ├── SantaClausModule.cs            # Modulo principale
  │   └── SemanticKernelModule.cs        # Configurazione Semantic Kernel
  ├── Plugin/
  │   └── SantaClausPlugin.cs            # Plugin con le funzioni per l'AI
  └── Services/
      ├── Abstract/
      │   └── IAgentService.cs           # Interfaccia servizio agente
      └── Concrete/
          └── AgentService.cs            # Implementazione servizio agente
```

## 🎄 Tecnologie Utilizzate

- **.NET 9.0** - Framework principale
- **ASP.NET Core Minimal API** - Per gli endpoint REST
- **Microsoft Semantic Kernel** - Orchestrazione dell'AI
- **OpenAI** - Modello GPT con Function Calling
- **DotNetEnv** - Gestione variabili d'ambiente
- **Scalar** - Documentazione API interattiva
- **System.Text.Json** - Serializzazione/deserializzazione JSON

## 🔧 Frontend

Questo backend è progettato per funzionare con un frontend React. Il frontend deve effettuare chiamate HTTP agli endpoint esposti su `http://localhost:5288`.

## 📝 Note

- I file JSON (naughtyList.json, niceList.json, santaLetter.json) vengono creati automaticamente al primo avvio se non esistono
- L'agente AI risponde sempre in italiano con un tono allegro e festoso tipico di Babbo Natale
- Il Function Calling è configurato in modalità automatica: l'AI decide autonomamente quando e quali funzioni chiamare
- Tutti i log delle chiamate alle funzioni sono disponibili nella console per debugging

## 🎁 Esempi di Richieste all'Agente

```
"Aggiungi Marco alla lista dei buoni"
"Aggiungi 2 trenini per Luca alla letterina"
"Metti Giovanni nella lista dei cattivi e aggiungi 1 pallone per Andrea"
```

L'agente interpreterà le richieste e chiamerà automaticamente le funzioni necessarie!

