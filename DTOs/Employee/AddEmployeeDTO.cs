using System;

namespace EG_ERP.DTOs.Employee;

public class AddEmployeeDTO
{
    public required string FName { get; set; }
    public required string LName { get; set; }
    public string? JobTitle { get; set; }
    public required string Email { get; set; }
    public required string PhoneNumber { get; set; }
    public string? Address { get; set; }
    public required DateOnly BirthDate { get; set; }
    
}
