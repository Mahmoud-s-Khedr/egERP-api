
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
        return Ok(customers);
    }
}