using System;

namespace EG_ERP.DTOs.Product;

public class UpdateProductDTO
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public decimal? Price { get; set; }
    public string? CategoryId { get; set; }
}
