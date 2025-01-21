using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EG_ERP.Models;

public class PayrollPayment
{
    [Required]
    [ForeignKey("Payroll")]
    public required int PayrollId { get; set; }

    [Required]
    [ForeignKey("Payment")]
    public required int PaymentId { get; set; }

    public virtual Payroll? Payroll { get; set; }
    public virtual Payment? Payment { get; set; }
}
