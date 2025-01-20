using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

    [Column(TypeName = "date")]
    [Required]
    public DateOnly BirthDate { get; set; }

    [Column(TypeName = "money")]
    [Required]
    public decimal Salary { get; set; }

    [ForeignKey("Department")]
    [Required]
    public int DepartmentId { get; set; }

    public virtual Department? Department { get; set; }
    public virtual ICollection<Payroll> Payrolls { get; set; } = new List<Payroll>();
    public virtual Department? ManagerOf { get; set; }
}
