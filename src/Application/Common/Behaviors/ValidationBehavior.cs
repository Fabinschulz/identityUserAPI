using FluentValidation;
using FluentValidation.Results;
using IdentityUser.src.Application.Common.Exceptions;
using MediatR;

namespace IdentityUser.src.Application.Common.Behaviors
{
    /// <summary>
    /// Behavior for validating requests.
    /// </summary>
    /// <typeparam name="TRequest">The type of the request.</typeparam>
    /// <typeparam name="TResponse">The type of the response.</typeparam>
    public sealed class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationBehavior{TRequest, TResponse}"/> class.
        /// </summary>
        /// <param name="validators">The validators to use for validating the request.</param>
        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators) => _validators = validators;

        /// <summary>
        /// Handles the request and validates it using the provided validators.
        /// </summary>
        /// <param name="request">The request to handle.</param>
        /// <param name="next">The next delegate in the pipeline.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The response from the next delegate in the pipeline.</returns>
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var context = new ValidationContext<TRequest>(request);
            var validationFailures = new List<ValidationFailure>();

            foreach (var validator in _validators)
            {
                var validationResult = await validator.ValidateAsync(context, cancellationToken);
                if (!validationResult.IsValid)
                {
                    validationFailures.AddRange(validationResult.Errors);
                }
            }

            if (validationFailures.Any())
            {
                var errorMessages = validationFailures.Select(e => e.ErrorMessage).ToArray();
                throw new BadRequestException(errorMessages);
            }

            return await next();
        }

    }
}
