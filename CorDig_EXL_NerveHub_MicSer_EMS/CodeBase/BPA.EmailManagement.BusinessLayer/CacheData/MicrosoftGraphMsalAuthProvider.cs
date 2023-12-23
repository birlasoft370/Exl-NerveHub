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
    public class MicrosoftGraphMsalAuthProvider : IAuthenticationProvider
    {
        private IPublicClientApplication _clientApplication;
        //private string[] _scopes;
        private BEMailConfiguration _mailConfig;

        public MicrosoftGraphMsalAuthProvider(IPublicClientApplication clientApplication, BEMailConfiguration mailConfig)
        {
            _clientApplication = clientApplication;
            // _scopes = scopes;
            _mailConfig = mailConfig;
        }
        /// <summary>
        /// Update HttpRequestMessage with credentials
        /// </summary>
        public async Task AuthenticateRequestAsync(HttpRequestMessage request)
        {
            var authentication = await GetAuthenticationAsync();
            request.Headers.Authorization = AuthenticationHeaderValue.Parse(authentication.CreateAuthorizationHeader());
        }
        /// <summary>
        /// Acquire Token for user
        /// </summary>
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
                    // authResult = await _clientApplication.AcquireTokenInteractive(_scopes).ExecuteAsync();
                    authResult = await _clientApplication.AcquireTokenByUsernamePassword(Ienum, _mailConfig.sEmailID.Trim(), convertToSecureString(BPA.Utility.EncryptDecrypt.Decrypt(_mailConfig.sPassword))).ExecuteAsync();
                    //authResult = await _clientApplication.AcquireTokenByUsernamePassword(_scopes, mailConfig.sEmailID.Trim(), convertToSecureString(BPA.Utility.EncryptDecrypt.Decrypt(mailConfig.oUserCredential.sPassword))).ExecuteAsync();
                    //authResult = await _clientApplication.AcquireTokenByIntegratedWindowsAuth(_scopes).ExecuteAsync();
                }

                catch (MsalException ex)
                {
                    throw ex;
                }
            }

            return authResult;
        }
        public System.Security.SecureString convertToSecureString(string strPassword)
        {
            var secureStr = new System.Security.SecureString();
            if (strPassword.Length > 0)
            {
                foreach (var c in strPassword.ToCharArray()) secureStr.AppendChar(c);
            }
            return secureStr;
        }


    }
}
