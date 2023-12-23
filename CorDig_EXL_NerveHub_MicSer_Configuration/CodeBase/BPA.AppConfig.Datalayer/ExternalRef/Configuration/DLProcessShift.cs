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

namespace BPA.AppConfig.Datalayer.ExternalRef.Configuration
{
    public class DLProcessShift : IDisposable
    {
        private BETenant _oTenant = null;
        public DLProcessShift(BETenant oTenant)
        { _oTenant = oTenant; }
        public void Dispose()
        { _oTenant = null; }

        private const string SQL_INSERT_PROCESSSHIFT = @"INSERT INTO Config.tblProcessShiftMapping(ProcessID, ShiftID, Disabled, CreatedBy) Values(@ProcessID, @ShiftID, @Disabled, @CreatedBy)";

        private const string SQL_SELECT_PROCESSSHIFT = @"SELECT Distinct Shift.ShiftID, Shift.ShiftName, ISNULL(Shift.Description, '') AS DESCRIPTION,
Shift.StartTime, Shift.EndTime, Shift.Disabled,Shift.CreatedBy ,     Shift.ShiftName + ' ('+  Shift.StartTime + ' - ' + Shift.EndTime +  ')'  AS NewShiftName                                                    
FROM Config.tblShiftMaster AS Shift WITH (NOLOCK) INNER JOIN
 Config.tblProcessShiftMapping AS ProcessShift WITH (NOLOCK) ON ProcessShift.ShiftID = Shift.ShiftID                                                       
 WHERE ProcessShift.ProcessID = @ProcessID  AND  ProcessShift.Disabled = 0";

        private const string SQL_CHECK_PROCESSSHIFT = @"Select * from Config.tblProcessShiftMapping (NOLOCK) WHERE ProcessID=@ProcessID AND ShiftID=@ShiftID";

        private const string SQL_INACTIVATE_SHIFT = @"UPDATE Config.tblProcessShiftMapping SET Disabled=1 WHERE ProcessID=@ProcessID";
        private const string SQL_ACTIVATE_SHIFT = @"UPDATE Config.tblProcessShiftMapping SET Disabled=0 WHERE ProcessID=@ProcessID AND ShiftID=@ShiftID";

        private const string PARAM_SHIFTID = "@ShiftID";
        private const string PARAM_PROCESSID = "@ProcessID";
        private const string PARAM_DISABLED = "@Disabled";
        private const string PARAM_CREATEDBY = "@CreatedBy";

        public List<BEProcessShiftInfo> GetProcessShift(int ProcessID)
        {
            List<BEProcessShiftInfo> lShift = new List<BEProcessShiftInfo>();
            List<BEShiftInfo> lShiftInfo = new List<BEShiftInfo>();

            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand;
            dbCommand = db.GetSqlStringCommand(SQL_SELECT_PROCESSSHIFT);
            db.AddInParameter(dbCommand, PARAM_PROCESSID, DbType.String, ProcessID);
            using (IDataReader rdr = db.ExecuteReader(dbCommand))
            {
                while (rdr.Read())
                {
                    BEShiftInfo ShiftItem = new BEShiftInfo(Convert.ToInt32(rdr["ShiftID"]), rdr["NewShiftName"].ToString(), rdr["DESCRIPTION"].ToString(), rdr["StartTime"].ToString(), rdr["EndTime"].ToString(), Convert.ToInt32(rdr["CreatedBy"]), Convert.ToBoolean(rdr["Disabled"]));
                    lShiftInfo.Add(ShiftItem);
                    ShiftItem = null;

                }
                BEProcessShiftInfo objShift = new BEProcessShiftInfo(ProcessID);
                objShift.oShift = lShiftInfo;
                lShift.Add(objShift);
                objShift = null;
            }
            return lShift;
        }

