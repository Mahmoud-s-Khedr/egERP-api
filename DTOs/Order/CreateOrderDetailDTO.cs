using System;

namespace EG_ERP.DTOs.Order;

public class CreateOrderDetailDTO
{
    public required string ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}
