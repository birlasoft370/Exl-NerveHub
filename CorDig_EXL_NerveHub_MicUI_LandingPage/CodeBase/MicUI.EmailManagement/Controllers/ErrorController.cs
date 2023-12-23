using Microsoft.AspNetCore.Mvc;

namespace MicUI.EmailManagement.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult PageNotFound()
        {
            Response.StatusCode = 404;

            Response.ContentType = "text/html";

            return View();
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
        public IActionResult InternalServerError()
        {
            return View();
        }
    }
}
