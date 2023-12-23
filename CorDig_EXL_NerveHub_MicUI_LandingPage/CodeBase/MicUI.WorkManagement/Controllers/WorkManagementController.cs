using Microsoft.AspNetCore.Mvc;

namespace MicUI.WorkManagement.Controllers
{
    public class WorkManagementController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
