using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using MicUI.NerveHub.Helper;
using MicUI.NerveHub.Models;
using MicUI.NerveHub.Module.Security;
using Newtonsoft.Json;
using System.Data;
using System.DirectoryServices;
using System.Text;

namespace MicUI.NerveHub.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IConfiguration _configuration;
        private readonly IAuthentication _authentication;

        public HomeController(ILogger<HomeController> logger, IHttpContextAccessor httpContextAccessor, IConfiguration configuration, IAuthentication authentication)
        {

            _contextAccessor = httpContextAccessor;
            _logger = logger;
            _configuration = configuration;
            _authentication = authentication;
        }

        internal string UserIdentityName
        {
            get
            {
                return User.Identity.Name.Substring(User.Identity.Name.IndexOf('\\') + 1);
            }
        }


        public async Task<IActionResult> Index(string tanentName)
        {
            HomeViewModel oHome = new HomeViewModel();

            var tanent = tanentName == "" || tanentName == null ? "Default" : tanentName;
            ViewBag.DQAUrl = _configuration["DQAUrl"];
            string userId = User.Identity.Name.Substring(User.Identity.Name.IndexOf('\\') + 1);
            _authentication.UserId = userId;
            HttpContext.Session.SetString("tenantName", tanent);
            var userDetail = new
            {
                loginName = userId,
                tenantName = tanent,
                hostName = System.Net.Dns.GetHostName(),
                ipAddress = GetIpAddress(),
                SystemSessionID = "string"
            };
            StringContent parameter = new StringContent(JsonConvert.SerializeObject(userDetail), Encoding.UTF8, "application/json");

            try
            {
                string token = await _authentication.GetToken(parameter);//Tenant
                await GetUserSession(userId, token);
                HttpContext.Session.SetString("token", token);
                ViewBag.token = token;


                //oHome.ClientName = oTenant.ClientName.Replace(" ", "_") + "_" + oTenant.ClientID;
                oHome.TenantName = tanent;
                if (!string.IsNullOrWhiteSpace(HttpContext.Session.GetString("RoleId")))
                {
                    oHome.lactiveModule = _authentication.GetModuleData();
                    oHome.lactiveModuleAction = GetLandingPageAction();
                }

                return View(oHome);
            }
            catch (Exception)
            {
                return RedirectToAction("Home", "Home");
            }
        }

        [NonAction]
        public async Task GetUserSession(string UserName, string tokenl)
        {
            BESession oBESession = new BESession();
            try
            {

                oBESession.sHostName = System.Net.Dns.GetHostName();
                oBESession.sIPAddress = GetIpAddress();
                var UserInfo = await _authentication.IsLDapUser(UserName, tokenl);

                if (UserInfo != null)
                {
                    if (UserInfo.roleId > 0)
                    {
                        HttpContext.Session.SetString("culture", UserInfo.language == null ? "en-US" : UserInfo.language);
                        HttpContext.Session.SetString("RoleId", UserInfo.roleId.ToString());
                        ChangeCulture(HttpContext.Session.GetString("culture"));
                    }
                    else
                    {
                        TempData["PageError"] = "</BR>AD security group mapping is not correct</BR>";
                        if (UserInfo.roleId > 0)
                        {
                            TempData["PageError"] += "</BR> Your current Role in Application is " + UserInfo.roleName + "</BR>&nbsp;";
                        }
                        else
                        {
                            TempData["PageError"] = "No Role Assign. </BR> Kindly ask your supervisor to assign role for you.";
                        }
                    }
                }
                else
                {
                    TempData["PageError"] = UserName + "<br/> not a Authorized user";
                }

            }
            catch (ApplicationException ex)
            {
                TempData["PageError"] = ex.Message;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void ChangeCulture(string culture)
        {
            Response.Cookies.Append(CookieRequestCultureProvider.DefaultCookieName, CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)), new CookieOptions()
            { Expires = DateTimeOffset.UtcNow.AddYears(1) });
            //  return Redirect(Request.Headers["Referer"].ToString());

        }

        // Menu data

        public ActionResult Home()
        {
            return View();
        }

        private string GetIpAddress()
        {
            var remoteIp = HttpContext.Connection.RemoteIpAddress;
            string clientIp = "";
            if (remoteIp != null)
            {
                clientIp = remoteIp.ToString();
            }
            return clientIp;
        }

        private IList<Module_Data> GetLandingPageAction()
        {
            var dsLanding = _authentication.GetLandingPagedata();
            IList<Module_Data> lactive_Module = new List<Module_Data>();
            foreach (var itm in dsLanding)
            {
                Module_Data odata = new Module_Data();
                odata.ModuleName = itm.modulename;
                odata.DisplayOrder = 1;
                odata.ModuleAction = "";
                odata.Module_Action = Convert.ToBoolean(itm.readaction);
                odata.ModuleText = itm.text.ToString();
                lactive_Module.Add(odata);
            }

            return lactive_Module;
        }

        #region Password Expiry

        public JsonResult CheckPasExpire()
        {
            string ErrorOut = "", Textdisplay = "";
            try
            {
                string Domain = string.Empty;// ConfigurationManager.AppSettings["LDAP"];
                //  IAuthenticate oLDAP = new  LDAPUser(Domain);
                var sLoginName = User.Identity.Name.Substring(User.Identity.Name.IndexOf('\\') + 1);

                int DaysLeft = 0;
                Textdisplay = CheckPasswordExpire(sLoginName, out ErrorOut, out DaysLeft);
                //if (DaysLeft > 7)
                //{
                //    Textdisplay = "";
                //}
            }
            catch (Exception ex)
            {
                TempData["PageError"] = "InnerException :- " + ex.InnerException + " ,Message :- " + ex.Message;
            }
            return Json(Textdisplay);

        }

        public string CheckPasswordExpire(string strLoginName, out string output, out int DaysLeft)
        {
            string returnValue = "";
            output = "";

            System.DirectoryServices.DirectoryEntry entry = new();
            try
            {
                entry.AuthenticationType = AuthenticationTypes.Secure;
                using (DirectorySearcher search = new DirectorySearcher(entry))
                {
                    search.Filter = "(SAMAccountName=" + ValidationRegex.whitelist(strLoginName) + ")";
                    search.SearchScope = SearchScope.Subtree;
                    search.CacheResults = false;

                    using (SearchResultCollection results = search.FindAll())
                    {
                        if (results.Count > 0)
                        {
                            foreach (SearchResult result in results)
                            {
                                try
                                {
                                    entry = result.GetDirectoryEntry();
                                }
                                catch
                                {
                                    output += "<BR><FONT COLOR='RED'><b>Login ID not found !</b></FONT>";
                                }
                            }
                        }
                        TimeSpan tSpan = GetTimeLeft(entry);
                        DaysLeft = tSpan.Days;
                        if (tSpan == TimeSpan.MaxValue)
                            returnValue += "Your password will never expire !";
                        else if (tSpan.Days == 1)
                            returnValue += "Your password will expire after " + tSpan.Days.ToString() + " day ! ";
                        else
                            returnValue += "Your password will expire after " + tSpan.Days.ToString() + " days ! ";
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                entry = null;
            }
            return returnValue;
        }
        public TimeSpan GetTimeLeft(System.DirectoryServices.DirectoryEntry user)
        {
            DateTime willExpire = GetExpiration(user);
            if (willExpire == DateTime.MaxValue)
                return TimeSpan.MaxValue;

            if (willExpire == DateTime.MinValue || willExpire.AddDays(30) < DateTime.Now)
                return TimeSpan.MinValue;

            return willExpire.AddDays(30).Subtract(DateTime.Now);
        }

        public DateTime GetExpiration(System.DirectoryServices.DirectoryEntry user)
        {
            const int UF_DONT_EXPIRE_P = 0x10000;

            int flags = (int)user.Properties["userAccountControl"][0];

            //check to see if password is set to expire
            if (Convert.ToBoolean(flags & UF_DONT_EXPIRE_P))
            {
                //the user’s password will never expire
                return DateTime.MaxValue;
            }

            long ticks = GetInt64(user, "pwdLastSet");

            //user must change password at next login
            if (ticks == 0)
                return DateTime.MinValue;

            //password has never been set
            if (ticks == -1)
            {
                throw new InvalidOperationException(
                  "User does not have a password"
                  );
            }

            //get when the user last set their password;
            DateTime pwdLastSet = DateTime.FromFileTime(
              ticks
              );

            //use our policy class to determine when
            //it will expire
            return pwdLastSet;
        }
        private Int64 GetInt64(DirectoryEntry entry, string attr)
        {
            //we will use the marshaling behavior of
            //the searcher
            using (DirectorySearcher ds = new(entry, String.Format("({0}=*)", attr), new string[] { attr }, SearchScope.Base))
            {
                SearchResult sr = ds.FindOne();
                if (sr != null)
                {
                    if (sr.Properties.Contains(attr))
                    {
                        return (Int64)sr.Properties[attr][0];
                    }
                }

            }
            return -1;
        }

        #endregion

        #region ChangePassword

        public JsonResult FindUserLoginName()
        {
            return Json(UserIdentityName);
        }

        public ActionResult ChangePassword()
        {
            ChangePaswViewModel obj = new ChangePaswViewModel();

            return View("ChangePassword", obj);
        }
        [HttpPost]
        public ActionResult ChangePassword(ChangePaswViewModel objViewModel)
        {

            ChangePaswViewModel obj = new ChangePaswViewModel();
            var sLoginName = UserIdentityName;

            string txtOldPasw = EncryptDecrypt.Encrypt(objViewModel.sOldPasw.Trim());
            string txtNewPasw = EncryptDecrypt.Encrypt(objViewModel.sNewPasw);
            string txtReNewPasw = EncryptDecrypt.Encrypt(objViewModel.sRenewPasw);

            var isSuccess = Change_Password(sLoginName, txtOldPasw, txtNewPasw);
            var result = isSuccess == true ? "Sucessfully" : "Error";
            if (result == "Sucessfully")
            {
                obj.sResultMsg = "Password Changed Sucessfully.";
                ViewData["result"] = "Password Changed Sucessfully.";
            }
            else
            {
                obj = objViewModel;
                obj.sResultMsg = "Error in password change.Please contact adminstrator.";
                ViewData["result"] = "Error in password change.Please contact adminstrator.";
            }
            return View(obj);
        }

        public bool Change_Password(string sLoginName, string sCurrentPassword, string sNewPass)
        {
            bool returnvalue = false;
            DirectoryEntry entry = new DirectoryEntry();
            try
            {
                entry.AuthenticationType = AuthenticationTypes.Secure;
                using (DirectorySearcher search = new DirectorySearcher(entry))
                {
                    search.Filter = "(SAMAccountName=" + ValidationRegex.whitelist(sLoginName) + ")";
                    search.SearchScope = SearchScope.Subtree;
                    search.CacheResults = false;

                    using (SearchResultCollection results = search.FindAll())
                    {
                        if (results.Count > 0)
                        {
                            foreach (SearchResult result in results)
                            {
                                try
                                {
                                    entry = result.GetDirectoryEntry();
                                }
                                catch
                                {
                                    throw new Exception(@"Login ID not found");
                                }
                            }

                            try
                            {
                                entry.Invoke("ChangePassword", new object[] { EncryptDecrypt.Decrypt(sCurrentPassword), EncryptDecrypt.Decrypt(sNewPass) });
                                entry.CommitChanges();
                                returnvalue = true;
                            }
                            catch (Exception error)
                            {
                                throw new Exception("Invalid current password or check Password Policy");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                returnvalue = false;
            }
            finally
            {
                if (entry != null)
                {
                    entry = null;
                }
            }

            return returnvalue;
        }

        #endregion
    }
}