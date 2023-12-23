using Microsoft.AspNetCore.Mvc;
using MicUI.WorkManagement.Models.ViewModels;
using MicUI.WorkManagement.Module.WorkManagement.WorkMaster;
using MicUI.WorkManagement.Services.ServiceModel;
using Newtonsoft.Json;
using System.Data;

namespace MicUI.WorkManagement.Controllers
{
    public class GridConfigurationController : Controller
    {
        private readonly IControlTypeService iControlTypeService;
        private readonly IStoreService iStoreService;
        private readonly IWorkObjectService iWorkObjectService;

        public GridConfigurationController(IControlTypeService iControlTypeService, IStoreService iStoreService, IWorkObjectService iWorkObjectService)
        {
            this.iControlTypeService = iControlTypeService;
            this.iStoreService = iStoreService;
            this.iWorkObjectService = iWorkObjectService;
        }

        public IActionResult Index()
        {
            WorkDefinitionViewModel oWorkObject = new WorkDefinitionViewModel();
            return View(oWorkObject);
        }
        public JsonResult FillObjectType()
        {
            TempData.Keep("sTMSetupId");
            return Json(iControlTypeService.GetControlTypeList("", true));
        }
        public JsonResult FillWDObjectType(string campID)
        {
            TempData.Keep("sTMSetupId");
            return Json(iStoreService.GetStoreObjectList(Convert.ToInt32(campID == "" ? "0" : campID), "", true, 118964));
        }
        [HttpPost]
        public JsonResult SaveData(TMSetupViewModel objBEFreeFieldkObject)
        {

            TMSetupViewModel obj = new TMSetupViewModel();
            obj.FreeFieldList = new List<TMSetupViewModel>();
            if (TempData["MappingData"] == null)
            {

                TempData["MappingData"] = JsonConvert.SerializeObject(objBEFreeFieldkObject);
                TempData.Keep("MappingData");
            }
            else
            {
                var oldvalues = JsonConvert.DeserializeObject<TMSetupViewModel>(TempData["MappingData"].ToString()) ?? new();
                for (int i = 0; i < objBEFreeFieldkObject.FreeFieldList.Count; i++)
                {
                    TMSetupViewModel x = new TMSetupViewModel();
                    x.p_gRD_id = objBEFreeFieldkObject.FreeFieldList[i].p_gRD_id;
                    x.FreeFieldID = objBEFreeFieldkObject.FreeFieldList[i].FreeFieldID;
                    x.FreeName = objBEFreeFieldkObject.FreeFieldList[i].FreeName;
                    x.FreeDescription = objBEFreeFieldkObject.FreeFieldList[i].FreeDescription;
                    x.iControlTypeID = objBEFreeFieldkObject.FreeFieldList[i].iControlTypeID;
                    x.DataTypeValue = objBEFreeFieldkObject.FreeFieldList[i].DataTypeValue;
                    x.ValidateId = objBEFreeFieldkObject.FreeFieldList[i].ValidateId;
                    x.IsRequired = objBEFreeFieldkObject.FreeFieldList[i].IsRequired;
                    x.IsCTC = objBEFreeFieldkObject.FreeFieldList[i].IsCTC;
                    x.IsVasible = objBEFreeFieldkObject.FreeFieldList[i].IsVasible;
                    x.Disable = objBEFreeFieldkObject.FreeFieldList[i].Disable;
                    x.IsAuditEmail = false;
                    x.MaxLength = objBEFreeFieldkObject.FreeFieldList[i].MaxLength;

                    obj.FreeFieldList.Add(x);
                }
                for (int i = 0; i < oldvalues.FreeFieldList.Count; i++)
                {
                    TMSetupViewModel Y = new TMSetupViewModel();
                    Y.p_gRD_id = oldvalues.FreeFieldList[i].p_gRD_id;
                    Y.FreeFieldID = oldvalues.FreeFieldList[i].FreeFieldID;
                    Y.FreeName = oldvalues.FreeFieldList[i].FreeName;
                    Y.FreeDescription = oldvalues.FreeFieldList[i].FreeDescription;
                    Y.iControlTypeID = oldvalues.FreeFieldList[i].iControlTypeID;
                    Y.DataTypeValue = oldvalues.FreeFieldList[i].DataTypeValue;
                    Y.ValidateId = oldvalues.FreeFieldList[i].ValidateId;
                    Y.IsRequired = oldvalues.FreeFieldList[i].IsRequired;
                    Y.IsCTC = oldvalues.FreeFieldList[i].IsCTC;
                    Y.IsVasible = oldvalues.FreeFieldList[i].IsVasible;
                    Y.Disable = oldvalues.FreeFieldList[i].Disable;
                    Y.IsAuditEmail = false;
                    Y.MaxLength = oldvalues.FreeFieldList[i].MaxLength;

                    obj.FreeFieldList.Add(Y);
                }
                TempData["MappingData"] = JsonConvert.SerializeObject(obj);
                TempData.Keep("MappingData");
            }


            return Json(new { strMessage = "Save" });

        }

