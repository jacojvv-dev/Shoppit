using ApplicationCore.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace API.Filters
{
    public class ApplicationExceptionFilter : IActionFilter, IOrderedFilter
    {
        public int Order { get; set; } = int.MaxValue - 10;

        public void OnActionExecuting(ActionExecutingContext context)
        {
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            switch (context.Exception)
            {
                case ApplicationDataNotFoundException notFoundException:
                    context.Result = new NotFoundObjectResult(notFoundException.Message);
                    context.ExceptionHandled = true;
                    break;
                case ApplicationInvalidOperationException applicationInvalidOperationException:
                    context.Result = new BadRequestObjectResult(applicationInvalidOperationException.Message);
                    context.ExceptionHandled = true;
                    break;
            }
        }
    }
}