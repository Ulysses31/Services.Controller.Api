namespace Services.Controllers.API.Controllers;

using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

/// <summary>
/// Controller for handling errors in the application. 
/// </summary>
/// <remarks>Error Controller</remarks>
[ApiExplorerSettings(IgnoreApi = true)]
public class ErrorController : ControllerBase
{
  /// <summary>
  /// Handles errors in the application. 
  /// </summary>
  /// <remarks>Handle Production Error</remarks>
  /// <returns>ObjectResult</returns>
  [Route("/error")]
  [ProducesResponseType<ObjectResult>(StatusCodes.Status500InternalServerError, "application/json", ["application/xml"])]
  public IActionResult HandleError() =>
    Problem();

  /// <summary>
  /// Handles development errors in the application. 
  /// </summary>
  /// <remarks>Handle Development Error</remarks>
  /// <returns>ObjectResult</returns>
  [Route("/error-development")]
  [ProducesResponseType<ObjectResult>(StatusCodes.Status500InternalServerError, "application/json", ["application/xml"])]
  public IActionResult HandleErrorDevelopment(
    [FromServices] IHostEnvironment hostEnvironment
  )
  {
    if (!hostEnvironment.IsDevelopment())
    {
      return NotFound();
    }

    var exceptionHandlerFeature =
        HttpContext.Features.Get<IExceptionHandlerFeature>()!;

    return Problem(
        detail: exceptionHandlerFeature.Error.StackTrace,
        title: exceptionHandlerFeature.Error.Message);
  }
}
