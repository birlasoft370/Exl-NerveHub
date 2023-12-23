using BPA.EmailManagement.BusinessEntity.ExternalRef.Application;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BPA.EmailManagement.BusinessEntity.ExternalRef.WorkAllocation;

namespace BPA.EmailManagement.DataLayer.ExternalRef.WorkAllocation
{
    public class DLWorkObject : IDisposable
    {
        private BETenant _oTenant = null;
        #region Constructor and Dispose

        /// <summary>
        /// Initializes a new instance of the <see cref="DLWorkObject"/> class.
        /// </summary>
        public DLWorkObject(BETenant oTenant)
        { _oTenant = oTenant; }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        { _oTenant = null; }

        #endregion Constructor and Dispose

        private const string PARAM_GETLINKCAMPAIGN = @"[WM].[Usp_GetLinkCampaign]";
        private const string PARAM_GetLinkColumnList = @"[WM].[GetCampaignLinkingColumnList]";
        private const string SP_SELECT_STOREOBJECTLANGUAGE = @"[WM].[Usp_WorkObjectLanguage]";
        private const string SQL_SELECTOBJECTCHOICEALL = @"SELECT DSObjChoiceID, DSObjID,GroupID=isnull(GroupID,0), ChoiceValue, Disabled,isnull(DSOrderID,0)as DSOrderID from [WM].[tblDStoreObjChoice] (NOLOCK) where DSObjID=@ObjID";
        private const string SP_SELECT_STOREOBJECT = @"[WM].[Usp_WorkObject]";
        private const string SQL_SELECT_SEARCHOBJECT = @"SELECT Obj.DSObjID,DStoreID,DSObjName, isnull(DSObjDescription,'') as DESCRIPTION,
                                                        DSObjLabelText, DSObjControlID, IsEditable, IsReadOnly, IsRequired, IsWorkID,
                                                        ObjDataType, MaxLength, Disabled, IsSearchable,isnull(IsSearchableSearch,0) IsSearchableSearch,isnull(IsReportOrderSearch,0) IsReportOrderSearch, isnull(RowNumber,0) RowNumber,
                                                        isnull(ColumnNumber,0) ColumnNumber, isnull(ColSpan,0) ColSpan,
                                                        ISNULL(ValidationID, 0) ValidationID,isnull(IsVisible,1) IsVisible
                                                        FROM WM.tblDStoreObjMap Obj (NOLOCK)
                                                        LEFT Outer join WM.tblDStoreLayout Layout (nolock)
                                                        on Obj.DSObjID=Layout.DSObjID
                                                        WHERE Obj.Disabled=0 and (Obj.IsSearchable=1 OR DSObjNAme='DateTime' or DSObjNAme='BatchCode') and Obj.DStoreID=@StoreID  order by isnull(IsReportOrderSearch,0)
                                                        ";
        private const string SQL_GET_GRIDDATA = @"[WM].[Usp_GetGridAllObj]";
        private const string SQL__GETWORKOBJECTFORMULAE = @"select * from WM.tblGetObjectFormula (NOLOCK) where CampaignID=@CampaignId and Disabled=0";
        private const string SQL_SELECTOBJECTCHOICE = "[WM].[Usp_WorkObjectChoice]";


        private const string PARAM_OBJID = "@ObjID";
        private const string PARAM_SCampaignId = "@SCampaignId";
        private const string PARAM_DGridId = "@DGridId";
        private const string PARAM_ISWORKID = "@WorkID";
        private const string PARAM_USERID = "@UserId";
        private const string PARAM_STOREID = "@StoreID";
        private const string PARAM_CULTURE = "@Culture";
        private const string PARAM_DOBJIDOBJECT = "@DSObjID";
        private const string PARAM_CAMPAIGNID = "@CampaignId";
        public DataSet GetAllLinkCampaignData(int SCampaignId)
        {
            DataSet ds = new DataSet();
            Database db = DL_Shared.dbFactory(_oTenant); ;
            DbCommand dbCommand = db.GetStoredProcCommand(PARAM_GETLINKCAMPAIGN);
            db.AddInParameter(dbCommand, PARAM_SCampaignId, DbType.Int32, SCampaignId);
            db.LoadDataSet(dbCommand, ds, "GridData");
            return ds;
        }

