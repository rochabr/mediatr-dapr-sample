// Subscriber.API/Modules/MessagesModule.cs
using Carter;
using MediatR;
using Microsoft.AspNetCore.Http;
using Shared.Contracts.Models;
using Subscriber.API.Commands;
using System.Text.Json;

namespace Subscriber.API.Modules;

public class MessagesModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var employeeHandler = async (HttpContext context, IMediator mediator, ILogger<MessagesModule> logger) =>
        {
            using var reader = new StreamReader(context.Request.Body);
            var json = await reader.ReadToEndAsync();
            
            // Parse the event
            var jsonDoc = JsonDocument.Parse(json);
            
            logger.LogInformation(json);

            // Deserialize it as Employee
            var employee = JsonSerializer.Deserialize<Employee>(jsonDoc);
            if (employee != null)   
            {
                logger.LogInformation("Received employee - Id: {Id}, Name: {Name}", 
                    employee.Id, 
                    employee.Name);
                
                await mediator.Send(new ProcessEmployeeCommand(employee));
            }

            return Results.Ok();
        };

        // Root endpoint
        app.MapGet("/", () => "Subscriber API")
           .WithTags("Home");

        // Dapr subscription endpoint - using absolute path to avoid conflicts
        app.MapGet("/dapr/subscribe", (HttpContext context) =>
        {
            var subscriptions = new[]
            {
                new
                {
                    pubsubname = "redis-pubsub",
                    topic = "employeemessage",
                    route = "/messages",
                    metadata = new Dictionary<string, string>
                    {
                        { "isRawPayload", "true" },
                        { "content-type", "application/json" }
                    }
                }
            };
            return Results.Ok(subscriptions);
        })
        .ExcludeFromDescription();  // This excludes it from OpenAPI/Swagger

        // Message handler endpoint
        app.MapPost("/messages", employeeHandler)
           .WithTags("Messages");
    }
}