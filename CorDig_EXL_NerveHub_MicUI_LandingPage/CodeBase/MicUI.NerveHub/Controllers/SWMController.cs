using Microsoft.AspNetCore.Mvc;
using MicUI.NerveHub.Models;
using Newtonsoft.Json;
using static MicUI.NerveHub.Models.TokenModel;
using System.Text;
using MicUI.NerveHub.Helper;
using Telerik.SvgIcons;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Xml.Linq;
using System.ComponentModel.DataAnnotations;
using MicUI.NerveHub.Models.Security;
using Microsoft.AspNetCore.Components.Routing;
using MicUI.NerveHub.Module.Security;

namespace MicUI.NerveHub.Controllers
{
    public class SWMController : Controller
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IConfiguration _configuration;
        private readonly IAuthentication _authentication;



        public SWMController(IHttpContextAccessor httpContextAccessor, IConfiguration configuration, IAuthentication authentication)
        {

            _contextAccessor = httpContextAccessor;
            _configuration = configuration;
            _authentication = authentication;

        }
        public async Task<IActionResult> Index()
        {
            string user = _authentication.UserId;

            IList<ModuleData> lactiveModule = new List<ModuleData>();
            HomeViewModel oHome = new HomeViewModel();
            _authentication.Token = HttpContext.Session.GetString("token").ToString();
            _authentication.RoleId = Convert.ToInt32(HttpContext.Session.GetString("RoleId"));
            _authentication.UserName = User.Identity.Name.Substring(User.Identity.Name.IndexOf('\\') + 1);
            var data = _authentication.GetLandingPagedata();
            lactiveModule = _authentication.GetModuleData();
            IList<Module_Data> lactive_Module = new List<Module_Data>();
            foreach (var itm in data)
            {
                Module_Data odata = new Module_Data();
                odata.ModuleName = itm.modulename;
                odata.DisplayOrder = 1;
                odata.ModuleAction = "";
                odata.Module_Action = Convert.ToBoolean(itm.readaction);
                odata.ModuleText = itm.text.ToString();
                lactive_Module.Add(odata);
            }
            oHome.lactiveModule = lactiveModule;
            oHome.lactiveModuleAction = lactive_Module;
            oHome.TenantName = HttpContext.Session.GetString("tenantName").ToString();
            return View(oHome);
        }


        
       
        public PartialViewResult NavMenu()
        {

            NavMenuViewModel model = new NavMenuViewModel();

            try
            {   
                        model.lactiveModule = _authentication.GetModuleData();
                        model.Language = HttpContext.Session.GetString("culture")  == null ? "en-US" : HttpContext.Session.GetString("culture");
                      // model.TimeZone = String.IsNullOrEmpty(_oUser.sUserTimeZone) == true ? "NA" : _oUser.sUserTimeZone;
                

            }
            catch (Exception ex)
            {
                TempData["PageError"] = ex.Message;
            }
            return PartialView("_NavMenu", model);
        }
       
    }
}
