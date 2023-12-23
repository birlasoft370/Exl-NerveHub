using BPA.GlobalResources;
using BPA.GlobalResources.UI.AppConfiguration;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Security.Application;
using MicUI.WorkManagement.Helper;
using MicUI.WorkManagement.Models.ViewModels;
using MicUI.WorkManagement.Module.Common;
using MicUI.WorkManagement.Module.WorkManagement.WorkMaster;
using MicUI.WorkManagement.Services.ServiceModel;
using Newtonsoft.Json;
using System.Data;

namespace MicUI.WorkManagement.Controllers
{
    public class WorkDefinitionController : BaseController
    {
        private readonly IClientService _repositoryClient;
        private readonly IProcessService _repositoryProcess;
        private readonly ICampaignService _repositoryCampaign;
        private readonly ILanguagesService _languagesService;
        private readonly IWorkObjectService _repositoryWork;
        private readonly IControlTypeService _repositoryControlType;
        private readonly IStoreService _repositoryStore;
        private readonly IDocDetailService _repositoryDocDetail;
        private readonly IAgentDashBoardService _repositoryAgentDashBoard;
        private readonly IWorkLayoutService _repositoryWorkLayout;
        string strError = string.Empty;
        public WorkDefinitionController(IClientService repositoryClient, IProcessService repositoryProcess, ICampaignService repositoryCampaign, ILanguagesService languagesService, IWorkObjectService repositoryWork, IControlTypeService repositoryControlType, IStoreService repositoryStore, IDocDetailService repositoryDocDetail, IAgentDashBoardService repositoryAgentDashBoard, IWorkLayoutService repositoryWorkLayout)
        {
            _repositoryClient = repositoryClient;
            _repositoryProcess = repositoryProcess;
            _repositoryCampaign = repositoryCampaign;
            _languagesService = languagesService;
            _repositoryWork = repositoryWork;
            _repositoryControlType = repositoryControlType;
            _repositoryStore = repositoryStore;
            _repositoryDocDetail = repositoryDocDetail;
            _repositoryAgentDashBoard = repositoryAgentDashBoard;
            _repositoryWorkLayout = repositoryWorkLayout;
        }

        [HttpGet]
        public IActionResult Index(string key)
        {
            // TempData["HiddenFormula"] = "Hai";
            // check();
            // TempData["iStoreId"] = "2376";
            ////it's Check the User Permission
            if (!base.CheckViewPermission(int.Parse(Resources_FormID.WorkDefinition)))
            {
                return RedirectToActionPermanent("AccessDenied", "Error");
            }
            //objWorkList = null;
            CreateRESTControls(0);

            if (key == "s") { TempData["GridObjControllName"] = null; TempData["ChoiceDataTempGRD"] = null; TempData["lstTABList"] = null; }
            WorkDefinitionViewModel oWorkObject = new WorkDefinitionViewModel();
            if (oTenant != null)
            {
                if (bool.Parse(oTenant.ClientMultiLanguage))
                {
                    var providerName = _languagesService.GetCampaignWiseLangList(-1, 2);
                    if (providerName.Count > 0)
                    {
                        foreach (BEMailTranslatorConfiguration item in providerName)
                        {
                            oWorkObject.ApiKey = item.ApiKey;
                            oWorkObject.ApiUrl = item.ApiUrl;
                            break;
                        }
                    }
                }
            }

            oWorkObject.oTenant = base.oTenant;
            oWorkObject.GridControlF = null;
            if (Convert.ToString(TempData["iStoreId"]) == "")
            {
                TempData["_SearchID"] = 0;
                TempData["bmodify"] = false;
                TempData["lstTABList"] = null;
                //TempData.Keep("bmodify");
                WorkObject AddNewBlankRow = new WorkObject();
                if (TempData["GridFlag"] != null)
                {
                    //if (TempData["iClientID"] != null && TempData["iProcessID"] != null && TempData["iCampaignID"] != null)
                    //{
                    //    oWorkObject.ClientName = Convert.ToString(TempData["iClientID"]);
                    //    oWorkObject.ProcessName = Convert.ToString(TempData["iProcessID"]);
                    //    oWorkObject.CampaignName = Convert.ToString(TempData["iCampaignID"]);
                    //    //TempData.Keep("iClientID");
                    //    //TempData.Keep("iProcessID");
                    //    //TempData.Keep("iCampaignID");
                    //}
                    oWorkObject.GridControlF = "RG";
                    TempData["GridFlag"] = null;
                    TempData["iClientID"] = null;
                    TempData["iProcessID"] = null;
                    TempData["iCampaignID"] = null;
                }
                //add new row in a grid
                oWorkObject.WorkDefinition.Add(AddNewBlankRow);
            }
            else
            {
                TempData["_SearchID"] = int.Parse(Convert.ToString(TempData["iStoreId"]));
                TempData["bmodify"] = true;
                //TempData.Keep("bmodify");
                //display data in view
                oWorkObject = DisplayControl(int.Parse(Convert.ToString(TempData["iStoreId"])), oWorkObject);
            }

            return View(oWorkObject);
        }

        private WorkDefinitionViewModel DisplayControl(int StoreID, WorkDefinitionViewModel objWorkModel)
        {
            // WorkDefinitionViewModel objWorkModel = new WorkDefinitionViewModel();
            objWorkModel.oTenant = base.oTenant;
            BEStoreInfo _ostoreInfo = new BEStoreInfo();
            try
            {
                var lst = _repositoryStore.GetStoreListByStoreId(StoreID);
                if (lst.Count > 0)
                {
                    _ostoreInfo = lst[0];// _repositoryStore.GetStoreListByStoreId(StoreID)[0];
                    if (_ostoreInfo != null && _ostoreInfo.iStoreId > 0)
                    {
                        //Display Store Data
                        List<BECampaignClientProcess> lstCampaignClientProcess = _repositoryStore.GetClientProcessList(int.Parse(_ostoreInfo.iCampId.ToString()));
                        objWorkModel.ClientName = Convert.ToString(lstCampaignClientProcess[0].ClientID);
                        objWorkModel.ProcessName = Convert.ToString(lstCampaignClientProcess[0].ProcessID);
                        objWorkModel.CampaignName = Convert.ToString(lstCampaignClientProcess[0].CampaignID);
                        objWorkModel.IsForStaging = _ostoreInfo.bIsForStaging;
                        objWorkModel.WorkDefinitionName = _ostoreInfo.sStoreName;
                        objWorkModel.Description = _ostoreInfo.sStoreDescription;
                        objWorkModel.NoOfRows = _ostoreInfo.iRows;
                        objWorkModel.NoOfColumns = Convert.ToString(_ostoreInfo.iColumns);
                        objWorkModel.DisableWork = _ostoreInfo.bDisableWork;
                        objWorkModel.GenerateLetter = _ostoreInfo.bGenerateLetter;
                        objWorkModel.IsDistributionBot = _ostoreInfo.bDistributionBot;
                        objWorkModel.bIsRunTimeUploadRequired = _ostoreInfo.bRunTimeUploadRequired;
                        objWorkModel.IncreaseSearch = _ostoreInfo.iIncreaseSearch.ToString();
                        objWorkModel.ErrorSnapshot = _ostoreInfo.bErrorSnapshot;
                        objWorkModel.ErrorDuration = Convert.ToString(_ostoreInfo.iErrorDuration);
                        objWorkModel.IsEmail = _ostoreInfo.bIsEmail;
                        objWorkModel.TABMapping = _ostoreInfo.bTABMapping;
                        objWorkModel.IsGridConfiguration = _ostoreInfo.bGridObject;
                        List<BEWorkObjectTAB> lstMasterTab = new List<BEWorkObjectTAB>();
                        if (_ostoreInfo.bTABMapping == true)
                        {
                            if (TempData["lstTABList"] == null)
                            {
                                lstMasterTab = _repositoryStore.GetTabMasterList(StoreID).ToList();
                                objWorkModel.AddTAB = lstMasterTab;
                                // TempData["lstTABList"] = null;
                                TempData["lstTABList"] = lstMasterTab;
                                TempData.Keep("lstTABList");
                            }
                            else
                            {
                                lstMasterTab = (List<BEWorkObjectTAB>)TempData["lstTABList"];
                                objWorkModel.AddTAB = lstMasterTab;
                                TempData.Keep("lstTABList");
                            }
                        }
                        if (_ostoreInfo.bGridObject == true)
                        {
                            //  DisplayGridConfiguration(_ostoreInfo.bGridObject, StoreID);
                            // objWorkModel.ls = lstMasterTab;
                        }

                        //Display  Grid data in  view
                        for (int i = 0; i < _ostoreInfo.oWorkObject.Count(); i++)
                        {
                            WorkObject ObjItemAdd = new WorkObject();
                            ObjItemAdd.ISExistingRow = "YES";
                            ObjItemAdd.iObjectID = _ostoreInfo.oWorkObject[i].iObjectID;
                            ObjItemAdd.iStoreID = _ostoreInfo.oWorkObject[i].iStoreID;
                            ObjItemAdd.sObjectName = _ostoreInfo.oWorkObject[i].sObjectName;
                            ObjItemAdd.bWorkID = _ostoreInfo.oWorkObject[i].bWorkID;
                            ObjItemAdd.sObjectDescription = _ostoreInfo.oWorkObject[i].sObjectDescription;
                            ObjItemAdd.sObjectLabel = _ostoreInfo.oWorkObject[i].sObjectLabel;
                            //Selected ObjectType Value.
                            ObjItemAdd.iObjectType = _ostoreInfo.oWorkObject[i].iObjectType;
                            foreach (var BEControlTypeInfo in (IList<BEControlTypeInfo>)FillObjectType("dfdf").Value)
                            {
                                if (BEControlTypeInfo.iControlTypeID == _ostoreInfo.oWorkObject[i].iObjectType)
                                {
                                    ObjItemAdd.selectControlType.iControlTypeID = BEControlTypeInfo.iControlTypeID;
                                    ObjItemAdd.selectControlType.sControlType = BEControlTypeInfo.sControlType;
                                }
                            }
                            //DataType set in row in a grid
                            ObjItemAdd.sDataType = _ostoreInfo.oWorkObject[i].sDataType;
                            ObjItemAdd.selectedDataType.Text = _ostoreInfo.oWorkObject[i].sDataType;
                            ObjItemAdd.selectedDataType.Value = _ostoreInfo.oWorkObject[i].sDataType;
                            ObjItemAdd.iLength = _ostoreInfo.oWorkObject[i].iLength;
                            ObjItemAdd.iLengthReadonly = _ostoreInfo.oWorkObject[i].iLength;
                            //fill Choice Data
                            ObjItemAdd.oChoice = _ostoreInfo.oWorkObject[i].oChoice;
                            //objWorkModel.Choice = item.oChoice;

                            //Set Validation in a grid
                            ObjItemAdd.iValidationID = _ostoreInfo.oWorkObject[i].iValidationID;
                            foreach (var BEValidations in (IList<BEValidations>)FillValidations().Value)
                            {
                                if (BEValidations.ValidationId == _ostoreInfo.oWorkObject[i].iValidationID)
                                {
                                    ObjItemAdd.selectedValidation.ValidationId = BEValidations.ValidationId;
                                    ObjItemAdd.selectedValidation.ValidationType = BEValidations.ValidationType;
                                }
                            }
                            ObjItemAdd.bVisible = _ostoreInfo.oWorkObject[i].bVisible;//Set textbox visibility in gent dashboard
                            ObjItemAdd.bSearch = _ostoreInfo.oWorkObject[i].bSearch;//Set  Searchable textbox  in gent dashboard
                            ObjItemAdd.bEditable = _ostoreInfo.oWorkObject[i].bEditable;//Set  Editable textbox  in gent dashboard
                            ObjItemAdd.bRequired = _ostoreInfo.oWorkObject[i].bRequired;//Set  Required textbox  in gent dashboard
                            ObjItemAdd.bDisabled = _ostoreInfo.oWorkObject[i].bDisabled;//Set  Disabled textbox  in gent dashboard
                            ObjItemAdd.bUniqueID = _ostoreInfo.oWorkObject[i].bUniqueID;//Set  UniqueID textbox  in gent dashboard
                            ObjItemAdd.bTransactionType = _ostoreInfo.oWorkObject[i].bTransactionType;
                            ObjItemAdd.bLANID = _ostoreInfo.oWorkObject[i].bEmpIdLanId;
                            ObjItemAdd.bIsUpload = _ostoreInfo.oWorkObject[i].bIsUpload;
                            ObjItemAdd.bIsReport = _ostoreInfo.oWorkObject[i].bIsReport;
                            ObjItemAdd.bCustomerIdentifier = _ostoreInfo.oWorkObject[i].bCustomerIdentifier;
                            ObjItemAdd.iIsReportOrder = _ostoreInfo.oWorkObject[i].iIsReportOrder;
                            ObjItemAdd.irow_No = _ostoreInfo.oWorkObject[i].irow_No;
                            ObjItemAdd.iRowNumber = _ostoreInfo.oWorkObject[i].irow_No;
                            ObjItemAdd.selectedRow.Text = Convert.ToString(_ostoreInfo.oWorkObject[i].irow_No);
                            ObjItemAdd.selectedRow.Value = Convert.ToString(_ostoreInfo.oWorkObject[i].irow_No);
                            ObjItemAdd.icolumn_No = _ostoreInfo.oWorkObject[i].icolumn_No;
                            ObjItemAdd.iColumnNumber = _ostoreInfo.oWorkObject[i].icolumn_No;
                            ObjItemAdd.selectedcolumn.Text = Convert.ToString(_ostoreInfo.oWorkObject[i].icolumn_No);
                            ObjItemAdd.selectedcolumn.Value = Convert.ToString(_ostoreInfo.oWorkObject[i].icolumn_No);

                            ObjItemAdd.iIsReportOrder = _ostoreInfo.oWorkObject[i].iIsReportOrder;

                            ObjItemAdd.bSearchableSearch = _ostoreInfo.oWorkObject[i].bSearchableSearch;
                            ObjItemAdd.iReportsOrderSearch = _ostoreInfo.oWorkObject[i].iReportsOrderSearch;

                            ObjItemAdd.iTAB_ID = _ostoreInfo.oWorkObject[i].iTAB_ID;
                            foreach (var BEControlTypeInfo in (IList<BEWorkObjectTAB>)lstMasterTab)
                            {
                                if (BEControlTypeInfo.sChoiceValue == _ostoreInfo.oWorkObject[i].iTAB_ID)
                                {
                                    ObjItemAdd.selectedRowTAB.sChoiceValue = BEControlTypeInfo.sChoiceValue;
                                    ObjItemAdd.selectedRowTAB.sTABNameValue = BEControlTypeInfo.sTABNameValue;
                                }
                            }
                            if (_ostoreInfo.bGridObject == true)
                            {
                                if (_ostoreInfo.oWorkObject[i].iObjectType == 16)
                                {
                                    ObjItemAdd.sGridControlID = _ostoreInfo.oWorkObject[i].sObjectName;
                                }

                                var dsList = _repositoryStore.GetGridObjectName(StoreID);

                                if (dsList.Count > 0)
                                {
                                    foreach (GetGridObjectNameModel GridName in dsList)
                                    {
                                        if (GridName.GridName.ToString() == ObjItemAdd.sGridControlID)
                                        {
                                            ObjItemAdd.selectedGridControlObj.sGridChoiceValue = GridName.GridName.ToString();
                                            ObjItemAdd.selectedGridControlObj.iObjectGridChoiceID = GridName.GridName.ToString();
                                        }
                                    }
                                }

                            }
                            ObjItemAdd.icolumn_Span = _ostoreInfo.oWorkObject[i].icolumn_Span;
                            //ObjItemAdd.iColumn = _ostoreInfo.oWorkObject[i].icolumn_No;
                            ObjItemAdd.selectedcolumnSpan.Text = Convert.ToString(_ostoreInfo.oWorkObject[i].icolumn_Span);
                            ObjItemAdd.selectedcolumnSpan.Value = Convert.ToString(_ostoreInfo.oWorkObject[i].icolumn_Span);

                            ObjItemAdd.bIsTranslate = _ostoreInfo.oWorkObject[i].bIsTranslate;
                            objWorkModel.WorkDefinition.Add(ObjItemAdd);
                        }
                        //Display Benefits Data in popup View
                        List<BEWorkObjectApprover> lstApprover = new List<BEWorkObjectApprover>();
                        lstApprover = _repositoryStore.GetObjectApprovalList(StoreID).ToList();
                        if (lstApprover.Count > 0)
                        {
                            objWorkModel.Location = lstApprover[0].sLocations;
                            objWorkModel.ShiftWindow = lstApprover[0].sShiftwindows;
                            if (lstApprover[0].sPurposeswm)
                                objWorkModel.PurposeofcreationofWork.AddRange(new List<string> { Resources_Campaign.display_WorkManagement });
                            if (lstApprover[0].sPurposetime)
                                objWorkModel.PurposeofcreationofWork.AddRange(new List<string> { Resources_Campaign.display_TimeTracking });
                            if (lstApprover[0].sPurposetrans)
                                objWorkModel.PurposeofcreationofWork.AddRange(new List<string> { Resources_Campaign.display_TransactionsMonitoring });
                            objWorkModel.BusinessJustifications = lstApprover[0].sBusinessJustifications;
                            objWorkModel.Q1 = int.Parse(Convert.ToString(lstApprover[0].sTargetq1));
                            objWorkModel.Q2 = int.Parse(Convert.ToString(lstApprover[0].sTargetq2));
                            objWorkModel.Q3 = int.Parse(Convert.ToString(lstApprover[0].sTargetq3));
                            objWorkModel.Y1 = int.Parse(Convert.ToString(lstApprover[0].sTargety1));
                            objWorkModel.Y2 = int.Parse(Convert.ToString(lstApprover[0].sTargety2));
                            objWorkModel.Y3 = int.Parse(Convert.ToString(lstApprover[0].sTargety3));
                            objWorkModel.KeyBenefits = lstApprover[0].skeybenifits;
                        }
                    }
                }

            }
            catch (Exception et)
            {
                throw;
            }
            return objWorkModel;
        }

