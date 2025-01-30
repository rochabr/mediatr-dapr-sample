// Publisher.API/Controllers/DaprPublisherController.cs
using Microsoft.AspNetCore.Mvc;
using Dapr.Client;
using Shared.Contracts.Models;

namespace Publisher.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DaprPublisherController : ControllerBase
    {
        private readonly DaprClient _daprClient;
        private readonly ILogger<DaprPublisherController> _logger;
        private const string PUBSUB_NAME = "redis-pubsub";
        private const string TOPIC_NAME = "employeemessage";

        public DaprPublisherController(DaprClient daprClient, ILogger<DaprPublisherController> logger)
        {
            _daprClient = daprClient;
            _logger = logger;
        }

        [HttpPost("publish")]
        public async Task<IActionResult> PublishMessage([FromBody] Employee employee)
        {
            try
            {
                await _daprClient.PublishEventAsync(PUBSUB_NAME, TOPIC_NAME, employee);
                _logger.LogInformation("Published employee message for Id: {Id}, Name: {Name}", 
                    employee.Id, employee.Name);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error publishing message");
                return StatusCode(500, ex.Message);
            }
        }
    }
}