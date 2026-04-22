using ECommerceApi.API.Extensions;
using ECommerceApi.Application.Features.Categories.Commands;
using ECommerceApi.Application.Features.Categories.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceApi.API.Controllers.Admin;

[ApiController]
[Route("api/admin/categories")]
[Tags("Admin - Categories")]
public class CategoriesController : ControllerBase
{
    private readonly IMediator _mediator;
    public CategoriesController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> GetTree(CancellationToken ct)
        => (await _mediator.Send(new GetCategoriesTreeQuery(), ct)).ToActionResult(this);

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateNewCategoryCommand command, CancellationToken ct)
        => (await _mediator.Send(command, ct)).ToActionResult(this);

    [HttpPost("{categoryId:guid}/translations/{languageCode}")]
    public async Task<IActionResult> AddOrUpdateTranslation(
        Guid categoryId, string languageCode,
        [FromBody] AddOrUpdateCategoryTranslationRequest body, CancellationToken ct)
    {
        var command = new AddOrUpdateCategoryTranslationCommand(
            categoryId, languageCode, body.Title, body.Description, body.MetaTitle, body.MetaDescription);
        return (await _mediator.Send(command, ct)).ToActionResult(this);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateCategoryCommand command, CancellationToken ct)
        => (await _mediator.Send(command, ct)).ToActionResult(this);

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
        => (await _mediator.Send(new SoftDeleteCategoryCommand(id), ct)).ToActionResult(this);
}

public record AddOrUpdateCategoryTranslationRequest(
    string Title, string? Description, string? MetaTitle, string? MetaDescription);
