using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Workers.Application.Categories.Commands.CreateCategory;
using Workers.Application.Categories.Commands.DeleteCategory;
using Workers.Application.Categories.Commands.RestoreCategory;
using Workers.Application.Categories.Commands.UpdateCategory;
using Workers.Application.Categories.Queries.GetCategories;
using Workers.Application.Categories.Queries.GetCategoryById;

    namespace Workers.Api.Controllers;

    public class CategoriesController(IMediator mediator) : ApiControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get(
            [FromQuery] Guid? parentId,
            [FromQuery] string mode = "direct",
            [FromQuery] bool overpassIsDeleteFilter = false,
            CancellationToken cancellationToken = default)
        {
            var parsedMode = mode.Equals("all", StringComparison.OrdinalIgnoreCase)
                ? CategoryLoadMode.All
                : CategoryLoadMode.Direct;

            var data = await mediator.Send(
                new GetCategoriesQuery(parentId, parsedMode, overpassIsDeleteFilter), 
                cancellationToken);
            
            return OkResult(data);
        }

        [HttpGet("admin")]
        public async Task<IActionResult> GetForAdmin(
            [FromQuery] Guid? parentId,
            [FromQuery] string mode = "direct",
            CancellationToken cancellationToken = default)
        {
            var parsedMode = mode.Equals("all", StringComparison.OrdinalIgnoreCase)
                ? CategoryLoadMode.All
                : CategoryLoadMode.Direct;

            var data = await mediator.Send(
                new GetCategoriesQuery(parentId, parsedMode, OverpassIsDeleteFilter: true),
                cancellationToken);

            return OkResult(data);
        }

        [HttpGet("{categoryId:guid}")]
        public async Task<IActionResult> GetById(
            Guid categoryId,
            [FromQuery] string mode = "direct",
            [FromQuery] bool overpassIsDeleteFilter = false,
            CancellationToken cancellationToken = default)
        {
            var parsedMode = mode.Equals("all", StringComparison.OrdinalIgnoreCase)
                ? CategoryLoadMode.All
                : CategoryLoadMode.Direct;

            var data = await mediator.Send(
                new GetCategoryByIdQuery(categoryId, parsedMode, overpassIsDeleteFilter), 
                cancellationToken);
            
            return data is null
                ? NotFoundResult("Category not found")
                : OkResult(data);
        }

        [HttpPost] // Admin
        public async Task<IActionResult> Create(
            [FromBody] CreateCategoryCommand categoryCommand,
            CancellationToken cancellationToken = default)
        {
            var created = await mediator.Send(
                categoryCommand, 
                cancellationToken);
            
            return OkResult(created);
        }

        [HttpPost("{categoryId:guid}/restore")] // Admin
        public async Task<IActionResult> Restore(
            [FromRoute] Guid categoryId,
            CancellationToken cancellationToken = default)
        {
            await mediator.Send(
                new RestoreCategoryCommand(categoryId),
                cancellationToken);

            return OkResult(
                new
                {
                    message = "Restored"
                });
        }

        [HttpPut("{categoryId:guid}")] // Admin
        public async Task<IActionResult> Update(
            [FromRoute] Guid categoryId, 
            [FromBody] UpdateCategoryCommand categoryCommand, 
            CancellationToken cancellationToken = default)
        {
            if (categoryId != categoryCommand.Id) return BadRequestResult("Route id != body id");
            
            var updated = await mediator.Send(
                categoryCommand, 
                cancellationToken);
            
            return OkResult(updated);
        }

        [HttpDelete("{categoryId:guid}")] // Admin
        public async Task<IActionResult> Delete(
            [FromRoute] Guid categoryId, 
            CancellationToken cancellationToken = default)
        {
            await mediator.Send(
                new DeleteCategoryCommand(categoryId), 
                cancellationToken);
            
            return OkResult(
                new
                {
                    message = "Deleted"
                });
        }
    }

