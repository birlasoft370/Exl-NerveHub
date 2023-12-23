using BPA.Security.BusinessEntity.ExtrernalRefre;
using BPA.Security.BusinessEntity.Security;
using BPA.Security.ServiceContract.ExternalRef;
using BPA.Security.ServiceContracts.Security;
using Microsoft.AspNetCore.Mvc;
using MicSer.Security.UserMgt.BaseController;
using MicSer.Security.UserMgt.Helper;
using MicSer.Security.UserMgt.Helper.Filter;
using MicSer.Security.UserMgt.Helper.Shared;
using MicSer.Security.UserMgt.Models;
using MicSer.Security.UserMgt.Models.Request;
using System.Data;
using System.Reflection;
using System.Text;

namespace MicSer.Security.UserMgt.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [JwtAuthentication]
    public class RolesController : BaseController<RolesController>
    {
        private IRolesService _iRolesService;
        private IMasterTableService _masterTableService;
        public RolesController(IRolesService iRolesService, ILogger<RolesController> logger, IWebHostEnvironment env, IConfiguration configuration, IMasterTableService masterTableService) : base(logger, env, configuration)
        {
            _iRolesService = iRolesService;
            _masterTableService = masterTableService;
        }

        [ProducesResponseType(typeof(List<RoleInfoModel>), StatusCodes.Status200OK)]
        [HttpGet]
        [Route("GetRoleList")]
        public async Task<MessageResponse<List<RoleInfoModel>>> GetRoleList(int iRoleID)
        {

            var result = new MessageResponse<List<RoleInfoModel>>();
            try
            {
                var results = await Task.Run(() => _iRolesService.GetRoleList(iRoleID, TanenetInfo));
                var response = results.Select(x => new RoleInfoModel()
                {
                    RoleID = x.iRoleID,
                    RoleName = x.sRoleName,
                    RoleDescription = x.sRoleDescription,
                    IsClientRole = x.bIsClientRole,
                    Disabled = x.bDisabled,
                    LevelID = x.iLevelID,
                    FormActions = x.dtFormData.AsEnumerable().Select(row => new FormAction()
                    {
                        FormName = row["Form Name"].ToString(),
                        FormID = Convert.ToInt64(row["FormID"].ToString()),
                        ModuleName = row["ModuleName"].ToString(),
                        View = row["View"].ToString(),
                        Add = row["Add"].ToString(),
                        Modify = row["Modify"].ToString(),
                        Delete = row["Delete"].ToString(),
                        Approve = row["Approve"].ToString(),
                        ChildID = row["ChildID"].ToString(),
                        Description = row["Description"].ToString()
                    }).ToList()

                }).ToList();
                result.data = response;
                result.totalCount = response.Count;
                return result;

            }
            catch (Exception ex)
            {
                result.message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }

        [ProducesResponseType(typeof(List<RoleInfoModel>), StatusCodes.Status200OK)]
        [HttpGet]
        [Route("GetRoleListByName")]
        public ActionResult<MessageResponse<List<BERoleInfo>>> GetRoleListByName([FromQuery] string? iRoleName, [FromQuery] bool activeRoles)
        {

            var result = new MessageResponse<List<BERoleInfo>>();
            try
            {
                iRoleName = iRoleName ?? "";
                var results = _iRolesService.GetRoleList(iRoleName, activeRoles, TanenetInfo);
                result.totalCount = results.Count;
                result.data = results;
                return result;

            }
            catch (Exception ex)
            {
                result.message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }

        [ProducesResponseType(typeof(List<RoleApprovalRequestModel>), StatusCodes.Status200OK)]
        [HttpGet]
        [Route("GetRoleApprovalList")]
        public async Task<MessageResponse<List<RoleApprovalRequestModel>>> GetRoleApprovalList(int iUserID)
        {

            var result = new MessageResponse<List<RoleApprovalRequestModel>>();
            try
            {
                List<RoleApprovalRequestModel> listdata = new();
                var data = await Task.Run(() => _iRolesService.GetRoleApprovalList(iUserID, TanenetInfo));
                listdata = data.Tables[0].AsEnumerable().Select(item => new RoleApprovalRequestModel()
                {
                    Approver = Convert.ToString(item["Approver"]) ?? "",
                    RequestStatus = Convert.ToString(item["RequestStatus"]) ?? "",
                    RequestedBy = Convert.ToString(item["RequestedBy"]) ?? "",
                    RequestId = Convert.ToInt32(item["RequestId"]),
                    Cancelable = Convert.ToString(item["Cancelable"]) ?? "",
                    RequestDesc = Convert.ToString(item["RequestDesc"]) ?? "",
                    RequestedOn = Convert.ToDateTime(item["RequestedOn"])
                }).ToList();

                result.data = listdata;
                return result;
            }
            catch (Exception ex)
            {
                result.message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }

        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [HttpGet]
        [Route("ApproveRoleRequest")]
        public async Task<MessageResponse<string>> ApproveRoleRequest(int iRequestID, int iFormID)
        {

            var result = new MessageResponse<string>();
            try
            {
                await Task.Run(() => _iRolesService.ApproveRoleRequest(iRequestID, base.oUser.iUserID, iFormID, TanenetInfo));
                result.data = "Ok";
                return result;

            }
            catch (Exception ex)
            {
                result.message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }

        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [HttpGet]
        [Route("RejectRoleRequest")]
        public async Task<MessageResponse<string>> RejectRoleRequest(int iRequestID, int iFormID)
        {

            var result = new MessageResponse<string>();
            try
            {
                await Task.Run(() => _iRolesService.RejectRoleRequest(iRequestID, base.oUser.iUserID, iFormID, TanenetInfo));
                result.data = "Ok";
                return result;

            }
            catch (Exception ex)
            {
                result.message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }

        [ProducesResponseType(typeof(List<RoleApprovalRequestModel>), StatusCodes.Status200OK)]
        [HttpGet]
        [Route("GetRoleRequestStatus")]
        public async Task<MessageResponse<List<RoleApprovalRequestModel>>> GetRoleRequestStatus(DateTime dtFromDate, DateTime dtToDate)
        {

            var result = new MessageResponse<List<RoleApprovalRequestModel>>();
            try
            {
                List<RoleApprovalRequestModel> listdata = new();
                var data = await Task.Run(() => _iRolesService.GetRoleRequestStatus(base.oUser.iUserID, dtFromDate, dtToDate, TanenetInfo));

                listdata = data.Tables[0].AsEnumerable().Select(item => new RoleApprovalRequestModel()
                {
                    Approver = Convert.ToString(item["Approver"]) ?? "",
                    RequestStatus = Convert.ToString(item["RequestStatus"]) ?? "",
                    RequestedBy = Convert.ToString(item["RequestedBy"]) ?? "",
                    RequestId = Convert.ToInt32(item["RequestId"]),
                    Cancelable = Convert.ToString(item["Cancelable"]) ?? "",
                    RequestDesc = Convert.ToString(item["RequestDesc"]) ?? "",
                    RequestedOn = Convert.ToDateTime(item["RequestedOn"])
                }).ToList();
                result.data = listdata;
                return result;

            }
            catch (Exception ex)
            {
                result.message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }

        [ProducesResponseType(typeof(List<BEFormActionInfo>), StatusCodes.Status200OK)]
        [HttpGet]
        [Route("GetFormAction")]
        public ActionResult<MessageResponse<List<BEFormActionInfo>>> GetFormAction(int roleId)
        {
            var result = new MessageResponse<List<BEFormActionInfo>>();
            try
            {
                List<BEFormActionInfo> approvalList = new List<BEFormActionInfo>();
                var data = _iRolesService.GetFormAction(roleId, TanenetInfo);
                foreach (DataRow item in data.Tables[0].Rows)
                {
                    BEFormActionInfo approval = new BEFormActionInfo();
                    approval.FormID = Convert.ToInt32(item["FormID"]);
                    approval.ModuleName = item["ModuleName"].ToString();
                    approval.Description = item["Description"].ToString();
                    approval.FormName = item["Form Name"].ToString();
                    approval.Approve = item["Approve"].ToString();
                    approval.Modify = item["Modify"].ToString();
                    approval.Delete = item["Delete"].ToString();
                    approval.ChildID = item["ChildID"].ToString();
                    approval.Add = item["Add"].ToString();
                    approval.View = item["View"].ToString();
                    approvalList.Add(approval);
                }
                result.data = approvalList.ToList();

            }
            catch (Exception ex)
            {
                result.message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }

        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [HttpGet]
        [Route("CancelRoleRequest")]
        public async Task<MessageResponse<string>> CancelRoleRequest(int iRequestID, int iFormID)
        {

            var result = new MessageResponse<string>();
            try
            {
                await Task.Run(() => _iRolesService.CancelRoleRequest(iRequestID, base.oUser.iUserID, iFormID, TanenetInfo));
                result.data = "Ok";
                return result;

            }
            catch (Exception ex)
            {
                result.message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }

        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [HttpPost]
        [Route("InsertData")]
        public async Task<MessageResponse<string>> InsertData([FromBody] RoleInfoModel oRole, [FromQuery] int iApproverId, [FromQuery] int iFormID)
        {
            var result = new MessageResponse<string>();
            StringBuilder mailContent = new();
            mailContent.Append(oRole.MailBodyContent);

            try
            {
                await Task.Run(() => _iRolesService.InsertData(CatchRecord(oRole), iApproverId, iFormID, mailContent, TanenetInfo));
                result.data = "Ok";
            }
            catch (Exception ex)
            {
                result.message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }

        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [HttpPut]
        [Route("UpdateData")]
        public async Task<MessageResponse<string>> UpdateData([FromBody] RoleInfoModel oRole, [FromQuery] int iApproverId, [FromQuery] int iFormID)
        {
            StringBuilder sbMail = new StringBuilder();
            sbMail.Append(oRole.MailBodyContent);
            var result = new MessageResponse<string>();
            try
            {

                await Task.Run(() => _iRolesService.UpdateData(CatchRecord(oRole), iApproverId, iFormID, sbMail, TanenetInfo));
                result.data = "Ok";
            }
            catch (Exception ex)
            {
                result.message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }

        [HttpDelete]
        [Route("DeleteData")]
        public ActionResult<MessageResponse<int>> DeleteData(BERoleInfo oRole, int iApproverId, int iFormID, string sbMailBody)
        {
            StringBuilder sbMail = new StringBuilder();
            sbMail.Append(sbMailBody);
            var result = new MessageResponse<int>();
            try
            {

                _iRolesService.DeleteData(oRole, iApproverId, iFormID, sbMail, TanenetInfo);
                result.data = 1;
            }
            catch (Exception ex)
            {
                result.message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }

        [HttpGet]
        [Route("FillRoleLevel")]
        public ActionResult<MessageResponse<IList<BEMasterTable>>> FillRoleLevel()
        {

            var result = new MessageResponse<IList<BEMasterTable>>();
            try
            {
                var results = _masterTableService.FillRoleLevel(TanenetInfo);
                result.data = results;
                return result;

            }
            catch (Exception ex)
            {
                result.message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }

        private BERoleInfo CatchRecord(RoleInfoModel roleInfo)
        {
            var datatable = ListtoDataTableConverter.ToDataTable(roleInfo.FormActions);
            var result = new BERoleInfo()
            {
                iRoleID = roleInfo.RoleID,
                sRoleName = roleInfo.RoleName ?? "",
                sRoleDescription = roleInfo.RoleDescription ?? "",
                bDisabled = roleInfo.Disabled,
                bIsClientRole = roleInfo.IsClientRole,
                iLevelID = roleInfo.LevelID,
                iCreatedBy = base.oUser.iUserID,
                iModifiedBy = base.oUser.iUserID,
                dtFormData = datatable// JsonConvert.DeserializeObject<DataTable>(roleInfo.StrdtFormData ?? "") ?? new DataTable()               
            };
            return result;
        }
    }
}
