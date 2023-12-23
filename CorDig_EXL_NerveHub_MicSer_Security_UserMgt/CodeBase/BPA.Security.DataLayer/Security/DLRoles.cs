/* Copyright © 2007 company 
 * project Name                 :DataLayer
 * Class Name                   :DLRoles
 * Namespace                    :BPA.Security.Datalayer.Security
 * Purpose                      :Do all the datarelated operation for role management
 * Description                  :
 * Dependency                   :Microsoft.Practices.EnterpriseLibrary.Data,Microsoft.Practices.EnterpriseLibrary.Data.Sql
 * Related Table                :Config.tblRoleMaster,Config.tblRoleFormMapping
 * Related Class                :BERoleInfo
 * Related StoreProcdure        :
 * Author                       :Tarun Bounthiyal
 * Created on                   :27-mar-2007
 * Reviewed on                  :          
 * Reviewed by                  :
 * Tested on                    :
 * Tested by                    :
 * Modification history         :
 * modify1 on                   :24-apr-2007
 * modify1 By                   :Tarun Bounthiyal
 * Overall effect               :Get Roles By campaignID
 */




using BPA.Security.BusinessEntity.ExtrernalRefre;
using BPA.Security.BusinessEntity.Security;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data;
using System.Data.Common;
using System.Text;

namespace BPA.Security.Datalayer.Security
{
    /// <summary>
    /// Roles
    /// </summary>
    public class DLRoles : IDisposable
    {

        #region Fields
        private BETenant _oTenant = null;

        private const string SQL_SELECT_ROLESOFUSERTYPE = @"SELECT ROLEID,ROLENAME,Description,isnull(LevelID,0) as LevelID, disabled,isnull(IsClientRole,0) as IsClientRole,createdby FROM Config.tblRoleMaster (Nolock) WHERE Disabled=0 AND RoleName like @RoleName And IsClientRole=0 Order By ROLENAME";

        private const string SQL_SELECT_CLIENTROLES = @"SELECT ROLEID,ROLENAME,Description,isnull(LevelID,0) as LevelID, disabled,isnull(IsClientRole,0) as IsClientRole,createdby FROM Config.tblRoleMaster (Nolock) WHERE Disabled=0 AND RoleName like @RoleName And IsClientRole=1 Order By ROLENAME";

        private const string SQL_SELECT_CLIENTROLESALL = @"SELECT ROLEID,ROLENAME,Description,isnull(LevelID,0) as LevelID, disabled,isnull(IsClientRole,0) as IsClientRole,createdby FROM Config.tblRoleMaster (Nolock) WHERE RoleName like @RoleName And IsClientRole=1 Order By ROLENAME";


        private const string SQL_USP_GETCLIENTLISTBYUSER = @"[Config].[Usp_GetRoleListUserCreation]";

        private const string SQL_SELECT_ROLES = @"SELECT ROLEID,ROLENAME,Description,isnull(LevelID,0) as LevelID, disabled,isnull(IsClientRole,0) as IsClientRole,isnull(createdby,0) as createdby FROM Config.tblRoleMaster (Nolock) WHERE Disabled=0 AND RoleName like @RoleName  Order By ROLENAME";
        private const string SQL_SELECT_ROLESALL = @"SELECT ROLEID,ROLENAME,Description,isnull(LevelID,0) as LevelID, disabled,isnull(IsClientRole,0) as IsClientRole,isnull(createdby,0) as createdby FROM Config.tblRoleMaster (Nolock) WHERE RoleName like @RoleName  Order By ROLENAME";
        private const string SQL_SELECT_ROLESBYID = @"SELECT ROLEID,ROLENAME,Description, isnull(LevelID,0) as LevelID, isnull(SecurityGroup,0) as SecurityGroup,disabled,isnull(IsClientRole,0) as IsClientRole,createdby FROM Config.tblRoleMaster (Nolock) WHERE RoleID = @RoleID";


        private const string SQL_INSERT_ROLES = @"INSERT INTO Config.tblRoleMaster (RoleName,LevelID,Description,disabled,IsClientRole,SecurityGroup,CreatedBy)" +
                                                " VALUES (@RoleName,@LevelID,@Description,@disabled,@IsClientRole,@SecurityGroup,@CreatedBy)";
        private const string SQL_MAX_ROLEID = @"SELECT isnull(MAX(ROLEID),0) FROM Config.tblRoleMaster (Nolock) ";
        private const string SQL_INSERT_ROLESFORM = @"INSERT INTO Config.tblRoleFormMapping(RoleID, FormActionID, AllowAction, CreatedBy)" +
                                                    " SELECT  @RoleID, FAction.FormActionID, @AllowAction,@CreatedBy" +
                                                    " FROM Config.tblFormActionMap AS FAction WITH (NOLOCK) INNER JOIN" +
                                                    " Config.tblActionMaster AS SAction WITH (NOLOCK) ON FAction.ActionID = SAction.ActionID" +
                                                    " WHERE (FAction.FormID = @FormID) AND (SAction.ActionName = @ActionName)";

