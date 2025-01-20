using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EG_ERP.Models;

public class OrderPayment
{
    [Required]
    [ForeignKey("Order")]
    public required int OrderId { get; set; }

    [Required]
    [ForeignKey("Payment")]
    public required int PaymentId { get; set; }

    public virtual Order? Order { get; set; }
    public virtual Payment? Payment { get; set; }
}
