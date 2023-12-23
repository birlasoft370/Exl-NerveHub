using BPA.EmailManagement.BusinessEntity;
using Microsoft.Graph;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace BPA.EmailManagement.BusinessLayer.CacheData
{
    public class MicrosoftGraphConfidentialClientAuthProvider : IAuthenticationProvider
    {
        IConfidentialClientApplication _clientApplication;
        private BEMailConfiguration _mailConfig;

        public MicrosoftGraphConfidentialClientAuthProvider(IConfidentialClientApplication clientApplication, BEMailConfiguration mailConfig)
        {
            _clientApplication = clientApplication;
            _mailConfig = mailConfig;
        }

        public async Task AuthenticateRequestAsync(HttpRequestMessage request)
        {
            var authentication = await GetAuthenticationAsync();
            request.Headers.Authorization = AuthenticationHeaderValue.Parse(authentication.CreateAuthorizationHeader());
        }
        public async Task<AuthenticationResult> GetAuthenticationAsync()
        {
            AuthenticationResult authResult = null;
            IEnumerable<string> Ienum = new List<string>();
            Ienum = _mailConfig.Scope.Split(',').ToList<string>();

            var accounts = await _clientApplication.GetAccountsAsync();
            try
            {
                authResult = await _clientApplication.AcquireTokenSilent(Ienum, accounts.FirstOrDefault()).ExecuteAsync();
            }
            catch (MsalUiRequiredException)
            {
                try
                {
                    authResult = await _clientApplication.AcquireTokenForClient(Ienum).ExecuteAsync();
                }

                catch (MsalException ex)
                {
                    throw ex;
                }
            }

            return authResult;
        }
    }
}
