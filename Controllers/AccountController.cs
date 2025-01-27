using System.Security.Claims;
using EG_ERP.Data.Service;
using EG_ERP.DTOs.Account;
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

    public AccountController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, IAuthenticationService authenticationService)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _authenticationService = authenticationService;
    }


    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequestDTO dto)
    {
        var result = await _signInManager.PasswordSignInAsync(dto.UserName, dto.Password, false, false);

        if (result.Succeeded)
        {
            AppUser user = await _userManager.FindByNameAsync(dto.UserName);
            ICollection<string> roles = await _userManager.GetRolesAsync(user);
            List<Claim> claims = new List<Claim>{
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.UserName)};

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            LoginResponseDTO response = new LoginResponseDTO
            {
                Token = _authenticationService.GenerateToken(claims),
                RefreshToken = _authenticationService.GenerateRefreshToken(),
                Expiration = DateTime.Now.AddMinutes(5),
                UserName = user.UserName
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

    [HttpGet("ConfirmEmail")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public async Task<IActionResult> ConfirmEmail(string userId, string token)
    {
        if (userId == null || token == null)
            return BadRequest("Invalid email confirmation request.");

        AppUser user = await _userManager.Users.SingleOrDefaultAsync(u => u.Uuid == userId);
        if (user == null)
            return NotFound($"User Not Found");

        IdentityResult result = await _userManager.ConfirmEmailAsync(user, token);

        if (!result.Succeeded)
            return BadRequest("Email confirmed Failed.");

        return Ok("Email confirmed successfully.");
    }
}

