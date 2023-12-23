using BPA.Security.BusinessEntity.ExtrernalRefre;
using BPA.Security.Datalayer;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.EnterpriseLibrary.Data;
using BPA.Security.DataLayer.ExternalRef.Utitlity;

namespace BPA.Security.DataLayer.ExternalRef
{/// <summary>
 /// Client Clas for Database Related Operation 
 /// </summary>
    public sealed class DLClient : IDisposable
    {
        #region Declare & Initialize Constants
        private BETenant _oTenant = null;
        private const string SQL_SELECT_CLIENT_MONTH_YEAR = @"SELECT * FROM Config.tblClientMaster(nolock) WHERE ClientName like @ClientName and CLIENTID in (select ClientId from prompt.cds_P_casdata(nolock) where month=@month And Year=@year)";
        private const string SQL_SP_CLIENT = @"Usp_CDS_GetClientList";
        private const string SQL_SP_CLIENTACCESSLIST = @"[Config].[Usp_GetAccessList]";

        private const string SQL_SELECT_CLIENTID = @"Begin SELECT CLIENTID,[VERTICALID]=isnull(VERTICALID,0),CLIENTNAME,isnull(DESCRIPTION,'') as DESCRIPTION,isnull(EndDate,'')EndDate, isnull(EXLSpecificClient,0) EXLSpecificClient,DISABLED FROM Config.tblClientMaster WHERE CLIENTID = @ClientID                                                          
                                                     End
                                                    ";
        private const string SQL_SELECT_ERPCLIENT = @" SELECT EC.ERPCLIENTID,EC.ERPCLIENTNAME, CASE WHEN ERPCLIENTMAPPINGID IS NULL THEN 0 ELSE 1 END 'ISMAPPED'  FROM Config.tblERPClient(NOLOCK) EC
                                                                 LEFT OUTER JOIN Config.tblERPClientMAP(NOLOCK) ECM ON EC.ERPCLIENTID=ECM.ERPCLIENTID and ClientId=@ClientId and ECM.Disabled=0 and EC.Disabled=0 Order By EC.ERPCLIENTNAME";

        private const string SQL_SELECT_ERPCLIENT_BYID = @" SELECT EC.ERPCLIENTID,EC.ERPCLIENTNAME, CASE WHEN ERPCLIENTMAPPINGID IS NULL THEN 0 ELSE 1 END 'ISMAPPED'  FROM Config.tblERPClient(NOLOCK) EC
                                                                 INNER JOIN Config.tblERPClientMAP(NOLOCK) ECM ON EC.ERPCLIENTID=ECM.ERPCLIENTID and ClientId=@ClientId and ECM.Disabled=0 and EC.Disabled=0 Order By EC.ERPCLIENTNAME";

        private const string SQL_DELETE_ERPCLIENTMAP = @"update Config.tblERPClientMap set disabled=1,ModifiedBy=@CreatedBy,ModifiedOn=GetDate()  where ERPClientId=@ERPClientId and ClientId=@ClientId";
        private const string SQL_INSERT_ERPCLIENTMAP = @"if exists (select  ERPClientMappingId from Config.tblERPClientMap where ERPClientId=@ERPClientId and ClientId=@ClientId)
                                                              update Config.tblERPClientMap set disabled=0,ModifiedBy=@CreatedBy,ModifiedOn=GetDate()  where ERPClientId=@ERPClientId and ClientId=@ClientId
                                                            else
                                                              insert into Config.tblERPClientMap(ERPClientMappingId,ERPClientId,ClientId,disabled,CreatedBy) values ((Select max(ERPClientMappingId)+1 from Config.tblERPClientMap),@ERPClientId,@ClientId,0,@CreatedBy)
                                                            ";
        private const string SQL_MANAGE_CLIENT = @"Config.Usp_ManageClient";

        private const string PARAM_USERID = "@UserID";
        private const string PARAM_CLIENTID = "@ClientID";
        private const string PARAM_VERTICALID = "@VerticalID";
        private const string PARAM_CLIENTNAME = "@ClientName";
        private const string PARAM_CLIENTDESC = "@Description";
        private const string PARAM_ENDDATE = "@EndDate";
        private const string PARAM_DISABLED = "@Disabled";
        private const string PARAM_CREATEDBY = "@CreatedBy";

