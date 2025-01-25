
using EG_ERP.DTOs.Product;
using EG_ERP.Utils;
namespace EG_ERP.DTOs.Order;

public class ViewOrderDTO
{
    public string? Id { get; set; }

    public DateTime OrderDate { get; set; }

    public decimal Total { get; set; }

    public Status PaymentStatus { get; set; }

    public ShippingStatus ShippingStatus { get; set; }

    public ICollection<VeiwOrderDetailDTO> OrderDetails
     { get; set; } = new List<VeiwOrderDetailDTO>();

    public IEnumerable<ViewPaymentDTO> OrderPayments
     { get; set; } = new List<ViewPaymentDTO>();

}