        [HttpPost]
        [ValidateAntiForgeryToken] // Cross-Site Request Forgery fixed
        public JsonResult editWorkDefination(string iStoreId)
        {
            TempData["iStoreId"] = iStoreId;
            TempData.Keep("iStoreId");
            return Json("1");
        }

        public ActionResult CreateRESTControls(int WorkId)
        {
            System.Text.StringBuilder ObjWork = new System.Text.StringBuilder();
            if (WorkId != 0)
            {
                IList<BEWorkObject> lstObject;

                lstObject = _repositoryWork.GetObjectList(WorkId, false);

                return PartialView("_CreateRESTControls", lstObject.ToList());
            }
            return Json(string.Empty);
        }
        public JsonResult GetCascadeClient()
        {
            return Json(_repositoryClient.GetClientList(true, ""));
        }
        public JsonResult GetCascadeProcess(int iClientID)
        {
            return Json(_repositoryProcess.GetProcessList(iClientID, "", true));
        }
        public JsonResult GetCascadeCamp(int iProcessID)
        {
            return Json(_repositoryCampaign.GetProcessWiseCampaignList(iProcessID));
        }
        public JsonResult FillNoOfColumns()
        {
            List<SelectListItem> ObjProcessList = new List<SelectListItem>();
            ObjProcessList.Insert(0, (new SelectListItem { Text = "1", Value = "1" }));
            ObjProcessList.Insert(1, (new SelectListItem { Text = "2", Value = "2" }));
            ObjProcessList.Insert(2, (new SelectListItem { Text = "3", Value = "3" }));
            return Json(ObjProcessList);
        }
        public JsonResult FilliErrorDuration()
        {
            List<SelectListItem> ObjProcessList = new List<SelectListItem>();
            ObjProcessList.Insert(0, (new SelectListItem { Text = @BPA.GlobalResources.UI.Resources_common.display_Select, Value = "0" }));
            ObjProcessList.Insert(1, (new SelectListItem { Text = "7", Value = "7" }));
            ObjProcessList.Insert(2, (new SelectListItem { Text = "15", Value = "15" }));
            ObjProcessList.Insert(3, (new SelectListItem { Text = "30", Value = "30" }));
            ObjProcessList.Insert(4, (new SelectListItem { Text = "45", Value = "45" }));
            ObjProcessList.Insert(5, (new SelectListItem { Text = "60", Value = "60" }));
            return Json(ObjProcessList);
        }
        public JsonResult GetTranslateList()
        {
            IList<BELanguages> tempLstLanguages = null;
            tempLstLanguages = _languagesService.GetLanguageList(true);
            return Json(tempLstLanguages);
        }
        public JsonResult GetChoiceLanguageTemp(WorkDefinitionViewModel objWorkDefinitionViewModel)
        {
            if (objWorkDefinitionViewModel.oLanguage_Choice.Count > 0)
            {
                TempData["ChiceLanguageData"] = objWorkDefinitionViewModel.oLanguage_Choice;
                TempData.Keep("ChiceLanguageData");
            }
            return Json("ok");
        }
        public JsonResult FillObjectType(string DllFlag)
        {
            IList<BEControlTypeInfo> lControlInfo = new List<BEControlTypeInfo>();
            lControlInfo = _repositoryControlType.GetControlTypeList("", true);
            IList<BEControlTypeInfo> lControlInfo1 = new List<BEControlTypeInfo>();
            if (DllFlag == "NoGridControl")
            {

                for (int i = 0; i < lControlInfo.Count; i++)
                {
                    if (lControlInfo[i].iControlTypeID == 0)
                    {
                        BEControlTypeInfo objControl = new BEControlTypeInfo(lControlInfo[i].iControlTypeID, lControlInfo[i].sControlType);
                        lControlInfo1.Add(objControl);
                        objControl = null;
                    }
                    else
                    if (lControlInfo[i].iControlTypeID == 6)
                    {
                        BEControlTypeInfo objControl = new BEControlTypeInfo(lControlInfo[i].iControlTypeID, lControlInfo[i].sControlType);
                        lControlInfo1.Add(objControl);
                        objControl = null;
                    }
                    else if (lControlInfo[i].iControlTypeID == 1)
                    {
                        BEControlTypeInfo objControl = new BEControlTypeInfo(lControlInfo[i].iControlTypeID, lControlInfo[i].sControlType);
                        lControlInfo1.Add(objControl);
                        objControl = null;
                    }
                    else
                    {
                        //var cid = lControlInfo[i].iControlTypeID;
                        //var toRemove = lControlInfo.FirstOrDefault(x => x.iControlTypeID == cid);
                        //if (toRemove != null) lControlInfo.Remove(toRemove);
                    }

                }
            }
            else if (DllFlag == "NoGridControl_Index")
            {
                var toRemove = lControlInfo.FirstOrDefault(x => x.iControlTypeID == 16);
                if (toRemove != null) lControlInfo.Remove(toRemove);

                lControlInfo1 = lControlInfo;
            }
            else
            {
                lControlInfo1 = lControlInfo;

            }
            return Json(lControlInfo1);
        }
        public JsonResult FillObjectDataType()
        {
            List<SelectListItem> ObjProcessList = new List<SelectListItem>();
            ObjProcessList.Insert(0, (new SelectListItem { Text = @BPA.GlobalResources.UI.Resources_common.display_Select, Value = "0" }));
            ObjProcessList.Insert(1, (new SelectListItem { Text = "Character", Value = "Character" }));
            ObjProcessList.Insert(2, (new SelectListItem { Text = "Integer", Value = "Integer" }));
            ObjProcessList.Insert(2, (new SelectListItem { Text = "Float", Value = "Float" }));
            ObjProcessList.Insert(2, (new SelectListItem { Text = "DateTime", Value = "DateTime" }));
            ObjProcessList.Insert(2, (new SelectListItem { Text = "Time", Value = "Time" }));
            ObjProcessList.Insert(2, (new SelectListItem { Text = "Boolean", Value = "Boolean" }));
            var dataContext = ObjProcessList;
            var categories = dataContext
                       .Select(c => new SelectListItem
                       {
                           Value = c.Value,
                           Text = c.Text
                       })
                       .OrderBy(e => e.Text);

            ViewData["defaultCategory"] = categories;
            return Json(ObjProcessList);

        }
        public JsonResult FillValidations()
        {
            IList<BEValidations> ovalidation = new List<BEValidations>();
            ovalidation = _repositoryWork.GetValidations();
            return Json(ovalidation);
        }
        public JsonResult FillRow(int NoOfRow)
        {
            List<SelectListItem> ObjProcessList = new List<SelectListItem>();
            for (int i = 0; i <= NoOfRow; i++)
            {
                //int j;
                //j = i + 1; ;
                ObjProcessList.Insert(i, (new SelectListItem { Text = Convert.ToString(i == 0 ? "Select" : i.ToString()), Value = Convert.ToString(i) }));
            }

            return Json(ObjProcessList);
        }
        public JsonResult FillColumn(int NoOfCol)
        {
            List<SelectListItem> ObjProcessList = new List<SelectListItem>();
            for (int i = 0; i <= NoOfCol; i++)
            {
                //int j;
                //j = i + 1; ;
                ObjProcessList.Insert(i, (new SelectListItem { Text = Convert.ToString(i == 0 ? "Select" : i.ToString()), Value = Convert.ToString(i) }));
            }

            return Json(ObjProcessList);
        }
        public JsonResult FillSpan(int NoOfSpan)
        {
            List<SelectListItem> ObjProcessList = new List<SelectListItem>();
            for (int i = 0; i <= NoOfSpan; i++)
            {

                int j;
                j = i + 1; ;
                ObjProcessList.Insert(i, (new SelectListItem { Text = Convert.ToString(j), Value = Convert.ToString(j) }));
            }

            return Json(ObjProcessList);
        }

