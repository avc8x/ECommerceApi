using ECommerceApi.API.Extensions;
using ECommerceApi.Application.Features.SwiperSlides.Commands;
using ECommerceApi.Application.Features.SwiperSlides.DTOs;
using ECommerceApi.Application.Features.SwiperSlides.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceApi.API.Controllers.Admin;

[ApiController]
[Route("api/admin/home-slides")]
[Tags("Admin - Home Slides")]
public class HomeSlidesController : ControllerBase
{
    private readonly IMediator _mediator;
    public HomeSlidesController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
        => (await _mediator.Send(new GetHomeSlidesQuery(), ct)).ToActionResult(this);

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateNewSlideCommand command, CancellationToken ct)
        => (await _mediator.Send(command, ct)).ToActionResult(this);

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateSlideBody body, CancellationToken ct)
    {
        var command = new UpdateSlideCommand(id, body.ImageUrl, body.CategoryId);
        return (await _mediator.Send(command, ct)).ToActionResult(this);
    }

    [HttpPut("{slideId:guid}/translations/{languageCode}")]
    public async Task<IActionResult> UpdateTranslation(
        Guid slideId, string languageCode,
        [FromBody] UpdateSlideTranslationBody body, CancellationToken ct)
    {
        var command = new UpdateSlideTranslationCommand(
            slideId, languageCode,
            body.TopText, body.BigTitle,
            body.HighlightedTitleNormal, body.HighlightedTitleColor,
            body.HighlightedTitleBold, body.BottomText);
        return (await _mediator.Send(command, ct)).ToActionResult(this);
    }

    [HttpPut("order")]
    public async Task<IActionResult> UpdateOrder([FromBody] IEnumerable<NewItemOrderDto> orders, CancellationToken ct)
        => (await _mediator.Send(new ChangeSlideOrderCommand(orders), ct)).ToActionResult(this);

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
        => (await _mediator.Send(new DeleteSlideCommand(id), ct)).ToActionResult(this);
}

public record UpdateSlideBody(string ImageUrl, Guid CategoryId);

public record UpdateSlideTranslationBody(
    string TopText, string BigTitle,
    string HighlightedTitleNormal, string HighlightedTitleColor,
    string? HighlightedTitleBold, string BottomText);
