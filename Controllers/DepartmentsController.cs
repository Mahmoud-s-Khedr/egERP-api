using EG_ERP.Data.Repos;
using EG_ERP.Data.UoWs;
using EG_ERP.DTOs.Department;
using EG_ERP.DTOs.Employee;
using EG_ERP.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EG_ERP.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DepartmentsController : ControllerBase
{
    private readonly IUnitOfWork unit;
    private readonly UserManager<AppUser> userManager;

    public DepartmentsController(IUnitOfWork unit, UserManager<AppUser> userManager)
    {
        this.unit = unit;
        this.userManager = userManager;
    }

    [HttpGet]
    public async Task<IActionResult> GetDepartments()
    {
        IGenericRepository<Department> repo = unit.GetRepository<Department>();
        List<Department> departments = await repo.GetAll(includes: new[] { "Manager", "Employees" });

        List<ViewDepartmentDTO> views = departments.Select(department => new ViewDepartmentDTO
        {
            Id = department.Uuid,
            Name = department.Name,
            Manager = $"{department.Manager?.FName} {department.Manager?.LName}",
            Description = department.Description,
            Location = department.Location,
            NumberOfEmployees = department.Employees.Count
        }).ToList();

        return Ok(views);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetDepartment(string id)
    {
        IGenericRepository<Department> repo = unit.GetRepository<Department>();
        Department? department = await repo.GetById(id, includes: new[] { "Manager", "Employees" });

        if (department == null)
            return NotFound("Department not found");

        ViewDepartmentDTO view = new ViewDepartmentDTO
        {
            Id = department.Uuid,
            Name = department.Name,
            Description = department.Description,
            Location = department.Location,
            Manager = $"{department.Manager?.FName} {department.Manager?.LName}",
            NumberOfEmployees = department.Employees.Count
        };

        return Ok(view);
    }

    [HttpGet("{id}/Employees")]
    public async Task<IActionResult> GetEmployees(string id)
    {
        IGenericRepository<Department> repo = unit.GetRepository<Department>();
        Department? department = await repo.GetById(id, includes: new[] { "Employees" });

        if (department == null)
            return NotFound("Department not found");

        List<ViewEmployeeDTO> views = department.Employees.Select(employee => new ViewEmployeeDTO
        {
            Id = employee.Uuid,
            Name = $"{employee.FName} {employee.LName}",
            JobTitle = employee.JobTitle,
            Email = employee.Email,
            PhoneNumber = employee.PhoneNumber,
            Address = employee.Address,
            BirthDate = employee.BirthDate,
            HireDate = employee.HireDate,
            Department = employee.Department?.Name ?? "No Department",
        }).ToList();

        return Ok(views);
    }

    [HttpGet("{id}/Manager")]
    public async Task<IActionResult> GetManager(string id)
    {
        IGenericRepository<Department> repo = unit.GetRepository<Department>();
        Department? department = await repo.GetById(id, includes: new[] { "Manager" });

        if (department == null)
            return NotFound("Department not found");

        ViewEmployeeDTO? view = department.Manager == null ? null : new ViewEmployeeDTO
        {
            Id = department.Manager.Uuid,
            Name = $"{department.Manager.FName} {department.Manager.LName}",
            JobTitle = department.Manager.JobTitle,
            Email = department.Manager.Email,
            PhoneNumber = department.Manager.PhoneNumber,
            Address = department.Manager.Address,
            BirthDate = department.Manager.BirthDate,
            HireDate = department.Manager.HireDate,
            Department = department.Manager.Department?.Name ?? "No Department",
        };

        return Ok(view);
    }

    [HttpPost]
    public async Task<IActionResult> CreateDepartment(AddDepartmentDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        Employee? manager = await userManager.Users.Include("Department").FirstOrDefaultAsync(e => e.Uuid == dto.ManagerId) as Employee;
        if (manager == null)
            return BadRequest("Manager not found");

        Department department = new Department
        {
            Name = dto.Name,
            Description = dto.Description,
            Location = dto.Location,
            ManagerId = manager.Id
        };

        IGenericRepository<Department> repo = unit.GetRepository<Department>();
        await repo.Add(department);

        IdentityResult result = await userManager.AddToRoleAsync(manager, "Manager");
        if (!result.Succeeded)
            return BadRequest(result.Errors);

        await unit.Commit();

        ViewDepartmentDTO view = new ViewDepartmentDTO
        {
            Id = department.Uuid,
            Name = department.Name,
            Description = department.Description,
            Location = department.Location,
            Manager = $"{manager.FName} {manager.LName}",
            NumberOfEmployees = 0
        };

        return CreatedAtAction(nameof(GetDepartment), new { id = department.Uuid }, view);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateDepartment(string id, UpdateDepartmentDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        IGenericRepository<Department> repo = unit.GetRepository<Department>();
        Department? department = await repo.GetById(id);
        if (department == null)
            return NotFound("Department not found");

        department.Name = dto.Name ?? department.Name;
        department.Description = dto.Description ?? department.Description;
        department.Location = dto.Location ?? department.Location;

        await unit.Commit();

        return NoContent();
    }

    [HttpPatch("{id}/Manager/{managerId}")]
    public async Task<IActionResult> UpdateManager(string id, string managerId)
    {
        IGenericRepository<Department> repo = unit.GetRepository<Department>();
        Department? department = await repo.GetById(id);
        if (department == null)
            return NotFound("Department not found");

        Employee? manager = await userManager.Users.Include("Department").FirstOrDefaultAsync(e => e.Uuid == managerId) as Employee;
        if (manager == null)
            return BadRequest("Manager not found");

        if (department.ManagerId == manager.Id)
            return NoContent();

        if (department.Manager != null)
        {
            IdentityResult result = await userManager.RemoveFromRoleAsync(department.Manager, "Manager");
            if (!result.Succeeded)
                return BadRequest(result.Errors);
        }

        department.ManagerId = manager.Id;

        IdentityResult res = await userManager.AddToRoleAsync(manager, "Manager");
        if (!res.Succeeded)
            return BadRequest(res.Errors);

        await unit.Commit();

        return NoContent();
    }

    [HttpPatch("{id}/Employees/{employeeId}")]
    public async Task<IActionResult> AddEmployee(string id, string employeeId)
    {
        IGenericRepository<Department> repo = unit.GetRepository<Department>();
        Department? department = await repo.GetById(id, includes: new[] { "Employees" });
        if (department == null)
            return NotFound("Department not found");

        Employee? employee = await userManager.Users.Include("Department").FirstOrDefaultAsync(e => e.Uuid == employeeId) as Employee;
        if (employee == null)
            return BadRequest("Employee not found");

        if (employee.DepartmentId == department.Id)
            return NoContent();

        employee.DepartmentId = department.Id;

        await unit.Commit();

        return NoContent();
    }
}
