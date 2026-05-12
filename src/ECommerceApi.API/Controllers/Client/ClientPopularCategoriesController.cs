using ECommerceApi.API.Extensions;
using ECommerceApi.Application.Features.PopularCategories.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceApi.API.Controllers.Client;

[ApiController]
[Route("api/popular-categories")]
[Tags("Client - Popular Categories")]
public class ClientPopularCategoriesController : ControllerBase
{
    private readonly IMediator _mediator;
    public ClientPopularCategoriesController(IMediator mediator) => _mediator = mediator;

    [HttpGet("{lang}")]
    public async Task<IActionResult> GetByLang(string lang, CancellationToken ct)
        => (await _mediator.Send(new GetPopularCategoriesQuery(lang), ct)).ToActionResult(this);
}
