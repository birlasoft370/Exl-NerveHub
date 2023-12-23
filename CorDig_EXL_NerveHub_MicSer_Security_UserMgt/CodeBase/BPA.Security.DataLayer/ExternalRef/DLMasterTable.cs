using BPA.Security.BusinessEntity.ExtrernalRefre;
using BPA.Security.BusinessEntity;
using BPA.Security.Datalayer;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace BPA.Security.DataLayer.ExternalRef
{/// <summary>
 /// Class used to handle database related opertions for master values management
 /// </summary>
    public class DLMasterTable : IDisposable
    {
        private BETenant _oTenant = null;
        private const string SQL_SELECT_MasterTable = @"SELECT MASTERID, VALUE FROM Config.tblMasterTable(NOLOCK) WHERE FIELDID=@FIELDID and Disabled=0";
        private const string SQL_SELECT_ROLELEVEL = @"SELECT UserLevelID, LevelName FROM [Config].[tblRoleLevel](NOLOCK) WHERE  Disabled=0";

        private const string SQL_SELECT_MasterTable_ValueField = @"SELECT cast(MASTERID as varchar(5))+'|'+case when FieldId=14 then 'T' when FieldId=50 then 'C' when FieldId=51 then 'Q' end as MasterId, VALUE FROM Config.tblMasterTable (NOLOCK) where fieldid in (select * from fn_split(@FieldId,',')) and disabled=0 order by fieldid,value";
        private const string SQL_SELECT_MASTERLIST = @"SELECT FIELDID,FIELDDESCRIPTION FROM Config.tblMasterType(NOLOCK) WHERE FIELDDESCRIPTION LIKE @FIELDDESCRIPTION ORDER BY FIELDDESCRIPTION";
        private const string SQL_SELECT_MASTERLIST1 = @"SELECT FIELDID,FIELDDESCRIPTION FROM Config.tblMasterType(NOLOCK) WHERE FIELDDESCRIPTION LIKE @FIELDDESCRIPTION AND DISABLED=0 ORDER BY FIELDDESCRIPTION";
        private const string SQL_SELECT_MASTERDETAILS = @"BEGIN SELECT FIELDID,FIELDDESCRIPTION,DISABLED FROM Config.tblMasterType(NOLOCK) WHERE FIELDID=@FIELDID SELECT MASTERID, VALUE, DISABLED FROM Config.tblMasterTable WHERE FIELDID=@FIELDID  END";
        private const string SQL_UPDATE_MASTERTYPE = @"BEGIN UPDATE Config.tblMasterType SET FIELDDESCRIPTION=@FIELDDESCRIPTION,DISABLED=@DISABLED,MODIFIEDBY=@MODIFIEDBY WHERE FIELDID=@FIELDID UPDATE Config.tblMasterTable SET DISABLED=@DISABLED,MODIFIEDBY=@MODIFIEDBY WHERE FIELDID=@FIELDID END";

        private const string SQL_INSERT_MASTERTYPE = @"if exists(select FIELDDESCRIPTION from Config.tblMasterType where FIELDDESCRIPTION=@FIELDDESCRIPTION)
                                                                 Begin
                                                                 RAISERROR ('Master Type Name Already Exist !',12,12)
                                                                 End
                                                                 else
                                                               BEGIN 
              INSERT INTO Config.tblMasterType (FIELDDESCRIPTION,DISABLED,CREATEDBY) VALUES(@FIELDDESCRIPTION,@DISABLED,@CREATEDBY) SELECT SCOPE_IDENTITY() AS FIELDID 
             END";
        private const string SQL_DELETE_MASTERTYPE = @"BEGIN DELETE Config.tblMasterType WHERE FIELDID=@FIELDID DELETE Config.tblMasterTable WHERE FIELDID=@FIELDID END";
        private const string SQL_UPDATE_MASTERVALUES = @"UPDATE Config.tblMasterTable SET VALUE=@VALUE,DISABLED=@DISABLED,MODIFIEDBY=@MODIFIEDBY WHERE MASTERID=@MASTERID";
        private const string SQL_INSERT_MASTERVALUES = @"INSERT INTO Config.tblMasterTable (FIELDID,VALUE,DISABLED,CREATEDBY) VALUES(@FIELDID,@VALUE,@DISABLED,@CREATEDBY)";
        private const string SQL_DELETE_MASTERVALUES = @"DELETE Config.tblMasterTable WHERE MASTERID=@MASTERID";

        private const string PARAM_FIELDID = "@FieldID";
        private const string PARAM_FIELDDESCRIPTION = "@FieldDescription";
        private const string PARAM_DISABLED = "@Disabled";
        private const string PARAM_MASTERID = "@MasterID";
        private const string PARAM_VALUE = "@Value";
        private const string PARAM_CREATEDBY = "@CreatedBy";
        private const string PARAM_MODIFIEDBY = "@ModifiedBy";

        #region Constructor and Dispose
        /// <summary>
        /// Initializes a new instance of the <see cref="DLMasterTable"/> class.
        /// </summary>
        public DLMasterTable(BETenant oTenant)
        { _oTenant = oTenant; }


        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        { }
        #endregion


        /// <summary>
        /// Gets the master list.
        /// </summary>
        /// <param name="iFieldID">The i field ID.</param>
        /// <returns></returns>
        public List<BEMasterTable> GetMasterList(int iFieldID)
        {
            List<BEMasterTable> lMasterList = new List<BEMasterTable>();
            // Database db = DL_Shared.dbFactory(_oTenant);

            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand;
            dbCommand = db.GetSqlStringCommand(SQL_SELECT_MasterTable);
            db.AddInParameter(dbCommand, PARAM_FIELDID, DbType.Int32, iFieldID);
            using (IDataReader rdr = db.ExecuteReader(dbCommand))
            {
                while (rdr.Read())
                {
                    BEMasterTable oMasterList = new BEMasterTable();
                    oMasterList.iMasterId = Convert.ToInt32(rdr["MasterId"]);
                    oMasterList.sValue = rdr["Value"].ToString(); ;
                    lMasterList.Add(oMasterList);
                    oMasterList = null;
                }
            }
            return lMasterList;

        }

        /// <summary>
        /// Gets the master list.
        /// </summary>
        /// <param name="sFieldID">The s field ID.</param>
        /// <returns></returns>
        public List<BEMasterTable> GetMasterList(string sFieldID)
        {
            List<BEMasterTable> lMasterList = new List<BEMasterTable>();
            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand;
            dbCommand = db.GetSqlStringCommand(SQL_SELECT_MasterTable_ValueField);
            db.AddInParameter(dbCommand, PARAM_FIELDID, DbType.String, sFieldID);
            using (IDataReader rdr = db.ExecuteReader(dbCommand))
            {
                while (rdr.Read())
                {
                    BEMasterTable oMasterList = new BEMasterTable();
                    oMasterList.sTempMasterId = rdr["MasterId"].ToString();
                    oMasterList.sValue = rdr["Value"].ToString(); ;
                    lMasterList.Add(oMasterList);
                    oMasterList = null;
                }
            }
            return lMasterList;

        }



        #region DeleteData

        /// <summary>
        /// Deletes the data.
        /// </summary>
        /// <param name="oMasterType">The oMasterType.</param>
        public void DeleteData(BEMasterType oMasterType)
        {
            try
            {

                Database db = DL_Shared.dbFactory(_oTenant);

                DbCommand dbCommand = db.GetSqlStringCommand(SQL_DELETE_MASTERTYPE);
                db.AddInParameter(dbCommand, PARAM_FIELDID, DbType.Int32, oMasterType.iFieldId);

                using (DbConnection conn = db.CreateConnection())
                {
                    conn.Open();// Connection Open

                    using (DbTransaction trans = conn.BeginTransaction()) //Start Transaction
                    {
                        try
                        {
                            db.ExecuteNonQuery(dbCommand, trans);
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

        #region InsertData

        /// <summary>
        /// Inserts the data.
        /// </summary>
        /// <param name="oMasterType">The o MasterType.</param>
        public void InsertData(BEMasterType oMasterType)
        {
            try
            {
                //Database db = DL_Shared.dbFactory(_oTenant);

                Database db = DL_Shared.dbFactory(_oTenant);
                DbCommand dbCommand = db.GetSqlStringCommand(SQL_INSERT_MASTERTYPE);
                db.AddInParameter(dbCommand, PARAM_FIELDDESCRIPTION, DbType.String, oMasterType.sFieldDescription);
                db.AddInParameter(dbCommand, PARAM_DISABLED, DbType.Boolean, oMasterType.bDisabled);
                db.AddInParameter(dbCommand, PARAM_CREATEDBY, DbType.Int32, oMasterType.iCreatedBy);

                using (DbConnection conn = db.CreateConnection())
                {
                    conn.Open();// Connection Open

                    using (DbTransaction trans = conn.BeginTransaction()) //Start Transaction
                    {
                        //try
                        //{
                        //db.ExecuteNonQuery(dbCommand, trans);
                        string iFieldID = Convert.ToString((db.ExecuteScalar(dbCommand, trans)));
                        if (oMasterType.lMasterTable.Count > 0 && Convert.ToInt32(iFieldID) > 0)
                        {
                            ManageMasterValues(db, trans, Convert.ToInt32(iFieldID), oMasterType.lMasterTable);
                        }
                        trans.Commit(); //Commit Transaction
                        //}

                        //catch (System.Data.SqlClient.SqlException ex)
                        //{
                        //    trans.Rollback();
                        //    if (ex.Number == 547)
                        //    {
                        //        throw new ExceptionHandler.ExceptionType.DataAccessCustomException(BPA.GlobalResources.DataLayer.Resources.ReferenceConstraint);
                        //    }
                        //    if (ex.Number == 2627)
                        //    {
                        //        throw new ExceptionHandler.ExceptionType.DataAccessCustomException(BPA.GlobalResources.DataLayer.Resources.UniqueConstraints);
                        //    }
                        //    if (ex.Message.Contains(BPA.GlobalResources.DataLayer.Resources.msg_MasterTypeAlreadyExist.Trim()))
                        //    {
                        //        throw new ExceptionHandler.ExceptionType.DataAccessCustomException(BPA.GlobalResources.DataLayer.Resources.msg_MasterTypeAlreadyExist);
                        //    }
                        //    throw new ExceptionHandler.ExceptionType.DataAccessException(ex.Message);
                        //}
                        //catch (Exception ex)
                        //{
                        //    trans.Rollback();
                        //    throw new ExceptionHandler.ExceptionType.DataAccessException(ex.Message);
                        //}
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
                   // throw new ExceptionHandler.ExceptionType.DataAccessCustomException(BPA.GlobalResources.DataLayer.Resources.UniqueConstraints);
                }
                //if (ex.Message.Contains(BPA.GlobalResources.DataLayer.Resources.msg_MasterTypeAlreadyExist.Trim()))
                //{
                //   // throw new ExceptionHandler.ExceptionType.DataAccessCustomException(BPA.GlobalResources.DataLayer.Resources.msg_MasterTypeAlreadyExist);
                //}
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
        /// Updates the data.
        /// </summary>
        /// <param name="oMasterType">The o MasterType.</param>
        public void UpdateData(BEMasterType oMasterType)
        {
            try
            {
                //*************************************

                Database db = DL_Shared.dbFactory(_oTenant);
                DbCommand dbCommand = db.GetSqlStringCommand(SQL_UPDATE_MASTERTYPE);
                db.AddInParameter(dbCommand, PARAM_FIELDID, DbType.Int32, oMasterType.iFieldId);
                db.AddInParameter(dbCommand, PARAM_FIELDDESCRIPTION, DbType.String, oMasterType.sFieldDescription);
                db.AddInParameter(dbCommand, PARAM_DISABLED, DbType.Boolean, oMasterType.bDisabled);
                db.AddInParameter(dbCommand, PARAM_MODIFIEDBY, DbType.Int32, oMasterType.iCreatedBy);


                using (DbConnection conn = db.CreateConnection())
                {
                    conn.Open();// Connection Open
                    using (DbTransaction trans = conn.BeginTransaction()) //Start Transaction
                    {
                        try
                        {
                            db.ExecuteNonQuery(dbCommand, trans);
                            if (oMasterType.lMasterTable.Count > 0)
                            {
                                ManageMasterValues(db, trans, oMasterType.iFieldId, oMasterType.lMasterTable);
                            }

                            trans.Commit(); //Commit Transaction
                        }
                        catch (System.Data.SqlClient.SqlException ex)
                        {
                            trans.Rollback();
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
                            trans.Rollback();
                            throw;// new ExceptionHandler.ExceptionType.DataAccessException(ex.Message);
                        }
                    }
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Manage Master Values Data
        /// <summary>
        /// Manages the Master Values data.
        /// </summary>
        /// <param name="db">The db.</param>
        /// <param name="trans">The trans.</param>
        /// <param name="iFieldID">The i Field id.</param>
        /// <param name="lMasterTable">The lMasterTable.</param>
        public void ManageMasterValues(Database db, DbTransaction trans, int iFieldID, IList<BEMasterTable> lMasterTable)
        {
            try
            {
                DbCommand dbCommand = null;
                foreach (BEMasterTable item in lMasterTable)
                {
                    if (item.oRowState == RowState.NEW)
                    {
                        dbCommand = db.GetSqlStringCommand(SQL_INSERT_MASTERVALUES);
                        db.AddInParameter(dbCommand, PARAM_FIELDID, DbType.Int32, iFieldID);
                        db.AddInParameter(dbCommand, PARAM_VALUE, DbType.String, item.sValue);
                        db.AddInParameter(dbCommand, PARAM_DISABLED, DbType.Boolean, item.bDisabled);
                        db.AddInParameter(dbCommand, PARAM_CREATEDBY, DbType.Int32, item.iCreatedBy);
                        db.ExecuteNonQuery(dbCommand, trans);
                    }
                    else if (item.oRowState == RowState.UPDATED)
                    {
                        dbCommand = db.GetSqlStringCommand(SQL_UPDATE_MASTERVALUES);
                        db.AddInParameter(dbCommand, PARAM_MASTERID, DbType.Int32, item.iMasterId);
                        db.AddInParameter(dbCommand, PARAM_VALUE, DbType.String, item.sValue);
                        db.AddInParameter(dbCommand, PARAM_DISABLED, DbType.Boolean, item.bDisabled);
                        db.AddInParameter(dbCommand, PARAM_MODIFIEDBY, DbType.Int32, item.iCreatedBy);

                        db.ExecuteNonQuery(dbCommand, trans);
                    }
                    else if (item.oRowState == RowState.DELETED)
                    {
                        dbCommand = db.GetSqlStringCommand(SQL_DELETE_MASTERVALUES);
                        if (item.oRowState == RowState.DELETED)
                        {
                            db.AddInParameter(dbCommand, PARAM_MASTERID, DbType.Int32, item.iMasterId);
                            db.ExecuteNonQuery(dbCommand, trans);
                        }
                    }
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


        #region Get Master List

        /// <summary>
        /// Gets the MasterType list.
        /// </summary>
        /// <param name="bGetAll">if set to <c>true</c> [b get all].</param>
        /// <returns></returns>
        public List<BEMasterType> GetMasterList(bool bGetAll)
        {
            return GetMasterList("", bGetAll);
        }

        /// <summary>
        /// Gets the Master list.
        /// </summary>
        /// <param name="sName">Field dec.</param>
        /// <param name="bGetAll">if set to <c>true</c> [b get all].</param>
        /// <returns></returns>
        public List<BEMasterType> GetMasterList(string sFieldDesc, bool bGetAll)
        {
            List<BEMasterType> lMasterType = new List<BEMasterType>();

            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand;
            if (bGetAll)
            {
                dbCommand = db.GetSqlStringCommand(SQL_SELECT_MASTERLIST);
            }
            else
                dbCommand = db.GetSqlStringCommand(SQL_SELECT_MASTERLIST1);
            db.AddInParameter(dbCommand, PARAM_FIELDDESCRIPTION, DbType.String, "" + sFieldDesc + "%");
            using (IDataReader rdr = db.ExecuteReader(dbCommand))
            {
                while (rdr.Read())
                {
                    BEMasterType oMasterType = new BEMasterType();
                    oMasterType.iFieldId = Convert.ToInt32(rdr["FieldID"]);
                    oMasterType.sFieldDescription = rdr["FieldDescription"].ToString();
                    //oMasterType.bDisabled = Convert.ToBoolean(rdr["Disabled"]);
                    lMasterType.Add(oMasterType);
                    oMasterType = null;
                }
            }
            return lMasterType;

        }


        /// <summary>
        /// Gets the Master Details.
        /// </summary>
        /// <param name="iFieldID">iFieldID.</param>
        /// <returns></returns>
        public BEMasterType GetMasterDetails(int iFieldID)
        {
            BEMasterType oMasterType = new BEMasterType();

            //Database db = DL_Shared.dbFactory(_oTenant);

            Database db = DL_Shared.dbFactory(_oTenant);

            DbCommand dbCommand = db.GetSqlStringCommand(SQL_SELECT_MASTERDETAILS);
            db.AddInParameter(dbCommand, PARAM_FIELDID, DbType.Int32, iFieldID);

            using (IDataReader rdr = db.ExecuteReader(dbCommand))
            {
                // Scroll through the results
                while (rdr.Read())
                {
                    oMasterType.iFieldId = Convert.ToInt32(rdr["FieldID"]);
                    oMasterType.sFieldDescription = rdr["FieldDescription"].ToString();
                    oMasterType.bDisabled = Convert.ToBoolean(rdr["Disabled"]);
                }
                //Process group data
                rdr.NextResult();
                List<BEMasterTable> lMasterTable = new List<BEMasterTable>();
                while (rdr.Read())
                {
                    BEMasterTable oMasterTable = new BEMasterTable();
                    oMasterTable.iMasterId = int.Parse(rdr["MasterID"].ToString());
                    oMasterTable.sValue = rdr["Value"].ToString();
                    oMasterTable.bDisabled = Convert.ToBoolean(rdr["Disabled"]);
                    oMasterTable.oRowState = RowState.NONE;
                    lMasterTable.Add(oMasterTable);
                    oMasterTable = null;
                }
                oMasterType.lMasterTable = lMasterTable;
            }
            return oMasterType;
        }


        public IList<BEMasterTable> FillRoleLevel()
        {
            IList<BEMasterTable> lMasterList = new List<BEMasterTable>();
            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand;
            dbCommand = db.GetSqlStringCommand(SQL_SELECT_ROLELEVEL);
            using (IDataReader rdr = db.ExecuteReader(dbCommand))
            {
                while (rdr.Read())
                {
                    BEMasterTable oMasterList = new BEMasterTable();
                    oMasterList.iMasterId = Convert.ToInt32(rdr["UserLevelID"]);
                    oMasterList.sValue = rdr["LevelName"].ToString(); ;
                    lMasterList.Add(oMasterList);
                    oMasterList = null;
                }
            }
            return lMasterList;
        }

        #endregion
    }
}
