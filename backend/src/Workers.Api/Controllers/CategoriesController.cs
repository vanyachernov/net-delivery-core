using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Workers.Domain.Entities.Categories;

namespace Workers.Api.Controllers;

[ApiController]
[AllowAnonymous]
[Authorize]
public class CategoriesController : ApiControllerBase
{
    [HttpGet]
    public IActionResult GetAll()
    {
        var categories = new List<Category>
        {
            new()
            {
                Id = Guid.NewGuid(), 
                Name = "Electrical", 
                Slug = "electrical", 
                CreatedAt = DateTime.UtcNow
            }
        };

        return OkResult(categories);
    }

    [HttpGet("{id:guid}")]
    public IActionResult GetById(Guid id)
    {
        if (id == Guid.Empty)
        {
            return NotFoundResult($"Category with ID {id} not found");
        }

        var category = new Category
        {
            Id = id, 
            Name = "Test Category", 
            Slug = "test"
        };
        
        return OkResult(category);
    }
}
