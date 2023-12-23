using BPA.AppConfig.BusinessEntity.Application;
using BPA.AppConfig.BusinessEntity.Config;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPA.AppConfig.Datalayer.Config
{
    public class DLProcessShiftWindow : IDisposable
    {
        private BETenant _oTenant = null;

        public DLProcessShiftWindow(BETenant oTenant)
        { _oTenant = oTenant; }


        public void Dispose()
        { _oTenant = null; }

        private const string SQL_SELECT_PROCESSID = @"Select ProcessMaster.ClientID,ProcessShiftWindowId,ProcessShiftWindow .ProcessId,LocationId,StartTime, EndTime,MaxTimeForBreak,ProcessShiftWindow .Disabled,ProcessShiftWindow .CreatedBy FROM Config.tblProcessMaster ProcessMaster INNER JOIN CDS_ProcessShiftWindow ProcessShiftWindow ON ProcessMaster.ProcessID=ProcessShiftWindow.ProcessId WHERE ProcessShiftWindow.ProcessId = @ProcessId";
        private const string SQL_UPDATE_PROCESSSHIFTWINDOW = @"UPDATE CDS_ProcessShiftWindow  SET ProcessId=@ProcessId,LocationId=@LocationId,StartTime=@StartTime,EndTime=@EndTime,Disabled=@Disabled,MaxTimeForBreak=@MaxTimeForBreak,ModifiedBy=@ModifiedBy , ModifiedOn=GetDate()  WHERE ProcessShiftWindowId=@ProcessShiftWindowId";

        private const string SQL_INSERT_PROCESSSHIFTWINDOW = @"INSERT INTO CDS_ProcessShiftWindow (ProcessID,LocationID,StartTime, EndTime,MaxTimeForBreak, Disabled,CreatedBy,CreatedOn) VALUES(@ProcessID,@LocationID,@StartTime, @EndTime,@MaxTimeForBreak, @Disabled,@CreatedBy,GetDate() )";
        private const string SQL_DELETE_PROCESSSHIFTWINDOW = @"DELETE FROM CDS_ProcessShiftWindow  WHERE ProcessShiftWindowId=@ProcessShiftWindowId";

        private const string SQL_SP_PROCESSSHIFTWINDOW = @"Usp_CDS_GetShiftWindowProcessList";
        private const string SQL_INSERT_PROCESSSHIFTCONFIG = @"[config].[USP_InsertProcessShiftconfiguration]";
        private const string SQL_UPDATE_PROCESSSHIFTCONFIG = @"[config].USP_UpdateProcessShiftconfiguration";
        private const string SQL_DELETE_PROCESSSHIFTCONFIG = @"[config].USP_DeleteProcessShiftconfiguration";
        private const string SQL_SELECT_PROCESSSHIFTCONFIG = @"select distinct PSC.ProcessShiftConfigId, PM.ProcessId,[ProcessName]= PM.ProcessName+' ('+LM.LocationName+')' from Config.tblProcessMaster PM
                                                               join Config.tblProcessShiftConfiguration PSC on PSC.ProcessId=PM.ProcessId
                                                               join Config.tblLocationMaster LM on PSC.LocationID=LM.LocationId
                                                               where PM.ProcessName like @ProcessName+'%'";
        private const string SQL_SELECT_PROCESSSHIFTCONFIGDETAIL = @"[config].[USP_GetProcessShiftconfigDetail]";
        private const string SQL_INSERT_BREAKALERTSUMMARY = @"declare @ProcessID as int
                                                            select @ProcessID=ProcessID from [Prompt].[CDS_P_UserProcessMap] where userid=@UserID
                                                            insert into CDS_S_BreakAlertSummary(ProcessID,UserID,BreakTime)values(@ProcessID,@UserID,GetDate() )";


        private const string PARAM_PROCESSSHIFTWINDOW = "@ProcessShiftWindowId";
        private const string PARAM_PROCESSID = "@ProcessID";
        private const string PARAM_LOCATIONID = "@LocationID";
        private const string PARAM_STARTTIME = "@StartTime";
        private const string PARAM_ENDTIME = "@EndTime";
        private const string PARAM_MAXTIMEFORBREAK = "@MaxTimeForBreak";
        private const string PARAM_DISABLED = "@Disabled";
        private const string PARAM_CREATEDBY = "@CreatedBy";
        private const string PARAM_MODIFIEDBY = "@ModifiedBy";
        private const string PARAM_PROCESSNAME = "@ProcessName";
        private const string PARAM_USERID = "@UserID";
        private const string PARAM_ACTIVEPROCESSLIST = "@ActiveProcessList";
        private const string PARAM_ProcessShiftConfigId = "@ProcessShiftConfigId";
        private const string PARAM_BREAKXML = "@xml";

        public List<BEProcessShiftWindow> GetProcessShift(int ProcessID)
        {
            List<BEProcessShiftWindow> lShift = new List<BEProcessShiftWindow>();

            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand;
            dbCommand = db.GetSqlStringCommand(SQL_SELECT_PROCESSID);
            db.AddInParameter(dbCommand, PARAM_PROCESSID, DbType.String, ProcessID);
            using (IDataReader rdr = db.ExecuteReader(dbCommand))
            {
                while (rdr.Read())
                {
                    BEProcessShiftWindow ShiftItem = new BEProcessShiftWindow(Convert.ToInt32(rdr["ClientID"]), Convert.ToInt32(rdr["ProcessShiftWindowId"]), Convert.ToInt32(rdr["ProcessID"]), Convert.ToInt32(rdr["LocationId"]), rdr["StartTime"].ToString(), rdr["EndTime"].ToString(), rdr["MaxTimeForBreak"].ToString(), Convert.ToInt32(rdr["CreatedBy"]), Convert.ToBoolean(rdr["Disabled"]));
                    lShift.Add(ShiftItem);
                    ShiftItem = null;

                }

            }
            return lShift;
        }
        public List<BEProcessInfo> GetProcessShiftWindow(int iLoggedinUserID, string ProcessName, bool bActiveProcess)
        {
            List<BEProcessInfo> lProcess = new List<BEProcessInfo>();

            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetStoredProcCommand(SQL_SP_PROCESSSHIFTWINDOW);
            db.AddInParameter(dbCommand, PARAM_PROCESSNAME, DbType.String, "" + ProcessName + "%");
            db.AddInParameter(dbCommand, PARAM_USERID, DbType.Int32, iLoggedinUserID);
            db.AddInParameter(dbCommand, PARAM_ACTIVEPROCESSLIST, DbType.Boolean, bActiveProcess);
            using (IDataReader rdr = db.ExecuteReader(dbCommand))
            {
                while (rdr.Read())
                {
                    BEProcessInfo objCamp = new BEProcessInfo(Convert.ToInt32(rdr["ProcessID"]), Convert.ToInt32(rdr["ClientID"]), rdr["ProcessName"].ToString() + " " + (Convert.ToBoolean(rdr["Disabled"]) ? "(Disabled)" : ""), rdr["Description"].ToString(), Convert.ToBoolean(rdr["Disabled"]), 0);
                    lProcess.Add(objCamp);
                    objCamp = null;
                }

            }
            return lProcess;
        }
        public int InsertProcessSiftConfig(string strBreakXml)
        {
            int rowsEffected = 0;
            try
            {

                Database db = DL_Shared.dbFactory(_oTenant);
                DbCommand dbCommand = db.GetStoredProcCommand(SQL_INSERT_PROCESSSHIFTCONFIG);
                db.AddInParameter(dbCommand, PARAM_BREAKXML, DbType.String, strBreakXml);
                using (DbConnection conn = db.CreateConnection())
                {
                    conn.Open();// Connection Open

                    using (DbTransaction trans = conn.BeginTransaction()) //Start Transaction
                    {
                        try
                        {
                            rowsEffected = db.ExecuteNonQuery(dbCommand, trans);
                            trans.Commit(); //Commit Transaction                           
                        }
                        catch (Exception ex)
                        {
                            trans.Rollback();//Transaction RollBack
                            throw ex;
                        }
                    }
                    conn.Close();
                }
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
            return rowsEffected;
        }
        public int UpdateProcessSiftConfig(string strBreakXml)
        {
            int rowsEffected = 0;
            try
            {


                Database db = DL_Shared.dbFactory(_oTenant);
                DbCommand dbUpdateShiftCommand = db.GetStoredProcCommand(SQL_UPDATE_PROCESSSHIFTCONFIG);
                db.AddInParameter(dbUpdateShiftCommand, PARAM_BREAKXML, DbType.String, strBreakXml);

                using (DbConnection conn = db.CreateConnection())
                {
                    conn.Open();// Connection Open
                    using (DbTransaction trans = conn.BeginTransaction()) //Start Transaction
                    {
                        try
                        {
                            rowsEffected = db.ExecuteNonQuery(dbUpdateShiftCommand, trans);
                            trans.Commit(); //Commit Transaction
                        }
                        catch (Exception ex)
                        {
                            trans.Rollback();//Transaction RollBack
                            throw ex;
                        }
                    }
                    conn.Close();
                }
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
            return rowsEffected;
        }
        public DataTable GetProcessShiftConfig(string sProcessName)
        {

            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand;
            dbCommand = db.GetSqlStringCommand(SQL_SELECT_PROCESSSHIFTCONFIG);
            db.AddInParameter(dbCommand, PARAM_PROCESSNAME, DbType.String, sProcessName);

            DataTable dt = new DataTable();
            dt = db.ExecuteDataSet(dbCommand).Tables[0];
            return dt;

        }
        public DataSet GetProcessShiftConfigDetail(int iProcessshiftConfigId)
        {
            DataSet ds = new DataSet();

            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand;
            dbCommand = db.GetStoredProcCommand(SQL_SELECT_PROCESSSHIFTCONFIGDETAIL);
            db.AddInParameter(dbCommand, PARAM_ProcessShiftConfigId, DbType.Int32, iProcessshiftConfigId);

            ds = db.ExecuteDataSet(dbCommand);
            return ds;

        }
    }
}
