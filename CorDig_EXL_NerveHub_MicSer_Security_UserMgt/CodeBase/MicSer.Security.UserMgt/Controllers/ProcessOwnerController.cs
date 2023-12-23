using BPA.Security.BusinessEntity.ExtrernalRefre;
using BPA.Security.ServiceContract.ExternalRef;
using BPA.Security.ServiceContracts.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MicSer.Security.UserMgt.BaseController;
using MicSer.Security.UserMgt.Helper.Filter;
using MicSer.Security.UserMgt.Helper;
using MicSer.Security.UserMgt.Models;
using System.Data;
using System.Reflection;
using BPA.Security.BusinessEntity.Security;
using MicSer.Security.UserMgt.Models.Response;

namespace MicSer.Security.UserMgt.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProcessOwnerController : BaseController<ProcessOwnerController>
    {
        #region Private Variable

        private readonly IProcessService _repositoryProcess;
        private readonly IPermissionService _repsitoryPermission;

        #endregion

        #region Constructors
        public ProcessOwnerController(IProcessService repositoryProcess, IPermissionService repsitoryPermission, ILogger<ProcessOwnerController> logger, IWebHostEnvironment env, IConfiguration configuration) : base(logger, env, configuration)
        {
            this._repositoryProcess = repositoryProcess;
            this._repsitoryPermission = repsitoryPermission;
        }
        #endregion

        [HttpPost]
        [JwtAuthentication]
        [Route("CheckProcessOwnerApproverLevel")]
        public ActionResult<MessageResponse<string>> CheckProcessOwnerApproverLevel(ProcessOwnerModel oProcess)
        {
            var result = new MessageResponse<string>();
            try
            {
                BEProcessInfo oProcessInfo = CatchRecord(oProcess);
                var value = _repositoryProcess.CheckProcessOwnerApproverLevel(oProcessInfo, TanenetInfo);
                result.data = value;
            }
            catch (Exception ex)
            {
                result.message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }
        [HttpPost]
        [JwtAuthentication]
        [Route("SendApproveProcessReqest")]
        public ActionResult<MessageResponse<int>> SendApproveProcessReqest(ProcessOwnerModel oProcess, int FormID, int ProcRequest_Id, int Action)
        {
            var result = new MessageResponse<int>();
            try
            {
                BEProcessInfo oProcessInfo = CatchRecord(oProcess);
                var results= _repositoryProcess.SendApproveProcessReqest(oProcessInfo, FormID, ProcRequest_Id, Action, TanenetInfo);
                result.data = results;
            }
            catch (Exception ex)
            {
                result.message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }
        [HttpGet]
        [JwtAuthentication]
        [Route("ExistingUserRequest")]
        public ActionResult<MessageResponse<string>> ExistingUserRequest(int ProcessId, string ProcessOwner)
        {
            var result = new MessageResponse<string>();
            try
            {
               
                var results = _repositoryProcess.ExistingUserRequest(ProcessId, ProcessOwner, TanenetInfo);
                result.data = results;
            }
            catch (Exception ex)
            {
                result.message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }
        [HttpGet]
        [JwtAuthentication]
        [Route("GetClientWiseProcessList")]
        public ActionResult<MessageResponse<List<BEProcessInfo>>> GetClientWiseProcessList(int userId,int ClientId)
        {
            var result = new MessageResponse<List<BEProcessInfo>>();
            try
            {
               
                var results = _repositoryProcess.GetClientWiseProcessList(userId,ClientId, TanenetInfo);
                result.data = results;
            }
            catch (Exception ex)
            {
                result.message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }
        [HttpGet]
        [JwtAuthentication]
        [Route("GetUserProcessOwner")]
        public ActionResult<MessageResponse<List<BEApproval>>> GetUserProcessOwner(int processId)
        {
            var result = new MessageResponse<List<BEApproval>>();
            try
            {
                List<BEApproval> approvalList = new List<BEApproval>();
                var results = _repositoryProcess.GetUserProcessOwner(processId, TanenetInfo);
                foreach (DataRow item in results.Tables[0].Rows)
                {
                    BEApproval approval = new BEApproval();
                    approval.UserID = Convert.ToInt32(item[0]);
                    approval.UserName = item[1].ToString();
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
        [HttpGet]
        [JwtAuthentication]
        [Route("GetUserProcessOwnerUserList")]
        public ActionResult<MessageResponse<ProcessOwnerApprovalUser>> GetUserProcessOwnerUserList(int processId)
        {
            var result = new MessageResponse<ProcessOwnerApprovalUser>();
            try
            {
                ProcessOwnerApprovalUser procssesOwnerList = new();
                List<BEApproval> approvalList = new List<BEApproval>();
                List<int> userIdList = new();
                var results = _repositoryProcess.GetUserProcessOwner(processId, TanenetInfo);
                foreach (DataRow item in results.Tables[0].Rows)
                {
                    BEApproval approval = new BEApproval();
                    approval.UserID = Convert.ToInt32(item[0]);
                    approval.UserName = item[1].ToString();
                    approvalList.Add(approval);
                }
                procssesOwnerList.UserList= approvalList;
                foreach (DataRow item in results.Tables[1].Rows)
                {
                    userIdList.Add(Convert.ToInt32(item[0]));
                }
                if (userIdList.ToList() != null)
                {
                    procssesOwnerList.UserIdList = userIdList.ToList();
                }
                result.data= procssesOwnerList;

            }
            catch (Exception ex)
            {
                result.message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }
        [HttpGet]
        [JwtAuthentication]
        [Route("GetUserRoleApproverList")]
        public ActionResult<MessageResponse<DataSet>> GetUserRoleApproverList(int RoleId)
        {
            var result = new MessageResponse<DataSet>();
            try
            {
               
                var results = _repsitoryPermission.GetUserRoleApproverList(RoleId, TanenetInfo);
                result.data = results;
            }
            catch (Exception ex)
            {
                result.message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }
        [HttpGet]
        [JwtAuthentication]
        [Route("GetPandingApproval")]
        public ActionResult<MessageResponse<List<ProcessOwnerApproval>>> GetPandingApproval(int iUserId, string sFromDate, string sToDate)
        {
            var response = new MessageResponse<List<ProcessOwnerApproval>>();
            try
            {
             
                var results = _repositoryProcess.GetPandingApproval(iUserId, sFromDate, sToDate, TanenetInfo);
                var report = results.AsEnumerable().Select(x => new ProcessOwnerApproval()
                {
                    RequestId = Convert.ToInt32(x["RequestId"]),
                    ClientName = x["ClientName"].ToString() ?? "",
                    ProcessName = x["ProcessName"].ToString() ?? "",
                    Creater = x["Creater"].ToString() ?? "",
                    Approver = x["Approver"].ToString() ?? "",
                    CreateDate = x["CreateDate"].ToString() ?? "",
                    ForUser = x["ForUser"].ToString() ?? "",
                    ForApprover = x["ForApprover"].ToString() ?? "",
                    ForCancel = x["ForCancel"].ToString() ?? "",
                    TransStatus = x["TransStatus"].ToString() ?? "",
                }).ToList();
                response.totalCount = report.Count;
                response.data = report;

               
            }
            catch (Exception ex)
            {
                response.message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return response;
        }
        private BEProcessInfo CatchRecord(ProcessOwnerModel oProcessOwnerViewModel)
        {
            BEProcessInfo objProcess = new BEProcessInfo();
            objProcess.iClientID = Convert.ToInt32(oProcessOwnerViewModel.ClientName);
            objProcess.iProcessID = Convert.ToInt32(oProcessOwnerViewModel.ProcessName);
            objProcess.sProcessOwner = Convert.ToString(oProcessOwnerViewModel.ProcessOwnerName);
            objProcess.iCreatedBy = Convert.ToInt32(oProcessOwnerViewModel.UserId);
            objProcess.iApprover = Convert.ToInt32(oProcessOwnerViewModel.Approver);
            // objProcess.iApprover = Convert.ToInt32(oProcessOwnerViewModel.Approver);
            return objProcess;
        }

    }
}
