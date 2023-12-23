using MicUI.WorkManagement.Helper.Sessions;
using MicUI.WorkManagement.Models.ViewModels;
using MicUI.WorkManagement.Services.ModuleMenus;
using System.Resources;

namespace MicUI.WorkManagement.Module.Menus
{
    public interface IMenus
    {
        NavMenuViewModel GetAllMenus();
    }
    public class Menus : IMenus
    {

        private readonly ISecurityApiService _moduleMenusService;
        private readonly IGetSetSessionValues _getSessionValues;

        public Menus(ISecurityApiService moduleMenusService, IGetSetSessionValues getSessionValues)
        {
            _moduleMenusService = moduleMenusService;
            _getSessionValues = getSessionValues;
        }
        public NavMenuViewModel GetAllMenus()
        {
            NavMenuViewModel model = new();

            ResourceManager rm = BPA.GlobalResources.Resources_MenuItems.ResourceManager;

            var UserInfo = _getSessionValues.GetSessionUserInfo();

            model.Language = UserInfo.sLanguage;
            model.TimeZone = UserInfo.sUserTimeZone.ToString();
            model.UserName = UserInfo.sFirstName + " " + UserInfo.sLastName;


            var UserMenus = _moduleMenusService.GetRoleWiseMenuAsync(UserInfo.oRoles[0].iRoleID).Result;

            //Module Menus
            var Modulemenulist = from n in UserMenus.data
                                 where n.bHasPermission == true
                                 group n.sModuleName by n.sModuleName into nGroup
                                 select nGroup.Key;

            model.lactiveModule = Modulemenulist.Select(x => new ModuleData()
            {
                DisplayOrder = Displayorder(x),
                ModuleName = x
            }).OrderBy(p => p.DisplayOrder).ToList();

            //Page Menus
            model.MenuViewModel = UserMenus.data.Where(itmeValue => ((itmeValue.sModuleName == "WorkManagement" || itmeValue.URL == "")
            && itmeValue.iFormID != null && itmeValue.NodeName != null
            && itmeValue.ParentID != null && itmeValue.MenuOrder != null
            && ((itmeValue.bHasPermission && itmeValue.URL != "") || (itmeValue.URL == "" && itmeValue.bHasPermission == false))))
                 .Select(item => new MenuViewModel()
                 {
                     Id = item.NodeID,
                     Name = rm.GetString("text_" + item.NodeID.ToString()) == null ? item.NodeName : rm.GetString("text_" + item.NodeID.ToString()),
                     ParentId = item.ParentID,
                     SortOrder = item.MenuOrder,
                     Url = item.URL,
                     //Flag = item.Flag != "" ? AntiXssEncoder.HtmlEncode(EncryptDecrypt.Encrypt(item.Flag), false) : "",
                     Flag = item.Flag != "" ? item.Flag : "",
                     FormID = item.iFormID,
                     Areas = item.sModuleName,
                     RouterController = item.sController,
                     RouterAction = item.sAction,
                     IconClass = item.sIconClass
                 }).ToList();

            _getSessionValues.SetMenusFormInfo(model); //To Verify the Page Access :- Authorization

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
