using BPA.AppConfig.BusinessEntity.Application;
using BPA.AppConfig.BusinessEntity.Config;
using System.Data.Common;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace BPA.AppConfig.Datalayer.Config
{
    public class DLVertical : IDisposable
    {
        private BETenant _oTenant = null;

        public DLVertical(BETenant oTenant)
        { _oTenant = oTenant; }
        public void Dispose()
        { _oTenant = null; }


        private const string SQL_SELECT_Vertical = @"SELECT VerticalID,ERPID,Name,isnull(Description,'') as Description, Disabled FROM Config.tblVerticalMaster (nolock) WHERE Disabled=0 and   Name Like @Name ORDER BY Name";
        private const string SQL_SELECT_VerticalALL = @"SELECT VerticalID,ERPID,Name,isnull(Description,'') as Description, Disabled FROM Config.tblVerticalMaster (nolock) WHERE   Name Like @Name ORDER BY Name";
        private const string SQL_SELECT_VerticalID = @"SELECT VerticalID,ERPID,Name,isnull(Description,'') as Description, Disabled FROM Config.tblVerticalMaster (nolock) WHERE VerticalID = @VerticalID";


        private const string SQL_MANAGE_VERTICAL = @"Config.Usp_ManageVertical";


        private const string PARAM_VERTICALID = "@VerticalID";
        private const string PARAM_ERPID = "@ERPID";
        private const string PARAM_NAME = "@Name";
        private const string PARAM_VerticalDESC = "@Description";
        private const string PARAM_DISABLED = "@Disabled";
        private const string PARAM_CREATEDBY = "@CreatedBy";
        private const string PARAM_ACTION = "@Action";

        public List<BEVerticalInfo> GetVerticalList(string Name, bool IsActiveVertical)
        {
            List<BEVerticalInfo> lVertical = new List<BEVerticalInfo>();


            Database db = DL_Shared.dbFactory(_oTenant);


            DbCommand dbCommand;
            if (IsActiveVertical)
            {
                dbCommand = db.GetSqlStringCommand(SQL_SELECT_Vertical);
            }
            else
            {
                dbCommand = db.GetSqlStringCommand(SQL_SELECT_VerticalALL);
            }
            db.AddInParameter(dbCommand, PARAM_NAME, DbType.String, "%" + Name + "%");
            using (IDataReader rdr = db.ExecuteReader(dbCommand))
            {
                while (rdr.Read())
                {
                    BEVerticalInfo objVertical = new BEVerticalInfo(Convert.ToInt32(rdr["VerticalID"]), Convert.ToInt32(rdr["ERPID"]), rdr["Name"].ToString(), rdr["Description"].ToString(), Convert.ToBoolean(rdr["Disabled"]), 0);
                    lVertical.Add(objVertical);
                    objVertical = null;
                }
            }
            return lVertical;
        }
        public List<BEVerticalInfo> GetVerticalList(int VerticalID)
        {
            List<BEVerticalInfo> lVertical = new List<BEVerticalInfo>();


            Database db = DL_Shared.dbFactory(_oTenant);


            DbCommand dbCommand = db.GetSqlStringCommand(SQL_SELECT_VerticalID);
            db.AddInParameter(dbCommand, PARAM_VERTICALID, DbType.Int32, VerticalID);
            using (IDataReader rdr = db.ExecuteReader(dbCommand))
            {
                while (rdr.Read())
                {
                    BEVerticalInfo objVertical = new BEVerticalInfo(Convert.ToInt32(rdr["VerticalID"]), Convert.ToInt32(rdr["ERPID"]), rdr["Name"].ToString(), rdr["Description"].ToString(), Convert.ToBoolean(rdr["Disabled"]), 0);
                    lVertical.Add(objVertical);
                    objVertical = null;
                }
            }
            return lVertical;

        }

        public void ManageVertical(BEVerticalInfo oVertical, string Action)
        {
            try
            {

                Database db = DL_Shared.dbFactory(_oTenant);

                DbCommand dbCommand = db.GetStoredProcCommand(SQL_MANAGE_VERTICAL);
                db.AddInParameter(dbCommand, PARAM_VERTICALID, DbType.Int32, oVertical.iVerticalID);
                db.AddInParameter(dbCommand, PARAM_ERPID, DbType.Int32, oVertical.iERPID);
                db.AddInParameter(dbCommand, PARAM_NAME, DbType.String, oVertical.sVerticalName);
                db.AddInParameter(dbCommand, PARAM_VerticalDESC, DbType.String, oVertical.sDescription);
                db.AddInParameter(dbCommand, PARAM_DISABLED, DbType.Boolean, oVertical.bDisabled);
                db.AddInParameter(dbCommand, PARAM_ACTION, DbType.String, Action);
                if (Action == "Add" || Action == "Delete")
                    db.AddInParameter(dbCommand, PARAM_CREATEDBY, DbType.Int32, oVertical.iCreatedBy);
                else
                    db.AddInParameter(dbCommand, PARAM_CREATEDBY, DbType.Int32, oVertical.iModifiedBy);


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
                if (ex.Message.Contains(BPA.GlobalResources.DataLayer.Resources.msg_VerticalAlready))
                {
                    throw new ExceptionHandler.ExceptionType.DataAccessCustomException(BPA.GlobalResources.DataLayer.Resources.msg_VerticalAlready);
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