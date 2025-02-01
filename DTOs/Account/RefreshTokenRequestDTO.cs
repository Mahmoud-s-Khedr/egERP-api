using System;

namespace EG_ERP.DTOs.Account;

public class RefreshTokenRequestDTO
{
    public required string UserId { get; set; }
    public required string RefreshToken { get; set; }
}
