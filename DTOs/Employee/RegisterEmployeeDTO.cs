using System;

namespace EG_ERP.DTOs.Employee;

public class RegisterEmployeeDTO
{
    public required string Id { get; set; }
    public required string UserName { get; set; }
    public required string Password { get; set; }
    public string? FName { get; set; }
    public string? LName { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Address { get; set; }
}
