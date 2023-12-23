using BPA.AppConfig.BusinessEntity.Config;
using BPA.AppConfig.ServiceContracts.ServiceContracts.Config;
using BPA.AppConfiguration.BaseController;
using BPA.AppConfiguration.Helper;
using BPA.AppConfiguration.Helper.Filter;
using BPA.AppConfiguration.Models;
using BPA.AppConfiguration.Models.Request;
using BPA.AppConfiguration.Models.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Security.Application;
using System.Reflection;

namespace BPA.AppConfiguration.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [JwtAuthentication]
    public class FacilityController : BaseController<FacilityController>
    {
        private readonly IFacilityService _repositoryFacility;
        private readonly ILocationService _repositoryLocation;

        public FacilityController(ILogger<FacilityController> logger, IWebHostEnvironment env, IConfiguration configuration,
            IFacilityService repositoryFacility, ILocationService repositoryLocation) : base(logger, env, configuration)
        {
            _repositoryFacility = repositoryFacility;
            _repositoryLocation = repositoryLocation;
        }

        [ProducesResponseType(typeof(SelectList), StatusCodes.Status200OK)]
        [HttpGet]
        [Route("GetLocations")]
        public async Task<MessageResponse<SelectList>> GetLocations()
        {
            var result = new MessageResponse<SelectList>();
            try
            {
                var tempLstLocation = await Task.Run(() => _repositoryLocation.GetLocationList("", false, base.oTenant));
                IList<SelectListItem> Locations = new List<SelectListItem>();
                foreach (var item in tempLstLocation)
                {
                    Locations.Add(new SelectListItem { Text = item.sLocationName, Value = item.iLocationID.ToString() });
                }
                result.Data = new SelectList(Locations, "Value", "Text");
                result.TotalCount = result.Data.Count();
            }
            catch (Exception ex)
            {
                result.Message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }


        [ProducesResponseType(typeof(List<BEFacility>), StatusCodes.Status200OK)]
        [HttpGet]
        [Route("GetFacilityList")]
        public async Task<MessageResponse<List<BEFacility>>> GetFacilityList([FromQuery] string? searchFacility, [FromQuery] bool activeFacility)
        {
            var result = new MessageResponse<List<BEFacility>>();

            try
            {
                var dbresult = await Task.Run(() => _repositoryFacility.GetFacilityList(searchFacility == null ? "" : searchFacility.Trim(), activeFacility, base.oTenant));
                result.Data = dbresult;
                result.TotalCount = dbresult.Count;
            }
            catch (Exception ex)
            {
                result.Message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }

        [ProducesResponseType(typeof(FacilityModel), StatusCodes.Status200OK)]
        [HttpGet]
        [Route("GetFacilityById")]
        public async Task<MessageResponse<FacilityModel>> GetFacilityById([FromQuery] int facilityID)
        {
            var result = new MessageResponse<FacilityModel>();
            FacilityModel objFacility = new();
            try
            {
                var SelectedFacility = await Task.Run(() => _repositoryFacility.GetFacilityList(facilityID, oTenant)[0]);
                if (SelectedFacility != null)
                {
                    objFacility.FacilityID = SelectedFacility.iFacilityID;
                    objFacility.LocationID = SelectedFacility.iLocationID;
                    objFacility.FacilityName = SelectedFacility.sFacilityName;
                    objFacility.FacilityDescription = SelectedFacility.sFacilityDescription;
                    objFacility.Disabled = SelectedFacility.bDisabled;
                }
                result.Data = objFacility;
            }
            catch (Exception ex)
            {
                result.Message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }

        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [HttpPost]
        [Route("AddFacility")]
        public async Task<MessageResponse<string>> AddFacility([FromBody] FacilityModel FacilityModel)
        {
            var result = new MessageResponse<string>();
            try
            {
                await Task.Run(() => _repositoryFacility.InsertData(CatchRecord(FacilityModel), 62, base.oTenant));
                result.Data = "displayFacilitySaved";
            }
            catch (Exception ex)
            {
                result.Message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }

        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [HttpPut]
        [Route("UpdateFacility")]
        public async Task<MessageResponse<string>> UpdateFacility([FromBody] FacilityModel FacilityModel)
        {
            var result = new MessageResponse<string>();
            try
            {
                await Task.Run(() => _repositoryFacility.UpdateData(CatchRecord(FacilityModel), 62, base.oTenant));
                result.Data = "displayFacilityUpdated";
            }
            catch (Exception ex)
            {
                result.Message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }

        [NonAction]
        private BEFacility CatchRecord(FacilityModel oModel)
        {
            BEFacility oFacility = new();
            oFacility.iFacilityID = oModel.FacilityID;
            oFacility.sFacilityName = Encoder.HtmlEncode(oModel.FacilityName, false);
            oFacility.sFacilityDescription = Encoder.HtmlEncode(oModel.FacilityDescription, false);
            oFacility.iLocationID = oModel.LocationID;
            oFacility.bDisabled = oModel.Disabled;
            if (oFacility.iFacilityID == 0)
            {
                oFacility.iCreatedBy = oModel.UserId;
            }
            else
            {
                oFacility.iModifiedBy = oModel.UserId;
            }
            return oFacility;
        }

    }
}
