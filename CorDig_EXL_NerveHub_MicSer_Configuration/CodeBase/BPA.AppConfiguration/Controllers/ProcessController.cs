using BPA.AppConfig.BusinessEntity;
using BPA.AppConfig.BusinessEntity.Config;
using BPA.AppConfig.ServiceContracts.ServiceContracts.Config;
using BPA.AppConfiguration.BaseController;
using BPA.AppConfiguration.Helper;
using BPA.AppConfiguration.Helper.Filter;
using BPA.AppConfiguration.Models;
using BPA.AppConfiguration.Models.Request;
using BPA.AppConfiguration.Models.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Security.Application;
using System.Collections;
using System.Reflection;

namespace BPA.AppConfiguration.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [JwtAuthentication]
    public class ProcessController : BaseController<ProcessController>
    {
        private readonly ICalenderService _repositoryCalender;
        private readonly IMasterTableService _repositoryMasterTable;
        private readonly ILocationService _repositoryLocation;
        private readonly IProcessService _repositoryProcess;
        public ProcessController(ILogger<ProcessController> logger, IWebHostEnvironment env, IConfiguration configuration,
                                  ICalenderService repositoryCalender,
                                 IMasterTableService repositoryMasterTable, ILocationService repositoryLocation,
                                 IProcessService repositoryPermission) : base(logger, env, configuration)
        {
            _repositoryCalender = repositoryCalender;
            _repositoryMasterTable = repositoryMasterTable;
            _repositoryLocation = repositoryLocation;
            _repositoryProcess = repositoryPermission;
        }


        [ProducesResponseType(typeof(IList<BECalendarInfo>), StatusCodes.Status200OK)]
        [HttpGet]
        [Route("GetCalendarList")]
        public async Task<MessageResponse<IList<BECalendarInfo>>> GetCalendarList()
        {
            var result = new MessageResponse<IList<BECalendarInfo>>();
            try
            {
                var calendarList = await Task.Run(() => _repositoryCalender.GetCalendarList(true, base.oTenant));
                result.Data = calendarList;
                result.TotalCount = calendarList.Count;
            }
            catch (Exception ex)
            {
                result.Message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }

        [ProducesResponseType(typeof(List<BEMasterTable>), StatusCodes.Status200OK)]
        [HttpGet]
        [Route("GetProcessWorkTypeList")]
        public async Task<MessageResponse<List<BEMasterTable>>> GetProcessWorkTypeList()
        {
            var result = new MessageResponse<List<BEMasterTable>>();
            try
            {
                var processWorkTypeList = await Task.Run(() => _repositoryMasterTable.GetMasterList(48, base.oTenant));
                result.Data = processWorkTypeList;
                result.TotalCount = processWorkTypeList.Count;
            }
            catch (Exception ex)
            {
                result.Message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }

        [ProducesResponseType(typeof(List<BEMasterTable>), StatusCodes.Status200OK)]
        [HttpGet]
        [Route("GetProcessComplexityList")]
        public async Task<MessageResponse<List<BEMasterTable>>> GetProcessComplexityList()
        {
            var result = new MessageResponse<List<BEMasterTable>>();
            try
            {
                var processComplexityList = await Task.Run(() => _repositoryMasterTable.GetMasterList(15, base.oTenant));
                result.Data = processComplexityList;
                result.TotalCount = processComplexityList.Count;
            }
            catch (Exception ex)
            {
                result.Message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }

        [ProducesResponseType(typeof(List<BELocation>), StatusCodes.Status200OK)]
        [HttpGet]
        [Route("GetLocationList")]
        public async Task<MessageResponse<List<BELocation>>> GetLocationList()
        {
            var result = new MessageResponse<List<BELocation>>();
            try
            {
                var locationList = await Task.Run(() => _repositoryLocation.GetLocationList(false, base.oTenant));
                result.Data = locationList;
                result.TotalCount = locationList.Count;
            }
            catch (Exception ex)
            {
                result.Message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }

        [ProducesResponseType(typeof(List<BEERPProcess>), StatusCodes.Status200OK)]
        [HttpGet]
        [Route("GetERPGridWithSearchList")]
        public async Task<MessageResponse<List<BEERPProcess>>> GetERPGridWithSearchList([FromQuery] string? erpProcessName, [FromQuery] int locationId)
        {
            var result = new MessageResponse<List<BEERPProcess>>();
            try
            {
                erpProcessName = string.IsNullOrWhiteSpace(erpProcessName) ? "" : erpProcessName;
                var erpProcessList = await Task.Run(() => _repositoryProcess.GetERPProcessList(erpProcessName, locationId, 0, base.oTenant));
                result.Data = erpProcessList;//.OrderBy(x => x.iERPProcessID).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
                result.TotalCount = result.Data.Count;
            }
            catch (Exception ex)
            {
                result.Message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }

        [ProducesResponseType(typeof(List<BEERPProcess>), StatusCodes.Status200OK)]
        [HttpPost]
        [Route("GetERPProcessListByERPProcessIds")]
        public async Task<MessageResponse<List<BEERPProcess>>> GetERPProcessListByERPProcessIds([FromBody] ArrayList aryDistinctERPProcessId)
        {
            var result = new MessageResponse<List<BEERPProcess>>();
            try
            {
                var erpProcessList = await Task.Run(() => _repositoryProcess.GetERPProcessList(aryDistinctERPProcessId, base.oTenant));
                result.Data = erpProcessList;
                result.TotalCount = result.Data.Count;
            }
            catch (Exception ex)
            {
                result.Message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }

        [ProducesResponseType(typeof(List<BEProcessInfo>), StatusCodes.Status200OK)]
        [HttpGet]
        [Route("GetProcessListByClientId")]
        public async Task<MessageResponse<List<BEProcessInfo>>> GetProcessListByClientId([FromQuery] int clientId, [FromQuery] string? processName, [FromQuery] bool activeProcess)
        {
            var result = new MessageResponse<List<BEProcessInfo>>();
            try
            {
                processName = processName ?? "";
                var processList = await Task.Run(() => _repositoryProcess.GetProcessList(base.oUser.iUserID, clientId, processName, activeProcess, oTenant));
                result.Data = processList;
                result.TotalCount = processList.Count;
            }
            catch (Exception ex)
            {
                result.Message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }
        [ProducesResponseType(typeof(List<BEProcessInfo>), StatusCodes.Status200OK)]
        [HttpGet]
        [Route("GetProcessListReports")]
        public async Task<MessageResponse<List<BEProcessInfo>>> GetProcessListReports()
        {
            var result = new MessageResponse<List<BEProcessInfo>>();
            try
            {
                var processList = await Task.Run(() => _repositoryProcess.GetProcessList(base.oUser.iUserID, true, oTenant));
                result.Data = processList;
                result.TotalCount = processList.Count;
            }
            catch (Exception ex)
            {
                result.Message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }

        [ProducesResponseType(typeof(List<BEProcessInfo>), StatusCodes.Status200OK)]
        [HttpGet]
        [Route("GetProcessListSearch")]
        public async Task<MessageResponse<List<BEProcessInfo>>> GetProcessListSearch([FromQuery] int clientId, [FromQuery] string? processName)
        {
            var result = new MessageResponse<List<BEProcessInfo>>();
            try
            {
                processName = processName ?? "";
                var processList = await Task.Run(() => _repositoryProcess.GetProcessListSearch(base.oUser.iUserID, clientId, processName, true, base.oTenant));
                result.Data = processList;
                result.TotalCount = processList.Count;
            }
            catch (Exception ex)
            {
                result.Message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }

        [ProducesResponseType(typeof(BEProcessInfo), StatusCodes.Status200OK)]
        [HttpGet]
        [Route("GetProcessDetailsById")]
        public async Task<MessageResponse<BEProcessInfo>> GetProcessDetailsById([FromQuery] int iProcessId)
        {
            var result = new MessageResponse<BEProcessInfo>();
            try
            {
                var process = await Task.Run(() => _repositoryProcess.GetProcessDetails(iProcessId, base.oTenant));
                result.Data = process;
            }
            catch (Exception ex)
            {
                result.Message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }


        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [HttpGet]
        [Route("GetCheckRoleForOrgProcess")]
        public async Task<MessageResponse<int>> GetCheckRoleForOrgProcess()
        {
            var result = new MessageResponse<int>();
            try
            {
                var roleExist = await Task.Run(() => _repositoryProcess.CheckRoleForOrgProcess(base.oUser.iUserID, base.oTenant));
                result.Data = roleExist;
            }
            catch (Exception ex)
            {
                result.Message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }

        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [HttpGet]
        [Route("GetCheckProcessByClientForUniqueness")]
        public async Task<MessageResponse<bool>> GetCheckProcessByClientForUniqueness([FromQuery] string processName, [FromQuery] int clientID, [FromQuery] int processID)
        {
            var result = new MessageResponse<bool>();
            try
            {
                var Isunique = await Task.Run(() => _repositoryProcess.CheckProcessByClientForUniqueness(Encoder.HtmlEncode(processName, false), clientID, processID, base.oTenant));
                result.Data = Isunique;
            }
            catch (Exception ex)
            {
                result.Message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }

        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [HttpPost]
        [Route("AddProcess")]
        public async Task<MessageResponse<string>> AddProcess([FromBody] ProcessModel processModel)
        {
            var result = new MessageResponse<string>();
            try
            {
                await Task.Run(() => _repositoryProcess.InsertData(CatchRecord(processModel), 12, base.oTenant));
                result.Data = "1";
            }
            catch (Exception ex)
            {
                result.Message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }

        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [HttpPut]
        [Route("UpdateProcess")]
        public async Task<MessageResponse<string>> UpdateProcess([FromBody] ProcessModel processModel)
        {
            var result = new MessageResponse<string>();
            try
            {
                await Task.Run(() => _repositoryProcess.UpdateData(CatchRecord(processModel), 12, base.oTenant));
                result.Data = "2";
            }
            catch (Exception ex)
            {
                result.Message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }

        private BEProcessInfo CatchRecord(ProcessModel ObjvmProcess)
        {
            BEProcessInfo oProcess = new();
            BEProcessInfo oProcessNew = new();

            //if (TempData["lProcessGroup"] != null)
            //{

            foreach (var item in ObjvmProcess.lProcessGroup)
            {
                BEProcessGroup oProcessGroup = new();
                BEERPProcess oERPProcessNew = new()
                {
                    bDisabled = false,
                    iERPProcessID = item.ERPProcessID,
                    iERPCode = item.ERPCode,
                    sName = item.Name
                };
                oProcessGroup.oERPProcess = oERPProcessNew;
                oProcessGroup.iProcessGroupID = item.ProcessGroupID;
                oProcessGroup.oRowState = (item.ProcessGroupID > 0 ? RowState.NONE : RowState.NEW);
                oProcessNew.lProcessGroup.Add(oProcessGroup);
            }

            oProcess.lProcessFTE = oProcessNew.lProcessFTE;
            oProcess.lProcessGroup = oProcessNew.lProcessGroup;

            oProcess.iProcessID = ObjvmProcess.Processid;
            oProcess.sProcessName = Encoder.HtmlEncode(ObjvmProcess.Processname);
            oProcess.sProcessDescription = Encoder.HtmlEncode(ObjvmProcess.Description);
            //if (ObjvmProcess.ClientName == null)
            //{
            //    if (ObjvmProcess.ClientId != null)
            //    {
            //        ObjvmProcess.ClientName = ObjvmProcess.CLIENTID.ToString();
            //    }
            //}
            oProcess.iClientID = ObjvmProcess.Clientid;// int.Parse(ObjvmProcess.ClientName == null ? "0" : ObjvmProcess.ClientName);
            //oProcess.iProcessType = ObjvmProcess.Processtype;
            oProcess.iSBUID = ObjvmProcess.Sbuid;
            oProcess.iCalendarID = ObjvmProcess.Calendarid == null ? 0 : Convert.ToInt32(ObjvmProcess.Calendarid);
            oProcess.iProcessType = ObjvmProcess.Processtype == null ? 0 : Convert.ToInt32(ObjvmProcess.Processtype);
            oProcess.iProcessWorkType = ObjvmProcess.Processworktype;
            oProcess.sScope = Encoder.HtmlEncode(ObjvmProcess.Scope);

            oProcess.oProcessSLA.dPilotStartDate = ObjvmProcess.Pilotstartdate;
            oProcess.oProcessSLA.dPilotEndDate = ObjvmProcess.Pilotenddate;
            //if (txtGoLiveDate.Text != "") oProcess.dGoLiveDate = Convert.ToDateTime(txtGoLiveDate.Text);
            oProcess.bDisabled = Convert.ToBoolean(ObjvmProcess.Disabled);
            oProcess.iCreatedBy = base.oUser.iUserID;
            //Catch process SLA information
            BEProcessSLA oProcessSLA = new();
            //if (ObjvmProcess.ProcessSLA != "")
            //    oProcessSLA.iProcessSLAID = ObjvmProcess.ProcessSLAId;

            //oProcessSLA.dStartDate = ObjvmProcess.SLAStartDate;
            //oProcessSLA.dEndDate = ObjvmProcess.SLAEndDate;

            oProcessSLA.dPilotStartDate = ObjvmProcess.Pilotstartdate;
            oProcessSLA.dPilotEndDate = ObjvmProcess.Pilotenddate;

            //oProcessSLA.iProcessSLAID = ObjvmProcess.ProcessSLAId == 0 ? 0 : ObjvmProcess.ProcessSLAId;
            //oProcessSLA.sStage = Encoder.HtmlEncode(ObjvmProcess.Stage);

            oProcessSLA.bDisabled = Convert.ToBoolean(ObjvmProcess.Disabled);
            oProcessSLA.iCreatedBy = base.oUser.iUserID;

            oProcess.oProcessSLA = oProcessSLA;

            oProcess.dStabilizationStartDate = ObjvmProcess.StabilizationStartDate;
            oProcess.dStabilizationEndDate = ObjvmProcess.StabilizationEndDate;
            oProcess.dProductionStartDate = ObjvmProcess.ProductionStartDate;
            oProcess.dProductionEndDate = ObjvmProcess.ProductionEndDate;

            oProcess.iProcessComplexity = ObjvmProcess.ProcessComplexity;
            oProcess.iCAPType = ObjvmProcess.Captype;

            oProcess.iQCAFeebackTragetPerWeek = ObjvmProcess.QCAFeebackTragetPerWeek;

            oProcess.iSupervisorFeedbackTargetFrequency = ObjvmProcess.SupervisorFeedbackTargetFrequency;
            oProcess.iSupervisorFeebackTragetPerWeek = ObjvmProcess.SupervisorFeebackTragetPerWeek;
            oProcess.iTargetAuditPerMonth = ObjvmProcess.TargetAuditPerMonth;
            oProcess.iTargetQCAHrs = ObjvmProcess.TargetQCAHrs;
            return oProcess;
        }
    }
}
