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
using System.Reflection;

namespace BPA.AppConfiguration.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [JwtAuthentication]
    public class SubProcessMasterController : BaseController<SubProcessMasterController>
    {
        private readonly ISubProcessService _repositorySubProcess;
        private readonly IProcessService _repositoryProcess;

        public SubProcessMasterController(ILogger<SubProcessMasterController> logger, IWebHostEnvironment env, IConfiguration configuration,
            ISubProcessService repositorySubProcess,
            IProcessService repositoryProcess) : base(logger, env, configuration)
        {
            _repositorySubProcess = repositorySubProcess;
            _repositoryProcess = repositoryProcess;
        }

        [ProducesResponseType(typeof(List<GetSubProcessMaster>), StatusCodes.Status200OK)]
        [HttpGet]
        [Route("GetSubProcessListByProcessId")]
        public async Task<MessageResponse<List<GetSubProcessMaster>>> GetSubProcessListByProcessId([FromQuery] int processId)
        {
            var result = new MessageResponse<List<GetSubProcessMaster>>();
            try
            {
                var subProcessList = await Task.Run(() =>
                _repositorySubProcess.GetSubProcessListProcessWise(processId, base.oTenant)
                 .Select(item => new GetSubProcessMaster
                 {
                     SubProcessID = item.iSubProcessID,
                     SubProcessName = item.sSubProcessName
                 }).ToList());
                result.Data = subProcessList;
                result.TotalCount = subProcessList.Count;
                return result;
            }
            finally
            {
                result = null;
                //_logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
                //result.Message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }

        }

        [ProducesResponseType(typeof(List<BESubProcess>), StatusCodes.Status200OK)]
        [HttpGet]
        [Route("GetSubProcessList")]
        public async Task<MessageResponse<List<BESubProcess>>> GetSubProcessList([FromQuery] int processId, [FromQuery] string? subProcessName, [FromQuery] bool activeSubProcess)
        {
            var result = new MessageResponse<List<BESubProcess>>();
            try
            {
                var subProcessList = await Task.Run(() =>
                _repositorySubProcess.GetSubProcessList(processId, subProcessName, activeSubProcess, base.oTenant));
                result.Data = subProcessList;
                result.TotalCount = subProcessList.Count;
            }
            catch (Exception ex)
            {
                result.Message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }

        [ProducesResponseType(typeof(BESubProcess), StatusCodes.Status200OK)]
        [HttpGet]
        [Route("GetSubProcessById")]
        public async Task<MessageResponse<BESubProcess>> GetSubProcessById([FromQuery] int subProcessID)
        {
            var result = new MessageResponse<BESubProcess>();
            try
            {
                var subProcess = await Task.Run(() =>
                _repositorySubProcess.GetSubProcessList(subProcessID, base.oTenant));
                result.Data = subProcess;
            }
            catch (Exception ex)
            {
                result.Message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }

        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [HttpPost]
        [Route("AddSubProcess")]
        public async Task<MessageResponse<string>> AddSubProcess([FromBody] SubProcessMasterModel oSubProcessModel)
        {
            var result = new MessageResponse<string>();
            try
            {
                if (_repositoryProcess.CheckCalenderExistance(oSubProcessModel.ProcessID, oSubProcessModel.SubProcessStartDate, oSubProcessModel.SubProcessEndDate, 0, base.oTenant) == 0)
                {
                    result.Data = "required_NoCalender";
                }
                else
                {
                    await Task.Run(() => _repositorySubProcess.InsertData(CatchRecord(oSubProcessModel), 158, base.oTenant));
                    result.Data = "display_SaveMessage";
                }
            }
            catch (Exception ex)
            {
                result.Message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }

        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [HttpPut]
        [Route("UpdateSubProcess")]
        public async Task<MessageResponse<string>> UpdateSubProcess([FromBody] SubProcessMasterModel oSubProcessModel)
        {
            var result = new MessageResponse<string>();
            try
            {
                if (_repositoryProcess.CheckCalenderExistance(oSubProcessModel.ProcessID, oSubProcessModel.SubProcessStartDate, oSubProcessModel.SubProcessEndDate, 0, base.oTenant) == 0)
                {
                    result.Data = "required_NoCalender";
                }
                else
                {
                    await Task.Run(() => _repositorySubProcess.UpdateData(CatchRecord(oSubProcessModel), 158, base.oTenant));
                    result.Data = "display_Update";
                }
            }
            catch (Exception ex)
            {
                result.Message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }

        [NonAction]
        private SubProcessMasterModel DisplayRecord(BESubProcess oSubProcess)
        {
            SubProcessMasterModel oSubProcessModel = new SubProcessMasterModel();
            oSubProcessModel.ClientID = oSubProcess.iClientID;
            oSubProcessModel.ProcessID = oSubProcess.iProcessID;
            oSubProcessModel.SubProcessID = oSubProcess.iSubProcessID;
            oSubProcessModel.SubProcessName = oSubProcess.sSubProcessName;
            oSubProcessModel.Description = oSubProcess.sDescription;
            oSubProcessModel.GoLiveDate = oSubProcess.dGoLiveDate;
            oSubProcessModel.ProductionEndDate = oSubProcess.dProductionEndDate;
            oSubProcessModel.ProductionStartDate = oSubProcess.dProductionStartDate;
            oSubProcessModel.StabilizationEndDate = oSubProcess.dStabilizationEndDate;
            oSubProcessModel.StabilizationStartDate = oSubProcess.dStabilizationStartDate;
            oSubProcessModel.SubProcessEndDate = oSubProcess.dSubProcessEndDate;
            oSubProcessModel.SubProcessStartDate = oSubProcess.dSubProcessStartDate;
            // oSubProcessModel.PDSubProcessName = oSubProcess.sPDSubProcessName;
            // oSubProcessModel.HFPDSubProcessId = oSubProcess.iPDSubProcessID.ToString();
            oSubProcessModel.Disabled = oSubProcess.bDisabled;
            return oSubProcessModel;
        }

        [NonAction]
        private BESubProcess CatchRecord(SubProcessMasterModel oSubProcessModel)
        {
            BESubProcess oBESubProcess = new BESubProcess();
            oBESubProcess.iProcessID = oSubProcessModel.ProcessID;
            oBESubProcess.sSubProcessName = Encoder.HtmlEncode(oSubProcessModel.SubProcessName);
            oBESubProcess.sDescription = Encoder.HtmlEncode(oSubProcessModel.Description);
            oBESubProcess.iSubProcessID = oSubProcessModel.SubProcessID;
            oBESubProcess.dGoLiveDate = oSubProcessModel.GoLiveDate;// DateTime.Parse(oSubProcessModel.GoLiveDate, CultureInfo.InvariantCulture);
            oBESubProcess.dProductionEndDate = oSubProcessModel.ProductionEndDate; // DateTime.ParseExact(oSubProcessViewModel.ProductionEndDate.ToString(), "MM/dd/yyyy", CultureInfo.InvariantCulture); 
            oBESubProcess.dProductionStartDate = oSubProcessModel.ProductionStartDate; // DateTime.ParseExact(oSubProcessViewModel.ProductionStartDate.ToString(), "MM/dd/yyyy", CultureInfo.InvariantCulture); 
            oBESubProcess.dStabilizationEndDate = oSubProcessModel.StabilizationEndDate; //DateTime.ParseExact(oSubProcessViewModel.StabilizationEndDate.ToString(), "MM/dd/yyyy", CultureInfo.InvariantCulture); 
            oBESubProcess.dStabilizationStartDate = oSubProcessModel.StabilizationStartDate; // DateTime.ParseExact(oSubProcessViewModel.StabilizationStartDate.ToString(), "MM/dd/yyyy", CultureInfo.InvariantCulture); 
            oBESubProcess.dSubProcessEndDate = oSubProcessModel.SubProcessEndDate; // DateTime.ParseExact(oSubProcessViewModel.SubProcessEndDate.ToString(), "MM/dd/yyyy", CultureInfo.InvariantCulture); 
            oBESubProcess.dSubProcessStartDate = oSubProcessModel.SubProcessStartDate; // DateTime.ParseExact(oSubProcessViewModel.SubProcessStartDate.ToString(), "MM/dd/yyyy", CultureInfo.InvariantCulture); 
            oBESubProcess.bDisabled = Convert.ToBoolean(oSubProcessModel.Disabled);
            oBESubProcess.iCreatedBy = base.oUser.iUserID;
            //if (oBESubProcess.dProductionStartDate > oBESubProcess.dProductionEndDate)
            //    throw new ApplicationException(BPA.GlobalResources.UI.AppConfiguration.Resources_SubProcessMaster.display_ProductionStartDateCannotGreater);
            //if (oBESubProcess.dStabilizationStartDate > oBESubProcess.dStabilizationEndDate)
            //    throw new ApplicationException(BPA.GlobalResources.UI.AppConfiguration.Resources_SubProcessMaster.display_StabilizationDatecannotgreater);
            //if (oBESubProcess.dSubProcessStartDate > oBESubProcess.dSubProcessEndDate)
            //    throw new ApplicationException(BPA.GlobalResources.UI.AppConfiguration.Resources_SubProcessMaster.display_SubprocessStartDateGreater);
            //oBESubProcess.iPDSubProcessID = Convert.ToInt32(oSubProcessModel.PDSubProcessName);
            return oBESubProcess;
        }
    }
}
