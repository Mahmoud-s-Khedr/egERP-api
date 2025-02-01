using System;

namespace EG_ERP.DTOs.Account;

public class LoginResponseDTO
{
    public string? RefreshToken { get; set; }
    public string? AccessToken { get; set; }
    public string? UserName { get; set; }
    public string? UserId { get; set; }
}
