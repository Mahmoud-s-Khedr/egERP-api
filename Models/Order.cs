using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EG_ERP.Utils;

namespace EG_ERP.Models;

public class Order : BaseEntity
{
    [Required]
    public DateTime OrderDate { get; set; }
    [Required]
    [Column(TypeName = "money")]
    public double Price { get; set; }
    [Required]
    public Status PaymentStatus { get; set; } = Status.Pending;
    [Required]
    public ShippingStatus ShippingStatus { get; set; } = ShippingStatus.Pending;

    [ForeignKey("Customer")]
    [Required]
    public int CustomerId { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    public virtual Customer? Customer { get; set; }
}
