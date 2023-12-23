using MicUI.NerveHub.Helper;
using MicUI.NerveHub.Models;
using MicUI.NerveHub.Models.Security;
using Newtonsoft.Json;
using static MicUI.NerveHub.Models.TokenModel;

namespace MicUI.NerveHub.Module.Security
{
    public class Authentication : IAuthentication
    {
        private readonly IConfiguration _configuration;
        public Authentication()
        { }
        public Authentication(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        private int _roleId;
        public int RoleId
        {
            get { return _roleId; }
            set { _roleId = value; }
        }
        private string _userName;
        public string UserName
        {
            get { return _userName; }
            set { _userName = value; }
        }
        private string _token;
        public string Token
        {
            get { return _token; }
            set { _token = value; }
        }
        private string _userId;
        public string UserId
        {
            get { return _userId; }
            set { _userId = value; }
        }

        private string _area;
        public string Area
        {
            get { return _area; }
            set { _area = value; }
        }
        private string _language;
        public string Language
        {
            get { return _language; }
            set { _language = value; }
        }
        private string _timeZone;
        public string TimeZone
        {
            get { return _timeZone; }
            set { _timeZone = value; }
        }




        public async Task<string> GetToken(StringContent userDetails)
        {
            string token = "";

            AuthenticationResponse userData = new();
            using (var httpclinet = new HttpClient())
            {
                using (var response = await httpclinet.PostAsync($"{_configuration["SSOAuthS"]}", userDetails))
                {
                    token = await response.Content.ReadAsStringAsync();
                    userData = JsonConvert.DeserializeObject<AuthenticationResponse>(token);

                }

            }
            _token = userData.JwtToken;
            return userData.JwtToken;


        }
        public async Task<UserInfo> IsLDapUser(string loginUser, string token)
        {

            UserInfo userInfodata = new UserInfo();
            var UserInfo = CallAPI.HttpClientAsync<MessageResponse<List<BEUserInfo>>>(HttpMethod.Get, $"{_configuration["SecurityService"]}UserManagement/IsLADPUser", string.Empty, token: token).Result;
            userInfodata.empid = UserInfo.data.FirstOrDefault().iEmployeeID;
            userInfodata.firstname = UserInfo.data.FirstOrDefault().sFirstName;
            userInfodata.lastname = UserInfo.data.FirstOrDefault().sLastName;
            userInfodata.loginname = UserInfo.data.FirstOrDefault().sLoginName;
            userInfodata.userid = UserInfo.data.FirstOrDefault().iUserID;
            userInfodata.language = UserInfo.data.FirstOrDefault().sLanguage;
            userInfodata.userlevelid = UserInfo.data.FirstOrDefault().iUserLevel.ToString();
            userInfodata.roleName = UserInfo.data.FirstOrDefault().oRoles.FirstOrDefault().sRoleName;
            userInfodata.roleId = UserInfo.data.FirstOrDefault().oRoles.FirstOrDefault().iRoleID;
            userInfodata.timezonename = UserInfo.data.FirstOrDefault().sUserTimeZone.ToString();
            string user = UserId;
            _roleId = userInfodata.roleId;
            return userInfodata;
        }
        public List<BELandingPageMenu> GetLandingPagedata()
        {
            var landingPagedata = CallAPI.HttpClientAsync<MessageResponse<List<BELandingPageMenu>>>(HttpMethod.Get, _configuration["SecurityService"] + "Menus/GetLandingData", token: Token).Result;
            return landingPagedata.data;
        }

        public List<ModuleData> GetModuleData()
        {
            int roleId = Convert.ToInt32(_roleId);
            string tokenl = _token.ToString();
            List<ModuleData> lactiveModule = new List<ModuleData>();
            var menulist = CallAPI.HttpClientNormalAsync<MessageResponse<List<BEMenuItems>>>(HttpMethod.Get, $"{_configuration["SecurityService"]}Menus/GetRoleWiseMenu?RoleId={roleId}&isMossApplicationMenu=true", string.Empty, token: tokenl).Result;
            var menu = from n in menulist.data
                       where n.bHasPermission == true
                       group n.sModuleName by n.sModuleName into nGroup
                       select nGroup.Key;
            foreach (string str in menu)
            {
                ModuleData odata = new ModuleData();
                odata.ModuleName = str;
                odata.DisplayOrder = Displayorder(str);
                lactiveModule.Add(odata);
            }
            lactiveModule = lactiveModule.OrderBy(p => p.DisplayOrder).ToList<ModuleData>();
            return lactiveModule;
        }
        private List<MenuViewModel> GetMenuData(int roleID, string sArea)
        {
            string tokenl = _token;
            List<MenuViewModel> mmList = new List<MenuViewModel>();
            if (sArea == null) return mmList;
            try
            {
                if (sArea == "")
                { return mmList; }
                var menulist = CallAPI.HttpClientNormalAsync<MessageResponse<List<BEMenuItems>>>(HttpMethod.Get, $"{_configuration["SecurityService"]}Menus/GetRoleWiseMenu?RoleId={roleID}&isMossApplicationMenu=true", string.Empty, token: tokenl).Result;


                var MenuItems = menulist.data.Where(a => (a.sModuleName == sArea || a.URL == ""));


                foreach (var itmeValue in MenuItems)
                {
                    if (itmeValue.iFormID != null && itmeValue.NodeName != null && itmeValue.ParentID != null && itmeValue.MenuOrder != null)
                    {
                        if (itmeValue.iFormID == 0 && itmeValue.URL == "" && itmeValue.bHasPermission == false)
                        {
                            MenuViewModel objMenuModel = new MenuViewModel();
                            objMenuModel.Id = itmeValue.NodeID;
                            objMenuModel.Name = itmeValue.NodeName;//rm.GetString("text_" + item.Current.nodeid.ToString()) == null ? item.Current.nodename : rm.GetString("text_" + item.Current.NodeID.ToString());
                            objMenuModel.ParentId = itmeValue.ParentID;
                            objMenuModel.SortOrder = itmeValue.MenuOrder;
                            objMenuModel.Url = itmeValue.URL;
                            objMenuModel.Flag = itmeValue.Flag;// != "" ? AntiXssEncoder.HtmlEncode(EncryptDecrypt.Encrypt(item.Current.Flag), false) : "";
                            objMenuModel.FormID = itmeValue.iFormID;
                            objMenuModel.Areas = itmeValue.sModuleName;
                            objMenuModel.RouterController = itmeValue.sController;
                            objMenuModel.RouterAction = itmeValue.sAction;
                            objMenuModel.IconClass = itmeValue.sIconClass;
                            mmList.Add(objMenuModel);
                        }
                        else if (itmeValue.bHasPermission && itmeValue.URL != "")
                        {
                            MenuViewModel objMenuModel = new MenuViewModel();
                            objMenuModel.Id = itmeValue.NodeID;
                            objMenuModel.Name = itmeValue.NodeName;//rm.GetString("text_" + item.Current.nodeid.ToString()) == null ? item.Current.nodename : rm.GetString("text_" + item.Current.NodeID.ToString());
                            objMenuModel.ParentId = itmeValue.ParentID;
                            objMenuModel.SortOrder = itmeValue.MenuOrder;
                            objMenuModel.Url = itmeValue.URL;
                            objMenuModel.Flag = itmeValue.Flag;// != "" ? AntiXssEncoder.HtmlEncode(EncryptDecrypt.Encrypt(item.Current.Flag), false) : "";
                            objMenuModel.FormID = itmeValue.iFormID;
                            objMenuModel.Areas = itmeValue.sModuleName;
                            objMenuModel.RouterController = itmeValue.sController;
                            objMenuModel.RouterAction = itmeValue.sAction;
                            objMenuModel.IconClass = itmeValue.sIconClass;
                            mmList.Add(objMenuModel);
                        }
                    }
                }


            }
            catch (Exception ex)
            {

                throw;


            }
            finally
            { }
            return mmList;
        }
        public List<MenuViewModel> GenerateMenu(string sArea)
        {

            List<MenuViewModel> model = null;
            try
            {

                model = GetMenuData(_roleId, sArea);

            }
            catch (Exception ex)
            {
                // TempData["PageError"] = ex.Message;
            }
            return model;
        }
        public NavMenuViewModel NavMenu()
        {
            NavMenuViewModel model = new NavMenuViewModel();
            if (string.IsNullOrWhiteSpace(_token))
            {
                return model;
            }
            var UserInfo = CallAPI.HttpClientAsync<MessageResponse<List<BEUserInfo>>>(HttpMethod.Get, $"{_configuration["SecurityService"]}UserManagement/IsLADPUser?LoginName={UserName}", string.Empty, token: _token).Result;
            try
            {
                model.lactiveModule = GetModuleData();
                model.Language = UserInfo.data.FirstOrDefault().sLanguage;
                model.TimeZone = UserInfo.data.FirstOrDefault().sUserTimeZone.ToString();
                model.UserName = UserInfo.data.FirstOrDefault().sFirstName + " " + UserInfo.data.FirstOrDefault().sLastName;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return model;
        }
        private int Displayorder(string ModuleName)
        {
            int displayorder = 0;
            switch (ModuleName)
            {

                case "Dashboard":
                    displayorder = 1;
                    break;
                case "WorkManagement":
                    displayorder = 2;
                    break;
                case "QualityManagement":
                    displayorder = 3;
                    break;
                case "QueryTracker":
                    displayorder = 4;
                    break;
                case "LetterLibrary":
                    displayorder = 5;
                    break;
                case "AssessMe":
                    displayorder = 6;
                    break;
                case "EmailManagement":
                    displayorder = 7;
                    break;
                case "AppConfiguration":
                    displayorder = 8;
                    break;
                case "ProcessGuidance":
                    displayorder = 9;
                    break;
                case "BIReports":
                    displayorder = 10;
                    break;

            }
            return displayorder;
        }
    }
}
