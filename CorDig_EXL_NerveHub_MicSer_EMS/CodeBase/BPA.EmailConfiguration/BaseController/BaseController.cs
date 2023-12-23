using BPA.EmailConfiguration.Constants;
using BPA.EmailConfiguration.Models;
using BPA.EmailManagement.BusinessEntity;
using BPA.EmailManagement.BusinessEntity.ExternalRef.Application;
using BPA.EmailManagement.BusinessEntity.ExternalRef.Security;
using BPA.EmailManagement.BusinessLayer.ExternalRef.Security;
using BPA.EmailManagement.ServiceContract.ExternalRef.Security;
using Microsoft.AspNetCore.Mvc;

namespace BPA.EmailConfiguration.BaseController
{
    public abstract class BaseController<T> : ControllerBase
    {
        internal readonly ILogger<T> _logger;
        internal readonly IWebHostEnvironment _env;
        internal readonly IConfiguration _configuration;

        public BaseController(ILogger<T> logger, IWebHostEnvironment env, IConfiguration configuration)
        {
            _logger = logger;
            _env = env;
            GlobalObject.IConfigurationObject = _configuration = configuration;
        }

        internal string Token
        {
            get
            {
                return Common.GetValue<string>(this.HttpContext, GlobalConst.TokenKey);
            }
        }

        internal TokenDetail TokenDetails
        {
            get
            {
                return Common.GetValue<TokenDetail>(this.HttpContext, GlobalConst.TokenInfo);
            }
        }

        internal string CorrelationId
        {
            get
            {
                return Common.GetValue<string>(this.HttpContext, GlobalConst.CorrelationIdHeader);
            }
        }

        internal string ApplicationName
        {
            get
            {
                return GlobalObject.ApplicationName;
            }
        }

        internal BETenant oTenant
        {
            get
            {
                return EmailConfiguration.Helper.TenantInfo.GetTanentInfo(TokenDetails);
            }
        }

        internal string IpAddress
        {
            get
            {
                return Common.GetValue<string>(this.HttpContext, "ipAddress");
            }
        }
        internal BEUserInfo oUser
        {
            get
            {
                int Cacheduration = Convert.ToInt16(_configuration.GetSection("CacheSetting:DurationInHours").Value ?? "24");

                IList<BEUserInfo> oLDAPUser = new List<BEUserInfo>();
                BESession oBESession = new();
                int SessionID = 0;
                bool bProcessMapped = false;

                oBESession.sSystemSessionID = SessionID.ToString();
                oBESession.sHostName = string.Empty;
                oBESession.sIPAddress = IpAddress;

                using (IAuthenticateService oBLAuthenticate = new BLAuthenticate())
                {
                    oLDAPUser = oBLAuthenticate.IsLADPUser(TokenDetails.LoginName, oBESession, out SessionID, out bProcessMapped, oTenant, cachDuration: Cacheduration);
                }
                return oLDAPUser[0];
            }
        }



    }
}
