using System;
using System.ComponentModel.DataAnnotations;

namespace EG_ERP.Models;

public class Admin : AppUser
{
    [Required]
    [StringLength(255)]
    public string? Name { get; set; }
}
