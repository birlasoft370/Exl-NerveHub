using Microsoft.Extensions.Configuration;
using MicSer.SSOAuthorization.HelperModule;
using MicSer.SSOAuthorization.Models;
using MicSer.SSOAuthorization.Repository;
using MicSer.Tenant.Models;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace MicSer.SSOAuthorization.Services
{
    public interface IAuthenticateService
    {
        Task<AuthenticationResponse> IsLDAPUser(AuthenticationRequest authenticationRequest);

    }
    public class AuthenticateService : IAuthenticateService
    {
        private readonly IAuthenticationOperations _authenticationOperations;
        private readonly IJwtTokenService _jwtTokenHandler;
        private readonly IConfiguration _configuration;
        public AuthenticateService(IAuthenticationOperations authenticationOperations, IJwtTokenService jwtTokenHandler, IConfiguration configuration)
        {
            _configuration = configuration;
            _authenticationOperations = authenticationOperations;
            _jwtTokenHandler = jwtTokenHandler;
        }


        internal async Task<BETenant> GetBETenantAsync(string path)
        {
            BETenant product = null;
            var client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
               var content = await response.Content.ReadAsStringAsync();
                product = Convertor. Deserialize<BETenant>(content);
            }
            return product;
        }

        public async Task<AuthenticationResponse> IsLDAPUser(AuthenticationRequest authenticationRequest)
        {
            var BETenant = await GetBETenantAsync(string.Format(GlobalObject.TenantAPI, authenticationRequest.TenantName));

            if (BETenant == null)
                throw new Exception($"Not able to get information : {authenticationRequest.TenantName}.");

            var userInfo = await _authenticationOperations.IsLDAPUser(authenticationRequest, BETenant.DatabaseConnectionString);

            //var userInfo = new BEUserInfo()
            //{
            //    DISABLED = false,
            //    LOGINNAME = authenticationRequest.LoginName,

            //    ConnectionString = BETenant.DatabaseConnectionString,
            //    USERID = 12345,
            //    EMPID = 12345,
            //    TIMEZONEID = 3,

            //    TIMEZONENAME = "India",
            //    LANGUAGE = "Eng",
            //    ServerTimeZone = "",
            //    EMAILID = $"{authenticationRequest.LoginName}@exl.com"
            //};


            if (userInfo != null)
            {
                if(userInfo.DISABLED)
                    throw new Exception($"LOGINNAME : {authenticationRequest.LoginName}. DISABLED");

                var userRole = await _authenticationOperations.UserRoleInfoById(userInfo.USERID, BETenant.DatabaseConnectionString);//124100

                //var userRole = new BERoleInfo()
                //{
                //    RoleName = "SuperAdmin",
                //    RoleID = 124100
                //};

                var jWTInfo = new JWTInfo()
                {
                    UserInfo = userInfo.LOGINNAME.IsNullThenEmpty(),
                    Role = userRole.RoleName.IsNullThenEmpty(),
                    RoleID = Convert.ToString(userRole.RoleID).IsNullThenEmpty(),
                    ConString = userInfo.ConnectionString.IsNullThenEmpty(),
                    UserId = Convert.ToString(userInfo.USERID).IsNullThenEmpty(),
                    EmpId = Convert.ToString(userInfo.EMPID).IsNullThenEmpty(),
                    TimeZoneId = Convert.ToString(userInfo.TIMEZONEID).IsNullThenEmpty(),
                    TimeZoneName = userInfo.TIMEZONENAME.IsNullThenEmpty(),
                    Language = userInfo.LANGUAGE.IsNullThenEmpty(),
                    ServerTimeZone = userInfo.ServerTimeZone.IsNullThenEmpty(),
                    Email = userInfo.EMAILID.IsNullThenEmpty()
                };


                if (string.IsNullOrWhiteSpace(userInfo.LOGINNAME) || string.IsNullOrWhiteSpace(userRole.RoleName))
                {
                    throw new Exception($"LOGINNAME : {authenticationRequest.LoginName}.Not mapping to Any Role");
                }
                else
                {
                  return await _jwtTokenHandler.GenerateJwtToken(jWTInfo);
                }
            }
            else
            {
                throw new Exception($"LOGINNAME : {authenticationRequest.LoginName}.Not Found");
            }
        }
    }
}
