using BPA.EmailManagement.BusinessEntity.ExternalRef.Application;
using BPA.EmailManagement.BusinessEntity.ExternalRef.WorkAllocation;
using BPA.EmailManagement.BusinessLayer.ExternalRef.WorkAllocation;
using Microsoft.Graph.TermStore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPA.EmailManagement.BusinessLayer.CacheData
{
    /// <summary>
    /// Cache used to store WorkItems
    /// </summary>
    public class BLWorkItems : IDisposable
    {
        private int _iCampaignID;
        BETenant _oTenant;
        CacheHelper _WorkItemsCache = null;
        //Vipul
        private string _sCulture = string.Empty;

        public BLWorkItems()
        {
            _WorkItemsCache = new CacheHelper("WorkItems");
        }
        public BLWorkItems(int CampaignID, BETenant oTenant, string culture = null)
        {
            _oTenant = oTenant;
            _iCampaignID = CampaignID;
            _sCulture = culture;
            _WorkItemsCache = new CacheHelper("WorkItems");
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BLWorkItems"/> class.
        /// </summary>
        /// <param name="CampaignID">The campaign ID.</param>
        public BLWorkItems(int CampaignID, BETenant oTenant)
        {
            _oTenant = oTenant;
            _iCampaignID = CampaignID;
            _WorkItemsCache = new CacheHelper("WorkItems");
        }

        public void Dispose()
        { }

        public string GetTableNameAndVersion
        {
            get { return GetCacheItems(_iCampaignID, _oTenant).TableNameAndVersion; }
        }

        private BEWorkObjectList GetCacheItems(int CampaignID, BETenant oTenant, string cultre = null)
        {
            if (oTenant.ClientMultiLanguage)
            {
                string cacheName = "WorkItem_" + oTenant.TenantID.ToString() + "_" + CampaignID.ToString();
                BEWorkObjectList oAppCacheObject = (BEWorkObjectList)_WorkItemsCache.GetFromCache(cacheName);
                if (oAppCacheObject == null || oAppCacheObject.sCulture != null || oAppCacheObject.sCulture.ToString() != cultre)
                {

                    oAppCacheObject = GetData(CampaignID, oTenant, cultre);
                    if (cultre == null)
                        oAppCacheObject.sCulture = string.Empty;
                    else
                        oAppCacheObject.sCulture = cultre;
                    _WorkItemsCache.AddToCache(cacheName, oAppCacheObject, TimeSpan.FromHours(8));
                }
                return oAppCacheObject;
            }
            else
            {
                string cacheName = "WorkItem_" + oTenant.TenantID.ToString() + "_" + CampaignID.ToString();
                BEWorkObjectList oAppCacheObject = (BEWorkObjectList)_WorkItemsCache.GetFromCache(cacheName);
                if (oAppCacheObject == null)
                {
                    oAppCacheObject = GetData(CampaignID, oTenant);
                    _WorkItemsCache.AddToCache(cacheName, oAppCacheObject, TimeSpan.FromHours(8));
                }
                return oAppCacheObject;
            }
        }

        private BEWorkObjectList GetData(int CampaignID, BETenant oTenant, string culture = null)
        {
            string TableName = null;
            string Version = null;
            string IsGenerateLetter = null;
            string IsRunTimeUploadRequired = null;
            BEWorkObjectList oCacheObject = new BEWorkObjectList();
            BLWorkObject oObject = new BLWorkObject();
            BLWorkRule objRule = new BLWorkRule();
            try
            {
                //WorkObject List
                int iDStoreID;
                using (BLAgentDashBoard oBlAgentDashBoard = new BLAgentDashBoard())
                {
                    bool isMailCampaign = false;
                    bool isPGT = false;
                    bool IsTabConfiguration = false;
                    bool isTicketLogger = false;
                    bool isDistributionBot = false;
                    int iIncreaseSearch = 0;
                    bool IsGridConfiguration = false;
                    iDStoreID = oBlAgentDashBoard.GetDStoreID(CampaignID, out isMailCampaign, out isPGT, out IsTabConfiguration, out iIncreaseSearch, out IsGridConfiguration, out isDistributionBot, oTenant);
                    oCacheObject.IsMailCampaign = isMailCampaign;
                    oCacheObject.IsPGTCampaign = isPGT;
                    oCacheObject.IsDistributionBot = isDistributionBot;
                    oCacheObject.iIncreaseSearch = iIncreaseSearch;
                    //Deepak -----------
                    // Dynamic Tab Details ---------------
                    oCacheObject.bIsTabConfiguration = IsTabConfiguration;
                    oCacheObject.bIsGridConfiguration = IsGridConfiguration;
                    if (IsTabConfiguration == true)
                    {
                        using (BLStore objBLStore = new BLStore())
                        {
                            oCacheObject.lstTABMaster = objBLStore.GetTabMasterList(iDStoreID, oTenant);
                        }

                    }
                    else
                    {
                        oCacheObject.lstTABMaster = null;
                    }
                    //Deepak -------------- Link Campaign Data
                    DataSet dslink = new DataSet();
                    //bool isLinkCampaign = false;
                    dslink = oBlAgentDashBoard.GetLinkCampaignData(CampaignID, oTenant);
                    if (dslink.Tables.Count > 0)
                    {
                        if (dslink.Tables[0].Rows.Count > 0)
                        {
                            oCacheObject.bIsLinkCampaign = true;
                            oCacheObject.lstLinkCampaignData = GetLinkCampaign_Data(dslink, oTenant);
                        }
                        else
                        {
                            oCacheObject.bIsLinkCampaign = false;
                            oCacheObject.lstLinkCampaignData = null;
                        }
                    }
                    else
                    {
                        oCacheObject.bIsLinkCampaign = false;
                    }
                    //oCacheObject.bIsTabConfiguration=
                    //Vipul
                    if (oTenant.ClientMultiLanguage)
                    {
                        oCacheObject.lstWorkObject = oObject.GetObjectList(Convert.ToInt32(iDStoreID), oTenant, culture);
                    }
                    else
                    {
                        oCacheObject.lstWorkObject = oObject.GetObjectList(Convert.ToInt32(iDStoreID), false, oTenant);
                    }
                    //oCacheObject.lstWorkObject = oObject.GetObjectList(Convert.ToInt32(iDStoreID), false,oTenant);
                    oCacheObject.lstSearchWorkObject = oObject.GetSearchableObject(iDStoreID, oTenant);
                    if (IsGridConfiguration == true)
                    {
                        oCacheObject.lstGRDWorkObject = GetGridData(oCacheObject.lstWorkObject, oTenant);
                    }
                    else
                    {
                        oCacheObject.lstGRDWorkObject = null;
                    }

                    oCacheObject.lstWorkObjectChoice = GetChoiceData(oCacheObject.lstWorkObject, oTenant);
                    oCacheObject.lstTerminationCode = oBlAgentDashBoard.GetTerminationCode(CampaignID, oTenant);
                    oCacheObject.lstBreakCode = oBlAgentDashBoard.GetBreakCode(CampaignID, oTenant);
                    oCacheObject.lstDelayCode = oBlAgentDashBoard.GetDelayCodes(oTenant);

                    DataSet ds = oBlAgentDashBoard.GetDStoreInfo(iDStoreID, CampaignID, oTenant);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        TableName = ds.Tables[0].Rows[0]["TableName"].ToString();
                        Version = ds.Tables[0].Rows[0]["Version"].ToString();
                        IsGenerateLetter = ds.Tables[0].Rows[0]["IsGenerateLetter"].ToString();
                        IsRunTimeUploadRequired = ds.Tables[0].Rows[0]["IsRunTimeUploadRequired"].ToString();
                    }
                    oCacheObject.WorkObjectFormulaEvent = GetWorkObjectFormula(CampaignID, oTenant);
                    oCacheObject.TableNameAndVersion = TableName + "," + Version + "," + IsGenerateLetter + "," + IsRunTimeUploadRequired;
                }
                //Work Rule object
                oCacheObject.lstRule = objRule.GetEventRule(iDStoreID, oTenant);
                return oCacheObject;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oObject = null;
                objRule = null;
            }
        }

        private List<BEGridConfiguration> GetLinkCampaign_Data(DataSet dslink, BETenant oTenant)
        {
            BEWorkObjectList objlink = new BEWorkObjectList();
            objlink.lstLinkCampaignData = new List<BEGridConfiguration>();
            BLWorkObject oObjectLink = new BLWorkObject();
            BEGridConfiguration objEntity = new BEGridConfiguration();
            objEntity.lstObject = new List<ObjectfiveSD>();
            int i = 0;
            foreach (DataRow dr in dslink.Tables[0].Rows)
            {
                //objMappingObject.lstObject[j].lstObjectIN = new List<MappingObject>();

                ObjectfiveSD objObjectfiveSD = new ObjectfiveSD();
                objObjectfiveSD.iDLinkCampaignID = Convert.ToInt32(dr["DGrdID"].ToString());
                objObjectfiveSD.iObjectID = dr["RowNum"].ToString();
                objObjectfiveSD.iObjProcessID = dr["ProcessID"].ToString();
                objObjectfiveSD.iObjCampaignID = dr["DCampaignID"].ToString();
                objObjectfiveSD.sDCampaignName = dr["DCampaignName"].ToString();
                objEntity.lstObject.Add(objObjectfiveSD);
                objEntity.lstObject[i].lstObjectIN = new List<MappingObjectSD>();

                DataSet dsGrdColumn = new DataSet();
                dsGrdColumn = oObjectLink.GetLinkCampaignColumn(Convert.ToInt32(dr["DGrdID"].ToString()), 0, 0, oTenant);
                if (dsGrdColumn.Tables.Count > 0)
                {
                    if (dsGrdColumn.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow drLC in dsGrdColumn.Tables[0].Rows)
                        {

                            MappingObjectSD objBE = new MappingObjectSD();



                            objBE.iMappID = 0;
                            objBE.iObjectID = dr["DGrdID"].ToString() + "_" + dr["RowNum"].ToString();
                            objBE.sColumnName = drLC["item"].ToString();
                            objBE.sRowNumTab = dr["RowNum"].ToString();

                            objEntity.lstObject[i].lstObjectIN.Add(objBE);

                        }

                    }
                }
                i++;
            }
            objlink.lstLinkCampaignData.Add(objEntity);
            return objlink.lstLinkCampaignData;
        }

        private List<BEStoreInfo> GetGridData(IList<BEWorkObject> lBEWorkObject, BETenant oTenant)
        {


            List<BEStoreInfo> lstGridAllData = new List<BEStoreInfo>();
            int countworkobject = lBEWorkObject.Count;
            BLWorkObject oObject = new BLWorkObject();
            if (countworkobject > 0)
            {


                for (int i = 0; i < countworkobject; i++)
                {
                    BEStoreInfo objStoreInfo = new BEStoreInfo();

                    objStoreInfo.iStoreId = lBEWorkObject[i].iStoreID;
                    objStoreInfo.iObjectid = lBEWorkObject[i].iObjectID;
                    objStoreInfo.iObjectTypeid = lBEWorkObject[i].iObjectType;
                    objStoreInfo.iTAB_ID = Convert.ToInt32(lBEWorkObject[i].iTAB_ID == "" ? "0" : lBEWorkObject[i].iTAB_ID);
                    if (lBEWorkObject[i].sObjectName != "TOPID")
                    {
                        DataSet ds = new DataSet();
                        ds = oObject.GetAllGridData(lBEWorkObject[i].iStoreID, lBEWorkObject[i].iObjectID, oTenant);
                        if (ds.Tables.Count > 0)
                        {
                            if (ds.Tables[0].Rows.Count > 0)
                            {


                                foreach (DataRow dr in ds.Tables[0].Rows)
                                {
                                    objStoreInfo.iGridObject = Convert.ToInt32(dr["GridID"].ToString());
                                    objStoreInfo.sGridObjectName = dr["GridName"].ToString();
                                    objStoreInfo.iRows = Convert.ToInt32(dr["Row_No"].ToString());
                                    objStoreInfo.iColumns = Convert.ToInt32(dr["Column_No"].ToString());
                                    objStoreInfo.bGridIsEditable = Convert.ToBoolean(dr["IsEditable"].ToString());
                                    //  objStoreInfo.is = Convert.ToBoolean(ObjvmWork.DisableGridObject);
                                    objStoreInfo.oWorkObjectGRD = new List<BEWorkObject>();
                                    foreach (DataRow drGrdColum in ds.Tables[1].Rows)
                                    {
                                        BEWorkObject ObjItemAdd = new BEWorkObject();

                                        ObjItemAdd.iObjectID = Convert.ToInt32(drGrdColum["DSObjID"].ToString());//DSObjectID
                                        ObjItemAdd.iStoreID = lBEWorkObject[i].iStoreID;
                                        //catch ObjectName data in grid DSObjName
                                        ObjItemAdd.sObjectName = drGrdColum["DSObjName"].ToString();
                                        ObjItemAdd.sObjectDescription = drGrdColum["DSObjName"].ToString();
                                        ObjItemAdd.sObjectLabel = drGrdColum["DSObjLabelText"].ToString();
                                        ObjItemAdd.iObjectType = Convert.ToInt32(drGrdColum["DSObjControlID"].ToString());
                                        ObjItemAdd.sDataType = drGrdColum["ControlType"].ToString();
                                        ObjItemAdd.iLength = Convert.ToInt32(drGrdColum["MaxLength"].ToString());
                                        ObjItemAdd.iValidationID = Convert.ToInt32(drGrdColum["ValidationID"].ToString());
                                        ObjItemAdd.bRequired = Convert.ToBoolean(drGrdColum["IsRequired"].ToString());
                                        ObjItemAdd.bEditable = Convert.ToBoolean(drGrdColum["IsEditable"].ToString());
                                        ObjItemAdd.sDataType = drGrdColum["ObjDataType"].ToString();
                                        ObjItemAdd.bSearch = Convert.ToBoolean(drGrdColum["IsSearchable"].ToString());
                                        int ControlType = Convert.ToInt32(drGrdColum["DSObjControlID"].ToString());
                                        if (ControlType == 6)
                                        {
                                            List<BEWorkObjectChoice> lstchoice = new List<BEWorkObjectChoice>();
                                            foreach (DataRow drddlChoice in ds.Tables[2].Rows)
                                            {
                                                BEWorkObjectChoice objchoice = new BEWorkObjectChoice();
                                                if (Convert.ToInt32(drGrdColum["DSObjID"].ToString()) == Convert.ToInt32(drddlChoice["DSObjID"].ToString()))
                                                {
                                                    objchoice.iObjectChoiceID = Convert.ToInt32(drddlChoice["DSObjChoiceID"].ToString());
                                                    objchoice.sChoiceValue = drddlChoice["ChoiceValue"].ToString();
                                                    lstchoice.Add(objchoice);

                                                }

                                            }
                                            ObjItemAdd.oChoice = lstchoice;

                                        }

                                        objStoreInfo.oWorkObjectGRD.Add(ObjItemAdd);

                                    }


                                }
                            }
                        }
                        lstGridAllData.Add(objStoreInfo);
                    }

                }
            }
            return lstGridAllData;

        }

        private IList<BEWorkObjectChoice> GetChoiceData(IList<BEWorkObject> lBEWorkObject, BETenant oTenant)
        {
            IList<BEWorkObjectChoice> lWorkobj = new List<BEWorkObjectChoice>();
            int countworkobject = lBEWorkObject.Count;
            if (countworkobject > 0)
            {
                using (BLWorkObject objWorkObject = new BLWorkObject())
                {
                    for (int i = 0; i < countworkobject; i++)
                    {
                        if (lBEWorkObject[i].iObjectType == 4 || lBEWorkObject[i].iObjectType == 5 || lBEWorkObject[i].iObjectType == 6 || lBEWorkObject[i].iObjectType == 8 || lBEWorkObject[i].iObjectType == 9 || lBEWorkObject[i].iObjectType == 14)
                        {
                            IList<BEWorkObjectChoice> ltemWorkobj = objWorkObject.GetObjectChoiceList(lBEWorkObject[i].iObjectID, oTenant);
                            if (ltemWorkobj != null)
                            {
                                for (int j = 0; j < ltemWorkobj.Count; j++)
                                {
                                    lWorkobj.Add(ltemWorkobj[j]);
                                }
                            }
                            ltemWorkobj = null;
                        }
                    }
                }
            }
            return lWorkobj;
        }

        private DataTable GetWorkObjectFormula(int iCampaignID, BETenant oTenant)
        {
            using (BLWorkObject objWork = new BLWorkObject())
            {
                return objWork.GetWorkObjectFormulaEventValues(iCampaignID, oTenant);
            }

        }
    }
}