        [HttpPost]
        [ValidateAntiForgeryToken] // Cross-Site Request Forgery fixed
        public JsonResult SaveBusJustData(BEWorkObjectApprover objBEWorkObjectApprover)
        {

            SetPurposeWork(objBEWorkObjectApprover);
            BEWorkObjectApprover objCamp = new BEWorkObjectApprover();
            objCamp.sLocations = objBEWorkObjectApprover.sLocations;
            objCamp.sShiftwindows = objBEWorkObjectApprover.sShiftwindows;
            objCamp.sPurposeswm = objBEWorkObjectApprover.sPurposeswm;
            objCamp.sPurposetime = objBEWorkObjectApprover.sPurposetime;
            objCamp.sPurposetrans = objBEWorkObjectApprover.sPurposetrans;
            objCamp.sBusinessJustifications = objBEWorkObjectApprover.sBusinessJustifications;
            objCamp.sTargetq1 = Convert.ToString(objBEWorkObjectApprover.sTargetq1);
            objCamp.sTargetq2 = Convert.ToString(objBEWorkObjectApprover.sTargetq2);
            objCamp.sTargetq3 = Convert.ToString(objBEWorkObjectApprover.sTargetq3);
            objCamp.sTargety1 = Convert.ToString(objBEWorkObjectApprover.sTargety1);
            objCamp.sTargety2 = Convert.ToString(objBEWorkObjectApprover.sTargety2);
            objCamp.sTargety3 = Convert.ToString(objBEWorkObjectApprover.sTargety3);
            objCamp.skeybenifits = objBEWorkObjectApprover.skeybenifits;
            objCamp.iBuisnessID = int.Parse(objBEWorkObjectApprover.iBuisnessID.ToString());
            objCamp.iTechID = int.Parse(objBEWorkObjectApprover.iTechID.ToString());
            objCamp.sStatus = Convert.ToBoolean(TempData.Peek("bmodify")) == true ? "Update" : "insertObject";

            TempData["BusJustData"] = JsonConvert.SerializeObject(objCamp);

            return Json(new { strMessage = "Save" });
        }
        private void SetPurposeWork(BEWorkObjectApprover oCampaignViewModel)
        {
            for (int i = 0; i < oCampaignViewModel.PurposeofcreationofWork.Count; i++)
            {
                if (oCampaignViewModel.PurposeofcreationofWork[i] == Resources_Campaign.display_WorkManagement)
                {
                    oCampaignViewModel.sPurposeswm = true;
                    continue;
                }
                else if (oCampaignViewModel.PurposeofcreationofWork[i] == Resources_Campaign.display_TimeTracking)
                {
                    oCampaignViewModel.sPurposetime = true;
                    continue;
                }
                else if (oCampaignViewModel.PurposeofcreationofWork[i] == Resources_Campaign.display_TransactionsMonitoring)
                {
                    oCampaignViewModel.sPurposetrans = true;
                    continue;
                }
            }
        }
        public JsonResult TempChoiceDataSave(List<BEWorkObjectChoice> objchoiceData)
        {
            TempData["ChoiceDataTemp"] = JsonConvert.SerializeObject(objchoiceData);
            //var deserialize = JsonConvert.DeserializeObject<List<BEWorkObjectChoice>>(TempData["ChoiceDataTemp"].ToString());

            TempData.Peek("ChoiceDataTemp");

            return Json("OK");
        }
        // [HttpParamAction]
        [HttpPost]
        // [ValidateAntiForgeryToken]
        public JsonResult SaveWorkData(WorkDefinitionViewModel objWorkDefinitionViewModel)
        {
            WorkDefinitionViewModel obje = objWorkDefinitionViewModel;
            var sStatus = Convert.ToBoolean(TempData.Peek("bmodify")) == true ? "Update" : "insertObject";
            try
            {
                List<BEStoreInfo> lstBEStoreInfo = new List<BEStoreInfo>();
                var lst = lstBEStoreInfo;
                this._repositoryStore.InsertDataXML(CatchRecord(obje), Int32.Parse(BPA.GlobalResources.Resources_FormID.WorkDefinition), true);
                TempData["GridObjControllName"] = null;
                TempData["lstTABList"] = null;
            }
            catch (Exception ex)
            {
                return Json(new { strMessage = sStatus, Successed = true, msg = ex.Message.ToString() });
            }
            return Json(new { strMessage = sStatus, Successed = false, msg = "" });
        }
        private WorkDefinitionModel CatchRecord(WorkDefinitionViewModel ObjvmWork)
        {
            //Catch  store Data
            WorkDefinitionModel objStoreInfo = new WorkDefinitionModel();

            if (TempData["BusJustData"] != null)
            {
                BEWorkObjectApprover objBEWorkObjectApprover = new BEWorkObjectApprover();
                objBEWorkObjectApprover = JsonConvert.DeserializeObject<BEWorkObjectApprover>(TempData["BusJustData"].ToString()) ?? new BEWorkObjectApprover();

                BusinessJustificatonModel objCamp = new BusinessJustificatonModel();
                objCamp.Locations = objBEWorkObjectApprover.sLocations;
                objCamp.Shiftwindows = objBEWorkObjectApprover.sShiftwindows;
                objCamp.Purposeswm = objBEWorkObjectApprover.sPurposeswm;
                objCamp.Purposetime = objBEWorkObjectApprover.sPurposetime;
                objCamp.Purposetrans = objBEWorkObjectApprover.sPurposetrans;
                objCamp.BusinessJustifications = objBEWorkObjectApprover.sBusinessJustifications ?? "";
                objCamp.Targetq1 = Convert.ToString(objBEWorkObjectApprover.sTargetq1);
                objCamp.Targetq2 = Convert.ToString(objBEWorkObjectApprover.sTargetq2);
                objCamp.Targetq3 = Convert.ToString(objBEWorkObjectApprover.sTargetq3);
                objCamp.Targety1 = Convert.ToString(objBEWorkObjectApprover.sTargety1);
                objCamp.Targety2 = Convert.ToString(objBEWorkObjectApprover.sTargety2);
                objCamp.Targety3 = Convert.ToString(objBEWorkObjectApprover.sTargety3);
                objCamp.keybenifits = objBEWorkObjectApprover.skeybenifits;
                objCamp.BuisnessID = int.Parse(objBEWorkObjectApprover.iBuisnessID.ToString());
                objCamp.TechID = int.Parse(objBEWorkObjectApprover.iTechID.ToString());
                objCamp.Status = objBEWorkObjectApprover.sStatus;
                objStoreInfo.BusJustData = objCamp;
            }

            objStoreInfo.StoreId = int.Parse((TempData["_SearchID"] == null ? "0" : TempData["_SearchID"]).ToString());
            objStoreInfo.CampId = int.Parse(ObjvmWork.CampaignName);
            objStoreInfo.WorkDefinitionName = Encoder.HtmlEncode(ObjvmWork.WorkDefinitionName.ToString(), false);//Store name
            objStoreInfo.Description = Encoder.HtmlEncode(ObjvmWork.Description == null ? "" : ObjvmWork.Description.ToString(), false);
            objStoreInfo.DisableWork = Convert.ToBoolean(ObjvmWork.DisableWork);
            objStoreInfo.IsForStaging = Convert.ToBoolean(ObjvmWork.IsForStaging);
            objStoreInfo.NoOfRows = ObjvmWork.NoOfRows;
            objStoreInfo.NoOfColumns = int.Parse(ObjvmWork.NoOfColumns);
            objStoreInfo.GenerateLetter = Convert.ToBoolean(ObjvmWork.GenerateLetter);

            objStoreInfo.DistributionBot = Convert.ToBoolean(ObjvmWork.IsDistributionBot);
            objStoreInfo.IsRunTimeUploadRequired = Convert.ToBoolean(ObjvmWork.bIsRunTimeUploadRequired);
            objStoreInfo.IncreaseSearch = int.Parse(ObjvmWork.IncreaseSearch == null ? "0" : ObjvmWork.IncreaseSearch);
            objStoreInfo.ErrorSnapshot = Convert.ToBoolean(ObjvmWork.ErrorSnapshot);
            objStoreInfo.ErrorDuration = Convert.ToInt32(String.IsNullOrEmpty(ObjvmWork.ErrorDuration) == true ? "0" : ObjvmWork.ErrorDuration);

            if (TempData["lstTABList"] != null)
            {
                objStoreInfo.WorkObjectTABList = JsonConvert.DeserializeObject<List<BEWorkObjectTAB>>(TempData["lstTABList"].ToString()).Select(t => new TabMasterModel()
                {
                    ObjectChoiceID = t.iObjectChoiceID,
                    ChoiceValue = t.sChoiceValue ?? "",
                    TABNameValue = t.sTABNameValue ?? "",
                    Order = t.iOrder,
                    Disabled = t.bDisabled
                }).ToList();
            }

            objStoreInfo.TABMapping = Convert.ToBoolean(ObjvmWork.TABMapping);
            objStoreInfo.IsGridConfiguration = Convert.ToBoolean(ObjvmWork.IsGridConfiguration);
            objStoreInfo.WorkDefinition = new List<WorkObjectModel>();

            //Catch Grid Data
            string GridObjectMulti = "";
            for (int i = 0; i < ObjvmWork.WorkDefinition.Count(); i++)
            {
                WorkObjectModel ObjItemAdd = new WorkObjectModel();
                if ((ObjvmWork.WorkDefinition[i]).sDataType == "Character" && (ObjvmWork.WorkDefinition[i]).iLength == 0)
                {
                    ViewData["Message"] = "Invalid Length";
                }
                ObjItemAdd.ObjectID = ObjvmWork.WorkDefinition[i].iObjectID;
                ObjItemAdd.StoreID = ObjvmWork.WorkDefinition[i].iStoreID;
                //catch ObjectName data in grid
                ObjItemAdd.ObjectName = Encoder.HtmlEncode(ObjvmWork.WorkDefinition[i].sObjectName, false).Replace("&#39;", "'"); ;
                ObjItemAdd.WorkID = Convert.ToBoolean(ObjvmWork.WorkDefinition[i].bWorkID);
                if (string.IsNullOrEmpty(ObjvmWork.WorkDefinition[i].sObjectDescription))
                {
                    ObjItemAdd.ObjectDescription = "";
                }
                else
                {
                    ObjItemAdd.ObjectDescription = Encoder.HtmlEncode(ObjvmWork.WorkDefinition[i].sObjectDescription, false).Replace("&#39;", "'");
                }

                ObjItemAdd.ObjectLabel = Encoder.HtmlEncode(ObjvmWork.WorkDefinition[i].sObjectLabel, false).Replace("&#39;", "'"); ;
                ObjItemAdd.ObjectType = ObjvmWork.WorkDefinition[i].iObjectType;
                ObjItemAdd.DataType = ObjvmWork.WorkDefinition[i].sDataType;
                ObjItemAdd.Length = ObjvmWork.WorkDefinition[i].iLength;

                //Catch Choice Data

                ObjItemAdd.Choice = DataMerge(ObjvmWork.WorkDefinition[i].UID);
                ObjItemAdd.TranslateList = ObjvmWork.WorkDefinition[i].oTranslateList.Select(x => new WorkObjectTranslateModel()
                {
                    SDOBJLanID = x.iSDOBJLanID,
                    LanguageID = x.iLanguageID,
                    ConvertText = x.sConvertText
                }).ToList();

                ObjItemAdd.ValidationID = ObjvmWork.WorkDefinition[i].iValidationID;
                //Catch Checkbox data
                ObjItemAdd.Visible = Convert.ToBoolean(ObjvmWork.WorkDefinition[i].bVisible);//Set textbox visibility in gent dashboard
                ObjItemAdd.Search = Convert.ToBoolean(ObjvmWork.WorkDefinition[i].bSearch);//Set  Searchable textbox  in gent dashboard
                ObjItemAdd.Editable = Convert.ToBoolean(ObjvmWork.WorkDefinition[i].bEditable);//Set  Editable textbox  in gent dashboard
                ObjItemAdd.Required = Convert.ToBoolean(ObjvmWork.WorkDefinition[i].bRequired);//Set  Required textbox  in gent dashboard
                ObjItemAdd.Disabled = Convert.ToBoolean(ObjvmWork.WorkDefinition[i].bDisabled);//Set  Disabled textbox  in gent dashboard
                ObjItemAdd.UniqueID = Convert.ToBoolean(ObjvmWork.WorkDefinition[i].bUniqueID);//Set  UniqueID textbox  in gent dashboard
                ObjItemAdd.EmpIdLanId = Convert.ToBoolean(ObjvmWork.WorkDefinition[i].bLANID);//Set  bEmpIdLanId textbox  in gent dashboard
                ObjItemAdd.TransactionType = Convert.ToBoolean(ObjvmWork.WorkDefinition[i].bTransactionType);
                ObjItemAdd.IsUpload = Convert.ToBoolean(ObjvmWork.WorkDefinition[i].bIsUpload);
                ObjItemAdd.IsReport = Convert.ToBoolean(ObjvmWork.WorkDefinition[i].bIsReport);
                ObjItemAdd.CustomerIdentifier = Convert.ToBoolean(ObjvmWork.WorkDefinition[i].bCustomerIdentifier);
                ObjItemAdd.ReportOrder = ObjvmWork.WorkDefinition[i].iIsReportOrder;
                ObjItemAdd.Row_No = ObjvmWork.WorkDefinition[i].irow_No;
                ObjItemAdd.Column_No = ObjvmWork.WorkDefinition[i].icolumn_No;
                ObjItemAdd.Column_Span = ObjvmWork.WorkDefinition[i].icolumn_Span;
                ObjItemAdd.TAB_ID = ObjvmWork.WorkDefinition[i].iTAB_ID;
                ObjItemAdd.GridControlID = ObjvmWork.WorkDefinition[i].sGridControlID;
                GridObjectMulti = ObjvmWork.WorkDefinition[i].sGridControlID != null ? (GridObjectMulti == "" ? ObjvmWork.WorkDefinition[i].sGridControlID : GridObjectMulti + "," + ObjvmWork.WorkDefinition[i].sGridControlID) : (GridObjectMulti == "" ? "" : GridObjectMulti);
                ObjItemAdd.SearchableSearch = Convert.ToBoolean(ObjvmWork.WorkDefinition[i].bSearchableSearch);
                ObjItemAdd.ReportsOrderSearch = ObjvmWork.WorkDefinition[i].iReportsOrderSearch;
                if (oTenant != null)
                {
                    if (bool.Parse(oTenant.ClientMultiLanguage))
                    {
                        ObjItemAdd.IsTranslate = Convert.ToBoolean(ObjvmWork.WorkDefinition[i].bIsTranslate);
                    }
                }
                objStoreInfo.WorkDefinition.Add(ObjItemAdd);
            }
            objStoreInfo.GridObjectMultiName = GridObjectMulti;
            return objStoreInfo;
        }
        private List<WorkObjectChoiceModel> DataMerge(string UID)
        {
            List<BEWorkObjectChoice> objlst_1 = new List<BEWorkObjectChoice>();
            List<BEWorkObjectChoice> objlst = new List<BEWorkObjectChoice>();
            objlst = JsonConvert.DeserializeObject<List<BEWorkObjectChoice>>(Convert.ToString(TempData["ChoiceDataTemp"]) ?? "") ?? new List<BEWorkObjectChoice>();
            if (objlst != null)
            {
                if (objlst.Count > 0)
                {
                    objlst_1 = objlst.Where(x => (x.iUid == UID)).ToList<BEWorkObjectChoice>();
                }
            }
            var workObjChoices = objlst_1.Select(c => new WorkObjectChoiceModel()
            {
                ObjectChoiceID = c.iObjectChoiceID,
                ChoiceValue = c.sChoiceValue ?? "",
                Disabled = c.bDisabled,
                Order = c.iOrder,
                GroupID = c.iGroupID,
                LanguageId = c.sCultureid,
                ChoiceLanguageID = c.iChoiceLanguageID
            }).ToList();
            return workObjChoices;
        }
        public ActionResult GetFilter_WM_List(string iCampaignName, string sname)
        {
            string IStore_ID = "0";
            if (Convert.ToString(TempData["_SearchID"]) != "")
            {
                IStore_ID = Convert.ToString(TempData["_SearchID"]);
            }
            TempData.Keep("_SearchID");
            WorkDefinitionViewModel objvm = new WorkDefinitionViewModel();
            objvm.oTenant = base.oTenant;
            objvm.oStore = _repositoryStore.GetStoreList(int.Parse(iCampaignName == "" ? "0" : iCampaignName), sname, true);
            var ValCount = objvm.oStore.Count;
            return Json(new { strAction = IStore_ID, PreviesData = ValCount });
        }