        private const string SQL_INSERT_ROLES_APPROVAL = @"If(@RequestType=1)
Begin  
    If not exists(select RoleId from Config.tblRoleMaster where ltrim(rtrim(RoleName))=ltrim(rtrim(@RoleName)))
    Begin
        If not exists(select RequestID from Config.tblRoleMaster_Approval where ltrim(rtrim(RoleName))=ltrim(rtrim(@RoleName)) and (IsApproved=0 and IsRejected=0 and IsCancelled=0))
        Begin       
            INSERT INTO Config.tblRoleMaster_Approval (RoleId,RoleName,LevelID,Description,ApproverID,disabled,IsClientRole,SecurityGroup,CreatedBy,RequestType)
            VALUES (@RoleID,@RoleName,@LevelID,@Description,@ApproverId,@disabled,@IsClientRole,0,@CreatedBy,@RequestType)
        End
        Else
        Begin
            Raiserror('A request for approval is pending for a role with the same name. Please get that request approved.',15,1)
        End
    End
    Else
    Begin
        Raiserror('Role already exists. Please provide different Role Name for addition.',15,1)
    End
End
Else
    Begin  
        If not exists(select RequestID from Config.tblRoleMaster_Approval where RoleId=@RoleId and (IsApproved=0 and IsRejected=0 and IsCancelled=0))
        Begin
            If(@RequestType=2)
            Begin
                If not exists(select RoleId from Config.tblRoleMaster where RoleId<>RoleId and ltrim(rtrim(RoleName))=ltrim(rtrim(@RoleName)))
                Begin      
                    INSERT INTO Config.tblRoleMaster_Approval (RoleId,RoleName,LevelID,Description,ApproverID,disabled,IsClientRole,SecurityGroup,CreatedBy,RequestType)
                    VALUES (@RoleID,@RoleName,@LevelID,@Description,@ApproverId,@disabled,@IsClientRole,0,@CreatedBy,@RequestType)
                End 
                Else
                Begin
                    Raiserror('Role already exists with the same name. Please provide different Role Name.',15,1)
                End
            End
            else
            Begin
                INSERT INTO Config.tblRoleMaster_Approval (RoleId,RoleName,LevelID,Description,ApproverID,disabled,IsClientRole,SecurityGroup,CreatedBy,RequestType)
                VALUES (@RoleID,@RoleName,@LevelID,@Description,@ApproverId,@disabled,@IsClientRole,0,@CreatedBy,@RequestType)
            End
        End
        Else
        Begin
            Raiserror('A request for approval is pending for this role. Please get that request approved first before making any role modification request.',15,1)
        End
    End";
        private const string SQL_MAX_REQUESTID_APPROVAL = @"SELECT isnull(MAX(REQUESTID),0) FROM Config.tblRoleMaster_Approval (Nolock) ";
        private const string SQL_INSERT_ROLESFORM_APPROVAL = @"INSERT INTO Config.tblRoleFormMapping_Approval(RequestId,RoleID, FormActionID, AllowAction, CreatedBy)" +
                                                    " SELECT  @RequestID,@RoleID, FAction.FormActionID, @AllowAction,@CreatedBy" +
                                                    " FROM Config.tblFormActionMap AS FAction WITH (NOLOCK) INNER JOIN" +
                                                    " Config.tblActionMaster AS SAction WITH (NOLOCK) ON FAction.ActionID = SAction.ActionID" +
                                                    " WHERE (FAction.FormID = @FormID) AND (SAction.ActionName = @ActionName)";

        private const string SQL_UPDATE_ROLES = @"UPDATE Config.tblRoleMaster SET RoleName=@RoleName,LevelID=@LevelID,Description=@Description,disabled=@disabled,IsClientRole=@IsClientRole,SecurityGroup=@SecurityGroup,ModifiedBy=@ModifiedBy WHERE RoleID=@RoleID";
        private const string SQL_DELETE_ROLESFROM = @"DELETE FROM Config.tblRoleFormMapping WHERE RoleID=@RoleID";
        private const string SQL_DELETE_ROLES = @"DELETE FROM Config.tblRoleMaster WHERE RoleID=@RoleID";

        private const string SP_GET_ROLE_REQUESTSTATUS = @"dbo.Usp_CDS_GetRoleRequestStatus";
        private const string SP_GET_ROLE_APPROVALLIST = @"dbo.Usp_CDS_GetRoleApprovalList";
        private const string SP_APPROVE_ROLEREQUEST = @"dbo.USP_CDS_ApproveRoleRequest";
        private const string SP_REJECT_ROLEREQUEST = @"dbo.USP_CDS_RejectRoleRequest";
        private const string SP_CANCEL_ROLEREQUEST = @"dbo.Usp_CDS_Cancel_RoleRequest";
        private const string SP_SENDMAIL_ROLEREQUEST = @"dbo.Usp_CDS_RoleMapping_SendMail";

        private const string SP_JOBCODE = @"Config.USP_GetJobDesc";

        private const string PARAM_ROLEID = "@RoleID";
        private const string PARAM_ROLENAME = "@RoleName";
        private const string PARAM_DESCRIPTION = "@Description";
        private const string PARAM_DISABLED = "@Disabled";

        private const string PARAM_LEVELID = "@LevelID";
        private const string PARAM_CREATEDBY = "@CreatedBY";
        private const string PARAM_MODIFIEDBY = "@ModifiedBy";

        private const string PARAM_FORMID = "@FormID";
        private const string PARAM_ALLOWACTION = "@AllowAction";
        private const string PARAM_ACTIONNAME = "@ActionName";
        private const string PARAM_ISCLIENTROLE = "@IsClientRole";
        private const string PARAM_SECURITYGROUP = "@SecurityGroup";
        private const string PARAM_REQUESTID = "@RequestID";
        private const string PARAM_REQUESTTYPE = "@RequestType";
        private const string PARAM_APPROVERID = "@ApproverId";
        private const string PARAM_MODE = "@Mode";
        private const string PARAM_USERID = "@UserId";
        private const string PARAM_FROMDATE = "@FromDate";
        private const string PARAM_TODATE = "@ToDate";
        private const string PARAM_MAILBODY = "@MailBody";
        #endregion

