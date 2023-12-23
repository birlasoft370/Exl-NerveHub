using BPA.Security.BusinessEntity.ExtrernalRefre;
using BPA.Security.BusinessEntity.Security;
using BPA.Security.ServiceContracts.Security;
using Microsoft.AspNetCore.Mvc;
using MicSer.Security.UserMgt.BaseController;
using MicSer.Security.UserMgt.Helper;
using MicSer.Security.UserMgt.Helper.Filter;
using MicSer.Security.UserMgt.Models;
using System.Data;
using System.Reflection;

namespace MicSer.Security.UserMgt.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [JwtAuthentication]
    public class ERPJobRoleMapController : BaseController<ERPJobRoleMapController>
    {
        private readonly IERPJobRoleMapService _ERPJobRoleMapService;
        private readonly IPermissionService _repositoryPermission;
        public ERPJobRoleMapController(IERPJobRoleMapService ERPJobRoleMapService, IPermissionService repositoryPermission, ILogger<ERPJobRoleMapController> logger, IWebHostEnvironment env, IConfiguration configuration) : base(logger, env, configuration)

        {
            this._ERPJobRoleMapService = ERPJobRoleMapService;
            this._repositoryPermission = repositoryPermission;
            //this._repositoryAuthenticate = repositoryAuthenticate;
        }
        [HttpGet]
        [Route("GetJobRoleMap")]
        public ActionResult<MessageResponse<List<BEErpJobRoleMap>>> GetJobRoleMap(int filterId)
        {
            var result = new MessageResponse<List<BEErpJobRoleMap>>();
            try
            {

                var results = _ERPJobRoleMapService.GetJobRoleMap(filterId, TanenetInfo);
                result.data = results;
            }
            catch (Exception ex)
            {
                result.message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }
        [HttpGet]
        [Route("GetJobRoleMapByName")]
        public async Task<ActionResult<MessageResponse<List<BEErpJobRoleMap>>>> GetJobRoleMapByName([FromQuery] string? jobDesc)
        {
            var result = new MessageResponse<List<BEErpJobRoleMap>>();
            try
            {
                jobDesc = jobDesc ?? "";
                var results = _ERPJobRoleMapService.GetJobRoleMap(jobDesc, TanenetInfo);
                result.data = results;
            }
            catch (Exception ex)
            {
                result.message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }
        [HttpPost]
        [Route("InsertData")]
        public ActionResult<MessageResponse<string>> InsertData([FromBody] ErpJobRoleMap oJobRole, int FormID)
        {
            var result = new MessageResponse<string>();
            try
            {

                _ERPJobRoleMapService.InsertData(CatchRecordSave(oJobRole), FormID, TanenetInfo);

            }
            catch (Exception ex)
            {
                result.message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }
        [HttpGet]
        [Route("GetJob")]
        public ActionResult<MessageResponse<List<BEJobCodeInfo>>> GetJob()
        {
            var result = new MessageResponse<List<BEJobCodeInfo>>();
            try
            {

                var data = _ERPJobRoleMapService.GetJob(TanenetInfo);
                result.data = data;

            }
            catch (Exception ex)
            {
                result.message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }
        [HttpGet]
        [Route("GetRoleList")]
        public ActionResult<MessageResponse<List<BERoleInfo>>> GetRoleList()
        {
            var result = new MessageResponse<List<BERoleInfo>>();
            try
            {

                var data = _ERPJobRoleMapService.GetRoleList(TanenetInfo);
                result.data = data;

            }
            catch (Exception ex)
            {
                result.message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }
        [HttpGet]

        [Route("GetUserRoleApproverList")]
        public async Task<MessageResponse<List<BEApproval>>> GetUserRoleApproverList(int roleId)
        {
            var result = new MessageResponse<List<BEApproval>>();
            try
            {
                List<BEApproval> approvalList = new List<BEApproval>();
                var data = await Task.Run(() => _repositoryPermission.GetUserRoleApproverList(roleId, TanenetInfo));

                foreach (DataRow item in data.Tables[0].Rows)
                {
                    BEApproval approval = new BEApproval();
                    approval.UserID = Convert.ToInt32(item[0]);
                    approval.UserName = item[1].ToString();
                    approvalList.Add(approval);
                }
                result.data = approvalList.OrderBy(x => x.UserName).ToList();

            }
            catch (Exception ex)
            {
                result.message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }

        [HttpGet]
        [Route("GetERPJobRoleApprovalList")]
        public ActionResult<MessageResponse<List<BEErpJobRoleMap>>> GetERPJobRoleApprovalList(int userId)
        {
            var result = new MessageResponse<List<BEErpJobRoleMap>>();
            try
            {

                var data = _ERPJobRoleMapService.GetERPJobRoleApprovalList(userId, TanenetInfo);
                result.data = data;

            }
            catch (Exception ex)
            {
                result.message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }
        [HttpDelete]
        [Route("CancelERPJobRoleRequest")]
        public ActionResult<MessageResponse<string>> CancelERPJobRoleRequest(int iRequestID, int iUserID, int iFormID)
        {
            var result = new MessageResponse<string>();
            try
            {

                _ERPJobRoleMapService.CancelERPJobRoleRequest(iRequestID, iUserID, iFormID, TanenetInfo);

            }
            catch (Exception ex)
            {
                result.message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }
        [HttpDelete]
        [Route("RejectERPJobRoleRequest")]
        public ActionResult<MessageResponse<string>> RejectERPJobRoleRequest(int iRequestID, int iUserID, int iFormID)
        {
            var result = new MessageResponse<string>();
            try
            {

                _ERPJobRoleMapService.RejectERPJobRoleRequest(iRequestID, iUserID, iFormID, TanenetInfo);

            }
            catch (Exception ex)
            {
                result.message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }
        [HttpPut]
        [Route("ApproveERPJobRoleRequest")]
        public ActionResult<MessageResponse<string>> ApproveERPJobRoleRequest(int iRequestID, int iUserID, int iFormID)
        {
            var result = new MessageResponse<string>();
            try
            {

                _ERPJobRoleMapService.ApproveERPJobRoleRequest(iRequestID, iUserID, iFormID, TanenetInfo);

            }
            catch (Exception ex)
            {
                result.message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }
        private BEErpJobRoleMap CatchRecordSave(ErpJobRoleMap objERPJobRoleMapping)
        {

            BEErpJobRoleMap objRoleJob = new BEErpJobRoleMap();

            objRoleJob.oRole = new BERoleInfo { iRoleID = objERPJobRoleMapping.RoleId, sRoleName = objERPJobRoleMapping.RoleName };
            objRoleJob.oJob = new BEJobCodeInfo { iJOBID = objERPJobRoleMapping.jobId, sJobDesc = objERPJobRoleMapping.jobDescription };
            objRoleJob.iMappedOn = objERPJobRoleMapping.iMappedOn;
            objRoleJob.bDisabled = objERPJobRoleMapping.bDisabled;
            objRoleJob.bDefaultRole = objERPJobRoleMapping.bDefaultRole;
            objRoleJob.iCreatedBy = objERPJobRoleMapping.CreatedBy;
            objRoleJob.iApprover = objERPJobRoleMapping.ApproverId;
            objRoleJob.iMode = objERPJobRoleMapping.iMode;


            return objRoleJob;
        }

    }
}
