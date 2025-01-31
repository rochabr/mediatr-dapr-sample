# Publisher-Subscriber Demo with MediatR and Dapr

This solution demonstrates a publisher-subscriber pattern using .NET, MediatR, and Dapr with Redis pub/sub. It consists of two services:

- Publisher.API: Sends messages via HTTP and Dapr pub/sub
- Subscriber.API: Receives messages through both HTTP endpoints and Dapr subscriptions

## Prerequisites

- .NET 8.0 or later
- Docker
- Dapr 1.14.4 or later

## Project Structure

```text
Publisher-Subscriber-Demo/
├── Publisher.API/
├── Subscriber.API/
├── Shared.Contracts/
└── components/
    └── redis-pubsub.yaml
```

## Running the Solution

1. Start the Subscriber API:

```bash
cd Subscriber.API
dapr run --app-id subscriber --app-port 5002 --dapr-http-port 3502 --resources-path ./../components -- dotnet run
```

2. In a new terminal, start the Publisher API:

```bash
cd Publisher.API
dapr run --app-id publisher --app-port 5001 --dapr-http-port 3501 --resources-path ./../components -- dotnet run
```

## Testing

1. Test the direct HTTP endpoint:

```bash
curl -X POST http://localhost:5001/employees \
-H "Content-Type: application/json" \
-d '{"id": 1, "name": "John Doe"}'
```

2. Test the Dapr pub/sub endpoint:

```bash
curl -X POST http://localhost:5001/daprpublisher/publish \
-H "Content-Type: application/json" \
-d '{"id": 2, "name": "Jane Doe"}'
```

## Architecture Notes

1. The solution uses:
   - MediatR for command handling
   - Dapr for pub/sub messaging
   - Redis as the pub/sub broker
   - Declarative subscriptions for Dapr topics

2. Key endpoints:
   - `POST /employees` - Direct HTTP endpoint
   - `POST /daprpublisher/publish` - Dapr pub/sub endpoint
   - `POST /messages` - Subscriber endpoint (receives both HTTP and Dapr messages)

3. Common issues:

   - Ensure the components directory path is correct
   - Verify Redis is running and accessible
   - Check both services are running with Dapr
   - Ensure the subscription.yaml file is in the components directory