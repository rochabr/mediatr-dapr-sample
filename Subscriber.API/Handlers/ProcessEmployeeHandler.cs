using MediatR;
using Subscriber.API.Commands;

namespace Subscriber.API.Handlers
{
    public class ProcessEmployeeHandler : IRequestHandler<ProcessEmployeeCommand>
    {
        private readonly ILogger<ProcessEmployeeHandler> _logger;

        public ProcessEmployeeHandler(ILogger<ProcessEmployeeHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(ProcessEmployeeCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Received employee - Id: {Id}, Name: {Name}", 
                request.Employee.Id, 
                request.Employee.Name);
            
            return Task.CompletedTask;
        }
    }
}