        public JsonResult GetBusinessApprover(int? iProcessID)
        {
            return Json(_repositoryWork.GetBusinessList(iProcessID.GetValueOrDefault()));
        }
        [HttpPost]
        public JsonResult GetTABMasterData()
        {
            List<BEWorkObjectTAB> lstMasterData = new List<BEWorkObjectTAB>();
            if (TempData["lstTABList"] != null)
            {
                lstMasterData = JsonConvert.DeserializeObject<List<BEWorkObjectTAB>>(TempData["lstTABList"].ToString()) ?? new List<BEWorkObjectTAB>();
                TempData.Keep("lstTABList");
            }

            return Json(lstMasterData);
        }

        [HttpPost]
        public JsonResult SaveTabName_Temp(List<BEWorkObjectTAB> objchoiceData)
        {
            TempData["lstTABList"] = JsonConvert.SerializeObject(objchoiceData);
            TempData.Keep("lstTABList");
            return Json("1");
        }

        public JsonResult GetTmplateList()
        {
            IList<BEDocDetail> lTemptaleList = new List<BEDocDetail>();
            lTemptaleList = _repositoryDocDetail.GetTemplateList(true);
            return Json(lTemptaleList);
        }

        public JsonResult CheckGridConfigurations(string ClientProcesscampId)
        {
            string checkGrd = "Empty";
            var iCampaignID = ClientProcesscampId.Split('/')[0];
            var iClientID = ClientProcesscampId.Split('/')[1];
            var iProcessID = ClientProcesscampId.Split('/')[2];
            var lstGridObject = this._repositoryStore.GetGridObject_Temp(iProcessID, iCampaignID, "0");

            if (lstGridObject.Count > 0)
            {
                checkGrd = "Done";
            }

            return Json(checkGrd);
        }
        public JsonResult FillRowTAB(int NoOfRow)
        {
            // List<SelectListItem> ObjProcessList = new List<SelectListItem>();
            List<BEWorkObjectTAB> objTabData = new List<BEWorkObjectTAB>();
            List<BEWorkObjectTAB> objTabDataFinal = new List<BEWorkObjectTAB>();
            objTabData = JsonConvert.DeserializeObject<List<BEWorkObjectTAB>>(TempData["lstTABList"].ToString());
            TempData.Keep("lstTABList");
            // objTabDataFinal = objTabData;
            if (objTabData != null)
            {
                int tivalues = 1;
                var list = objTabData.Where(f => f.sTABNameValue == "Select").Select(n => n.sChoiceValue);
                if (list.Count() <= 0)
                {
                    objTabDataFinal.Insert(0, (new BEWorkObjectTAB { sTABNameValue = Convert.ToString("Select"), sChoiceValue = Convert.ToString(0) }));
                    for (int i = 0; i < objTabData.Count; i++)
                    {
                        if (objTabData[i].bDisabled == false)
                        {
                            objTabDataFinal.Insert(tivalues, (new BEWorkObjectTAB { sTABNameValue = objTabData[i].sTABNameValue.ToString(), sChoiceValue = objTabData[i].sChoiceValue.ToString() }));
                            tivalues = tivalues + 1;
                        }
                    }

                }
                else
                {
                    for (int i = 0; i < objTabData.Count; i++)
                    {
                        if (objTabData[i].bDisabled == false)
                        {

                            objTabDataFinal.Insert(i, (new BEWorkObjectTAB { sTABNameValue = objTabData[i].sTABNameValue.ToString(), sChoiceValue = objTabData[i].sChoiceValue.ToString() }));
                            // tivalues = tivalues + 1;
                        }
                    }
                }

            }
            return Json(objTabDataFinal);
        }

        public JsonResult FillRowGridControl(string ClientProcesscampId = "")
        {
            List<BEWorkObjectGrid> objGrdbData = new List<BEWorkObjectGrid>();

            var iCampaignID = ClientProcesscampId.Split('/')[0];
            var iClientID = ClientProcesscampId.Split('/')[1];
            var iProcessID = ClientProcesscampId.Split('/')[2];
            var storeID = TempData["_SearchID"].ToString();
            TempData.Keep("_SearchID");
            var dsListTemp = this._repositoryStore.GetGridObject_Temp(iProcessID, iCampaignID, storeID);

            if (dsListTemp.Count > 0)
            {
                BEWorkObjectGrid objControl1 = new BEWorkObjectGrid("0", 1000, 0, "--Select--", false, 1);
                objGrdbData.Add(objControl1);
                objControl1 = null;
                for (int i = 0; i < dsListTemp.Count; i++)
                {
                    BEWorkObjectGrid objControl = new BEWorkObjectGrid(dsListTemp[i].GridObjectName.ToString(), 1000, 0, dsListTemp[i].GridObjectName.ToString(), false, 1);
                    objGrdbData.Add(objControl);
                    objControl = null;
                }
            }

            return Json(objGrdbData);
        }

        #region GridConfiguration

        public ActionResult _GridConfigurations()
        {

            WorkDefinitionViewModel oWorkObject = new WorkDefinitionViewModel();

            WorkObject AddNewBlankRow = new WorkObject();
            //add new row in a grid
            if (Convert.ToString(TempData["iStoreId"]) != "")
            {
                var StoreID = int.Parse(Convert.ToString(TempData["iStoreId"]));
                TempData.Keep("iStoreId");
                TempData["_SearchID"] = int.Parse(Convert.ToString(TempData["iStoreId"]));
                // DisplayGridConfiguration(true, StoreID);

            };
            oWorkObject = Display_Grid_Control();
            oWorkObject.oTenant = base.oTenant;
            oWorkObject.WorkDefinitionGRD.Add(AddNewBlankRow);
            return View(oWorkObject);
        }

        private WorkDefinitionViewModel Display_Grid_Control()
        {
            WorkDefinitionViewModel oWorkObject = new WorkDefinitionViewModel();
            List<WorkDefinitionViewModel> lstWorkObject_ = JsonConvert.DeserializeObject<List<WorkDefinitionViewModel>>(Convert.ToString(TempData["GridObjControllName"]));
            TempData.Keep("GridObjControllName");
            List<WorkDefinitionViewModel> lstWorkObject = new List<WorkDefinitionViewModel>();
            List<BEWorkObjectTAB> lstGridControl = new List<BEWorkObjectTAB>();
            if (lstWorkObject_ != null)
            {
                for (int i = 0; i < lstWorkObject_.Count; i++)
                {

                    BEWorkObjectTAB ObjGrid = new BEWorkObjectTAB();
                    //oWorkObject.iStoreId = iStoreID;
                    //oWorkObject.GridObjectName = drGrd1["GridName"].ToString();// ObjvmWork.GridObjectName;
                    //oWorkObject.GridObjectNameID = drGrd1["GridID"].ToString();
                    //oWorkObject.DisableGridObject = Convert.ToBoolean(drGrd1["Disabled"].ToString());

                    //oWorkObject.text = drGrd1["GridName"].ToString();


                    ObjGrid.iObjectChoiceID = Convert.ToInt32(lstWorkObject_[i].GridObjectNameID.ToString());
                    ObjGrid.iObjID = Convert.ToInt32(lstWorkObject_[i].GridObjectNameID.ToString());
                    ObjGrid.sChoiceValue = lstWorkObject_[i].GridObjectName.ToString();
                    ObjGrid.bDisabled = Convert.ToBoolean(lstWorkObject_[i].DisableGridObject.ToString());
                    ObjGrid.bGrdEditable = Convert.ToBoolean(lstWorkObject_[i].IsGrdEditable.ToString());
                    //WorkDefinitionViewModel obj = new WorkDefinitionViewModel();
                    //obj = lstWorkObject_[i];
                    //lstWorkObject.Add(obj);
                    lstGridControl.Add(ObjGrid);
                }
            }
            //oWorkObject.
            oWorkObject.AddGrid = lstGridControl;
            return oWorkObject;
        }

        [HttpPost]
        [ValidateAntiForgeryToken] // Cross-Site Request Forgery fixed
        public JsonResult GetGridObjectDetails(string ClientProcesscampId = "")
        {
            WorkDefinitionViewModel oWorkObject = new WorkDefinitionViewModel();
            if (ClientProcesscampId != "")
            {
                var iCampaignID = ClientProcesscampId.Split('/')[0];
                var iClientID = ClientProcesscampId.Split('/')[1];
                var iProcessID = ClientProcesscampId.Split('/')[2];
                string StoreID = "0";
                WorkDefinitionViewModel objvm = new WorkDefinitionViewModel();
                objvm.oTenant = base.oTenant;
                objvm.oStore = _repositoryStore.GetStoreList(int.Parse(iCampaignID == "" ? "0" : iCampaignID), "", true);
                Boolean bGridObject = false;
                if (objvm.oStore.Count > 0)
                {
                    if (objvm.oStore[0].bGridObject)
                    {
                        StoreID = Convert.ToString(objvm.oStore[0].iStoreId);
                    }
                    bGridObject = objvm.oStore[0].bGridObject;


                }

                TempData["iGridStoreId"] = StoreID;
                TempData.Keep("iGridStoreId");
                TempData["bIsGridConfiguration"] = bGridObject;
                TempData.Keep("bIsGridConfiguration");


                var dsListTemp = this._repositoryStore.GetGridObject_Temp(iProcessID, iCampaignID, StoreID);

                List<WorkDefinitionViewModel> lstWorkObject = new List<WorkDefinitionViewModel>();
                List<BEWorkObjectTAB> lstGridControl = new List<BEWorkObjectTAB>();

                if (dsListTemp.Count > 0)
                {
                    for (int i = 0; i < dsListTemp.Count; i++)
                    {

                        BEWorkObjectTAB ObjGrid = new BEWorkObjectTAB();

                        ObjGrid.iObjectChoiceID = Convert.ToInt32(dsListTemp[i].GridID.ToString());
                        ObjGrid.iObjID = Convert.ToInt32(dsListTemp[i].StoreID.ToString());
                        ObjGrid.sChoiceValue = dsListTemp[i].GridObjectName.ToString() ?? "";
                        ObjGrid.bDisabled = Convert.ToBoolean(dsListTemp[i].DisableGridObject.ToString());
                        ObjGrid.bGrdEditable = Convert.ToBoolean(dsListTemp[i].IsGrdEditable.ToString());
                        //WorkDefinitionViewModel obj = new WorkDefinitionViewModel();
                        //obj = lstWorkObject_[i];
                        //lstWorkObject.Add(obj);
                        lstGridControl.Add(ObjGrid);
                    }
                }
                //oWorkObject.
                oWorkObject.AddGrid = lstGridControl;

            }
            return Json(oWorkObject.AddGrid);
        }

        public JsonResult SaveGridValuesRefresh(string GridID, string AFlag, string iProcessID, string iCampaignID, string sGridObjectName)
        {
            WorkDefinitionViewModel oWorkObject = new WorkDefinitionViewModel();
            string DisplayMsg = "";
            WorkObject AddNewBlankRow = new WorkObject();
            if (AFlag == "REF")
            {

                //add new row in a grid
                oWorkObject.WorkDefinitionGRD.Add(AddNewBlankRow);
                // Catch_GRD_Record( objWorkDefinitionViewModel);
                DisplayMsg = "REF";
            }
            else
            {
                // oWorkObject = Display_GridControl(GridID);
                oWorkObject = DisplayGridConfiguration_DB(iProcessID, iCampaignID, sGridObjectName);
                if (oWorkObject.WorkDefinitionGRD.Count > 0)
                {
                    DisplayMsg = "YES";
                }
                else
                {
                    DisplayMsg = "REF";

                    //add new row in a grid
                    oWorkObject.WorkDefinitionGRD.Add(AddNewBlankRow);
                }
            }
            return Json(new
            {
                displaymsg = DisplayMsg,
                lstWorkDefinitionGRD = oWorkObject.WorkDefinitionGRD

            });
        }

