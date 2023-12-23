using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MicUI.EmailManagement.Helper.Sessions;
using MicUI.EmailManagement.Services.ServiceModel;

namespace MicUI.EmailManagement.Helper.Extensions
{
    //Reference :- https://www.codeproject.com/Articles/5247609/ASP-NET-CORE-Token-Authentication-and-Authorizat-2
    public class BPAAuthorizeAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext filterContext)
        {
            HttpContext ctx = filterContext.HttpContext;

            // If the browser session has expired...
            if (ctx.Session.GetObjectFromJson<BEUserInfo>(SessionVariables.SessionKeyUserInfo) == null)
            {
                if (ctx.Request.IsAjaxRequest())
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
            //else if (filterContext.HttpContext.User.Identity.IsAuthenticated)
            //{
            //    // Otherwise the reason we got here was because the user didn't have access rights to the
            //    // operation, and a 403 should be returned.
            //    filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
            //}
            else
            {
                //base.HandleUnauthorizedRequest(filterContext);
                //filterContext.Result = new RedirectResult("~/Dashboard/NoPermission");
            }
        }

    }
}
