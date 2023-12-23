using BPA.AppConfig.BusinessEntity.Application;
using BPA.AppConfig.BusinessEntity.ExternalRef.WorkAllocation;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPA.AppConfig.Datalayer.ExternalRef.WorkAllocation
{
    public class DLBreakCode : IDisposable
    {
        private BETenant _oTenant = null;

        public DLBreakCode(BETenant oTenant)
        { _oTenant = oTenant; }

        public void Dispose()
        { _oTenant = null; }

        private const string SQL_SELECT_BREAK_PROCESSID = @"select TBM.BreakID,TBM.BreakName,TBM.Description,TBM.Disabled,TBM.CreatedBy, TBM.CreatedOn 
  from TM.tblBreakMaster TBM (NoLock)  inner join 
  Config.tblProcessBreakMapping     TPBM (NoLock) on
  TBM.BreakID=TPBM.BreakID   INNER JOIN Config.tblProcessMaster ProcessMaster (NoLock)
 on TPBM.ProcessID =  ProcessMaster.ProcessID where TBM.Disabled=0  and ProcessMaster.ProcessID=@ProcessID Order By BreakName";


        private const string SQL_SELECT_BREAK = @"select BreakID,BreakName,Description,Disabled,CreatedBy, CreatedOn from TM.tblBreakMaster (NoLock) where Disabled=0 and BreakName like @BreakName Order By BreakName";
        private const string SQL_SELECT_BREAKALL = @"select BreakID,BreakName,Description,Disabled,CreatedBy, CreatedOn from TM.tblBreakMaster (NoLock) where BreakName like @BreakName Order By BreakName";
        private const string SQL_SELECT_BREAKCODE = @"SELECT a.* FROM TM.tblBreakMaster a (NOLOCK) JOIN Config.tblProcessBreakMapping b (NOLOCK) on a.BreakID=b.BreakID JOIN Config.tblCampaignMaster c (NOLOCK) ON b.ProcessID=c.ProcessID WHERE c.CampaignID=@CampaignID AND b.Disabled=0";
        private const string SQL_SELECT_BREAKID = @"select BreakID,BreakName,Description,Disabled,CreatedBy, CreatedOn from TM.tblBreakMaster (NoLock) where BreakID = @BreakID";

        private const string SQL_INSERT_BREAK = @"insert into TM.tblBreakMaster(BreakName,Description,Disabled,CreatedBy) values(@BreakName,@Description,@Disabled,@CreatedBy)";
        private const string SQL_UPDATE_BREAK = @"Update TM.tblBreakMaster set BreakName=@BreakName,Description=@Description,Disabled=@Disabled,ModifiedBy=@ModifiedBy where BreakID=@BreakID";
        private const string SQL_DELETE_BREAK = @"Delete from TM.tblBreakMaster where BreakID = @BreakID";
        //added for Break for process
        private const string SQL_SELECT_BREAKCODEFORPROCESS = @"SELECT distinct a.* FROM TM.tblBreakMaster a (NOLOCK) 
                                                                JOIN Config.tblProcessBreakMapping b (NOLOCK) on a.BreakID=b.BreakID 
                                                                WHERE b.ProcessID=@ProcessID And b.Disabled=0 order by BreakName";

        private const string SP_SEARCH_BREACKNAME = @"WM.Usp_GetBreackNameBySearch";

        private const string PARAM_CAMPAIGNID = "@CampaignID";
        private const string PARAM_BREAKID = "@BreakID";
        private const string PARAM_BREAKNAME = "@BreakName";
        private const string PARAM_DESCRIPTION = "@Description";
        private const string PARAM_DISABLED = "@Disabled";
        private const string PARAM_CREATEDBY = "@CreatedBy";
        private const string PARAM_MODIFIEDBY = "@ModifiedBy";
        private const string PARAM_PROCESSID = "@ProcessID";
        public List<BEBreakInfo> GetBreakList(bool bGetActive)
        {
            return GetBreakList("", bGetActive);
        }
        public List<BEBreakInfo> GetBreakList(int iBreakID)
        {
            List<BEBreakInfo> lBreak = new List<BEBreakInfo>();

            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetSqlStringCommand(SQL_SELECT_BREAKID);
            db.AddInParameter(dbCommand, PARAM_BREAKID, DbType.String, iBreakID);
            using (IDataReader rdr = db.ExecuteReader(dbCommand))
            {
                while (rdr.Read())
                {
                    BEBreakInfo objBreak = new BEBreakInfo(Convert.ToInt32(rdr["BreakID"]), rdr["BreakName"].ToString(), rdr["Description"].ToString(), Convert.ToBoolean(rdr["Disabled"]), Convert.ToInt32(rdr["CreatedBy"]), Convert.ToDateTime(rdr["CreatedOn"]), false);
                    lBreak.Add(objBreak);
                    objBreak = null;
                }
            }
            return lBreak;
        }
        public List<BEBreakInfo> GetBreakList(string sBreakName, bool bGetActive)
        {
            List<BEBreakInfo> lBreak = new List<BEBreakInfo>();

            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = null;
            if (bGetActive)
                dbCommand = db.GetSqlStringCommand(SQL_SELECT_BREAK);
            else
                dbCommand = db.GetSqlStringCommand(SQL_SELECT_BREAKALL);

            db.AddInParameter(dbCommand, PARAM_BREAKNAME, DbType.String, "" + sBreakName + "%");
            using (IDataReader rdr = db.ExecuteReader(dbCommand))
            {
                while (rdr.Read())
                {
                    BEBreakInfo objBreak = new BEBreakInfo(Convert.ToInt32(rdr["BreakID"]), rdr["BreakName"].ToString() + " " + (Convert.ToBoolean(rdr["Disabled"]) ? "(Disabled)" : ""), rdr["Description"].ToString(), Convert.ToBoolean(rdr["Disabled"]), Convert.ToInt32(rdr["CreatedBy"]), Convert.ToDateTime(rdr["CreatedOn"]), false);
                    lBreak.Add(objBreak);
                    objBreak = null;
                }
            }
            return lBreak;
        }

        public List<BEBreakInfo> GetBreakListByProcessID(int iProcessID)
        {
            List<BEBreakInfo> lBreak = new List<BEBreakInfo>();

            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = null;
            dbCommand = db.GetSqlStringCommand(SQL_SELECT_BREAK_PROCESSID);

            db.AddInParameter(dbCommand, PARAM_PROCESSID, DbType.Int32, iProcessID);
            using (IDataReader rdr = db.ExecuteReader(dbCommand))
            {
                while (rdr.Read())
                {
                    BEBreakInfo objBreak = new BEBreakInfo(Convert.ToInt32(rdr["BreakID"]), rdr["BreakName"].ToString() + " " + (Convert.ToBoolean(rdr["Disabled"]) ? "(Disabled)" : ""), rdr["Description"].ToString(), Convert.ToBoolean(rdr["Disabled"]), Convert.ToInt32(rdr["CreatedBy"]), Convert.ToDateTime(rdr["CreatedOn"]), false);
                    lBreak.Add(objBreak);
                    objBreak = null;
                }
            }
            return lBreak;
        }

        public List<BEBreakInfo> GetBreakListBySearch(string sBreakName, int iProcessID)
        {
            List<BEBreakInfo> lBreak = new List<BEBreakInfo>();

            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = null;
            dbCommand = db.GetStoredProcCommand(SP_SEARCH_BREACKNAME);
            db.AddInParameter(dbCommand, PARAM_BREAKNAME, DbType.String, sBreakName);
            db.AddInParameter(dbCommand, PARAM_PROCESSID, DbType.Int32, iProcessID);
            using (IDataReader rdr = db.ExecuteReader(dbCommand))
            {
                while (rdr.Read())
                {
                    BEBreakInfo objBreak = new BEBreakInfo(Convert.ToInt32(rdr["BreakID"]), rdr["BreakName"].ToString() + " " + (Convert.ToBoolean(rdr["Disabled"]) ? "(Disabled)" : ""), rdr["Description"].ToString(), Convert.ToBoolean(rdr["Disabled"]), Convert.ToInt32(rdr["CreatedBy"]), Convert.ToDateTime(rdr["CreatedOn"]), false);
                    lBreak.Add(objBreak);
                    objBreak = null;
                }
            }
            return lBreak;
        }

        public void UpdateData(BEBreakInfo oBreak)
        {
            try
            {

                Database db = DL_Shared.dbFactory(_oTenant);
                DbCommand dbCommand = db.GetSqlStringCommand(SQL_UPDATE_BREAK);
                db.AddInParameter(dbCommand, PARAM_BREAKID, DbType.Int32, oBreak.iBreakID);
                db.AddInParameter(dbCommand, PARAM_BREAKNAME, DbType.String, oBreak.sBreakName);
                db.AddInParameter(dbCommand, PARAM_DESCRIPTION, DbType.String, oBreak.sBreakDescription);
                db.AddInParameter(dbCommand, PARAM_DISABLED, DbType.Boolean, oBreak.bDisabled);
                db.AddInParameter(dbCommand, PARAM_MODIFIEDBY, DbType.Int32, oBreak.iModifiedBy);
                db.ExecuteNonQuery(dbCommand);
            }
            /*
            catch (System.Data.SqlClient.SqlException ex)
            {
                if (ex.Number == 547)
                {
                    throw new ExceptionHandler.ExceptionType.DataAccessCustomException(BPA.GlobalResources.DataLayer.Resources.ReferenceConstraint);
                }
                if (ex.Number == 2627)
                {
                    throw new ExceptionHandler.ExceptionType.DataAccessCustomException(BPA.GlobalResources.DataLayer.Resources.UniqueConstraints);
                }
                throw new ExceptionHandler.ExceptionType.DataAccessException(ex.Message);
            }*/
            catch (Exception ex)
            {
                throw;// new ExceptionHandler.ExceptionType.DataAccessException(ex.Message);
            }
        }
        public void InsertData(BEBreakInfo oBreak)
        {
            try
            {

                Database db = DL_Shared.dbFactory(_oTenant);
                DbCommand dbCommand = db.GetSqlStringCommand(SQL_INSERT_BREAK);
                db.AddInParameter(dbCommand, PARAM_BREAKNAME, DbType.String, oBreak.sBreakName);
                db.AddInParameter(dbCommand, PARAM_DESCRIPTION, DbType.String, oBreak.sBreakDescription);
                db.AddInParameter(dbCommand, PARAM_DISABLED, DbType.Boolean, oBreak.bDisabled);
                db.AddInParameter(dbCommand, PARAM_CREATEDBY, DbType.Int32, oBreak.iCreatedBy);
                db.ExecuteNonQuery(dbCommand);
            }
            /*
            catch (System.Data.SqlClient.SqlException ex)
            {
                if (ex.Number == 547)
                {
                    throw new ExceptionHandler.ExceptionType.DataAccessCustomException(BPA.GlobalResources.DataLayer.Resources.ReferenceConstraint);
                }
                if (ex.Number == 2627)
                {
                    throw new ExceptionHandler.ExceptionType.DataAccessCustomException(BPA.GlobalResources.DataLayer.Resources.UniqueConstraints);
                }
                throw new ExceptionHandler.ExceptionType.DataAccessException(ex.Message);
            }*/
            catch (Exception ex)
            {
                throw;// new ExceptionHandler.ExceptionType.DataAccessException(ex.Message);
            }
        }
    }
}
