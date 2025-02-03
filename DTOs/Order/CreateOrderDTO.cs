using System;

namespace EG_ERP.DTOs.Order;

public class CreateOrderDTO
{

    public required decimal Price { get; set; }

    public required string CustomerId { get; set; }

    public ICollection<CreateOrderDetailDTO> OrderDetails { get; set; } = new List<CreateOrderDetailDTO>();
}
