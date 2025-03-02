using FluentValidation;
using MediatR;

namespace Application.Common.Behaviors
{
    public class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull
    {
        public async Task<TResponse> Handle
        (
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken
        )
        {
            if (validators.Any())
            {
                var context = new ValidationContext<TRequest>(request);

                var validationResults = await Task.WhenAll
                (
                    validators.Select
                    (
                        validator =>
                            validator.ValidateAsync(context, cancellationToken)
                    )
                );

                var failures = validationResults
                    .Where(result => result.Errors.Count is not 0)
                    .SelectMany(result => result.Errors)
                    .ToList();

                if (failures.Count is not 0)
                {
                    throw new ValidationException(failures);
                }
            }
            return await next();
        }
    }
}
