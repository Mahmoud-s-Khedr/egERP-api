using System;
using System.ComponentModel.DataAnnotations;

namespace EG_ERP.DTOs.Account;

public class LoginRequestDTO
{   
    [Required]
    public string UserName { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }

}
