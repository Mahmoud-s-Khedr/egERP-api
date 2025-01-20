using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EG_ERP.Utils;

namespace EG_ERP.Models;

public class Payroll : BaseEntity
{
    public int EmployeeId { get; set; }

    [Column(TypeName = "money")]
    [Required]
    public decimal BaseSalary { get; set; }

    [Column(TypeName = "money")]
    [Required]
    public decimal Bonus { get; set; }

    [Column(TypeName = "money")]
    [Required]
    public decimal Deduction { get; set; }

    [Column(TypeName = "money")]
    [Required]
    public decimal Tax { get; set; }

    public Status Status { get; set; } = Status.Pending;
    public virtual Employee? Employee { get; set; }

    public virtual ICollection<PayrollPayment> PayrollPayments { get; set; } = new List<PayrollPayment>();
}
