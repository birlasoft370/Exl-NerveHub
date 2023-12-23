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
using BPA.EmailManagement.BusinessEntity.ExternalRef.Config;

namespace BPA.EmailManagement.DataLayer.ExternalRef.WorkAllocation
{
    public class DLAgentDashBoard : IDisposable
    {
        private BETenant _oTenant = null;
        public DLAgentDashBoard(BETenant oTenant)
        {
            _oTenant = oTenant;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        { _oTenant = null; }


        private const string SQL_SELECT_GETDSTOREID = @"SELECT DSC.DStoreiD,ISNULL(IsEmail,0)IsEmail,ISNULL(IsDistributionBot,0)IsDistributionBot,ISNULL(IsPGT,0) IsPGT ,ISNULL(DS.IsTabConfiguration,0) IsTabConfiguration,ISNULL(DS.IncreaseSearch,0) IncreaseSearch,ISNULL(IsGridConfiguration,0) IsGridConfiguration FROM WM.tblDStoreCampaignMap (NOLOCK)DSC
                                                        INNER JOIN WM.tblDStore (NOLOCK)DS ON DS.DStoreID= DSC.DStoreID  WHERE  CampaignID=@CampaignID 
                                                        AND DStore_Camp_MapID = (SELECT MAX(DStore_Camp_MapID) FROM WM.tblDStoreCampaignMap (NOLOCK) WHERE Disabled=0 AND CampaignID=@CampaignID)";
        private const string SQL_SELECT_TERMINATIONCODE = @"SELECT a.TerminationID, a.TerminationName,b.IsEnd FROM WM.tblTerminationCodeMaster a (NOLOCK) JOIN WM.tblCampaignTermCodeMap b (NOLOCK) ON a.TerminationID=b.TerminationID WHERE b.CampaignID=@CampaignID AND b.Disabled=0 order by a.TerminationName";
        private const string SQL_SELECT_DELAYCODES = @"SELECT MasterID, [Value] FROM Config.tblMasterTable (NOLOCK) where FieldID=1";
        private const string SQL_SELECT_GETDSTOREINFO = @"SELECT a.CampaignWorkTable[TableName],b.Version[Version],b.DStoreName[StoreName],b.IsGenerateLetter,b.IsRunTimeUploadRequired FROM WM.tblDStoreCampaignMap a (NOLOCK) JOIN WM.tblDStore b (NOLOCK) ON a.DStoreID=b.DStoreID AND a.DStoreID=@DStoreID AND a.CampaignID=@CampaignID";



        private const string PARAM_CAMPAIGNID = "@CampaignID";
        private const string PARAM_DSTOREID = "@DStoreID";

        public int GetDStoreID(int CampaignID, out bool isEmailCampaign, out bool isPGT, out bool IsTabConfiguration, out int iIncreaseSearch, out bool IsGridConfiguration, out bool isDistributionBot)
        {
            DataSet ds = new DataSet();

            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbSelectCommand = db.GetSqlStringCommand(SQL_SELECT_GETDSTOREID);
            db.AddInParameter(dbSelectCommand, PARAM_CAMPAIGNID, DbType.Int32, CampaignID);
            db.LoadDataSet(dbSelectCommand, ds, "DStoreID");
            if (ds.Tables[0].Rows.Count > 0)
            {
                isEmailCampaign = bool.Parse(ds.Tables[0].Rows[0]["IsEmail"].ToString());
                isPGT = bool.Parse(ds.Tables[0].Rows[0]["IsPGT"].ToString());
                IsTabConfiguration = bool.Parse(ds.Tables[0].Rows[0]["IsTabConfiguration"].ToString());
                iIncreaseSearch = int.Parse(ds.Tables[0].Rows[0]["IncreaseSearch"].ToString());
                IsGridConfiguration = bool.Parse(ds.Tables[0].Rows[0]["IsGridConfiguration"].ToString());
                isDistributionBot = bool.Parse(ds.Tables[0].Rows[0]["IsDistributionBot"].ToString());
                return Convert.ToInt32(ds.Tables[0].Rows[0][0]);
            }
            else
            {
                isEmailCampaign = false;
                isPGT = false;
                IsTabConfiguration = false;
                iIncreaseSearch = 0;
                IsGridConfiguration = false;
                isDistributionBot = false;
                return 0;
            }
        }

        public List<BETerminationCodeInfo> GetTerminationCode(int CampaignID)
        {
            List<BETerminationCodeInfo> ds = new List<BETerminationCodeInfo>();

            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbSelectCommand = db.GetSqlStringCommand(SQL_SELECT_TERMINATIONCODE);
            db.AddInParameter(dbSelectCommand, PARAM_CAMPAIGNID, DbType.Int32, CampaignID);
            using (IDataReader rdr = db.ExecuteReader(dbSelectCommand))
            {
                while (rdr.Read())
                {
                    BETerminationCodeInfo oTermination = new BETerminationCodeInfo();
                    oTermination.iTerminationCodeID = Convert.ToInt32(rdr["TerminationID"]);
                    oTermination.sTermCodeName = rdr["TerminationName"].ToString();
                    oTermination.IsEnd = (rdr["IsEnd"] == System.DBNull.Value) ? false : Convert.ToBoolean(rdr["IsEnd"].ToString());
                    ds.Add(oTermination);
                    oTermination = null;
                }
            }
            return ds;
        }

        public List<BEMasterTable> GetDelayCodes()
        {
            List<BEMasterTable> lMasterList = new List<BEMasterTable>();

            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbSelectCommand = db.GetSqlStringCommand(SQL_SELECT_DELAYCODES);
            using (IDataReader rdr = db.ExecuteReader(dbSelectCommand))
            {
                while (rdr.Read())
                {
                    BEMasterTable oMasterList = new BEMasterTable();
                    oMasterList.iMasterId = Convert.ToInt32(rdr["MasterId"]);
                    oMasterList.sValue = rdr["Value"].ToString(); ;
                    lMasterList.Add(oMasterList);
                    oMasterList = null;
                }
            }
            return lMasterList;
        }

        public DataSet GetDStoreInfo(int DStoreID, int CampaignID)
        {
            DataSet ds = new DataSet();

            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbSelectCommand = db.GetSqlStringCommand(SQL_SELECT_GETDSTOREINFO);
            db.AddInParameter(dbSelectCommand, PARAM_DSTOREID, DbType.Int32, DStoreID);
            db.AddInParameter(dbSelectCommand, PARAM_CAMPAIGNID, DbType.Int32, CampaignID);
            db.LoadDataSet(dbSelectCommand, ds, "DStoreInfo");
            return ds;
        }
    }
}
