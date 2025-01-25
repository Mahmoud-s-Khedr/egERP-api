using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using EG_ERP.Utils;
namespace EG_ERP.Models;

[Index(nameof(SerialNumber), IsUnique = true)]
public class Payment : BaseEntity
{
    [Required]
    [Column(TypeName = "decimal(15, 2)")]
    public decimal Amount { get; set; }

    [Required]
    [Column(TypeName = "date")]
    public DateTime PaymentDate { get; set; }

    public string? Description { get; set; }

    [Required]
    [StringLength(255)]
    public required string SerialNumber { get; set; }
    public required MoneyState moueyState { get; set; }  = MoneyState.In;
    public virtual ICollection<OrderPayment> Orders { get; set; } = new List<OrderPayment>();
    public virtual ICollection<PayrollPayment> Payrolls { get; set; } = new List<PayrollPayment>();
}
