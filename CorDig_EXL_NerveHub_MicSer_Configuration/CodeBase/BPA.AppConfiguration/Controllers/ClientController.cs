using BPA.AppConfig.BusinessEntity.Config;
using BPA.AppConfig.BusinessEntity.ExternalRef.Configuration;
using BPA.AppConfig.BusinessLayer.ExternalRef.Configuration;
using BPA.AppConfig.ServiceContracts.ServiceContracts.Config;
using BPA.AppConfig.ServiceContracts.ServiceContracts.ExternalRef.Configuration;
using BPA.AppConfiguration.BaseController;
using BPA.AppConfiguration.Helper;
using BPA.AppConfiguration.Helper.Filter;
using BPA.AppConfiguration.Models;
using BPA.AppConfiguration.Models.Request;
using BPA.AppConfiguration.Models.Response;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Reflection;

namespace BPA.AppConfiguration.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [JwtAuthentication]
    public class ClientController : BaseController<ClientController>
    {
        private readonly IClientService _repositoryClient;
        private readonly ISBUInfoService _repositorySBU;
        private readonly IVerticalService _repositoryVertical;
        public ClientController(ILogger<ClientController> logger, IWebHostEnvironment env, IConfiguration configuration,
            IClientService repositoryClient,
            ISBUInfoService repositorySBU,
            IVerticalService repositoryVertical) : base(logger, env, configuration)
        {
            this._repositoryClient = repositoryClient;
            this._repositorySBU = repositorySBU;
            this._repositoryVertical = repositoryVertical;
        }

        [ProducesResponseType(typeof(IEnumerable<BEVerticalInfo>), StatusCodes.Status200OK)]
        [HttpGet]
        [Route("GetVerticalList")]
        public async Task<MessageResponse<IEnumerable<BEVerticalInfo>>> GetVerticalList()
        {
            var result = new MessageResponse<IEnumerable<BEVerticalInfo>>();
            try
            {
                var verticalList = await Task.Run(() => _repositoryVertical.GetVerticalList(true, base.oTenant));
                result.Data = verticalList;
                result.TotalCount = verticalList.Count;
            }
            catch (Exception ex)
            {
                result.Message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }

        /*
        [ProducesResponseType(typeof(IEnumerable<BESBUInfo>), StatusCodes.Status200OK)]
        [HttpGet]
        [Route("GetSBUList")]
        public async Task<MessageResponse<IEnumerable<BESBUInfo>>> GetSBUList()
        {
            var result = new MessageResponse<IEnumerable<BESBUInfo>>();
            try
            {
                var sbuList = await Task.Run(() => _repositorySBU.GetSBUList(true, base.oTenant));
                result.Data = sbuList;
                result.TotalCount = sbuList.Count;
            }
            catch (Exception ex)
            {
                result.Message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }
        */
        [ProducesResponseType(typeof(IEnumerable<BESBUInfo>), StatusCodes.Status200OK)]
        [HttpGet]
        [Route("GetSBUListbasedONClient")]
        public async Task<MessageResponse<IEnumerable<BESBUInfo>>> GetSBUListbasedONClient([FromQuery] int clientID)
        {
            var result = new MessageResponse<IEnumerable<BESBUInfo>>();
            try
            {
                var sbuList = await Task.Run(() => _repositorySBU.GetSBUListbasedONClient(clientID, base.oTenant));
                result.Data = sbuList;
                result.TotalCount = sbuList.Count;
            }
            catch (Exception ex)
            {
                result.Message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }

        [ProducesResponseType(typeof(List<BEClientInfo>), StatusCodes.Status200OK)]
        [HttpGet]
        [Route("GetClient")]
        public async Task<MessageResponse<List<BEClientInfo>>> SearchClient([FromQuery] string? searchText, [FromQuery] bool isActive)
        {
            var result = new MessageResponse<List<BEClientInfo>>();
            try
            {
                await Task.Run(() =>
                {
                    if (searchText == null)
                    {
                        result.Data = _repositoryClient.GetClientList(oUser.iUserID, searchText, isActive, oTenant);
                    }
                    else
                    {
                        result.Data = _repositoryClient.GetClientList(oUser.iUserID, searchText.ToString().Trim(), isActive, base.oTenant);
                    }
                });
                result.TotalCount = result.Data.Count;
            }
            catch (Exception ex)
            {
                result.Message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }

        [ProducesResponseType(typeof(List<BEClientInfo>), StatusCodes.Status200OK)]
        [HttpGet]
        [Route("GetClientById")]
        public async Task<MessageResponse<BEClientInfo>> GetClientById([FromQuery] int clientID)
        {
            var result = new MessageResponse<BEClientInfo>();
            try
            {
                var clientList = await Task.Run(() => _repositoryClient.GetClientList(clientID, base.oTenant));
                result.Data = clientList.Select(x => new BEClientInfo()
                {
                    bDisabled = x.bDisabled,
                    bEXLSpecific = x.bEXLSpecific,
                    dCreateDate = x.dCreateDate,
                    dModifyDate = x.dModifyDate,
                    iClientID = clientID,
                    iVerticalID = x.iVerticalID,
                    sClientName = x.sClientName,
                    sClientDescription = x.sClientDescription,
                    iCreatedBy = x.iCreatedBy,
                    iModifiedBy = x.iModifiedBy
                }).FirstOrDefault();
                result.TotalCount = clientList.Count;
            }
            catch (Exception ex)
            {
                result.Message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }

        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [HttpPost]
        [Route("AddClient")]
        public async Task<MessageResponse<string>> AddClient([FromBody] ClientModel ObjClient)
        {
            var result = new MessageResponse<string>();
            try
            {
                await Task.Run(() => _repositoryClient.InsertData(CatchRecord(ObjClient), 10, base.oTenant));
                await Task.Run(() => _repositorySBU.InsertDataSBU(CatchRecordSbu(ObjClient), 10, base.oTenant));
                result.Data = "save";
            }
            catch (Exception ex)
            {
                result.Message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }

        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [HttpPut]
        [Route("UpdateClient")]
        public async Task<MessageResponse<string>> UpdateClient([FromBody] ClientModel ObjClient)
        {
            var result = new MessageResponse<string>();
            try
            {
                await Task.Run(() => _repositoryClient.UpdateData(CatchRecord(ObjClient), 10, base.oTenant));
                await Task.Run(() => _repositorySBU.UpdateDataSBU(CatchRecordSbuUpdate(ObjClient), 10, base.oTenant));
                result.Data = "update";
            }
            catch (Exception ex)
            {
                result.Message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }

        [NonAction]
        private BEClientInfo CatchRecord(ClientModel ObjClient)
        {
            BEClientInfo objClientEntity = new BEClientInfo();

            objClientEntity.iClientID = ObjClient.ClientID;
            objClientEntity.sClientName = ObjClient.ClientName;
            objClientEntity.iVerticalID = ObjClient.VerticalID;
            objClientEntity.sClientDescription = ObjClient.Description;
            objClientEntity.bEXLSpecific = ObjClient.EXLSpecificClient;
            objClientEntity.bDisabled = ObjClient.Disabled;
            objClientEntity.iCreatedBy = base.oUser.iUserID;
            objClientEntity.iModifiedBy = base.oUser.iUserID;
            return objClientEntity;
        }

        [NonAction]
        private BESBUInfo CatchRecordSbu(ClientModel ObjClient)
        {
            BLSBUInfo objSbu = new BLSBUInfo();
            BESBUInfo objSbuClient = new BESBUInfo();
            int getMaxClientId = objSbu.GetMaxClientId(base.oTenant);

            objSbuClient.iSBUID = getMaxClientId;

            DataTable dtSBA = new DataTable();
            dtSBA.Columns.Add("CLIENTID", System.Type.GetType("System.Int32"));
            dtSBA.Columns.Add("SBUID", System.Type.GetType("System.Int32"));
            dtSBA.Columns.Add("DISABLED", System.Type.GetType("System.Boolean"));
            if (ObjClient.ListSBU != null)
            {
                var EnumchkSelected = ObjClient.ListSBU.GetEnumerator();
                while (EnumchkSelected.MoveNext())
                {
                    DataRow dr = dtSBA.NewRow();
                    dr["CLIENTID"] = getMaxClientId;
                    dr["SBUID"] = Convert.ToInt16(EnumchkSelected.Current);
                    dr["DISABLED"] = 0;
                    dtSBA.Rows.Add(dr);
                }
            }
            objSbuClient.dtClientSBUMap = dtSBA;
            objSbuClient.iCreatedBy = base.oUser.iUserID;
            return objSbuClient;
        }
        [NonAction]
        private BESBUInfo CatchRecordSbuUpdate(ClientModel objClient)
        {
            BESBUInfo objSBU = new BESBUInfo();

            DataTable dtSBU = new DataTable();
            dtSBU.Columns.Add("CLIENTID", System.Type.GetType("System.Int32"));
            dtSBU.Columns.Add("SBUID", System.Type.GetType("System.Int32"));
            dtSBU.Columns.Add("Disabled", System.Type.GetType("System.Boolean"));
            if (objClient.ListSBU != null)
            {
                var EnumchkSelected = objClient.ListSBU.GetEnumerator();
                while (EnumchkSelected.MoveNext())
                {
                    DataRow dr = dtSBU.NewRow();
                    dr["CLIENTID"] = objClient.ClientID;
                    dr["SBUID"] = Convert.ToInt16(EnumchkSelected.Current);
                    dr["DISABLED"] = 0;
                    dtSBU.Rows.Add(dr);
                }
            }
            objSBU.dtClientSBUMap = dtSBU;
            objSBU.iCreatedBy = base.oUser.iUserID;
            return objSBU;
        }
    }
}
