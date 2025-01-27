using System;

namespace EG_ERP.DTOs.Account;

public class LoginResponseDTO
{
    public string RefreshToken { get; set; }
    public string Token { get; set; }
    public DateTime Expiration { get; set; }
    public string UserName { get; set; }

}
