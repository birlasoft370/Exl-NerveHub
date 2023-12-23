using MicUI.Configuration.Services.Security;

namespace MicUI.Configuration.Module.Security.UserMaster
{
    public interface IUserAccessRequestService
    {
        string GetUserDetails(int iUserID);
    }
    public class UserAccessRequestService : IUserAccessRequestService
    {
        private readonly ISecurityApiService _securityService;

        public UserAccessRequestService(ISecurityApiService securityService)
        {
            _securityService = securityService;
        }

        public string GetUserDetails(int iUserID)
        {
            var result = _securityService.GetUserDetailsAsync(iUserID).GetAwaiter().GetResult();
            return result.data ?? string.Empty;
        }
    }
}
