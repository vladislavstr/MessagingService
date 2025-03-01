using MediatR;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace Api.Controllers.Base;

[ApiController]
[Route("api/[controller]")]
public abstract class BaseController<TController>(ISender sender) : ControllerBase
{
    protected readonly Serilog.ILogger Logger = Log.ForContext<TController>();

    protected virtual async Task<IActionResult> Action<TCommandOrQuery>(TCommandOrQuery commandOrQuery, CancellationToken cancellationToken)
    where TCommandOrQuery : class
    {
        try
        {
            var result = await sender.Send(commandOrQuery, cancellationToken);
            return Ok(result);
        }
        catch (Exception ex)
        {
            Logger.Error("Message: {Message}", ex.Message);
            return BadRequest(ex.Message);
        }
    }
}