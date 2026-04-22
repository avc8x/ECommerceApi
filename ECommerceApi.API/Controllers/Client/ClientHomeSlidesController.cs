using ECommerceApi.API.Extensions;
using ECommerceApi.Application.Features.SwiperSlides.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceApi.API.Controllers.Client;

[ApiController]
[Route("api/home-slides")]
[Tags("Client - Home Slides")]
public class ClientHomeSlidesController : ControllerBase
{
    private readonly IMediator _mediator;
    public ClientHomeSlidesController(IMediator mediator) => _mediator = mediator;

    [HttpGet("{lang}")]
    public async Task<IActionResult> GetByLang(string lang, CancellationToken ct)
        => (await _mediator.Send(new GetHomeSlidesByLangQuery(lang), ct)).ToActionResult(this);
}