        public void InsertData(BEProcessShiftInfo oShift)
        {
            try
            {

                Database db = DL_Shared.dbFactory(_oTenant);
                DbCommand dbCommand = db.GetSqlStringCommand(SQL_INSERT_PROCESSSHIFT);
                db.AddInParameter(dbCommand, PARAM_PROCESSID, DbType.Int32, oShift.iProcessID);
                db.AddInParameter(dbCommand, PARAM_DISABLED, DbType.Boolean, oShift.bDisabled);
                db.AddInParameter(dbCommand, PARAM_CREATEDBY, DbType.Int32, oShift.iCreatedBy);
                db.AddInParameter(dbCommand, PARAM_SHIFTID, DbType.Int32);

                using (DbConnection conn = db.CreateConnection())
                {
                    conn.Open();// Connection Open

                    using (DbTransaction trans = conn.BeginTransaction()) //Start Transaction
                    {
                        try
                        {
                            int MemnerCount = oShift.oShift.Count;
                            for (int i = 0; i < MemnerCount; i++)
                            {
                                db.SetParameterValue(dbCommand, PARAM_SHIFTID, oShift.oShift[i].iShiftID);
                                db.ExecuteNonQuery(dbCommand, trans);
                            }
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
        }
        public void UpdateData(BEProcessShiftInfo oShift)
        {
            try
            {

                Database db = DL_Shared.dbFactory(_oTenant);

                DbCommand dbCommand = db.GetSqlStringCommand(SQL_INSERT_PROCESSSHIFT);
                db.AddInParameter(dbCommand, PARAM_PROCESSID, DbType.Int32, oShift.iProcessID);
                db.AddInParameter(dbCommand, PARAM_DISABLED, DbType.Boolean, oShift.bDisabled);
                db.AddInParameter(dbCommand, PARAM_CREATEDBY, DbType.Int32, oShift.iCreatedBy);
                db.AddInParameter(dbCommand, PARAM_SHIFTID, DbType.Int32);



                ////*********************************************************************************
                DbCommand dbCheckShiftCommand = db.GetSqlStringCommand(SQL_CHECK_PROCESSSHIFT);
                db.AddInParameter(dbCheckShiftCommand, PARAM_PROCESSID, DbType.Int32, oShift.iProcessID);
                db.AddInParameter(dbCheckShiftCommand, PARAM_SHIFTID, DbType.Int32);

                DbCommand dbInactivateShiftCommand = db.GetSqlStringCommand(SQL_INACTIVATE_SHIFT);
                db.AddInParameter(dbInactivateShiftCommand, PARAM_PROCESSID, DbType.Int32, oShift.iProcessID);

                DbCommand dbActivateShiftCommand = db.GetSqlStringCommand(SQL_ACTIVATE_SHIFT);
                db.AddInParameter(dbActivateShiftCommand, PARAM_PROCESSID, DbType.Int32, oShift.iProcessID);
                db.AddInParameter(dbActivateShiftCommand, PARAM_SHIFTID, DbType.Int32);

                using (DbConnection conn = db.CreateConnection())
                {
                    conn.Open();// Connection Open
                    using (DbTransaction trans = conn.BeginTransaction()) //Start Transaction
                    {
                        try
                        {
                            int MemberCount = oShift.oShift.Count;
                            db.ExecuteNonQuery(dbInactivateShiftCommand, trans);

                            for (int i = 0; i < MemberCount; i++)
                            {
                                db.SetParameterValue(dbCheckShiftCommand, PARAM_SHIFTID, oShift.oShift[i].iShiftID);
                                object oRowCounter = db.ExecuteScalar(dbCheckShiftCommand, trans);
                                if (oRowCounter == null)
                                {
                                    db.SetParameterValue(dbCommand, PARAM_SHIFTID, oShift.oShift[i].iShiftID);
                                    db.ExecuteNonQuery(dbCommand, trans);
                                }
                                else
                                {
                                    db.SetParameterValue(dbActivateShiftCommand, PARAM_SHIFTID, oShift.oShift[i].iShiftID);
                                    db.ExecuteNonQuery(dbActivateShiftCommand, trans);
                                }
                            }
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
        }
    }
}
