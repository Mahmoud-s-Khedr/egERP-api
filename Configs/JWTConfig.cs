using System;

namespace EG_ERP.Configs;

public class JWTConfig
{
    public string? Key { get; set; }
    public string? Issuer { get; set; }
    public string? Audience { get; set; }
    public int? TokenExpirationInMin { get; set; }
    public int? RefreshTokenExpirationInDays { get; set; }
}
