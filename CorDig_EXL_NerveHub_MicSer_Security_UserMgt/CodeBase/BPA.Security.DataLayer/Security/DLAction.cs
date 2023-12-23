/* Copyright © 2007 company 
 * project Name                 :
 * Class Name                   :
 * Namespace                    :
 * Purpose                      :
 * Description                  :
 * Dependency                   :
 * Related Table                :
 * Related Class                :
 * Related StoreProcdure        :
 * Author                       :
 * Created on                   :
 * Reviewed on                  :          
 * Reviewed by                  :
 * Tested on                    :
 * Tested by                    :
 * Modification history         :
 * modify1 on                   :
 * modify1 By                   :
 * Overall effect               :
 */


using BPA.Security.BusinessEntity.ExtrernalRefre;
using BPA.Security.BusinessEntity.Security;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;


namespace BPA.Security.Datalayer.Security
{
    public class DLAction: IDisposable
    {
        #region Fields
        private BETenant _oTenant = null;
        private const string SQL_SELECT_ACTION = @"SELECT ActionID,ActionName,Description,Disabled FROM Config.tblActionMaster (NOLOCK) WHERE ACTIONNAME LIKE @ActionName";
        private const string SQL_INSERT_ACTION = @"INSERT INTO Config.tblActionMaster (ActionName,Description,CreatedBy) VALUES (@ActionName,@Description,@CreatedBy)";
        private const string SQL_UPDATE_ACTION = @"UPDATE Config.tblActionMaster SET ActionName=@ActionName,Description=@Description,ModifiedBy=@ModifiedBy,ModifiedON=GetDate()  WHERE ActionID=@ActionID)";
        
        
        private const string PARAM_ACTIONID = "@ActionID";
        private const string PARAM_ACTIONNAME = "@ActionName";
        private const string PARAM_ACTIONDESC = "@Description";
        private const string PARAM_DISABLED = "@Disabled";
        private const string PARAM_CREATEDBY = "@CreatedBy";
        private const string PARAM_MODIFIEDBY = "@ModifiedBy";

        #endregion

        #region Constructor and Dispose
        /// <summary>
        /// Initializes a new instance of the <see cref="DLAction"/> class.
        /// </summary>
        public DLAction(BETenant oTenant)
        { _oTenant = oTenant; }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        { _oTenant = null; }
        #endregion
              
        #region GetActionList
        public List<BEActionInfo> GetActionList()
        {
            return GetActionList("");
        }

        public List<BEActionInfo> GetActionList(string ActionName)
        {
            List<BEActionInfo> lAction = new List<BEActionInfo>();
            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetSqlStringCommand(SQL_SELECT_ACTION);
            db.AddInParameter(dbCommand, PARAM_ACTIONNAME, DbType.String, "%"+ActionName+"%");
            using (IDataReader rdr = db.ExecuteReader(dbCommand))
            {
                while (rdr.Read())
                {
                    BEActionInfo oAction = new BEActionInfo(Convert.ToInt32(rdr["ActionID"]), rdr["ActionName"].ToString(), rdr["Description"].ToString(), Convert.ToBoolean(rdr["Disabled"]), 0);
                    lAction.Add(oAction);
                    oAction = null;
                }
            }
            return lAction;

        }
          #endregion

        #region Insert, Update, Delete  
        /// <summary>
        /// Add this instance.
        /// </summary>
        public void InsertData(BEActionInfo objAction)
        {
            try
            {
                Database db = DL_Shared.dbFactory(_oTenant);
                DbCommand dbCommand = db.GetSqlStringCommand(SQL_INSERT_ACTION);
                db.AddInParameter(dbCommand, PARAM_ACTIONID, DbType.Int32, objAction.iActionID);
                db.AddInParameter(dbCommand, PARAM_ACTIONNAME, DbType.String, objAction.sActionName);
                db.AddInParameter(dbCommand, PARAM_ACTIONDESC, DbType.String, objAction.sActionDescription);
                db.AddInParameter(dbCommand, PARAM_DISABLED, DbType.Boolean, objAction.bDisabled);
                db.AddInParameter(dbCommand, PARAM_CREATEDBY, DbType.Int32, objAction.iCreatedBy);
                db.ExecuteNonQuery(dbCommand);
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                if (ex.Number == 547)
                {
                   // throw new ExceptionHandler.ExceptionType.DataAccessCustomException(BPA.GlobalResources.DataLayer.Resources.ReferenceConstraint);
                }
                if (ex.Number == 2627)
                {
                    //throw new ExceptionHandler.ExceptionType.DataAccessCustomException(BPA.GlobalResources.DataLayer.Resources.UniqueConstraints);
                }
                throw;// new ExceptionHandler.ExceptionType.DataAccessException(ex.Message);
            }
            catch (Exception ex)
            {
                throw;// new ExceptionHandler.ExceptionType.DataAccessException(ex.Message);
            }
        }

        /// <summary>
        /// Update this instance.
        /// </summary>
        public void UpdateData(BEActionInfo objAction)
        {
            try
            {
                Database db = DL_Shared.dbFactory(_oTenant);
                DbCommand dbCommand = db.GetSqlStringCommand(SQL_UPDATE_ACTION);
                db.AddInParameter(dbCommand, PARAM_ACTIONID, DbType.Int32, objAction.iActionID);
                db.AddInParameter(dbCommand, PARAM_ACTIONNAME, DbType.String, objAction.sActionName);
                db.AddInParameter(dbCommand, PARAM_ACTIONDESC, DbType.String, objAction.sActionDescription);
                db.AddInParameter(dbCommand, PARAM_DISABLED, DbType.Boolean, objAction.bDisabled);
                db.AddInParameter(dbCommand, PARAM_MODIFIEDBY, DbType.Int32, objAction.iCreatedBy);
                db.ExecuteNonQuery(dbCommand);
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                if (ex.Number == 547)
                {
                  //  throw new ExceptionHandler.ExceptionType.DataAccessCustomException(BPA.GlobalResources.DataLayer.Resources.ReferenceConstraint);
                }
                if (ex.Number == 2627)
                {
                   // throw new ExceptionHandler.ExceptionType.DataAccessCustomException(BPA.GlobalResources.DataLayer.Resources.UniqueConstraints);
                }
                throw;// new ExceptionHandler.ExceptionType.DataAccessException(ex.Message);
            }
            catch (Exception ex)
            {
                throw;// new ExceptionHandler.ExceptionType.DataAccessException(ex.Message);
            }
        }

        /// <summary>
        /// Delete this instance.
        /// </summary>
        public void DeleteData(BEActionInfo objAction)
        { }

        #endregion

    }
}
