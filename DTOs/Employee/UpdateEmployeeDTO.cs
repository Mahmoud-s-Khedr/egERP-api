using System;

namespace EG_ERP.DTOs.Employee;

public class UpdateEmployeeDTO
{
    public string? FName { get; set; }
    public string? LName { get; set; }
    public string? JobTitle { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Address { get; set; }
    public DateOnly? BirthDate { get; set; }
    public DateOnly? HireDate { get; set; }
    public int? DepartmentId { get; set; }
}