        public DataSet GetLinkCampaignColumn(int DGridId, int UserID, int WorkId)
        {
            DataSet ds = new DataSet();
            Database db = DL_Shared.dbFactory(_oTenant); ;
            DbCommand dbCommand = db.GetStoredProcCommand(PARAM_GetLinkColumnList);
            db.AddInParameter(dbCommand, PARAM_DGridId, DbType.Int32, DGridId);
            db.AddInParameter(dbCommand, PARAM_USERID, DbType.Int32, UserID);
            db.AddInParameter(dbCommand, PARAM_ISWORKID, DbType.Int32, WorkId);
            db.LoadDataSet(dbCommand, ds, "GridData");
            return ds;
        }

        public List<BEWorkObject> GetObjectListLang(int StoreId, string culture)
        {
            List<BEWorkObject> lObjectInfo = new List<BEWorkObject>();
            try
            {

                Database db = DL_Shared.dbFactory(_oTenant);
                DbCommand dbCommand;
                //Vipul
                dbCommand = db.GetStoredProcCommand(SP_SELECT_STOREOBJECTLANGUAGE);
                db.AddInParameter(dbCommand, PARAM_STOREID, DbType.String, StoreId);
                if (!string.IsNullOrEmpty(culture))
                    db.AddInParameter(dbCommand, PARAM_CULTURE, DbType.String, culture);
                using (IDataReader rdr = db.ExecuteReader(dbCommand))
                {
                    while (rdr.Read())
                    {
                        BEWorkObject objObject = new BEWorkObject
                        {
                            iObjectID = Convert.ToInt32(rdr["DSObjID"]),
                            iStoreID = Convert.ToInt32(rdr["DStoreID"]),
                            sObjectName = rdr["DSObjName"].ToString(),
                            sObjectDescription = rdr["DESCRIPTION"].ToString(),
                            sObjectLabel = rdr["DSObjLabelText"].ToString(),
                            iObjectType = Convert.ToInt32(rdr["DSObjControlID"]),
                            bEditable = Convert.ToBoolean(rdr["IsEditable"]),
                            bRequired = Convert.ToBoolean(rdr["IsRequired"]),
                            bReadOnly = Convert.ToBoolean(rdr["IsReadOnly"]),
                            bWorkID = Convert.ToBoolean(rdr["IsWorkID"]),
                            bIsUpload = Convert.ToBoolean(rdr["IsUpload"]),
                            sDataType = rdr["ObjDataType"].ToString(),
                            iLength = Convert.ToInt32(rdr["MaxLength"]),
                            bDisabled = Convert.ToBoolean(rdr["Disabled"]),
                            bTATComparison = Convert.ToBoolean(rdr["TATComparison"]),
                            sTATType = ((rdr["TATType"].ToString() == "HH") ? "Hour" : (rdr["TATType"].ToString() == "DD") ? "Day" : rdr["TATType"].ToString()),
                            bTargetTAT = Convert.ToBoolean(rdr["TargetTAT"]),
                            bUniqueID = Convert.ToBoolean(rdr["UniqueID"]),
                            bCustomerIdentifier = Convert.ToBoolean(rdr["CustomerIdentifier"]),
                            bSearch = Convert.ToBoolean(rdr["IsSearchable"]),
                            bSearchableSearch = Convert.ToBoolean(rdr["IsSearchableSearch"]),
                            iReportsOrderSearch = (rdr["IsReportOrderSearch"]).ToString(),
                            //iColSpan = Convert.ToInt32(rdr["ColSpan"]),
                            iValidationID = Convert.ToInt32(rdr["ValidationID"]),

                            bIsTranslate = Convert.ToBoolean(rdr["IsTranslate"]),
                            bVisible = Convert.ToBoolean(rdr["IsVisible"]),
                            irow_No = Convert.ToInt32(rdr["Row_No"]),
                            icolumn_No = Convert.ToInt32(rdr["Column_No"]),
                            icolumn_Span = Convert.ToInt32(rdr["Column_Span"]),
                            bSystemField = Convert.ToBoolean(rdr["IsSystemField"]),
                            iTAB_ID = Convert.ToString(rdr["TAB_ObjID"]),
                            iGridObjectMappID = Convert.ToString(rdr["DSGridObjectID"])

                        };
                        using (DLWorkObject objWorkObj = new DLWorkObject(_oTenant))
                        {
                            objObject.oChoice = (List<BEWorkObjectChoice>)objWorkObj.GetObjectChoiceList(objObject.iObjectID, true);
                        }
                        lObjectInfo.Add(objObject);
                        objObject = null;
                    }
                }
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                if (ex.Number == 547)
                {
                    throw;// new ExceptionHandler.ExceptionType.DataAccessCustomException(BPA.GlobalResources.DataLayer.Resources.ReferenceConstraint);
                }
                if (ex.Number == 2627)
                {
                    throw;// new ExceptionHandler.ExceptionType.DataAccessCustomException(BPA.GlobalResources.DataLayer.Resources.UniqueConstraints);
                }
                throw;// new ExceptionHandler.ExceptionType.DataAccessException(ex.Message);
            }
            catch (Exception ex)
            {
                throw;// new ExceptionHandler.ExceptionType.DataAccessException(ex.Message);
            }
            return lObjectInfo;
        }

