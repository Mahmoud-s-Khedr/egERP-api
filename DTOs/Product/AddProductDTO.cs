using System;

namespace EG_ERP.DTOs.Product;

public class AddProductDTO
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public required decimal Price { get; set; }
    public required string CategoryId { get; set; }
}
