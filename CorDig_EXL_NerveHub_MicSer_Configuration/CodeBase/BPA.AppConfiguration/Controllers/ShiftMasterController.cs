using BPA.AppConfig.BusinessEntity.ExternalRef.WorkAllocation;
using BPA.AppConfig.ServiceContracts.ServiceContracts.ExternalRef.Configuration;
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
    public class ShiftMasterController : BaseController<ShiftMasterController>
    {
        private readonly IShiftService _repositoryShiftService;

        public ShiftMasterController(ILogger<ShiftMasterController> logger, IWebHostEnvironment env, IConfiguration configuration,
            IShiftService repositoryShiftService) : base(logger, env, configuration)
        {
            _repositoryShiftService = repositoryShiftService;
        }


        [ProducesResponseType(typeof(List<ShiftMasterModel>), StatusCodes.Status200OK)]
        [HttpGet]
        [Route("GetShiftList")]
        public async Task<MessageResponse<List<ShiftMasterModel>>> GetShiftList([FromQuery] string? searchShiftName)
        {
            var result = new MessageResponse<List<ShiftMasterModel>>();
            List<ShiftMasterModel> lstShiftMasterview = new List<ShiftMasterModel>();

            try
            {
                var dbresult = await Task.Run(() => searchShiftName == null ? _repositoryShiftService.GetShiftList(true, base.oTenant) :
                                                       _repositoryShiftService.GetShiftList(searchShiftName.Trim(), false, base.oTenant));

                lstShiftMasterview = dbresult.Select(x => new ShiftMasterModel
                {
                    ShiftID = x.iShiftID,
                    ShiftName = x.sShiftName,
                    Description = x.sShiftDescription,
                    Disabled = x.bDisabled,
                    UserId = x.iCreatedBy
                }).ToList();
                result.Data = lstShiftMasterview;
                result.TotalCount = lstShiftMasterview.Count;
            }
            catch (Exception ex)
            {
                result.Message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }

        [ProducesResponseType(typeof(ShiftMasterModel), StatusCodes.Status200OK)]
        [HttpGet]
        [Route("GetShiftById")]
        public async Task<MessageResponse<ShiftMasterModel>> GetShiftById([FromQuery] int shiftID)
        {
            var result = new MessageResponse<ShiftMasterModel>();
            try
            {
                var shiftMaster = await Task.Run(() => DisplayRecord(_repositoryShiftService.GetShiftList(shiftID, oTenant)[0]));
                result.Data = shiftMaster;
            }
            catch (Exception ex)
            {
                result.Message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }

        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [HttpPost]
        [Route("AddShift")]
        public async Task<MessageResponse<string>> AddShift([FromBody] ShiftMasterModel ShiftMasterModel)
        {
            var result = new MessageResponse<string>();
            try
            {
                await Task.Run(() => _repositoryShiftService.InsertData(CatchRecord(ShiftMasterModel), 15, base.oTenant));
                result.Data = string.Empty;
            }
            catch (Exception ex)
            {
                result.Message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }

        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [HttpPut]
        [Route("UpdateShift")]
        public async Task<MessageResponse<string>> UpdateShift([FromBody] ShiftMasterModel ShiftMasterModel)
        {
            var result = new MessageResponse<string>();
            try
            {
                await Task.Run(() => _repositoryShiftService.UpdateData(CatchRecord(ShiftMasterModel), 15, base.oTenant));
                result.Data = string.Empty;
            }
            catch (Exception ex)
            {
                result.Message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }

        [NonAction]
        private ShiftMasterModel DisplayRecord(BEShiftInfo oShift)
        {
            ShiftMasterModel oShiftViewModel = new();
            oShiftViewModel.ShiftID = oShift.iShiftID;
            oShiftViewModel.ShiftName = oShift.sShiftName;
            oShiftViewModel.Description = oShift.sShiftDescription;
            oShiftViewModel.Disabled = oShift.bDisabled;
            // string[] ArrayStartTime = oShift.sStartTime.Split(':');
            //oShiftViewModel.ShiftStartHrTime = ArrayStartTime[0].ToString();
            //oShiftViewModel.ShiftStartMinTime = ArrayStartTime[1].ToString();
            //string[] ArrayEndTime = oShift.sEndTime.Split(':');
            oShiftViewModel.ShiftStartTime = oShift.sStartTime;
            oShiftViewModel.ShiftEndTime = oShift.sEndTime;
            return oShiftViewModel;
        }

        [NonAction]
        private BEShiftInfo CatchRecord(ShiftMasterModel oShiftViewModel)
        {
            using (BEShiftInfo objShiftInfo = new BEShiftInfo())
            {
                objShiftInfo.iShiftID = oShiftViewModel.ShiftID;
                objShiftInfo.sShiftName = Encoder.HtmlEncode(oShiftViewModel.ShiftName.ToString(), false);
                objShiftInfo.sShiftDescription = Encoder.HtmlEncode(oShiftViewModel.Description.ToString(), false);
                objShiftInfo.bDisabled = oShiftViewModel.Disabled;
                if (oShiftViewModel.ShiftID == 0)
                {
                    objShiftInfo.iCreatedBy = oShiftViewModel.UserId;
                }
                else
                {
                    objShiftInfo.iModifiedBy = oShiftViewModel.UserId;
                }
                objShiftInfo.sStartTime = oShiftViewModel.ShiftStartTime.ToString();
                objShiftInfo.sEndTime = oShiftViewModel.ShiftEndTime.ToString();
                return objShiftInfo;
            }
        }
    }
}
