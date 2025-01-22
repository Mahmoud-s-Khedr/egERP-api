using EG_ERP.Data.Repos;
using EG_ERP.Data.UoWs;
using EG_ERP.DTOs.Product;
using EG_ERP.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EG_ERP.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly IUnitOfWork unit;

    public ProductController(IUnitOfWork unit)
    {
        this.unit = unit;
    }

    [HttpGet]
    public async Task<IActionResult> GetProducts()
    {
        IGenericRepository<Product> repo = unit.GetRepository<Product>();
        List<Product> products = await repo.GetAll(includes : new string[] { "Category" });

        List<ViewProductDTO> views = products.Select(product => new ViewProductDTO
        {
            Id = product.Uuid,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            Category = product.Category?.Name ?? "No Category"
        }).ToList();

        return Ok(views);
    }
}

