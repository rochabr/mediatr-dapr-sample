// Shared.Contracts/Models/Employee.cs
using System.Text.Json.Serialization;

namespace Shared.Contracts.Models;

public class Employee
{

    [JsonPropertyName("id")]
    public int Id { get; set; }
    
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
}