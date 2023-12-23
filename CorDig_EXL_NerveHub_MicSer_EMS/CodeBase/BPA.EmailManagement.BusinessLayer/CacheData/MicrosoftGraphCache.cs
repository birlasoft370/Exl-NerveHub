using BPA.EmailManagement.BusinessEntity;
using Microsoft.Graph;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPA.EmailManagement.BusinessLayer.CacheData
{
    public class MicrosoftGraphCache : IDisposable
    {
        public void Dispose()
        {
            //throw new NotImplementedException();
        }

        public GraphServiceClient GetGraphServiceClient(BEMailConfiguration mailConfig)
        {
            if (string.IsNullOrEmpty(mailConfig.Authority))
            {
                mailConfig.Authority = string.Format(System.Globalization.CultureInfo.InvariantCulture, mailConfig.Instance == null ? "" : mailConfig.Instance, mailConfig.TenentID == null ? "" : mailConfig.TenentID);
            }
            var authenticationProvider = CreateAuthorizationProvider(mailConfig);
            return new GraphServiceClient(authenticationProvider);
        }

        private IAuthenticationProvider CreateAuthorizationProvider(BEMailConfiguration mailConfig)
        {
            if (mailConfig.bEMSWebServerHosting)
            {
                var application = ConfidentialClientApplicationBuilder.Create(mailConfig.ClinetID).WithClientSecret(mailConfig.sAutoDiscoveryPath).WithAuthority(mailConfig.Authority).WithRedirectUri(mailConfig.RedirectUrl)
                         .Build();
                return new MicrosoftGraphConfidentialClientAuthProvider(application, mailConfig);
            }
            else
            {
                var application = PublicClientApplicationBuilder.Create(mailConfig.ClinetID).WithAuthority(mailConfig.Authority).WithRedirectUri(mailConfig.RedirectUrl)
                  .Build();
                return new MicrosoftGraphMsalAuthProvider(application, mailConfig);

            }
        }
    }
}