        private const string PARAM_ACTIVECLIENTLIST = "@ActiveClientList";
        private const string PARAM_MONTH = "@month";
        private const string PARAM_YEAR = "@year";
        private const string PARAM_EXLSPECIFICCLIENT = "@EXLSpecificClient";
        private const string PARAM_ERPCLIENTID = "@ERPClientID";
        private const string PARAM_AGENTID = "@AgentID";
        private const string PARAM_FLAG = "@Flag";
        private const string PARAM_ISACTIVE = "@IsActive";
        private const string PARAM_ACTION = "@Action";
        private const string SQL_SP_CLIENT_DATAUTILITY = @"[dbo].[Usp_DU_GetClientList]";
        #endregion

        #region Constructor and Dispose
        /// <summary>
        /// Initializes a new instance of the <see cref="DLClient"/> class.
        /// </summary>
        public DLClient(BETenant oTenant)
        { _oTenant = oTenant; }
        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        { _oTenant = null; }
        #endregion

        #region GetClientList
        /// <summary>
        /// Gets the all Client data as list.
        /// </summary>
        /// <param name="iLoggedinUserID">The loggedin user ID.</param>
        /// <param name="bActiveClient">if set to <c>true</c> [active client].</param>
        /// <returns></returns>
        public List<BEClientInfo> GetClientList(int iLoggedinUserID, bool bActiveClient)
        {
            return GetClientList(iLoggedinUserID, "", bActiveClient);
        }

        /// <summary>
        /// Gets the Matching list of Client
        /// </summary>
        /// <param name="iLoggedinUserID">The loggedin user ID.</param>
        /// <param name="ClientName">Name of the client.</param>
        /// <param name="bActiveClient">if set to <c>true</c> [active client].</param>
        /// <returns></returns>
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
        /// <summary>
        /// To Get Client Access List
        /// </summary>
        /// <param name="iLoggedinUserID"></param>
        /// <param name="iAgentID"></param>
        /// <param name="bActiveClient"></param>
        /// <returns></returns>
        public List<BEClientInfo> GetClientAccessList(int iLoggedinUserID, int iAgentID, bool bActiveClient)
        {
            List<BEClientInfo> lClient = new List<BEClientInfo>();
            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetStoredProcCommand(SQL_SP_CLIENTACCESSLIST);
            db.AddInParameter(dbCommand, PARAM_USERID, DbType.Int32, iLoggedinUserID);
            db.AddInParameter(dbCommand, PARAM_ISACTIVE, DbType.Boolean, bActiveClient);
            db.AddInParameter(dbCommand, PARAM_AGENTID, DbType.Int32, iAgentID);
            db.AddInParameter(dbCommand, PARAM_FLAG, DbType.Int32, 1);
            using (IDataReader rdr = db.ExecuteReader(dbCommand))
            {
                while (rdr.Read())
                {
                    BEClientInfo objClient = new BEClientInfo(Convert.ToInt32(rdr["ClientID"]), rdr["ClientName"].ToString() + " " + (Convert.ToBoolean(rdr["Disabled"]) ? "(Disabled)" : ""), rdr["Description"].ToString(), Convert.ToDateTime(rdr["EndDate"].ToString()), Convert.ToBoolean(rdr["Disabled"]), 0);
                    lClient.Add(objClient);
                    objClient = null;
                }
            }
            return lClient;

        }


        /// <summary>
        /// Gets the client list.
        /// </summary>
        /// <param name="iLoggedinUserID">The i loggedin user ID.</param>
        /// <param name="Month">The month.</param>
        /// <param name="Year">The year.</param>
        /// <param name="bActiveClient">if set to <c>true</c> [b active client].</param>
        /// <returns></returns>
        public List<BEClientInfo> GetClientList(int iLoggedinUserID, string Month, string Year, bool bActiveClient)
        {
            List<BEClientInfo> lClient = new List<BEClientInfo>();
            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetSqlStringCommand(SQL_SELECT_CLIENT_MONTH_YEAR);
            db.AddInParameter(dbCommand, PARAM_CLIENTNAME, DbType.String, "%");
            db.AddInParameter(dbCommand, PARAM_MONTH, DbType.String, "" + Month + "");
            db.AddInParameter(dbCommand, PARAM_YEAR, DbType.String, "" + Year + "");
            db.AddInParameter(dbCommand, PARAM_ACTIVECLIENTLIST, DbType.Boolean, bActiveClient);
            using (IDataReader rdr = db.ExecuteReader(dbCommand))
            {
                while (rdr.Read())
                {
                    BEClientInfo objClient = new BEClientInfo(Convert.ToInt32(rdr["ClientID"]), rdr["ClientName"].ToString() + " " + (Convert.ToBoolean(rdr["Disabled"]) ? "(Disabled)" : ""), rdr["Description"].ToString(), Convert.ToDateTime("2009-09-22 14:39:59.677"), Convert.ToBoolean(rdr["Disabled"]), 0);
                    lClient.Add(objClient);
                    objClient = null;
                }
            }
            return lClient;

        }

