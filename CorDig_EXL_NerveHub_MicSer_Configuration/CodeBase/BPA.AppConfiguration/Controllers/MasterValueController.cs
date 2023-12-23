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
using System.Reflection;

namespace BPA.AppConfiguration.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [JwtAuthentication]
    public class MasterValueController : BaseController<MasterValueController>
    {
        private readonly IMasterTableService _repositoryMasterTable;

        public MasterValueController(ILogger<MasterValueController> logger, IWebHostEnvironment env, IConfiguration configuration,
            IMasterTableService repositoryMasterTable) : base(logger, env, configuration)
        {
            _repositoryMasterTable = repositoryMasterTable;
        }

        [ProducesResponseType(typeof(List<MasterValueModel>), StatusCodes.Status200OK)]
        [HttpGet]
        [Route("GetMasterList")]
        public async Task<MessageResponse<List<MasterValueModel>>> GetMasterList([FromQuery] string? masterValueSearchName)
        {
            var result = new MessageResponse<List<MasterValueModel>>();
            List<MasterValueModel> oMasterValueModel = new List<MasterValueModel>();

            try
            {
                var masterTypeValueList = await Task.Run(() =>
                _repositoryMasterTable.GetMasterList(Encoder.HtmlEncode(masterValueSearchName), true, base.oTenant));

                foreach (var item in masterTypeValueList)
                {
                    oMasterValueModel.Add(new MasterValueModel { MasterValueID = item.iFieldId, MasterType = item.sFieldDescription });
                }
                result.Data = oMasterValueModel;
                result.TotalCount = oMasterValueModel.Count;
            }
            catch (Exception ex)
            {
                result.Message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }

        [ProducesResponseType(typeof(MasterValueModel), StatusCodes.Status200OK)]
        [HttpGet]
        [Route("GetMasterDetails")]
        public async Task<MessageResponse<MasterValueModel>> GetMasterDetails([FromQuery] int masterValueID)
        {
            var result = new MessageResponse<MasterValueModel>();
            MasterValueModel objMasterViewModel = new MasterValueModel();

            try
            {
                var masterTypeValueList = await Task.Run(() =>
                objMasterViewModel = this.DisplayRecord(this._repositoryMasterTable.GetMasterDetails(masterValueID, base.oTenant)));

                result.Data = objMasterViewModel;
            }
            catch (Exception ex)
            {
                result.Message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }

        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [HttpPost]
        [Route("AddMasterTypeValue")]
        public async Task<MessageResponse<string>> AddMasterTypeValue([FromBody] MasterValueModel model)
        {
            var result = new MessageResponse<string>();

            string strParameter = string.Empty;
            string FieldID = string.Empty;
            string Values = string.Empty;
            string Disable = string.Empty;

            try
            {
                strParameter = model.MasterValueID + ";" + model.MasterType + ";" + model.Disabled;
                for (var i = 0; i < model.ValueList.Count; i++)
                {
                    FieldID = FieldID + ";" + model.ValueList[i].FieldID;
                    Values = Values + ";" + model.ValueList[i].Values;
                    Disable = Disable + ";" + model.ValueList[i].Disabled;
                }
                await Task.Run(() => _repositoryMasterTable.InsertData(CatchRecord(strParameter, FieldID, Values, Disable, base.oUser.iUserID), 159, base.oTenant));
                result.Data = "disp_SaveMasterValue";
            }
            catch (Exception ex)
            {
                result.Message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }

        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [HttpPut]
        [Route("UpdateMasterTypeValue")]
        public async Task<MessageResponse<string>> UpdateMasterTypeValue([FromBody] MasterValueModel model)
        {
            var result = new MessageResponse<string>();

            string strParameter = string.Empty;
            string FieldID = string.Empty;
            string Values = string.Empty;
            string Disable = string.Empty;

            try
            {
                strParameter = model.MasterValueID + ";" + model.MasterType + ";" + model.Disabled;
                for (var i = 0; i < model.ValueList.Count; i++)
                {
                    FieldID = FieldID + ";" + model.ValueList[i].FieldID;
                    Values = Values + ";" + model.ValueList[i].Values;
                    Disable = Disable + ";" + model.ValueList[i].Disabled;
                }
                await Task.Run(() => _repositoryMasterTable.UpdateData(CatchRecord(strParameter, FieldID, Values, Disable, base.oUser.iUserID), 159, base.oTenant));
                result.Data = "disp_UpdatedMasterValue";
            }
            catch (Exception ex)
            {
                result.Message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }

        [NonAction]
        private BEMasterType CatchRecord(string strParameter, string FieldID, string Values, string Disable, int UserId)
        {
            BEMasterType objMasterType = new BEMasterType();

            string[] cstrParameter = null;
            string[] cMasterId = null;
            string[] cValues = null;
            if (strParameter != null)
            {
                cstrParameter = strParameter.Split(';');
            }
            if (FieldID != null)
            {
                cMasterId = FieldID.Split(';');
            }
            if (Values != null)
            {
                cValues = Values.Split(';');
            }


            string[] cDisable = Disable.Split(';');

            if (cstrParameter[0] != null)
                objMasterType.iFieldId = Convert.ToInt32(cstrParameter[0]);

            objMasterType.sFieldDescription = Encoder.HtmlEncode(cstrParameter[1]);
            objMasterType.bDisabled = Convert.ToBoolean(cstrParameter[2]) == false ? false : true;
            objMasterType.iCreatedBy = UserId;
            BEMasterTable oMasterTable = null;
            BEMasterType oMasterType = null;

            if (cMasterId != null)
            {

                oMasterType = this._repositoryMasterTable.GetMasterDetails(Convert.ToInt32(cstrParameter[0]), base.oTenant);
                if (cValues != null)
                {
                    for (int i = 1; i <= cValues.Length - 1; i++)
                    {

                        if (Convert.ToInt32(cstrParameter[0]) == 0)
                        {
                            oMasterTable = new BEMasterTable();
                            oMasterTable.iMasterId = cMasterId[i] == null ? 0 : Convert.ToInt32(cMasterId[i]);// remove null
                            oMasterTable.sValue = Encoder.HtmlEncode(cValues[i]);
                            if (cDisable[i] != "null")
                            {
                                oMasterTable.bDisabled = Convert.ToBoolean(cDisable[i]);
                            }
                            oMasterTable.oRowState = RowState.NEW;
                            oMasterTable.iCreatedBy = UserId;
                            objMasterType.lMasterTable.Add(oMasterTable);
                            oMasterTable = null;
                        }
                        else
                        {
                            if (Convert.ToInt32(cMasterId[i]) != null && Convert.ToInt32(cMasterId[i]) > 0)
                            {
                                oMasterTable = new BEMasterTable();
                                oMasterTable.iMasterId = Convert.ToInt32(cMasterId[i]);
                                oMasterTable.sValue = Encoder.HtmlEncode(cValues[i]);
                                if (cDisable[i] != "null")
                                {
                                    oMasterTable.bDisabled = Convert.ToBoolean(cDisable[i]);
                                }
                                oMasterTable.oRowState = RowState.UPDATED;
                                oMasterTable.iModifiedBy = UserId;
                                objMasterType.lMasterTable.Add(oMasterTable);
                                oMasterTable = null;
                            }
                            else
                            {
                                oMasterTable = new BEMasterTable();
                                oMasterTable.iMasterId = Convert.ToInt32(cMasterId[i]);
                                oMasterTable.sValue = Encoder.HtmlEncode(cValues[i]);
                                if (cDisable[i] != "null")
                                {
                                    oMasterTable.bDisabled = Convert.ToBoolean(cDisable[i]);
                                }
                                oMasterTable.oRowState = RowState.NEW;
                                oMasterTable.iModifiedBy = UserId;
                                objMasterType.lMasterTable.Add(oMasterTable);
                                oMasterTable = null;
                            }
                        }

                        if (i == cValues.Length - 1)
                        {
                            if (Convert.ToInt32(cstrParameter[0]) != 0)
                            {
                                oMasterType.lMasterTable = oMasterType.lMasterTable.Where(x => !cMasterId.Contains(x.iMasterId.ToString())).ToList();
                                if (oMasterType.lMasterTable.Count > 0)
                                {
                                    foreach (var item in oMasterType.lMasterTable)
                                    {
                                        oMasterTable = new BEMasterTable();
                                        oMasterTable.iMasterId = item.iMasterId;
                                        oMasterTable.sValue = Encoder.HtmlEncode(item.sValue);
                                        oMasterTable.bDisabled = item.bDisabled;
                                        oMasterTable.oRowState = RowState.DELETED;
                                        oMasterTable.iModifiedBy = UserId;
                                        objMasterType.lMasterTable.Add(oMasterTable);
                                        oMasterTable = null;
                                    }
                                }
                            }
                        }

                    }
                }
                else
                {

                    if (oMasterType.lMasterTable.Count > 0)
                    {
                        foreach (var item in oMasterType.lMasterTable)
                        {
                            oMasterTable = new BEMasterTable();
                            oMasterTable.iMasterId = item.iMasterId;
                            oMasterTable.sValue = Encoder.HtmlEncode(item.sValue);
                            oMasterTable.bDisabled = item.bDisabled;
                            oMasterTable.oRowState = RowState.DELETED;
                            oMasterTable.iModifiedBy = UserId;
                            objMasterType.lMasterTable.Add(oMasterTable);
                            oMasterTable = null;
                        }
                    }
                }
            }
            return objMasterType;
        }

        [NonAction]
        private MasterValueModel DisplayRecord(BEMasterType oBEMasterType)
        {
            MasterValueModel oMasterValueModel = new MasterValueModel();
            oMasterValueModel.MasterValueID = oBEMasterType.iFieldId;
            oMasterValueModel.MasterType = oBEMasterType.sFieldDescription;
            oMasterValueModel.Disabled = oBEMasterType.bDisabled;
            foreach (var item in oBEMasterType.lMasterTable)
            {
                oMasterValueModel.ValueList.Add(new MasterValueLists { FieldID = item.iMasterId, Values = item.sValue, Disabled = item.bDisabled });
            }
            return oMasterValueModel;
        }
    }
}
