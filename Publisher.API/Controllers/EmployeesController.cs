using Microsoft.AspNetCore.Mvc;
using Publisher.API.Services;
using Shared.Contracts.Models;

namespace Publisher.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeePublisher _publisher;

        public EmployeesController(IEmployeePublisher publisher)
        {
            _publisher = publisher;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Employee employee)
        {
            await _publisher.PublishEmployeeAsync(employee);
            return Ok();
        }
    }
}
