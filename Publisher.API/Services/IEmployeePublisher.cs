using Shared.Contracts.Models;

namespace Publisher.API.Services
{
    public interface IEmployeePublisher
    {
        Task PublishEmployeeAsync(Employee employee);
    }
}