        public JsonResult SaveFinalData(GridConfigurationViewModel objMappingObject)
        {
            iWorkObjectService.GridInserUpdateData(CatchRecord(objMappingObject), 11234, "IN");
            return Json(new { strMessage = "Save" });
        }

        private LinkCampaignGridConfigurationModel CatchRecord(GridConfigurationViewModel objMappingObject)
        {
            var oldvalues = JsonConvert.DeserializeObject<TMSetupViewModel>(TempData["MappingData"].ToString());
            LinkCampaignGridConfigurationModel objEntity = new LinkCampaignGridConfigurationModel();
            objEntity.ClientID = int.Parse(objMappingObject.iClientID);
            objEntity.ProcessID = int.Parse(objMappingObject.iProcessID);
            objEntity.CampaignID = int.Parse(objMappingObject.iCampaignID);

            for (int j = 0; j < objMappingObject.lstObject.Count; j++)
            {
                if (objMappingObject.lstObject[j].iObjCampaignID != null)
                {
                    ObjectfiveSDGrid objObjectfiveSD = new ObjectfiveSDGrid();
                    objObjectfiveSD.ObjectID = int.Parse(objMappingObject.lstObject[j].iObjectID);
                    objObjectfiveSD.ProcessID = int.Parse(objMappingObject.lstObject[j].iObjProcessID);
                    objObjectfiveSD.CampaignID = int.Parse(objMappingObject.lstObject[j].iObjCampaignID);
                    objObjectfiveSD.CPendingTransactions = objMappingObject.lstObject[j].bCPendingTransactions;
                    objEntity.lstObject.Add(objObjectfiveSD);

                    if (oldvalues != null)
                    {
                        for (int i = 0; i < oldvalues.FreeFieldList.Count; i++)
                        {
                            if (objMappingObject.lstObject[j].iObjectID == oldvalues.FreeFieldList[i].MaxLength.ToString())
                            {
                                MappingbjectSDGrid objBE = new MappingbjectSDGrid();
                                objBE.MappID = 0;
                                objBE.MaxLength = int.Parse(oldvalues.FreeFieldList[i].MaxLength.ToString());
                                objBE.SourceObjectID = int.Parse(oldvalues.FreeFieldList[i].iControlTypeID.ToString());
                                objBE.DestinationObjectID = int.Parse(oldvalues.FreeFieldList[i].DataTypeValue.ToString());
                                objBE.Disabled = oldvalues.FreeFieldList[i].Disable;
                                objEntity.lstObject[j].lstObjectIN.Add(objBE);
                            }
                        }
                    }
                }
            }
            return objEntity;
        }

