using Microsoft.AspNetCore.Mvc;

namespace MicUI.EmailManagement.Controllers
{
    public class EmailManagementController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
