using BPA.AppConfig.BusinessEntity.Application;
using BPA.AppConfig.BusinessEntity.ExternalRef.Configuration;
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
    public class DLSBUInfo : IDisposable
    {
        private BETenant _oTenant = null;

        public DLSBUInfo(BETenant oTenant)
        { _oTenant = oTenant; }
        public void Dispose()
        { _oTenant = null; }

        private const string SQL_SELECT_SBUALL = @"SELECT SBUId,isnull(ERPId,0) as ERPId,Name,Description,IsclientSBU,isnull(Createdby,0) as Createdby,DISABLED FROM Config.tblSBUMaster WHERE Disabled=0 AND Name Like '%' + @Name + '%' Order by Name ";
        private const string SQL_SELECT_SBU = @"SELECT sbuid,ISNULL(Erpid,0) ErpId,Name,Description,IsClientSBU,Disabled,Createdby FROM Config.tblSBUMaster(NOLOCK) SB where  Name like '%' + @Name + '%' order by Name ";
        private const string SP_GETMAX_CLIENTID = @"Usp_CDS_GetMaxClient_ID";
        private const string SELECT_SBU_DATAUPDATE = @"Usp_CDS_UpdateSBU";
        private const string SQL_DISABLE_CLIENTSBUMAP = @"UPDATE Config.tblClientSBUMap SET Disabled=1, ModifiedBy=@ModifiedBy, ModifiedOn=GetDate()  where ClientId=@clientId";
        private const string SQL_SELECT_CLIENTSBUCOUNT = @"SELECT COUNT(*) FROM Config.tblClientSBUMap (NOLOCK) WHERE ClientId=@clientId AND SBUId=@SBUId";
        private const string SQL_ENABLE_CLIENTSBUMAP = @"UPDATE Config.tblClientSBUMap SET Disabled=0, ModifiedBy=@ModifiedBy, ModifiedOn=GetDate()  where ClientId=@clientId AND SBUId=@SBUId";
        private const string SQL_INSERT_SBUDATA = @"INSERT INTO Config.tblClientSBUMap (clientId, sbuid,  Disabled,CreatedBy) 
                                                VALUES(@clientId,@SBUId,@Disabled,@CreatedBy)";
        private const string SQL_CHECK_PROCESSBREAKMAP = @"SELECT COUNT(*) FROM Config.tblClientSBUMap (NoLock) where clientId = @clientId AND sbuid=@sbuid";
        private const string SELECT_SBU_DATA = @"Select CL.ClientSBUId, sb.SBUID,ISNULL(CL.clientid,0) clientid,sb.Name,sb.description,ISNULL(sb.IsClientSBU,'0') IsClientSBU,ISNULL(CL.disabled,'0')  disabled,sb.Createdby 
                                                from Config.tblClientSBUMap(NOLOCK) CL
                                                inner JOIN  Config.tblSBUMaster(NOLOCK) sb
                                                ON CL.SBUId = sb.SBUId and CL.clientid =@ClientID and CL.Disabled=0";
        private const string SQL_SELECT_SBUID = @"SELECT SBUId,isnull(ERPId,0) as ERPId,Name,Description,IsClientSBU,isnull(Createdby,0) as Createdby,DISABLED FROM Config.tblSBUMaster WHERE SBUID = @SBUId";
        private const string SQL_INSERT_SBU = @"if exists(select Name from Config.tblSBUMaster where Name=@Name)
                                                                 Begin
                                                                 select 'SBU Name Already Exists!'
                                                                 End
                                                                 else
                                                                 Begin
                                                              INSERT INTO Config.tblSBUMaster (ERPId,Name,Description,IsClientSBU,DISABLED,CreatedBy) VALUES(@ERPId,@Name,@Description,@IsClientSBU,@Disabled,@CreatedBy)
                                                                  select ''  
                                                                 End";
        private const string SQL_UPDATE_SBU = @"UPDATE Config.tblSBUMaster SET ERPId=@ERPId,Name=@Name,Description=@Description,Disabled=@Disabled,IsClientSBU=@IsClientSBU,MODIFIEDBY=@ModifiedBy WHERE SBUId=@SBUId";


        private const string PARAM_NAME = "@Name";
        private const string PARAM_OUT = "@ReturnValue";
        private const string PARAM_CLIENTID = "@clientId";
        private const string PARAM_SBUID = "@SBUId";
        private const string PARAM_ERPID = "@ERPId";
        private const string PARAM_DESCRIPTION = "@Description";
        private const string PARAM_DISABLED = "@Disabled";
        private const string PARAM_CREATEDBY = "@CreatedBy";
        private const string PARAM_MODIFIEDBY = "@ModifiedBy";
        private const string PARAM_ISCLIENTSBU = "@IsClientSBU";

        public List<BESBUInfo> GetSBUList(string sName, bool bGetAll)
        {
            List<BESBUInfo> lSBU = new List<BESBUInfo>();

            Database db = DL_Shared.dbFactory(_oTenant);

            DbCommand dbCommand;
            if (bGetAll)
            {
                dbCommand = db.GetSqlStringCommand(SQL_SELECT_SBUALL);
            }
            else
                dbCommand = db.GetSqlStringCommand(SQL_SELECT_SBU);
            db.AddInParameter(dbCommand, PARAM_NAME, DbType.String, "" + sName + "%");
            using (IDataReader rdr = db.ExecuteReader(dbCommand))
            {
                while (rdr.Read())
                {
                    BESBUInfo objSBU = new BESBUInfo(Convert.ToInt32(rdr["SBUID"]), Convert.ToInt32(rdr["ErpId"]), rdr["Name"].ToString() + " " + (Convert.ToBoolean(rdr["Disabled"]) ? "(Disabled)" : ""), rdr["DESCRIPTION"].ToString(), Convert.ToBoolean(rdr["Disabled"]), Convert.ToBoolean(rdr["IsClientSBU"]), Convert.ToInt32(rdr["CreatedBy"]));
                    lSBU.Add(objSBU);
                    objSBU = null;
                }
            }
            return lSBU;

        }

        public List<BESBUInfo> GetSBUList(int iSBUID)
        {
            List<BESBUInfo> lSBUInfo = new List<BESBUInfo>();


            Database db = DL_Shared.dbFactory(_oTenant);


            DbCommand dbCommand = db.GetSqlStringCommand(SQL_SELECT_SBUID);
            db.AddInParameter(dbCommand, PARAM_SBUID, DbType.Int32, iSBUID);

            using (IDataReader rdr = db.ExecuteReader(dbCommand))
            {
                // Scroll through the results
                while (rdr.Read())
                {

                    BESBUInfo objSBU = new BESBUInfo(Convert.ToInt32(rdr["SBUID"]), Convert.ToInt32(rdr["ERPId"]), rdr["Name"].ToString(), rdr["DESCRIPTION"].ToString(), Convert.ToBoolean(rdr["Disabled"]), Convert.ToBoolean(rdr["IsClientSBU"]), Convert.ToInt32(rdr["CreatedBy"]));
                    lSBUInfo.Add(objSBU);
                    objSBU = null;
                }
            }
            return lSBUInfo;

        }

        public int GetMaxClientId()
        {
            try
            {

                int iFoundRecord;

                Database db = DL_Shared.dbFactory(_oTenant);

                DbCommand dbCommand = db.GetStoredProcCommand(SP_GETMAX_CLIENTID);
                db.AddOutParameter(dbCommand, PARAM_OUT, DbType.Int32, 20);

                using (DbConnection conn = db.CreateConnection())
                {
                    conn.Open();// Connection Open

                    using (DbTransaction trans = conn.BeginTransaction()) //Start Transaction
                    {
                        try
                        {
                            db.ExecuteNonQuery(dbCommand, trans);
                            iFoundRecord = Convert.ToInt32(db.GetParameterValue(dbCommand, PARAM_OUT));
                            trans.Commit(); //Commit Transaction

                        }
                        catch (Exception ex)
                        {
                            trans.Rollback();//Transaction RollBack
                            throw ex;
                        }
                    }
                    conn.Close();
                    return iFoundRecord;
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

        public List<BESBUInfo> GetSBUListbasedONClient(int iclientId)
        {
            List<BESBUInfo> lSBUInfo = new List<BESBUInfo>();


            Database db = DL_Shared.dbFactory(_oTenant);


            DbCommand dbCommand = db.GetSqlStringCommand(SELECT_SBU_DATA);
            db.AddInParameter(dbCommand, PARAM_CLIENTID, DbType.Int32, iclientId);

            using (IDataReader rdr = db.ExecuteReader(dbCommand))
            {
                // Scroll through the results
                while (rdr.Read())
                {

                    BESBUInfo objSBU = new BESBUInfo(Convert.ToInt32(rdr["ClientSBUId"]), Convert.ToInt32(rdr["SBUID"]), Convert.ToInt32(rdr["ClientId"]), rdr["Name"].ToString(), rdr["DESCRIPTION"].ToString(), Convert.ToBoolean(rdr["Disabled"]), Convert.ToBoolean(rdr["IsClientSBU"]), Convert.ToInt32(rdr["CreatedBy"]));
                    lSBUInfo.Add(objSBU);
                    objSBU = null;
                }
            }
            return lSBUInfo;

        }

        public void InsertDataSBU(BESBUInfo oSBU)
        {
            try
            {

                Database db = DL_Shared.dbFactory(_oTenant);
                DbCommand dbCommand = db.GetSqlStringCommand(SQL_INSERT_SBUDATA);
                db.AddInParameter(dbCommand, PARAM_CLIENTID, DbType.Int32, oSBU.iCLIENTID);
                db.AddInParameter(dbCommand, PARAM_SBUID, DbType.String, oSBU.iSBUID);
                db.AddInParameter(dbCommand, PARAM_DISABLED, DbType.Boolean, oSBU.bDisabled);
                db.AddInParameter(dbCommand, PARAM_CREATEDBY, DbType.Int32, oSBU.iCreatedBy);
                //Query to check the Deuplicacy in DB
                DbCommand dbCheckDuplicacy = db.GetSqlStringCommand(SQL_CHECK_PROCESSBREAKMAP);
                db.AddInParameter(dbCheckDuplicacy, PARAM_CLIENTID, DbType.Int32);
                db.AddInParameter(dbCheckDuplicacy, PARAM_SBUID, DbType.Int32, oSBU.iSBUID);


                using (DbConnection conn = db.CreateConnection())
                {
                    conn.Open();// Connection Open

                    using (DbTransaction trans = conn.BeginTransaction()) //Start Transaction
                    {

                        try
                        {
                            foreach (DataRow dr in oSBU.dtClientSBUMap.Rows)
                            {
                                db.SetParameterValue(dbCommand, PARAM_CLIENTID, dr[0]);
                                db.SetParameterValue(dbCommand, PARAM_SBUID, dr[1]);
                                db.SetParameterValue(dbCommand, PARAM_DISABLED, dr[2]);
                                db.ExecuteNonQuery(dbCommand, trans);

                            }

                            trans.Commit();
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
        public void UpdateDataSBU(BESBUInfo oSBU)
        {
            try
            {

                Database db = DL_Shared.dbFactory(_oTenant);

                DbCommand dbCommand = db.GetStoredProcCommand(SELECT_SBU_DATAUPDATE);
                db.AddInParameter(dbCommand, PARAM_CLIENTID, DbType.Int32, oSBU.iERPID);
                db.AddInParameter(dbCommand, PARAM_SBUID, DbType.String, oSBU.iERPID);
                db.AddInParameter(dbCommand, PARAM_DISABLED, DbType.Boolean, oSBU.bDisabled);
                db.AddInParameter(dbCommand, PARAM_CREATEDBY, DbType.Int32, oSBU.iCreatedBy);

                DbCommand dbDisableCommand = db.GetSqlStringCommand(SQL_DISABLE_CLIENTSBUMAP);
                db.AddInParameter(dbDisableCommand, PARAM_CLIENTID, DbType.Int32);
                db.AddInParameter(dbDisableCommand, PARAM_MODIFIEDBY, DbType.Int32, oSBU.iCreatedBy);

                DbCommand dbCOUNTCommand = db.GetSqlStringCommand(SQL_SELECT_CLIENTSBUCOUNT);
                db.AddInParameter(dbCOUNTCommand, PARAM_CLIENTID, DbType.Int32);
                db.AddInParameter(dbCOUNTCommand, PARAM_SBUID, DbType.Int32);

                DbCommand dbEnableCommand = db.GetSqlStringCommand(SQL_ENABLE_CLIENTSBUMAP);
                db.AddInParameter(dbEnableCommand, PARAM_CLIENTID, DbType.Int32);
                db.AddInParameter(dbEnableCommand, PARAM_SBUID, DbType.Int32);
                db.AddInParameter(dbEnableCommand, PARAM_MODIFIEDBY, DbType.Int32, oSBU.iCreatedBy);

                DbCommand dbInsertCommand = db.GetSqlStringCommand(SQL_INSERT_SBUDATA);
                db.AddInParameter(dbInsertCommand, PARAM_CLIENTID, DbType.Int32);
                db.AddInParameter(dbInsertCommand, PARAM_SBUID, DbType.Int32);

                db.AddInParameter(dbInsertCommand, PARAM_DISABLED, DbType.Boolean, 0);
                db.AddInParameter(dbInsertCommand, PARAM_CREATEDBY, DbType.Int32, oSBU.iCreatedBy);


                using (DbConnection conn = db.CreateConnection())
                {
                    conn.Open();// Connection Open

                    using (DbTransaction trans = conn.BeginTransaction()) //Start Transaction
                    {

                        try
                        {
                            if (oSBU.dtClientSBUMap.Rows.Count > 0)
                            {
                                db.SetParameterValue(dbDisableCommand, PARAM_CLIENTID, oSBU.dtClientSBUMap.Rows[0][0]);
                                db.ExecuteNonQuery(dbDisableCommand, trans);
                            }
                            foreach (DataRow dr in oSBU.dtClientSBUMap.Rows)
                            {
                                db.SetParameterValue(dbCOUNTCommand, PARAM_CLIENTID, dr[0]);
                                db.SetParameterValue(dbCOUNTCommand, PARAM_SBUID, dr[1]);
                                int cnt = int.Parse(db.ExecuteScalar(dbCOUNTCommand, trans).ToString());
                                if (cnt > 0)
                                {
                                    db.SetParameterValue(dbEnableCommand, PARAM_CLIENTID, dr[0]);
                                    db.SetParameterValue(dbEnableCommand, PARAM_SBUID, dr[1]);
                                    db.ExecuteNonQuery(dbEnableCommand, trans);
                                }
                                if (cnt == 0)
                                {
                                    db.SetParameterValue(dbInsertCommand, PARAM_CLIENTID, dr[0]);
                                    db.SetParameterValue(dbInsertCommand, PARAM_SBUID, dr[1]);
                                    db.ExecuteNonQuery(dbInsertCommand, trans);
                                }

                            }

                            trans.Commit();
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

        public string InsertData(BESBUInfo oSBU)
        {
            try
            {

                Database db = DL_Shared.dbFactory(_oTenant);

                DbCommand dbCommand = db.GetSqlStringCommand(SQL_INSERT_SBU);
                db.AddInParameter(dbCommand, PARAM_ERPID, DbType.Int32, oSBU.iERPID);
                db.AddInParameter(dbCommand, PARAM_NAME, DbType.String, oSBU.sName);
                db.AddInParameter(dbCommand, PARAM_DESCRIPTION, DbType.String, oSBU.sDescription);
                db.AddInParameter(dbCommand, PARAM_ISCLIENTSBU, DbType.Boolean, oSBU.bIsClientSBU);
                db.AddInParameter(dbCommand, PARAM_DISABLED, DbType.Boolean, oSBU.bDisabled);
                db.AddInParameter(dbCommand, PARAM_CREATEDBY, DbType.Int32, oSBU.iCreatedBy);
                return db.ExecuteScalar(dbCommand).ToString();
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

        public void UpdateData(BESBUInfo oSBU)
        {
            try
            {
                //*************************************

                Database db = DL_Shared.dbFactory(_oTenant);

                DbCommand dbUpdateSBUCommand = db.GetSqlStringCommand(SQL_UPDATE_SBU);
                db.AddInParameter(dbUpdateSBUCommand, PARAM_SBUID, DbType.Int32, oSBU.iSBUID);
                db.AddInParameter(dbUpdateSBUCommand, PARAM_ERPID, DbType.String, oSBU.iERPID);
                db.AddInParameter(dbUpdateSBUCommand, PARAM_NAME, DbType.String, oSBU.sName);
                db.AddInParameter(dbUpdateSBUCommand, PARAM_DESCRIPTION, DbType.String, oSBU.sDescription);
                db.AddInParameter(dbUpdateSBUCommand, PARAM_ISCLIENTSBU, DbType.Boolean, oSBU.bIsClientSBU);
                db.AddInParameter(dbUpdateSBUCommand, PARAM_DISABLED, DbType.Boolean, oSBU.bDisabled);
                db.AddInParameter(dbUpdateSBUCommand, PARAM_MODIFIEDBY, DbType.Int32, oSBU.iModifiedBy);


                using (DbConnection conn = db.CreateConnection())
                {
                    conn.Open();// Connection Open
                    using (DbTransaction trans = conn.BeginTransaction()) //Start Transaction
                    {
                        try
                        {
                            db.ExecuteNonQuery(dbUpdateSBUCommand, trans);
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
