using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MicUI.WorkManagement.Helper;
using MicUI.WorkManagement.Helper.Extensions;
using MicUI.WorkManagement.Helper.Sessions;
using MicUI.WorkManagement.Services.ServiceModel;
using System.Globalization;
using System.Reflection;

namespace MicUI.WorkManagement.Controllers
{
    [BPAActionFilter]
    [BPAAuthorize]
    public class BaseController : Controller, IExceptionFilter
    {
        private BEUserInfo _oUser;
        private int _iFormID;
        private BETenantInfo _oTenant;

        #region Public Field
        /// <summary>
        /// Gets or sets the userInfo Data.
        /// </summary>
        /// <value>The userInfo.</value>
        public BEUserInfo oUser
        {
            set { _oUser = value; }
            get { return _oUser; }
        }

        public BETenantInfo oTenant
        {
            set { _oTenant = value; }
            get { return _oTenant; }
        }

        public int iFormID
        {
            set { _iFormID = value; }
            get { return _iFormID; }
        }
        #endregion

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            var session = context.HttpContext.Session;
            if ((session.GetObjectFromJson<BEUserInfo>(SessionVariables.SessionKeyUserInfo) == null || session.GetObjectFromJson<BETenantInfo>(SessionVariables.SessionKeyTenantInfo) == null))
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
            else if (session.GetObjectFromJson<BEUserInfo>(SessionVariables.SessionKeyUserInfo) != null && session.GetObjectFromJson<BETenantInfo>(SessionVariables.SessionKeyTenantInfo) != null)
            {
                _oUser = session.GetObjectFromJson<BEUserInfo>(SessionVariables.SessionKeyUserInfo) ?? new BEUserInfo();
                _oTenant = session.GetObjectFromJson<BETenantInfo>(SessionVariables.SessionKeyTenantInfo) ?? new BETenantInfo();
                _oUser.sLanguage = _oUser.sLanguage ?? GlobalConstant.DefaultLangCulture;
                Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture =
                    new CultureInfo(_oUser.sLanguage.ToString());

                var CultureDateFormat = Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern + " " + Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortTimePattern;
                ViewBag.StartupScript = "var CultureDateFormat=\"" + CultureDateFormat.ToString() + "\";" +
                                        "kendo.culture(\"" + _oUser.sLanguage.ToString() + "\");";
                TempData["Currentculture"] = _oUser.sLanguage.ToString();
                TempData.Keep("Currentculture");

                /*
                if (!context.HttpContext.Request.IsAjaxRequest())
                {
                    if (context.RequestContext.RouteData.Values["tenantName"] != null)
                    {
                        string tenentName = context.RequestContext.RouteData.Values["tenantName"].ToString();
                        if (_oTenant.ClientName.ToUpper() != tenentName.ToUpper())
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

        public void OnException(ExceptionContext filterContext)
        {
            var session = filterContext.HttpContext.Session;
            Exception exceptionToThrow = null;

            if (filterContext.Exception != null)
            {
                //Here Inject Logger Service
                //var _loggerService = filterContext.HttpContext.RequestServices.GetService<ILoggerService>();
                /*if (ExceptionPolicy.HandleException(filterContext.Exception, "UserInterfacePolicy", out exceptionToThrow))
                {
                    if (exceptionToThrow == null)
                        exceptionToThrow = filterContext.Exception;

                }*/
                //  Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{filterContext.Exception?.Message} {filterContext.Exception?.InnerException?.Message} {filterContext.Exception?.StackTrace}", requestId = CorrelationId });
                exceptionToThrow = filterContext.Exception;
            }
            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                if (session.GetObjectFromJson<BEUserInfo>(SessionVariables.SessionKeyUserInfo) == null || session.GetObjectFromJson<BEUserInfo>(SessionVariables.SessionKeyTenantInfo) == null)
                {
                    filterContext.HttpContext.Response.StatusCode = 500;
                    if (filterContext.Exception != null && filterContext.Exception.Message.ToString().Contains("\r\n"))
                    {
                        filterContext.HttpContext.Response.WriteAsync(exceptionToThrow != null ? exceptionToThrow.Message.ToString().Replace("\r\n", "") : "Exception");
                    }
                    else
                    {
                        filterContext.HttpContext.Response.WriteAsync("Session_Expire");
                    }
                    filterContext.ExceptionHandled = true;
                    ApplicationException Appex = new ApplicationException("Session_Expire");
                    filterContext.Exception = Appex;
                    filterContext.Result = new JsonResult(new
                    {
                        Data = new { message = Appex.Message.ToString(), success = false, error = "" }
                    });
                }
                else
                {
                    filterContext.HttpContext.Response.StatusCode = 500;
                    if (filterContext.Exception != null && filterContext.Exception.Message.ToString().Contains("\r\n"))
                    {
                        filterContext.HttpContext.Response.WriteAsync(exceptionToThrow != null ? exceptionToThrow.Message.ToString().Replace("\r\n", "") : "Exception");
                    }
                    else
                    {
                        filterContext.HttpContext.Response.WriteAsync(exceptionToThrow != null ? exceptionToThrow.Message.ToString() : "Exception");
                    }
                    filterContext.ExceptionHandled = true;
                    filterContext.Result = new JsonResult(new
                    {
                        Data = new { message = (exceptionToThrow != null ? exceptionToThrow.Message.ToString() : "Exception"), success = false, error = "" },
                    });
                }
            }
        }

        [NonAction]
        internal bool CheckViewPermission(int iFormID)
        {
            bool hasPermission = false;
            var _getSessionValues = HttpContext.RequestServices.GetService<IGetSetSessionValues>();

            var oMenu = _getSessionValues.GetMenusFormInfo();

            int MenuItems = oMenu.MenuViewModel.Where(p => p.FormID == iFormID).Count();

            if (MenuItems > 0)
            {
                hasPermission = true;
            }
            return hasPermission;
        }
    }
}
