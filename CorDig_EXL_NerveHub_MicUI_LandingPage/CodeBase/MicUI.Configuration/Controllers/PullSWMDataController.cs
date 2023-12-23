using Microsoft.AspNetCore.Mvc;

namespace MicUI.Configuration.Controllers
{
    public class PullSWMDataController : BaseController
    {
        public IActionResult Index()
        {
            return RedirectToAction(nameof(Index), nameof(Configuration));
        }
    }
}
