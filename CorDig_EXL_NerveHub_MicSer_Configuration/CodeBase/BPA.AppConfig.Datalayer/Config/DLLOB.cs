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
    public class DLLOB : IDisposable
    {
        private BETenant _oTenant = null;
        private const string SQL_SELECT_LOB = @"SELECT LOBID,isnull(ERPID,0) as ERPID,LOBNAME,isnull(Description,'') as Description, Disabled FROM Config.tblLOBMaster (nolock) WHERE Disabled=0 and LOBName Like @LOBName ORDER BY LOBName";
        private const string SQL_SELECT_LOBALL = @"SELECT LOBID,isnull(ERPID,0) as ERPID,LOBNAME,isnull(Description,'') as Description, Disabled FROM Config.tblLOBMaster (nolock) WHERE LOBName Like @LOBName ORDER BY LOBName";
        private const string SQL_SELECT_LOBID = @"SELECT LOBID,isnull(ERPID,0) as ERPID,LOBName,isnull(Description,'') as Description, Disabled FROM Config.tblLOBMaster (nolock) WHERE LOBID = @LOBID";


        private const string SQL_MANAGE_LOB = @"Config.Usp_ManageLOB";


        private const string PARAM_LOBID = "@LOBID";
        private const string PARAM_ERPID = "@ERPID";
        private const string PARAM_LOBNAME = "@LOBName";
        private const string PARAM_LOBDESC = "@Description";
        private const string PARAM_DISABLED = "@Disabled";
        private const string PARAM_CREATEDBY = "@CreatedBy";
        private const string PARAM_ACTION = "@Action";

        #region Constructor and Dispose
        /// <summary>
        /// Initializes a new instance of the <see cref="DLLOB"/> class.
        /// </summary>
        public DLLOB(BETenant oTenant)
        { _oTenant = oTenant; }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        { _oTenant = null; }
        #endregion

        #region GetLOBList
        /// <summary>
        /// Gets the all LOB data as list.
        /// </summary>
        /// <returns></returns>
        public List<BELOBInfo> GetLOBList(bool IsActiveLOB)
        {
            return GetLOBList("", IsActiveLOB);

        }

        /// <summary>
        /// Gets the Matching list of LOB
        /// </summary>
        /// <param name="LOBName">Name of the LOB.</param>
        /// <param name="IsActiveLOB">if set to <c>true</c> [is active LOB].</param>
        /// <returns></returns>
        public List<BELOBInfo> GetLOBList(string LOBName, bool IsActiveLOB)
        {
            List<BELOBInfo> lLOB = new List<BELOBInfo>();

            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand;
            if (IsActiveLOB)
            {
                dbCommand = db.GetSqlStringCommand(SQL_SELECT_LOB);
            }
            else
            {
                dbCommand = db.GetSqlStringCommand(SQL_SELECT_LOBALL);
            }
            db.AddInParameter(dbCommand, PARAM_LOBNAME, DbType.String, "%" + LOBName + "%");
            using (IDataReader rdr = db.ExecuteReader(dbCommand))
            {
                while (rdr.Read())
                {
                    BELOBInfo objLOB = new BELOBInfo(Convert.ToInt32(rdr["LOBID"]), Convert.ToInt32(rdr["ERPID"]), rdr["LOBName"].ToString(), rdr["Description"].ToString(), Convert.ToBoolean(rdr["Disabled"]), 0);
                    lLOB.Add(objLOB);
                    objLOB = null;
                }
            }
            return lLOB;

        }

        /// <summary>
        /// Get The LOB Data
        /// </summary>
        public List<BELOBInfo> GetLOBList(int LOBID)
        {
            List<BELOBInfo> lLOB = new List<BELOBInfo>();

            Database db = DL_Shared.dbFactory(_oTenant); //DatabaseF
            DbCommand dbCommand = db.GetSqlStringCommand(SQL_SELECT_LOBID);
            db.AddInParameter(dbCommand, PARAM_LOBID, DbType.Int32, LOBID);
            using (IDataReader rdr = db.ExecuteReader(dbCommand))
            {
                while (rdr.Read())
                {
                    BELOBInfo objLOB = new BELOBInfo(Convert.ToInt32(rdr["LOBID"]), Convert.ToInt32(rdr["ERPID"]), rdr["LOBName"].ToString(), rdr["Description"].ToString(), Convert.ToBoolean(rdr["Disabled"]), 0);
                    lLOB.Add(objLOB);
                    objLOB = null;
                }
            }
            return lLOB;

        }


        #endregion

        #region ManageData
        /// <summary>
        /// Inserts LOB data.
        /// </summary>
        /// <param name="oLOB">LOB.</param>
        public void ManageLOB(BELOBInfo oLOB, string Action)
        {
            try
            {

                Database db = DL_Shared.dbFactory(_oTenant); //DatabaseF
                DbCommand dbCommand = db.GetStoredProcCommand(SQL_MANAGE_LOB);
                db.AddInParameter(dbCommand, PARAM_LOBID, DbType.Int32, oLOB.iLOBID);
                db.AddInParameter(dbCommand, PARAM_ERPID, DbType.Int32, oLOB.iERPID);
                db.AddInParameter(dbCommand, PARAM_LOBNAME, DbType.String, oLOB.sLOBName);
                db.AddInParameter(dbCommand, PARAM_LOBDESC, DbType.String, oLOB.sDescription);
                db.AddInParameter(dbCommand, PARAM_DISABLED, DbType.Boolean, oLOB.bDisabled);
                if (Action == "Add")
                    db.AddInParameter(dbCommand, PARAM_CREATEDBY, DbType.Int32, oLOB.iCreatedBy);
                if (Action == "Update")
                    db.AddInParameter(dbCommand, PARAM_CREATEDBY, DbType.Int32, oLOB.iModifiedBy);
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
                if (ex.Number == 2627 || ex.Number == 2016)
                {
                    throw new ExceptionHandler.ExceptionType.DataAccessCustomException(BPA.GlobalResources.DataLayer.Resources.UniqueConstraints);
                }
                if (ex.Message.Contains(BPA.GlobalResources.DataLayer.Resources.msg_LOBAlready))
                {
                    throw new ExceptionHandler.ExceptionType.DataAccessCustomException(BPA.GlobalResources.DataLayer.Resources.msg_LOBAlready);
                }
                throw new ExceptionHandler.ExceptionType.DataAccessException(ex.Message);
            }*/
            catch (Exception ex)
            {
                throw;// new ExceptionHandler.ExceptionType.DataAccessException(ex.Message);
            }
        }
        #endregion



    }
}
