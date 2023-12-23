using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MicUI.WorkManagement.Helper.Sessions;
using MicUI.WorkManagement.Services.ServiceModel;

namespace MicUI.WorkManagement.Helper.Extensions
{
    //Reference :- https://www.codeproject.com/Articles/5247609/ASP-NET-CORE-Token-Authentication-and-Authorizat-2
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class BPAAuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext filterContext)
        {
            HttpContext ctx = filterContext.HttpContext;

            // If the browser session has expired...
            if (ctx.Session.GetObjectFromJson<BEUserInfo>(SessionVariables.SessionKeyUserInfo) == null)
            {
                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    // For AJAX requests, we're overriding the returned JSON result with a simple string,
                    // indicating to the calling JavaScript code that a redirect should be performed.
                    filterContext.Result = new JsonResult(new { Data = "_SessionExpire_" });
                }
                else
                {
                    filterContext.Result = new RedirectToRouteResult(
                        new RouteValueDictionary(new
                        {
                            action = "Logout",
                            controller = "Home",
                            area = ""

                        }));
                }
            }
        }
    }
}