        public JsonResult GetLinkCampaignMappingData(string iCampaignID, string RowNum)
        {
            DataSet ds = new DataSet();
            string PresentData = "NO";
            GridConfigurationViewModel objGCD = new GridConfigurationViewModel();
            ds = iWorkObjectService.GetAllLinkCampaignData(Convert.ToInt32(iCampaignID));
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[1].Rows.Count > 0)
                    {


                        // objGCD.lstObject = new List<Objectfive>();
                        objGCD.lstmappingObjects = new List<MappingObject>();
                        //foreach (DataRow dr in ds.Tables[0].Rows)
                        //{
                        //    Objectfive objin = new Objectfive();
                        //    objin.iDLinkCampaignID = Convert.ToInt32(dr["DGrdID"].ToString());
                        //    objin.iObjectID = dr["RowNum"].ToString();
                        //    objin.iObjProcessID = dr["ProcessID"].ToString();
                        //    objin.iObjCampaignID = dr["DCampaignID"].ToString();
                        //    objGCD.lstObject.Add(objin);
                        //}
                        foreach (DataRow dr2 in ds.Tables[1].Rows)
                        {
                            MappingObject objMapp = new MappingObject();
                            if (RowNum == dr2["RowNum"].ToString())
                            {
                                objMapp.iMappID = Convert.ToInt32(dr2["DGrdMappID"].ToString());
                                objMapp.iObjectID = dr2["RowNum"].ToString();
                                objMapp.iDLinkCampaignID = Convert.ToInt32(dr2["DGrdConfigID"].ToString());
                                objMapp.iSourceObjID = dr2["SourceObjectID"].ToString();
                                objMapp.iDestination = dr2["DestinationObjectID"].ToString();
                                objMapp.bDesabled = Convert.ToBoolean(dr2["Disabled"].ToString());
                                objGCD.lstmappingObjects.Add(objMapp);
                            }

                        }
                        PresentData = "YES";
                    }
                }
            }


            return Json(new { strMessage = PresentData, lstGridMapp = objGCD.lstmappingObjects });
        }
        public JsonResult GetLinkCampaignData(string iCampaignID)
        {
            DataSet ds = new DataSet();
            string PresentData = "NO";
            GridConfigurationViewModel objGCD = new GridConfigurationViewModel();
            ds = iWorkObjectService.GetAllLinkCampaignData(Convert.ToInt32(iCampaignID));
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    objGCD.lstObject = new List<Objectfive>();
                    objGCD.lstmappingObjects = new List<MappingObject>();
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        Objectfive objin = new Objectfive();
                        objin.iDLinkCampaignID = Convert.ToInt32(dr["DGrdID"].ToString());
                        objin.iObjectID = dr["RowNum"].ToString();
                        objin.iObjProcessID = dr["ProcessID"].ToString();
                        objin.iObjCampaignID = dr["DCampaignID"].ToString();
                        objin.bCPendingTransactions = Convert.ToBoolean(dr["IsCPendingTransactions"].ToString());
                        objGCD.lstObject.Add(objin);
                        PresentData = "YES";
                    }
                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        foreach (DataRow dr2 in ds.Tables[1].Rows)
                        {
                            MappingObject objMapp = new MappingObject();
                            objMapp.iMappID = Convert.ToInt32(dr2["DGrdMappID"].ToString());
                            objMapp.iObjectID = dr2["RowNum"].ToString();
                            objMapp.iDLinkCampaignID = Convert.ToInt32(dr2["DGrdConfigID"].ToString());
                            objMapp.iSourceObjID = dr2["SourceObjectID"].ToString();
                            objMapp.iDestination = dr2["DestinationObjectID"].ToString();
                            objMapp.bDesabled = Convert.ToBoolean(dr2["Disabled"].ToString());
                            objGCD.lstmappingObjects.Add(objMapp);

                        }
                        PresentData = "YES";
                    }
                }
            }


            return Json(new { strMessage = PresentData, lstGridData = objGCD.lstObject, lstGridMapp = objGCD.lstmappingObjects });
        }
    }
}
