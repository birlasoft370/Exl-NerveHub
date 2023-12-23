using MicUI.Configuration.Models;
using MicUI.Configuration.Models.Security;
using MicUI.Configuration.Services.ServiceModel;

namespace MicUI.Configuration.Helper.Sessions
{
    public interface IGetSetSessionValues
    {
        BETenantInfo GetTenantInfo();
        BEUserInfo GetSessionUserInfo();
        void SetSessionUserInfo(BEUserInfo bEUserInfo);
        void SetMenusFormInfo(NavMenuViewModel navMenuViewModel);
        NavMenuViewModel GetMenusFormInfo();
        string GetUserToken();
    }

    public class GetSetSessionValues : IGetSetSessionValues
    {
        private IHttpContextAccessor _httpContextAccessor;

        public GetSetSessionValues(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public BETenantInfo GetTenantInfo()
        {
            return _httpContextAccessor.HttpContext.Session.GetObjectFromJson<BETenantInfo>(SessionVariables.SessionKeyTenantInfo);
        }

        public BEUserInfo GetSessionUserInfo()
        {
            return _httpContextAccessor.HttpContext.Session.GetObjectFromJson<BEUserInfo>(SessionVariables.SessionKeyUserInfo);
        }

        public void SetSessionUserInfo(BEUserInfo bEUserInfo)
        {
            _httpContextAccessor.HttpContext.Session.SetObjectAsJson(SessionVariables.SessionKeyUserInfo, bEUserInfo);
        }

        public NavMenuViewModel GetMenusFormInfo()
        {
            return _httpContextAccessor.HttpContext.Session.GetObjectFromJson<NavMenuViewModel>(SessionVariables.SessionKeyMenusInfo);
        }

        public void SetMenusFormInfo(NavMenuViewModel navMenuViewModel)
        {
            _httpContextAccessor.HttpContext.Session.SetObjectAsJson(SessionVariables.SessionKeyMenusInfo, navMenuViewModel);
        }

        public string GetUserToken()
        {
            return _httpContextAccessor.HttpContext.Session.GetString(SessionVariables.SessionKeyUserToken);
        }
    }
}
