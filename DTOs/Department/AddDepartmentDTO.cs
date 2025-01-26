using System;

namespace EG_ERP.DTOs.Department;

public class AddDepartmentDTO
{
    public required string Name { get; set; }
    public required string ManagerId { get; set; }
    public string? Description { get; set; }
    public string? Location { get; set; }   
}
