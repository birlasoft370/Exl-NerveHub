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
    public class LocationController : BaseController<LocationController>
    {
        private readonly ILocationService _repositoryLocation;

        public LocationController(ILogger<LocationController> logger, IWebHostEnvironment env, IConfiguration configuration, 
            ILocationService repositoryLocation) : base(logger, env, configuration)
        {
            _repositoryLocation = repositoryLocation;
        }


        [ProducesResponseType(typeof(List<LocationModel>), StatusCodes.Status200OK)]
        [HttpGet]
        [Route("GetLocationList")]
        public async Task<MessageResponse<List<LocationModel>>> GetLocationList([FromQuery] string? searchLocation)
        {
            var result = new MessageResponse<List<LocationModel>>();
            List<LocationModel> lstLocationview = new List<LocationModel>();

            try
            {
                var dbresult = await Task.Run(() => searchLocation == null ? _repositoryLocation.GetLocationList(true, base.oTenant) :
                                                       _repositoryLocation.GetLocationList(Encoder.HtmlEncode(searchLocation, false), true, base.oTenant));

                lstLocationview = dbresult.Select(x => new LocationModel
                {
                    LocationID = x.iLocationID,
                    LocationName = x.sLocationName,
                    LocationDesc = x.sLocationDescription,
                    Disabled = x.bDisabled
                }).ToList();
                result.Data = lstLocationview;
                result.TotalCount = lstLocationview.Count;
            }
            catch (Exception ex)
            {
                result.Message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }

        [ProducesResponseType(typeof(LocationModel), StatusCodes.Status200OK)]
        [HttpGet]
        [Route("GetLocationById")]
        public async Task<MessageResponse<LocationModel>> GetLocationById([FromQuery] int LocationID)
        {
            var result = new MessageResponse<LocationModel>();
            try
            {
                var Location = await Task.Run(() => DisplayRecord(_repositoryLocation.GetLocationList(LocationID, oTenant).FirstOrDefault()));
                result.Data = Location;
            }
            catch (Exception ex)
            {
                result.Message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }

        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [HttpPost]
        [Route("AddLocation")]
        public async Task<MessageResponse<string>> AddLocation([FromBody] LocationModel LocationModel)
        {
            var result = new MessageResponse<string>();
            try
            {
                await Task.Run(() => _repositoryLocation.InsertData(CatchRecord(LocationModel), 61, base.oTenant));
                result.Data = "display_LocationSaved";
            }
            catch (Exception ex)
            {
                result.Message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }

        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [HttpPut]
        [Route("UpdateLocation")]
        public async Task<MessageResponse<string>> UpdateLocation([FromBody] LocationModel LocationModel)
        {
            var result = new MessageResponse<string>();
            try
            {
                await Task.Run(() => _repositoryLocation.UpdateData(CatchRecord(LocationModel), 61, base.oTenant));
                result.Data = "display_LocationUpdated";
            }
            catch (Exception ex)
            {
                result.Message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }

        [NonAction]
        private LocationModel DisplayRecord(BELocation oLocation)
        {
            LocationModel objLocationModel = new LocationModel();
            objLocationModel.LocationID = int.Parse(oLocation.iLocationID.ToString());
            objLocationModel.LocationName = Encoder.HtmlEncode(oLocation.sLocationName, false);
            objLocationModel.LocationDesc = oLocation.sLocationDescription;
            objLocationModel.Disabled = oLocation.bDisabled;
            return objLocationModel;
        }

        [NonAction]
        private BELocation CatchRecord(LocationModel location)
        {
            BELocation objLocationInfo = new BELocation();
            objLocationInfo.iLocationID = location.LocationID;
            objLocationInfo.sLocationName = Encoder.HtmlEncode(location.LocationName, false);
            objLocationInfo.sLocationDescription = Encoder.HtmlEncode(location.LocationDesc, false);
            objLocationInfo.bDisabled = location.Disabled;
            objLocationInfo.iCreatedBy = location.UserId;
            objLocationInfo.iModifiedBy = location.UserId;
            return objLocationInfo;
        }

    }
}
