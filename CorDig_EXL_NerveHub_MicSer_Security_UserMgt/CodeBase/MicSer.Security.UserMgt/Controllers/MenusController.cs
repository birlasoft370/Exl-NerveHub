using BPA.Security.ServiceContract;
using BPA.Security.ServiceContracts.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MicSer.Security.UserMgt.BaseController;
using MicSer.Security.UserMgt.Helper.Filter;
using MicSer.Security.UserMgt.Helper;
using MicSer.Security.UserMgt.Models;
using System.Reflection;
using BPA.Security.BusinessEntity;
using BPA.Security.BusinessEntity.ExtrernalRefre;
using System.Data;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;

namespace MicSer.Security.UserMgt.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenusController : BaseController<MenusController>
    {
        private readonly IMenuService _menuService;
        public MenusController(IMenuService menuService, ILogger<MenusController> logger, IWebHostEnvironment env, IConfiguration configuration) : base(logger, env, configuration)

        {
            this._menuService = menuService;


        }
        [HttpGet]
        [JwtAuthentication]
       
        [Route("GetLandingData")]
        public ActionResult<MessageResponse<List<BELandingPageMenu>>>GetLandingData()
        {
            var result = new MessageResponse<List<BELandingPageMenu>>();
            try
            {
               
              
              var data  =  _menuService.GetLandingData(TanenetInfo);
                List<BELandingPageMenu> lactive_Module = new List<BELandingPageMenu>();
                foreach (DataRow dr in data.Tables[0].Rows)
                {
                    BELandingPageMenu odata = new BELandingPageMenu();
                    odata.MODULENAME = dr["ModuleName"].ToString();
                   
                   
                    odata.READACTION = Convert.ToBoolean(dr["ReadAction"].ToString());
                    odata.TEXT = dr["Text"].ToString();
                    lactive_Module.Add(odata);
                }
                result.data = lactive_Module;
            }
            catch (Exception ex)
            {
                result.message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;

        }
        [HttpGet]
        [JwtAuthentication]
        [Route("GetRoleWiseMenu")]
        public ActionResult<MessageResponse<List<BEMenuItems>>> GetRoleWiseMenu(int RoleId, bool isMossApplicationMenu)
        {
            var result = new MessageResponse<List<BEMenuItems>>();
            try
            {

               
                result.data =  _menuService.GetRoleWiseMenu(RoleId, isMossApplicationMenu, TanenetInfo);

            }
            catch (Exception ex)
            {
                result.message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;

        }
        private BETenant GetTanent(TokenDetail _tokenDetails)
        {
            BETenant bETenant = new BETenant();
            bETenant.TenantID = Convert.ToInt32(_tokenDetails.TenantID);
            bETenant.ClientID = Convert.ToInt32(_tokenDetails.ClientID); ;
            bETenant.ClientName = _tokenDetails.ClientID;
            bETenant.ApplicationHostName = _tokenDetails.ApplicationHostName;
            bETenant.DatabaseName = _tokenDetails.DatabaseName;
            bETenant.DatabaseInstanceIP = _tokenDetails.DatabaseInstanceIP;
            bETenant.DatabaseConnectionString = _tokenDetails.DatabaseConnectionString;
            bETenant.ClientMultiLanguage = Convert.ToBoolean(_tokenDetails.ClientMultiLanguage);
            bETenant.HBMReportDatabaseInstanceName = _tokenDetails.HBMReportDatabaseInstanceName;
            bETenant.HBMReportDatabaseInstanceIP = _tokenDetails.HBMReportDatabaseInstanceIP;
            bETenant.HBMReportDatabaseConnectionString = _tokenDetails.HBMReportDatabaseConnectionString;
            bETenant.DataUtilityDatabaseInstanceName = _tokenDetails.DataUtilityDatabaseInstanceName;
            bETenant.DataUtilityDatabaseInstanceIP = _tokenDetails.HBMReportDatabaseInstanceIP;
            bETenant.DataUtilityDatabaseConnectionString = _tokenDetails.DataUtilityDatabaseConnectionString;
            bETenant.ADSDatabaseInstanceName = _tokenDetails.ADSDatabaseInstanceName;
            bETenant.ADSDatabaseInstanceIP = _tokenDetails.ADSDatabaseInstanceIP;
            bETenant.ADSDatabaseConnectionString= _tokenDetails.ADSDatabaseConnectionString;
            bETenant.XMLfilepath = _tokenDetails.XMLfilepath;
            return bETenant;
        }
    }
}
