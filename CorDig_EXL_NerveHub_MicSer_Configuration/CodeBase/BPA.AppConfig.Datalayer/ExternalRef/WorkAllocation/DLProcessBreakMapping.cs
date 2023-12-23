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
using BPA.AppConfig.BusinessEntity.ExternalRef.WorkAllocation;

namespace BPA.AppConfig.Datalayer.ExternalRef.WorkAllocation
{
    public class DLProcessBreakMapping : IDisposable
    {
        private BETenant _oTenant = null;
        public DLProcessBreakMapping(BETenant oTenant)
        { _oTenant = oTenant; }
        public void Dispose()
        { _oTenant = null; }

        private const string SQL_INSERT_PROCESSBREAKMAP = @"INSERT INTO Config.tblProcessBreakMapping (ProcessID,BreakID, IsProductive,IncludeInBreakSchedule, Disabled, CreatedBy) VALUES(@ProcessID,@BreakID,@IsProductive,@IsBreakSchedule,@Disabled, @CreatedBy)";
        private const string SQL_CHECK_PROCESSBREAKMAP = @"SELECT COUNT(*) FROM Config.tblProcessBreakMapping (NoLock) where ProcessID = @ProcessID AND BreakID=@BreakID";
        private const string SQL_SP_PROCBRKMAPPEDLIST = @"Usp_CDS_GetProcessBreakMap";
        private const string SQL_SELECT_PROCBRKMAPPEDDETAILS = @"select ClientID,ProcessMaster.ProcessID ProcessID,BreakID,ProcessBreakMapping.Disabled,ProcessBreakMapping.IsProductive,ProcessBreakMapping.IncludeInBreakSchedule " +
                                                                "from Config.tblProcessBreakMapping ProcessBreakMapping (NoLock)" +
                                                                " INNER JOIN Config.tblProcessMaster ProcessMaster (NoLock)" +
                                                                " on ProcessBreakMapping.ProcessID =  ProcessMaster.ProcessID" +
                                                                " where ProcessMaster.ProcessID=@ProcessID";

        private const string SQL_DELETE_PROCESSBREAKMAP = "Delete from Config.tblProcessBreakMapping where ProcessID =@ProcessID ";

        private const string PARAM_PROCESSID = "@ProcessID";
        private const string PARAM_USERID = "@UserId";
        private const string PARAM_PROCESSNAME = "@ProcessName";
        private const string PARAM_BREAKID = "@BreakID";
        private const string PARAM_DISABLED = "@Disabled";
        private const string PARAM_PRODUCTIVE = "@IsProductive";
        private const string PARAM_CREATEDBY = "@CreatedBy";
        private const string PARAM_BREAKSCHEDULE = "@IsBreakSchedule";
        private const string PARAM_CLIENTID = "@ClientId";

