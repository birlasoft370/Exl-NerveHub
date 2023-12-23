using MicUI.WorkManagement.Models.ViewModels;
using MicUI.WorkManagement.Services.ServiceModel;

namespace MicUI.WorkManagement.Helper.Sessions
{
    public interface IGetSetSessionValues
    {
        BETenantInfo GetTenantInfo();
        BEUserInfo GetSessionUserInfo();
        void SetSessionUserInfo(BEUserInfo bEUserInfo);
        void SetMenusFormInfo(NavMenuViewModel navMenuViewModel);
        NavMenuViewModel GetMenusFormInfo();
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
            return _httpContextAccessor.HttpContext?.Session.GetObjectFromJson<BETenantInfo>(SessionVariables.SessionKeyTenantInfo) ?? new BETenantInfo();
        }

        public BEUserInfo GetSessionUserInfo()
        {
            return _httpContextAccessor.HttpContext?.Session.GetObjectFromJson<BEUserInfo>(SessionVariables.SessionKeyUserInfo) ?? new BEUserInfo();
        }

        public void SetSessionUserInfo(BEUserInfo bEUserInfo)
        {
            _httpContextAccessor.HttpContext?.Session.SetObjectAsJson(SessionVariables.SessionKeyUserInfo, bEUserInfo);
        }

        public NavMenuViewModel GetMenusFormInfo()
        {
            return _httpContextAccessor.HttpContext?.Session.GetObjectFromJson<NavMenuViewModel>(SessionVariables.SessionKeyMenusInfo)?? new NavMenuViewModel();
        }

        public void SetMenusFormInfo(NavMenuViewModel navMenuViewModel)
        {
            _httpContextAccessor.HttpContext?.Session.SetObjectAsJson(SessionVariables.SessionKeyMenusInfo, navMenuViewModel);
        }
    }
}
