
using EG_ERP.DTOs.Product;
namespace EG_ERP.DTOs.Order;
public class VeiwOrderDetailDTO{

    public ViewProductDTO Product {get; set; }

    public int Quantity {get; set; }

    public decimal UnitPrice {get; set; }



}