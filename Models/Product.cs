using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace EG_ERP.Models;
public class Product : BaseEntity
{
    [StringLength(255,MinimumLength = 1, ErrorMessage = "")]
    [Required]
    public required string Name { get; set; }
    public string? Description { get; set; }
    [Required]
    public decimal Price { get; set; }

    [ForeignKey("Category")]
    public int? CategoryId { get; set; }

    public virtual Category? Category { get; set; }
    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}
