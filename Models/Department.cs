using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EG_ERP.Models;

public class Department : BaseEntity
{
    [Required]
    [StringLength(255)]
    public required string Name { get; set; }

    [Required]
    [ForeignKey("Manager")]
    public int ManagerId { get; set; }

    public virtual Employee? Manager { get; set; }
}
