using MediatR;
using Serilog;

namespace Application.Common.Behaviors
{
    public class UnhandledExceptionBehavior<TRequest, TResponse>
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly ILogger _logger = Log.ForContext<TRequest>();

        public async Task<TResponse> Handle
        (
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken
        )
        {
            try { return await next(); }
            catch (Exception ex)
            {
                var requestName = typeof(TRequest).Name;
                _logger.Error
                (
                    "Error: {@message}. Request: Unhandled Exception for Request {@name} {@request}",
                    ex.Message,
                    requestName,
                    request
                );
                throw;
            }
        }
    }
}
