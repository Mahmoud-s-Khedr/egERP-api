using System;

namespace EG_ERP.DTOs.Employee;

public class UpdateProfileDTO
{
    public string? FName { get; set; }
    public string? LName { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Address { get; set; }
}
