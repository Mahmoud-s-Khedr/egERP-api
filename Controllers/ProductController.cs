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

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProduct(string id)
    {
        IGenericRepository<Product> repo = unit.GetRepository<Product>();
        Product? product = await repo.GetById(id, includes : new string[] { "Category" });

        if (product == null)
            return NotFound("Product not found");

        ViewProductDTO view = new ViewProductDTO
        {
            Id = product.Uuid,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            Category = product.Category?.Name ?? "No Category"
        };

        return Ok(view);
    }

    [HttpPost]
    public async Task<IActionResult> CreateProduct(AddProductDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        IGenericRepository<Category> categoryRepo = unit.GetRepository<Category>();
        Category? category = await categoryRepo.GetById(dto.CategoryId);

        if (category == null)
            return BadRequest("Category not found");

        IGenericRepository<Product> productRepo = unit.GetRepository<Product>();
        Product product = new Product
        {
            Name = dto.Name,
            Description = dto.Description,
            Price = dto.Price,
            Category = category
        };

        await productRepo.Add(product);
        await unit.Commit();

        ViewProductDTO view = new ViewProductDTO
        {
            Id = product.Uuid,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            Category = category.Name
        };

        return CreatedAtAction(nameof(GetProduct), new { id = product.Uuid }, product);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct(string id, UpdateProductDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        IGenericRepository<Product> repo = unit.GetRepository<Product>();
        Product? product = await repo.GetById(id);

        if (product == null)
            return NotFound("Product not found");

        IGenericRepository<Category> categoryRepo = unit.GetRepository<Category>();

        Category? category = dto.CategoryId != null ? await categoryRepo.GetById(dto.CategoryId) : null;

        if (category == null && dto.CategoryId != null)
            return BadRequest("Category not found");

        product.Name = dto.Name ?? product.Name;
        product.Description = dto.Description ?? product.Description;
        product.Price = dto.Price ?? product.Price;
        product.CategoryId = category?.Id ?? product.CategoryId;
        
        await repo.Update(product);
        await unit.Commit();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(string id)
    {
        IGenericRepository<Product> repo = unit.GetRepository<Product>();
        Product? product = await repo.GetById(id);

        if (product == null)
            return NotFound("Product not found");

        await repo.Delete(product);
        await unit.Commit();

        return NoContent();
    }
}

