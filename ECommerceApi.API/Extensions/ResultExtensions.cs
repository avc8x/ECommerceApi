using ECommerceApi.Application.Common.Results;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceApi.API.Extensions;

/// <summary>
/// Maps Result pattern to IActionResult HTTP responses.
/// Keeps controllers thin — all they do is dispatch and translate.
/// </summary>
public static class ResultExtensions
{
    public static IActionResult ToActionResult<T>(this Result<T> result, ControllerBase controller)
    {
        if (!result.IsSuccess)
        {
            return result.StatusCode switch
            {
                404 => controller.NotFound(new { error = result.Error }),
                409 => controller.Conflict(new { error = result.Error }),
                _ => controller.BadRequest(new { error = result.Error })
            };
        }
        return result.StatusCode == 201
            ? controller.StatusCode(201, result.Value)
            : controller.Ok(result.Value);
    }

    public static IActionResult ToActionResult(this Result result, ControllerBase controller)
    {
        if (!result.IsSuccess)
        {
            return result.StatusCode switch
            {
                404 => controller.NotFound(new { error = result.Error }),
                _ => controller.BadRequest(new { error = result.Error })
            };
        }
        return controller.NoContent();
    }
}
