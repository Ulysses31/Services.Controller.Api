using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Controllers.API.Services;

namespace MyApp.Namespace
{
  [ApiController]
  [ApiVersion("1.0")]
  [Route("api/v{version:apiVersion}/[controller]")]
  public class HealthChecksDbController : ControllerBase
  {
    private readonly HealthCheckDbRepo _services;

    public HealthChecksDbController(
      HealthCheckDbRepo services
    )
    {
      _services = services;
    }

    [HttpGet("test-database")]
    [MapToApiVersion("1.0")]
    //[EndpointName("HealthCheckDb")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> HealthCheckTestDatabase()
    {
      string result = await _services.TestSqlScript();
      return Ok(result);

    }
  }
}
