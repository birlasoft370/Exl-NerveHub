using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MicUI.EmailManagement.Helper.Sessions;
using MicUI.EmailManagement.Models;
using MicUI.EmailManagement.Module.Authentication;
using MicUI.EmailManagement.Services.ModuleMenus;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Globalization;
using System.Text;

namespace MicUI.EmailManagement.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GenerateToken()
        {
            HttpContext.Response.Headers["Expires"] = "-1";
            HttpContext.Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
            HttpContext.Response.Headers["Pragma"] = "no-cache";
            HttpContext.Response.Headers["x-frame-options"] = "DENY";

            Rootobject tokenResponse = new();
            var token = HttpContext.Session != null ? HttpContext.Session.GetString("Token") : "";
            try
            {
                if (string.IsNullOrWhiteSpace(token))
                {
                    var UserName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
                    var LoggedinUser = UserName[(UserName.IndexOf('\\') + 1)..];
                    var userDetail = new
                    {
                        loginName = LoggedinUser,
                        tenantName = "default",
                        hostName = System.Net.Dns.GetHostName(),
                        ipAddress = "::1",
                        SystemSessionID = "string"
                    };
                    string apiUrl = "https://bsservice.exlservice.com/microservsso/SSOAuthorization";
                    StringContent parameter = new(JsonConvert.SerializeObject(userDetail), Encoding.UTF8, "application/json");

                    using (var httpClient = new HttpClient())
                    {

                        using var response = await httpClient.PostAsync(apiUrl, parameter);
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        tokenResponse = JsonConvert.DeserializeObject<Rootobject>(apiResponse);
                    }
                    token = tokenResponse.JwtToken;
                    HttpContext.Session.SetString("Token", tokenResponse.JwtToken);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return RedirectToAction(nameof(Index), new { token });
        }

        [AllowAnonymous]
        public IActionResult Index(string token)
        {
            if (!string.IsNullOrEmpty(token))
            {
                List<Task> tasks = new();
                var UserName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
                var LoggedinUser = UserName.Substring(UserName.IndexOf('\\') + 1);
                HttpContext.Session.SetString(SessionVariables.SessionKeyLoginName, LoggedinUser);
                HttpContext.Session.SetString(SessionVariables.SessionKeyUserToken, token);
                HttpContext.Session.SetString("Token", token);

                tasks.Add(Task.Run(() => SetUserInfoSession()));
                tasks.Add(Task.Run(() => SetTenantInfoSession()));

                Task.WaitAll(tasks.ToArray());
                return RedirectToAction(nameof(Index), nameof(EmailManagement));
            }
            else
            {
                return RedirectToAction(nameof(Logout));
            }
        }

        private void SetUserInfoSession()
        {
            var _moduleMenusService = HttpContext.RequestServices.GetService<ISecurityApiService>();
            var UserInfo = _moduleMenusService?.IsLADPUserAsync().Result;// To Retrieve the UserInfo for Lang Change
            HttpContext.Session.SetObjectAsJson(SessionVariables.SessionKeyUserInfo, UserInfo.data.FirstOrDefault());// "en-US"
        }

        private void SetTenantInfoSession()
        {
            var Token = HttpContext.Session.GetString("Token") ?? "";
            var _AuthService = HttpContext.RequestServices.GetService<IAuthenticationTokenService>();
            var tenantInfo = _AuthService?.GetTanentInfo(Token) ?? new Services.ServiceModel.BETenantInfo();
            HttpContext.Session.SetObjectAsJson(SessionVariables.SessionKeyTenantInfo, tenantInfo);// "en-US"
        }

        [AllowAnonymous]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            if (HttpContext.Session.GetString(SessionVariables.SessionKeyUserInfo) != null)
            {
                Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture =
                      new CultureInfo(TempData["Currentculture"].ToString());
                ViewBag.StartupScript = "kendo.culture(\"" + TempData["Currentculture"].ToString() + "\");";
            }
            return View();
        }

        public JsonResult GetSessionTimeout()
        {
            //  var sessionSection = (SessionStateSection)WebConfigurationManager.GetSection("system.web/sessionState");
            var timeoutvalue = 60;
            return Json(timeoutvalue);
        }      

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }

    public class Rootobject
    {
        public string UserName { get; set; }
        public string JwtToken { get; set; }
        public int ExpiresIn { get; set; }
    }
}