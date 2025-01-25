
using EG_ERP.Data.Repos;
using EG_ERP.Data.UoWs;
using EG_ERP.Models;
using Microsoft.AspNetCore.Mvc;
namespace EG_ERP.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomerController : ControllerBase
{
    private readonly IUnitOfWork unit;
    public CustomerController(IUnitOfWork unit)
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
        Customer customer = await repo.GetById(id);
        if (customer == null)
            return NotFound();
        else{
            ViewCustomerDTO viewCustomer = new ViewCustomerDTO
            {
                Id = customer.Uuid,
                Name = customer.Name,
                Address = customer.Address,
                Phone = customer.Phone,
                Email = customer.Email
            };
            return Ok(viewCustomer);
        }
        
    }

    [HttpGet("CustomerOrders/{id}")]
    public async Task<IActionResult> GetCustomerOrders(string id){
        IGenericRepository<Customer> customerRepo = unit.GetRepository<Customer>();
        Customer customer = await customerRepo.GetById(id,trackChanges:false,includes:["Orders"]);
        if(customer == null)
            return NotFound();
        else
            return Ok(customer.Orders);
        
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
        Customer customer = await repo.GetById(id);
        customer.Name = dto.Name;
        customer.Address = dto.Address;
        customer.Phone = dto.Phone;
        customer.Email = dto.Email;
        await repo.Update(customer);
        await unit.Commit();
        return Ok();
    }


    
}