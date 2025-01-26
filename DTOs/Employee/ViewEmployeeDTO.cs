using System;
using System.ComponentModel.DataAnnotations;

namespace EG_ERP.DTOs.Employee;

public class ViewEmployeeDTO
{
    public string? Id { get; set; }
    public string? Name { get; set; }
    public string? JobTitle { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Address { get; set; }

    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
    public DateOnly BirthDate { get; set; }
    public DateOnly HireDate { get; set; }

    public string? Department { get; set; }
}

