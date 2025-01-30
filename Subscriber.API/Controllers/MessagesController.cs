using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Contracts.Models;
using Subscriber.API.Commands;
using Dapr;

namespace Subscriber.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MessagesController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<MessagesController> _logger;

        public MessagesController(IMediator mediator, ILogger<MessagesController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost("/messages")]
        [Topic("redis-pubsub", "employeemessage")] // This makes the endpoint subscribe to Dapr messages
        public async Task<IActionResult> Post([FromBody] Employee employee)
        {
            try 
            {
                _logger.LogInformation("Received employee message - Id: {Id}, Name: {Name}", 
                    employee.Id, employee.Name);
                    
                await _mediator.Send(new ProcessEmployeeCommand(employee));
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing employee message");
                return StatusCode(500, ex.Message);
            }
        }
    }
}