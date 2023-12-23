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
    public class DLLocation : IDisposable
    {
        private BETenant _oTenant = null;
        public DLLocation(BETenant oTenant)
        { _oTenant = oTenant; }
        public void Dispose()
        { _oTenant = null; }

        private const string SQL_SELECT_LOCATIONALL = @"SELECT LocationID,LocationName,isnull(Description,'') as DESCRIPTION,Disabled FROM Config.tblLocationMaster (nolock) WHERE LocationName like @LocationName   ORDER BY LocationName";
        private const string SQL_SELECT_ACTIVELOCATIONS = @"SELECT LocationID,LocationName,isnull(Description,'') as DESCRIPTION,Disabled FROM Config.tblLocationMaster (nolock) WHERE LocationName like @LocationName AND Disabled=0 ORDER BY LocationName";

        private const string SQL_SELECT_ACTIVELOCATION = @"select LocationID,LocationName,Description,Disabled,CreatedOn,CreatedBy,ModifiedOn,'ModifiedBy'=isnull(ModifiedBy,0) from Config.tblLocationMaster (NOLOCK) where Disabled = 0 order by LocationName";

        private const string SQL_SELECT_LOCATIONID = @"SELECT LocationID,LocationName,isnull(Description,'') as DESCRIPTION,Disabled FROM Config.tblLocationMaster (nolock) WHERE LocationID = @LocationID";

        private const string SQL_MANAGE_LOCATION = @"Config.Usp_ManageLocation";

        // private const string PARAM_USERID = "@UserID";
        private const string PARAM_LOCATIONID = "@LocationID";
        private const string PARAM_LOCATIONNAME = "@LocationName";
        private const string PARAM_LOCATIONDESC = "@Description";
        private const string PARAM_DISABLED = "@Disabled";
        private const string PARAM_CREATEDBY = "@CreatedBy";
        private const string PARAM_ACTION = "@Action";

        public List<BELocation> GetLocationList(string LocationName, bool bActiveLocation)
        {
            List<BELocation> lLocation = new List<BELocation>();

            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand;
            if (bActiveLocation == true)
            {
                dbCommand = db.GetSqlStringCommand(SQL_SELECT_LOCATIONALL);
            }
            else
            {
                dbCommand = db.GetSqlStringCommand(SQL_SELECT_ACTIVELOCATIONS);
            }
            db.AddInParameter(dbCommand, PARAM_LOCATIONNAME, DbType.String, "%" + LocationName + "%");
            using (IDataReader rdr = db.ExecuteReader(dbCommand))
            {
                while (rdr.Read())
                {
                    BELocation objLocation = new BELocation(Convert.ToInt32(rdr["LocationID"]), rdr["LocationName"].ToString() + " " + (Convert.ToBoolean(rdr["disabled"]) ? "(Disabled)" : ""), rdr["Description"].ToString(), Convert.ToBoolean(rdr["Disabled"]), 0);
                    lLocation.Add(objLocation);
                    objLocation = null;
                }
            }
            return lLocation;
        }

        public List<BELocation> GetLocationList(int LocationID)
        {
            List<BELocation> lLocation = new List<BELocation>();

            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetSqlStringCommand(SQL_SELECT_LOCATIONID);
            db.AddInParameter(dbCommand, PARAM_LOCATIONID, DbType.Int32, LocationID);
            using (IDataReader rdr = db.ExecuteReader(dbCommand))
            {
                while (rdr.Read())
                {
                    BELocation objLocation = new BELocation(Convert.ToInt32(rdr["LocationID"]), rdr["LocationName"].ToString() + " " + (Convert.ToBoolean(rdr["Disabled"]) ? "(Disabled)" : ""), rdr["Description"].ToString(), Convert.ToBoolean(rdr["Disabled"]), 0);
                    lLocation.Add(objLocation);
                    objLocation = null;
                }
            }
            return lLocation;
        }

        public void ManageLocation(BELocation oLocation, string Action)
        {
            try
            {

                DbCommand dbCommand;
                Database db = DL_Shared.dbFactory(_oTenant);

                dbCommand = db.GetStoredProcCommand(SQL_MANAGE_LOCATION);
                if (Action == "Add" || Action == "Delete")
                {
                    db.AddInParameter(dbCommand, PARAM_CREATEDBY, DbType.Int32, oLocation.iCreatedBy);
                }
                else
                {
                    db.AddInParameter(dbCommand, PARAM_CREATEDBY, DbType.Int32, oLocation.iModifiedBy);
                }

                db.AddInParameter(dbCommand, PARAM_LOCATIONID, DbType.Int32, oLocation.iLocationID);
                db.AddInParameter(dbCommand, PARAM_LOCATIONNAME, DbType.String, oLocation.sLocationName);
                db.AddInParameter(dbCommand, PARAM_LOCATIONDESC, DbType.String, oLocation.sLocationDescription);
                db.AddInParameter(dbCommand, PARAM_DISABLED, DbType.Boolean, oLocation.bDisabled);
                db.AddInParameter(dbCommand, PARAM_ACTION, DbType.String, Action);
                db.ExecuteNonQuery(dbCommand);
            }
            /*
            catch (System.Data.SqlClient.SqlException ex)
            {
                if (ex.Number == 547)
                {
                    throw new ExceptionHandler.ExceptionType.DataAccessCustomException(BPA.GlobalResources.DataLayer.Resources.ReferenceConstraint);
                }
                if (ex.Number == 2627 || ex.Number == 2601)
                {
                    throw new ExceptionHandler.ExceptionType.DataAccessCustomException(BPA.GlobalResources.DataLayer.Resources.UniqueConstraints);
                }
                if (ex.Message.Contains(BPA.GlobalResources.DataLayer.Resources.msg_LocationAlready))
                {
                    throw new ExceptionHandler.ExceptionType.DataAccessCustomException(BPA.GlobalResources.DataLayer.Resources.msg_LocationAlready);
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
