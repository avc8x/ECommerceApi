using ECommerceApi.API.Extensions;
using ECommerceApi.Application.Features.Categories.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceApi.API.Controllers.Client;

[ApiController]
[Route("api/categories")]
[Tags("Client - Categories")]
public class ClientCategoriesController : ControllerBase
{
    private readonly IMediator _mediator;
    public ClientCategoriesController(IMediator mediator) => _mediator = mediator;

    [HttpGet("{lang}")]
    public async Task<IActionResult> GetByLang(string lang, CancellationToken ct)
        => (await _mediator.Send(new GetCategoryTreeByLangQuery(lang), ct)).ToActionResult(this);
}