        public List<BEWorkObjectChoice> GetObjectChoiceList(int ObjectId, bool bGetAll)
        {
            List<BEWorkObjectChoice> lChoiceInfo = new List<BEWorkObjectChoice>();
            //   Database db = DL_Shared.dbFactory(_oTenant);;

            Database db = DL_Shared.dbFactory(_oTenant);

            DbCommand dbCommand = db.GetSqlStringCommand(SQL_SELECTOBJECTCHOICEALL);
            db.AddInParameter(dbCommand, PARAM_OBJID, DbType.Int32, ObjectId);
            using (IDataReader rdr = db.ExecuteReader(dbCommand))
            {
                while (rdr.Read())
                {
                    BEWorkObjectChoice objChoice = new BEWorkObjectChoice()
                    {
                        iObjectChoiceID = Convert.ToInt32(rdr["DSObjChoiceID"]),
                        iGroupID = Convert.ToInt32(rdr["GroupID"]),
                        iObjID = Convert.ToInt32(rdr["DSObjID"]),
                        sChoiceValue = rdr["ChoiceValue"].ToString(),
                        bDisabled = Convert.ToBoolean(rdr["Disabled"]),
                        iOrder = Convert.ToInt32(rdr["DSOrderID"])
                    };
                    lChoiceInfo.Add(objChoice);
                    objChoice = null;
                }
            }

            return lChoiceInfo;
        }

