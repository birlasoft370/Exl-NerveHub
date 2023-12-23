using BPA.Security.BusinessEntity.Security;
using BPA.Security.ServiceContracts.Security;
using Microsoft.AspNetCore.Mvc;
using MicSer.Security.UserMgt.BaseController;
using MicSer.Security.UserMgt.Helper;
using MicSer.Security.UserMgt.Helper.Filter;
using MicSer.Security.UserMgt.Models;
using MicSer.Security.UserMgt.Models.Request;
using MicSer.Security.UserMgt.Models.Response;
using System.Data;
using System.Reflection;

namespace MicSer.Security.UserMgt.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [JwtAuthentication]
    public class PowerUserController : BaseController<PowerUserController>
    {
        #region private Properties
        private readonly IPermissionService _repositoryPermission;
        private readonly IAuthenticateService _repositoryAuthenticate;

        #endregion
        #region Parameterised Constructor
        public PowerUserController(IPermissionService repositoryPermission, IAuthenticateService repositoryAuthenticate, ILogger<PowerUserController> logger, IWebHostEnvironment env, IConfiguration configuration) : base(logger, env, configuration)
        {
            this._repositoryPermission = repositoryPermission;
            this._repositoryAuthenticate = repositoryAuthenticate;
        }
        #endregion

        [ProducesResponseType(typeof(BEUserInfo), StatusCodes.Status200OK)]
        [HttpGet]
        [Route("GetUserDetailsWithRole")]
        public async Task<MessageResponse<BEUserInfo>> GetUserDetailsWithRole([FromQuery] int iUserID)
        {
            var result = new MessageResponse<BEUserInfo>();
            try
            {

                var value = await Task.Run(() => _repositoryPermission.GetUserDetailsWithRole(iUserID, TanenetInfo));
                result.data = value;
            }
            catch (Exception ex)
            {
                result.message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }

        [ProducesResponseType(typeof(List<RoleApproverUserList>), StatusCodes.Status200OK)]
        [HttpGet]
        [Route("GetUserRoleApproverList")]
        public async Task<MessageResponse<List<RoleApproverUserList>>> GetUserRoleApproverList([FromQuery] int iRoleId)
        {
            var result = new MessageResponse<List<RoleApproverUserList>>();
            try
            {

                var value = await Task.Run(() => _repositoryPermission.GetUserRoleApproverList(iRoleId, TanenetInfo));
                var roleApproverList = value.Tables[0].AsEnumerable().Select(x => new RoleApproverUserList()
                {
                    UserId = Int32.Parse(Convert.ToString(x["UserId"]) ?? ""),
                    Agent = Convert.ToString(x["Agent"])
                }).ToList();
                result.data = roleApproverList;
            }
            catch (Exception ex)
            {
                result.message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }
        /*
        [HttpGet]
        [Route("GetUserList")]
        public ActionResult<MessageResponse<IList<BELdapUserInfo>>> GetUserList(string LoginName, string Domain)
        {
            var result = new MessageResponse<IList<BELdapUserInfo>>();
            try
            {

                var Results = _repositoryAuthenticate.GetUserList(LoginName, Domain, TanenetInfo);
                result.data = Results;
            }
            catch (Exception ex)
            {
                result.message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }*/
        [HttpPost]
        [Route("InsertUserRoleData")]
        public async Task<MessageResponse<string>> InsertUserRoleData([FromBody] PowerUserInfo oUser, [FromQuery] int iMode, [FromQuery] int iFormID)
        {
            var result = new MessageResponse<string>();
            try
            {
                await Task.Run(() => _repositoryPermission.InsertUserRoleData(CatchRecord(oUser), iMode, iFormID, TanenetInfo));
                result.data = "Ok";
            }
            catch (Exception ex)
            {
                result.message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }

        private BEUserInfo CatchRecord(PowerUserInfo userInfo)
        {
            BEUserInfo oUserData = new()
            {
                iEmployeeID = userInfo.EmployeeID,
                bLanID = userInfo.LanID,
                sFirstName = userInfo.FirstName,
                sMiddleName = userInfo.MiddleName,
                sLastName = userInfo.LastName,
                sEmail = userInfo.Email,
                sLoginName = userInfo.LoginName,
                bDisabled = userInfo.Disabled,
                iCreatedBy = base.oUser.iUserID,//userinfo.createdby
                oRoles = new List<BERoleInfo>()
                {
                    new BERoleInfo(){
                        iCreatedBy = userInfo.Roles[0].CreatedBy,
                        iRoleID=userInfo.Roles[0].RoleID
                    }
                },
                dModifyDate = userInfo.ModifyDate ?? default,
                iModifiedBy = userInfo.ModifiedBy ?? base.oUser.iUserID
            };
            return oUserData;
        }

        [HttpDelete]
        [Route("DeleteData")]
        public ActionResult<MessageResponse<int>> DeleteData(BEUserInfo oUser, int iFormID)
        {
            var result = new MessageResponse<int>();
            try
            {

                _repositoryPermission.DeleteData(oUser, iFormID, TanenetInfo);
                result.data = 1;
            }
            catch (Exception ex)
            {
                result.message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }

    }
}
