using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using System.Text.Json;

namespace CQRS.MediatR.WebApi
{
    public class ValidationExceptionHandler : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            if (exception is ValidationException ex)
            {
                var errors = ex.Errors.Select(x => new { x.PropertyName, x.ErrorMessage }).ToList();
                httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                await httpContext.Response.WriteAsync(JsonSerializer.Serialize(errors));
            }
            else
            {
                throw exception; 
            }
            return true;
        }
    }
}
