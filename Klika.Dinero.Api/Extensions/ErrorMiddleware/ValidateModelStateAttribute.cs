using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Collections.Generic;
using Klika.Dinero.Model.Errors;
using Klika.Dinero.Model.Response;

namespace Klika.Dinero.Api.Extensions.ErrorMiddleware
{
    public class ValidateModelStateAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                List<ApiError> validationErrors = new List<ApiError>();
                foreach (var modelState in context.ModelState.Values)
                    foreach (var error in modelState.Errors)
                        validationErrors.Add(new ApiError(ErrorCodes.InvalidFormat, error.ErrorMessage));

                context.Result = new JsonResult(new ApiErrorResponse(validationErrors))
                {
                    StatusCode = StatusCodes.Status400BadRequest
                };
            }
        }
    }
}
