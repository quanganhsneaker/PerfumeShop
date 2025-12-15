using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace PerfumeShop.Presentation.Filters
{
    public class ValidationExceptionFilter : IExceptionFilter
    {
        private readonly ITempDataDictionaryFactory _tempDataFactory;

        public ValidationExceptionFilter(ITempDataDictionaryFactory tempDataFactory)
        {
            _tempDataFactory = tempDataFactory;
        }

        public void OnException(ExceptionContext context)
        {
            if (context.Exception is ValidationException ex)
            {
                var tempData = _tempDataFactory.GetTempData(context.HttpContext);

                tempData["error"] = ex.Errors.First().ErrorMessage;

                context.Result = new RedirectToActionResult(
                    context.RouteData.Values["action"]!.ToString(),
                    context.RouteData.Values["controller"]!.ToString(),
                    context.RouteData.Values
                );

                context.ExceptionHandled = true;
            }
        }
    }
}