        private WorkDefinitionViewModel DisplayGridConfiguration_DB(string ProcessID, string CampID, string GridObjectName)
        {
            WorkDefinitionViewModel oWorkObject = new WorkDefinitionViewModel();
            try
            {
                DataSet ds = new DataSet();
                string StoreID = Convert.ToString(TempData["iGridStoreId"]) == "" ? "0" : Convert.ToString(TempData["iGridStoreId"]);
                TempData.Keep("iGridStoreId");
                var bIsGridConfiguration = Convert.ToBoolean(TempData["bIsGridConfiguration"]);
                TempData.Keep("bIsGridConfiguration");
                if (!bIsGridConfiguration)
                {

                    ds = _repositoryStore.GetGridObjectcONTROL_Temp(ProcessID, CampID, GridObjectName, StoreID);
                    //Catch Grid Data
                    foreach (DataRow drGrd2 in ds.Tables[0].Rows)
                    {


                        WorkObject ObjItemAdd = new WorkObject();
                        TreeViewGrd objtreeView = new TreeViewGrd();

                        ObjItemAdd.iObjectID = Convert.ToInt32(drGrd2["iObjectID"].ToString());
                        ObjItemAdd.iStoreID = 0;

                        //catch ObjectName data in grid
                        ObjItemAdd.sObjectName = drGrd2["ObjectName"].ToString();
                        // ObjItemAdd.bWorkID = Convert.ToBoolean(drGrd2["IsWorkID"].ToString());
                        ObjItemAdd.sObjectDescription = drGrd2["ObjectDescription"].ToString();
                        ObjItemAdd.sObjectLabel = drGrd2["ObjectLabel"].ToString();
                        ObjItemAdd.iObjectType = Convert.ToInt32(drGrd2["ObjectType"].ToString());

                        // ObjItemAdd.iObjectType = Convert.ToInt32(drGrd2["DSObjControlID"].ToString());
                        foreach (var BEControlTypeInfo in (IList<BEControlTypeInfo>)FillObjectType("dfdf").Value)
                        {
                            if (BEControlTypeInfo.iControlTypeID == Convert.ToInt32(drGrd2["ObjectType"].ToString()))
                            {
                                ObjItemAdd.selectControlType.iControlTypeID = BEControlTypeInfo.iControlTypeID;
                                ObjItemAdd.selectControlType.sControlType = BEControlTypeInfo.sControlType;
                            }
                        }
                        ObjItemAdd.sDataType = drGrd2["DataType"].ToString();
                        ObjItemAdd.selectedDataType.Text = drGrd2["DataType"].ToString();
                        ObjItemAdd.selectedDataType.Value = drGrd2["DataType"].ToString();
                        ObjItemAdd.iLength = Convert.ToInt32(drGrd2["iLength"].ToString());
                        if (ds.Tables[1].Rows.Count > 0)
                        {
                            //Catch Choice Data
                            ObjItemAdd.oChoice = GRDChoiceDataMerge_DB(ds.Tables[1], drGrd2["lstNewID"].ToString());
                        }



                        ObjItemAdd.iValidationID = Convert.ToInt32(drGrd2["ValidationID"].ToString());
                        foreach (var BEValidations in (IList<BEValidations>)FillValidations().Value)
                        {
                            if (BEValidations.ValidationId == Convert.ToInt32(drGrd2["ValidationID"].ToString()))
                            {
                                ObjItemAdd.selectedValidation.ValidationId = BEValidations.ValidationId;
                                ObjItemAdd.selectedValidation.ValidationType = BEValidations.ValidationType;
                            }
                        }
                        //Catch Checkbox data
                        ObjItemAdd.bVisible = Convert.ToBoolean(drGrd2["Visible"].ToString());//Set textbox visibility in gent dashboard
                        ObjItemAdd.bSearch = Convert.ToBoolean(drGrd2["Search"].ToString());//Set  Searchable textbox  in gent dashboard
                        ObjItemAdd.bEditable = Convert.ToBoolean(drGrd2["Editable"].ToString());//Set  Editable textbox  in gent dashboard
                        ObjItemAdd.bRequired = Convert.ToBoolean(drGrd2["Required"].ToString());//Set  Required textbox  in gent dashboard
                        ObjItemAdd.bDisabled = Convert.ToBoolean(drGrd2["Disabled"].ToString());//Set  Disabled textbox  in gent dashboard

                        oWorkObject.WorkDefinitionGRD.Add(ObjItemAdd);
                        //  }
                    }


                }
                else
                {
                    ds = _repositoryWork.GetGridConfigurationData(Convert.ToInt32(StoreID));
                    List<WorkDefinitionViewModel> lstWorkObject = new List<WorkDefinitionViewModel>();

                    //Catch Grid Data
                    string findValues = "NO";
                    foreach (DataRow drGrd2 in ds.Tables[1].Rows)
                    {
                        if (drGrd2["GridName"].ToString() == GridObjectName)
                        {

                            findValues = "YES";
                            WorkObject ObjItemAdd = new WorkObject();
                            TreeViewGrd objtreeView = new TreeViewGrd();

                            ObjItemAdd.iObjectID = Convert.ToInt32(drGrd2["DSObjID"].ToString());
                            ObjItemAdd.iStoreID = Convert.ToInt32(StoreID);

                            //catch ObjectName data in grid
                            ObjItemAdd.sObjectName = Encoder.HtmlEncode(drGrd2["DSObjName"].ToString(), false);
                            // ObjItemAdd.bWorkID = Convert.ToBoolean(drGrd2["IsWorkID"].ToString());
                            ObjItemAdd.sObjectDescription = Encoder.HtmlEncode(drGrd2["DESCRIPTION"].ToString(), false);
                            ObjItemAdd.sObjectLabel = Encoder.HtmlEncode(drGrd2["DSObjLabelText"].ToString(), false);
                            ObjItemAdd.iObjectType = Convert.ToInt32(drGrd2["DSObjControlID"].ToString());

                            // ObjItemAdd.iObjectType = Convert.ToInt32(drGrd2["DSObjControlID"].ToString());
                            foreach (var BEControlTypeInfo in (IList<BEControlTypeInfo>)FillObjectType("dfdf").Value)
                            {
                                if (BEControlTypeInfo.iControlTypeID == Convert.ToInt32(drGrd2["DSObjControlID"].ToString()))
                                {
                                    ObjItemAdd.selectControlType.iControlTypeID = BEControlTypeInfo.iControlTypeID;
                                    ObjItemAdd.selectControlType.sControlType = BEControlTypeInfo.sControlType;
                                }
                            }
                            ObjItemAdd.sDataType = drGrd2["ObjDataType"].ToString();
                            ObjItemAdd.selectedDataType.Text = drGrd2["ObjDataType"].ToString();
                            ObjItemAdd.selectedDataType.Value = drGrd2["ObjDataType"].ToString();
                            ObjItemAdd.iLength = Convert.ToInt32(drGrd2["MaxLength"].ToString());
                            if (ds.Tables[2].Rows.Count > 0)
                            {
                                //Catch Choice Data
                                ObjItemAdd.oChoice = GRDChoiceDataMerge(ds.Tables[2], drGrd2["DSObjID"].ToString());
                            }



                            ObjItemAdd.iValidationID = Convert.ToInt32(drGrd2["ValidationID"].ToString());
                            foreach (var BEValidations in (IList<BEValidations>)FillValidations().Value)
                            {
                                if (BEValidations.ValidationId == Convert.ToInt32(drGrd2["ValidationID"].ToString()))
                                {
                                    ObjItemAdd.selectedValidation.ValidationId = BEValidations.ValidationId;
                                    ObjItemAdd.selectedValidation.ValidationType = BEValidations.ValidationType;
                                }
                            }
                            //Catch Checkbox data
                            ObjItemAdd.bVisible = Convert.ToBoolean(drGrd2["IsVisible"].ToString());//Set textbox visibility in gent dashboard
                            ObjItemAdd.bSearch = Convert.ToBoolean(drGrd2["IsSearchable"].ToString());//Set  Searchable textbox  in gent dashboard
                            ObjItemAdd.bEditable = Convert.ToBoolean(drGrd2["IsEditable"].ToString());//Set  Editable textbox  in gent dashboard
                            ObjItemAdd.bRequired = Convert.ToBoolean(drGrd2["IsRequired"].ToString());//Set  Required textbox  in gent dashboard
                            ObjItemAdd.bDisabled = Convert.ToBoolean(drGrd2["Disabled"].ToString());//Set  Disabled textbox  in gent dashboard

                            ObjItemAdd.iIsReportOrder = Convert.ToInt32(drGrd2["IsReportOrder"].ToString());
                            oWorkObject.WorkDefinitionGRD.Add(ObjItemAdd);
                        }
                    }
                    // lstWorkObject.Add(oWorkObject);

                    if (findValues == "NO")
                    {
                        ds = _repositoryStore.GetGridObjectcONTROL_Temp(ProcessID, CampID, GridObjectName, StoreID);
                        //Catch Grid Data
                        foreach (DataRow drGrd2 in ds.Tables[0].Rows)
                        {


                            WorkObject ObjItemAdd = new WorkObject();
                            TreeViewGrd objtreeView = new TreeViewGrd();

                            ObjItemAdd.iObjectID = Convert.ToInt32(drGrd2["iObjectID"].ToString());
                            ObjItemAdd.iStoreID = 0;

                            //catch ObjectName data in grid
                            ObjItemAdd.sObjectName = drGrd2["ObjectName"].ToString();
                            // ObjItemAdd.bWorkID = Convert.ToBoolean(drGrd2["IsWorkID"].ToString());
                            ObjItemAdd.sObjectDescription = drGrd2["ObjectDescription"].ToString();
                            ObjItemAdd.sObjectLabel = drGrd2["ObjectLabel"].ToString();
                            ObjItemAdd.iObjectType = Convert.ToInt32(drGrd2["ObjectType"].ToString());

                            // ObjItemAdd.iObjectType = Convert.ToInt32(drGrd2["DSObjControlID"].ToString());
                            foreach (var BEControlTypeInfo in (IList<BEControlTypeInfo>)FillObjectType("dfdf").Value)
                            {
                                if (BEControlTypeInfo.iControlTypeID == Convert.ToInt32(drGrd2["ObjectType"].ToString()))
                                {
                                    ObjItemAdd.selectControlType.iControlTypeID = BEControlTypeInfo.iControlTypeID;
                                    ObjItemAdd.selectControlType.sControlType = BEControlTypeInfo.sControlType;
                                }
                            }
                            ObjItemAdd.sDataType = drGrd2["DataType"].ToString();
                            ObjItemAdd.selectedDataType.Text = drGrd2["DataType"].ToString();
                            ObjItemAdd.selectedDataType.Value = drGrd2["DataType"].ToString();
                            ObjItemAdd.iLength = Convert.ToInt32(drGrd2["iLength"].ToString());
                            if (ds.Tables[1].Rows.Count > 0)
                            {
                                //Catch Choice Data
                                ObjItemAdd.oChoice = GRDChoiceDataMerge_DB(ds.Tables[1], drGrd2["lstNewID"].ToString());
                            }



                            ObjItemAdd.iValidationID = Convert.ToInt32(drGrd2["ValidationID"].ToString());
                            foreach (var BEValidations in (IList<BEValidations>)FillValidations().Value)
                            {
                                if (BEValidations.ValidationId == Convert.ToInt32(drGrd2["ValidationID"].ToString()))
                                {
                                    ObjItemAdd.selectedValidation.ValidationId = BEValidations.ValidationId;
                                    ObjItemAdd.selectedValidation.ValidationType = BEValidations.ValidationType;
                                }
                            }
                            //Catch Checkbox data
                            ObjItemAdd.bVisible = Convert.ToBoolean(drGrd2["Visible"].ToString());//Set textbox visibility in gent dashboard
                            ObjItemAdd.bSearch = Convert.ToBoolean(drGrd2["Search"].ToString());//Set  Searchable textbox  in gent dashboard
                            ObjItemAdd.bEditable = Convert.ToBoolean(drGrd2["Editable"].ToString());//Set  Editable textbox  in gent dashboard
                            ObjItemAdd.bRequired = Convert.ToBoolean(drGrd2["Required"].ToString());//Set  Required textbox  in gent dashboard
                            ObjItemAdd.bDisabled = Convert.ToBoolean(drGrd2["Disabled"].ToString());//Set  Disabled textbox  in gent dashboard

                            oWorkObject.WorkDefinitionGRD.Add(ObjItemAdd);
                            //  }
                        }
                    }
                }
            }
            catch (Exception ert)
            {
                throw ert;
            }
            return oWorkObject;
        }

        private List<BEWorkObjectChoice> GRDChoiceDataMerge(DataTable dt, string DSObjID)
        {
            List<BEWorkObjectChoice> objlst_1 = new List<BEWorkObjectChoice>();

            foreach (DataRow drChoice in dt.Rows)
            {
                if (drChoice["DSObjID"].ToString() == DSObjID)
                {
                    BEWorkObjectChoice objChoice = new BEWorkObjectChoice()
                    {
                        iObjectChoiceID = Convert.ToInt32(drChoice["DSObjChoiceID"]),
                        iGroupID = Convert.ToInt32(drChoice["GroupID"]),
                        iObjID = Convert.ToInt32(drChoice["DSObjID"]),
                        sChoiceValue = drChoice["ChoiceValue"].ToString(),
                        bDisabled = Convert.ToBoolean(drChoice["Disabled"]),
                        iOrder = Convert.ToInt32(drChoice["DSOrderID"])
                    };
                    objlst_1.Add(objChoice);
                    objChoice = null;
                }
            }
            return objlst_1;
        }

