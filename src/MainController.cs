using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using src.Application.UseCase;

namespace src;

[ApiController]
public class MainController : ControllerBase
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMediator _mediator;

    public MainController(
        IHttpContextAccessor httpContextAccessor,
        IMediator mediator)
    {
        _httpContextAccessor = httpContextAccessor;
        _mediator = mediator;
    }

    [HttpPost("generate_invoices")]
    public async Task<object> Post([FromBody] Input input, CancellationToken cancellationToken)
    {
        HttpContext httpContext = _httpContextAccessor.HttpContext;

        var userAgent = httpContext?.Request.Headers["User-Agent"]; 
        var host = httpContext?.Request.Host;

        input.DefineRequestResponsible(userAgent?.ToString(), host?.ToString());

        var output = await _mediator.Send(input, cancellationToken);

        return output;
    }
}
