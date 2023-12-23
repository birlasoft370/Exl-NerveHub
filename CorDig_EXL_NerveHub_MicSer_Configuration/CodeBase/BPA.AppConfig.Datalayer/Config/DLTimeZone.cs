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
    public class DLTimeZone : IDisposable
    {
        private BETenant _oTenant = null;

        public DLTimeZone(BETenant oTenant)
        { _oTenant = oTenant; }

        public void Dispose()
        { _oTenant = null; }

        private const string SQL_SELECT_TIMEZONEALL = @"select TimeZoneID,ID,TimeZoneName,isnull(Description,'') as DESCRIPTION,OffsetGMT,Disabled,CreatedBy from Config.tblTimeZoneMaster (NoLock) where TimeZoneName like @TimeZoneName ORDER BY TimeZoneName";
        private const string SQL_SELECT_TIMEZONE = @"select TimeZoneID,ID,TimeZoneName,isnull(Description,'') as DESCRIPTION,OffsetGMT,Disabled,CreatedBy from Config.tblTimeZoneMaster (NoLock) where TimeZoneName like @TimeZoneName ORDER BY TimeZoneName";
        private const string SQL_MANAGE_TIMEZONE = @"Config.Usp_ManageTimeZone";
        private const string SQL_SELECT_TIMEZONEID = @"select TimeZoneID,ID,TimeZoneName,isnull(Description,'') as DESCRIPTION,OffsetGMT,Disabled,CreatedBy from Config.tblTimeZoneMaster (NoLock) where TimeZoneID = @TimeZoneID";

        private const string PARAM_TIMEZONEID = "@TimeZoneID";
        private const string PARAM_TIMEZONENAME = "@TimeZoneName";
        private const string PARAM_TIMEZONEDESC = "@Description";
        private const string PARAM_OFFSETGMT = "@OffsetGMT";
        private const string PARAM_DISABLED = "@Disabled";
        private const string PARAM_CREATEDBY = "@CreatedBy";
        private const string PARAM_ACTION = "@Action";
        public List<BETimeZoneInfo> GetTimeZoneList(string sTimeZoneName, bool bGetAll)
        {
            List<BETimeZoneInfo> lTimeZone = new List<BETimeZoneInfo>();


            Database db = DL_Shared.dbFactory(_oTenant);
            // Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = null;

            if (bGetAll)
                dbCommand = db.GetSqlStringCommand(SQL_SELECT_TIMEZONEALL);
            else
                dbCommand = db.GetSqlStringCommand(SQL_SELECT_TIMEZONE);

            db.AddInParameter(dbCommand, PARAM_TIMEZONENAME, DbType.String, "%" + sTimeZoneName + "%");
            using (IDataReader rdr = db.ExecuteReader(dbCommand))
            {
                while (rdr.Read())
                {
                    BETimeZoneInfo objTimeZone = new BETimeZoneInfo()
                    {
                        iTimeZoneID = Convert.ToInt32(rdr["TimeZoneID"]),
                        sTimeZoneID = rdr["ID"].ToString() + "/" + rdr["OffsetGMT"].ToString(),
                        sTimeZoneName = rdr["TimeZoneName"].ToString() + " " + (Convert.ToBoolean(rdr["disabled"]) ? "(Disabled)" : ""),
                        sTimeZoneDescription = rdr["DESCRIPTION"].ToString(),
                        sOffsetGMT = rdr["OffsetGMT"].ToString() + "/" + rdr["TimeZoneID"].ToString(),
                        bDisabled = Convert.ToBoolean(rdr["Disabled"])
                    };
                    lTimeZone.Add(objTimeZone);
                    objTimeZone = null;
                }
            }
            return lTimeZone;
        }

        public List<BETimeZoneInfo> GetTimeZoneList(int iTimeZoneID)
        {
            List<BETimeZoneInfo> lTimeZone = new List<BETimeZoneInfo>();
            //Database db = DL_Shared.dbFactory(_oTenant);


            Database db = DL_Shared.dbFactory(_oTenant);

            DbCommand dbCommand = db.GetSqlStringCommand(SQL_SELECT_TIMEZONEID);
            db.AddInParameter(dbCommand, PARAM_TIMEZONEID, DbType.Int32, iTimeZoneID);
            using (IDataReader rdr = db.ExecuteReader(dbCommand))
            {
                while (rdr.Read())
                {
                    BETimeZoneInfo objTimeZone = new BETimeZoneInfo(Convert.ToInt32(rdr["TimeZoneID"]), rdr["ID"].ToString(), rdr["TimeZoneName"].ToString(), rdr["DESCRIPTION"].ToString(), (rdr["OffsetGMT"].ToString() + "/" + Convert.ToString(rdr["TimeZoneID"])), Convert.ToBoolean(rdr["Disabled"]), Convert.ToInt32(rdr["CreatedBy"].ToString() == "" ? "0" : rdr["CreatedBy"]));
                    lTimeZone.Add(objTimeZone);
                    objTimeZone = null;
                }
            }
            return lTimeZone;
        }

        public void ManageTimeZone(BETimeZoneInfo oTimeZone, string Action)
        {
            try
            {
                Database db = DL_Shared.dbFactory(_oTenant);
                DbCommand dbCommand = db.GetStoredProcCommand(SQL_MANAGE_TIMEZONE);
                db.AddInParameter(dbCommand, PARAM_TIMEZONEID, DbType.Int32, oTimeZone.iTimeZoneID);
                db.AddInParameter(dbCommand, PARAM_TIMEZONENAME, DbType.String, oTimeZone.sTimeZoneName);
                db.AddInParameter(dbCommand, PARAM_TIMEZONEDESC, DbType.String, oTimeZone.sTimeZoneDescription);
                db.AddInParameter(dbCommand, PARAM_OFFSETGMT, DbType.String, oTimeZone.sOffsetGMT);
                db.AddInParameter(dbCommand, PARAM_DISABLED, DbType.Boolean, oTimeZone.bDisabled);
                db.AddInParameter(dbCommand, PARAM_ACTION, DbType.String, Action);

                if (Action == "Add" || Action == "Delete")
                    db.AddInParameter(dbCommand, PARAM_CREATEDBY, DbType.Int32, oTimeZone.iCreatedBy);
                else
                    db.AddInParameter(dbCommand, PARAM_CREATEDBY, DbType.Int32, oTimeZone.iModifiedBy);

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
                if (ex.Message.Contains(BPA.GlobalResources.DataLayer.Resources.msg_TimeZone_Already_Exist.Trim()))
                {
                    throw new ExceptionHandler.ExceptionType.DataAccessCustomException(BPA.GlobalResources.DataLayer.Resources.msg_TimeZone_Already_Exist);
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