        private List<BEWorkObjectChoice> GRDChoiceDataMerge_DB(DataTable dt, string DSObjID)
        {
            List<BEWorkObjectChoice> objlst_1 = new List<BEWorkObjectChoice>();

            foreach (DataRow drChoice in dt.Rows)
            {
                if (drChoice["iNewID"].ToString() == DSObjID)
                {
                    BEWorkObjectChoice objChoice = new BEWorkObjectChoice()
                    {
                        iObjectChoiceID = Convert.ToInt32(drChoice["ObjectChoiceID"]),
                        iGroupID = Convert.ToInt32(drChoice["GroupID"]),
                        iObjID = Convert.ToInt32(drChoice["objectID"]),
                        sChoiceValue = drChoice["ChoiceValue"].ToString(),
                        bDisabled = Convert.ToBoolean(drChoice["disabled"]),
                        iOrder = Convert.ToInt32(drChoice["DSOrderID"])
                    };
                    objlst_1.Add(objChoice);
                    objChoice = null;
                }
            }
            return objlst_1;
        }

        private List<WorkObjectChoiceModel> GRDDataMerge(string UID)
        {
            List<BEWorkObjectChoice> objlst_1 = new List<BEWorkObjectChoice>();
            List<BEWorkObjectChoice> objlst = new List<BEWorkObjectChoice>();
            objlst = JsonConvert.DeserializeObject<List<BEWorkObjectChoice>>(Convert.ToString(TempData["ChoiceDataTempGRD"]) ?? "") ?? new List<BEWorkObjectChoice>();
            TempData.Peek("ChoiceDataTempGRD");
            if (objlst != null)
            {
                if (objlst.Count > 0)
                {
                    objlst_1 = objlst.Where(x => (x.iUid == UID)).ToList<BEWorkObjectChoice>();
                    //objlst_1 = (List<BEWorkObjectChoice>)lstTextObject;
                    //var result=  objlst.Find(item => item.iUid == UID);

                }
            }
            var workObjChoices = objlst_1.Select(c => new WorkObjectChoiceModel()
            {
                ObjectChoiceID = c.iObjectChoiceID,
                ChoiceValue = c.sChoiceValue ?? "",
                Disabled = c.bDisabled,
                Order = c.iOrder,
                GroupID = c.iGroupID,
                LanguageId = c.sCultureid,
                ChoiceLanguageID = c.iChoiceLanguageID
            }).ToList();
            return workObjChoices;
        }

        private GridConfigurationModel Grid_Get_DataStruct(GRDWorkObject ObjvmWork)
        {
            //Catch  store Data
            GridConfigurationModel objStoreInfo = new GridConfigurationModel();
            objStoreInfo.StoreID = int.Parse((TempData["_SearchID"] == null ? "0" : TempData["_SearchID"]).ToString());
            objStoreInfo.GridObject.GridObjectID = Convert.ToInt32(ObjvmWork.GridObjectNameID);
            objStoreInfo.GridObject.GridObjectName = ObjvmWork.GridObjectName;
            objStoreInfo.GridObject.Disabled = Convert.ToBoolean(ObjvmWork.DisableGridObject);
            objStoreInfo.GridObject.GridEditable = Convert.ToBoolean(ObjvmWork.IsGrdEditable);

            objStoreInfo.GridObject.Grid_LstValues = new List<GridWorkObjectControlModel>();
            //Catch Grid Data
            for (int i = 0; i < ObjvmWork.Grid_LstValues.Count(); i++)
            {
                GridWorkObjectControlModel ObjItemAdd = new GridWorkObjectControlModel();
                if ((ObjvmWork.Grid_LstValues[i]).sDataType == "Character" && (ObjvmWork.Grid_LstValues[i]).iLength == 0)
                {
                    ViewData["Message"] = "Invalid Length";
                }
                ObjItemAdd.ObjectID = ObjvmWork.Grid_LstValues[i].iObjectID;
                ObjItemAdd.StoreID = ObjvmWork.Grid_LstValues[i].iStoreID;
                //catch ObjectName data in grid
                ObjItemAdd.ObjectName = Encoder.HtmlEncode(ObjvmWork.Grid_LstValues[i].sObjectName, false);
                //  ObjItemAdd.bWorkID = Convert.ToBoolean(ObjvmWork.Grid_LstValues[i].bWorkID);
                ObjItemAdd.ObjectDescription = Encoder.HtmlEncode(ObjvmWork.Grid_LstValues[i].sObjectDescription, false);
                ObjItemAdd.ObjectLabel = Encoder.HtmlEncode(ObjvmWork.Grid_LstValues[i].sObjectLabel, false);
                ObjItemAdd.ObjectType = ObjvmWork.Grid_LstValues[i].iObjectType;
                ObjItemAdd.DataType = ObjvmWork.Grid_LstValues[i].sDataType;
                ObjItemAdd.Length = ObjvmWork.Grid_LstValues[i].iLength;
                //Catch Choice Data
                ObjItemAdd.Choice = GRDDataMerge(ObjvmWork.Grid_LstValues[i].UID);
                //ObjvmWork.Grid_LstValues[i].oChoice;
                //ObjItemAdd.oTranslateLanguage = ObjvmWork.WorkDefinitionGRD[i].oTranslateList;
                // ObjItemAdd.oChoice = ObjvmWork.WorkDefinitionGRD[i].oChoice;
                ObjItemAdd.ValidationID = ObjvmWork.Grid_LstValues[i].iValidationID;
                //Catch Checkbox data
                ObjItemAdd.Visible = Convert.ToBoolean(ObjvmWork.Grid_LstValues[i].bVisible);//Set textbox visibility in gent dashboard
                ObjItemAdd.Search = Convert.ToBoolean(ObjvmWork.Grid_LstValues[i].bSearch);//Set  Searchable textbox  in gent dashboard
                ObjItemAdd.Editable = Convert.ToBoolean(ObjvmWork.Grid_LstValues[i].bEditable);//Set  Editable textbox  in gent dashboard
                ObjItemAdd.Required = Convert.ToBoolean(ObjvmWork.Grid_LstValues[i].bRequired);//Set  Required textbox  in gent dashboard
                ObjItemAdd.Disabled = Convert.ToBoolean(ObjvmWork.Grid_LstValues[i].bDisabled);//Set  Disabled textbox  in gent dashboard
                ObjItemAdd.Row_No = 0;
                ObjItemAdd.Column_No = 0;
                ObjItemAdd.Column_Span = 0;
                ObjItemAdd.ReportsOrderSearch = ObjvmWork.Grid_LstValues[i].iGrdColumeOrder;
                objStoreInfo.GridObject.Grid_LstValues.Add(ObjItemAdd);
            }
            return objStoreInfo;
        }
        public JsonResult SaveGridControlValues(GRDWorkObject GRDWorkObject)
        {
            string str = "";
            WorkDefinitionViewModel oWorkObject = new WorkDefinitionViewModel();
            try
            {
                WorkObject AddNewBlankRow = new WorkObject();
                //add new row in a grid
                oWorkObject.WorkDefinitionGRD.Add(AddNewBlankRow);
                // var val = Catch_GRD_Record(objWorkDefinitionViewModel);

                var GridObj = Grid_Get_DataStruct(GRDWorkObject);
                GridObj.ProcessID = Convert.ToInt32(GRDWorkObject.gProcessName);
                GridObj.CampaignID = Convert.ToInt32(GRDWorkObject.gCampaignName);
                string StoreID = Convert.ToString(TempData["iGridStoreId"]) == "" ? "0" : Convert.ToString(TempData["iGridStoreId"]) ?? "0";
                TempData.Keep("iGridStoreId");
                var bIsGridConfiguration = Convert.ToBoolean(TempData["bIsGridConfiguration"]);
                TempData.Keep("bIsGridConfiguration");
                if (!bIsGridConfiguration)
                {
                    this._repositoryStore.InsertGridConfiguration_Temp(GridObj, Convert.ToInt32(StoreID));
                }
                else
                {
                    if (GRDWorkObject.GridObjectNameID == "0")
                    {
                        this._repositoryStore.InsertGridConfiguration_Temp(GridObj, Convert.ToInt32(StoreID));
                    }
                    else
                    {
                        this._repositoryStore.UpdateGridTableMain(StoreID, false, GridObj);
                    }
                }
                str = "Hello";
            }
            catch (Exception er)
            {
                str = er.Message;
            }
            //Local_Data_Binding_Get_Default_Inline_Data();
            return Json(new
            {
                response = oWorkObject.WorkDefinitionGRD,
                lstReeview = str

            });

            // return Json(LstResult1: , JsonRequestBehavior.AllowGet);
        }

        public JsonResult TempGRDChoiceDataSave(List<BEWorkObjectChoice> objchoiceData)
        {
            TempData["ChoiceDataTempGRD"] = JsonConvert.SerializeObject(objchoiceData);

            TempData.Peek("ChoiceDataTempGRD");

            return Json("OK");
        }
        #endregion

        #region Search
        public ActionResult SearchView()
        {
            WorkDefinitionViewModel objWork = new WorkDefinitionViewModel();
            objWork.oTenant = base.oTenant;
            ClearTempData();
            return View(objWork);
        }
        private void ClearTempData()
        {
            TempData["GridObjControllName"] = null;
            TempData["lstTABList"] = null;
        }

        public ActionResult GetFilterList([DataSourceRequest] DataSourceRequest request, string iCampaignName, string sname)
        {
            WorkDefinitionViewModel objvm = new WorkDefinitionViewModel();
            objvm.oTenant = base.oTenant;
            sname = sname ?? "";
            objvm.oStore = _repositoryStore.GetStoreList(int.Parse(iCampaignName == "" ? "0" : iCampaignName), sname, true);
            return Json(objvm.oStore.ToDataSourceResult(request));
        }
        #endregion

