using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MicUI.Configuration.Helper;
using MicUI.Configuration.Helper.Extensions;
using MicUI.Configuration.Helper.Sessions;
using MicUI.Configuration.Models.Security;
using MicUI.Configuration.Services.ServiceModel;

namespace MicUI.Configuration.Controllers
{

    [BPAActionFilter]
    [BPAAuthorize]
    public class BaseController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            var session = context.HttpContext.Session;

            if (string.IsNullOrWhiteSpace(session.GetString(SessionVariables.SessionKeyUserToken)) || session.GetObjectFromJson<BEUserInfo>(SessionVariables.SessionKeyUserInfo) == null)
            {
                if (context.HttpContext.Request.IsAjaxRequest())
                {
                    // For AJAX requests, return result as a simple string, 
                    // and inform calling JavaScript code that a user should be redirected.
                    JsonResult result = Json(data: "Session_Expire");
                    context.Result = result;

                }
                else
                {
                    // For round-trip requests,
                    context.Result = new RedirectToRouteResult(
                       new RouteValueDictionary(new
                       {
                           action = "Logout",
                           controller = "Home",
                           area = ""
                       }));
                }
            }
            else if (session.GetObjectFromJson<BEUserInfo>(SessionVariables.SessionKeyUserInfo) != null)
            {
                var userInfo = session.GetObjectFromJson<BEUserInfo>(SessionVariables.SessionKeyUserInfo);
                userInfo.sLanguage = userInfo.sLanguage ?? GlobalConstant.DefaultLangCulture;

                Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture =
                new System.Globalization.CultureInfo(userInfo.sLanguage.ToString());


                session.SetString(SessionVariables.SessionKeyCultureDateFormat, Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern + " " + Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortTimePattern);
                ViewBag.StartupScript =
                    "var CultureDateFormat=\"" + session.GetString(SessionVariables.SessionKeyCultureDateFormat.ToString()) + "\";" +
                                       "kendo.culture(\"" + userInfo.sLanguage.ToString() + "\");";
                TempData["Currentculture"] = userInfo.sLanguage.ToString();
                TempData.Keep("Currentculture");

                /* need client name from backend to validate
                if (!context.HttpContext.Request.IsAjaxRequest())
                {
                    if (context.HttpContext.Request.Path.Value != null)
                    {
                        string tenentName = context.HttpContext.Request.RouteValues["tenantName"].ToString();
                        
                        if (userInfo.cl.ClientName.ToUpper() != tenentName.ToUpper())
                        {
                            context.Result = new RedirectToRouteResult(
                                new RouteValueDictionary(new
                                {
                                    action = "Logout",
                                    controller = "Home",
                                    area = ""
                                }));
                        }
                    }
                }*/
            }
        }

        [NonAction]
        internal bool CheckViewPermission(int iFormID)
        {
            bool haspermission = false;
            var _getSessionValues = HttpContext.RequestServices.GetService<IGetSetSessionValues>();

            var oMenu = _getSessionValues.GetMenusFormInfo();

            int MenuItems = oMenu.MenuViewModel.Where(p => p.FormID == iFormID).Count();

            if (MenuItems > 0)
            {
                haspermission = true;
            }

            return haspermission;
        }

        internal BETenantInfo oTenant
        {
            get
            {
                var _getSessionValues = HttpContext.RequestServices.GetService<IGetSetSessionValues>();
                var tenantInfo = _getSessionValues.GetTenantInfo();
                return tenantInfo;
            }
        }
        internal BEUserInfo oUser
        {
            get
            {
                var _getSessionValues = HttpContext.RequestServices.GetService<IGetSetSessionValues>();
                var userInfo = _getSessionValues.GetSessionUserInfo();
                return userInfo;
            }
        }
    }
}
