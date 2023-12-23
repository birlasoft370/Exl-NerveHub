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
    public class VerticalMasterController : BaseController<VerticalMasterController>
    {
        private readonly IVerticalService _repositoryVertical;

        public VerticalMasterController(ILogger<VerticalMasterController> logger, IWebHostEnvironment env, IConfiguration configuration,
            IVerticalService VerticalService) : base(logger, env, configuration)
        {
            this._repositoryVertical = VerticalService;
        }

        [ProducesResponseType(typeof(List<VerticalMasterModel>), StatusCodes.Status200OK)]
        [HttpGet]
        [Route("GetVerticalMasterList")]
        public async Task<MessageResponse<List<VerticalMasterModel>>> GetVerticalMasterList([FromQuery] string? vName)
        {
            var result = new MessageResponse<List<VerticalMasterModel>>();
            List<VerticalMasterModel> ObjTMaster = new();

            List<BEVerticalInfo> ObjListBE = new();
            try
            {
                await Task.Run(() =>
                {
                    if (vName == null)
                    {
                        ObjListBE = this._repositoryVertical.GetVerticalList(false, base.oTenant);
                    }
                    else
                    {
                        ObjListBE = this._repositoryVertical.GetVerticalList(vName, false, base.oTenant);
                    }
                });
                foreach (var item in ObjListBE)
                {
                    ObjTMaster.Add(new VerticalMasterModel
                    {
                        VerticalID = item.iVerticalID,
                        ERPID = item.iERPID,
                        VerticaName = item.sVerticalName + " " + (Convert.ToBoolean(((BPA.AppConfig.BusinessEntity.ObjectBase)(item)).bDisabled) ? "(Disabled)" : ""),
                        VerticalDescription = item.sDescription,
                        Disabled = item.bDisabled,
                        UserId = item.iCreatedBy
                    });
                }
                result.Data = ObjTMaster;
                result.TotalCount = ObjTMaster.Count;
            }
            catch (Exception ex)
            {
                result.Message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }

        [ProducesResponseType(typeof(VerticalMasterModel), StatusCodes.Status200OK)]
        [HttpGet]
        [Route("GetVerticalMasterById")]
        public async Task<MessageResponse<VerticalMasterModel>> GetVerticalMasterById([FromQuery] int verticalMasterID)
        {
            var result = new MessageResponse<VerticalMasterModel>();
            VerticalMasterModel ObjVertical = new();
            try
            {
                var ObjListBE = await Task.Run(() => _repositoryVertical.GetVerticalList(verticalMasterID, oTenant));

                foreach (var item in ObjListBE)
                {
                    ObjVertical.VerticalID = item.iVerticalID;
                    ObjVertical.ERPID = item.iERPID;
                    ObjVertical.VerticaName = item.sVerticalName;
                    ObjVertical.VerticalDescription = item.sDescription;
                    ObjVertical.Disabled = item.bDisabled;
                    ObjVertical.UserId = item.iCreatedBy;
                }
                result.Data = ObjVertical;
            }
            catch (Exception ex)
            {
                result.Message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }

        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [HttpPost]
        [Route("AddVerticalMaster")]
        public async Task<MessageResponse<string>> AddVerticalMaster([FromBody] VerticalMasterModel VerticalMasterModel)
        {
            var result = new MessageResponse<string>();
            try
            {
                await Task.Run(() => this._repositoryVertical.InsertData(CatchRecords(VerticalMasterModel), 123, base.oTenant));

                result.Data = "display_Save_msg";
            }
            catch (Exception ex)
            {
                result.Message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }

        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [HttpPut]
        [Route("UpdateVerticalMaster")]
        public async Task<MessageResponse<string>> UpdateVerticalMaster([FromBody] VerticalMasterModel VerticalMasterModel)
        {
            var result = new MessageResponse<string>();
            try
            {
                await Task.Run(() => this._repositoryVertical.UpdateData(CatchRecords(VerticalMasterModel), 123, base.oTenant));

                result.Data = "display_update_msg";

            }
            catch (Exception ex)
            {
                result.Message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }

        [NonAction]
        private BEVerticalInfo CatchRecords(VerticalMasterModel ObjVertical)
        {
            using (BEVerticalInfo objBEVerticalMaster = new())
            {
                objBEVerticalMaster.iVerticalID = ObjVertical.VerticalID;
                objBEVerticalMaster.iERPID = ObjVertical.ERPID.GetValueOrDefault();
                objBEVerticalMaster.sVerticalName = Encoder.HtmlEncode(ObjVertical.VerticaName, false);
                objBEVerticalMaster.sDescription = ObjVertical.VerticalDescription;
                objBEVerticalMaster.bDisabled = Convert.ToBoolean(ObjVertical.Disabled);
                objBEVerticalMaster.iModifiedBy = ObjVertical.UserId;

                return objBEVerticalMaster;
            }
        }
    }
}
