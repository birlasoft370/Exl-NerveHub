using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MicSer.SSOAuthorization.BaseController;
using MicSer.SSOAuthorization.Constants;
using MicSer.SSOAuthorization.HelperModule;
using MicSer.SSOAuthorization.Models;
using MicSer.SSOAuthorization.Services;
using System;
using System.Threading.Tasks;

namespace MicSer.SSOAuthorization.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SSOAuthorizationController : BaseController<SSOAuthorizationController>
    {
        IAuthenticateService _authenticateService;
        IAuthenticateServiceDummy _authenticateServiced;

        IJwtTokenService _jwtTokenService;

        public SSOAuthorizationController(IAuthenticateService authenticateService, IJwtTokenService jwtTokenService, ILogger<SSOAuthorizationController> logger, IWebHostEnvironment env, IConfiguration configuration, IAuthenticateServiceDummy authenticateServiced) : base(logger, env, configuration)
        {
            _authenticateService = authenticateService;
            _jwtTokenService = jwtTokenService;
            _authenticateServiced = authenticateServiced;
        }

        [HttpGet(Name = "ServiceHealth")]
        public async Task<ActionResult<string>> Get()
        {
            try
            {
                    return $"{_applicationName} {GlobalConst.WelcomeMessage}";

            }
            catch (Exception ex)
            {
                return $"{ ex?.Message} { ex?.InnerException?.Message} { ex?.StackTrace}";
            }
        }

        [HttpPost(Name = "SSOTokenAuthorization")]
        public async Task< ActionResult<string>> Post(AuthenticationRequest authenticationRequest)
        {
            try
            {
                if (authenticationRequest.TenantName == null)
                    return $"{_applicationName} {GlobalConst.WelcomeMessage}";

                var result = await _authenticateService.IsLDAPUser(authenticationRequest);
                //var result = GetTenantInfo(tenantName);

                return Convertor.Serialize<AuthenticationResponse>(result);
            }
            catch (Exception ex)
            {
                return $"{ ex?.Message} { ex?.InnerException?.Message} { ex?.StackTrace}";
            }
        }

        [HttpPut(Name = "SSOTokenAuthorization2")]
        public async Task<ActionResult<string>> Put(AuthenticationRequest authenticationRequest)
        {
            try
            {
                if (authenticationRequest.TenantName == null)
                    return $"{_applicationName} {GlobalConst.WelcomeMessage}";
                 
                var result = await _authenticateServiced.IsLDAPUser(authenticationRequest);
                //var result = GetTenantInfo(tenantName);

                return Convertor.Serialize<AuthenticationResponse>(result);
            }
            catch (Exception ex)
            {
                return $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}";
            }
        }


        //private BETenant GetTenantInfo(string TenantName)
        //{
        //    BETenant oTenant = null;
        //    DataSet ds = new DataSet();

        //    var XMLDirectoryPath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), _configuration.GetSection(GlobalConst.TenantFileName)?.Value);
        //    ds.ReadXml(XMLDirectoryPath);

        //    if (ds == null || ds.Tables.Count <= 0)
        //    {
        //        return oTenant;
        //    }
        //    ds.Tables[0].CaseSensitive = false;

        //    var T = ds.Tables[0].Select("ClientName='" + TenantName.ToUpper() + "'");

        //    if (T != null && T.Count() > 0)
        //    {
        //        oTenant = new BETenant()
        //        {
        //            ApplicationHostName = T[0]["ApplicationHostName"].ToString(),
        //            ClientID = int.Parse(T[0]["ClientID"].ToString()),
        //            ClientName = T[0]["ClientName"].ToString(),
        //            DatabaseInstanceIP = T[0]["DatabaseInstanceIP"].ToString(),
        //            DatabaseName = T[0]["DatabaseInstanceName"].ToString(),
        //            TenantID = int.Parse(T[0]["ClientDeploymentID"].ToString()),
        //            XMLfilepath = XMLDirectoryPath,
        //            HBMReportDatabaseInstanceIP = T[0]["HBMReportDatabaseInstanceIP"].ToString(),
        //            HBMReportDatabaseInstanceName = T[0]["HBMReportDatabaseInstanceName"].ToString(),
        //            DataUtilityDatabaseInstanceName = T[0]["DataUtilityDatabaseInstanceName"].ToString(),
        //            DataUtilityDatabaseInstanceIP = T[0]["DataUtilityDatabaseInstanceIP"].ToString(),
        //            ADSDatabaseInstanceName = T[0]["ADSDatabaseInstanceName"].ToString(),
        //            ADSDatabaseInstanceIP = T[0]["ADSDatabaseInstanceIP"].ToString(),
        //            ClientMultiLanguage = T[0]["ClientMultiLanguage"].ToString().ToUpper() == "TRUE" ? true : false
        //        };
        //        // oTenant.DatabaseConnectionString = @"Server=" + T[0]["DatabaseInstanceIP"].ToString() + ";Trusted_Connection=True;database=" + T[0]["DatabaseInstanceName"].ToString() + "; Min Pool Size=10;Max Pool Size=500;MultipleActiveResultSets=True;");
        //        oTenant.DatabaseConnectionString = @"Server=" + T[0]["DatabaseInstanceIP"].ToString() + ";Trusted_Connection=True;database=" + T[0]["DatabaseInstanceName"].ToString() + "; Min Pool Size=10;Max Pool Size=500;MultipleActiveResultSets=True;";
        //        oTenant.HBMReportDatabaseConnectionString = @"Server=" + T[0]["HBMReportDatabaseInstanceIP"].ToString() + ";Trusted_Connection=True;database=" + T[0]["HBMReportDatabaseInstanceName"].ToString() + "; Min Pool Size=10;Max Pool Size=500;MultipleActiveResultSets=True;";
        //        oTenant.DataUtilityDatabaseConnectionString = @"Server=" + T[0]["DataUtilityDatabaseInstanceIP"].ToString() + ";Trusted_Connection=True;database=" + T[0]["DataUtilityDatabaseInstanceName"].ToString() + "; Min Pool Size=10;Max Pool Size=500;MultipleActiveResultSets=True;";
        //        oTenant.ADSDatabaseConnectionString = @"Server=" + T[0]["ADSDatabaseInstanceIP"].ToString() + ";Trusted_Connection=True;database=" + T[0]["ADSDatabaseInstanceName"].ToString() + "; Min Pool Size=10;Max Pool Size=500;MultipleActiveResultSets=True;";
        //    }
        //    return oTenant;
        //}
    }
}
