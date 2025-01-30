using System.Net.Http.Json;
using Shared.Contracts.Models;

namespace Publisher.API.Services
{
    public class EmployeePublisher : IEmployeePublisher
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly ILogger<EmployeePublisher> _logger;

        public EmployeePublisher(
            HttpClient httpClient,
            IConfiguration configuration,
            ILogger<EmployeePublisher> logger)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task PublishEmployeeAsync(Employee employee)
        {
            var subscriberUrl = _configuration["SubscriberUrl"] ?? "http://localhost:5002";
            var response = await _httpClient.PostAsJsonAsync(
                $"{subscriberUrl}/messages", 
                employee);
                
            response.EnsureSuccessStatusCode();
            _logger.LogInformation("Successfully published employee - Id: {Id}, Name: {Name}", 
                employee.Id, 
                employee.Name);
        }
    }
}