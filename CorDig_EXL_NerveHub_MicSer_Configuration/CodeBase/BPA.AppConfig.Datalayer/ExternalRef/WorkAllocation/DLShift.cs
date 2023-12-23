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
using BPA.AppConfig.BusinessEntity.Config;

namespace BPA.AppConfig.Datalayer.ExternalRef.WorkAllocation
{
    public class DLShift : IDisposable
    {
        private BETenant _oTenant = null;
        public DLShift(BETenant oTenant)
        { _oTenant = oTenant; }
        public void Dispose()
        { _oTenant = null; }

        private const string SQL_SELECT_SHIFTALL = @"SELECT ShiftID,ShiftName,isnull(Description,'') as DESCRIPTION,StartTime, EndTime, Disabled, [CreatedBy]=isnull(CreatedBy,0) FROM Config.tblShiftMaster (NOLOCK) WHERE   Config.tblShiftMaster.Disabled=0  and ShiftName Like @ShiftName  Order by ShiftName ";
        private const string SQL_SELECT_SHIFT = @"SELECT ShiftID,ShiftName,isnull(Description,'') as DESCRIPTION,StartTime, EndTime, Disabled,[CreatedBy]=isnull(CreatedBy,0) FROM Config.tblShiftMaster (NOLOCK) WHERE ShiftName Like @ShiftName Order by ShiftName ";
        private const string SQL_SELECT_SHIFTID = @"Select ShiftID,ShiftName,isnull(Description,'') as DESCRIPTION,StartTime, EndTime, Disabled,[CreatedBy]=isnull(CreatedBy,0) FROM Config.tblShiftMaster (NOLOCK)WHERE ShiftID = @ShiftID";
        private const string SQL_UPDATE_SHIFT = @"UPDATE Config.tblShiftMaster SET ShiftName=@ShiftName,Description=@Description,StartTime=@StartTime,EndTime=@EndTime,Disabled=@Disabled,ModifiedBy=@ModifiedBy , ModifiedOn=GetDate()  WHERE ShiftID=@ShiftID";

        private const string SQL_INSERT_SHIFT = @"if  exists (Select ShiftName from  Config.tblShiftMaster where ShiftName=@ShiftName)
                                                        Begin
                                                        Select 'Shift Name already exists'
                                                        End
                                                        Else
                                                        Begin
                                                        INSERT INTO Config.tblShiftMaster (ShiftName,Description,StartTime, EndTime, Disabled,CreatedBy)
                                                         VALUES(@ShiftName,@Description,@StartTime, @EndTime, @Disabled,@CreatedBy)
                                                         select ''
                                                         End";
        private const string SQL_DELETE_SHIFT = @"DELETE FROM Config.tblShiftMaster WHERE ShiftID=@ShiftID";

        private const string PARAM_SHIFTID = "@ShiftID";
        private const string PARAM_SHIFTNAME = "@ShiftName";
        private const string PARAM_SHIFTDESC = "@Description";
        private const string PARAM_STARTTIME = "@StartTime";
        private const string PARAM_ENDTIME = "@EndTime";
        private const string PARAM_DISABLED = "@Disabled";
        private const string PARAM_CREATEDBY = "@CreatedBy";
        private const string PARAM_MODIFIEDBY = "@ModifiedBy";
        public List<BEShiftInfo> GetShiftList(bool bGetAll)
        {
            return GetShiftList("", bGetAll);
        }
        public List<BEShiftInfo> GetShiftList(string ShiftName, bool bGetAll)
        {
            List<BEShiftInfo> lShift = new List<BEShiftInfo>();

            Database db = DL_Shared.dbFactory(_oTenant); //DatabaseFactory.CreateDatabase(DL_Shared.Connection);
            DbCommand dbCommand;
            if (bGetAll)
            {
                dbCommand = db.GetSqlStringCommand(SQL_SELECT_SHIFTALL);
            }
            else
            {
                dbCommand = db.GetSqlStringCommand(SQL_SELECT_SHIFT);
            }

            db.AddInParameter(dbCommand, PARAM_SHIFTNAME, DbType.String, "%" + ShiftName + "%");

            using (IDataReader rdr = db.ExecuteReader(dbCommand))
            {
                while (rdr.Read())
                {
                    BEShiftInfo objShift = new BEShiftInfo(Convert.ToInt32(rdr["ShiftID"]), rdr["ShiftName"].ToString() + " " + (Convert.ToBoolean(rdr["disabled"]) ? "(Disabled)" : ""), rdr["DESCRIPTION"].ToString(), rdr["StartTime"].ToString(), rdr["EndTime"].ToString(), Convert.ToInt32(rdr["CreatedBy"]), Convert.ToBoolean(rdr["Disabled"]));
                    lShift.Add(objShift);
                    objShift = null;
                }
            }
            return lShift;
        }

