using BPA.AppConfig.BusinessEntity.Application;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BPA.AppConfig.BusinessEntity.ExternalRef.WorkAllocation;

namespace BPA.AppConfig.Datalayer.ExternalRef.WorkAllocation
{
    public class DLStore : IDisposable
    {
        private BETenant _oTenant = null;
        public DLStore(BETenant oTenant)
        { _oTenant = oTenant; }
        public void Dispose()
        { _oTenant = null; }

        private const string SQL_SELECT_PROCEES_CLIENT_CAMP = @"Select Campaign.CampaignID, Process.ProcessID,Client.ClientID from Config.tblClientMaster Client inner join Config.tblProcessMaster Process on Client.ClientID=Process.ClientID  AND Process.Disabled=0 AND Client.Disabled=0" +
                                                         "INNER JOIN Config.tblCampaignMaster CampaIgn on Campaign.ProcessID=Process.ProcessID " +
                                                         "  AND CampaIgn.Disabled=0 WHERE Campaign.CampaignID=@CampID";
        private const string SQL_SELECT_STORE = @"WM.Usp_GetStoreList";
        private const string SQL_SELECT_STOREALL = @"WM.Usp_GetStoreList";

        private const string PARAM_CAMPID = "@CampID";
        private const string PARAM_ACTIVE = "@ActiveStoreList";
        private const string PARAM_STORENAME = "@StoreName";
        private const string PARAM_USERID = "@UserId";

        public DataSet GetClientProcessList(int iCampID)
        {
            DataSet ds = new DataSet();


            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetSqlStringCommand(SQL_SELECT_PROCEES_CLIENT_CAMP);
            db.AddInParameter(dbCommand, PARAM_CAMPID, DbType.Int32, iCampID);
            db.LoadDataSet(dbCommand, ds, "CampaignDetail");
            return ds;
        }
        public List<BEStoreInfo> GetStoreList(string StoreName, bool bActiveStore, int UserId)
        {
            return GetStoreList(0, StoreName, bActiveStore, UserId);
        }
        public List<BEStoreInfo> GetStoreList(int iCampaignId, string StoreName, bool bActiveStore, int UserId)
        {
            List<BEStoreInfo> lStoreInfo = new List<BEStoreInfo>();


            Database db = DL_Shared.dbFactory(_oTenant);

            DbCommand dbCommand;
            if (bActiveStore)
            {
                dbCommand = db.GetStoredProcCommand(SQL_SELECT_STORE);
                db.AddInParameter(dbCommand, PARAM_ACTIVE, DbType.Boolean, 1);
            }
            else
            {
                dbCommand = db.GetStoredProcCommand(SQL_SELECT_STOREALL);
                db.AddInParameter(dbCommand, PARAM_ACTIVE, DbType.Boolean, 0);
            }
            db.AddInParameter(dbCommand, PARAM_CAMPID, DbType.Int32, iCampaignId);
            db.AddInParameter(dbCommand, PARAM_STORENAME, DbType.String, "" + StoreName + "%");
            db.AddInParameter(dbCommand, PARAM_USERID, DbType.Int32, UserId);
            using (IDataReader rdr = db.ExecuteReader(dbCommand))
            {
                while (rdr.Read())
                {
                    BEStoreInfo objStore = new BEStoreInfo
                    {
                        iStoreId = Convert.ToInt32(rdr["DStoreID"]),
                        sStoreName = rdr["DStoreName"].ToString() + " " + (Convert.ToBoolean(rdr["Disabled"]) ? "(Disabled)" : ""),
                        sStoreDescription = rdr["DESCRIPTION"].ToString(),
                        iRows = Convert.ToInt32(rdr["Rows"]),
                        iColumns = Convert.ToInt32(rdr["Columns"]),
                        bGridObject = Convert.ToBoolean(rdr["IsGridConfiguration"].ToString() == "" ? false : rdr["IsGridConfiguration"])
                    };
                    lStoreInfo.Add(objStore);
                    objStore = null;
                }
            }
            return lStoreInfo;
        }
    }
}
