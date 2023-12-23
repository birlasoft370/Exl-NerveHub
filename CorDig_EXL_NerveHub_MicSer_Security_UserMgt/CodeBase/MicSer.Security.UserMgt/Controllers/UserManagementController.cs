using BPA.Security.BusinessEntity.ExtrernalRefre;
using BPA.Security.BusinessEntity.Security;
using BPA.Security.ServiceContract.ExternalRef;
using BPA.Security.ServiceContracts.Security;
using Microsoft.AspNetCore.Mvc;
using MicSer.Security.UserMgt.BaseController;
using MicSer.Security.UserMgt.Helper;
using MicSer.Security.UserMgt.Helper.Filter;
using MicSer.Security.UserMgt.Models;
using MicSer.Security.UserMgt.Models.Request;
using MicSer.Security.UserMgt.Models.Response;
using Newtonsoft.Json;
using System.Data;
using System.Reflection;

namespace MicSer.Security.UserMgt.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [JwtAuthentication]
    public class UserManagementController : BaseController<UserManagementController>
    {
        private readonly IFacilityService _repositoryFacility;
        private readonly ILOBService _repositoryLOB;
        private readonly ISBUInfoService _repositorySBUInfo;
        private readonly IRolesService _repositoryRole;
        private readonly IPermissionService _repositoryPermission;
        private readonly IClientService _repositoryClient;
        private readonly ICampaignService _repositoryCampaign;
        private readonly IProcessService _repositoryProcess;
        private readonly IAuthenticateService _repositoryAuthenticate;
        private readonly IUserAccessRequestService _repositoryUserAccess;

        public UserManagementController(
            IFacilityService repositoryFacility,
            ILOBService repositoryLOB,
            ISBUInfoService repositorySBUInfo,
            IRolesService repositoryRole,
            IPermissionService repositoryPermission,
            IClientService repositoryClient,
            ICampaignService repositoryCampaign,
            IProcessService repositoryProcess,
            IAuthenticateService repositoryAuthenticate,
            IUserAccessRequestService repositoryUserAccess,
            ILogger<UserManagementController> logger, IWebHostEnvironment env, IConfiguration configuration) : base(logger, env, configuration)
        {

            _repositoryFacility = repositoryFacility;
            _repositoryLOB = repositoryLOB;
            _repositorySBUInfo = repositorySBUInfo;
            _repositoryRole = repositoryRole;
            _repositoryPermission = repositoryPermission;
            _repositoryClient = repositoryClient;
            _repositoryCampaign = repositoryCampaign;
            _repositoryProcess = repositoryProcess;
            _repositoryAuthenticate = repositoryAuthenticate;
            _repositoryUserAccess = repositoryUserAccess;
        }
        [HttpGet]

        [Route("IsLADPUser")]
        public ActionResult<MessageResponse<List<BEUserInfo>>> IsLADPUser()
        {
            var result = new MessageResponse<List<BEUserInfo>>();
            try
            {
                int iSessionid = 1;
                bool bProcessMap = true;
                BESession oBESession = new BESession();
                oBESession.sIPAddress = "::1";
                oBESession.sHostName = "bi";
                oBESession.iSessionID = 1;
                oBESession.sSystemSessionID = "t";
                var results = _repositoryAuthenticate.IsLADPUser(TokenDetails.LoginName, oBESession, out iSessionid, out bProcessMap, TanenetInfo); ;
                result.data = results;
            }
            catch (Exception ex)
            {
                result.message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }

        [HttpGet]

        [Route("GetFacilityList")]
        public ActionResult<MessageResponse<List<BEFacility>>> GetFacilityList()
        {
            var result = new MessageResponse<List<BEFacility>>();
            try
            {

                var value = _repositoryFacility.GetFacilityList(TanenetInfo);
                result.data = value;
            }
            catch (Exception ex)
            {
                result.message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }
        [HttpGet]

        [Route("GetLOBList")]
        public ActionResult<MessageResponse<List<BELOBInfo>>> GetLOBList(bool IsActiveLOB)
        {
            var result = new MessageResponse<List<BELOBInfo>>();
            try
            {

                var value = _repositoryLOB.GetLOBList(IsActiveLOB, TanenetInfo);
                result.data = value;
            }
            catch (Exception ex)
            {
                result.message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }
        [HttpGet]

        [Route("GetSBUList")]
        public ActionResult<MessageResponse<List<BESBUInfo>>> GetSBUList(bool IsActiveLOB)
        {
            var result = new MessageResponse<List<BESBUInfo>>();
            try
            {

                var value = _repositorySBUInfo.GetSBUList(IsActiveLOB, TanenetInfo);
                result.data = value;
            }
            catch (Exception ex)
            {
                result.message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }

        [ProducesResponseType(typeof(List<BEUserInfo>), StatusCodes.Status200OK)]
        [HttpGet]
        [Route("GetUserJobCode")]
        public async Task<MessageResponse<List<BEUserInfo>>> GetUserJobCode()
        {
            var result = new MessageResponse<List<BEUserInfo>>();
            try
            {

                var value = await Task.Run(() => _repositoryRole.GetUserJobCode(TanenetInfo));
                result.data = value;
            }
            catch (Exception ex)
            {
                result.message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }

        [HttpGet]
        [Route("GetClientRoleListByUser")]
        public ActionResult<MessageResponse<List<BERoleInfo>>> GetClientRoleListByUser(int userID)
        {
            var result = new MessageResponse<List<BERoleInfo>>();
            try
            {

                var value = _repositoryRole.GetClientRoleListByUser(userID, TanenetInfo);
                result.data = value;
            }
            catch (Exception ex)
            {
                result.message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }

        [ProducesResponseType(typeof(List<BERoleInfo>), StatusCodes.Status200OK)]
        [HttpGet]
        [Route("GetClientRoleList")]
        public async Task<MessageResponse<List<BERoleInfo>>> GetClientRoleList()
        {
            var result = new MessageResponse<List<BERoleInfo>>();
            try
            {

                var value = await Task.Run(() => _repositoryRole.GetClientRoleList(TanenetInfo));
                result.totalCount = value.Count;
                result.data = value;
            }
            catch (Exception ex)
            {
                result.message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }

        [ProducesResponseType(typeof(List<BEUserSetting>), StatusCodes.Status200OK)]
        [HttpGet]
        [Route("GetUserSetting")]
        public async Task<MessageResponse<List<BEUserSetting>>> GetUserSetting(int iUserID)
        {
            var result = new MessageResponse<List<BEUserSetting>>();
            try
            {

                var value = await Task.Run(() => _repositoryPermission.GetUserSetting(iUserID, TanenetInfo));
                result.totalCount = value.Count;
                result.data = value;
            }
            catch (Exception ex)
            {
                result.message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }

        [ProducesResponseType(typeof(List<BEUserInfo>), StatusCodes.Status200OK)]
        [HttpGet]
        [Route("GetUserRoleListUserRole")]
        public async Task<MessageResponse<List<BEUserInfo>>> GetUserRoleListUserRole()
        {
            var result = new MessageResponse<List<BEUserInfo>>();
            try
            {

                var value = await Task.Run(() => _repositoryPermission.GetUserRoleListUserRole(base.oUser.iUserID, 4, TanenetInfo));
                result.data = value;
            }
            catch (Exception ex)
            {
                result.message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }

        [HttpGet]
        [Route("GetUserApproverListByProcessId")]
        public ActionResult<MessageResponse<List<BEUserInfo>>> GetUserApproverListByProcessId(int iUserid, int iFormId, int ProcessId)
        {
            var result = new MessageResponse<List<BEUserInfo>>();
            try
            {

                var value = _repositoryPermission.GetUserApproverListByProcessId(iUserid, iFormId, ProcessId, TanenetInfo);
                result.data = value;
            }
            catch (Exception ex)
            {
                result.message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }

        [ProducesResponseType(typeof(List<UserApproverDetail>), StatusCodes.Status200OK)]
        [HttpGet]
        [Route("GetUserApproverList")]
        public async Task<MessageResponse<List<UserApproverDetail>>> GetUserApproverList([FromQuery] int userId, [FromQuery] int clientId, [FromQuery] int processId, [FromQuery] int flag, [FromQuery] int iFormID)
        {
            var result = new MessageResponse<List<UserApproverDetail>>();
            try
            {

                var value = await Task.Run(() => _repositoryPermission.GetUserApproverList(userId, clientId, processId, flag, iFormID, TanenetInfo));
                var approverList = value.Tables[0].AsEnumerable().Select(x => new UserApproverDetail()
                {
                    ApproverId = x.Field<int>("UserId"),
                    ApproverName = x.Field<string>("Agent") ?? ""
                }).ToList();
                result.data = approverList;
                result.totalCount = approverList.Count;
            }
            catch (Exception ex)
            {
                result.message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }


        [ProducesResponseType(typeof(List<BEUserInfo>), StatusCodes.Status200OK)]
        [HttpGet]
        [Route("GetUserList")]
        public async Task<MessageResponse<List<BEUserInfo>>> GetUserList([FromQuery] string? sLoginName, [FromQuery] bool bActiveUser, [FromQuery] string? searchCondition)
        {
            var result = new MessageResponse<List<BEUserInfo>>();
            try
            {
                sLoginName = sLoginName ?? "";
                var value = await Task.Run(() => _repositoryPermission.GetUserList(sLoginName, bActiveUser, base.oUser.iUserID, searchCondition, TanenetInfo));
                result.data = value;
            }
            catch (Exception ex)
            {
                result.message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }

        [ProducesResponseType(typeof(BEUserInfo), StatusCodes.Status200OK)]
        [HttpGet]
        [Route("GetClientUserDetails")]
        public async Task<MessageResponse<BEUserInfo>> GetClientUserDetails(int userId)
        {
            var result = new MessageResponse<BEUserInfo>();
            try
            {

                var value = await Task.Run(() => _repositoryPermission.GetClientUserDetails(userId, TanenetInfo));
                result.data = value;
            }
            catch (Exception ex)
            {
                result.message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }

        [ProducesResponseType(typeof(List<UserRequestStatus>), StatusCodes.Status200OK)]
        [HttpGet]
        [Route("GetUserRequestStatus")]
        public async Task<MessageResponse<List<UserRequestStatus>>> GetUserRequestStatus([FromQuery] DateTime dtFromDate, [FromQuery] DateTime dtToDate)
        {
            var result = new MessageResponse<List<UserRequestStatus>>();
            try
            {

                var value = await Task.Run(() => _repositoryPermission.GetUserRequestStatus(base.oUser.iUserID, dtFromDate, dtToDate, TanenetInfo));
                var response = value.Tables[0].AsEnumerable().Select(x => new UserRequestStatus()
                {
                    Approver = x["Approver"].ToString() ?? "",
                    Cancelable = Convert.ToInt16(x["Cancelable"]),
                    RequestDesc = x["RequestDesc"].ToString() ?? "",
                    RequestedBy = x["RequestedBy"].ToString() ?? "",
                    RequestedFor = x["RequestedFor"].ToString() ?? "",
                    RequestedOn = Convert.ToDateTime(x["RequestedOn"]),
                    RequestId = Convert.ToInt32(x["RequestId"].ToString()),
                    RequestStatus = x["RequestStatus"].ToString() ?? ""
                }).ToList();
                result.totalCount = response.Count;
                result.data = response;
            }
            catch (Exception ex)
            {
                result.message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }

        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [HttpGet]
        [Route("CancelAccessRequest")]
        public async Task<MessageResponse<string>> CancelAccessRequest([FromQuery] int iRequestID, [FromQuery] int iFormID)
        {
            var result = new MessageResponse<string>();
            try
            {
                await Task.Run(() => _repositoryPermission.CancelAccessRequest(iRequestID, base.oUser.iUserID, iFormID, TanenetInfo));
                result.data = "Ok";
            }
            catch (Exception ex)
            {
                result.message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }

        [ProducesResponseType(typeof(List<AccessRequestApprovalList>), StatusCodes.Status200OK)]
        [HttpGet]
        [Route("GetUserApprovalList")]
        public async Task<MessageResponse<List<AccessRequestApprovalList>>> GetUserApprovalList()
        {
            var result = new MessageResponse<List<AccessRequestApprovalList>>();
            try
            {
                var results = await Task.Run(() => _repositoryPermission.GetUserApprovalList(base.oUser.iUserID, TanenetInfo));
                var response = results.Tables[0].AsEnumerable().Select(x => new AccessRequestApprovalList()
                {
                    ApprovalLevel = Convert.ToInt16(x["ApprovalLevel"]),
                    RquestedBy = x["RquestedBy"].ToString() ?? "",
                    RequestType = Convert.ToInt16(x["RequestType"]),
                    RequestTypeID = Convert.ToInt16(x["RequestTypeID"]),
                    RequestDesc = x["RequestDesc"].ToString() ?? "",
                    RequestedOn = Convert.ToDateTime(x["RequestedOn"]),
                    RequestId = Convert.ToInt32(x["RequestId"]),
                }).ToList();
                result.data = response;
                result.totalCount = response.Count;
            }
            catch (Exception ex)
            {
                result.message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }

        [ProducesResponseType(typeof(List<UserRequestType>), StatusCodes.Status200OK)]
        [HttpGet]
        [Route("GetUserRequestType")]
        public async Task<MessageResponse<List<UserRequestType>>> GetUserRequestType([FromQuery] int userId, [FromQuery] int requestId, [FromQuery] int mappedOn)
        {
            var result = new MessageResponse<List<UserRequestType>>();
            try
            {
                BEUserMapping oUserMapping = new BEUserMapping()
                {
                    oUser = new BEUserInfo()
                    {
                        iUserID = userId
                    },
                    MappedOn = mappedOn
                };


                var results = await Task.Run(() => _repositoryPermission.GetUserRequestType(oUserMapping, requestId, TanenetInfo));
                var response = results.Tables[0].AsEnumerable().Select(o => new UserRequestType()
                {
                    Id = o.Field<int>("Id"),
                    RequestName = o.Field<string>("RequestName"),
                    RequestType = o.Field<int>("RequestType"),
                    RequestTypeDetails = o.Field<string>("RequestTypeDetails"),
                    RequestTypeId = o.Field<int>("RequestTypeId")
                }).ToList();
                result.data = response;
                result.totalCount = response.Count;
            }
            catch (Exception ex)
            {
                result.message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }

        [ProducesResponseType(typeof(List<BEClientInfo>), StatusCodes.Status200OK)]
        [HttpGet]
        [Route("GetClientAccessList")]
        public async Task<MessageResponse<List<BEClientInfo>>> GetClientAccessList([FromQuery] int iAgentID, [FromQuery] bool bActiveClient)
        {
            var result = new MessageResponse<List<BEClientInfo>>();
            try
            {

                var results = await Task.Run(() => _repositoryClient.GetClientAccessList(base.oUser.iUserID, iAgentID, bActiveClient, TanenetInfo));
                result.totalCount = results.Count;
                result.data = results;
            }
            catch (Exception ex)
            {
                result.message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }

        [ProducesResponseType(typeof(List<BECampaignInfo>), StatusCodes.Status200OK)]
        [HttpGet]
        [Route("GetCampaignAccessList")]
        public async Task<MessageResponse<List<BECampaignInfo>>> GetCampaignAccessList(int iAgentID, bool bActiveCampaign)
        {
            var result = new MessageResponse<List<BECampaignInfo>>();
            try
            {

                var results = await Task.Run(() => _repositoryCampaign.GetCampaignAccessList(base.oUser.iUserID, iAgentID, bActiveCampaign, TanenetInfo));
                result.totalCount = results.Count;
                result.data = results;
            }
            catch (Exception ex)
            {
                result.message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }

        [HttpGet]
        [Route("GetClientWiseProcessList")]
        public ActionResult<MessageResponse<List<BEProcessInfo>>> GetClientWiseProcessList(int iLoggedinUserID, int ClientID)
        {
            var result = new MessageResponse<List<BEProcessInfo>>();
            try
            {

                var results = _repositoryProcess.GetClientWiseProcessList(iLoggedinUserID, ClientID, TanenetInfo);
                result.data = results;
            }
            catch (Exception ex)
            {
                result.message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }

        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [HttpGet]
        [Route("GetProcessAccessList")]
        public async Task<MessageResponse<List<BEProcessInfo>>> GetProcessAccessList([FromQuery] int iAgentID, [FromQuery] bool bActiveProcess)
        {
            var result = new MessageResponse<List<BEProcessInfo>>();
            try
            {

                var results = await Task.Run(() => _repositoryProcess.GetProcessAccessList(base.oUser.iUserID, iAgentID, bActiveProcess, TanenetInfo));
                result.totalCount = results.Count;
                result.data = results;
            }
            catch (Exception ex)
            {
                result.message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }


        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [HttpGet]
        [Route("GetUserDetails")]
        public async Task<MessageResponse<string>> GetUserDetails(int iUserID)
        {
            var result = new MessageResponse<string>();
            try
            {

                var results = await Task.Run(() => _repositoryUserAccess.GetUserDetails(iUserID, TanenetInfo));
                result.data = JsonConvert.SerializeObject(results);
            }
            catch (Exception ex)
            {
                result.message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }

        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [HttpPut]
        [Route("CancelRequestInBetween")]
        public async Task<MessageResponse<string>> CancelRequestInBetween([FromQuery] int iRequestID, [FromQuery] int iFormID)
        {
            var result = new MessageResponse<string>();
            try
            {
                await Task.Run(() => _repositoryPermission.CancelRequestInBetween(iRequestID, base.oUser.iUserID, iFormID, TanenetInfo));
                result.data = "display_RequestCanceledSuccessfully";
            }
            catch (Exception ex)
            {
                result.message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }

        [ProducesResponseType(typeof(List<RoleInfoModel>), StatusCodes.Status200OK)]
        [HttpPost]
        [Route("InsertClientUserRecord")]
        public async Task<MessageResponse<string>> InsertClientUserRecord([FromBody] ClientUserInfoModel oUser, int iFormID)
        {
            var result = new MessageResponse<string>();
            try
            {

                await Task.Run(() => _repositoryPermission.InsertClientUserRecord(CatchRecordUser(oUser), iFormID, TanenetInfo));
                result.data = "OK";
            }
            catch (Exception ex)
            {
                result.message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }

        private BEUserInfo CatchRecordUser(ClientUserInfoModel oModel)
        {
            BEUserInfo oUser = new BEUserInfo()
            {
                iEmployeeID = oModel.EmployeeID,
                bLanID = oModel.LanID,
                bClientUser = oModel.ClientUser,
                sFirstName = oModel.FirstName,
                sMiddleName = oModel.MiddleName,
                sLastName = oModel.LastName,
                sEmail = oModel.Email,
                sLoginName = oModel.LoginName,
                bDisabled = oModel.Disabled ?? false,
                bIsBot = oModel.bIsBot,
                iCreatedBy = base.oUser.iCreatedBy,
                iClientID = oModel.ClientID,
                sProcess = oModel.Process,
                oRoles = new List<BERoleInfo>()
                {
                  new BERoleInfo()
                  {
                      iRoleID=oModel.RoleID
                  }
                },
                iRoleApprover = oModel.RoleApprover,
                dDOJ = oModel.DOJ,
                iFacilityId = oModel.FacilityId,
                iSupervisorID = oModel.SupervisorID,
                iLOBID = oModel.LOBID,
                iSBUID = oModel.SBUID,
                iJobID = oModel.JobID,
                iUserID = oModel.UserID

            };// oModel.UserInfo;
            oUser.bClientUser = oModel.ClientUser;
            oUser.iCreatedBy = base.oUser.iUserID;
            oUser.sDeletedProcess = oModel.DeletedProcess;
            oUser.sProcess = oModel.Process;
            oUser.dDOJ = oModel.DOJ;
            return oUser;
        }

        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [HttpPut]
        [Route("UpdateClientUserRecord")]
        public async Task<MessageResponse<string>> UpdateClientUserRecord([FromBody] ClientUserInfoModel oUser, int iFormID)
        {
            var result = new MessageResponse<string>();
            try
            {

                await Task.Run(() => _repositoryPermission.UpdateClientUserRecord(CatchRecordUser(oUser), iFormID, TanenetInfo));
                result.data = "OK";
            }
            catch (Exception ex)
            {
                result.message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }
        [HttpPut]

        [Route("UpdateUserDetails")]
        public ActionResult<MessageResponse<string>> UpdateUserDetails(BEUserInfo oUser, int iFormID)
        {
            var result = new MessageResponse<string>();
            try
            {

                _repositoryUserAccess.UpdateUserDetails(oUser, iFormID, TanenetInfo);

            }
            catch (Exception ex)
            {
                result.message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }

        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [HttpPut]
        [Route("InsertUserMappingForApproval")]
        public async Task<MessageResponse<int>> InsertUserMappingForApproval([FromBody] InsertUserMappingForApprovalModel oUserMapping, [FromQuery] int iFormID)
        {
            var result = new MessageResponse<int>();
            try
            {

                var response = await Task.Run(() => _repositoryPermission.InsertUserMappingForApproval(CatchRecord(oUserMapping), (oUserMapping.DeletedNodes ?? ""), iFormID, TanenetInfo));
                result.data = response;
            }
            catch (Exception ex)
            {
                result.message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }

        private BEUserMapping CatchRecord(InsertUserMappingForApprovalModel model)
        {
            BEUserMapping oUserMapping = new();
            oUserMapping.oRole = new BERoleInfo()
            {
                iRoleID = model.RoleInfo.RoleID
            };
            oUserMapping.oUser = new BEUserInfo()
            {
                iUserID = model.UserInfo.UserID
            };
            oUserMapping.iCreatedBy = base.oUser.iUserID;
            oUserMapping.bDisabled = model.Disabled;
            oUserMapping.MappedOn = model.MappedOn;
            oUserMapping.dtEffectiveDate = model.EffectiveDate;
            oUserMapping.oClient = model.ClientInfo.Select(x => new BEClientInfo()
            {
                iClientID = x.ClientID
            }).ToList();
            oUserMapping.oProcess = model.ProcessInfo.Select(x => new BEProcessInfo()
            {
                iProcessID = x.ProcessID
            }).ToList();
            oUserMapping.oCampaign = model.CampaignInfo.Select(x => new BECampaignInfo()
            {
                iCampaignID = x.CampaignID
            }).ToList();
            return oUserMapping;
        }


        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [HttpPut]
        [Route("ApproveAccessRequest")]
        public async Task<MessageResponse<string>> ApproveAccessRequest([FromQuery] int iRequestID, [FromQuery] int iRequestTypeID, [FromQuery] int iRequestType, [FromQuery] int iApprovalLevel, [FromQuery] int iFormID)
        {
            var result = new MessageResponse<string>();
            try
            {
                await Task.Run(() => _repositoryPermission.ApproveAccessRequest(iRequestID, iRequestTypeID, iRequestType, iApprovalLevel, base.oUser.iUserID, iFormID, TanenetInfo));
                result.data = "Approved";
            }
            catch (Exception ex)
            {
                result.message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }

        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [HttpPut]
        [Route("RejectAccessRequest")]
        public async Task<MessageResponse<string>> RejectAccessRequest([FromQuery] int iRequestID, [FromQuery] int iRequestTypeID, [FromQuery] int iRequestType, [FromQuery] int iApprovalLevel, [FromQuery] int iFormID)
        {
            var result = new MessageResponse<string>();
            try
            {
                await Task.Run(() => _repositoryPermission.RejectAccessRequest(iRequestID, iRequestTypeID, iRequestType, iApprovalLevel, base.oUser.iUserID, iFormID, TanenetInfo));
                result.data = "Rejected";
            }
            catch (Exception ex)
            {
                result.message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }

        [HttpDelete]
        [Route("DeleteClientUserRecord")]
        public ActionResult<MessageResponse<int>> DeleteClientUserRecord(BEUserInfo oUser, int iFormID)
        {
            var result = new MessageResponse<int>();
            try
            {

                _repositoryPermission.DeleteClientUserRecord(oUser, iFormID, TanenetInfo);
                result.data = 1;
            }
            catch (Exception ex)
            {
                result.message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }


        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [HttpPut]
        [Route("InsertUserMappingApproversAndSendMail")]
        public async Task<MessageResponse<string>> InsertUserMappingApproversAndSendMail([FromBody] UserMappingApproverModel oUser, [FromQuery] int iFormID)
        {
            var result = new MessageResponse<string>();
            try
            {
                await Task.Run(() => _repositoryPermission.InsertUserMappingApprovers(CatchRecord(oUser), base.oUser.iUserID, iFormID, TanenetInfo));
                result.data = "display_RequestRaisedSuccessfully";
            }
            catch (Exception ex)
            {
                result.message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }

        private DataTable CatchRecord(UserMappingApproverModel request)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("RequestId");
            dt.Columns.Add("RequestType");
            dt.Columns.Add("RequestTypeId");
            dt.Columns.Add("Approver1Id");
            dt.Columns.Add("Approver2Id");
            for (int i = 0; i < request.UserMappingApprovers.Count; i++)
            {
                DataRow dr = dt.NewRow();
                dr["RequestId"] = request.UserMappingApprovers[i].RequestId;
                dr["RequestType"] = request.UserMappingApprovers[i].RequestType;
                dr["RequestTypeId"] = request.UserMappingApprovers[i].RequestTypeId;
                dr["Approver1Id"] = request.UserMappingApprovers[i].Approver1Id;
                dr["Approver2Id"] = "";// lstTreeData[i].SelectedApproverL2.iApproverId;
                dt.Rows.Add(dr);
            }
            return dt;
        }

    }
}