        public List<BEShiftInfo> GetShiftList(int ShiftID)
        {
            List<BEShiftInfo> lShiftInfo = new List<BEShiftInfo>();
            List<BEProcessInfo> lProcessInfo = new List<BEProcessInfo>();


            Database db = DL_Shared.dbFactory(_oTenant); //DatabaseFactory.CreateDatabase(DL_Shared.Connection);

            DbCommand dbCommand = db.GetSqlStringCommand(SQL_SELECT_SHIFTID);
            db.AddInParameter(dbCommand, PARAM_SHIFTID, DbType.Int32, ShiftID);

            using (IDataReader rdr = db.ExecuteReader(dbCommand))
            {
                // Scroll through the results
                while (rdr.Read())
                {
                    BEShiftInfo item = new BEShiftInfo(Convert.ToInt32(rdr["ShiftID"]), rdr["ShiftName"].ToString(), rdr["DESCRIPTION"].ToString(), rdr["StartTime"].ToString(), rdr["EndTime"].ToString(), Convert.ToInt32(rdr["CreatedBy"]), Convert.ToBoolean(rdr["Disabled"]));

                    lShiftInfo.Add(item);
                    item = null;
                }
            }
            return lShiftInfo;

        }

        public string InsertData(BEShiftInfo oShift)
        {
            try
            {
                string Message = "";

                Database db = DL_Shared.dbFactory(_oTenant); //DatabaseFactory.CreateDatabase(DL_Shared.Connection);
                DbCommand dbCommand = db.GetSqlStringCommand(SQL_INSERT_SHIFT);
                db.AddInParameter(dbCommand, PARAM_SHIFTID, DbType.Int32, oShift.iShiftID);
                db.AddInParameter(dbCommand, PARAM_SHIFTNAME, DbType.String, oShift.sShiftName);
                db.AddInParameter(dbCommand, PARAM_SHIFTDESC, DbType.String, oShift.sShiftDescription);
                db.AddInParameter(dbCommand, PARAM_STARTTIME, DbType.String, oShift.sStartTime);
                db.AddInParameter(dbCommand, PARAM_ENDTIME, DbType.String, oShift.sEndTime);
                db.AddInParameter(dbCommand, PARAM_DISABLED, DbType.Boolean, oShift.bDisabled);
                db.AddInParameter(dbCommand, PARAM_CREATEDBY, DbType.Int32, oShift.iCreatedBy);

                using (DbConnection conn = db.CreateConnection())
                {
                    conn.Open();// Connection Open

                    using (DbTransaction trans = conn.BeginTransaction()) //Start Transaction
                    {
                        try
                        {
                            Message = db.ExecuteScalar(dbCommand, trans).ToString();
                            trans.Commit(); //Commit Transaction                          
                        }
                        catch (Exception ex)
                        {
                            trans.Rollback();//Transaction RollBack
                            throw ex;
                        }
                    }
                    conn.Close();
                    return Message.ToString();
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
        public void UpdateData(BEShiftInfo oShift)
        {
            try
            {
                //*************************************

                Database db = DL_Shared.dbFactory(_oTenant); //DatabaseFactory.CreateDatabase(DL_Shared.Connection);
                DbCommand dbUpdateShiftCommand = db.GetSqlStringCommand(SQL_UPDATE_SHIFT);
                db.AddInParameter(dbUpdateShiftCommand, PARAM_SHIFTID, DbType.Int32, oShift.iShiftID);
                db.AddInParameter(dbUpdateShiftCommand, PARAM_SHIFTNAME, DbType.String, oShift.sShiftName);
                db.AddInParameter(dbUpdateShiftCommand, PARAM_SHIFTDESC, DbType.String, oShift.sShiftDescription);
                db.AddInParameter(dbUpdateShiftCommand, PARAM_STARTTIME, DbType.String, oShift.sStartTime);
                db.AddInParameter(dbUpdateShiftCommand, PARAM_ENDTIME, DbType.String, oShift.sEndTime);
                db.AddInParameter(dbUpdateShiftCommand, PARAM_DISABLED, DbType.Boolean, oShift.bDisabled);
                db.AddInParameter(dbUpdateShiftCommand, PARAM_MODIFIEDBY, DbType.Int32, oShift.iModifiedBy);


                using (DbConnection conn = db.CreateConnection())
                {
                    conn.Open();// Connection Open
                    using (DbTransaction trans = conn.BeginTransaction()) //Start Transaction
                    {
                        try
                        {
                            db.ExecuteNonQuery(dbUpdateShiftCommand, trans);
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
