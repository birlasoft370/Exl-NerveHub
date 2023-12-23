/* Copyright © 2007 company 
 * project Name                 :Datalayer
 * Class Name                   :DLUserLevel
 * Namespace                    :BPA.Security.Datalayer.Security
 * Purpose                      :Do all the Data related operation for User Levels
 * Description                  :
 * Dependency                   :
 * Related Table                :
 * Related Class                :
 * Related StoreProcdure        :
 * Author                       :Tarun Bounthiyal
 * Created on                   :12-Apr-2007
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
    /// <summary>
    /// User Level 
    /// </summary>
    public class DLUserLevel: IDisposable
    {
        #region Fields
        private BETenant _oTenant = null;
        private const string SQL_SELECT_USERLEVEL = @"SELECT UserLevelID,LevelName,Description,ParentID,Disabled,createdBy FROM Config.tblUserLevel (NOLOCK) WHERE Disabled=0 AND LevelName LIKE @LevelName ORDER By LevelName ";
        private const string SQL_SELECT_USERLEVELALL = @"SELECT UserLevelID,LevelName,Description,ParentID,Disabled,CreatedBy FROM Config.tblUserLevel (NOLOCK) WHERE LevelName LIKE @LevelName ORDER By LevelName ";
        private const string SQL_SELECT_USERLEVELID = @"SELECT UserLevelID,LevelName,Description,ParentID,Disabled,CreatedBy FROM Config.tblUserLevel (NOLOCK) WHERE UserLevelID=@UserLevelID";

        private const string SQL_INSERT_USERLEVEL = @"INSERT INTO Config.tblUserLevel (LevelName,Description,ParentID,CreatedBy) VALUES (@LevelName,@Description,@ParentID,@CreatedBy)";
        private const string SQL_UPDATE_USERLEVEL = @"UPDATE Config.tblUserLevel SET LevelName=@LevelName,Description=@Description,ParentID=@ParentID,Disabled=@Disabled,ModifiedBy=@ModifiedBy,ModifiedON=GetDate()  WHERE UserLevelID=@UserLevelID";
        private const string SQL_DELETE_USERLEVEL = @"DELETE FROM Config.tblUserLevel WHERE UserLevelID=@UserLevelID";

        private const string PARAM_LEVELID = "@UserLevelID";
        private const string PARAM_LEVELNAME = "@LevelName";
        private const string PARAM_DESCRIPTION = "@Description";
        private const string PARAM_PARENTID = "@ParentID";
        private const string PARAM_DISABLED = "@Disabled";
        private const string PARAM_CREATEDBY = "@CreatedBy";
        private const string PARAM_MODIFIEDBY = "@ModifiedBy";

        #endregion

        #region Constructor and Dispose
        /// <summary>
        /// Initializes a new instance of the <see cref="DLAction"/> class.
        /// </summary>
        public DLUserLevel(BETenant oTenant)
        { _oTenant = oTenant; }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        { _oTenant = null; }
        #endregion
              
        #region GetUserLevelList
        /// <summary>
        /// Gets the action list.
        /// </summary>
        /// <param name="bActiveLevel">if set to <c>true</c> [active level].</param>
        /// <returns></returns>
        public List<BEUserLevelInfo> GetUserLevelList(bool bActiveLevel)
        {
            return GetUserLevelList("", bActiveLevel);
        }

        /// <summary>
        /// Gets the action list.
        /// </summary>
        /// <param name="sUserLevelName">Name of the s user level.</param>
        /// <param name="bActiveLevel">if set to <c>true</c> [active level].</param>
        /// <returns></returns>
        public List<BEUserLevelInfo> GetUserLevelList(string sUserLevelName, bool bActiveLevel)
        {
            List<BEUserLevelInfo> lUserLevel = new List<BEUserLevelInfo>();
            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand ;

            if(bActiveLevel)
                dbCommand = db.GetSqlStringCommand(SQL_SELECT_USERLEVEL);
            else
                dbCommand = db.GetSqlStringCommand(SQL_SELECT_USERLEVELALL);

            db.AddInParameter(dbCommand, PARAM_LEVELNAME, DbType.String, sUserLevelName + "%");
            using (IDataReader rdr = db.ExecuteReader(dbCommand))
            {
                while (rdr.Read())
                {
                    BEUserLevelInfo oUserLevelInfo = new BEUserLevelInfo(Convert.ToInt32(rdr["UserLevelID"]), rdr["LevelName"].ToString() + " " + (Convert.ToBoolean(rdr["Disabled"]) ? "(Disabled)" : ""), rdr["Description"].ToString(), Convert.ToInt32(rdr["ParentID"]), Convert.ToBoolean(rdr["Disabled"]), Convert.ToInt32(rdr["createdBy"]));
                    lUserLevel.Add(oUserLevelInfo);
                    oUserLevelInfo = null;
                }
            }
            return lUserLevel;

        }
        /// <summary>
        /// Gets the action list.
        /// </summary>
        /// <param name="iUserLevelID">The user level ID.</param>
        /// <returns></returns>
        public List<BEUserLevelInfo> GetUserLevelList(int iUserLevelID)
        {
            List<BEUserLevelInfo> lUserLevel = new List<BEUserLevelInfo>();
            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetSqlStringCommand(SQL_SELECT_USERLEVELID);
            db.AddInParameter(dbCommand, PARAM_LEVELID, DbType.Int32, iUserLevelID);
            using (IDataReader rdr = db.ExecuteReader(dbCommand))
            {
                while (rdr.Read())
                {
                    BEUserLevelInfo oUserLevelInfo = new BEUserLevelInfo(Convert.ToInt32(rdr["UserLevelID"]), rdr["LevelName"].ToString(), rdr["Description"].ToString(), Convert.ToInt32(rdr["ParentID"]), Convert.ToBoolean(rdr["Disabled"]), Convert.ToInt32(rdr["createdBy"]));
                    lUserLevel.Add(oUserLevelInfo);
                    oUserLevelInfo = null;
                }
            }
            return lUserLevel;
        }
          #endregion

        #region Insert, Update, Delete  
        /// <summary>
        /// Add this instance.
        /// </summary>
        public void InsertData(BEUserLevelInfo oUserLevel)
        {
            try
            {
                Database db = DL_Shared.dbFactory(_oTenant);
                DbCommand dbCommand = db.GetSqlStringCommand(SQL_INSERT_USERLEVEL);
                db.AddInParameter(dbCommand, PARAM_LEVELID, DbType.Int32, oUserLevel.iUserLevelID);
                db.AddInParameter(dbCommand, PARAM_LEVELNAME, DbType.String, oUserLevel.sLevelName);
                db.AddInParameter(dbCommand, PARAM_DESCRIPTION, DbType.String, oUserLevel.sDescription);
                db.AddInParameter(dbCommand, PARAM_PARENTID, DbType.Int32, oUserLevel.iParentID);
                db.AddInParameter(dbCommand, PARAM_DISABLED, DbType.Boolean, oUserLevel.bDisabled);
                db.AddInParameter(dbCommand, PARAM_CREATEDBY, DbType.Int32, oUserLevel.iCreatedBy);
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
        public void UpdateData(BEUserLevelInfo oUserLevel)
        {
            try
            {
                Database db = DL_Shared.dbFactory(_oTenant);
                DbCommand dbCommand = db.GetSqlStringCommand(SQL_UPDATE_USERLEVEL);
                db.AddInParameter(dbCommand, PARAM_LEVELID, DbType.Int32, oUserLevel.iUserLevelID);
                db.AddInParameter(dbCommand, PARAM_LEVELNAME, DbType.String, oUserLevel.sLevelName);
                db.AddInParameter(dbCommand, PARAM_DESCRIPTION, DbType.String, oUserLevel.sDescription);
                db.AddInParameter(dbCommand, PARAM_PARENTID, DbType.Int32, oUserLevel.iParentID);
                db.AddInParameter(dbCommand, PARAM_DISABLED, DbType.Boolean, oUserLevel.bDisabled);
                db.AddInParameter(dbCommand, PARAM_MODIFIEDBY, DbType.Int32, oUserLevel.iCreatedBy);
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
        public void DeleteData(BEUserLevelInfo oUserLevel)
        {
            try
            {
                Database db = DL_Shared.dbFactory(_oTenant);
                DbCommand dbCommand = db.GetSqlStringCommand(SQL_DELETE_USERLEVEL);
                db.AddInParameter(dbCommand, PARAM_LEVELID, DbType.Int32, oUserLevel.iUserLevelID);
                db.ExecuteNonQuery(dbCommand);
            }
              catch (System.Data.SqlClient.SqlException ex)
            {
                if (ex.Number == 547)
                {
                    //throw new ExceptionHandler.ExceptionType.DataAccessCustomException(BPA.GlobalResources.DataLayer.Resources.ReferenceConstraint);
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

        #endregion

    }
}
