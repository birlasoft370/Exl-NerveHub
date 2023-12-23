using BPA.AppConfig.BusinessEntity.Config;
using BPA.AppConfig.BusinessEntity.Security;
using BPA.AppConfig.ServiceContracts.ServiceContracts.Config;
using BPA.AppConfig.ServiceContracts.ServiceContracts.ExternalRef.WorkAllocation;
using BPA.AppConfig.ServiceContracts.ServiceContracts.Security;
using BPA.AppConfiguration.BaseController;
using BPA.AppConfiguration.Helper;
using BPA.AppConfiguration.Helper.Filter;
using BPA.AppConfiguration.Models;
using BPA.AppConfiguration.Models.Request;
using BPA.AppConfiguration.Models.Response;
using BPA.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Security.Application;
using System.Reflection;

namespace BPA.AppConfiguration.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [JwtAuthentication]
    public class CampaignController : BaseController<CampaignController>
    {
        private readonly ITimeZoneService _repositoryTimeZone;
        private readonly ICampaignService _repositoryCampaign;
        private readonly IAuthorizationUserService _repositoryAuthorization;
        private readonly IWorkObjectService _repositoryWorkObject;
        public CampaignController(ILogger<CampaignController> logger, IWebHostEnvironment env, IConfiguration configuration,
            ITimeZoneService repositoryTimeZone, ICampaignService repositoryCampaign, IAuthorizationUserService repositoryAuthorization, IWorkObjectService repositoryWorkObject) : base(logger, env, configuration)
        {
            _repositoryTimeZone = repositoryTimeZone;
            _repositoryCampaign = repositoryCampaign;
            _repositoryAuthorization = repositoryAuthorization;
            _repositoryWorkObject = repositoryWorkObject;
        }

        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [HttpGet]
        [Route("GetCheckPermission")]
        public async Task<MessageResponse<bool>> GetCheckPermission()
        {
            var result = new MessageResponse<bool>();
            try
            {
                var hasPermission = await Task.Run(() => _repositoryAuthorization.CheckPermission(201, base.oUser.iUserID, AppConfig.BusinessEntity.PermissionSet.APPROVE, base.oTenant));

                result.Data = hasPermission;
            }
            catch (Exception ex)
            {
                result.Message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }

        [ProducesResponseType(typeof(List<BECampaignInfo>), StatusCodes.Status200OK)]
        [HttpGet]
        [Route("GetProcessWiseCampaignList")]
        public async Task<MessageResponse<List<BECampaignInfo>>> GetProcessWiseCampaignList([FromQuery] int processId)
        {
            var result = new MessageResponse<List<BECampaignInfo>>();
            try
            {
                var campaignListbyProcess = await Task.Run(() => _repositoryCampaign.GetProcessWiseCampaignList(33, base.oUser.iUserID, processId, base.oTenant));
                result.Data = campaignListbyProcess;
                result.TotalCount = campaignListbyProcess.Count;
            }
            catch (Exception ex)
            {
                result.Message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }

        [ProducesResponseType(typeof(List<BETimeZoneInfo>), StatusCodes.Status200OK)]
        [HttpGet]
        [Route("GetTimeZoneList")]
        public async Task<MessageResponse<List<BETimeZoneInfo>>> GetTimeZoneList()
        {
            var result = new MessageResponse<List<BETimeZoneInfo>>();
            try
            {
                var timeZoneList = await Task.Run(() => _repositoryTimeZone.GetTimeZoneList(false, base.oTenant));
                result.Data = timeZoneList;
                result.TotalCount = timeZoneList.Count;
            }
            catch (Exception ex)
            {
                result.Message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }

        [ProducesResponseType(typeof(List<BEUserInfo>), StatusCodes.Status200OK)]
        [HttpGet]
        [Route("GetBusinessApproverList")]
        public async Task<MessageResponse<List<BEUserInfo>>> GetBusinessApproverList([FromQuery] int processID)
        {
            var result = new MessageResponse<List<BEUserInfo>>();
            try
            {
                var businessApproverList = await Task.Run(() => _repositoryCampaign.GetUserApproverListByProcess(base.oUser.iUserID, 4, processID, base.oTenant));
                result.Data = businessApproverList;
                result.TotalCount = businessApproverList.Count;
            }
            catch (Exception ex)
            {
                result.Message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }


        [ProducesResponseType(typeof(List<BECampaignInfo>), StatusCodes.Status200OK)]
        [HttpGet]
        [Route("GetCampaignList")]
        public async Task<MessageResponse<List<BECampaignInfo>>> GetCampaignList([FromQuery] int processID, [FromQuery] string? campSearchName, [FromQuery] bool activeCampaign)
        {
            var result = new MessageResponse<List<BECampaignInfo>>();
            try
            {
                var campaignList = await Task.Run(() =>
                _repositoryCampaign.GetCampaignList(base.oUser.iUserID, processID, campSearchName, activeCampaign, base.oTenant));

                result.Data = campaignList;
                result.TotalCount = campaignList.Count;
            }
            catch (Exception ex)
            {
                result.Message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }

        [ProducesResponseType(typeof(CampaignModel), StatusCodes.Status200OK)]
        [HttpGet]
        [Route("GetCampaignById")]
        public async Task<MessageResponse<BECampaignInfo>> GetCampaignById([FromQuery] int campaignID)
        {
            var result = new MessageResponse<BECampaignInfo>();
            try
            {
                var campaignList = await Task.Run(() => _repositoryCampaign.GetCampaignList(campaignID, base.oTenant));

                result.Data = campaignList[0];
                result.TotalCount = campaignList.Count;
            }
            catch (Exception ex)
            {
                result.Message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }

        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [HttpPost]
        [Route("AddCampaign")]
        public async Task<MessageResponse<string>> AddCampaign([FromBody] CampaignModel campaignModel)
        {
            var result = new MessageResponse<string>();
            try
            {
                await Task.Run(() => _repositoryCampaign.InsertData(CatchRecord(campaignModel), 4, base.oTenant));
                result.Data = "display_Save";
            }
            catch (Exception ex)
            {
                result.Message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }


        [ProducesResponseType(typeof(List<CampaignApproval>), StatusCodes.Status200OK)]
        [HttpGet]
        [Route("GetPendingCampaignApprovalList")]
        public async Task<MessageResponse<List<CampaignApproval>>> GetPendingCampaignApprovalList([FromQuery] DateTime dFrom, [FromQuery] DateTime dTo)
        {
            var result = new MessageResponse<List<CampaignApproval>>();
            try
            {
                var pendingCampaignList = await Task.Run(() =>
                _repositoryCampaign.GetPandingCampaignApproval(base.oUser.iUserID, dFrom, dTo, base.oTenant).ConvertToList<CampaignApproval>());

                result.Data = pendingCampaignList;
                result.TotalCount = pendingCampaignList.Count;
            }
            catch (Exception ex)
            {
                result.Message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }

        [ProducesResponseType(typeof(CampaignRequestDetails), StatusCodes.Status200OK)]
        [HttpGet]
        [Route("GetPendingCampaignApprovalById")]
        public async Task<MessageResponse<CampaignRequestDetails>> FetchCampaignRequestDetails([FromQuery] int level, [FromQuery] int approvalId)
        {
            CampaignRequestDetails? model = new CampaignRequestDetails();
            var result = new MessageResponse<CampaignRequestDetails>();
            try
            {
                var dtWorkObjReqDetails = await Task.Run(() =>
                _repositoryCampaign.FetchCampaignRequestDetails(approvalId, base.oTenant));

                if (dtWorkObjReqDetails.Rows.Count > 0)
                {
                    model.IsLevel = level;
                    model.ApprovelId = approvalId;
                    model.Requestor = dtWorkObjReqDetails.Rows[0]["Requestor"].ToString();
                    model.ClientName = dtWorkObjReqDetails.Rows[0]["ClientName"].ToString();
                    model.ProcessName = dtWorkObjReqDetails.Rows[0]["ProcessName"].ToString();
                    model.CampaignName = dtWorkObjReqDetails.Rows[0]["CampaignName"].ToString();
                    model.Location = dtWorkObjReqDetails.Rows[0]["Location"].ToString();
                    model.ShiftWindow = dtWorkObjReqDetails.Rows[0]["ShiftWindow"].ToString();
                    model.Purpose = dtWorkObjReqDetails.Rows[0]["Purpose"].ToString();
                    model.Q1 = int.Parse(dtWorkObjReqDetails.Rows[0]["TargetUsersQ1"].ToString());
                    model.Q2 = int.Parse(dtWorkObjReqDetails.Rows[0]["TargetUsersQ2"].ToString());
                    model.Q3 = int.Parse(dtWorkObjReqDetails.Rows[0]["TargetUsersQ3"].ToString());
                    model.Y1 = int.Parse(dtWorkObjReqDetails.Rows[0]["TargetUsersY1"].ToString());
                    model.Y2 = int.Parse(dtWorkObjReqDetails.Rows[0]["TargetUsersY2"].ToString());
                    model.Y3 = int.Parse(dtWorkObjReqDetails.Rows[0]["TargetUsersY3"].ToString());
                    model.BusinessJustifications = dtWorkObjReqDetails.Rows[0]["BusinessJustification"].ToString();
                    model.KeyBenefits = dtWorkObjReqDetails.Rows[0]["KeyBenfits"].ToString();
                    model.ChangeRequest = dtWorkObjReqDetails.Rows[0]["ChangeRequest"].ToString();
                    model.ChangeRequestStatus = dtWorkObjReqDetails.Rows[0]["ChangeRequestStatus"].ToString();
                }
                result.Data = model;
            }
            catch (Exception ex)
            {
                result.Message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }

        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [HttpPost]
        [Route("AddCampaignRequestChange")]
        public async Task<MessageResponse<string>> AddCampaignRequestChange([FromQuery] int level, [FromQuery] int approvalId, string changeReq)
        {
            var result = new MessageResponse<string>();
            try
            {
                await Task.Run(() => _repositoryCampaign.InsertCampaignRequestChange(approvalId, base.oUser.iUserID, level, changeReq, base.oTenant));
                result.Data = "Save";
            }
            catch (Exception ex)
            {
                result.Message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }

        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [HttpPut]
        [Route("UpdateCampaignRequestChange")]
        public async Task<MessageResponse<string>> UpdateCampaignRequestChange([FromBody] CampaignRequestChange objCampaignInfo)
        {
            var result = new MessageResponse<string>();
            try
            {
                await Task.Run(() => _repositoryCampaign.UpdateCampaignRequestChange(CatchRecordUpdateCampaignRequestChange(objCampaignInfo), base.oTenant));
                result.Data = "Save";
            }
            catch (Exception ex)
            {
                result.Message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }

        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [HttpPut]
        [Route("ApprovalAction")]
        public async Task<MessageResponse<string>> ApprovalAction([FromQuery] int approvalId, [FromQuery] string action)
        {
            var result = new MessageResponse<string>();
            try
            {
                BECampaignInfo objdata = new BECampaignInfo();
                objdata.iCreatedBy = base.oUser.iUserID;
                objdata.iApprovalId = approvalId;
                objdata.sStatus = action;

                await Task.Run(() => _repositoryCampaign.InsertData(objdata, 4, base.oTenant));
                result.Data = action;
            }
            catch (Exception ex)
            {
                result.Message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }

        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [HttpPut]
        [Route("UpdateCampaign")]
        public async Task<MessageResponse<string>> UpdateCampaign([FromBody] CampaignModel campaignModel)
        {
            var result = new MessageResponse<string>();
            try
            {
                await Task.Run(() => _repositoryCampaign.UpdateData(CatchRecord(campaignModel), 4, base.oTenant));
                result.Data = "Campaign data update successfully !.";
            }
            catch (Exception ex)
            {
                result.Message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }

        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [HttpGet]
        [Route("GetCheckUserIsSuperOrFunctionalAdmin")]
        public async Task<MessageResponse<int>> GetCheckUserIsSuperOrFunctionalAdmin()
        {
            var result = new MessageResponse<int>();
            try
            {
                var userIsSuperOrFunctionalAdmin = await Task.Run(() =>
                _repositoryWorkObject.CheckUserIsSuperOrFunctionalAdmin(base.oUser.iUserID, base.oTenant));

                result.Data = userIsSuperOrFunctionalAdmin;
            }
            catch (Exception ex)
            {
                result.Message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }

        [NonAction]
        private BECampaignInfo CatchRecordCampaignRequestChange(CampaignModel oCampaignModel)
        {
            BECampaignInfo oBECampInfo = new BECampaignInfo();
            oBECampInfo.sLocations = Encoder.HtmlEncode(oCampaignModel.Location);
            oBECampInfo.sShiftwindows = oCampaignModel.ShiftWindow;
            oBECampInfo.bPurposesWM = oCampaignModel.PurposesWM;
            oBECampInfo.bPurposeTime = oCampaignModel.PurposeTime;
            oBECampInfo.bPurposeTrans = oCampaignModel.PurposeTrans;
            oBECampInfo.sBusinessJustifications = oCampaignModel.BusinessJustifications;
            oBECampInfo.sTargetq1 = Encoder.HtmlEncode(oCampaignModel.Q1.ToString());
            oBECampInfo.sTargetq2 = Encoder.HtmlEncode(oCampaignModel.Q2.ToString());
            oBECampInfo.sTargetq3 = Encoder.HtmlEncode(oCampaignModel.Q3.ToString());
            oBECampInfo.sTargety1 = Encoder.HtmlEncode(oCampaignModel.Y1.ToString());
            oBECampInfo.sTargety2 = Encoder.HtmlEncode(oCampaignModel.Y2.ToString());
            oBECampInfo.sTargety3 = Encoder.HtmlEncode(oCampaignModel.Y3.ToString());
            oBECampInfo.sKeyBenefits = Encoder.HtmlEncode(oCampaignModel.KeyBenefits);
            oBECampInfo.iBuisnessID = oCampaignModel.ApproverId;
            oBECampInfo.iModeId = 0;
            return oBECampInfo;
        }


        [NonAction]
        private BECampaignInfo CatchRecord(CampaignModel oCampaignModel)
        {

            BECampaignInfo oBECampInfo = new BECampaignInfo();
            //IList<BEParameterInfo> lParameterInfo = new List<BEParameterInfo>();
            IList<BESkillInfo> lSkillInfo = new List<BESkillInfo>();

            oBECampInfo.iCampaignID = oCampaignModel.CampaignID;
            oBECampInfo.sCampaignName = Encoder.HtmlEncode(oCampaignModel.CampaignName);
            oBECampInfo.sCampaignDescription = Encoder.HtmlEncode(oCampaignModel.Description);
            oBECampInfo.sModeIds = Encoder.HtmlEncode(String.Join(",", oCampaignModel.Mode).Trim());
            //if (oCampaignModel.EndDate.ToString() != "") oBECampInfo.dtEndDate = oCampaignModel.EndDate.ToString() == null ? DateTimeTimeZoneConversion.GetCurrentDateTime(false, base.oUser.sUserTimeZone, base.oUser.sServerTimeZone) : Convert.ToDateTime(oCampaignModel.EndDate);
            oBECampInfo.dtEndDate = Convert.ToDateTime(oCampaignModel.EndDate);
            //DateTime.ParseExact(oCampaignViewModel.EndDate.ToString(), "MM/dd/yyyy", CultureInfo.InvariantCulture); ;
            oBECampInfo.iProcessID = oCampaignModel.ProcessID;
            oBECampInfo.iTimeZoneID = oCampaignModel.TimeZone;
            oBECampInfo.iClientID = oCampaignModel.ClientID;
            oBECampInfo.bDisabled = Convert.ToBoolean(oCampaignModel.Disabled);
            oBECampInfo.bFreeField = Convert.ToBoolean(oCampaignModel.NoFieldDataEntry);
            oBECampInfo.iCreatedBy = base.oUser.iUserID;
            oBECampInfo.iSkillID = lSkillInfo;
            oBECampInfo.sLocations = Encoder.HtmlEncode(oCampaignModel.Location);

            oBECampInfo.sShiftwindows = oCampaignModel.ShiftWindow;

            oBECampInfo.bPurposesWM = oCampaignModel.PurposesWM;
            oBECampInfo.bPurposeTime = oCampaignModel.PurposeTime;
            oBECampInfo.bPurposeTrans = oCampaignModel.PurposeTrans;

            //oBECampInfo.bPurposesWM = Convert.ToBoolean(oCampaignModel.WorkManagement);
            //oBECampInfo.bPurposeTime = Convert.ToBoolean(oCampaignModel.TimeTracking);
            //oBECampInfo.bPurposeTrans = Convert.ToBoolean(oCampaignModel.TransactionsMonitoring);

            oBECampInfo.sBusinessJustifications = oCampaignModel.BusinessJustifications;
            oBECampInfo.sTargetq1 = Encoder.HtmlEncode(oCampaignModel.Q1.ToString());
            oBECampInfo.sTargetq2 = Encoder.HtmlEncode(oCampaignModel.Q2.ToString());
            oBECampInfo.sTargetq3 = Encoder.HtmlEncode(oCampaignModel.Q3.ToString());
            oBECampInfo.sTargety1 = Encoder.HtmlEncode(oCampaignModel.Y1.ToString());
            oBECampInfo.sTargety2 = Encoder.HtmlEncode(oCampaignModel.Y2.ToString());
            oBECampInfo.sTargety3 = Encoder.HtmlEncode(oCampaignModel.Y3.ToString());
            oBECampInfo.sKeyBenefits = Encoder.HtmlEncode(oCampaignModel.KeyBenefits);

            oBECampInfo.bBillingSystem = Convert.ToBoolean(oCampaignModel.BillingSystem.ToString());
            oBECampInfo.sEmail = oCampaignModel.Email;
            oBECampInfo.iThresholdForCompletion = int.Parse(oCampaignModel.ThresholdForCompletion.ToString());
            oBECampInfo.iThresholdForToOpen = int.Parse(oCampaignModel.ThresholdForToOpen.ToString());
            oBECampInfo.dTargetEfficiency = double.Parse(oCampaignModel.TargetEfficiency.ToString());
            //if (oCampaignModel.BusinessApprover != null)
            //{
            oBECampInfo.iBuisnessID = oCampaignModel.ApproverId;
            oBECampInfo.iTechID = int.Parse("0");
            //}
            oBECampInfo.sStatus = "insertcampaign";
            return oBECampInfo;
        }


        [NonAction]
        private BECampaignInfo CatchRecordUpdateCampaignRequestChange(CampaignRequestChange oCampaignModel)
        {

            BECampaignInfo oBECampInfo = new BECampaignInfo();
            oBECampInfo.sLocations = Encoder.HtmlEncode(oCampaignModel.Location);
            oBECampInfo.sShiftwindows = oCampaignModel.ShiftWindow;
            oBECampInfo.sBusinessJustifications = oCampaignModel.BusinessJustifications;
            oBECampInfo.sTargetq1 = Encoder.HtmlEncode(oCampaignModel.Q1.ToString());
            oBECampInfo.sTargetq2 = Encoder.HtmlEncode(oCampaignModel.Q2.ToString());
            oBECampInfo.sTargetq3 = Encoder.HtmlEncode(oCampaignModel.Q3.ToString());
            oBECampInfo.sTargety1 = Encoder.HtmlEncode(oCampaignModel.Y1.ToString());
            oBECampInfo.sTargety2 = Encoder.HtmlEncode(oCampaignModel.Y2.ToString());
            oBECampInfo.sTargety3 = Encoder.HtmlEncode(oCampaignModel.Y3.ToString());
            oBECampInfo.sKeyBenefits = Encoder.HtmlEncode(oCampaignModel.KeyBenefits);
            oBECampInfo.bPurposesWM = oCampaignModel.PurposesWM;
            oBECampInfo.bPurposeTrans = oCampaignModel.PurposeTrans;
            oBECampInfo.bPurposeTime = oCampaignModel.PurposeTime;
            oBECampInfo.iApprovalId = oCampaignModel.ApprovalId;
            oBECampInfo.iModifiedBy = base.oUser.iUserID;
            oBECampInfo.iModeId = 0;
            return oBECampInfo;
        }


    }
}