        #region ApprovalView 
        public ActionResult ApprovalView()
        {
            WorkApproval oWork = new WorkApproval();
            oWork.oTenant = base.oTenant;
            return View(oWork);
        }
        public ActionResult WorkApproval_ReadP([DataSourceRequest] DataSourceRequest request, string dFrom, string dTo)
        {
            List<WorkApproval> lstWorkApp = new();

            var listWorkObject = this._repositoryWork.GetPendingWorkObjectApproval(Convert.ToDateTime(dFrom), Convert.ToDateTime(dTo));
            lstWorkApp = listWorkObject.Select(x => new WorkApproval()
            {
                ApprovalId = x.ApprovalId,
                BusinessApprover = x.BusinessApprover ?? "",
                BusinessApproverId = x.BusinessApproverId,
                CampaignName = x.CampaignName ?? "",
                ProcessName = x.ProcessName ?? "",
                ChangeRequest = x.ChangeRequest ?? "",
                ClientName = x.ClientName ?? "",
                CreatedBy = x.CreatedBy,
                KeyBenfits = x.KeyBenfits ?? "",
                StoreName = x.StoreName ?? "",
                RequestCreator = x.RequestCreator ?? "",
                RequestedOn = x.RequestedOn ?? "",
                TechApprover = x.TechApprover ?? "",
                Status = x.Status ?? "",
                StatusToShowHideButtons = x.StatusToShowHideButtons ?? "",
                TechnologyApproverId = x.TechnologyApproverId
            }).ToList();

            return Json(lstWorkApp.ToDataSourceResult(request));
        }
        [HttpGet]
        public ActionResult Details(int level, int iApprovalId)
        {
            WorkDefinitionViewModel Viewmodel = new WorkDefinitionViewModel();
            Viewmodel.oTenant = base.oTenant;
            var dtWorkObjReqDetails = this._repositoryWork.FetchWorkObjRequestDetails(iApprovalId);
            if (dtWorkObjReqDetails != null && dtWorkObjReqDetails.ClientName != null)
            {
                Viewmodel.IsLevel = level;
                Viewmodel.iApproverId = iApprovalId;
                Viewmodel.RequestCreator = dtWorkObjReqDetails.Requestor.ToString();
                Viewmodel.ClientName = dtWorkObjReqDetails.ClientName.ToString();
                Viewmodel.ProcessName = dtWorkObjReqDetails.ProcessName.ToString();
                Viewmodel.CampaignName = dtWorkObjReqDetails.CampaignName.ToString();
                Viewmodel.WorkDefinitionName = dtWorkObjReqDetails.StoreName.ToString();
                Viewmodel.Location = dtWorkObjReqDetails.Location.ToString();
                Viewmodel.ShiftWindow = dtWorkObjReqDetails.Shiftwindow.ToString();
                string[] arrPurpose = dtWorkObjReqDetails.Purpose.ToString().Split(',');
                for (int i = 0; i < arrPurpose.Count(); i++)
                {
                    if (arrPurpose[i].ToString().Trim() == "Work Management")
                    {
                        Viewmodel.PurposeofcreationofWork.AddRange(new List<string> { Resources_Campaign.display_WorkManagement });
                    }
                    else if (arrPurpose[i].ToString().Trim() == "Time Tracking")
                    {
                        Viewmodel.PurposeofcreationofWork.AddRange(new List<string> { Resources_Campaign.display_TimeTracking });
                    }
                    else if (arrPurpose[i].ToString().Trim() == "Transaction Monitoring")
                    {
                        Viewmodel.PurposeofcreationofWork.AddRange(new List<string> { Resources_Campaign.display_TransactionsMonitoring });
                    }
                }
                Viewmodel.Q1 = int.Parse(dtWorkObjReqDetails.TargetUsersQ1.ToString());
                Viewmodel.Q2 = int.Parse(dtWorkObjReqDetails.TargetUsersQ2.ToString());
                Viewmodel.Q3 = int.Parse(dtWorkObjReqDetails.TargetUsersQ3.ToString());
                Viewmodel.Y1 = int.Parse(dtWorkObjReqDetails.TargetUsersY1.ToString());
                Viewmodel.Y2 = int.Parse(dtWorkObjReqDetails.TargetUsersY2.ToString());
                Viewmodel.Y3 = int.Parse(dtWorkObjReqDetails.TargetUsersY3.ToString());
                Viewmodel.BusinessJustifications = dtWorkObjReqDetails.BusinessJustification.ToString();
                Viewmodel.KeyBenefits = dtWorkObjReqDetails.KeyBenfits.ToString();
                Viewmodel.ChangeRequest = dtWorkObjReqDetails.ChangeRequest.ToString();
                Viewmodel.ChangeRequestStatus = dtWorkObjReqDetails.ChangeRequestStatus.ToString();
            }
            return PartialView("~/Views/WorkDefinition/_WorkRequest.cshtml", Viewmodel);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public JsonResult SaveRequestDetail(BEWorkObjectApprover objBEWorkObjectApprover, int? UserLevel, string ChangeReq)
        {

            objBEWorkObjectApprover.iModifiedBy = base.oUser.iUserID;
            if (UserLevel == 1)
            {
                _repositoryWork.InsertObjectRequestChange(objBEWorkObjectApprover.iApprovalId, UserLevel.Value, ChangeReq);
            }
            else if (UserLevel == 0)
            {
                _repositoryWork.UpdateObjectRequestChange(objBEWorkObjectApprover);
            }

            return Json(new { strMessage = "Save" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Approval(int iApprovalId, string action)
        {
            return Json(ApprovalAction(iApprovalId, action));
        }
        /// <summary>
        /// To  insert the data in table
        /// </summary>
        /// <param name="iApprovalId"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        [ValidateAntiForgeryToken]
        private JsonResult ApprovalAction(int iApprovalId, string action)
        {
            try
            {
                string Status = "";

                if (action == BPA.GlobalResources.UI.WorkManagement.Resource_WorkDefinition.display_Approve)
                {
                    Status = "Approved";
                }
                else if (action == BPA.GlobalResources.UI.WorkManagement.Resource_WorkDefinition.display_Reject)
                {
                    Status = "Rejected";
                }
                else if (action == BPA.GlobalResources.UI.WorkManagement.Resource_WorkDefinition.display_Cancel)
                {
                    Status = "Cancelled";
                }
                _repositoryStore.ApprovalAction(iApprovalId, Status, int.Parse(BPA.GlobalResources.Resources_FormID.WorkApproval));

            }
            catch (Exception ex)
            {

                return Json(new
                {
                    strAction = action,
                    Successed = true,
                    msg = ex.Message.ToString()
                });

            }
            return Json(new { strAction = action, Successed = false, msg = "" });
            //  return Json("1", JsonRequestBehavior.AllowGet);          
            //return Json(action, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Call when multiple records selected in side grid for approval
        /// rejeted,and cancelled.
        /// </summary>
        /// <param name="updatedWorkApproval"></param>
        /// <param name="modelData"></param>
        /// <returns></returns>
        public ActionResult ApprovalAction(
         [Bind(Prefix = "updated")] List<WorkApproval> updatedWorkApproval,
         [Bind(Prefix = "modelData")] string modelData)
        {
            if (updatedWorkApproval != null && updatedWorkApproval.Count > 0)/*To perform operation after updating any existing record in grid*/
            {
                for (int i = 0; i < updatedWorkApproval.Count; i++)
                {
                    if (Convert.ToBoolean(updatedWorkApproval[i].IsChecked))
                    {
                        ApprovalAction(updatedWorkApproval[i].ApprovalId, modelData);
                    }
                }
            }
            else
            {
                modelData = "Please select at least one checkbox Option !";
            }
            return Json(modelData);
        }
        #endregion

        #region Page PreView Work master

        [HttpPost]
        [ValidateAntiForgeryToken] // Cross-Site Request Forgery fixed
        public JsonResult GetProcessID(string ClientProcesscampId = "")
        {
            if (ClientProcesscampId != "")
            {
                TempData["iCampaignID"] = ClientProcesscampId.Split('/')[0];
                TempData["iClientID"] = ClientProcesscampId.Split('/')[1];
                TempData["iProcessID"] = ClientProcesscampId.Split('/')[2];

                //TempData.Peek("iCampaignID");
                //TempData.Peek("iClientID");
                //TempData.Peek("iProcessID");
            }
            return Json("1");
        }
        public ActionResult _WorkPreview()
        {

            WorkDefinitionViewModel oWorkObject = new WorkDefinitionViewModel();
            oWorkObject.oTenant = base.oTenant;
            //  return View(oWorkObject);
            return View(oWorkObject);
        }
        public JsonResult FillWorkList()
        {
            int iCampaignID = 0;

            List<BEStoreInfo> ObjPreView = new List<BEStoreInfo>();
            if (TempData["iCampaignID"] != null)
            {
                iCampaignID = int.Parse(Convert.ToString(TempData["iCampaignID"]));
            }
            TempData.Keep("iCampaignID");
            ObjPreView = _repositoryStore.GetStoreList(iCampaignID, "", true);
            return Json(ObjPreView);
        }
        public ActionResult FillPriviewGrid([DataSourceRequest] DataSourceRequest request, int iStoreId)
        {
            List<BEWorkObject> ListPreview = new List<BEWorkObject>();
            ListPreview = _repositoryWork.GetObjectList(iStoreId, false);
            WorkDefinitionViewModel objWorkModel = new WorkDefinitionViewModel();
            objWorkModel.oTenant = base.oTenant;
            for (int i = 0; i < ListPreview.Count(); i++)
            {
                if (ListPreview[i].irow_No != 0)
                {
                    PreView ObjPre = new PreView();
                    ObjPre.iObjectID = ListPreview[i].iObjectID;
                    ObjPre.sObjectName = ListPreview[i].sObjectName;
                    ObjPre.sObjectLabel = ListPreview[i].sObjectLabel;

                    ObjPre.irow_No = ListPreview[i].irow_No;
                    ObjPre.PreViewselectedRow.Text = ListPreview[i].irow_No.ToString();
                    ObjPre.PreViewselectedRow.Text = ListPreview[i].irow_No.ToString();

                    ObjPre.icolumn_No = ListPreview[i].icolumn_No;
                    ObjPre.PreViewselectedcolumn.Text = ListPreview[i].icolumn_No.ToString();
                    ObjPre.PreViewselectedcolumn.Value = ListPreview[i].icolumn_No.ToString();

                    ObjPre.icolumn_Span = ListPreview[i].icolumn_Span;
                    ObjPre.PreViewselectedcolumnSpan.Text = ListPreview[i].icolumn_Span.ToString();
                    ObjPre.PreViewselectedcolumnSpan.Value = ListPreview[i].icolumn_Span.ToString();

                    objWorkModel.oPreView.Add(ObjPre);
                }
            }

            return Json(objWorkModel.oPreView.ToDataSourceResult(request));
        }
        public JsonResult GetResolutionCodeList()
        {
            List<BETerminationCodeInfo> objTerminationCodeList = new List<BETerminationCodeInfo>();
            int iCampaignID = 0;
            if (TempData["iCampaignID"] != null)
            {
                iCampaignID = int.Parse(Convert.ToString(TempData["iCampaignID"]));
                objTerminationCodeList = _repositoryAgentDashBoard.GetTerminationCodeDirect(iCampaignID).ToList();
            }
            TempData.Keep("iCampaignID");
            return Json(objTerminationCodeList);
        }

        public JsonResult FillDrpChoiceData(int iObjectId)
        {
            BEWorkObjectChoice objchoicedefault = null;
            if (bool.Parse(oTenant.ClientMultiLanguage))
            {
                objchoicedefault = new BEWorkObjectChoice(0, 0, 0, "Select", false, 0, "", 0, "", 0, "");
            }
            else
            {
                objchoicedefault = new BEWorkObjectChoice(0, 0, 0, "Select", false, 0, "", 0, "", 0);
            }
            IList<BEWorkObjectChoice> ObjChoice = new List<BEWorkObjectChoice>();
            ObjChoice = _repositoryWork.GetObjectChoiceList(iObjectId);
            ObjChoice.Insert(0, objchoicedefault);

            return Json(ObjChoice);
        }
        public void SetTargetObjectId(string param)
        {
            TempData["TargetObjectId"] = param;
            //TempData.Keep("TargetObjectId");
        }
        public PartialViewResult ObjectFormula(string iObjectId = "")
        {
            WorkDefinitionViewModel objWorkDefinitionViewModel = new WorkDefinitionViewModel();
            objWorkDefinitionViewModel.oTenant = base.oTenant;
            IList<BEObjectformula> objBEObjectformula;
            if (iObjectId != "")
            {
                objBEObjectformula = _repositoryWork.GetWorkObjectFormulaSearch(int.Parse(iObjectId));
                objWorkDefinitionViewModel.Events = objBEObjectformula[0].sDObjEvent.ToString();
                objWorkDefinitionViewModel.Formula = objBEObjectformula[0].sDisplayFormula.ToString();
                objWorkDefinitionViewModel.HiddenFormula = objBEObjectformula[0].sDformula.ToString();
            }
            return PartialView("_ObjectFormula", objWorkDefinitionViewModel);
        }
        public JsonResult GetWorkObjectForControlName()
        {

            int iCampaignID = 0;
            if (TempData["iCampaignID"] != null)
            {
                iCampaignID = int.Parse(Convert.ToString(TempData["iCampaignID"]));
            }
            TempData.Keep("iCampaignID");
            var WorkObjList = _repositoryWork.GetWorkObjectForControlName(iCampaignID).ToList();
            List<ObjectList> objList = new List<ObjectList>();
            for (int i = 0; i < WorkObjList.Count; i++)
            {
                ObjectList obj = new ObjectList();
                obj.ObjName = WorkObjList[i].ObjName.ToString();
                objList.Add(obj);
                obj = null;
            }
            return Json(objList);
        }
        public JsonResult Formula_Read([DataSourceRequest] DataSourceRequest request)
        {
            int iCampaignID = 0;
            if (TempData["iCampaignID"] != null)
            {
                iCampaignID = int.Parse(Convert.ToString(TempData["iCampaignID"]));
            }
            TempData.Keep("iCampaignID");
            return Json(_repositoryWork.GetFilterList_Formula(iCampaignID, "", true).ToDataSourceResult(request));
        }
        public JsonResult GetEventsList()
        {

            string TargetObjectId = string.Empty;
            if (TempData["TargetObjectId"] != null)
            {
                TargetObjectId = Convert.ToString(TempData["TargetObjectId"]).Trim();
            }
            TempData.Keep("TargetObjectId");


            Dictionary<string, string> ListEvents = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(TargetObjectId))
            {
                DataTable dtEvent = new DataTable();
                DataTable dt = new DataTable();
                dt.Columns.Add("event_Id");
                dt.Columns.Add("event_Name");
                dt.Columns.Add("event_RelId");
                dtEvent.Columns.Add("event_Id");
                dtEvent.Columns.Add("event_Name");
                dtEvent.Columns.Add("event_RelId");
                // dt.Rows.Add("0", "---Select---", "0");
                dt.Rows.Add("1", "LostKeyboradFocus", "1");
                dt.Rows.Add("2", "LostFocus", "1");
                dt.Rows.Add("3", "GotKeyboradFocus", "1");
                dt.Rows.Add("4", "GotFocus", "1");
                dt.Rows.Add("8", "Loaded", "1");
                dt.Rows.Add("9", "Unloaded", "3");
                dt.Rows.Add("13", "KeyUp", "3");
                dt.Rows.Add("14", "KeyDown", "3");
                dtEvent.Rows.Add("16", "TextChanged", "1");
                dtEvent.Rows.Add("18", "Click", "3");
                dtEvent.Rows.Add("19", "Checked", "3");
                dtEvent.Rows.Add("21", "Unchecked", "3");
                dtEvent.Rows.Add("23", "Click", "4");
                dtEvent.Rows.Add("24", "Checked", "4");
                dtEvent.Rows.Add("26", "Unchecked", "4");
                dtEvent.Rows.Add("28", "SelectionChanged", "5");
                dtEvent.Rows.Add("29", "SelectionChanged", "9");
                dtEvent.Rows.Add("30", "SelectionChanged", "6");
                dtEvent.Rows.Add("31", "Click", "7");
                dtEvent.Rows.Add("32", "Click", "8");
                dtEvent.Rows.Add("33", "Checked", "7");
                dtEvent.Rows.Add("34", "Checked", "8");
                dtEvent.Rows.Add("35", "Unchecked", "7");
                dtEvent.Rows.Add("36", "Unchecked", "8");
                dt.Rows.Add("37", "WorkEndClick", "10");
                dt.Rows.Add("38", "buttonClick", "10");
                dtEvent.Rows.Add("39", "SelectionChanged", "14");
                dt.Rows.Add("40", "StartCaseClick", "15");
                dt.Rows.Add("41", "GetFromQueue", "16");
                dt.Rows.Add("42", "SearchClick", "1");
                string[] conditions = TargetObjectId.Split('_');
                string condition = string.Empty;
                if (conditions.Length >= 2)
                {
                    condition = conditions[2];
                }
                DataRow[] dr = dtEvent.Select("event_RelId=" + condition);
                for (int i = 0; i < dr.Length; i++)
                {
                    dt.Rows.Add(dr[i][0], dr[i][1], dr[i][2]);
                }

                dt.DefaultView.Sort = "event_Id,event_Name";

                //List<ComboBoxPairs> cbp = new List<ComboBoxPairs>();
                //ListEvents.Add("0", "--Select--");
                for (int j = 0; j < dt.Rows.Count; j++)
                {
                    ListEvents.Add(dt.DefaultView[j][1].ToString(), dt.DefaultView[j][0].ToString());
                }
            }
            return Json(new SelectList(ListEvents, "Value", "Key"));
        }
        public JsonResult GetTargetlstObject()
        {
            int iCampaignID = 0;
            if (TempData["iCampaignID"] != null)
            {
                iCampaignID = int.Parse(Convert.ToString(TempData["iCampaignID"]));
            }
            TempData.Keep("iCampaignID");
            var WorkObjList = _repositoryWork.GetWorkObjectForFormula(iCampaignID);
            IList<ObjectList> objList = new List<ObjectList>();
            for (int i = 0; i < WorkObjList.Count; i++)
            {
                ObjectList obj = new ObjectList();
                obj.ObjName = WorkObjList[i].ObjName.ToString();
                obj.ObjValue = WorkObjList[i].ObjValue.ToString();
                objList.Add(obj);
                obj = null;
            }
            return Json(new SelectList(objList, "ObjValue", "ObjName"));
        }
        public JsonResult GetOperatorList()
        {
            Dictionary<string, string> ListOperators = new Dictionary<string, string>();
            ListOperators.Add("--Select--", "-1");
            ListOperators.Add("If", "if");
            ListOperators.Add("ElseIf", "else if");
            ListOperators.Add("Else", "else");
            ListOperators.Add("Concat", "+");
            ListOperators.Add("+", "+");
            ListOperators.Add("-", "-");
            ListOperators.Add("*", "*");
            ListOperators.Add("/", "/");
            ListOperators.Add("=", "=");
            ListOperators.Add("==", "==");
            ListOperators.Add("!=", "!=");
            ListOperators.Add("And", "&&");
            ListOperators.Add("Or", "||");
            ListOperators.Add("{", "{");
            ListOperators.Add("}", "}");
            ListOperators.Add("(", "(");
            ListOperators.Add(")", ")");
            ListOperators.Add(">", ">");
            ListOperators.Add(">=", ">=");
            ListOperators.Add("<", "<");
            ListOperators.Add("<=", "<=");
            ListOperators.Add("MessageBox.Show(", "MessageBox.Show(");
            ListOperators.Add("Current Date", "DateTime.Now.ToShortDateString()");
            ListOperators.Add(".AddDays(", ".AddDays(");
            ListOperators.Add("DateTime.Parse(", "DateTime.Parse(");
            ListOperators.Add("int.Parse(", "int.Parse(");
            ListOperators.Add(".ToString()", ".ToString()");
            ListOperators.Add(".Substring(", ".Substring(");
            ListOperators.Add("Throw Message(", "throw new NotImplementedException(");
            ListOperators.Add(",", ",");
            ListOperators.Add("0", "0");
            ListOperators.Add("1", "1");
            ListOperators.Add("2", "2");
            ListOperators.Add("3", "3");
            ListOperators.Add("4", "4");
            ListOperators.Add("5", "5");
            ListOperators.Add("6", "6");
            ListOperators.Add("7", "7");
            ListOperators.Add("8", "8");
            ListOperators.Add("9", "9");
            ListOperators.Add(";", ";");

            return Json(new SelectList(ListOperators, "Value", "Key"));
        }
        public JsonResult GetConstant()
        {
            Dictionary<string, string> ListConstant = new Dictionary<string, string>();
            ListConstant.Add("---Select---", "0");
            ListConstant.Add("LAN ID", "hdnLanId");
            ListConstant.Add("User ID", "hdnUserId");
            ListConstant.Add("Emp ID", "hdnEmpId");
            ListConstant.Add("Full Name", "hdnFullName");
            ListConstant.Add("Team", "hdnTeam");
            ListConstant.Add("Supervisor Emp ID", "hdnSupEmpId");
            ListConstant.Add("Supervisor Lan ID", "hdnSupLanId");
            ListConstant.Add("Supervisor Full Name", "hdnSupFullName");
            ListConstant.Add("Emp Location", "hdnEmpLocation");
            ListConstant.Add("Emp Role", "hdnEmpRole");
            ListConstant.Add("Emp Description", "hdnEmpDescription");
            ListConstant.Add("Emp ERP Client", "hdnEmpERPClient");
            ListConstant.Add("Emp Erp Process", "hdnEmpERPProcess");
            ListConstant.Add("Emp Compaign", "hdnEmpCompaign");
            ListConstant.Add("Current Date & Time", "hdnCurrentDatetime");
            ListConstant.Add("Skill Set Array", "HdnSkillSet");
            ListConstant.Add("Mode", "hdnMode");
            return Json(new SelectList(ListConstant, "Value", "Key"));

        }

        public void check()
        {
            if (TempData["HiddenFormula"] != null)
                if (TempData["HiddenFormula"].ToString() != null)
                {
                    //SWMFormulaValidator.FormulaGenerator objparse = new SWMFormulaValidator.FormulaGenerator();
                    FormulaGen objparse = new();
                    string formula = TempData["HiddenFormula"].ToString().Replace("Microsoft.Windows.Controls.DatePicker", "DatePicker").Replace("BPA.Desktop.SWMModule.Views.DateTimePicker", "DatePicker");
                    objparse.test("test", true, out strError);
                    objparse.parse(formula, true, out strError);
                }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SaveObjectFormula(WorkDefinitionViewModel ObjectFormulaViewModel)
        {

            string Return = string.Empty;
            int iCampaignID = 0;
            if (TempData["iCampaignID"] != null)
            {
                iCampaignID = int.Parse(Convert.ToString(TempData["iCampaignID"]));
            }
            TempData.Keep("iCampaignID");

            //TempData.Peek("iCampaignID");
            //TempData.Peek("iClientID");
            //TempData.Peek("iProcessID"); ;
            string dsobjectId = (string)TempData.Peek("TargetObjectId");

            BEObjectformula obj = new BEObjectformula();

            obj.iDObjID = Convert.ToInt32(ObjectFormulaViewModel.ObjFormulaID);
            obj.iCampaignID = iCampaignID;
            obj.iDSObjID = int.Parse(dsobjectId.Split('_')[1]);
            obj.sDObjEvent = ObjectFormulaViewModel.Events;

            TempData["HiddenFormula"] = ObjectFormulaViewModel.HiddenFormula.Replace("#$", "<").Replace("$#", ">").Replace("&lt;", "<").Replace("&gt;", ">");
            //TempData.Peek("HiddenFormula");
            obj.sDformula = ObjectFormulaViewModel.HiddenFormula.Replace("#$", "<").Replace("$#", ">").Replace("&lt;", "<").Replace("&gt;", ">");
            obj.sDisplayFormula = ObjectFormulaViewModel.Formula;
            obj.bDisabled = bool.Parse(ObjectFormulaViewModel.Disable);
            obj.iCreatedBy = oUser.iUserID;

            //check();
            Thread FirstThread = new Thread(new ThreadStart(check));//


            FirstThread.SetApartmentState(ApartmentState.STA);
            FirstThread.Priority = ThreadPriority.Highest;
            FirstThread.Start();

            //Will loop untill Function to Compile
            while (FirstThread.IsAlive)
            {

            }
            if (!FirstThread.IsAlive)
            {
                if (strError == "")
                {
                    if (ObjectFormulaViewModel.ObjFormulaID == null)
                    {
                        if (_repositoryWork.GetExistObjFormula(obj))
                        {
                            Return = "2";
                        }
                        else
                        {
                            _repositoryWork.InsertObjectformula(CatchRecord(obj));
                            Return = "1"; // Fromula Saved
                        }
                    }
                    else if (ObjectFormulaViewModel.ObjFormulaID != null)
                    {
                        Return = UpdateObjectFormula(ObjectFormulaViewModel);
                    }
                }
                else if (strError != "")
                {
                    Return = strError;
                }
            }
            return Json(Return);
        }
        public string UpdateObjectFormula(WorkDefinitionViewModel ObjectFormulaViewModel)
        {
            string strError = string.Empty;
            int iCampaignID = 0;
            if (TempData["iCampaignID"] != null)
            {
                iCampaignID = int.Parse(Convert.ToString(TempData["iCampaignID"]));
            }
            TempData.Keep("iCampaignID");

            string TargetObjectId = string.Empty;
            if (TempData["TargetObjectId"] != null)
            {
                TargetObjectId = Convert.ToString(TempData["TargetObjectId"]);
            }
            TempData.Keep("TargetObjectId");
            string Return = "Duplicate";

            string dsobjectId = TargetObjectId;
            string EventName = string.Empty;
            BEObjectformula obj = new BEObjectformula();

            obj.iDObjID = int.Parse(ObjectFormulaViewModel.ObjFormulaID);
            obj.iCampaignID = iCampaignID;
            obj.iDSObjID = int.Parse(dsobjectId.Split('_')[1]);
            obj.sDObjEvent = ObjectFormulaViewModel.Events;
            obj.sDformula = ObjectFormulaViewModel.HiddenFormula.Replace("#$", "<").Replace("$#", ">").Replace("&lt;", "<").Replace("&gt;", ">");
            obj.sDisplayFormula = ObjectFormulaViewModel.Formula;
            obj.bDisabled = bool.Parse(ObjectFormulaViewModel.Disable);
            obj.iCreatedBy = oUser.iUserID;
            //Function to Compile
            // objparse.parse(obj.sDformula, true, out strError);

            DataTable dtFormulaExist = new DataTable();
            _repositoryWork.InsertObjectformula(CatchRecord(obj));
            Return = "1"; // Fromula Saved          

            return Return;

        }

        private ObjectformulaModel CatchRecord(BEObjectformula oObject)
        {
            ObjectformulaModel model = new()
            {
                CampaignID = oObject.iCampaignID,
                DSObjID = oObject.iDSObjID,
                DObjID = oObject.iDObjID,
                DObjEvent = oObject.sDObjEvent,
                Dformula = oObject.sDformula,
                DisplayFormula = oObject.sDisplayFormula,
                Disabled = oObject.bDisabled,
                SavePointXML = oObject.sSavePointXML
            };
            return model;
        }

        [HttpPost]
        [ValidateAntiForgeryToken] // Cross-Site Request Forgery fixed
        public string getEditRecords(string objID = "")
        {
            IList<BEObjectformula> objBEObjectformula;
            string data = "";
            if (objID != "")
            {

                objBEObjectformula = _repositoryWork.GetWorkObjectFormulaSearch(int.Parse(objID));
                if (objBEObjectformula.Count > 0)
                {
                    //              0                                   1                                                       2                                                   3                                                       4                                                       5  
                    data = objBEObjectformula[0].iDObjID + "#$$" + objBEObjectformula[0].iDSObjID.ToString() + "#$$" + objBEObjectformula[0].sDObjEvent.ToString() + "#$$" + objBEObjectformula[0].sDisplayFormula.ToString() + "#$$" + objBEObjectformula[0].sDformula.ToString() + "#$$" + objBEObjectformula[0].bDisabled.ToString();
                }
                //TempData.Peek("iCampaignID");
                //TempData.Peek("iClientID");
                //TempData.Peek("iProcessID");
            }

            return data;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Delete(string iObjectId = "")
        {
            int result = 0;
            result = _repositoryWork.DeleteObjectFormula(int.Parse(iObjectId));
            return Json(result.ToString());
        }

        public string getSelectList(string param)
        {
            //JavaScriptSerializer ser = new JavaScriptSerializer();
            Dictionary<string, string> ddlOption = new Dictionary<string, string>();
            if (param == "1")
            {
                ddlOption.Add("--select--", "-1");
                ddlOption.Add("True", "Visibility.Hidden");
                ddlOption.Add("False", "Visibility.Visible");

            }
            else if (param == "2")
            {
                ddlOption.Add("--select--", "-1");
                ddlOption.Add("True", "true");
                ddlOption.Add("False", "false");
            }
            return JsonConvert.SerializeObject(ddlOption.ToArray());

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult UpdatePreViewData(string SubJsonString)
        {
            var Result = BPA.GlobalResources.UI.Resources_common.display_Ok;
            List<PreView> lstTreeData = JsonConvert.DeserializeObject<IEnumerable<PreView>>(SubJsonString).ToList() ?? new List<PreView>();

            UpdatePreViewDataModel dt = new UpdatePreViewDataModel();
            var objLayoutInfo = lstTreeData.Select(item => new UpdatePreViewDataModel()
            {
                DSObjID = item.iObjectID,
                RowNo = item.irow_No,
                ColumnNo = item.icolumn_No,
                ColumnSpan = item.icolumn_Span
            }).ToList();

            _repositoryWorkLayout.UpdateData(objLayoutInfo, base.iFormID);
            ViewData["Message"] = "Update Data successfully";
            return Json(Result);
        }

        #endregion

    }
}
