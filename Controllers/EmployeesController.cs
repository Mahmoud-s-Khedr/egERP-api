using EG_ERP.Data.Repos;
using EG_ERP.Data.Service;
using EG_ERP.Data.UoWs;
using EG_ERP.DTOs.Employee;
using EG_ERP.DTOs.Payment;
using EG_ERP.DTOs.Payroll;
using EG_ERP.Models;
using EG_ERP.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EG_ERP.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EmployeesController : ControllerBase
{
    private readonly IUnitOfWork unit;
    private readonly UserManager<AppUser> userManager;
    private readonly IEmailService emailService;
    private readonly IAuthenticationService auth;
    private readonly IConfiguration config;

    public EmployeesController(IUnitOfWork unit, UserManager<AppUser> userManager,
        IEmailService emailService, IAuthenticationService auth, IConfiguration config)
    {
        this.unit = unit;
        this.userManager = userManager;
        this.emailService = emailService;
        this.auth = auth;
        this.config = config;
    }

    [Authorize(Roles = "Admin, Manager")]
    [HttpGet]
    public async Task<IActionResult> GetEmployees()
    {
        if (!User.IsInRole("Admin") && !User.IsInRole("HR"))
            return Forbid("You are not authorized to view this resource");
;
        List<Employee> employees = (await userManager.GetUsersInRoleAsync("Employee")).OfType<Employee>().ToList();

        List<ViewEmployeeDTO> views = employees.Select(employee => new ViewEmployeeDTO
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

    [Authorize(Roles = "Admin, Manager")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetEmployee(string id)
    {
        if (User.IsInRole("Manager"))
        {
            if (User.Identity?.Name == null)
                return BadRequest("Invalid Username");
        
            Employee? manager = await userManager.Users
                .OfType<Employee>()
                .Include(u => u.ManagerOf)
                .SingleOrDefaultAsync(u => u.UserName == User.Identity.Name);
            
            if (manager == null || manager.ManagerOf?.Name != "HR")
                return Forbid("You are not authorized to view this resource");
        }

        Employee? employee = await userManager.Users.OfType<Employee>().Include("Department").FirstOrDefaultAsync(e => e.Uuid == id);

        if (employee == null)
            return NotFound("Employee not found");

        ViewEmployeeDTO view = new ViewEmployeeDTO
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
        };

        return Ok(view);
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<IActionResult> GetProfile()
    {
        if (User.Identity?.Name == null)
            return BadRequest("Invalid Username");

        Employee? employee = await userManager.Users
            .OfType<Employee>()
            .Include("Department")
            .SingleOrDefaultAsync(e => e.UserName == User.Identity.Name);

        if (employee == null)
            return NotFound("Employee not found");

        ViewEmployeeDTO view = new ViewEmployeeDTO
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
        };

        return Ok(view);
    }

    [Authorize]
    [HttpGet("me/salary")]
    public async Task<IActionResult> GetMySalary()
    {
        if (User.Identity?.Name == null)
            return BadRequest("Invalid Username");

        Employee? employee = await userManager.Users
            .OfType<Employee>()
            .Include("Department")
            .SingleOrDefaultAsync(e => e.UserName == User.Identity.Name);

        if (employee == null)
            return NotFound("Employee not found");

        return Ok(employee.Salary);
    }

    [Authorize(Roles = "Admin, Manager")]
    [HttpGet("{id}/salary")]
    public async Task<IActionResult> GetEmployeeSalary(string id)
    {
        if (User.IsInRole("Manager"))
        {
            if (User.Identity?.Name == null)
                return BadRequest("Invalid Username");
        
            Employee? manager = await userManager.Users
                .OfType<Employee>()
                .Include(u => u.ManagerOf)
                .SingleOrDefaultAsync(u => u.UserName == User.Identity.Name);
            
            if (manager == null || manager.ManagerOf?.Name != "HR")
                return Forbid("You are not authorized to view this resource");
        }

        Employee? employee = await userManager.Users.OfType<Employee>().Include("Department").SingleOrDefaultAsync(e => e.Uuid == id);
        if (employee == null)
            return NotFound("Employee not found");

        return Ok(employee.Salary);
    }

    [Authorize(Roles = "Admin, Manager")]
    [HttpGet("{id}/payrolls")]
    public async Task<IActionResult> GetEmployeePayrolls(string id)
    {
        if (User.IsInRole("Manager"))
        {
            if (User.Identity?.Name == null)
                return BadRequest("Invalid Username");
        
            Employee? manager = await userManager.Users
                .OfType<Employee>()
                .Include(u => u.ManagerOf)
                .SingleOrDefaultAsync(u => u.UserName == User.Identity.Name);
            
            if (manager == null || manager.ManagerOf?.Name != "HR")
                return Forbid("You are not authorized to view this resource");
        }

        Employee? employee = await userManager.Users.OfType<Employee>().Include("Department").SingleOrDefaultAsync(e => e.Uuid == id);
        if (employee == null)
            return NotFound("Employee not found");

        IPayrollRepository repo = unit.GetRepository<Payroll>() as IPayrollRepository ?? throw new NullReferenceException("Payroll repository not found");
        List<Payroll> payrolls = await repo.GetByEmployee(employee.Id);

        List<ViewPayrollDTO> views = payrolls.Select(payroll => new ViewPayrollDTO
        {
            Id = payroll.Uuid,
            BaseSalary = payroll.BaseSalary,
            Bonus = payroll.Bonus,
            Deduction = payroll.Deduction,
            Tax = payroll.Tax,
            Status = payroll.Status,
            PaymentDate = payroll.PaymentDate,
            Employee = $"{employee.FName} {employee.LName}",
            Payment = new ViewPaymentDTO
            {
                Id = payroll.PayrollPayment?.Payment?.Uuid,
                Amount = payroll.PayrollPayment?.Payment?.Amount ?? 0,
                PaymentDate = payroll.PayrollPayment?.Payment?.PaymentDate ?? new DateTime(),
                Description = payroll.PayrollPayment?.Payment?.Description,
                SerialNumber = payroll.PayrollPayment?.Payment?.SerialNumber ?? "N/A",
                MoneyState = payroll.PayrollPayment?.Payment?.MoueyState ?? MoneyState.Out,
            }
        }).ToList();

        return Ok(views);
    }

    [Authorize]
    [HttpGet("me/payrolls")]
    public async Task<IActionResult> GetMyPayrolls()
    {
        if (User.Identity?.Name == null)
            return BadRequest("Invalid Username");

        Employee? employee = await userManager.Users
            .OfType<Employee>()
            .Include("Department")
            .SingleOrDefaultAsync(e => e.UserName == User.Identity.Name);

        if (employee == null)
            return NotFound("Employee not found");
        
        IPayrollRepository repo = unit.GetRepository<Payroll>() as IPayrollRepository ?? throw new NullReferenceException("Payroll repository not found");
        List<Payroll> payrolls = await repo.GetByEmployee(employee.Id);

        List<ViewPayrollDTO> views = payrolls.Select(payroll => new ViewPayrollDTO
        {
            Id = payroll.Uuid,
            BaseSalary = payroll.BaseSalary,
            Bonus = payroll.Bonus,
            Deduction = payroll.Deduction,
            Tax = payroll.Tax,
            Status = payroll.Status,
            PaymentDate = payroll.PaymentDate,
            Employee = $"{employee.FName} {employee.LName}",
            Payment = new ViewPaymentDTO
            {
                Id = payroll.PayrollPayment?.Payment?.Uuid,
                Amount = payroll.PayrollPayment?.Payment?.Amount ?? 0,
                PaymentDate = payroll.PayrollPayment?.Payment?.PaymentDate ?? new DateTime(),
                Description = payroll.PayrollPayment?.Payment?.Description,
                SerialNumber = payroll.PayrollPayment?.Payment?.SerialNumber ?? "N/A",
                MoneyState = payroll.PayrollPayment?.Payment?.MoueyState ?? MoneyState.Out,
            }
        }).ToList();

        return Ok(views);
    }


    // [Authorize(Roles = "Admin, Manager")]
    [HttpPost]
    public async Task<IActionResult> CreateEmployee(AddEmployeeDTO dto)
    {
        // if (User.IsInRole("Manager"))
        // {
        //     if (User.Identity?.Name == null)
        //         return BadRequest("Invalid Username");
        
        //     Employee? manager = await userManager.Users
        //         .OfType<Employee>()
        //         .Include(u => u.ManagerOf)
        //         .SingleOrDefaultAsync(u => u.UserName == User.Identity.Name);
            
        //     if (manager == null || manager.ManagerOf?.Name != "HR")
        //         return Forbid("You are not authorized to create an employee");
        // }

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        Employee employee = new Employee
        {
            FName = dto.FName,
            LName = dto.LName,
            JobTitle = dto.JobTitle,
            Email = dto.Email,
            PhoneNumber = dto.PhoneNumber,
            Address = dto.Address,
            BirthDate = dto.BirthDate,
            HireDate = dto.HireDate,
            Salary = dto.Salary,
            UserName = dto.Email,
            EmailConfirmed = false,
        };

        IdentityResult result = await userManager.CreateAsync(employee, "Test@123");

        if (!result.Succeeded)
            return BadRequest(result.Errors);
        
        result = await userManager.AddToRoleAsync(employee, "Employee");
        if (!result.Succeeded)
            return BadRequest(result.Errors);

        string token = await userManager.GenerateEmailConfirmationTokenAsync(employee);

        Uri client = new Uri(config["Client"] ?? "http://localhost:3000");
        UriBuilder builder = new UriBuilder(new Uri(client, "complete-profile"))
        {
            Query = $"token={token}&userId={employee.Uuid}"
        };
        Uri Link = builder.Uri;

        // string? Link = Url.Action("ValidateToken", "Account", new { user_id = employee.Uuid, token = token }, Request.Scheme ?? "http");
        // if (Link == null)
        //     return BadRequest("Failed to generate confirmation link" + $"{Request.Scheme} {new { userId = employee.Uuid, token = token }}");

        await emailService.SendEmailAsync(employee.Email, "Complete Registration",
            $"Please complete your account registration by clicking <a href='{Link}'>here</a>.", true);

        // return Ok(new { Id = employee.Uuid });
        return Ok(Link);  //TODO: Remove this line
    }

    [HttpPost("/complete-profile")]
    public async Task<IActionResult> CompleteProfile(RegisterEmployeeDTO dto, [FromQuery] string token)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        Employee? employee = await userManager.Users.OfType<Employee>().Include("Department").FirstOrDefaultAsync(e => e.Uuid == dto.Id);
        if (employee == null)
            return NotFound("Employee not found");

        if (auth.ValidateToken(token, employee) == false)
            return Unauthorized("Invalid token");

        if (employee.Vertified)
            return BadRequest("Employee already completed profile");

        employee.UserName = dto.UserName;
        IdentityResult res = await userManager.RemovePasswordAsync(employee);
        if (!res.Succeeded)
            return BadRequest(res.Errors);
        res = await userManager.AddPasswordAsync(employee, dto.Password);
        if (!res.Succeeded)
            return BadRequest(res.Errors);

        employee.FName = dto.FName ?? employee.FName;
        employee.LName = dto.LName ?? employee.LName;
        if (dto.Email is not null && dto.Email != employee.Email)
        {
            employee.Email = dto.Email;
            employee.EmailConfirmed = false;
        }
        if (dto.PhoneNumber is not null && dto.PhoneNumber != employee.PhoneNumber)
        {
            employee.PhoneNumber = dto.PhoneNumber;
            employee.PhoneNumberConfirmed = false;
        }
        employee.Address = dto.Address ?? employee.Address;

        employee.Vertified = true;

        IdentityResult result = await userManager.UpdateAsync(employee);
        if (!result.Succeeded)
            return BadRequest(result.Errors);

        await unit.Commit();
        return NoContent();
    }

    [Authorize(Roles = "Admin, Manager")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateEmployee(string id, UpdateEmployeeDTO dto)
    {
        if (User.IsInRole("Manager"))
        {
            if (User.Identity?.Name == null)
                return BadRequest("Invalid username");

            Employee? manager = await userManager.Users
                .OfType<Employee>()
                .Include(u => u.ManagerOf)
                .SingleOrDefaultAsync(u => u.UserName == User.Identity.Name);

            if (manager == null || manager.ManagerOf?.Name != "HR")
                return Forbid("You are not authorized to update an employee");
        }

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        Employee? employee = await userManager.Users.OfType<Employee>().Include("Department").FirstOrDefaultAsync(e => e.Uuid == id);
        if (employee == null)
            return NotFound("Employee not found");

        IGenericRepository<Department> repo = unit.GetRepository<Department>();
        Department? department = dto.DepartmentId != null ? await repo.GetById(dto.DepartmentId.Value) : null;

        if (department == null && dto.DepartmentId != null)
            return BadRequest("Department not found");

        employee.FName = dto.FName ?? employee.FName;
        employee.LName = dto.LName ?? employee.LName;
        employee.JobTitle = dto.JobTitle ?? employee.JobTitle;
        employee.Email = dto.Email ?? employee.Email;
        employee.PhoneNumber = dto.PhoneNumber ?? employee.PhoneNumber;
        employee.Address = dto.Address ?? employee.Address;
        employee.BirthDate = dto.BirthDate ?? employee.BirthDate;
        employee.HireDate = dto.HireDate ?? employee.HireDate;
        employee.DepartmentId = department?.Id ?? employee.DepartmentId;

        IdentityResult result = await userManager.UpdateAsync(employee);
        if (!result.Succeeded)
            return BadRequest(result.Errors);
        await unit.Commit();

        return NoContent();
    }

    [Authorize]
    [HttpPut("me")]
    public async Task<IActionResult> UpdateProfile(UpdateProfileDTO dto)
    {
        if (User.Identity?.Name == null)
            return BadRequest("Invalid Username");

        Employee? employee = await userManager.Users
            .OfType<Employee>()
            .Include("Department")
            .SingleOrDefaultAsync(e => e.UserName == User.Identity.Name);

        if (employee == null)
            return NotFound("Employee not found");
        
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        employee.FName = dto.FName ?? employee.FName;
        employee.LName = dto.LName ?? employee.LName;
        if (dto.Email is not null && dto.Email != employee.Email)
        {
            employee.Email = dto.Email;
            employee.EmailConfirmed = false;
        }
        if (dto.PhoneNumber is not null && dto.PhoneNumber != employee.PhoneNumber)
        {
            employee.PhoneNumber = dto.PhoneNumber;
            employee.PhoneNumberConfirmed = false;
        }
        employee.Address = dto.Address ?? employee.Address;

        IdentityResult result = await userManager.UpdateAsync(employee);
        if (!result.Succeeded)
            return BadRequest(result.Errors);
        
        await unit.Commit();
        return NoContent();
    }

    [Authorize(Roles = "Admin, Manager")]
    [HttpPatch("{id}/salary")]
    public async Task<IActionResult> UpdateSalary(string id, [FromBody] SalartRequestDTO dto)
    {
        if (User.IsInRole("Manager"))
        {
            if (User.Identity?.Name == null)
                return BadRequest("Invalid username");

            Employee? manager = await userManager.Users
                .OfType<Employee>()
                .Include(u => u.ManagerOf)
                .SingleOrDefaultAsync(u => u.UserName == User.Identity.Name);

            if (manager == null || manager.ManagerOf?.Name != "HR")
                return Forbid("You are not authorized to update an employee's salary");
        }

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        Employee? employee = await userManager.Users.OfType<Employee>().Include("Department").FirstOrDefaultAsync(e => e.Uuid == id);
        if (employee == null)
            return NotFound("Employee not found");

        employee.Salary = dto.Salary;

        IdentityResult result = await userManager.UpdateAsync(employee);
        if (!result.Succeeded)
            return BadRequest(result.Errors);
        await unit.Commit();

        return NoContent();
    }
}

