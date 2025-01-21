using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace EG_ERP.Models;

public class AppUser : IdentityUser<int>
{
    public string Uuid { get; set; } = Guid.NewGuid().ToString();
}
