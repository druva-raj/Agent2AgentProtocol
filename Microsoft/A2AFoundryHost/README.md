# A2A Foundry Agent Host

A simple ASP.NET Core application that hosts an Azure AI Foundry agent via the A2A (Agent-to-Agent) protocol.

## Prerequisites

- .NET 9 SDK
- Azure AI Foundry project with a deployed agent
- Azure CLI authenticated (`az login`)

## Configuration

Set the following environment variables:

```powershell
$env:AZURE_FOUNDRY_PROJECT_ENDPOINT = "https://your-project.services.ai.azure.com/api/projects/your-project"
$env:AZURE_FOUNDRY_AGENT_ID = "your-agent-id"
```

## Run

```bash
dotnet run --urls "http://localhost:5000"
```

## Endpoints

- `/v1/card` - Agent card discovery
- `/` - A2A message endpoint (JSON-RPC)

## Test with curl

```bash
# Get agent card
curl http://localhost:5000/v1/card

# Send a message
curl -X POST http://localhost:5000 \
  -H "Content-Type: application/json" \
  -d '{
    "jsonrpc": "2.0",
    "id": "1",
    "method": "message/send",
    "params": {
      "message": {
        "role": "user",
        "parts": [{"kind": "text", "text": "Hello!"}]
      }
    }
  }'
```