        #region Constructor & Dispose
        /// <summary>
        /// Initializes a new instance of the <see cref="DLRoles"/> class.
        /// </summary>
        public DLRoles(BETenant oTenant)
        { _oTenant = oTenant; }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        { _oTenant = null; }
        #endregion

        #region GetRoleList
        /// <summary>
        /// Gets the Client role list.
        /// </summary>
        /// <param name="sRoleName">Name of the role.</param>
        /// <param name="bActiveRoles">if set to <c>true</c> [active roles].</param>
        /// <returns></returns>
        public List<BERoleInfo> GetClientRoleListByUser(int userID)
        {
            List<BERoleInfo> lRoleInfo = new List<BERoleInfo>();
            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand;
            dbCommand = db.GetStoredProcCommand(SQL_USP_GETCLIENTLISTBYUSER);

            db.AddInParameter(dbCommand, PARAM_USERID, DbType.Int32, userID);

            using (IDataReader rdr = db.ExecuteReader(dbCommand))
            {
                // Scroll through the results
                while (rdr.Read())
                {
                    BERoleInfo item = new BERoleInfo(Convert.ToInt32(rdr["ROLEID"]), rdr["ROLENAME"].ToString(), "", Convert.ToInt32(rdr["UserLevelID"]), true, 1);
                    lRoleInfo.Add(item);
                }
            }
            return lRoleInfo;
        }




        /// <summary>
        /// Gets the role list.
        /// </summary>
        /// <param name="bActiveRoles">if set to <c>true</c> [active roles].</param>
        /// <returns></returns>
        public List<BERoleInfo> GetRoleList(bool bActiveRoles)
        {
            return GetRoleList("", bActiveRoles);
        }
        /// <summary>
        /// Gets the client role list.
        /// </summary>
        /// <param name="bActiveRoles">if set to <c>true</c> [b active roles].</param>
        /// <returns></returns>
        public List<BERoleInfo> GetClientRoleList(bool bActiveRoles)
        {
            return GetClientRoleList("", bActiveRoles);
        }
        /// <summary>
        /// Gets the Client role list.
        /// </summary>
        /// <param name="sRoleName">Name of the role.</param>
        /// <param name="bActiveRoles">if set to <c>true</c> [active roles].</param>
        /// <returns></returns>
        public List<BERoleInfo> GetClientRoleList(string sRoleName, bool bActiveRoles)
        {
            List<BERoleInfo> lRoleInfo = new List<BERoleInfo>();
            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand;
            if (bActiveRoles)
            { dbCommand = db.GetSqlStringCommand(SQL_SELECT_CLIENTROLES); }
            else
            { dbCommand = db.GetSqlStringCommand(SQL_SELECT_CLIENTROLESALL); }

            db.AddInParameter(dbCommand, PARAM_ROLENAME, DbType.String, "%" + sRoleName + "%");

            using (IDataReader rdr = db.ExecuteReader(dbCommand))
            {
                // Scroll through the results
                while (rdr.Read())
                {
                    BERoleInfo item = new BERoleInfo(Convert.ToInt32(rdr["ROLEID"]), rdr["ROLENAME"].ToString() + " " + (Convert.ToBoolean(rdr["disabled"]) ? "(Disabled)" : ""), rdr["Description"].ToString(), Convert.ToInt32(rdr["LevelID"]), Convert.ToBoolean(rdr["disabled"]), Convert.ToInt32(rdr["createdby"]));
                    lRoleInfo.Add(item);
                }
            }
            return lRoleInfo;
        }


        /// <summary>
        /// Gets the Client role list.
        /// </summary>
        /// <param name="sRoleName">Name of the role.</param>
        /// <param name="bActiveRoles">if set to <c>true</c> [active roles].</param>
        /// <returns></returns>
        public List<BERoleInfo> GetUsertRoleList(bool Isclient)
        {
            List<BERoleInfo> lRoleInfo = new List<BERoleInfo>();
            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand;
            if (Isclient)
            { dbCommand = db.GetSqlStringCommand(SQL_SELECT_CLIENTROLES); }
            else
            { dbCommand = db.GetSqlStringCommand(SQL_SELECT_ROLESOFUSERTYPE); }
            string sRoleName = "";
            db.AddInParameter(dbCommand, PARAM_ROLENAME, DbType.String, "%" + sRoleName + "%");
            using (IDataReader rdr = db.ExecuteReader(dbCommand))
            {
                // Scroll through the results
                while (rdr.Read())
                {
                    BERoleInfo item = new BERoleInfo(Convert.ToInt32(rdr["ROLEID"]), rdr["ROLENAME"].ToString() + " " + (Convert.ToBoolean(rdr["disabled"]) ? "(Disabled)" : ""), rdr["Description"].ToString(), Convert.ToInt32(rdr["LevelID"]), Convert.ToBoolean(rdr["disabled"]), Convert.ToInt32(rdr["createdby"]));
                    lRoleInfo.Add(item);
                }
            }
            return lRoleInfo;
        }



