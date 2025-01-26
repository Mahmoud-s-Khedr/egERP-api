using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EG_ERP.Models;

[Index(nameof(Uuid), IsUnique = true)]
public class AppUser : IdentityUser<int>
{
    public string Uuid { get; set; } = Guid.NewGuid().ToString();
    public bool Vertified { get; set; } = false;
}