        public List<BEProcessInfo> GetProcessBreakMappedList(string ProcessName, int UserId)
        {
            List<BEProcessInfo> lProcess = new List<BEProcessInfo>();

            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetStoredProcCommand(SQL_SP_PROCBRKMAPPEDLIST);
            db.AddInParameter(dbCommand, PARAM_PROCESSNAME, DbType.String, "" + ProcessName + "%");
            db.AddInParameter(dbCommand, PARAM_USERID, DbType.Int32, UserId);
            using (IDataReader rdr = db.ExecuteReader(dbCommand))
            {
                while (rdr.Read())
                {
                    BEProcessInfo objProcess = new BEProcessInfo(Convert.ToInt32(rdr["ProcessID"]), 0, rdr["ProcessName"].ToString(), "", true, 0);
                    lProcess.Add(objProcess);
                    objProcess = null;
                }

            }
            return lProcess;
        }
        public List<BEProcessInfo> GetProcessBreakMappedList(int ClientID, string ProcessName, int UserId)
        {
            List<BEProcessInfo> lProcess = new List<BEProcessInfo>();

            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetStoredProcCommand(SQL_SP_PROCBRKMAPPEDLIST);
            db.AddInParameter(dbCommand, PARAM_PROCESSNAME, DbType.String, "" + ProcessName + "%");
            db.AddInParameter(dbCommand, PARAM_CLIENTID, DbType.Int32, ClientID);
            db.AddInParameter(dbCommand, PARAM_USERID, DbType.Int32, UserId);
            using (IDataReader rdr = db.ExecuteReader(dbCommand))
            {
                while (rdr.Read())
                {
                    BEProcessInfo objProcess = new BEProcessInfo(Convert.ToInt32(rdr["ProcessID"]), 0, rdr["ProcessName"].ToString(), "", true, 0);
                    lProcess.Add(objProcess);
                    objProcess = null;
                }

            }
            return lProcess;
        }
        public BEProcessBreakMapping GetProcessBreakMappedDetails(int iProcessId)
        {
            BEProcessBreakMapping objProcessBreakMap = new BEProcessBreakMapping();

            DataTable dtProcessBreakMap = new DataTable("ProcessBreakMap");
            dtProcessBreakMap.Columns.Add("BreakId", System.Type.GetType("System.Int32"));
            dtProcessBreakMap.Columns.Add("IsProductive", System.Type.GetType("System.Boolean"));
            dtProcessBreakMap.Columns.Add("IncludeInBreakSchedule", System.Type.GetType("System.Boolean"));
            dtProcessBreakMap.Columns.Add("Disabled", System.Type.GetType("System.Int32"));


            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetSqlStringCommand(SQL_SELECT_PROCBRKMAPPEDDETAILS);
            db.AddInParameter(dbCommand, PARAM_PROCESSID, DbType.Int32, iProcessId);
            int iClientID = 0;
            using (IDataReader rdr = db.ExecuteReader(dbCommand))
            {
                while (rdr.Read())
                {
                    DataRow dr = dtProcessBreakMap.NewRow();
                    dr["BreakId"] = rdr["BreakID"];
                    dr["IsProductive"] = rdr["IsProductive"];
                    dr["IncludeInBreakSchedule"] = rdr["IncludeInBreakSchedule"];
                    dr["Disabled"] = rdr["Disabled"];
                    iClientID = Convert.ToInt32(rdr["ClientID"]);
                    dtProcessBreakMap.Rows.Add(dr);
                }
                objProcessBreakMap.iProcessId = iProcessId;
                objProcessBreakMap.iClientID = iClientID;
                objProcessBreakMap.dtProcessBreakMap = dtProcessBreakMap;
            }
            return objProcessBreakMap;
        }
        public void InsertData(BEProcessBreakMapping oProcessBrMap)
        {
            try
            {

                Database db = DL_Shared.dbFactory(_oTenant);
                DbCommand dbCommand = db.GetSqlStringCommand(SQL_INSERT_PROCESSBREAKMAP);
                db.AddInParameter(dbCommand, PARAM_BREAKID, DbType.Int32);
                db.AddInParameter(dbCommand, PARAM_PROCESSID, DbType.Int32, oProcessBrMap.iProcessId);
                db.AddInParameter(dbCommand, PARAM_PRODUCTIVE, DbType.Boolean);

                db.AddInParameter(dbCommand, PARAM_BREAKSCHEDULE, DbType.Boolean);
                db.AddInParameter(dbCommand, PARAM_DISABLED, DbType.Boolean);
                db.AddInParameter(dbCommand, PARAM_CREATEDBY, DbType.Int32, oProcessBrMap.iCreatedBy);

                //Query to check the Deuplicacy in DB
                DbCommand dbCheckDuplicacy = db.GetSqlStringCommand(SQL_CHECK_PROCESSBREAKMAP);
                db.AddInParameter(dbCheckDuplicacy, PARAM_BREAKID, DbType.Int32);
                db.AddInParameter(dbCheckDuplicacy, PARAM_PROCESSID, DbType.Int32, oProcessBrMap.iProcessId);


                using (DbConnection conn = db.CreateConnection())
                {
                    conn.Open();// Connection Open

                    using (DbTransaction trans = conn.BeginTransaction()) //Start Transaction
                    {
                        try
                        {
                            foreach (DataRow dr in oProcessBrMap.dtProcessBreakMap.Rows)
                            {
                                //For Each BreakId, Check if it alreday exits or not
                                db.SetParameterValue(dbCheckDuplicacy, PARAM_BREAKID, dr[0]);
                                int iCheckCounter = (int)db.ExecuteScalar(dbCheckDuplicacy, trans);

                                //If There is no entry in DB
                                if (iCheckCounter == 0)
                                {
                                    db.SetParameterValue(dbCommand, PARAM_BREAKID, dr[0]);
                                    db.SetParameterValue(dbCommand, PARAM_PRODUCTIVE, dr[1]);
                                    db.SetParameterValue(dbCommand, PARAM_BREAKSCHEDULE, dr[2]);
                                    db.SetParameterValue(dbCommand, PARAM_DISABLED, dr[3]);
                                    db.ExecuteNonQuery(dbCommand, trans);
                                }
                            }

                            trans.Commit();
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
        }
        public void UpdateData(BEProcessBreakMapping oProcessBrMap)
        {
            try
            {
                DeleteData(oProcessBrMap);
                InsertData(oProcessBrMap);
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
        public void DeleteData(BEProcessBreakMapping oProcessBrMap)
        {
            try
            {

                Database db = DL_Shared.dbFactory(_oTenant);
                DbCommand dbCommand = db.GetSqlStringCommand(SQL_DELETE_PROCESSBREAKMAP);
                db.AddInParameter(dbCommand, PARAM_PROCESSID, DbType.Int32, oProcessBrMap.iProcessId);
                db.ExecuteNonQuery(dbCommand);
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                /*
                if (ex.Number == 547)
                {
                    throw new ExceptionHandler.ExceptionType.DataAccessCustomException(BPA.GlobalResources.DataLayer.Resources.ReferenceConstraint);
                }
                if (ex.Number == 2627)
                {
                    throw new ExceptionHandler.ExceptionType.DataAccessCustomException(BPA.GlobalResources.DataLayer.Resources.UniqueConstraints);
                }*/
                throw;// new ExceptionHandler.ExceptionType.DataAccessException(ex.Message);
            }
            catch (Exception ex)
            {
                throw;// new ExceptionHandler.ExceptionType.DataAccessException(ex.Message);
            }
        }
    }
}
