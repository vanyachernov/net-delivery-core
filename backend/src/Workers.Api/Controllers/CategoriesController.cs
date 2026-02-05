    using MediatR;
    using Microsoft.AspNetCore.Mvc;
using Workers.Application.Categories.Commands;
using Workers.Application.Categories.Commands.CreateCategory;
using Workers.Application.Categories.Commands.DeleteCagtegory;
using Workers.Application.Categories.Commands.UpdateCategory;
using Workers.Application.Categories.Queries.GetCategories;
using Workers.Application.Categories.Queries.GetCategoryById;
using Workers.Domain.Entities.Categories;

    namespace Workers.Api.Controllers;

    [ApiController]
    [Route("api/categories")]
    public class CategoriesController(IMediator mediator) : ApiControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get(
            [FromQuery] Guid? parentId,
            [FromQuery] string mode = "direct",
            CancellationToken ct = default)
        {
            var parsedMode = mode.Equals("all", StringComparison.OrdinalIgnoreCase)
                ? CategoryLoadMode.All
                : CategoryLoadMode.Direct;

            var data = await mediator.Send(new GetCategoriesQuery(parentId, parsedMode), ct);
            return OkResult(data);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(
            Guid id,
            [FromQuery] string mode = "direct",
            CancellationToken ct = default)
        {
            var parsedMode = mode.Equals("all", StringComparison.OrdinalIgnoreCase)
                ? CategoryLoadMode.All
                : CategoryLoadMode.Direct;

            var data = await mediator.Send(new GetCategoryByIdQuery(id, parsedMode), ct);
            return data is null
                ? NotFoundResult(new { message = "Category not found" })
                : OkResult(data);
        }

        [HttpPost] // Admin
        public async Task<IActionResult> Create(
            [FromBody] CreateCategoryCommand cmd,
            CancellationToken ct)
        {
            var created = await mediator.Send(cmd, ct);
            return OkResult(created);
        }

        [HttpPut("{id:guid}")] // Admin
        public async Task<IActionResult> Update(
            Guid id, 
            [FromBody] UpdateCategoryCommand cmd, 
            CancellationToken ct)
        {
            if (id != cmd.Id) return BadRequestResult(new { message = "Route id != body id" });
            var updated = await mediator.Send(cmd, ct);
            return OkResult(updated);
        }

        [HttpDelete("{id:guid}")] // Admin
        public async Task<IActionResult> Delete(
            Guid id, 
            CancellationToken ct)
        {
            await mediator.Send(new DeleteCategoryCommand(id), ct);
            return OkResult(new { message = "Deleted" });
        }
    }

