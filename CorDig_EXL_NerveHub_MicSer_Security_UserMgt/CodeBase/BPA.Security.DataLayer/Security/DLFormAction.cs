/* Copyright © 2007 company 
 * project Name                 :Datalayer
 * Class Name                   :DLFormAction
 * Namespace                    :BPA.Security.Datalayer.Security
 * Purpose                      :This class will do all the database related operation for Form Action Mapping
 * Description                  :
 * Dependency                   :
 * Related Table                :Config.tblActionMaster,Config.tblFormActionMap,CDS_S_FORMS
 * Related Class                :
 * Related StoreProcdure        :
 * Author                       :Tarun Bounthiyal
 * Created on                   :26-mar-2007
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
    /// Form Action Class for Database Related Operation
    /// </summary>
    public class DLFormAction : IDisposable
    {
        private BETenant _oTenant = null;
        private const string SP_GETFORMACTION = @"Usp_CDS_GetFormsObject";

        private const string SQL_SELECT_FORM = @"SELECT FormID,FormName,Description,Disabled,isnull(CreatedBy,0) as CreatedBy FROM Config.tblFormsMaster (NOLOCK) WHERE FormName like @FormName";
        private const string SQL_SELECT_FORMID = @"SELECT FormID,FormName,Description,Disabled,isnull(CreatedBy,0) as CreatedBy FROM Config.tblFormsMaster (NOLOCK) WHERE FormID = @FormID";
        private const string SQL_SELECT_MAXFORMID = @"SELECT ISNULL(MAX(FORMID),1) FROM Config.tblFormsMaster (NOLOCK)";
        private const string SQL_SELECT_FORMACTIONBYID = @"SELECT SAction.ActionID,SAction.ActionName FROM Config.tblFormsMaster form (NOLOCK) " +
                                                    " LEFT JOIN Config.tblFormActionMap FAction (NOLOCK) " +
                                                    " ON form.FormID = FAction.FormID AND FAction.Disabled=0" +
                                                    " INNER JOIN Config.tblActionMaster SAction" +
                                                    " ON SAction.ActionID  = FAction.ActionID AND SAction.Disabled=0" +
                                                    " WHERE form.FormID = @FormID";
        private const string SQL_SELECT_CHECKACTION = @"SELECT * FROM Config.tblFormActionMap Where FormID=@FormID and ActionID =@ActionID";

        private const string SQL_UPDATE_INACTIVATEMEMBER = @"UPDATE Config.tblFormActionMap SET Disabled=1 WHERE FormID=@FormID";
        private const string SQL_UPDATE_ACTIVATEMEMBER = @"UPDATE Config.tblFormActionMap SET Disabled=0 WHERE FormID=@FormID AND ActionID=@ActionID";

        private const string SQL_INSERT_FORM = @"INSERT INTO Config.tblFormsMaster (FormName,Description,CreatedBy) VALUES (@FormName,@Description,@CreatedBy)";
        private const string SQL_INSERT_FORMACTION = @"INSERT INTO Config.tblFormActionMap(ActionID,FormID,Createdby) VALUES (@ActionID,@FormID,@CreatedBy)";
        
        private const string SQL_UPDATE_FORM= @"UPDATE Config.tblFormsMaster SET FormName=@FormName,Description=@Description,Disabled=@Disabled,ModifiedBy=@ModifiedBy WHERE FormID=@FormID";


        private const string SQL_DELETE_FORM = @"DELETE FROM Config.tblFormsMaster WHERE FormID=@FormID";
        private const string SQL_DELETE_FORMACTION = @"DELETE FROM Config.tblFormActionMap WHERE FormID=@FormID";

        private const string PARAM_ROLEID = "@RoleID";
        private const string PARAM_FORMID = "@FormID";
        private const string PARAM_ACTIONID = "@ActionID";
        private const string PARAM_FORMNAME = "@FormName";
        private const string PARAM_DESCRIPTION = "@Description";
        private const string PARAM_DISABLED = "@Disabled";
        private const string PARAM_CREATEDBY = "@CreatedBY";
        private const string PARAM_MODIFIEDBY = "@ModifiedBy";
        

        #region Constructor and Dispose
        public DLFormAction(BETenant oTenant)
        { _oTenant = oTenant; }


        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        { _oTenant = null; }
        #endregion

        #region GetFormAction
        /// <summary>
        /// Gets the form action.
        /// </summary>
        /// <param name="RoleID">The role ID.</param>
        /// <returns></returns>
        public DataSet GetFormAction(int RoleID)
        {
            DataSet dsFormAction = new DataSet();
            string[] dTables ={ "FormAction" };
            
           Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetStoredProcCommand(SP_GETFORMACTION);
            db.AddInParameter(dbCommand, PARAM_ROLEID, DbType.Int32, RoleID);
            db.LoadDataSet(dbCommand, dsFormAction, dTables);
            return dsFormAction;

        }
        #endregion
        
        #region form list
        /// <summary>
        /// Gets the form list.
        /// </summary>
        /// <param name="sFormName">Name of the form.</param>
        /// <returns></returns>
        public List<BEFormAction> GetFormList(string sFormName)
        {
            List<BEFormAction> lFormAction = new List<BEFormAction>();
            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetSqlStringCommand(SQL_SELECT_FORM);
            db.AddInParameter(dbCommand, PARAM_FORMNAME, DbType.String, "%" + sFormName + "%");
            using (IDataReader rdr = db.ExecuteReader(dbCommand))
            {
                while (rdr.Read())
                {
                    BEFormAction oFormAction = new BEFormAction(Convert.ToInt32(rdr["FormID"]), rdr["FormName"].ToString(), rdr["Description"].ToString(), Convert.ToBoolean(rdr["Disabled"]), Convert.ToInt32(rdr["CreatedBy"]));
                    lFormAction.Add(oFormAction);
                    oFormAction = null;
                }
            }
            return lFormAction;

        }

        /// <summary>
        /// Gets the form list.
        /// </summary>
        /// <param name="iFormID">The form ID.</param>
        /// <returns></returns>
        public List<BEFormAction> GetFormList(int iFormID)
        {
            List<BEFormAction> lFormAction = new List<BEFormAction>();
            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetSqlStringCommand(SQL_SELECT_FORMID);
            db.AddInParameter(dbCommand, PARAM_FORMID, DbType.Int32, iFormID);
            using (IDataReader rdr = db.ExecuteReader(dbCommand))
            {
                while (rdr.Read())
                {
                    BEFormAction oFormAction = new BEFormAction(Convert.ToInt32(rdr["FormID"]), rdr["FormName"].ToString(), rdr["Description"].ToString(), Convert.ToBoolean(rdr["Disabled"]), Convert.ToInt32(rdr["CreatedBy"]));

                    DbCommand dbCommandAction = db.GetSqlStringCommand(SQL_SELECT_FORMACTIONBYID);
                    db.AddInParameter(dbCommandAction, PARAM_FORMID, DbType.Int32, iFormID);
                    using (IDataReader dr = db.ExecuteReader(dbCommandAction))
                    {
                        List<BEActionInfo> IFAction = new List<BEActionInfo>(); 
                        while (dr.Read())
                        {
                            BEActionInfo oFAction = new BEActionInfo();
                            oFAction.iActionID = Convert.ToInt32(dr["ActionID"]);
                            oFAction.sActionName = dr["ActionName"].ToString();
                            IFAction.Add(oFAction);
                            oFAction = null;
                        }
                        oFormAction.oAction = IFAction;
                    }
                    
                    lFormAction.Add(oFormAction);
                    oFormAction = null;
                }
            }
            return lFormAction;

        }
        #endregion

        #region Insert Data
        /// <summary>
        /// Inserts the data.
        /// </summary>
        /// <param name="objFormAction">The obj form action.</param>
        public void InsertData(BEFormAction objFormAction)
        {

            try
            {
                //*************************************
                Database db = DL_Shared.dbFactory(_oTenant);
                DbCommand dbInsertFormCommand = db.GetSqlStringCommand(SQL_INSERT_FORM);
                db.AddInParameter(dbInsertFormCommand, PARAM_FORMNAME, DbType.String, objFormAction.sFormName);
                db.AddInParameter(dbInsertFormCommand, PARAM_DESCRIPTION, DbType.String, objFormAction.sDescription);
                db.AddInParameter(dbInsertFormCommand, PARAM_DISABLED, DbType.Boolean, objFormAction.bDisabled);
                db.AddInParameter(dbInsertFormCommand, PARAM_CREATEDBY, DbType.Int32, objFormAction.iCreatedBy);

                //*************************************
                DbCommand dbInsertFormActionCommand = db.GetSqlStringCommand(SQL_INSERT_FORMACTION);
                db.AddInParameter(dbInsertFormActionCommand, PARAM_FORMID, DbType.Int32);
                db.AddInParameter(dbInsertFormActionCommand, PARAM_ACTIONID, DbType.Int32);
                db.AddInParameter(dbInsertFormActionCommand, PARAM_CREATEDBY, DbType.Int32, objFormAction.iCreatedBy);

                //*************************************
                DbCommand dbSelectMaxCommand = db.GetSqlStringCommand(SQL_SELECT_MAXFORMID);


                using (DbConnection conn = db.CreateConnection())
                {
                    conn.Open();// Connection Open

                    using (DbTransaction trans = conn.BeginTransaction()) //Start Transaction
                    {
                        try
                        {
                            db.ExecuteNonQuery(dbInsertFormCommand, trans);
                            int MaxID = (int)db.ExecuteScalar(dbSelectMaxCommand, trans);
                            db.SetParameterValue(dbInsertFormActionCommand, PARAM_FORMID, MaxID);

                            int ActionCount = objFormAction.oAction.Count;
                            for (int i = 0; i < ActionCount; i++)
                            {
                                db.SetParameterValue(dbInsertFormActionCommand, PARAM_ACTIONID, objFormAction.oAction[i].iActionID);
                                db.ExecuteNonQuery(dbInsertFormActionCommand, trans);
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
            catch (System.Data.SqlClient.SqlException ex)
            {
                if (ex.Number == 547)
                {
                    throw;// new ExceptionHandler.ExceptionType.DataAccessCustomException(BPA.GlobalResources.DataLayer.Resources.ReferenceConstraint);
                }
                if (ex.Number == 2627)
                {
                    throw;// new ExceptionHandler.ExceptionType.DataAccessCustomException(BPA.GlobalResources.DataLayer.Resources.UniqueConstraints);
                }
                throw;// new ExceptionHandler.ExceptionType.DataAccessException(ex.Message);
            }
            catch (Exception ex)
            {
                throw;// new ExceptionHandler.ExceptionType.DataAccessException(ex.Message);
            }

        }
        #endregion

        #region Update Data
        /// <summary>
        /// Update this instance.
        /// </summary>
        /// <param name="objFormAction">The obj form action.</param>
        public void UpdateData(BEFormAction objFormAction)
        {
            try
            {
                //*************************************
                Database db = DL_Shared.dbFactory(_oTenant);
                DbCommand dbUpdateFormCommand = db.GetSqlStringCommand(SQL_UPDATE_FORM);
                db.AddInParameter(dbUpdateFormCommand, PARAM_FORMID, DbType.Int32, objFormAction.iFormID);
                db.AddInParameter(dbUpdateFormCommand, PARAM_FORMNAME, DbType.String, objFormAction.sFormName);
                db.AddInParameter(dbUpdateFormCommand, PARAM_DESCRIPTION, DbType.String, objFormAction.sDescription);
                db.AddInParameter(dbUpdateFormCommand, PARAM_DISABLED, DbType.Boolean, objFormAction.bDisabled);
                db.AddInParameter(dbUpdateFormCommand, PARAM_MODIFIEDBY, DbType.Int32, objFormAction.iCreatedBy);


                //*************************************
                DbCommand dbCheckActionCommand = db.GetSqlStringCommand(SQL_SELECT_CHECKACTION);
                db.AddInParameter(dbCheckActionCommand, PARAM_FORMID, DbType.Int32, objFormAction.iFormID);
                db.AddInParameter(dbCheckActionCommand, PARAM_ACTIONID, DbType.Int32);


                //*************************************
                DbCommand dbInActivateFormActionCommand = db.GetSqlStringCommand(SQL_UPDATE_INACTIVATEMEMBER);
                db.AddInParameter(dbInActivateFormActionCommand, PARAM_FORMID, DbType.Int32, objFormAction.iFormID);

                //*************************************
                DbCommand dbActivateFormActionCommand = db.GetSqlStringCommand(SQL_UPDATE_ACTIVATEMEMBER);
                db.AddInParameter(dbActivateFormActionCommand, PARAM_FORMID, DbType.Int32, objFormAction.iFormID);
                db.AddInParameter(dbActivateFormActionCommand, PARAM_ACTIONID, DbType.Int32);
                db.AddInParameter(dbActivateFormActionCommand, PARAM_MODIFIEDBY, DbType.Int32, objFormAction.iCreatedBy);

                //*************************************
                DbCommand dbInsertFormActionCommand = db.GetSqlStringCommand(SQL_INSERT_FORMACTION);
                db.AddInParameter(dbInsertFormActionCommand, PARAM_FORMID, DbType.Int32, objFormAction.iFormID);
                db.AddInParameter(dbInsertFormActionCommand, PARAM_ACTIONID, DbType.Int32);
                db.AddInParameter(dbInsertFormActionCommand, PARAM_CREATEDBY, DbType.Int32, objFormAction.iCreatedBy);

                //*************************************

                using (DbConnection conn = db.CreateConnection())
                {
                    conn.Open();// Connection Open
                    using (DbTransaction trans = conn.BeginTransaction()) //Start Transaction
                    {
                        try
                        {
                            db.ExecuteNonQuery(dbUpdateFormCommand, trans);
                            db.ExecuteNonQuery(dbInActivateFormActionCommand, trans);

                            int ActionCount = objFormAction.oAction.Count;
                            for (int i = 0; i < ActionCount; i++)
                            {
                                db.SetParameterValue(dbCheckActionCommand, PARAM_ACTIONID, objFormAction.oAction[i].iActionID);
                                object oRowCounter = db.ExecuteScalar(dbCheckActionCommand, trans);
                                if (oRowCounter == null)
                                {
                                    db.SetParameterValue(dbInsertFormActionCommand, PARAM_ACTIONID, objFormAction.oAction[i].iActionID);
                                    db.ExecuteNonQuery(dbInsertFormActionCommand, trans);
                                }
                                else
                                {
                                    db.SetParameterValue(dbActivateFormActionCommand, PARAM_ACTIONID, objFormAction.oAction[i].iActionID);
                                    db.ExecuteNonQuery(dbActivateFormActionCommand, trans);
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
        #endregion

        #region Delete Data
        /// <summary>
        /// Deletes the data.
        /// </summary>
        /// <param name="objFormAction">The obj form action.</param>
        public void DeleteData(BEFormAction objFormAction)
        {
            try
            {
                Database db = DL_Shared.dbFactory(_oTenant);
                DbCommand dbDeleteFormCommand = db.GetSqlStringCommand(SQL_DELETE_FORM);
                db.AddInParameter(dbDeleteFormCommand, PARAM_FORMID, DbType.Int32, objFormAction.iFormID);


                //*************************************
                DbCommand dbDeleteFormActionCommand = db.GetSqlStringCommand(SQL_DELETE_FORMACTION);
                db.AddInParameter(dbDeleteFormActionCommand, PARAM_FORMID, DbType.Int32, objFormAction.iFormID);
                using (DbConnection conn = db.CreateConnection())
                {
                    conn.Open();// Connection Open
                    using (DbTransaction trans = conn.BeginTransaction()) //Start Transaction
                    {
                        try
                        {
                            db.ExecuteNonQuery(dbDeleteFormActionCommand, trans);
                            db.ExecuteNonQuery(dbDeleteFormCommand, trans);
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
        #endregion
    }
}
