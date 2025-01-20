using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace EG_ERP.Models;

public class AppUser : IdentityUser<int>
{
    [Key]
    public override int Id { get; set; }

    public string Uuid { get; set; } = Guid.NewGuid().ToString();
}
