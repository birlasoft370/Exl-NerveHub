using BPA.AppConfig.BusinessEntity.ExternalRef.WorkAllocation;
using BPA.AppConfig.ServiceContracts.ServiceContracts.ExternalRef.WorkAllocation;
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
    public class CampTermMappingController : BaseController<CampTermMappingController>
    {
        private readonly ITerminationCodeService _repositoryTerminationCode;
        private readonly ICampTermCodeMappingService _repositoryCampTermCodeMapping;
        private readonly IStoreService _repositoryStore;
        public CampTermMappingController(ILogger<CampTermMappingController> logger, IWebHostEnvironment env, IConfiguration configuration,
            ITerminationCodeService repositoryTerminationCode, ICampTermCodeMappingService repositoryCampTermCodeMapping, IStoreService repositoryStore) : base(logger, env, configuration)
        {
            _repositoryTerminationCode = repositoryTerminationCode;
            _repositoryCampTermCodeMapping = repositoryCampTermCodeMapping;
            _repositoryStore = repositoryStore;
        }

        [ProducesResponseType(typeof(CampTermMappingModel), StatusCodes.Status200OK)]
        [HttpGet]
        [Route("GetCampTermMappedDetailsByCampaignId")]
        public async Task<MessageResponse<CampTermMappingModel>> GetCampTermMappedDetailsByCampaignId([FromQuery] int campaignID)
        {
            var result = new MessageResponse<CampTermMappingModel>();
            CampTermMappingModel oCampTermMappingModel = new CampTermMappingModel();
            List<TermCodeList> GridTermCodeMap = new List<TermCodeList>();
            try
            {
                var GetTermCodeList = this._repositoryTerminationCode.GetTermCodeListByCamp(Convert.ToInt32(campaignID), base.oTenant);
                oCampTermMappingModel.CampaignID = campaignID;
                for (int GetTermCodeCount = 0; GetTermCodeCount < GetTermCodeList.Count; GetTermCodeCount++)
                {
                    TermCodeList obj = new();
                    obj.TerminationID = GetTermCodeList[GetTermCodeCount].iTerminationCodeID;
                    obj.TerminatioName = GetTermCodeList[GetTermCodeCount].sTermCodeName;
                    GridTermCodeMap.Add(obj);
                }

                var ClientProcess = _repositoryStore.GetClientProcessList(oCampTermMappingModel.CampaignID, base.oTenant);
                for (int ClientProcessCount = 0; ClientProcessCount < ClientProcess.Count; ClientProcessCount++)
                {
                    oCampTermMappingModel.ClientID = ClientProcess[ClientProcessCount].ClientID;
                    oCampTermMappingModel.ProcessID = ClientProcess[ClientProcessCount].ProcessID;
                    //oCampTermMappingModel.CampaignName = oCampTermMappingViewModel.ClientProcess[ClientProcessCount].CampaignID;
                    oCampTermMappingModel.CampaignID = ClientProcess[ClientProcessCount].CampaignID;
                }

                var BECampTermCodeMapping = await Task.Run(() => _repositoryCampTermCodeMapping.GetCampTermMappedDetails(campaignID, oTenant));

                for (int dtTerminationCount = 0; dtTerminationCount < BECampTermCodeMapping.dtTerminationTable.Rows.Count; dtTerminationCount++)
                {
                    for (int GridTermCodeMapCount = 0; GridTermCodeMapCount < GridTermCodeMap.Count; GridTermCodeMapCount++)
                    {
                        if ((int.Parse(GridTermCodeMap[GridTermCodeMapCount].TerminationID.ToString())) == (int.Parse(BECampTermCodeMapping.dtTerminationTable.Rows[dtTerminationCount]["TerminationID"].ToString())))
                        {
                            GridTermCodeMap[GridTermCodeMapCount].Selected = 1;
                            if (BECampTermCodeMapping.dtTerminationTable.Rows[dtTerminationCount]["IsProductive"] != DBNull.Value)
                                GridTermCodeMap[GridTermCodeMapCount].IsProductive = Convert.ToInt16((BECampTermCodeMapping.dtTerminationTable.Rows[dtTerminationCount]["IsProductive"].ToString()));

                            if (BECampTermCodeMapping.dtTerminationTable.Rows[dtTerminationCount]["IsEnd"] != DBNull.Value)
                                GridTermCodeMap[GridTermCodeMapCount].IsEnd = Convert.ToInt16((BECampTermCodeMapping.dtTerminationTable.Rows[dtTerminationCount]["IsEnd"].ToString()));

                            if (BECampTermCodeMapping.dtTerminationTable.Rows[dtTerminationCount]["Disabled"] != DBNull.Value)
                                GridTermCodeMap[GridTermCodeMapCount].Disabled = Convert.ToInt16((BECampTermCodeMapping.dtTerminationTable.Rows[dtTerminationCount]["Disabled"].ToString()));
                        }
                    }
                }

                oCampTermMappingModel.GridTermCodeMapList = GridTermCodeMap;

                result.Data = oCampTermMappingModel;
            }
            catch (Exception ex)
            {
                result.Message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }

        [ProducesResponseType(typeof(IList<BETerminationCodeInfo>), StatusCodes.Status200OK)]
        [HttpGet]
        [Route("FillERPGridWithSearch")]
        public async Task<MessageResponse<IList<BETerminationCodeInfo>>> FillERPGridWithSearch([FromQuery] int campaignID, [FromQuery] string TerminationName, [FromQuery] string? sltTerminationID)
        {
            var result = new MessageResponse<IList<BETerminationCodeInfo>>();
            IList<BETerminationCodeInfo> GetTerminationList = null;
            try
            {
                GetTerminationList = _repositoryTerminationCode.GetTermCodeListByCamp(TerminationName, campaignID, base.oTenant);
                if (sltTerminationID != "" && sltTerminationID is not null)
                {
                    IList<string> lstRemoveData;
                    lstRemoveData = sltTerminationID.Split(',').ToList();
                    GetTerminationList = (from a in GetTerminationList
                                          where !lstRemoveData.Contains(a.iTerminationCodeID.ToString())
                                          select a).ToList();
                }

                result.Data = GetTerminationList;
                result.TotalCount = GetTerminationList.Count;
            }
            catch (Exception ex)
            {
                result.Message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }


        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [HttpPost]
        [Route("AddCampTermCodeMapping")]
        public async Task<MessageResponse<string>> AddCampTermCodeMapping([FromBody] CampTermMappingModel objCampTermMap)
        {
            var result = new MessageResponse<string>();
            try
            {
                BECampTermCodeMapping objBECampTermCodeMapping = CatchRecord(objCampTermMap);
                await Task.Run(() => _repositoryCampTermCodeMapping.InsertData(objBECampTermCodeMapping, 49, base.oTenant));
                result.Data = "display_Save_Message";
                objBECampTermCodeMapping = null;
            }
            catch (Exception ex)
            {
                result.Message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [HttpPut]
        [Route("UpdateCampTermCodeMapping")]
        public async Task<MessageResponse<string>> UpdateCampTermCodeMapping([FromBody] CampTermMappingModel objCampTermMap)
        {
            var result = new MessageResponse<string>();
            try
            {
                BECampTermCodeMapping objBECampTermCodeMapping = CatchRecord(objCampTermMap);
                await Task.Run(() => _repositoryCampTermCodeMapping.UpdateData(objBECampTermCodeMapping, 49, base.oTenant));
                result.Data = "display_Save_Message";
                objBECampTermCodeMapping = null;
            }
            catch (Exception ex)
            {
                result.Message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }

        [NonAction]
        private BECampTermCodeMapping CatchRecord(CampTermMappingModel objCampTermMap)
        {
            using (BECampTermCodeMapping objInfo = new BECampTermCodeMapping())
            {
                // objCampTermMap.GetTermCodeList = _repositoryTerminationCode.GetTermCodeList(true, base.oTenant);
                objInfo.iCampaignID = Convert.ToInt16(objCampTermMap.CampaignID);
                objInfo.iCreatedBy = base.oUser.iUserID;
                DataTable dt = new DataTable();
                dt.Columns.Add("TerminationId", System.Type.GetType("System.Int32"));
                dt.Columns.Add("IsProductive", System.Type.GetType("System.String"));
                dt.Columns.Add("IsEnd", System.Type.GetType("System.String"));
                dt.Columns.Add("Disabled", System.Type.GetType("System.String"));

                foreach (var item in objCampTermMap.GridTermCodeMapList)
                {
                    DataRow dr = dt.NewRow();
                    dr["TerminationId"] = Convert.ToInt16(item.TerminationID);
                    dr["IsProductive"] = item.IsProductive == 1 ?true:false;
                    dr["IsEnd"] = item.IsEnd == 1 ? true : false;
                    dr["Disabled"] = item.Disabled == 1 ? true : false;
                    dt.Rows.Add(dr);
                }

                objInfo.dtTerminationTable = dt;
                if (objCampTermMap.CampaignID == 0)
                {
                    objInfo.iCreatedBy = base.oUser.iUserID;
                }
                else
                {
                    objInfo.iModifiedBy = base.oUser.iUserID;
                }
                return objInfo;
            }
        }
    }
}
