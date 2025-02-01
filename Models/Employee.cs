using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EG_ERP.Utils.enums;

namespace EG_ERP.Models;

public class Employee : AppUser
{
    [Required]
    [StringLength(255)]
    public required string FName { get; set; }

    [Required]
    [StringLength(255)]
    public required string LName { get; set; }

    [StringLength(255)]
    public string? JobTitle { get; set; }

    public string? Address { get; set; }

    public Gender Gender { get; set; } //TODO: Add Gender to DTOs and Controllers

    [Column(TypeName = "date")]
    [Required]
    public DateOnly BirthDate { get; set; }

    [Column(TypeName = "date")]
    [Required]
    public DateOnly HireDate { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow);

    [Column(TypeName = "decimal(15, 2)")]
    [Required]
    public decimal Salary { get; set; }

    [ForeignKey("Department")]
    public int? DepartmentId { get; set; } = null;

    public virtual Department? Department { get; set; }
    public virtual ICollection<Payroll> Payrolls { get; set; } = new List<Payroll>();
    public virtual Department? ManagerOf { get; set; }
}

// Supervisor