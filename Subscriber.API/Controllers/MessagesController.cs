using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Contracts.Models;
using Subscriber.API.Commands;

namespace Subscriber.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MessagesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MessagesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Employee employee)
        {
            await _mediator.Send(new ProcessEmployeeCommand(employee));
            return Ok();
        }
    }
}
