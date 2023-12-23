using Microsoft.AspNetCore.Mvc.Filters;

namespace MicUI.WorkManagement.Helper.Extensions
{
    public class BPAActionFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // Reference :- https://stackoverflow.com/questions/41700759/httpcontext-response-cache-equivalent-in-net-core
            filterContext.HttpContext.Response.Headers["Expires"] = "-1";
            filterContext.HttpContext.Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
            filterContext.HttpContext.Response.Headers["Pragma"] = "no-cache";
            filterContext.HttpContext.Response.Headers["x-frame-options"] = "DENY";
            base.OnActionExecuting(filterContext);
        }
    }   
}
