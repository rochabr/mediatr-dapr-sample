using MediatR;
using Shared.Contracts.Models;

namespace Subscriber.API.Commands
{
    public record ProcessEmployeeCommand(Employee Employee) : IRequest;
}