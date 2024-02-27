using GeolocationAPI.SAPComponents;
using Microsoft.AspNetCore.Mvc.Filters;

namespace GeolocationAPI.Filters
{
    public class SAPConnectionFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var connector = (SAPConnector)context.HttpContext.RequestServices.GetService(typeof(SAPConnector));
            connector.Connect();
        }
    }
}