        /// <summary>
        /// Gets the role list.
        /// </summary>
        /// <param name="sRoleName">Name of the role.</param>
        /// <param name="bActiveRoles">if set to <c>true</c> [active roles].</param>
        /// <returns></returns>
        public List<BERoleInfo> GetRoleList(string sRoleName, bool bActiveRoles)
        {
            List<BERoleInfo> lRoleInfo = new List<BERoleInfo>();
            //Database db = DL_Shared.dbFactory(_oTenant);
            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand;
            if (bActiveRoles)
            { dbCommand = db.GetSqlStringCommand(SQL_SELECT_ROLES); }
            else
            { dbCommand = db.GetSqlStringCommand(SQL_SELECT_ROLESALL); }

            db.AddInParameter(dbCommand, PARAM_ROLENAME, DbType.String, "%" + sRoleName + "%");

            using (IDataReader rdr = db.ExecuteReader(dbCommand))
            {
                // Scroll through the results
                while (rdr.Read())
                {
                    BERoleInfo item = new BERoleInfo(Convert.ToInt32(rdr["ROLEID"]), rdr["ROLENAME"].ToString() + " " + (Convert.ToBoolean(rdr["disabled"]) ? "(Disabled)" : ""), rdr["Description"].ToString(), Convert.ToInt32(rdr["LevelID"]), Convert.ToBoolean(rdr["disabled"]), Convert.ToInt32(rdr["createdby"]));
                    lRoleInfo.Add(item);
                }
            }
            return lRoleInfo;
        }

        /// <summary>
        /// Gets the role list.
        /// </summary>
        /// <param name="iRoleID">The role ID.</param>
        /// <returns></returns>
        public List<BERoleInfo> GetRoleList(int iRoleID)
        {
            List<BERoleInfo> lRoleInfo = new List<BERoleInfo>();
            //Database db = DL_Shared.dbFactory(_oTenant);
            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetSqlStringCommand(SQL_SELECT_ROLESBYID);
            db.AddInParameter(dbCommand, PARAM_ROLEID, DbType.Int32, iRoleID);
            using (IDataReader rdr = db.ExecuteReader(dbCommand))
            {
                // Scroll through the results
                while (rdr.Read())
                {
                    BERoleInfo item = new BERoleInfo(Convert.ToInt32(rdr["ROLEID"]), rdr["ROLENAME"].ToString(), rdr["Description"].ToString(), Convert.ToInt32(rdr["LevelID"]), Convert.ToBoolean(rdr["disabled"]), Convert.ToInt32(rdr["createdby"]));
                    item.bIsClientRole = Convert.ToBoolean(rdr["IsClientRole"]);
                    //item.iSecurityGroup = Convert.ToInt32(rdr["SecurityGroup"]);
                    using (DLFormAction oFormAction = new DLFormAction(_oTenant))
                    {
                        item.dtFormData = oFormAction.GetFormAction(iRoleID).Tables[0];
                    }
                    lRoleInfo.Add(item);

                }
            }
            return lRoleInfo;
        }
        #endregion


        #region  GetRoleFormList
        /// <summary>
        /// Gets the role form list.
        /// </summary>
        /// <param name="iRoleID">The role ID.</param>
        /// <returns></returns>
        public List<BERoleInfo> GetRoleFormList(int iRoleID)
        {
            List<BERoleInfo> lRoleInfo = new List<BERoleInfo>();
            Database db = DL_Shared.dbFactory(_oTenant);
            //Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetSqlStringCommand(SQL_SELECT_ROLESBYID);
            db.AddInParameter(dbCommand, PARAM_ROLEID, DbType.Int32, iRoleID);
            using (IDataReader rdr = db.ExecuteReader(dbCommand))
            {
                // Scroll through the results
                while (rdr.Read())
                {
                    BERoleInfo item = new BERoleInfo(Convert.ToInt32(rdr["ROLEID"]), rdr["ROLENAME"].ToString(), rdr["Description"].ToString(), Convert.ToInt32(rdr["LevelID"]), Convert.ToBoolean(rdr["disabled"]), Convert.ToInt32(rdr["createdby"]));
                    lRoleInfo.Add(item);
                }
            }


            return lRoleInfo;
        }
        #endregion

