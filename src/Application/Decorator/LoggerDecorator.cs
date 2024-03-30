using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace src.Application.Decorator;

public class LoggerDecorator<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> 
    where TRequest : IRequest<object>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        Console.WriteLine(request);

        return await next().ConfigureAwait(false);
    }
}
