using MicUI.NerveHub.Models;
using MicUI.NerveHub.Models.Security;

namespace MicUI.NerveHub.Module.Security
{
    public interface IAuthentication
    {
        int RoleId { get; set; }
        string Token { get; set; }
        string UserId { get; set; }
        string Area { get; set; }
        string UserName { get; set; }
        string Language { get; set; }
        string TimeZone { get; set; }


        Task<string> GetToken(StringContent userDetails);
        Task<UserInfo> IsLDapUser(string loginUser,string token);
        List<BELandingPageMenu> GetLandingPagedata();
        List<ModuleData> GetModuleData();
        NavMenuViewModel NavMenu();
        List<MenuViewModel> GenerateMenu(string sArea);
    }
}
