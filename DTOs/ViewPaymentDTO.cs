using System;

namespace EG_ERP.DTOs.Product;

public class ViewPaymentDTO
{
    public decimal Amount { get; set; }

    public DateTime PaymentDate { get; set; }

    public string? Description { get; set; }

    public string SerialNumber { get; set; }

}
