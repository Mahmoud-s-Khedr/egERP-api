using System.Security.Cryptography.X509Certificates;
using EG_ERP.Data.Repos;
using EG_ERP.Data.UoWs;
using EG_ERP.DTOs.Order;
using EG_ERP.DTOs.Product;
using EG_ERP.Models;
using EG_ERP.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace EG_ERP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IUnitOfWork unit;
        public OrdersController(IUnitOfWork unit)
        {
            this.unit = unit;
        }

        [HttpGet]
        public async Task<IActionResult> GetOrders(){
            IGenericRepository<Order> repo = unit.GetRepository<Order>();
            List<Order> orders = await repo.GetAll();
            List<ViewOrderDTO> viewOrders = orders.Select(o => new ViewOrderDTO
            {
                Id = o.Uuid,
                OrderDate = o.OrderDate,
                Total = o.Price,
                PaymentStatus = o.PaymentStatus,
                ShippingStatus = o.ShippingStatus,
            }).ToList();
            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrder(string id){
            IGenericRepository<Order> repo = unit.GetRepository<Order>();
            Order order = await repo.GetById(id, includes: new[] { "OrderDetails", "OrderPayments" });
            if (order == null)
                return NotFound();
            else{
                ViewOrderDTO viewOrder = new ViewOrderDTO{
                    Id = order.Uuid,
                    OrderDate = order.OrderDate,
                    Total = order.Price,
                    PaymentStatus = order.PaymentStatus,
                    ShippingStatus = order.ShippingStatus,
                    OrderDetails = order.OrderDetails.Select(od => new VeiwOrderDetailDTO
                    {
                        Product = new ViewProductDTO{
                            Id = od.Product.Uuid,
                            Name = od.Product.Name,
                            Price = od.Product.Price,
                            Category = od.Product.Category.Name,
                            Description = od.Product.Description

                        },
                        Quantity = od.Quantity,
                        UnitPrice = od.UnitPrice
                    }).ToList(),
                    OrderPayments = order.OrderPayments.Select(
                        pay => {
                            return new ViewPaymentDTO{
                                Amount = pay.Payment.Amount,
                                PaymentDate = pay.Payment.PaymentDate,
                                Description = pay.Payment.Description,
                                SerialNumber = pay.Payment.SerialNumber
                            };
                        }
                    )

                };
                return Ok(viewOrder);
                }
            
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder(CreateOrderDTO createOrderDTO){
            IGenericRepository<Customer> customerRepo = unit.GetRepository<Customer>();
            Customer customer = await customerRepo.GetById(createOrderDTO.CustomerId);
            IGenericRepository<Order> orderRepo = unit.GetRepository<Order>();
            Order order = new Order{
                OrderDate = DateTime.Now,
                Price = createOrderDTO.Price,
                PaymentStatus = Status.Pending,
                ShippingStatus = ShippingStatus.Pending,
                Customer = customer
            };

            foreach(CreateOrderDetailDTO detail in createOrderDTO.OrderDetails){
                IGenericRepository<Product> productRepo = unit.GetRepository<Product>();
                Product product = await productRepo.GetById(detail.ProductId);
                OrderDetail orderDetail = new OrderDetail{
                    Product = product,
                    Quantity = detail.Quantity,
                    UnitPrice = product.Price
                };
                order.OrderDetails.Add(orderDetail);
            }


            
            await orderRepo.Add(order);
            await unit.Commit();
            return Ok();
        }

        [HttpPost("{id}/ChangeShippingStatus")]
        public async Task<IActionResult> ChangeShippingStatus(string id, ShippingStatus shippingStatus){
            IGenericRepository<Order> repo = unit.GetRepository<Order>();
            Order order = await repo.GetById(id);
            order.ShippingStatus = shippingStatus;
            await repo.Update(order);
            await unit.Commit();
            return Ok();
        }

        [HttpPost("{id}/ChangeOrderDetail")]
        public async Task<IActionResult> ChangeOrderDetail(string id, CreateOrderDetailDTO createOrderDetailDTO){
            IGenericRepository<Order> repo = unit.GetRepository<Order>();
            Order order = await repo.GetById(id, includes: new[] { "OrderDetails" });
            OrderDetail orderDetail = order.OrderDetails.FirstOrDefault(od => od.Product.Uuid == createOrderDetailDTO.ProductId);
            orderDetail.Quantity = createOrderDetailDTO.Quantity;
            await repo.Update(order);
            await unit.Commit();
            return Ok();
        }

        
    }


}
