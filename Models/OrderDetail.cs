

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class OrderDetail:BaseEntity
{

    [Required]
    [ForeignKey("Order")]
    public int OrderId { get; set; }
    [Required]
    [ForeignKey("Product")]
    public int ProductId { get; set; }
    [Required]
    public int Quantity { get; set; }
    [Required]
    [Column(TypeName = "money")]
    public double UnitPrice { get; set; }
    public virtual Order? Order { get; set; }
    public virtual Product? Product { get; set; }
}