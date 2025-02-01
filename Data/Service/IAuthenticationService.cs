using System;
using System.Security.Claims;
using EG_ERP.Models;

namespace EG_ERP.Data.Service;

public interface IAuthenticationService
{
    public TimeSpan RefreshTokenExpirationInDays { get; }

    public string GenerateToken(List<Claim> claims, TimeSpan? time = null);
    public string GenerateRefreshToken();

    public bool ValidateToken(string token, AppUser user);
}
