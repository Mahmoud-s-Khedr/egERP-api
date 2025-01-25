using EG_ERP.Data.Repos;
using EG_ERP.Data.UoWs;
using EG_ERP.DTOs.Category;
using EG_ERP.DTOs.Product;
using EG_ERP.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EG_ERP.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoriesController : ControllerBase
{
    private readonly IUnitOfWork unit;
    public CategoriesController(IUnitOfWork unit)
    {
        this.unit = unit;
    }

    [HttpGet]
    public async Task<IActionResult> GetCategories()
    {
        IGenericRepository<Category> repo = unit.GetRepository<Category>();
        List<Category> categories = await repo.GetAll();

        List<ViewCategoryDTO> views = categories.Select(category => new ViewCategoryDTO
        {
            Id = category.Uuid,
            Name = category.Name
        }).ToList();

        return Ok(views);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCategory(string id)
    {
        IGenericRepository<Category> repo = unit.GetRepository<Category>();
        Category? category = await repo.GetById(id);

        if (category == null)
        {
            return NotFound();
        }

        ViewCategoryDTO view = new ViewCategoryDTO
        {
            Id = category.Uuid,
            Name = category.Name
        };

        return Ok(view);
    }

    [HttpGet("{id}/products")]
    public async Task<IActionResult> GetProductsByCategory(string id)
    {
        IGenericRepository<Category> repo = unit.GetRepository<Category>();
        Category? category = await repo.GetById(id, includes: new[] { "Products" });

        if (category == null)
            return NotFound("Category not found");

        List<ViewProductDTO> views = category.Products.Select(product => new ViewProductDTO
        {
            Id = product.Uuid,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            Category = category.Name
        }).ToList();

        return Ok(views);
    }

    [HttpPost]
    public async Task<IActionResult> CreateCategory(AddCategoryDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        Category category = new Category
        {
            Name = dto.Name
        };

        IGenericRepository<Category> repo = unit.GetRepository<Category>();
        await repo.Add(category);
        await unit.Commit();

        ViewCategoryDTO view = new ViewCategoryDTO
        {
            Id = category.Uuid,
            Name = category.Name
        };

        return CreatedAtAction(nameof(GetCategory), new { id = category.Uuid }, view);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCategory(string id, AddCategoryDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        IGenericRepository<Category> repo = unit.GetRepository<Category>();
        Category? category = await repo.GetById(id);

        if (category == null)
            return NotFound("Category not found");

        category.Name = dto.Name;

        await repo.Update(category);
        await unit.Commit();

        return NoContent();
    }

    [HttpPatch("{id}/products")]
    public async Task<IActionResult> SetProducts(string id, List<string> productIds)
    {
        IGenericRepository<Category> categoryRepo = unit.GetRepository<Category>();
        Category? category = await categoryRepo.GetById(id);

        if (category == null)
            return NotFound("Category not found");

        IGenericRepository<Product> productRepo = unit.GetRepository<Product>();
        List<Product> products = await productRepo.GetAll(filter: new HashSet<string>(productIds), trackChanges: false);

        #region First approach
        foreach (Product product in products)
        {
            product.CategoryId = category.Id;
        }
        #endregion

        #region Second approach
        // category.Products = products;
        #endregion

        await unit.Commit();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategory(string id)
    {
        IGenericRepository<Category> repo = unit.GetRepository<Category>();
        Category? category = await repo.GetById(id);

        if (category == null)
            return NotFound("Category not found");

        await repo.Delete(category);
        await unit.Commit();

        return NoContent();
    }
}

