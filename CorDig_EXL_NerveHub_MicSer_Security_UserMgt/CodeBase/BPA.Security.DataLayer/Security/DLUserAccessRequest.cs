/* Copyright © 2007 company 
 * project Name                 :Datalayer
 * Class Name                   :DLUserAccessRequest
 * Namespace                    :BPA.Security.Datalayer.Security
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
using System.Data;
using System.Data.Common;

namespace BPA.Security.Datalayer.Security
{
    /// <summary>
    /// User Permission 
    /// </summary>
    public class DLUserAccessRequest : IDisposable
    {
        #region Fields
        private BETenant _oTenant = null;
        private const string SQL_SP_CLIENT = @"Usp_CDS_GetClientList";
        private const string SQL_SP_PROCESS = @"Usp_CDS_GetClientProcessList";
        private const string PARAM_USERID = "@UserID";       
        private const string PARAM_CLIENTNAME = "@ClientName";       
        private const string PARAM_ACTIVECLIENTLIST = "@ActiveClientList";
        private const string PARAM_ACTIVEPROCESSLIST = "@ActiveProcessList";
        private const string PARAM_CLIENTLIST = "@ClientList";
        private const string PARAM_LOGINID = "@Loginname";
        private const string PARAM_EMPID = "@Empid";
        private const string PARAM_FIRSTNAME = "@FirstName";
        private const string PARAM_MIDDLENAME ="@MiddleName";
        private const string PARAM_LASTNAME = "@LastName";
        private const string PARAM_EMAIL = "@Email";
        private const string PARAM_DISABLED = "@Disabled";
        private const string PARAM_MODIFIEDBY = "@ModifiedBy";
        private const string SQL_SP_GETUSERDETAILS = @"[Config].[USP_GetUserDetails]";
        private const string SQL_UPDATE_USERDETAILS = @"Update Config.tblUserMaster set loginname=@loginname,empid=@empid,FirstName=@FirstName,MiddleName=@MiddleName,LastName=@LastName,Email=@Email,disabled=@disabled,modifiedby=@ModifiedBy,modifiedon=GetDate()  where userid=@userid";
        #endregion

        #region Constructor and Dispose
        /// <summary>
        /// Initializes a new instance of the <see cref="DLUserAccessRequest"/> class.
        /// </summary>
        public DLUserAccessRequest(BETenant oTenant)
        { _oTenant = oTenant; }
        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        { _oTenant = null; }
        #endregion



        /// <summary>
        /// Gets the user details.
        /// </summary>
        /// <param name="iUserID">The i user ID.</param>
        /// <returns></returns>
        public DataSet GetUserDetails(int iUserID)
        {
            string[] aryTableNames={"UserDetails","ShiftDetails","ProjectDetails","QuotaDetails", "ERPDetails"};
            DataSet ds = new DataSet();
            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetStoredProcCommand(SQL_SP_GETUSERDETAILS);
            db.AddInParameter(dbCommand, PARAM_USERID, DbType.Int32, iUserID);
            db.LoadDataSet(dbCommand, ds, aryTableNames);
            return ds;
        }

        /// <summary>
        /// Gets the client list.
        /// </summary>
        /// <param name="iLoggedinUserID">The i loggedin user ID.</param>
        /// <param name="bActiveClient">if set to <c>true</c> [b active client].</param>
        /// <returns></returns>
        public DataSet GetClientList(int iLoggedinUserID, bool bActiveClient)
        {
            DataSet ds = new DataSet();
            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetStoredProcCommand(SQL_SP_CLIENT);
            db.AddInParameter(dbCommand, PARAM_CLIENTNAME, DbType.String, "%");        
            db.AddInParameter(dbCommand, PARAM_USERID, DbType.Int32, iLoggedinUserID);
            db.AddInParameter(dbCommand, PARAM_ACTIVECLIENTLIST, DbType.Boolean, bActiveClient);
            db.LoadDataSet(dbCommand, ds, "Record");
            return ds;

        }
        /// <summary>
        /// Gets the process list.
        /// </summary>
        /// <param name="iLoggedinUserID">The i loggedin user ID.</param>
        /// <param name="bActiveProcess">if set to <c>true</c> [b active process].</param>
        /// <param name="sClientId">The s client id.</param>
        /// <returns></returns>
        public DataSet GetProcessList(int iLoggedinUserID, bool bActiveProcess,string sClientId)
        {
            DataSet ds = new DataSet();
            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetStoredProcCommand(SQL_SP_PROCESS);
            db.AddInParameter(dbCommand, PARAM_USERID, DbType.Int32, iLoggedinUserID);
            db.AddInParameter(dbCommand, PARAM_ACTIVEPROCESSLIST, DbType.Boolean, bActiveProcess);
            db.AddInParameter(dbCommand, PARAM_CLIENTLIST, DbType.String, sClientId);
            db.LoadDataSet(dbCommand, ds, "Record");
            return ds;
            
        }
        public void UpdateUserDetails(BEUserInfo oUser)
        {
            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetSqlStringCommand(SQL_UPDATE_USERDETAILS);
            db.AddInParameter(dbCommand, PARAM_USERID, DbType.Int32, oUser.iUserID);
            db.AddInParameter(dbCommand, PARAM_LOGINID, DbType.String, oUser.sLoginName);
            db.AddInParameter(dbCommand, PARAM_EMPID, DbType.Int32, oUser.iEmployeeID);
            db.AddInParameter(dbCommand, PARAM_FIRSTNAME, DbType.String, oUser.sFirstName);
            db.AddInParameter(dbCommand, PARAM_MIDDLENAME, DbType.String, oUser.sMiddleName);
            db.AddInParameter(dbCommand, PARAM_LASTNAME, DbType.String, oUser.sLastName);
            db.AddInParameter(dbCommand, PARAM_EMAIL, DbType.String, oUser.sEmail);
            db.AddInParameter(dbCommand, PARAM_DISABLED, DbType.Boolean, oUser.bDisabled);
            db.AddInParameter(dbCommand, PARAM_MODIFIEDBY, DbType.Int32, oUser.iCreatedBy);
            db.ExecuteNonQuery(dbCommand);
        }
    }
}
