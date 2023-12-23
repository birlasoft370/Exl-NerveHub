
using BPA.Security.BusinessEntity.ExtrernalRefre;
using BPA.Security.BusinessEntity.Security;
using Microsoft.Practices.EnterpriseLibrary.Data;
/* Copyright © 2007 company 
 * project Name                 :Datalayer
 * Class Name                   :DLSession
 * Namespace                    :BPA.Security.Datalayer.Security
 * Purpose                      :Manage Sessions of user
 * Description                  :
 * Dependency                   :
 * Related Table                :Config.tblSessionLog,Config.tblSessionEndLog
 * Related Class                :
 * Related StoreProcdure        :
 * Author                       :Ravi Kumar Singh
 * Created on                   :25-APR-2007
 * Reviewed on                  :          
 * Reviewed by                  :
 * Tested on                    :
 * Tested by                    :
 * Modification history         :
 * modify1 on                   :
 * modify1 By                   :
 * Overall effect               :
 */
using System;
using System.Data;
using System.Data.Common;
namespace BPA.Security.Datalayer.Security
{
   public  class DLSession:IDisposable
   {
       private BETenant _oTenant = null;
         #region Constructor and Dispose
        /// <summary>
        /// Initializes a new instance of the <see cref="DLPermission"/> class.
        /// </summary>
        public DLSession(BETenant oTenant)
       { _oTenant = oTenant; }
        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        { _oTenant = null; }
        #endregion

       private const string SQL_SELECT_IDENTITY = @"SELECT isnull(MAX(SessionID),0) FROM Config.tblSessionLog (NOLOCK) WHERE UserId=@UserID";
       private const string SQL_SELECT_USERID = @"SELECT UserId FROM Config.tblSessionLog (NOLOCK) WHERE SessionID=@SessionID";
       

        private const string SQL_INSERT_SESSIONSTART = @"INSERT INTO Config.tblSessionLog(SystemSessionID,UserId,IPAddress,HostName) VALUES(@SystemSessionID,@UserID,@IPAddress,@HostName)";
        private const string SQL_INSERT_SESSIONEND = @"INSERT INTO Config.tblSessionEndLog(SessionID) VALUES(@SessionID)";
       private const string SP_FINAL_LOGOUT = @"Usp_CDS_AgentLogout";

       private const string SQL_INSERT_ERRORLOG = @"insert into History.AppErrorLog(PostTime,ErrorObject,ErrorMessage) values(@PostTime,@ErrorObject,@ErrorMessage)";

        private const string PARAM_USERID = "@UserID";
        private const string PARAM_SESSIONID = "@SessionID";
        private const string PARAM_SYSTEMSESSIONID = "@SystemSessionID";
        private const string PARAM_IPADDRESS = "@IPAddress";
        private const string PARAM_HOSTNAME = "@HostName";
        private const string PARAM_POSTTIME = "@PostTime";
        private const string PARAM_ERROROBJECT = "@ErrorObject";
        private const string PARAM_ERRORMESSAGE = "@ErrorMessage";


