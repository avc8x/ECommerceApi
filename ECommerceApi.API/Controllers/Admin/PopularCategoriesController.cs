using ECommerceApi.API.Extensions;
using ECommerceApi.Application.Features.PopularCategories.Commands;
using ECommerceApi.Application.Features.PopularCategories.DTOs;
using ECommerceApi.Application.Features.PopularCategories.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceApi.API.Controllers.Admin;

[ApiController]
[Route("api/admin/popular-categories")]
[Tags("Admin - Popular Categories")]
public class PopularCategoriesController : ControllerBase
{
    private readonly IMediator _mediator;
    public PopularCategoriesController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
        => (await _mediator.Send(new GetPopularCategoriesQuery(), ct)).ToActionResult(this);

    [HttpPost]
    public async Task<IActionResult> Add([FromBody] AddCategoryToPopularCategoriesCommand command, CancellationToken ct)
        => (await _mediator.Send(command, ct)).ToActionResult(this);

    [HttpPut("order")]
    public async Task<IActionResult> UpdateOrder([FromBody] IEnumerable<NewItemOrderDto> orders, CancellationToken ct)
        => (await _mediator.Send(new ChangePopularCategoryOrderCommand(orders), ct)).ToActionResult(this);

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Remove(Guid id, CancellationToken ct)
        => (await _mediator.Send(new RemoveCategoryFromPopularCategoriesCommand(id), ct)).ToActionResult(this);
}
