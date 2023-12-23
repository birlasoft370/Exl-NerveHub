using Microsoft.AspNetCore.Mvc;

namespace MicUI.Configuration.Controllers
{
    public class ErrorController : BaseController
    {
        [HttpGet("Error/Index")]
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet("Error/PageNotFound")]
        public ActionResult PageNotFound()
        {
            Response.StatusCode = 404;

            Response.ContentType = "text/html";
            //Response.TrySkipIisCustomErrors = true;

            return View("NotFound");
        }

        public ActionResult AccessDenied()
        {
            return View();
        }
    }
}