        /// <summary>
        /// Gets the client list.
        /// </summary>
        /// <param name="iLoggedinUserID">The i loggedin user ID.</param>
        /// <param name="ClientName">Name of the client.</param>
        /// <param name="Month">The month.</param>
        /// <param name="Year">The year.</param>
        /// <param name="bActiveClient">if set to <c>true</c> [b active client].</param>
        /// <returns></returns>
        public List<BEClientInfo> GetClientList(int iLoggedinUserID, string ClientName, string Month, string Year, bool bActiveClient)
        {
            List<BEClientInfo> lClient = new List<BEClientInfo>();
            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetSqlStringCommand(SQL_SELECT_CLIENT_MONTH_YEAR);
            db.AddInParameter(dbCommand, PARAM_CLIENTNAME, DbType.String, "" + ClientName + "%");
            db.AddInParameter(dbCommand, PARAM_MONTH, DbType.String, "" + Month + "");
            db.AddInParameter(dbCommand, PARAM_YEAR, DbType.String, "" + Year + "");
            db.AddInParameter(dbCommand, PARAM_ACTIVECLIENTLIST, DbType.Boolean, bActiveClient);
            using (IDataReader rdr = db.ExecuteReader(dbCommand))
            {
                while (rdr.Read())
                {
                    BEClientInfo objClient = new BEClientInfo(Convert.ToInt32(rdr["ClientID"]), rdr["ClientName"].ToString() + " " + (Convert.ToBoolean(rdr["Disabled"]) ? "(Disabled)" : ""), rdr["Description"].ToString(), Convert.ToDateTime("2009-09-22 14:39:59.677"), Convert.ToBoolean(rdr["Disabled"]), 0);
                    lClient.Add(objClient);
                    objClient = null;
                }
            }
            return lClient;

        }


        /// <summary>
        /// Gets the client list.
        /// </summary>
        /// <param name="Month">The month.</param>
        /// <param name="Year">The year.</param>
        /// <param name="bActiveClient">if set to <c>true</c> [b active client].</param>
        /// <returns></returns>
        public List<BEClientInfo> GetClientList(string Month, string Year, bool bActiveClient)
        {
            List<BEClientInfo> lClient = new List<BEClientInfo>();
            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetSqlStringCommand(SQL_SELECT_CLIENT_MONTH_YEAR);
            db.AddInParameter(dbCommand, PARAM_MONTH, DbType.String, "" + Month + "");
            db.AddInParameter(dbCommand, PARAM_YEAR, DbType.String, "" + Year + "");
            db.AddInParameter(dbCommand, PARAM_ACTIVECLIENTLIST, DbType.Boolean, bActiveClient);
            using (IDataReader rdr = db.ExecuteReader(dbCommand))
            {
                while (rdr.Read())
                {
                    BEClientInfo objClient = new BEClientInfo(Convert.ToInt32(rdr["ClientID"]), rdr["ClientName"].ToString() + " " + (Convert.ToBoolean(rdr["Disabled"]) ? "(Disabled)" : ""), rdr["Description"].ToString(), Convert.ToDateTime("2009-09-22 14:39:59.677"), Convert.ToBoolean(rdr["Disabled"]), 0);
                    lClient.Add(objClient);
                    objClient = null;
                }
            }
            return lClient;

        }
        public List<BEClientInfo> GetClientListDataUtility(int iLoggedinUserID, string ClientName, bool bActiveClient)
        {
            List<BEClientInfo> lClient = new List<BEClientInfo>();
            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetStoredProcCommand(SQL_SP_CLIENT_DATAUTILITY);
            db.AddInParameter(dbCommand, PARAM_CLIENTNAME, DbType.String, "" + ClientName + "%");
            db.AddInParameter(dbCommand, PARAM_USERID, DbType.Int32, iLoggedinUserID);
            db.AddInParameter(dbCommand, PARAM_ACTIVECLIENTLIST, DbType.Boolean, bActiveClient);
            using (IDataReader rdr = db.ExecuteReader(dbCommand))
            {
                while (rdr.Read())
                {
                    BEClientInfo objClient = new BEClientInfo(Convert.ToInt32(rdr["ClientID"]), rdr["ClientName"].ToString() + " " + (Convert.ToBoolean(rdr["Disabled"]) ? "(Disabled)" : ""), rdr["Description"].ToString(), Convert.ToDateTime(rdr["EndDate"].ToString()), Convert.ToBoolean(rdr["Disabled"]), 0);
                    lClient.Add(objClient);
                    objClient = null;
                }
            }
            return lClient;

        }

