// Simple A2A Host for Azure AI Foundry Agent
using A2A;
using Azure.AI.Agents.Persistent;
using Azure.Identity;
using Microsoft.Agents.AI;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpClient().AddLogging();

var app = builder.Build();

// Configuration
string endpoint = builder.Configuration["AZURE_FOUNDRY_PROJECT_ENDPOINT"]
    ?? throw new InvalidOperationException("AZURE_FOUNDRY_PROJECT_ENDPOINT is required");
string agentId = builder.Configuration["AZURE_FOUNDRY_AGENT_ID"]
    ?? throw new InvalidOperationException("AZURE_FOUNDRY_AGENT_ID is required");

// Create the Foundry agent
var persistentAgentsClient = new PersistentAgentsClient(endpoint, new AzureCliCredential());
PersistentAgent persistentAgent = await persistentAgentsClient.Administration.GetAgentAsync(agentId);

AIAgent agent = await persistentAgentsClient.GetAIAgentAsync(persistentAgent.Id);

// Define the agent card for A2A discovery
var agentCard = new AgentCard
{
    Name = persistentAgent.Name ?? "Foundry-MCP-Microsoft-Learn",
    Description = persistentAgent.Description ?? "Microsoft Learn Agent with A2A",
    Version = "1.0.0",
    Capabilities = new AgentCapabilities
    {
        Streaming = true
    }
};

// Map A2A endpoints
app.MapA2A(agent, path: "/", agentCard);

Console.WriteLine($"A2A Server running with agent: {agentCard.Name}");
Console.WriteLine("Endpoints:");
Console.WriteLine("  - Agent Card: /v1/card");
Console.WriteLine("  - A2A Messages: /");

await app.RunAsync();