        /// <summary>
        /// Inserts the Error Status.
        /// </summary>
        /// <param name="oBESession">The o BE session.</param>
       public string InsertErorLog(string ErrorMessage)
        {
           try
           {
               Database db = DL_Shared.dbFactory(_oTenant);
               DbCommand dbInsertError = db.GetSqlStringCommand(SQL_INSERT_ERRORLOG);
               db.AddInParameter(dbInsertError, PARAM_POSTTIME, DbType.DateTime, DateTime.Now);
               db.AddInParameter(dbInsertError, PARAM_ERROROBJECT, DbType.String, "OK");
               db.AddInParameter(dbInsertError, PARAM_ERRORMESSAGE, DbType.String, ErrorMessage);
               db.ExecuteNonQuery(dbInsertError);
               return "0";
           }
           catch(Exception er)
           {
           return er.Message;
           }
             
        }
        /// <summary>
        /// Inserts the session start.
        /// </summary>
        /// <param name="oBESession">The o BE session.</param>
        public int  InsertSessionStart(BESession oBESession)
        {
            try
            {
                Database db = DL_Shared.dbFactory(_oTenant);
                DbCommand dbInsertUserCommand = db.GetSqlStringCommand(SQL_INSERT_SESSIONSTART);
                db.AddInParameter(dbInsertUserCommand, PARAM_USERID, DbType.Int32, oBESession.iUserID);
                db.AddInParameter(dbInsertUserCommand, PARAM_SYSTEMSESSIONID, DbType.String, oBESession.sSystemSessionID);
                db.AddInParameter(dbInsertUserCommand, PARAM_IPADDRESS, DbType.String, oBESession.sIPAddress);
                db.AddInParameter(dbInsertUserCommand, PARAM_HOSTNAME, DbType.String, oBESession.sHostName);

                db.ExecuteNonQuery(dbInsertUserCommand);


                DataSet ds = new DataSet();
                db = DatabaseFactory.CreateDatabase(DL_Shared.Connection);
                DbCommand dbSelectCommand = db.GetSqlStringCommand(SQL_SELECT_IDENTITY);
                db.AddInParameter(dbSelectCommand, PARAM_USERID, DbType.Int32, oBESession.iUserID);
                db.LoadDataSet(dbSelectCommand, ds, "IDENTITY");
                if (ds.Tables[0].Rows.Count > 0)
                    return Convert.ToInt32(ds.Tables[0].Rows[0][0]);
                else
                    return 0;
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                if (ex.Number == 547)
                {
                    //throw new ExceptionHandler.ExceptionType.DataAccessCustomException(BPA.GlobalResources.DataLayer.Resources.ReferenceConstraint);
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
        /// Inserts the session end.
        /// </summary>
        /// <param name="SessionID">The session ID.</param>
        public void InsertSessionEnd(int SessionID)
        {
           
                Database db = DL_Shared.dbFactory(_oTenant);
                DbCommand dbInsertUserCommand = db.GetSqlStringCommand(SQL_INSERT_SESSIONEND);
                db.AddInParameter(dbInsertUserCommand, PARAM_SESSIONID, DbType.Int32, SessionID);
                try
                {
                    db.ExecuteNonQuery(dbInsertUserCommand);
                }
                catch (System.Data.SqlClient.SqlException ex)
                {
                    if (ex.Number == 547)
                    {
                        //Response.Write(ex.Message);
                    }
                    if (ex.Number == 2627)
                    {
                        //throw new DataLayerException(Properties.Resources.UniqueConstraints);
                        //Response.Write(ex.Message);
                    }
                }
           
        }
        /// <summary>
        /// Finals the logout.
        /// </summary>
        /// <param name="UserID">The user ID.</param>
       public void FinalLogout(int UserID)
       {
           Database db = DL_Shared.dbFactory(_oTenant);
           DbCommand dbInsertCommand = db.GetStoredProcCommand(SP_FINAL_LOGOUT);
           db.AddInParameter(dbInsertCommand, PARAM_USERID, DbType.Int32, UserID);

           db.ExecuteNonQuery(dbInsertCommand);
       }
       /// <summary>
       /// Gets the user ID.
       /// </summary>
       /// <param name="SessionID">The session ID.</param>
       /// <returns></returns>
       public int GetUserID(int SessionID)
       {
           Database db = DL_Shared.dbFactory(_oTenant);
           db = DatabaseFactory.CreateDatabase(DL_Shared.Connection);
           DbCommand dbSelectCommand = db.GetSqlStringCommand(SQL_SELECT_USERID);
           db.AddInParameter(dbSelectCommand, PARAM_SESSIONID, DbType.Int32, SessionID );
           DataSet ds = new DataSet();
           db.LoadDataSet(dbSelectCommand, ds, "User");
           return Convert.ToInt32(ds.Tables[0].Rows[0][0]);
       }
       /// <summary>
       /// Gets the session ID.
       /// </summary>
       /// <param name="oBESession">The o BE session.</param>
       /// <returns></returns>
       public int GetSessionID(BESession oBESession)
       {
           int SessionID=0;
           Database db = DL_Shared.dbFactory(_oTenant);
           db = DatabaseFactory.CreateDatabase(DL_Shared.Connection);
           DbCommand dbSelectCommand = db.GetSqlStringCommand(SQL_SELECT_IDENTITY);
           db.AddInParameter(dbSelectCommand, PARAM_USERID, DbType.Int32, oBESession.iUserID);
           using (IDataReader rdr = db.ExecuteReader(dbSelectCommand))
           {
               while (rdr.Read())
               {
                   SessionID=Convert.ToInt32(rdr[0]);
               }
           }
           return SessionID;
          
       }
    }

    

}
