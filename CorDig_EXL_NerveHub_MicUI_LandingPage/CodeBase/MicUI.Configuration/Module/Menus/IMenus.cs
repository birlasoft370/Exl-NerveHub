using MicUI.Configuration.Models;

namespace MicUI.Configuration.Module.Menus
{
    public interface IMenus
    {

        //int RoleId { get; set; }
        //string Token { get; set; }
        //string UserId { get; set; }
        //string Area { get; set; }
        //string UserName { get; set; }
        //string Language { get; set; }
        //string TimeZone { get; set; }
        // Task<UserInfo> IsLDapUser(string loginUser, string token);
        // List<BELandingPageMenu> GetLandingPagedata();
        // List<ModuleData> GetModuleData();
        // NavMenuViewModel NavMenu();//User and Module Menus
        //List<MenuViewModel> GetMenuData(int roleID, string sArea);// Page Menus

        NavMenuViewModel GetAllMenus();
    }
}
