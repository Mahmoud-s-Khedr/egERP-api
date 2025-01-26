
using EG_ERP.Data.Repos;
using EG_ERP.Data.UoWs;
using EG_ERP.DTOs.Order;
using EG_ERP.DTOs.Product;
using EG_ERP.Models;
using Microsoft.AspNetCore.Mvc;
namespace EG_ERP.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase
{
    private readonly IUnitOfWork unit;
    public CustomersController(IUnitOfWork unit)
    {
        this.unit = unit;
    }
    [HttpGet]
    public async Task<IActionResult> GetCustomers()
    {
        IGenericRepository<Customer> repo = unit.GetRepository<Customer>();
        List<Customer> customers = await repo.GetAll();
        List<ViewCustomerDTO> viewCustomers = customers.Select(c => new ViewCustomerDTO
        {
            Id = c.Uuid,
            Name = c.Name,
            Address = c.Address,
            Phone = c.Phone,
            Email = c.Email
        }).ToList();
        return Ok(customers);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCustomer(string id)
    {
        IGenericRepository<Customer> repo = unit.GetRepository<Customer>();
        Customer? customer = await repo.GetById(id);
        if (customer == null)
            return NotFound();
        else{
            ViewCustomerDTO viewCustomer = new ViewCustomerDTO
            {
                Id = customer.Uuid,
                Name = customer.Name,
                Address = customer.Address ?? "",
                Phone = customer.Phone ?? "",
                Email = customer.Email ?? ""
            };
            return Ok(viewCustomer);
        }
        
    }

    [HttpGet("{id}/Orders")]
    public async Task<IActionResult> GetCustomerOrders(string id)
    {
        IGenericRepository<Customer> repo = unit.GetRepository<Customer>();
        Customer? customer = await repo.GetById(id,includes: new[] { "Orders" });
        if (customer == null)
            return NotFound();
        else{
            List<ViewOrderDTO> orders = customer.Orders
            .Select(o => new ViewOrderDTO
            {
                Id = o.Uuid,
                OrderDate = o.OrderDate,
                Total = o.Price,
                PaymentStatus = o.PaymentStatus,
                ShippingStatus = o.ShippingStatus,
                OrderDetails = o.OrderDetails.Select(od => new VeiwOrderDetailDTO
                {
                    Product = new ViewProductDTO{
                        Id = od.Product?.Uuid,
                        Name = od.Product?.Name,
                        Price = od.Product?.Price ?? 0,
                    }

                }).ToList()
            }).ToList();
            return Ok(orders);
                
            }
            
    }
    

    [HttpPost]
    public async Task<IActionResult> AddCustomer(AddCustomerDTO dto)
    {
        IGenericRepository<Customer> repo = unit.GetRepository<Customer>();
        Customer customer = new Customer
        {
            Uuid = Guid.NewGuid().ToString(),
            Name = dto.Name,
            Address = dto.Address,
            Phone = dto.Phone,
            Email = dto.Email
        };
        await repo.Add(customer);
        await unit.Commit();
        return Ok();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCustomer(string id, AddCustomerDTO dto)
    {
        IGenericRepository<Customer> repo = unit.GetRepository<Customer>();
        Customer? customer = await repo.GetById(id);
        if (customer == null)
            return NotFound();

        customer.Name = dto.Name;
        customer.Address = dto.Address;
        customer.Phone = dto.Phone;
        customer.Email = dto.Email;
        await repo.Update(customer);
        await unit.Commit();
        return Ok();
    }


    
}