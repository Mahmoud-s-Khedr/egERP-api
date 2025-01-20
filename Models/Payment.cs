using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EG_ERP.Models;

public class Payment : BaseEntity
{
    [Required]
    [Column(TypeName = "money")]
    public decimal Amount { get; set; }

    [Required]
    [Column(TypeName = "date")]
    public DateTime PaymentDate { get; set; }

    public string? Description { get; set; }

    [Required]
    [StringLength(255)]
    public required string SerialNumber { get; set; }

    public virtual ICollection<OrderPayment> Orders { get; set; } = new List<OrderPayment>();
    public virtual ICollection<PayrollPayment> Payrolls { get; set; } = new List<PayrollPayment>();
}
