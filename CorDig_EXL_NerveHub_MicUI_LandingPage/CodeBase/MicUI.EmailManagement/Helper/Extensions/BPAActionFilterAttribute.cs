using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MicUI.EmailManagement.Controllers;
using MicUI.EmailManagement.Helper.Sessions;
using MicUI.EmailManagement.Services.ServiceModel;

namespace MicUI.EmailManagement.Helper.Extensions
{
    public class BPAActionFilterAttribute : ActionFilterAttribute, IExceptionFilter
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //if (!filterContext.IsChildAction)
            //{

            // Reference :- https://stackoverflow.com/questions/41700759/httpcontext-response-cache-equivalent-in-net-core
            filterContext.HttpContext.Response.Headers["Expires"] = "-1";
            filterContext.HttpContext.Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
            filterContext.HttpContext.Response.Headers["Pragma"] = "no-cache";
            //   filterContext.HttpContext.Response.Headers["x-frame-options"] = "DENY";

            //}

            base.OnActionExecuting(filterContext);
        }

        public void OnException(ExceptionContext filterContext)
        {
            var session = filterContext.HttpContext.Session;
            Exception exceptionToThrow = null;

            #region Developer Log

            if (filterContext.Exception != null)
            {

                if (exceptionToThrow == null)
                    exceptionToThrow = filterContext.Exception;
                var _logger = filterContext.HttpContext.RequestServices.GetService<ILogger<BaseController>>();
                _logger.Error(new LoggerInfo()
                {
                    ActionName = filterContext.RouteData.Values["action"].ToString(),
                    Message = $"{exceptionToThrow?.Message} {exceptionToThrow?.InnerException?.Message} {exceptionToThrow?.StackTrace}"
                });

                if (exceptionToThrow.Message.Contains(GlobalConstant.InternalServerError))
                {
                    filterContext.HttpContext.Response.StatusCode = 500;
                    session.SetString(SessionVariables.SessionKeyUserToken, string.Empty);
                    filterContext.ExceptionHandled = true;
                }
            }

            #endregion

            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                if (session.GetObjectFromJson<BEUserInfo>(SessionVariables.SessionKeyUserInfo) == null)
                {
                    filterContext.HttpContext.Response.StatusCode = 500;
                    if (filterContext.Exception.Message.ToString().Contains("\r\n"))
                    {
                        filterContext.HttpContext.Response.WriteAsync(exceptionToThrow.Message.ToString().Replace("\r\n", ""));
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
                    if (filterContext.Exception.Message.ToString().Contains("\r\n"))
                    {
                        filterContext.HttpContext.Response.WriteAsync(exceptionToThrow.Message.ToString().Replace("\r\n", ""));
                    }
                    else
                    {
                        filterContext.HttpContext.Response.WriteAsync(exceptionToThrow.Message.ToString());
                    }
                    filterContext.ExceptionHandled = true;
                    filterContext.Result = new JsonResult(new
                    {
                        Data = new { message = exceptionToThrow.Message.ToString(), success = false, error = "" }
                    });
                }
            }
        }
    }
}
