using System;
using System.ComponentModel.DataAnnotations;

namespace EG_ERP.Models;

public class Admin : BaseEntity
{
    [Required]
    [StringLength(255)]
    public required string Name { get; set; }
}