        public List<BEWorkObject> GetObjectList(int StoreId, bool bGetAll)
        {
            List<BEWorkObject> lObjectInfo = new List<BEWorkObject>();


            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand;


            //dbCommand = db.GetSqlStringCommand(SP_SELECT_STOREOBJECT_NEW);
            //db.AddInParameter(dbCommand, PARAM_STOREID, DbType.String, StoreId);
            dbCommand = db.GetStoredProcCommand(SP_SELECT_STOREOBJECT);
            db.AddInParameter(dbCommand, PARAM_STOREID, DbType.String, StoreId);
            using (IDataReader rdr = db.ExecuteReader(dbCommand))
            {
                while (rdr.Read())
                {
                    BEWorkObject objObject = new BEWorkObject
                    {
                        iObjectID = Convert.ToInt32(rdr["DSObjID"]),
                        iStoreID = Convert.ToInt32(rdr["DStoreID"]),
                        sObjectName = rdr["DSObjName"].ToString(),
                        sObjectDescription = rdr["DESCRIPTION"].ToString(),
                        sObjectLabel = rdr["DSObjLabelText"].ToString(),
                        iObjectType = Convert.ToInt32(rdr["DSObjControlID"]),
                        bEditable = Convert.ToBoolean(rdr["IsEditable"]),
                        bRequired = Convert.ToBoolean(rdr["IsRequired"]),
                        bReadOnly = Convert.ToBoolean(rdr["IsReadOnly"]),
                        bWorkID = Convert.ToBoolean(rdr["IsWorkID"]),
                        bIsUpload = Convert.ToBoolean(rdr["IsUpload"]),
                        sDataType = rdr["ObjDataType"].ToString(),
                        iLength = Convert.ToInt32(rdr["MaxLength"]),
                        bDisabled = Convert.ToBoolean(rdr["Disabled"]),
                        bTATComparison = Convert.ToBoolean(rdr["TATComparison"]),
                        sTATType = ((rdr["TATType"].ToString() == "HH") ? "Hour" : (rdr["TATType"].ToString() == "DD") ? "Day" : rdr["TATType"].ToString()),
                        bTargetTAT = Convert.ToBoolean(rdr["TargetTAT"]),
                        bUniqueID = Convert.ToBoolean(rdr["UniqueID"]),
                        bCustomerIdentifier = Convert.ToBoolean(rdr["CustomerIdentifier"]),
                        bSearch = Convert.ToBoolean(rdr["IsSearchable"]),
                        //iRowNumber = Convert.ToInt32(rdr["RowNumber"]),
                        //iColumnNumber = Convert.ToInt32(rdr["ColumnNumber"]),
                        //iColSpan = Convert.ToInt32(rdr["ColSpan"]),
                        iValidationID = Convert.ToInt32(rdr["ValidationID"]),
                        bVisible = Convert.ToBoolean(rdr["IsVisible"]),
                        irow_No = Convert.ToInt32(rdr["Row_No"]),
                        icolumn_No = Convert.ToInt32(rdr["Column_No"]),
                        icolumn_Span = Convert.ToInt32(rdr["Column_Span"]),
                        bSystemField = Convert.ToBoolean(rdr["IsSystemField"]),
                        iTAB_ID = Convert.ToString(rdr["TAB_ObjID"]),
                        iGridObjectMappID = Convert.ToString(rdr["DSGridObjectID"])
                    };
                    using (DLWorkObject objWorkObj = new DLWorkObject(_oTenant))
                    {
                        objObject.oChoice = (List<BEWorkObjectChoice>)objWorkObj.GetObjectChoiceList(objObject.iObjectID, true);
                    }
                    lObjectInfo.Add(objObject);
                    objObject = null;
                }
            }
            return lObjectInfo;
        }

        public List<BEWorkObject> GetSearchableObject(int iStoreId)
        {
            List<BEWorkObject> lObjectInfo = new List<BEWorkObject>();

            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetSqlStringCommand(SQL_SELECT_SEARCHOBJECT);
            db.AddInParameter(dbCommand, PARAM_STOREID, DbType.Int32, iStoreId);
            using (IDataReader rdr = db.ExecuteReader(dbCommand))
            {
                while (rdr.Read())
                {
                    BEWorkObject objObject = new BEWorkObject
                    {
                        iObjectID = Convert.ToInt32(rdr["DSObjID"]),
                        iStoreID = Convert.ToInt32(rdr["DStoreID"]),
                        sObjectName = rdr["DSObjName"].ToString(),
                        sObjectDescription = rdr["DESCRIPTION"].ToString(),
                        sObjectLabel = rdr["DSObjLabelText"].ToString(),
                        iObjectType = Convert.ToInt32(rdr["DSObjControlID"]),
                        bEditable = Convert.ToBoolean(rdr["IsEditable"]),
                        bRequired = Convert.ToBoolean(rdr["IsRequired"]),
                        bReadOnly = Convert.ToBoolean(rdr["IsReadOnly"]),
                        bWorkID = Convert.ToBoolean(rdr["IsWorkID"]),
                        bSearchableSearch = Convert.ToBoolean(rdr["IsSearchableSearch"]),
                        iReportsOrderSearch = rdr["IsReportOrderSearch"].ToString(),
                        sDataType = rdr["ObjDataType"].ToString(),
                        iLength = Convert.ToInt32(rdr["MaxLength"]),
                        bDisabled = Convert.ToBoolean(rdr["Disabled"]),
                        bSearch = Convert.ToBoolean(rdr["IsSearchable"]),
                        iRowNumber = Convert.ToInt32(rdr["RowNumber"]),
                        iColumnNumber = Convert.ToInt32(rdr["ColumnNumber"]),
                        iColSpan = Convert.ToInt32(rdr["ColSpan"]),
                        iValidationID = Convert.ToInt32(rdr["ValidationID"]),
                        bVisible = Convert.ToBoolean(rdr["IsVisible"])
                    };
                    lObjectInfo.Add(objObject);
                    objObject = null;
                }
            }
            return lObjectInfo;
        }

