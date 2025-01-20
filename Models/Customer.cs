using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace EG_ERP.Models;

public class Customer : BaseEntity
{
    [Required]
    [StringLength(255)]
    public required string Name { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}

