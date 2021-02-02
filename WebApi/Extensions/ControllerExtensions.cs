using FluentValidation.AspNetCore;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace bookstore_api.Extensions
{
    public static class ControllerExtensions
    {
        public static BadRequestObjectResult BadRequestFromValidationError(this ControllerBase controller, ValidationResult validationError)
        {
            validationError.AddToModelState(controller.ModelState, null);
            return controller.BadRequest(controller.ModelState);
        }
    }
}
