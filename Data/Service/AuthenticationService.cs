using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using EG_ERP.Models;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.IdentityModel.Tokens;

namespace EG_ERP.Data.Service;

public class AuthenticationService: IAuthenticationService
{
    private readonly IConfiguration _configuration;
    private readonly SymmetricSecurityKey _key;

    public AuthenticationService(IConfiguration configuration)
    {
        _configuration = configuration;
        _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("EgERP_JWT_KEY")
            ?? throw new ArgumentNullException("EgERP_JWT_KEY is not set in environment variables")));
        
    }

    public string GenerateToken(List<Claim> claims, TimeSpan? time = null)
    {
        time ??= TimeSpan.FromMinutes(int.Parse(_configuration["Jwt:TokenExpirationInMin"] ?? "10"));
        DateTime expiration = DateTime.UtcNow.Add(time.Value);

        SigningCredentials creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256);

        SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = expiration,
            SigningCredentials = creds
        };

        JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
        SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }

    public string GenerateRefreshToken()
    {
        return Guid.NewGuid().ToString();
    }

    public bool ValidateToken(string token, AppUser user)
    {
        if (string.IsNullOrEmpty(token))
            return false;
        try
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
                IssuerSigningKey = _key
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            ClaimsPrincipal principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken validatedToken);

            if (validatedToken is not JwtSecurityToken jwtToken ||
                !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                return false;
            }

            var userIdClaim = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userIdClaim != user.Id.ToString())
                return false;

            return true;
        }
        catch (SecurityTokenException)
        {
            return false;
        }
        catch (Exception)
        {
            return false;
            //TODO: Log Exception
        }
    }
}
