using System;

namespace EG_ERP.DTOs.Department;

public class ViewDepartmentDTO
{
    public string? Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Location { get; set; }
    public string? Manager { get; set; }
    public int NumberOfEmployees { get; set; }
}