        public DataSet GetAllGridData(int iStoreid, int iDObjectid)
        {
            DataSet ds = new DataSet();
            Database db = DL_Shared.dbFactory(_oTenant); ;
            DbCommand dbCommand = db.GetStoredProcCommand(SQL_GET_GRIDDATA);
            db.AddInParameter(dbCommand, PARAM_STOREID, DbType.Int32, iStoreid);
            db.AddInParameter(dbCommand, PARAM_DOBJIDOBJECT, DbType.Int32, iDObjectid);
            db.LoadDataSet(dbCommand, ds, "GridData");
            return ds;
        }

        public DataTable GetWorkObjectFormulaEvent(Int32 iCampId)
        {
            DataSet ds = new DataSet();
            Database db = DL_Shared.dbFactory(_oTenant); ;
            DbCommand dbCommand = db.GetSqlStringCommand(SQL__GETWORKOBJECTFORMULAE);
            db.AddInParameter(dbCommand, PARAM_CAMPAIGNID, DbType.Int32, iCampId);
            db.LoadDataSet(dbCommand, ds, "Template");
            DateTime clientDateTime = DateTime.Now.ToUniversalTime();
            ds.Tables[0].ExtendedProperties["UTCDifference"] = TimeZone.CurrentTimeZone.GetUtcOffset(clientDateTime).Ticks.ToString();
            return ds.Tables[0];
        }

        public List<BEWorkObjectChoice> GetObjectChoiceList(int ObjectId, int userID)
        {
            List<BEWorkObjectChoice> lChoiceInfo = new List<BEWorkObjectChoice>();
            Database db = DL_Shared.dbFactory(_oTenant); ;
            DbCommand dbCommand;

            dbCommand = db.GetStoredProcCommand(SQL_SELECTOBJECTCHOICE);
            db.AddInParameter(dbCommand, PARAM_OBJID, DbType.Int32, ObjectId);
            db.AddInParameter(dbCommand, PARAM_USERID, DbType.Int32, userID);

            using (IDataReader rdr = db.ExecuteReader(dbCommand))
            {
                while (rdr.Read())
                {
                    BEWorkObjectChoice objChoice = new BEWorkObjectChoice()
                    {
                        iObjectChoiceID = Convert.ToInt32(rdr["DSObjChoiceID"]),
                        iGroupID = Convert.ToInt32(rdr["GroupID"]),
                        iObjID = Convert.ToInt32(rdr["DSObjID"]),
                        sChoiceValue = rdr["ChoiceValue"].ToString(),
                        bDisabled = Convert.ToBoolean(rdr["Disabled"]),
                        iOrder = Convert.ToInt32(rdr["DSOrderID"])
                    };

                    lChoiceInfo.Add(objChoice);
                    objChoice = null;
                }
            }
            return lChoiceInfo;
        }
    }
}
