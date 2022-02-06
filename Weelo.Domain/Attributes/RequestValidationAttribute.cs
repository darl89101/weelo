using Microsoft.AspNetCore.Mvc.Filters;
using Weelo.Domain.Exceptions;

namespace Weelo.Domain.Attributes;

/// <summary>
/// Validate request and responses of the api if the validator exists
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
public sealed class RequestValidationAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            var errors = context.ModelState.Values
                .SelectMany(x => x.Errors.Select(e => e.ErrorMessage)).ToList();

            throw new BadRequestException(
                string.Format("Data validation has failed with {0} {1}",
                context.ModelState.ErrorCount, (context.ModelState.ErrorCount > 1) ? "errors" : "error"),
                errors
            );
        }
    }
}
