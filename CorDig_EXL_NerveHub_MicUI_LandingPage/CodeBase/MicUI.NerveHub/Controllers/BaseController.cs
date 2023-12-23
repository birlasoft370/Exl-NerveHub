using Microsoft.AspNetCore.Mvc;
using MicUI.NerveHub.Models;
using MicUI.NerveHub.Models.Security;

namespace MicUI.NerveHub.Controllers
{
    public class BaseController : Controller
    {

        //    public IList<ModuleData> GetModuleData(int roleID)
        //    {
        //        IList<ModuleData> lactiveModule = null;

        //        //string SessionName = "lactiveModule_" + ((BETenant)Session["TenantData"]).TenantID.ToString() + "_" + roleID.ToString();


        //        else
        //        {
        //            lactiveModule = new List<ModuleData>();
        //            using (BLMenuItems oMenu = new BLMenuItems(roleID, true, _oTenant))
        //            {
        //                var MenuItems = from n in oMenu.GetMenuItems
        //                                group n.sModuleName by n.sModuleName into nGroup
        //                                select nGroup.Key;
        //                foreach (string str in MenuItems)
        //                {
        //                    ModuleData odata = new ModuleData();
        //                    odata.ModuleName = str;
        //                    odata.DisplayOrder = UISharedLayer.Displayorder(str);
        //                    lactiveModule.Add(odata);
        //                }
        //            }
        //            lactiveModule = lactiveModule.OrderBy(p => p.DisplayOrder).ToList<ModuleData>();
        //          //  Session[SessionName] = lactiveModule;
        //        }
        //        return lactiveModule;
        //    }
        //    public PartialViewResult NavMenu()
        //    {

        //        NavMenuViewModel model = new NavMenuViewModel();
        //        UserInfo userInfo = new UserInfo();

        //        try
        //        {



        //                    model.lactiveModule = GetModuleData(userInfo.roleId);


        //              //  model.ClientName = oTenant.ClientName.Replace(" ", "_") + "_" + oTenant.ClientID;
        //               // model.flag = oTenant.ClientName.Replace(" ", "_");
        //                model.Language = HttpContext.Session.GetString("culture") == null ? "en-US" : HttpContext.Session.GetString("culture").ToString();
        //                // model.Language = String.IsNullOrEmpty(_oUser.sLanguage) == true ? "en-US" : _oUser.sLanguage;
        //               // model.TimeZone = String.IsNullOrEmpty(_oUser.sUserTimeZone) == true ? "NA" : _oUser.sUserTimeZone;
        //            }

        //        }
        //        catch (Exception ex)
        //        {
        //            TempData["PageError"] = ex.Message;
        //        }
        //        // model.Area = (sArea == null) ? "" : sArea;
        //        return PartialView("_NavMenu", model);
        //    }
        //    // [ChildActionOnly]
        //    public PartialViewResult GenerateMenu(string sArea)
        //    {

        //        IList<MenuViewModel> model = null;
        //        try
        //        {



        //                model = GetMenuData(_oUser.oRoles[i].iRoleID, sArea);

        //        }
        //        catch (Exception ex)
        //        {
        //            TempData["PageError"] = ex.Message;
        //        }
        //        return PartialView("_Menu", model);
        //    }
        //}
    }
}
