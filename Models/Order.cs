using System.ComponentModel.DataAnnotations;
public enum OrderStatus { Pending, Shipped, Delivered };
public class Order:BaseEntity{


    [Required]
    public DateTime OrderDate { get; set; }
    [Required]
    public double Price { get; set; }
    public OrderStatus Status { get; set; } = OrderStatus.Pending;

}