using BPA.EmailManagement.BusinessEntity;
using BPA.EmailManagement.BusinessEntity.ExternalRef.Application;
using BPA.EmailManagement.BusinessEntity.ExternalRef.Security;
using BPA.EmailManagement.BusinessLayer.CacheData;
using BPA.EmailManagement.DataLayer.ExternalRef.Security;
using BPA.EmailManagement.ServiceContract.ExternalRef.Security;

namespace BPA.EmailManagement.BusinessLayer.ExternalRef.Security
{
    public class BLAuthenticate : IAuthenticateService, IDisposable
    {
        CacheHelper _UserInfoCache = null;

        public BLAuthenticate()
        {
            _UserInfoCache = new CacheHelper("UserInfo");
        }
        public void Dispose()
        { }

        public List<BEUserInfo> IsLADPUser(string LoginName, BESession oBESession, out int iSessionID, out bool bProcessMap, BETenant oTenant, bool isWindowsAuthentication = true, int cachDuration = 24)
        {
            return IsLADPUser(LoginName, oBESession, out iSessionID, out bProcessMap, false, oTenant, isWindowsAuthentication, cachDuration);
        }
        public List<BEUserInfo> IsLADPUser(string LoginName, BESession oBESession, out int iSessionID, out bool bProcessMap, bool isMossApplicationMenu, BETenant oTenant, bool isWindowsAuthentication = true, int cachDuration = 24)
        {
            List<BEUserInfo> oAppUserCacheObject = new();
            string cacheName = $"UserInfo_{LoginName}_{oBESession.sIPAddress}";
            oAppUserCacheObject = (List<BEUserInfo>)_UserInfoCache.GetFromCache(cacheName);
            if (oAppUserCacheObject == null || oAppUserCacheObject.Count == 0)
            {
                using (DLPermission oDLPermission = new DLPermission(oTenant))
                {
                    oAppUserCacheObject = oDLPermission.IsLDAPUser(LoginName, oBESession, out iSessionID, out bProcessMap, isMossApplicationMenu, isWindowsAuthentication);
                    _UserInfoCache.AddToCache(cacheName, oAppUserCacheObject, TimeSpan.FromHours(cachDuration));
                }
            }
            iSessionID = 0;
            bProcessMap = false;
            return oAppUserCacheObject;
        }
    }
}
