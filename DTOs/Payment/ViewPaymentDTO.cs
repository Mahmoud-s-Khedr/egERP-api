using System;
using EG_ERP.Utils;

namespace EG_ERP.DTOs.Payment;

public class ViewPaymentDTO
{
    public string? Id { get; set; }
    public decimal Amount { get; set; }

    public DateTime PaymentDate { get; set; }

    public string? Description { get; set; }

    public required string SerialNumber { get; set; }

    public MoneyState MoneyState { get; set; }
}
