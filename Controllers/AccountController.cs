using System.Security.Claims;
using EG_ERP.Data.Service;
using EG_ERP.Data.UoWs;
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
    private readonly IUnitOfWork _unit;
    IAuthenticationService _auth;
    private readonly IEmailService _emailService;
    private readonly IConfiguration _config;

    public AccountController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, IUnitOfWork unit, IAuthenticationService auth, IEmailService emailService, IConfiguration config)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _unit = unit;
        _auth = auth;
        _emailService = emailService;
        _config = config;
    }


    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequestDTO dto)
    {
        var result = await _signInManager.PasswordSignInAsync(dto.UserName, dto.Password, false, false);

        if (!result.Succeeded)
        {
            // _logger.LogWarning("Login failed for user {UserName}", dto.UserName);
            return Unauthorized("Invalid username or password");
        }

        AppUser? user = await _userManager.FindByNameAsync(dto.UserName);
        if (user == null)
            return Unauthorized("Invalid username or password");

        if (!user.Vertified)
            return Forbid("Profile not Completed");

        if (!await _userManager.IsEmailConfirmedAsync(user))
            return Forbid("Email not confirmed");

        List<Claim> claims = await GetUserClaims(user);

        var token = _auth.GenerateToken(claims);
        var refreshToken = _auth.GenerateRefreshToken();
        var refreshTokenExpiration = DateTime.UtcNow.Add(_auth.RefreshTokenExpirationInDays);

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiedDate = refreshTokenExpiration;

        await _userManager.UpdateAsync(user);

        var response = new LoginResponseDTO
        {
            AccessToken = token,
            RefreshToken = refreshToken,
            UserName = user.UserName ?? "",
            UserId = user.Uuid
        };

        return Ok(response);
    }

    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        if (User.Identity?.Name == null)
            return BadRequest("Invalid username");
        AppUser? user = await _userManager.FindByNameAsync(User.Identity.Name);
        if (user == null)
            return BadRequest("Invalid username");
        
        user.RefreshToken = null;
        user.RefreshTokenExpiedDate = null;

        await _signInManager.SignOutAsync();
        await _userManager.UpdateAsync(user);
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
            Token = _auth.GenerateToken(claims, TimeSpan.FromMinutes(20))
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
        
        await _emailService.SendEmailAsync(user.Email ?? "", "Email Confirmation", $"Please confirm your email by clicking <a href='{url}'>here</a>");

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


    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequestDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        if (string.IsNullOrEmpty(dto.RefreshToken))
            return BadRequest("Refresh token is required");

        AppUser? user = await _userManager.Users.SingleOrDefaultAsync(u => u.Uuid == dto.UserId);
        if (user == null || user.RefreshToken != dto.RefreshToken)
            return Unauthorized("Invalid refresh token");

        if (user.RefreshTokenExpiedDate <= DateTime.UtcNow)
            return Unauthorized("Expired refresh token");

        List<Claim> claims = await GetUserClaims(user);

        var newAccessToken = _auth.GenerateToken(claims);
        var newRefreshToken = _auth.GenerateRefreshToken();
        DateTime refreshTokenExpiration = DateTime.UtcNow.Add(_auth.RefreshTokenExpirationInDays);

        user.RefreshToken = newRefreshToken;
        user.RefreshTokenExpiedDate = refreshTokenExpiration;

        await _userManager.UpdateAsync(user);

        return Ok(new LoginResponseDTO
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken,
            UserName = user.UserName ?? "",
            UserId = user.Uuid
        });
    }

    private async Task<List<Claim>> GetUserClaims(AppUser user)
    {
        ICollection<string> roles = await _userManager.GetRolesAsync(user);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Uuid.ToString()),
            new Claim(ClaimTypes.Name, user.UserName ?? "")
        };

        if (user is Employee employee)
        {
            Department? department = await _unit.GetRepository<Department>().GetById(employee.DepartmentId ?? 0);
            if (department is not null)
                claims.Add(new Claim(ClaimTypes.Role, department.Name));
        }

        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        return claims;
    }
}