        #region Insert the data.
        /// <summary>
        /// Insert the data.
        /// </summary>
        /// <param name="oRoles">The o roles.</param>
        public void InsertData(BERoleInfo oRoles, int iApproverID, int iMode, StringBuilder sbMailBody)
        {
            //try
            //{
            //Database db = DL_Shared.dbFactory(_oTenant);
            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetSqlStringCommand(SQL_INSERT_ROLES_APPROVAL);// Insert Roles Data for approval
            DbCommand dbMaxCommand = db.GetSqlStringCommand(SQL_MAX_REQUESTID_APPROVAL);
            DbCommand dbInsertFRoleCommand = db.GetSqlStringCommand(SQL_INSERT_ROLESFORM_APPROVAL);// Insert Role Form Mapping for approval
            DbCommand dbMailCommand = db.GetStoredProcCommand(SP_SENDMAIL_ROLEREQUEST);// Send Role request Email
            using (DbConnection conn = db.CreateConnection())
            {
                conn.Open();// Connection Open
                using (DbTransaction trans = conn.BeginTransaction()) //Start Transaction
                {
                    try
                    {
                        db.AddInParameter(dbCommand, PARAM_ROLEID, DbType.Int32, oRoles.iRoleID);
                        db.AddInParameter(dbCommand, PARAM_ROLENAME, DbType.String, oRoles.sRoleName);
                        db.AddInParameter(dbCommand, PARAM_DESCRIPTION, DbType.String, oRoles.sRoleDescription);
                        db.AddInParameter(dbCommand, PARAM_DISABLED, DbType.Boolean, oRoles.bDisabled);
                        db.AddInParameter(dbCommand, PARAM_APPROVERID, DbType.Int32, iApproverID);
                        db.AddInParameter(dbCommand, PARAM_REQUESTTYPE, DbType.Int32, iMode);
                        db.AddInParameter(dbCommand, PARAM_ISCLIENTROLE, DbType.Boolean, oRoles.bIsClientRole);
                        db.AddInParameter(dbCommand, PARAM_SECURITYGROUP, DbType.Int32, 0);
                        db.AddInParameter(dbCommand, PARAM_LEVELID, DbType.Int32, oRoles.iLevelID);
                        db.AddInParameter(dbCommand, PARAM_CREATEDBY, DbType.Int32, oRoles.iCreatedBy);
                        db.ExecuteNonQuery(dbCommand, trans);

                        db.AddInParameter(dbMailCommand, PARAM_APPROVERID, DbType.Int32, iApproverID);
                        db.AddInParameter(dbMailCommand, PARAM_MODE, DbType.Int32, iMode);
                        db.AddInParameter(dbMailCommand, PARAM_CREATEDBY, DbType.Int32, oRoles.iCreatedBy);
                        db.AddInParameter(dbMailCommand, PARAM_MAILBODY, DbType.String, sbMailBody.ToString());

                        int iRequestID = (int)db.ExecuteScalar(dbMaxCommand, trans);
                        if (iMode != 3)
                        {
                            db.AddInParameter(dbInsertFRoleCommand, PARAM_REQUESTID, DbType.Int32, iRequestID);
                            db.AddInParameter(dbInsertFRoleCommand, PARAM_ROLEID, DbType.Int32, oRoles.iRoleID);
                            db.AddInParameter(dbInsertFRoleCommand, PARAM_ACTIONNAME, DbType.String);
                            db.AddInParameter(dbInsertFRoleCommand, PARAM_FORMID, DbType.Int32);
                            db.AddInParameter(dbInsertFRoleCommand, PARAM_ALLOWACTION, DbType.Boolean);
                            db.AddInParameter(dbInsertFRoleCommand, PARAM_CREATEDBY, DbType.Int32);


                            DataTable dt = oRoles.dtFormData;
                            int Rowcount = dt.Rows.Count;
                            int ColCount = dt.Columns.Count;
                            //Stopwatch stopwatch = new();
                            //stopwatch.Start();

                            for (int i = 0; i < Rowcount; i++)// Loop through all rows
                            {

                                object otemp = dt.Rows[i]["FormID"];
                                int FormID = 0;
                                if (otemp != DBNull.Value)
                                {
                                    FormID = Convert.ToInt32(otemp);
                                }
                                for (int j = 4; j < ColCount - 1; j++)// Loop through all columns
                                {
                                    string ActionName = dt.Columns[j].ColumnName;
                                    bool AllowAction = false;
                                    otemp = dt.Rows[i][j];
                                    if (otemp != DBNull.Value)
                                    {
                                        AllowAction = Convert.ToBoolean(Convert.ToInt16(otemp));
                                    }

                                    db.SetParameterValue(dbInsertFRoleCommand, PARAM_ACTIONNAME, ActionName);
                                    db.SetParameterValue(dbInsertFRoleCommand, PARAM_FORMID, FormID);
                                    db.SetParameterValue(dbInsertFRoleCommand, PARAM_ALLOWACTION, AllowAction);
                                    db.SetParameterValue(dbInsertFRoleCommand, PARAM_CREATEDBY, oRoles.iCreatedBy);
                                    db.ExecuteNonQuery(dbInsertFRoleCommand, trans);
                                }
                            }

                            //stopwatch.Stop();
                            //long elapsedTime = stopwatch.ElapsedMilliseconds;
                        }
                        //db.ExecuteNonQuery(dbMailCommand, trans);
                        trans.Commit();// Commit transaction
                    }
                    /*
                    catch (System.Data.SqlClient.SqlException ex)
                    {
                        trans.Rollback();
                        if (ex.Number == 547)
                        {
                            // throw new ExceptionHandler.ExceptionType.DataAccessCustomException(BPA.GlobalResources.DataLayer.Resources.ReferenceConstraint);
                        }
                        if (ex.Number == 2627)
                        {
                            //throw new ExceptionHandler.ExceptionType.DataAccessCustomException(BPA.GlobalResources.DataLayer.Resources.UniqueConstraints);
                        }
                        //if (ex.Message.Contains(BPA.GlobalResources.DataLayer.Resources.msg_roleApprovalPending))
                        //{
                        //    throw new ExceptionHandler.ExceptionType.DataAccessCustomException(BPA.GlobalResources.DataLayer.Resources.msg_roleApprovalPending);
                        // }
                        //if (ex.Message.Contains(BPA.GlobalResources.DataLayer.Resources.msg_RoleAlreadyExists))
                        //{
                        //    throw new ExceptionHandler.ExceptionType.DataAccessCustomException(BPA.GlobalResources.DataLayer.Resources.msg_RoleAlreadyExists);
                        //}
                        //if (ex.Message.Contains(BPA.GlobalResources.DataLayer.Resources.msg_RoleAlreadySameName))
                        //{
                        //    throw new ExceptionHandler.ExceptionType.DataAccessCustomException(BPA.GlobalResources.DataLayer.Resources.msg_RoleAlreadySameName);
                        //}
                        //if (ex.Message.Contains(BPA.GlobalResources.DataLayer.Resources.msg_RoleApprovalBeforeMaking))
                        //{
                        //    throw new ExceptionHandler.ExceptionType.DataAccessCustomException(BPA.GlobalResources.DataLayer.Resources.msg_RoleApprovalBeforeMaking);
                        //}
                        throw;// new ExceptionHandler.ExceptionType.DataAccessException(ex.Message);
                    }*/
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        throw;// new ExceptionHandler.ExceptionType.DataAccessException(ex.Message);
                    }
                }
                conn.Close();
            }
            //}
            //catch (System.Data.SqlClient.SqlException ex)
            //{
            //    if (ex.Number == 547)
            //    {
            //        throw new ExceptionHandler.ExceptionType.DataAccessCustomException(BPA.GlobalResources.DataLayer.Resources.ReferenceConstraint);
            //    }
            //    if (ex.Number == 2627)
            //    {
            //        throw new ExceptionHandler.ExceptionType.DataAccessCustomException(BPA.GlobalResources.DataLayer.Resources.UniqueConstraints);
            //    }
            //    if (ex.Message.Contains(BPA.GlobalResources.DataLayer.Resources.msg_roleApprovalPending))
            //    {
            //        throw new ExceptionHandler.ExceptionType.DataAccessCustomException(BPA.GlobalResources.DataLayer.Resources.msg_roleApprovalPending);
            //    }
            //    if (ex.Message.Contains(BPA.GlobalResources.DataLayer.Resources.msg_RoleAlreadyExists))
            //    {
            //        throw new ExceptionHandler.ExceptionType.DataAccessCustomException(BPA.GlobalResources.DataLayer.Resources.msg_RoleAlreadyExists);
            //    }
            //    if (ex.Message.Contains(BPA.GlobalResources.DataLayer.Resources.msg_RoleAlreadySameName))
            //    {
            //        throw new ExceptionHandler.ExceptionType.DataAccessCustomException(BPA.GlobalResources.DataLayer.Resources.msg_RoleAlreadySameName);
            //    }
            //    if (ex.Message.Contains(BPA.GlobalResources.DataLayer.Resources.msg_RoleApprovalBeforeMaking))
            //    {
            //        throw new ExceptionHandler.ExceptionType.DataAccessCustomException(BPA.GlobalResources.DataLayer.Resources.msg_RoleApprovalBeforeMaking);
            //    }
            //    throw new ExceptionHandler.ExceptionType.DataAccessException(ex.Message);
            //}
            //catch (Exception ex)
            //{
            //    throw new ExceptionHandler.ExceptionType.DataAccessException(ex.Message);
            //}

        }
        #endregion

