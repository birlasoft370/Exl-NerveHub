using BPA.AppConfig.BusinessEntity.Application;
using BPA.AppConfig.BusinessEntity.Config;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data;
using System.Data.Common;
using BPA.Utility;

namespace BPA.AppConfig.Datalayer.Config
{
    public sealed class DLClient : IDisposable
    {
        private BETenant _oTenant = null;
        public DLClient(BETenant oTenant)
        { _oTenant = oTenant; }
        public void Dispose()
        { _oTenant = null; }

        private const string SQL_SP_CLIENT = @"Usp_CDS_GetClientList";
        private const string SQL_MANAGE_CLIENT = @"Config.Usp_ManageClient";
        private const string SQL_SELECT_CLIENTID = @"Begin SELECT CLIENTID,[VERTICALID]=isnull(VERTICALID,0),CLIENTNAME,isnull(DESCRIPTION,'') as DESCRIPTION,isnull(EndDate,'')EndDate, isnull(EXLSpecificClient,0) EXLSpecificClient,DISABLED FROM Config.tblClientMaster WHERE CLIENTID = @ClientID                                                          
                                                     End
                                                    ";
        private const string SQL_SELECT_ERPCLIENT = @" SELECT EC.ERPCLIENTID,EC.ERPCLIENTNAME, CASE WHEN ERPCLIENTMAPPINGID IS NULL THEN 0 ELSE 1 END 'ISMAPPED'  FROM Config.tblERPClient(NOLOCK) EC
                                                                 LEFT OUTER JOIN Config.tblERPClientMAP(NOLOCK) ECM ON EC.ERPCLIENTID=ECM.ERPCLIENTID and ClientId=@ClientId and ECM.Disabled=0 and EC.Disabled=0 Order By EC.ERPCLIENTNAME";
        private const string SQL_SELECT_ERPCLIENT_BYID = @" SELECT EC.ERPCLIENTID,EC.ERPCLIENTNAME, CASE WHEN ERPCLIENTMAPPINGID IS NULL THEN 0 ELSE 1 END 'ISMAPPED'  FROM Config.tblERPClient(NOLOCK) EC
                                                                 INNER JOIN Config.tblERPClientMAP(NOLOCK) ECM ON EC.ERPCLIENTID=ECM.ERPCLIENTID and ClientId=@ClientId and ECM.Disabled=0 and EC.Disabled=0 Order By EC.ERPCLIENTNAME";


        private const string PARAM_USERID = "@UserID";
        private const string PARAM_CLIENTID = "@ClientID";
        private const string PARAM_VERTICALID = "@VerticalID";
        private const string PARAM_CLIENTNAME = "@ClientName";
        private const string PARAM_CLIENTDESC = "@Description";
        private const string PARAM_ENDDATE = "@EndDate";
        private const string PARAM_DISABLED = "@Disabled";
        private const string PARAM_CREATEDBY = "@CreatedBy";
        private const string PARAM_EXLSPECIFICCLIENT = "@EXLSpecificClient";
        private const string PARAM_ACTION = "@Action";
        private const string PARAM_ACTIVECLIENTLIST = "@ActiveClientList";

        public List<BEClientInfo> GetClientList(int iLoggedinUserID, string ClientName, bool bActiveClient)
        {
            List<BEClientInfo> lClient = new List<BEClientInfo>();
            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetStoredProcCommand(SQL_SP_CLIENT);
            db.AddInParameter(dbCommand, PARAM_CLIENTNAME, DbType.String, "" + ClientName + "%");
            db.AddInParameter(dbCommand, PARAM_USERID, DbType.Int32, iLoggedinUserID);
            db.AddInParameter(dbCommand, PARAM_ACTIVECLIENTLIST, DbType.Boolean, bActiveClient);
            using (IDataReader rdr = db.ExecuteReader(dbCommand))
            {
                while (rdr.Read())
                {
                    BEClientInfo objClient = new BEClientInfo(Convert.ToInt32(rdr["ClientID"]), rdr["ClientName"].ToString().DecodeHtmlString() + " " + (Convert.ToBoolean(rdr["Disabled"]) ? "(Disabled)" : ""), rdr["Description"].ToString().DecodeHtmlString(), Convert.ToDateTime(rdr["EndDate"].ToString()), Convert.ToBoolean(rdr["Disabled"]), 0);
                    lClient.Add(objClient);
                    objClient = null;
                }
            }
            return lClient;

        }

