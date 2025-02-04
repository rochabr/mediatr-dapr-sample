using Carter;
using Carter.OpenApi;
using MediatR;
using Shared.Contracts.Models;
using Subscriber.API.Commands;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Dapr;

namespace Subscriber.API.Modules;

public class MessagesModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        // Create the route handler
        var handler = async (Employee employee, IMediator mediator, ILogger<MessagesModule> logger) =>
        {
            try 
            {
                logger.LogInformation("Received message - Employee: {Id}, {Name}", 
                    employee.Id, 
                    employee.Name);
                    
                await mediator.Send(new ProcessEmployeeCommand(employee));
                return Results.Ok();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error processing message");
                return Results.StatusCode(500);
            }
        };

        // Map the route for both HTTP and Dapr
        app.MapPost("/messages", handler)
           .WithTopic("redis-pubsub", "employeemessage")  // This enables Dapr subscription
           .WithName("ProcessMessage")
           .WithTags("Messages")
           .WithOpenApi();
    }
}