        /// <summary>
        /// Get The Client Data
        /// </summary>
        /// <param name="ClientID">The client ID.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Gets the ERP client.
        /// </summary>
        /// <param name="ClientID">The client ID.</param>
        /// <returns></returns>
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

        #endregion

        #region Insert Client
        /// <summary>
        /// Inserts Client data.
        /// </summary>
        /// <param name="oClient">client.</param>
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
            catch (System.Data.SqlClient.SqlException ex)
            {
                if (ex.Number == 547)
                {
                   // throw new ExceptionHandler.ExceptionType.DataAccessCustomException(BPA.GlobalResources.DataLayer.Resources.ReferenceConstraint);
                }
                if (ex.Number == 2627 || ex.Number == 2601)
                {
                   // throw new ExceptionHandler.ExceptionType.DataAccessCustomException(BPA.GlobalResources.DataLayer.Resources.UniqueConstraints);
                }
                //if (ex.Message.Contains(BPA.GlobalResources.DataLayer.Resources.msg_ClientAlready))
                //{
                //    throw new ExceptionHandler.ExceptionType.DataAccessCustomException(BPA.GlobalResources.DataLayer.Resources.msg_ClientAlready);
                //}
                throw;// new ExceptionHandler.ExceptionType.DataAccessException(ex.Message);
            }
            catch (Exception ex)
            {
                throw;// new ExceptionHandler.ExceptionType.DataAccessException(ex.Message);
            }

        }

        /// <summary>
        /// Manages the ERP client.
        /// </summary>
        /// <param name="dtERPClient">The dt ERP client.</param>
        /// <param name="ClientId">The client id.</param>
        /// <param name="CreatedBy">The created by.</param>
        /// <param name="db">The db.</param>
        /// <param name="trans">The trans.</param>
        //private void ManageERPClient(DataTable dtERPClient, int ClientId, int CreatedBy, Database db, DbTransaction trans)
        //{
        //    DbCommand dbCommand = null;
        //    foreach (DataRow drow in dtERPClient.Rows)
        //    {
        //        if (drow["RowState"].ToString() == "NEW")
        //        {
        //            dbCommand = db.GetSqlStringCommand(SQL_INSERT_ERPCLIENTMAP);
        //            db.AddInParameter(dbCommand, PARAM_ERPCLIENTID, DbType.Int32, Convert.ToInt32(drow["ERPClientID"]));
        //            db.AddInParameter(dbCommand, PARAM_CLIENTID, DbType.Int32, ClientId);
        //            db.AddInParameter(dbCommand, PARAM_CREATEDBY, DbType.Int32, CreatedBy);
        //            db.ExecuteNonQuery(dbCommand, trans);
        //        }

        //        else if (drow["RowState"].ToString() == "DELETED")
        //        {
        //            dbCommand = db.GetSqlStringCommand(SQL_DELETE_ERPCLIENTMAP);
        //            db.AddInParameter(dbCommand, PARAM_ERPCLIENTID, DbType.Int32, Convert.ToInt32(drow["ERPClientID"]));
        //            db.AddInParameter(dbCommand, PARAM_CLIENTID, DbType.Int32, ClientId);
        //            db.AddInParameter(dbCommand, PARAM_CREATEDBY, DbType.Int32, CreatedBy);
        //            db.ExecuteNonQuery(dbCommand, trans);
        //        }
        //    }
        //}
        #endregion


    }
}
