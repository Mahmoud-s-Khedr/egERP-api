using System;

namespace EG_ERP.DTOs.Order;

public class CreateOrderDTO
{

    public decimal Price { get; set; }

    public int CustomerId { get; set; }

    public ICollection<CreateOrderDetailDTO> OrderDetails { get; set; } = new List<CreateOrderDetailDTO>();



}
