using System;

namespace EG_ERP.DTOs.Order;

public class CreateOrderDetailDTO
{
    public string ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }

}
