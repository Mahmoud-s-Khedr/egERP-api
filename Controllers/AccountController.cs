using System.Security.Claims;
using EG_ERP.Data.Service;
using EG_ERP.DTOs.Account;
using EG_ERP.DTOs.Employee;
using EG_ERP.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EG_ERP.Controllers;

[Route("api/[controller]")]
[ApiController]
[AllowAnonymous]
public class AccountController : ControllerBase
{
    SignInManager<AppUser> _signInManager;
    UserManager<AppUser> _userManager;

    IAuthenticationService _authenticationService;
    private readonly EmailService emailService;

    public AccountController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, IAuthenticationService authenticationService, EmailService emailService)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _authenticationService = authenticationService;
        this.emailService = emailService;
    }


    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequestDTO dto)
    {
        var result = await _signInManager.PasswordSignInAsync(dto.UserName, dto.Password, false, false);

        if (result.Succeeded)
        {
            AppUser? user = await _userManager.FindByNameAsync(dto.UserName);
            if (user == null)
                return BadRequest("Invalid username or password");
            if (!user.Vertified)
                return BadRequest("Profile not Completed");
            if (!await _userManager.IsEmailConfirmedAsync(user))
                return BadRequest("Email not confirmed");

            ICollection<string> roles = await _userManager.GetRolesAsync(user);
            List<Claim> claims = new List<Claim>{
                    new Claim(ClaimTypes.NameIdentifier, user.Uuid.ToString()),
                    new Claim(ClaimTypes.Name, user.UserName ?? ""),};

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            LoginResponseDTO response = new LoginResponseDTO
            {
                Token = _authenticationService.GenerateToken(claims),
                RefreshToken = _authenticationService.GenerateRefreshToken(),
                Expiration = DateTime.Now.AddMinutes(5),
                UserName = user.UserName ?? ""
            };
            user.RefreshToken = response.RefreshToken;
            await _userManager.UpdateAsync(user);
            return Ok(response);
        }
        return BadRequest("Invalid username or password");
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return Ok();
    }

    [HttpGet("validate-token")] //TODO: Enhance Expire Token
    public async Task<IActionResult> ValidateToken(string userId, string token)
    {
        if (userId == null || token == null)
            return BadRequest("Invalid email confirmation request.");

        AppUser? user = await _userManager.Users.SingleOrDefaultAsync(u => u.Uuid == userId);
        if (user == null)
            return NotFound($"User Not Found");

        IdentityResult result = await _userManager.ConfirmEmailAsync(user, token);
        if (!result.Succeeded)
            return BadRequest("Token Invalid.");

        CompleteEmployeeDTO? view = null;
        if (user is Employee employee)
        {
            view = new CompleteEmployeeDTO
            {
                Id = employee.Uuid,
                FName = employee.FName,
                LName = employee.LName,
                JobTitle = employee.JobTitle,
                Email = employee.Email,
                PhoneNumber = employee.PhoneNumber,
                Address = employee.Address,
                BirthDate = employee.BirthDate,
                HireDate = employee.HireDate,
                Department = employee.Department?.Name,
                Salary = employee.Salary
            };
        }

        List<Claim> claims = new List<Claim>{
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.UserName ?? ""),
            new Claim(ClaimTypes.Role, "Employee")
        };
        
        return Ok(new {
            IsValid = true,
            Employee = view,
            Token = _authenticationService.GenerateToken(claims, TimeSpan.FromMinutes(20))
        });
    }

    [Authorize]
    [HttpPost("verify-email")]
    public async Task<IActionResult> VerifyEmail()
    {
        if (User.Identity?.Name == null)
            return BadRequest("Invalid username");
        
        AppUser? user = await _userManager.FindByNameAsync(User.Identity.Name);
        if (user == null)
            return BadRequest("Invalid username");
        
        string token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        string? url = Url.Action("VerifyEmail", "Account", new { userId = user.Uuid, token }, Request.Scheme);
        if (url == null)
            //TODO: Log Error
            return BadRequest("Error generating email confirmation link");
        
        await emailService.SendEmailAsync(user.Email ?? "", "Email Confirmation", $"Please confirm your email by clicking <a href='{url}'>here</a>");

        return Ok("Email Sent");
    }

    [HttpGet("verify-email")]
    public async Task<IActionResult> VerifyEmail(string userId, string token)
    {
        if (userId == null || token == null)
            return BadRequest("Invalid email confirmation request.");

        AppUser? user = await _userManager.Users.SingleOrDefaultAsync(u => u.Uuid == userId);
        if (user == null)
            return NotFound($"User Not Found");

        IdentityResult result = await _userManager.ConfirmEmailAsync(user, token);
        if (!result.Succeeded)
            return BadRequest("Token Invalid.");

        return Ok("Email Confirmed");
    }
}

