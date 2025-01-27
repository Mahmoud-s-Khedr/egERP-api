using System;
using System.Security.Claims;

namespace EG_ERP.Data.Service;

public interface IAuthenticationService
{
    public string GenerateToken(List<Claim> claims);

    public string GenerateRefreshToken();
}