        #region Insert the data.
        /// <summary>
        /// Insert the data.
        /// </summary>
        /// <param name="oRoles">The o roles.</param>
        public void InsertData(BERoleInfo oRoles)
        {
            try
            {
                //Database db = DL_Shared.dbFactory(_oTenant);
                Database db = DL_Shared.dbFactory(_oTenant);
                DbCommand dbCommand = db.GetSqlStringCommand(SQL_INSERT_ROLES);// Insert Roles Data
                DbCommand dbMaxCommand = db.GetSqlStringCommand(SQL_MAX_ROLEID);
                DbCommand dbInsertFRoleCommand = db.GetSqlStringCommand(SQL_INSERT_ROLESFORM);// Insert Role Form Mapping

                using (DbConnection conn = db.CreateConnection())
                {
                    conn.Open();// Connection Open
                    using (DbTransaction trans = conn.BeginTransaction()) //Start Transaction
                    {
                        try
                        {
                            db.AddInParameter(dbCommand, PARAM_ROLEID, DbType.Int32, oRoles.iRoleID);
                            db.AddInParameter(dbCommand, PARAM_ROLENAME, DbType.String, oRoles.sRoleName);
                            db.AddInParameter(dbCommand, PARAM_DESCRIPTION, DbType.String, oRoles.sRoleDescription);
                            db.AddInParameter(dbCommand, PARAM_DISABLED, DbType.Boolean, oRoles.bDisabled);
                            db.AddInParameter(dbCommand, PARAM_ISCLIENTROLE, DbType.Boolean, oRoles.bIsClientRole);
                            db.AddInParameter(dbCommand, PARAM_SECURITYGROUP, DbType.Int32, oRoles.iSecurityGroup);
                            db.AddInParameter(dbCommand, PARAM_LEVELID, DbType.Int32, oRoles.iLevelID);
                            db.AddInParameter(dbCommand, PARAM_CREATEDBY, DbType.Int32, oRoles.iCreatedBy);
                            db.ExecuteNonQuery(dbCommand, trans);

                            int RoleID = (int)db.ExecuteScalar(dbMaxCommand, trans);

                            db.AddInParameter(dbInsertFRoleCommand, PARAM_ROLEID, DbType.Int32, RoleID);
                            db.AddInParameter(dbInsertFRoleCommand, PARAM_ACTIONNAME, DbType.String);
                            db.AddInParameter(dbInsertFRoleCommand, PARAM_FORMID, DbType.Int32);
                            db.AddInParameter(dbInsertFRoleCommand, PARAM_ALLOWACTION, DbType.Boolean);
                            db.AddInParameter(dbInsertFRoleCommand, PARAM_CREATEDBY, DbType.Int32);


                            DataTable dt = oRoles.dtFormData;
                            int Rowcount = dt.Rows.Count;
                            int ColCount = dt.Columns.Count;
                            for (int i = 0; i < Rowcount; i++)// Loop through all rows
                            {

                                object otemp = dt.Rows[i][0];
                                int FormID = 0;
                                if (otemp != DBNull.Value)
                                {
                                    FormID = Convert.ToInt32(otemp);
                                }
                                for (int j = 3; j < ColCount; j++)// Loop through all columns
                                {
                                    string ActionName = dt.Columns[j].ColumnName;
                                    bool AllowAction = false;
                                    otemp = dt.Rows[i][j];
                                    if (otemp != DBNull.Value)
                                    {
                                        AllowAction = Convert.ToBoolean(otemp);
                                    }

                                    db.SetParameterValue(dbInsertFRoleCommand, PARAM_ACTIONNAME, ActionName);
                                    db.SetParameterValue(dbInsertFRoleCommand, PARAM_FORMID, FormID);
                                    db.SetParameterValue(dbInsertFRoleCommand, PARAM_ALLOWACTION, AllowAction);
                                    db.SetParameterValue(dbInsertFRoleCommand, PARAM_CREATEDBY, oRoles.iCreatedBy);

                                    db.ExecuteNonQuery(dbInsertFRoleCommand, trans);
                                }


                            }
                            trans.Commit();// Commit transaction
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
        #endregion

        #region Update the data.
        /// <summary>
        /// Update the data.
        /// </summary>
        /// <param name="oRoles">The o roles.</param>
        public void UpdateData(BERoleInfo oRoles)
        {
            try
            {
                //Database db = DL_Shared.dbFactory(_oTenant);
                Database db = DL_Shared.dbFactory(_oTenant);
                DbCommand dbCommand = db.GetSqlStringCommand(SQL_UPDATE_ROLES);
                DbCommand dbDeleteFRoleCommand = db.GetSqlStringCommand(SQL_DELETE_ROLESFROM);
                DbCommand dbInsertFRoleCommand = db.GetSqlStringCommand(SQL_INSERT_ROLESFORM);

                using (DbConnection conn = db.CreateConnection())
                {
                    conn.Open();// Connection Open
                    using (DbTransaction trans = conn.BeginTransaction()) //Start Transaction
                    {
                        try
                        {
                            db.AddInParameter(dbCommand, PARAM_ROLEID, DbType.Int32, oRoles.iRoleID);
                            db.AddInParameter(dbCommand, PARAM_ROLENAME, DbType.String, oRoles.sRoleName);
                            db.AddInParameter(dbCommand, PARAM_DESCRIPTION, DbType.String, oRoles.sRoleDescription);
                            db.AddInParameter(dbCommand, PARAM_SECURITYGROUP, DbType.Int32, 0);
                            db.AddInParameter(dbCommand, PARAM_LEVELID, DbType.Int32, oRoles.iLevelID);
                            db.AddInParameter(dbCommand, PARAM_DISABLED, DbType.Boolean, oRoles.bDisabled);
                            db.AddInParameter(dbCommand, PARAM_MODIFIEDBY, DbType.Int32, oRoles.iModifiedBy);
                            db.AddInParameter(dbCommand, PARAM_ISCLIENTROLE, DbType.Boolean, oRoles.bIsClientRole);

                            db.ExecuteNonQuery(dbCommand, trans);

                            db.AddInParameter(dbDeleteFRoleCommand, PARAM_ROLEID, DbType.Int32, oRoles.iRoleID);
                            db.ExecuteNonQuery(dbDeleteFRoleCommand, trans);

                            db.AddInParameter(dbInsertFRoleCommand, PARAM_ROLEID, DbType.Int32);
                            db.AddInParameter(dbInsertFRoleCommand, PARAM_ACTIONNAME, DbType.String);
                            db.AddInParameter(dbInsertFRoleCommand, PARAM_FORMID, DbType.Int32);
                            db.AddInParameter(dbInsertFRoleCommand, PARAM_ALLOWACTION, DbType.Boolean);
                            db.AddInParameter(dbInsertFRoleCommand, PARAM_CREATEDBY, DbType.Int32);


                            DataTable dt = oRoles.dtFormData;
                            int Rowcount = dt.Rows.Count;
                            int ColCount = dt.Columns.Count;
                            for (int i = 0; i < Rowcount; i++)
                            {

                                object otemp = dt.Rows[i]["FormID"];
                                int FormID = 0;
                                if (otemp != DBNull.Value)
                                {
                                    FormID = Convert.ToInt32(otemp);
                                }
                                for (int j = 4; j < ColCount; j++)
                                {
                                    string ActionName = dt.Columns[j].ColumnName;
                                    bool AllowAction = false;
                                    otemp = dt.Rows[i][j];
                                    if (otemp != DBNull.Value)
                                    {
                                        AllowAction = Convert.ToBoolean(otemp);
                                    }
                                    db.SetParameterValue(dbInsertFRoleCommand, PARAM_ROLEID, oRoles.iRoleID);
                                    db.SetParameterValue(dbInsertFRoleCommand, PARAM_ACTIONNAME, ActionName);
                                    db.SetParameterValue(dbInsertFRoleCommand, PARAM_FORMID, FormID);
                                    db.SetParameterValue(dbInsertFRoleCommand, PARAM_ALLOWACTION, AllowAction);
                                    db.SetParameterValue(dbInsertFRoleCommand, PARAM_CREATEDBY, oRoles.iCreatedBy);

                                    db.ExecuteNonQuery(dbInsertFRoleCommand, trans);
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

        #region Delete the data.
        /// <summary>
        /// Delete the data.
        /// </summary>
        /// <param name="oRoles">The roles.</param>
        public void DeleteData(BERoleInfo oRoles)
        {
            try
            {
                //Database db = DL_Shared.dbFactory(_oTenant);
                Database db = DL_Shared.dbFactory(_oTenant);
                DbCommand dbCommand = db.GetSqlStringCommand(SQL_DELETE_ROLES);// Insert Roles Data
                DbCommand dbDeleteFRoleCommand = db.GetSqlStringCommand(SQL_DELETE_ROLESFROM);// Insert Role Form Mapping

                using (DbConnection conn = db.CreateConnection())
                {
                    conn.Open();// Connection Open
                    using (DbTransaction trans = conn.BeginTransaction()) //Start Transaction
                    {
                        try
                        {
                            db.AddInParameter(dbDeleteFRoleCommand, PARAM_ROLEID, DbType.Int32, oRoles.iRoleID);
                            db.ExecuteNonQuery(dbDeleteFRoleCommand, trans);
                            db.AddInParameter(dbCommand, PARAM_ROLEID, DbType.Int32, oRoles.iRoleID);
                            db.ExecuteNonQuery(dbCommand, trans);
                            trans.Commit();// Commit transaction
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

        #region Role Mapping Request Approval workflow
        /// <summary>
        /// Gets the Role Request status.
        /// </summary>
        /// <param name="iUser">The i user.</param>
        /// <param name="dtFromDate">The dt from date.</param>
        /// <param name="dtToDate">The dt to date.</param>
        /// <returns></returns>
        public DataSet GetRoleRequestStatus(int iUser, DateTime dtFromDate, DateTime dtToDate)
        {
            DataSet ds = new DataSet();
            Database db = DL_Shared.dbFactory(_oTenant);
            //Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetStoredProcCommand(SP_GET_ROLE_REQUESTSTATUS);
            db.AddInParameter(dbCommand, PARAM_USERID, DbType.Int32, iUser);
            db.AddInParameter(dbCommand, PARAM_FROMDATE, DbType.DateTime, dtFromDate);
            db.AddInParameter(dbCommand, PARAM_TODATE, DbType.DateTime, dtToDate);
            db.LoadDataSet(dbCommand, ds, "User");
            return ds;
        }
        /// <summary>
        /// Gets the Role approval list.
        /// </summary>
        /// <param name="iUser">The i user.</param>
        /// <returns></returns>
        public DataSet GetRoleApprovalList(int iUser)
        {
            DataSet ds = new DataSet();
            Database db = DL_Shared.dbFactory(_oTenant);
            //Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetStoredProcCommand(SP_GET_ROLE_APPROVALLIST);
            db.AddInParameter(dbCommand, PARAM_USERID, DbType.Int32, iUser);
            db.LoadDataSet(dbCommand, ds, "User");
            return ds;
        }
        /// <summary>
        /// Rejects Role Request
        /// </summary>
        /// <param name="iRequestID"></param>
        /// <param name="iUserID"></param>
        public void RejectRoleRequest(int iRequestID, int iUserID)
        {
            //Database db = DL_Shared.dbFactory(_oTenant);
            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetStoredProcCommand(SP_REJECT_ROLEREQUEST);
            db.AddInParameter(dbCommand, PARAM_REQUESTID, DbType.Int32, iRequestID);
            db.AddInParameter(dbCommand, PARAM_USERID, DbType.Int32, iUserID);
            try
            {
                db.ExecuteNonQuery(dbCommand);
            }
            /*
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
            }*/
            catch (Exception ex)
            {
                throw;// new ExceptionHandler.ExceptionType.DataAccessException(ex.Message);
            }
        }
        /// <summary>
        /// Approve Role request
        /// </summary>
        /// <param name="iRequestID"></param>
        /// <param name="iUserID"></param>
        public void ApproveRoleRequest(int iRequestID, int iUserID)
        {
            //Database db = DL_Shared.dbFactory(_oTenant);
            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetStoredProcCommand(SP_APPROVE_ROLEREQUEST);
            db.AddInParameter(dbCommand, PARAM_REQUESTID, DbType.Int32, iRequestID);
            db.AddInParameter(dbCommand, PARAM_USERID, DbType.Int32, iUserID);
            try
            {
                db.ExecuteNonQuery(dbCommand);
            }
            /*
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
            }*/
            catch (Exception ex)
            {
                throw;// new ExceptionHandler.ExceptionType.DataAccessException(ex.Message);
            }

        }
        /// <summary>
        /// Cancels ERP Job Role Request
        /// </summary>
        /// <param name="iRequestID"></param>
        /// <param name="iUserID"></param>
        public void CancelRoleRequest(int iRequestID, int iUserID)
        {
            //Database db = DL_Shared.dbFactory(_oTenant);
            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetStoredProcCommand(SP_CANCEL_ROLEREQUEST);
            db.AddInParameter(dbCommand, PARAM_REQUESTID, DbType.Int32, iRequestID);
            db.AddInParameter(dbCommand, PARAM_USERID, DbType.Int32, iUserID);
            try
            {
                db.ExecuteNonQuery(dbCommand);
            }
            catch (Exception ex)
            {
                throw;// new ExceptionHandler.ExceptionType.DataAccessException(ex.Message);
            }
        }
        #endregion

        #region GetJobCode
        public List<BEUserInfo> GetUserJobCode()
        {
            List<BEUserInfo> lJobCode = new List<BEUserInfo>();
            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand;
            dbCommand = db.GetStoredProcCommand(SP_JOBCODE);

            //  db.AddInParameter(dbCommand, PARAM_USERID, DbType.Int32, userID);

            using (IDataReader rdr = db.ExecuteReader(dbCommand))
            {
                // Scroll through the results
                while (rdr.Read())
                {
                    BEUserInfo item = new BEUserInfo();
                    item.iJobID = Convert.ToInt32(rdr["JOBID"]); ;
                    item.sJobDesc = rdr["JobDesc"].ToString();
                    item.iJobCodeID = Convert.ToInt32(rdr["JobCode"]); ;
                    lJobCode.Add(item);
                }
            }
            return lJobCode;
        }
        #endregion
    }
}