        public List<BEClientInfo> GetClientList(int ClientID)
        {
            List<BEClientInfo> lClient = new List<BEClientInfo>();

            try
            {
                Database db = DL_Shared.dbFactory(_oTenant);
                DbCommand dbCommand = db.GetSqlStringCommand(SQL_SELECT_CLIENTID);
                db.AddInParameter(dbCommand, PARAM_CLIENTID, DbType.Int32, ClientID);
                using (IDataReader rdr = db.ExecuteReader(dbCommand))
                {
                    while (rdr.Read())
                    {
                        BEClientInfo objClient = new BEClientInfo
                        {
                            iClientID = int.Parse(rdr["ClientID"].ToString()),
                            iVerticalID = int.Parse(rdr["VerticalID"].ToString()),
                            sClientName = rdr["ClientName"].ToString().DecodeHtmlString(),
                            sClientDescription = rdr["Description"].ToString().DecodeHtmlString(),
                            dtEndDate = DateTime.Parse(rdr["EndDate"].ToString()),
                            bDisabled = bool.Parse(rdr["Disabled"].ToString())
                        };
                        objClient.bEXLSpecific = bool.Parse(rdr["EXLSpecificClient"].ToString());
                        objClient.dtERPClient = GetERPClient(ClientID);
                        lClient.Add(objClient);
                        objClient = null;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lClient;

        }

        public DataTable GetERPClient(int ClientID)
        {
            DataTable dtERSClient = new DataTable();
            try
            {
                Database db = DL_Shared.dbFactory(_oTenant);
                DbCommand dbCommand = null;
                if (ClientID == 0)
                {
                    dbCommand = db.GetSqlStringCommand(SQL_SELECT_ERPCLIENT);
                }
                else
                {
                    dbCommand = db.GetSqlStringCommand(SQL_SELECT_ERPCLIENT_BYID);
                }
                db.AddInParameter(dbCommand, PARAM_CLIENTID, DbType.Int32, ClientID);
                dtERSClient = db.ExecuteDataSet(dbCommand).Tables[0];
            }
            catch (Exception ex)
            {
                throw;
            }
            return dtERSClient;
        }

        public void ManageClientData(BEClientInfo oClient, string Action)
        {
            try
            {
                Database db = DL_Shared.dbFactory(_oTenant);
                DbCommand dbCommand = db.GetStoredProcCommand(SQL_MANAGE_CLIENT);
                db.AddInParameter(dbCommand, PARAM_CLIENTID, DbType.Int32, oClient.iClientID);
                db.AddInParameter(dbCommand, PARAM_VERTICALID, DbType.Int32, oClient.iVerticalID);
                db.AddInParameter(dbCommand, PARAM_CLIENTNAME, DbType.String, oClient.sClientName);
                db.AddInParameter(dbCommand, PARAM_CLIENTDESC, DbType.String, oClient.sClientDescription);
                if (oClient.dtEndDate.Year != 0001)
                {
                    db.AddInParameter(dbCommand, PARAM_ENDDATE, DbType.DateTime, oClient.dtEndDate);
                }
                else
                { db.AddInParameter(dbCommand, PARAM_ENDDATE, DbType.DateTime, DateTime.Now); }

                db.AddInParameter(dbCommand, PARAM_EXLSPECIFICCLIENT, DbType.Boolean, oClient.bEXLSpecific);
                db.AddInParameter(dbCommand, PARAM_DISABLED, DbType.Boolean, oClient.bDisabled);
                if (Action == "Add" || Action == "Delete")
                    db.AddInParameter(dbCommand, PARAM_CREATEDBY, DbType.Int32, oClient.iCreatedBy);
                if (Action == "Update")
                    db.AddInParameter(dbCommand, PARAM_CREATEDBY, DbType.Int32, oClient.iModifiedBy);


                db.AddInParameter(dbCommand, PARAM_ACTION, DbType.String, Action);
                using (DbConnection conn = db.CreateConnection())
                {
                    conn.Open();// Connection Open

                    using (DbTransaction trans = conn.BeginTransaction()) //Start Transaction
                    {
                        try
                        {
                            if (Action == "Add")
                            {
                                int iClientID = Convert.ToInt32(db.ExecuteScalar(dbCommand, trans));
                                // ManageERPClient(oClient.dtERPClient,iClientID, oClient.iCreatedBy, db, trans);
                            }
                            if (Action == "Update")
                            {
                                db.ExecuteNonQuery(dbCommand, trans);
                                //ManageERPClient(oClient.dtERPClient, oClient.iClientID, oClient.iModifiedBy, db, trans);
                            }
                            if (Action == "Delete")
                            {
                                db.ExecuteNonQuery(dbCommand, trans);
                            }

                            trans.Commit(); //Commit Transaction
                        }
                        catch (System.Data.SqlClient.SqlException ex)
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
                if (ex.Number == 2627 || ex.Number == 2601)
                {
                    throw new ExceptionHandler.ExceptionType.DataAccessCustomException(BPA.GlobalResources.DataLayer.Resources.UniqueConstraints);
                }
                if (ex.Message.Contains(BPA.GlobalResources.DataLayer.Resources.msg_ClientAlready))
                {
                    throw new ExceptionHandler.ExceptionType.DataAccessCustomException(BPA.GlobalResources.DataLayer.Resources.msg_ClientAlready);
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
