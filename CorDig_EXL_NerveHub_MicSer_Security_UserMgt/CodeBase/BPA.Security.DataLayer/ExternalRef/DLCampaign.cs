using BPA.Security.BusinessEntity.ExtrernalRefre;
using BPA.Security.BusinessEntity.Security;
using BPA.Security.Datalayer;
using BPA.Security.DataLayer.ExternalRef.Utitlity;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPA.Security.DataLayer.ExternalRef
{
    public class DLCampaign : IDisposable
    {
        #region Fields
        private BETenant _oTenant = null;

        #region Select Fields
        private const string SQL_SP_CAMPAIGN = @"[WM].[Usp_GetCampaignList]";
        private const string SQL_SP_CAMPAIGNACCESSLIST = @"[Config].[Usp_GetAccessList]";
        //private const string SQL_SPPROCESSCAMPAIGN = @"[Usp_CDS_GetProcessCampaignList]";

        //        private const string SQL_SELECT_CAMPAIGNID = @"SELECT Campaign.CampaignID,Campaign.CampaignName,Campaign.ProcessID, Campaign.TimeZoneID
        //	                                                ,Campaign.Description,isnull(Campaign.EndDate,'')EndDate,Campaign.Disabled,Campaign.FreeField,Campaign.CreatedBy,Process.ClientID  
        //	                                                FROM Config.tblCampaignMaster Campaign (nolock) 
        //	                                                INNER JOIN Config.tblProcessMaster Process (nolock) 
        //	                                                ON Campaign.ProcessID = Process.ProcessID
        //	                                                WHERE CampaignID = @CampaignID";

        private const string SP_SELECT_CAMPAIGN = @"[Config].[USP_GetCampaignDetail]";
        private const string SQL_SP_PROCESSWISECAMPAIGN = @"dbo.Usp_CDS_GetProcessWiseCampaignList";

        private const string SQL_SP_ClIENTWISECAMPAIGNANDROLELIST = @"dbo.Usp_CDS_GetRoleObject";

        // private const string SQL_SP_PROCESSWISECAMPAIGNLISTANDROLELIST = @"Usp_CDS_GetProcessWiseCampaignListAndRoleList";
        private const string SQL_SP_PROCESSWISECAMPAIGNLISTANDROLELIST = @"SELECT DISTINCT Campaign.CampaignName AS CampaignName,Campaign.CampaignID,Campaign.Disabled,Campaign.ProcessID,Campaign.Description,isnull(Campaign.EndDate,'')EndDate,Campaign.TimeZoneID,Campaign.CreatedBy, CDS_H_S_RoleMaster.RoleName
                                                                         ,CDS_H_S_RoleMaster.RoleID AS RoleID FROM Config.tblCampaignMaster AS Campaign WITH (NOLOCK) INNER JOIN
                                                                        CDS_H_S_UserRoleMapping ON Campaign.CampaignID = CDS_H_S_UserRoleMapping.CampaignID INNER JOIN
                                                                        CDS_H_S_RoleMaster ON CDS_H_S_UserRoleMapping.RoleID = CDS_H_S_RoleMaster.RoleID
                                                                        WHERE (Campaign.CampaignID = @CampaignID)";

        private const string SQL_SP_PROCESSWISECAMPAIGNLIST = @"SELECT DISTINCT Campaign.CampaignName AS CampaignName,Campaign.CampaignID AS CampaignID FROM Config.tblCampaignMaster AS Campaign WHERE (Campaign.ProcessID =@ProcessID)";

        // Dead Code: Unused Field
        //        private const string SQL_SELECT_PROCESSWISECAMPAIGNANDROLE = @"SELECT DISTINCT CDS_H_S_RoleMaster.RoleID, CDS_H_S_RoleMaster.RoleName, Config.tblCampaignMaster.CampaignID, Config.tblCampaignMaster.CampaignName, 
        //                                                                     Config.tblCampaignMaster.ProcessID FROM Config.tblCampaignMaster INNER JOIN
        //                                                                     CDS_H_S_UserRoleMapping ON Config.tblCampaignMaster.CampaignID = CDS_H_S_UserRoleMapping.CampaignID INNER JOIN
        //                                                                     CDS_H_S_RoleMaster ON CDS_H_S_UserRoleMapping.RoleID = CDS_H_S_RoleMaster.RoleID
        //                                                                     WHERE (Config.tblCampaignMaster.ProcessID = @ProcessID)";

        private const string SQL_SELECT_CAMPSKILLBYID = @"select Skill.SkillID, Skill.SkillName 
                                                        from Config.tblCampaignMaster CampMaster(NOLOCK)
                                                        inner join Config.tblCampaignSkillMap CampSkillMap (NOLOCK)
	                                                        on CampMaster.CampaignID=CampSkillMap.CampaignID 
                                                        INNER JOIN Config.tblSkillMaster Skill (NOLOCK) 
	                                                        on Skill.SkillID=CampSkillMap.SkillID 
                                                        where CampMaster.CampaignID=@CampaignID and CampSkillMap.Disabled=0";

        private const string SQL_SELECT_MAXCAMPAIGN = @"SELECT MAX(CampaignID) FROM Config.tblCampaignMaster (NOLOCK)";
        private const string SQL_SELECT_CHECKSKILL = @"SELECT * FROM Config.tblCampaignSkillMap (NOLOCK) Where CampaignID=@CampaignID and SkillID =@SkillID";
        private const string SQL_SELECT_LOGINNAMEWISECAMPAIGNLIST = @"SELECT DISTINCT Config.tblCampaignMaster.CampaignName, Config.tblProcessMaster.ProcessName,Config.tblUserMaster.UserID AS UserID, Config.tblUserMaster.LoginName, 
                                                                    Config.tblUserMaster.FirstName + '' + Config.tblUserMaster.MiddleName + ' ' + Config.tblUserMaster.LastName AS EmployeeName, 
                                                                    Config.tblRoleMaster.RoleName, Config.tblUserLevel.LevelName, Config.tblFacilities.FacilityName, Config.tblLocationMaster.LocationName
                                                                    FROM Config.tblProcessMaster WITH (NOLOCK) INNER JOIN
                                                                    Config.tblCampaignMaster WITH (NOLOCK) ON Config.tblProcessMaster.ProcessID = Config.tblCampaignMaster.ProcessID INNER JOIN
                                                                    Config.tblUserMaster WITH (NOLOCK) INNER JOIN
                                                                    Config.tblUserRoleMapping WITH (NOLOCK) ON Config.tblUserMaster.UserID = Config.tblUserRoleMapping.UserID ON 
                                                                    Config.tblCampaignMaster.CampaignID = Config.tblUserRoleMapping.CampaignID INNER JOIN
                                                                    Config.tblRoleMaster WITH (NOLOCK) ON Config.tblUserRoleMapping.RoleID = Config.tblRoleMaster.RoleID INNER JOIN
                                                                    Config.tblUserLevel WITH (NOLOCK) ON Config.tblUserMaster.UserLevelID = Config.tblUserLevel.UserLevelID INNER JOIN
                                                                    Config.tblFacilities WITH (NOLOCK) ON Config.tblUserMaster.FacilityId = Config.tblFacilities.FacilityID INNER JOIN
                                                                    Config.tblLocationMaster WITH (NOLOCK) ON Config.tblLocationMaster.LocationID = Config.tblFacilities.LocationID
                                                                    WHERE (Config.tblUserRoleMapping.Disabled=0) AND (Config.tblUserMaster.LoginName = @LoginName)";
        private const string SQL_SELECT_USERID = @"SELECT UserID FROM Config.tblUserMaster WHERE LoginName=@LoginName";
        private const string SQL_SELECT_ROLEID = @"SELECT RoleID FROM Config.tblRoleMaster WHERE RoleName='Agents'";

        private const string SQL_SELECT_ALLCAMPAIGN = "SELECT CampaignID,ProcessID,TimeZoneID,CampaignName ,Description,isnull(EndDate,'')EndDate,Disabled,FreeField,CreatedBy FROM Config.tblCampaignMaster WHERE Disabled=0";

        private const string SQL_SELECT_PANDINGCAMPAIGNAPPROVAL = @"[Config].[USP_GetPendingCampaignApproval]";

        private const string SQL_SELECT_DSTOREID = "select max(wm.tblDStoreCampaignMap.DStoreID) as DStoreID from  wm.tblDStoreCampaignMap (NOLOCK) where wm.tblDStoreCampaignMap.CampaignID=@CampaignID and wm.tblDStoreCampaignMap.Disabled=0";


        #endregion

        #region Insert Fields
        private const string SP_INSERT_CAMPAIGN = @"[Config].[USP_InsertCampaignMasterDetail]";
        // private const string SQL_INSERT_CAMPAIGN = @"INSERT INTO Config.tblCampaignMaster (CampaignName,Description,EndDate,ProcessID,TimeZoneID, Disabled,FreeField, CreatedBy) VALUES(@CampaignName,@CampaignDescription,@EndDate,@ProcessID,@TimeZoneID,@Disabled,@FreeField, @CreatedBy)";
        private const string SQL_INSERT_CAMPAIGNSKILL = @"INSERT INTO Config.tblCampaignSkillMap(CampaignID, SkillID, CreatedBy) values(@CampaignID, @SkillID, @CreatedBy)";

        private const string SP_FETCH_CAMPAIGN_REQUEST_DETAILS = @"[Config].[USP_FetchCampaignRequestDetails]";
        private const string SP_INSERT_CAMPAIGNCHNAGEREQUEST = @"[Config].[USP_InsertCampaignChangeRequest]";
        #endregion

        #region Update Fields
        // private const string SQL_UPDATE_CAMPAIGN = @"UPDATE Config.tblCampaignMaster SET CampaignName=@CampaignName,Description=@CampaignDescription,EndDate=@EndDate,ProcessID=@ProcessID,TimeZoneID=@TimeZoneID, MODIFIEDBY=@ModifiedBy,ModifiedON=GetDate() , Disabled=@Disabled,FreeField=@FreeField WHERE CampaignID=@CampaignID";
        private const string SP_UPDATE_CAMPAIGN = @"[Config].[USP_UpdateCampaignMasterDetail]";
        private const string SQL_UPDATE_INACTIVATESKILL = @"UPDATE Config.tblCampaignSkillMap SET Disabled=1 WHERE CampaignID=@CampaignID";
        private const string SQL_UPDATE_ACTIVATESKILL = @"UPDATE Config.tblCampaignSkillMap SET Disabled=0 Where CampaignID=@CampaignID and SkillID =@SkillID";

        #endregion

        #region Delete Fields

        private const string SQL_DELETE_CAMPAIGN = @"DELETE FROM Config.tblCampaignMaster WHERE CampaignID=@CampaignID";
        private const string SQL_DELETE_CAMPSKILLMAP = @"DELETE FROM Config.tblCampaignSkillMap WHERE CampaignID=@CampaignID";
        #endregion
        private const string SQL_USP_GETAPPROVERLIST = @"[Config].[Usp_GetApproverList]";

        private const string PARAM_CAMPAIGNID = "@CampaignID";
        private const string PARAM_CAMPAIGNNAME = "@CampaignName";
        private const string PARAM_CAMPAIGNDESC = "@CampaignDescription";
        private const string PARAM_EndDate = "@EndDate";

        private const string PARAM_PROCESSID = "@ProcessID";
        private const string PARAM_USERID = "@UserID";
        private const string PARAM_AGENTID = "@AgentID";
        private const string PARAM_FLAG = "@Flag";
        private const string PARAM_FORMID = "@FormID";
        private const string PARAM_TIMEZONEID = "@TimeZoneID";
        private const string PARAM_DISABLED = "@Disabled";

        private const string PARAM_FREEFIELD = "@FreeField";

        private const string PARAM_CREATEDBY = "@CreatedBy";
        private const string PARAM_MODIFIEDBY = "@ModifiedBy";
        private const string PARAM_SKILLID = "@SkillID";
        private const string PARAM_CLIENTID = "@ClientID";
        private const string PARAM_DASHBOARDREQUEST = "@DashboardRequest";
        private const string PARAM_ACTIVECAMPAIGNLIST = "@ACTIVECAMPAIGNLIST";
        private const string PARAM_LOGINNAME = "@LoginName";
        private const string PARAM_ISACTIVE = "@IsActive";
        private const string PARAM_STARTDATE = "@Startdate";
        private const string PARAM_ENDDATE = "@EndDate";

        private const string PARAM_MODEIDS = "@ModeIds";
        private const string PARAM_EMAILS = "@Emails";
        private const string PARAM_BILLINGSYSTEM = "@BillingSystem";
        private const string PARAM_THRESHOLDFORCOMPLETION = "@ThresholdForCompletion";
        private const string PARAM_THRESHOLDFORTOOPEN = "@ThresholdForToOpen";
        private const string PARAM_TARGETEFFICIENCY = "@TargetEfficiency";


        private const string PARAM_LOCATION = "@Location";
        private const string PARAM_SHIFTWINDOWS = "@ShiftWindow";
        private const string PARAM_ISWORKMANAGEMENT = "@IsWorkManagement";
        private const string PARAM_ISTRASACTIONMONTORING = "@IsTransactionMontoring";
        private const string PARAM_ISTIMEMANAMENT = "@IsTimeManagement";
        private const string PARAM_BUSINESSJUSTIFICATION = "@BusinessJustification";
        private const string PARAM_TRAGENTUSERSQ1 = "@TargetUsersQ1";
        private const string PARAM_TRAGENTUSERSQ2 = "@TargetUsersQ2";
        private const string PARAM_TRAGENTUSERSQ3 = "@TargetUsersQ3";
        private const string PARAM_TRAGENTUSERSY1 = "@TargetUsersY1";
        private const string PARAM_TRAGENTUSERSY2 = "@TargetUsersY2";
        private const string PARAM_TRAGENTUSERSY3 = "@TargetUsersY3";
        private const string PARAM_KEYBENFITS = "@KeyBenfits";
        private const string PARAM_BUSINESSAPPROVERID = "@BusinessApproverId";
        private const string PARAM_TECHNOLOGYAPPROVERID = "@TechnologyApproverId";
        private const string PARAM_STATUS = "@staus";
        private const string PARAM_APPROVALID = "@ApprovalId";
        private const string PARAM_CHANGEREQUEST = "@ChangeRequest";
        private const string PARAM_USERLEVEL = "@UserLevel";


        #endregion

        #region Constructor and Dispose
        /// <summary>
        /// Initializes a new instance of the <see cref="DLClient"/> class.
        /// </summary>
        public DLCampaign(BETenant oTenant)
        {
            _oTenant = oTenant;
        }
        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            _oTenant = null;
        }
        #endregion

        #region GetCampaignList
        /// <summary>
        /// Gets the campaign list.
        /// </summary>
        /// <param name="iLoggedinUserID">The loggedin user ID.</param>
        /// <param name="bActiveCampaign">if set to <c>true</c> [active campaign].</param>
        /// <returns></returns>
        public List<BECampaignInfo> GetCampaignList(int iLoggedinUserID, bool bActiveCampaign)
        {
            return GetCampaignList(iLoggedinUserID, "", bActiveCampaign, false);
        }
        /// <summary>
        /// Gets the DstoreID .
        /// </summary>
        /// <param name="Campaignid">Campaignid.</param>
        /// <returns></returns>
        public string GetDStoreID(string Campaignid)
        {
            List<BECampaignInfo> lCampaign = new List<BECampaignInfo>();
            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetSqlStringCommand(SQL_SELECT_DSTOREID);
            db.AddInParameter(dbCommand, PARAM_CAMPAIGNID, DbType.Int32, Convert.ToInt32(Campaignid));
            string DStoreID = "0";
            using (IDataReader rdr = db.ExecuteReader(dbCommand))
            {
                while (rdr.Read()) { DStoreID = rdr["DStoreID"].ToString(); }
            }
            return DStoreID;
        }
        /// <summary>
        /// Gets the campaign list.
        /// </summary>
        /// <param name="iLoggedinUserID">The loggedin user ID.</param>
        /// <param name="CampaignName">Name of the campaign.</param>
        /// <param name="bActiveCampaign">if set to <c>true</c> [active campaign].</param>
        /// <returns></returns>
        public List<BECampaignInfo> GetCampaignList(int iLoggedinUserID, string CampaignName, bool bActiveCampaign, bool DashboardRequest)
        {
            List<BECampaignInfo> lCampaign = new List<BECampaignInfo>();
            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetStoredProcCommand(SQL_SP_CAMPAIGN);
            db.AddInParameter(dbCommand, PARAM_CAMPAIGNNAME, DbType.String, "" + CampaignName + "%");
            db.AddInParameter(dbCommand, PARAM_USERID, DbType.Int32, iLoggedinUserID);
            db.AddInParameter(dbCommand, PARAM_ACTIVECAMPAIGNLIST, DbType.Boolean, bActiveCampaign);
            db.AddInParameter(dbCommand, PARAM_DASHBOARDREQUEST, DbType.Boolean, DashboardRequest);

            using (IDataReader rdr = db.ExecuteReader(dbCommand))
            {
                while (rdr.Read())
                {
                    BECampaignInfo objCamp = new BECampaignInfo
                    {
                        iCampaignID = int.Parse(rdr["CampaignID"].ToString()),
                        sCampaignName = rdr["CampaignName"].ToString().DecodeHtmlString() + " " + (Convert.ToBoolean(rdr["Disabled"]) ? "(Disabled)" : ""),
                        sCampaignDescription = rdr["Description"].ToString().DecodeHtmlString(),
                        dtEndDate = Convert.ToDateTime(rdr["EndDate"].ToString()),
                        iProcessID = string.IsNullOrEmpty(rdr["ProcessID"].ToString()) ? 0 : int.Parse(rdr["ProcessID"].ToString()),
                        iTimeZoneID = string.IsNullOrEmpty(rdr["TimeZoneID"].ToString()) ? 0 : int.Parse(rdr["TimeZoneID"].ToString()),
                        bDisabled = string.IsNullOrEmpty(rdr["Disabled"].ToString()) ? false : bool.Parse(rdr["Disabled"].ToString()),
                        iCreatedBy = string.IsNullOrEmpty(rdr["CreatedBy"].ToString()) ? 0 : int.Parse(rdr["CreatedBy"].ToString())
                    };
                    lCampaign.Add(objCamp);
                    objCamp = null;
                }
            }
            return lCampaign;
        }
        /// <summary>
        /// To Get Campaign access list
        /// </summary>
        /// <param name="iLoggedinUserID"></param>
        /// <param name="iAgentID"></param>
        /// <param name="bActiveCampaign"></param>
        /// <returns></returns>
        public List<BECampaignInfo> GetCampaignAccessList(int iLoggedinUserID, int iAgentID, bool bActiveCampaign)
        {
            List<BECampaignInfo> lCampaign = new List<BECampaignInfo>();
            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetStoredProcCommand(SQL_SP_CAMPAIGNACCESSLIST);
            db.AddInParameter(dbCommand, PARAM_USERID, DbType.Int32, iLoggedinUserID);
            db.AddInParameter(dbCommand, PARAM_ISACTIVE, DbType.Boolean, bActiveCampaign);
            db.AddInParameter(dbCommand, PARAM_AGENTID, DbType.Int32, iAgentID);
            db.AddInParameter(dbCommand, PARAM_FLAG, DbType.Int32, 3);

            using (IDataReader rdr = db.ExecuteReader(dbCommand))
            {
                while (rdr.Read())
                {
                    BECampaignInfo objCamp = new BECampaignInfo
                    {
                        iCampaignID = int.Parse(rdr["CampaignID"].ToString()),
                        sCampaignName = rdr["CampaignName"].ToString().DecodeHtmlString() + " " + (Convert.ToBoolean(rdr["Disabled"]) ? "(Disabled)" : ""),
                        sCampaignDescription = rdr["Description"].ToString().DecodeHtmlString(),
                        dtEndDate = Convert.ToDateTime(rdr["EndDate"].ToString()),
                        iProcessID = string.IsNullOrEmpty(rdr["ProcessID"].ToString()) ? 0 : int.Parse(rdr["ProcessID"].ToString()),
                        iTimeZoneID = string.IsNullOrEmpty(rdr["TimeZoneID"].ToString()) ? 0 : int.Parse(rdr["TimeZoneID"].ToString()),
                        bDisabled = string.IsNullOrEmpty(rdr["Disabled"].ToString()) ? false : bool.Parse(rdr["Disabled"].ToString()),
                        iCreatedBy = string.IsNullOrEmpty(rdr["CreatedBy"].ToString()) ? 0 : int.Parse(rdr["CreatedBy"].ToString())
                    };
                    lCampaign.Add(objCamp);
                    objCamp = null;
                }
            }
            return lCampaign;
        }
        /// <summary>
        /// Gets the campaign list.
        /// </summary>
        /// <param name="iLoggedinUserID">The i loggedin user ID.</param>
        /// <param name="iProcessID">The i process ID.</param>
        /// <param name="CampaignName">Name of the campaign.</param>
        /// <param name="bActiveCampaign">if set to <c>true</c> [b active campaign].</param>
        /// <returns></returns>
        public List<BECampaignInfo> GetCampaignList(int iLoggedinUserID, int iProcessID, string sCampaignName, bool bActiveCampaign)
        {
            List<BECampaignInfo> lCampaign = new List<BECampaignInfo>();

            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetStoredProcCommand(SQL_SP_CAMPAIGN);
            db.AddInParameter(dbCommand, PARAM_CAMPAIGNNAME, DbType.String, "" + sCampaignName + "%");
            db.AddInParameter(dbCommand, PARAM_USERID, DbType.Int32, iLoggedinUserID);
            db.AddInParameter(dbCommand, PARAM_PROCESSID, DbType.Int32, iProcessID);
            db.AddInParameter(dbCommand, PARAM_ACTIVECAMPAIGNLIST, DbType.Boolean, bActiveCampaign);
            db.AddInParameter(dbCommand, PARAM_DASHBOARDREQUEST, DbType.Boolean, false);

            using (IDataReader rdr = db.ExecuteReader(dbCommand))
            {
                while (rdr.Read())
                {
                    BECampaignInfo objCamp = new BECampaignInfo
                    {
                        iCampaignID = int.Parse(rdr["CampaignID"].ToString()),
                        sCampaignName = rdr["CampaignName"].ToString().DecodeHtmlString() + " " + (Convert.ToBoolean(rdr["Disabled"].ToString()) ? "(Disabled)" : ""),
                        sCampaignDescription = rdr["Description"].ToString().DecodeHtmlString(),
                        dtEndDate = Convert.ToDateTime(rdr["EndDate"].ToString()),
                        iProcessID = string.IsNullOrEmpty(rdr["ProcessID"].ToString()) ? 0 : int.Parse(rdr["ProcessID"].ToString()),
                        iTimeZoneID = string.IsNullOrEmpty(rdr["TimeZoneID"].ToString()) ? 0 : int.Parse(rdr["TimeZoneID"].ToString()),
                        bDisabled = string.IsNullOrEmpty(rdr["Disabled"].ToString()) ? false : bool.Parse(rdr["Disabled"].ToString()),
                        iCreatedBy = string.IsNullOrEmpty(rdr["CreatedBy"].ToString()) ? 0 : int.Parse(rdr["CreatedBy"].ToString())
                    };
                    lCampaign.Add(objCamp);
                    objCamp = null;
                }
            }
            return lCampaign;
        }

        /// <summary>
        /// Gets the campaign list.
        /// </summary>
        /// <param name="CampaignID">The campaign ID.</param>
        /// <returns></returns>
        public List<BECampaignInfo> GetCampaignList(int CampaignID)
        {
            List<BECampaignInfo> lCampaign = new List<BECampaignInfo>();
            //IList<BEParameterInfo> lParameterInfo = new List<BEParameterInfo>();
            IList<BESkillInfo> lSkillInfo = new List<BESkillInfo>();
            //IList<BETerminationCodeInfo> lTerminationCodeInfo = new List<BETerminationCodeInfo>();
            Database db = DL_Shared.dbFactory(_oTenant);
            // Database db = DL_Shared.dbFactory(_oTenant);
            //DbCommand dbCommand = db.GetSqlStringCommand(SQL_SELECT_CAMPAIGNID);
            DbCommand dbCommand = db.GetStoredProcCommand(SP_SELECT_CAMPAIGN);

            db.AddInParameter(dbCommand, PARAM_CAMPAIGNID, DbType.Int32, CampaignID);

            DbCommand dbSkillCommand = db.GetSqlStringCommand(SQL_SELECT_CAMPSKILLBYID);
            db.AddInParameter(dbSkillCommand, PARAM_CAMPAIGNID, DbType.Int32, CampaignID);

            using (IDataReader rdr = db.ExecuteReader(dbCommand))
            {
                while (rdr.Read())
                {
                    BECampaignInfo objCamp = new BECampaignInfo
                    {
                        iCampaignID = int.Parse(rdr["CampaignID"].ToString()),
                        sCampaignName = rdr["CampaignName"].ToString().DecodeHtmlString(),
                        sCampaignDescription = rdr["Description"].ToString().DecodeHtmlString(),
                        sModeIds = rdr["ModeIds"].ToString() == "" ? "" : rdr["ModeIds"].ToString(),
                        dtEndDate = DateTime.Parse(rdr["EndDate"].ToString()),
                        iProcessID = string.IsNullOrEmpty(rdr["ProcessID"].ToString()) ? 0 : int.Parse(rdr["ProcessID"].ToString()),
                        iTimeZoneID = string.IsNullOrEmpty(rdr["TimeZoneID"].ToString()) ? 0 : int.Parse(rdr["TimeZoneID"].ToString()),
                        bDisabled = string.IsNullOrEmpty(rdr["Disabled"].ToString()) ? false : bool.Parse(rdr["Disabled"].ToString()),
                        iCreatedBy = string.IsNullOrEmpty(rdr["CreatedBy"].ToString()) ? 0 : int.Parse(rdr["CreatedBy"].ToString()),


                        bFreeField = bool.Parse(rdr["FreeField"].ToString() == "" ? "false" : rdr["FreeField"].ToString()),
                        sLocations = rdr["Location"] == DBNull.Value ? "" : rdr["Location"].ToString(),
                        sShiftwindows = rdr["ShiftWindow"] == DBNull.Value ? "" : rdr["ShiftWindow"].ToString(),
                        bPurposesWM = rdr["IsWorkManagement"] == DBNull.Value ? false : bool.Parse(rdr["IsWorkManagement"].ToString()),
                        bPurposeTime = rdr["IsTransactionMontoring"] == DBNull.Value ? false : bool.Parse(rdr["IsTransactionMontoring"].ToString()),
                        bPurposeTrans = rdr["IsTimeManagement"] == DBNull.Value ? false : bool.Parse(rdr["IsTimeManagement"].ToString()),
                        sBusinessJustifications = rdr["BusinessJustification"] == DBNull.Value ? "" : rdr["BusinessJustification"].ToString(),
                        sTargetq1 = rdr["TargetUsersQ1"] == DBNull.Value ? "" : rdr["TargetUsersQ1"].ToString(),
                        sTargetq2 = rdr["TargetUsersQ2"] == DBNull.Value ? "" : rdr["TargetUsersQ2"].ToString(),
                        sTargetq3 = rdr["TargetUsersQ3"] == DBNull.Value ? "" : rdr["TargetUsersQ3"].ToString(),
                        sTargety1 = rdr["TargetUsersY1"] == DBNull.Value ? "" : rdr["TargetUsersY1"].ToString(),
                        sTargety2 = rdr["TargetUsersY2"] == DBNull.Value ? "" : rdr["TargetUsersY2"].ToString(),
                        sTargety3 = rdr["TargetUsersY3"] == DBNull.Value ? "" : rdr["TargetUsersY3"].ToString(),
                        sKeyBenefits = rdr["KeyBenfits"] == DBNull.Value ? "" : rdr["KeyBenfits"].ToString(),
                        bBillingSystem = bool.Parse(rdr["BillingSystem"].ToString()),
                        iThresholdForCompletion = int.Parse(rdr["ThresholdForCompletion"].ToString()),
                        iThresholdForToOpen = int.Parse(rdr["ThresholdForToOpen"].ToString()),
                        dTargetEfficiency = double.Parse(rdr["TargetEfficiency"].ToString()),
                        sEmail = rdr["Emails"] == DBNull.Value ? "" : rdr["Emails"].ToString()
                    };

                    objCamp.iClientID = Convert.ToInt32(rdr["ClientID"]);
                    using (IDataReader dr = db.ExecuteReader(dbSkillCommand))
                    {
                        // Scroll through the results
                        while (dr.Read())
                        {
                            BESkillInfo SkillItem = new BESkillInfo();
                            SkillItem.iSkillID = Convert.ToInt32(dr["SkillID"]);
                            SkillItem.sSkillName = dr["SkillName"].ToString();
                            lSkillInfo.Add(SkillItem);
                            SkillItem = null;
                        }
                    }
                    objCamp.iSkillID = lSkillInfo;
                    lCampaign.Add(objCamp);
                    objCamp = null;
                }
            }
            return lCampaign;
            //List<BECampaignInfo> lCampaign = new List<BECampaignInfo>();
            ////IList<BEParameterInfo> lParameterInfo = new List<BEParameterInfo>();
            //List<BESkillInfo> lSkillInfo = new List<BESkillInfo>();
            ////IList<BETerminationCodeInfo> lTerminationCodeInfo = new List<BETerminationCodeInfo>();


            // Database db = DL_Shared.dbFactory(_oTenant);
            ////DbCommand dbCommand = db.GetSqlStringCommand(SQL_SELECT_CAMPAIGNID);
            //DbCommand dbCommand = db.GetSqlStringCommand(SQL_SELECT_CAMPAIGNID);

            //db.AddInParameter(dbCommand, PARAM_CAMPAIGNID, DbType.Int32, CampaignID);

            //DbCommand dbSkillCommand = db.GetSqlStringCommand(SQL_SELECT_CAMPSKILLBYID);
            //db.AddInParameter(dbSkillCommand, PARAM_CAMPAIGNID, DbType.Int32, CampaignID);

            //using (IDataReader rdr = db.ExecuteReader(dbCommand))
            //{
            //    while (rdr.Read())
            //    {
            //        BECampaignInfo objCamp = new BECampaignInfo
            //        {
            //            iCampaignID = int.Parse(rdr["CampaignID"].ToString()),
            //            sCampaignName = rdr["CampaignName"].ToString() + " " + (Convert.ToBoolean(rdr["Disabled"]) ? "(Disabled)" : ""),
            //            sCampaignDescription = rdr["Description"].ToString(),
            //            sModeIds = rdr["ModeIds"].ToString() == "" ? "" : rdr["ModeIds"].ToString(),
            //            dtEndDate = Convert.ToDateTime(rdr["EndDate"].ToString()),
            //            iProcessID = int.Parse(rdr["ProcessID"].ToString()),
            //            iTimeZoneID = int.Parse(rdr["TimeZoneID"].ToString()),
            //            bDisabled = bool.Parse(rdr["Disabled"].ToString()),
            //            bFreeField = bool.Parse(rdr["FreeField"].ToString() == "" ? "false" : rdr["FreeField"].ToString()),
            //            iCreatedBy = int.Parse(rdr["CreatedBy"].ToString()),

            //            sLocations = rdr["Location"] == DBNull.Value ? "" : rdr["Location"].ToString(),
            //            sShiftwindows = rdr["ShiftWindow"] == DBNull.Value ? "" : rdr["ShiftWindow"].ToString(),
            //            bPurposesWM = rdr["IsWorkManagement"] == DBNull.Value ? false : bool.Parse(rdr["IsWorkManagement"].ToString()),
            //            bPurposeTrans = rdr["IsTransactionMontoring"] == DBNull.Value ? false : bool.Parse(rdr["IsTransactionMontoring"].ToString()),
            //            bPurposeTime = rdr["IsTimeManagement"] == DBNull.Value ? false : bool.Parse(rdr["IsTimeManagement"].ToString()),
            //            sBusinessJustifications = rdr["BusinessJustification"] == DBNull.Value ? "" : rdr["BusinessJustification"].ToString(),
            //            sTargetq1 = rdr["TargetUsersQ1"] == DBNull.Value ? "" : rdr["TargetUsersQ1"].ToString(),
            //            sTargetq2 = rdr["TargetUsersQ2"] == DBNull.Value ? "" : rdr["TargetUsersQ2"].ToString(),
            //            sTargetq3 = rdr["TargetUsersQ3"] == DBNull.Value ? "" : rdr["TargetUsersQ3"].ToString(),
            //            sTargety1 = rdr["TargetUsersY1"] == DBNull.Value ? "" : rdr["TargetUsersY1"].ToString(),
            //            sTargety2 = rdr["TargetUsersY2"] == DBNull.Value ? "" : rdr["TargetUsersY2"].ToString(),
            //            sTargety3 = rdr["TargetUsersY3"] == DBNull.Value ? "" : rdr["TargetUsersY3"].ToString(),
            //            sKeyBenefits = rdr["KeyBenfits"] == DBNull.Value ? "" : rdr["KeyBenfits"].ToString()
            //        };

            //        objCamp.iClientID = Convert.ToInt32(rdr["ClientID"]);
            //        using (IDataReader dr = db.ExecuteReader(dbSkillCommand))
            //        {
            //            // Scroll through the results
            //            while (dr.Read())
            //            {
            //                BESkillInfo SkillItem = new BESkillInfo();
            //                SkillItem.iSkillID = Convert.ToInt32(dr["SkillID"]);
            //                SkillItem.sSkillName = dr["SkillName"].ToString();
            //                lSkillInfo.Add(SkillItem);
            //                SkillItem = null;
            //            }
            //        }
            //        objCamp.iSkillID = lSkillInfo;
            //        lCampaign.Add(objCamp);
            //        objCamp = null;
            //    }
            //}
            //return lCampaign;

        }

        ///// <summary>
        ///// Gets the campaign list.
        ///// </summary>
        ///// <param name="CampaignID">The campaign ID.</param>
        ///// <returns></returns>
        //public List<BECampaignInfo> GetCampaignList(int iCampaignID)
        //{
        //    List<BECampaignInfo> lCampaign = new List<BECampaignInfo>();
        //    List<BEParameterInfo> lParameterInfo = new List<BEParameterInfo>();
        //    List<BESkillInfo> lSkillInfo = new List<BESkillInfo>();
        //    List<BETerminationCodeInfo> lTerminationCodeInfo = new List<BETerminationCodeInfo>();

        //    
        //   Database db = DL_Shared.dbFactory(_oTenant); //DatabaseFactory.CreateDatabase(DL_Shared.Connection);
        //    DbCommand dbCommand = db.GetSqlStringCommand(SQL_SELECT_CAMPAIGNID);
        //    db.AddInParameter(dbCommand, PARAM_CAMPAIGNID, DbType.Int32, iCampaignID);

        //    DbCommand dbSkillCommand = db.GetSqlStringCommand(SQL_SELECT_CAMPSKILLBYID);
        //    db.AddInParameter(dbSkillCommand, PARAM_CAMPAIGNID, DbType.Int32, iCampaignID);

        //    using (IDataReader rdr = db.ExecuteReader(dbCommand))
        //    {
        //        while (rdr.Read())
        //        {
        //            BECampaignInfo objCamp = new BECampaignInfo {
        //                iCampaignID = int.Parse(rdr["CampaignID"].ToString()),
        //                sCampaignName = rdr["CampaignName"].ToString() + " " + (Convert.ToBoolean(rdr["Disabled"]) ? "(Disabled)" : ""),
        //                sCampaignDescription = rdr["Description"].ToString(),
        //                dtEndDate = Convert.ToDateTime(rdr["EndDate"].ToString()),
        //                iProcessID = int.Parse(rdr["ProcessID"].ToString()),
        //                iTimeZoneID = int.Parse(rdr["TimeZoneID"].ToString()),
        //                bDisabled = bool.Parse(rdr["Disabled"].ToString()),
        //                bFreeField = bool.Parse(rdr["FreeField"].ToString()),
        //                iCreatedBy = int.Parse(rdr["CreatedBy"].ToString())
        //            };
        //            objCamp.iClientID = Convert.ToInt32(rdr["ClientID"]);
        //            using (IDataReader dr = db.ExecuteReader(dbSkillCommand))
        //            {
        //                // Scroll through the results
        //                while (dr.Read())
        //                {
        //                    BESkillInfo SkillItem = new BESkillInfo();
        //                    SkillItem.iSkillID = Convert.ToInt32(dr["SkillID"]);
        //                    SkillItem.sSkillName = dr["SkillName"].ToString();
        //                    lSkillInfo.Add(SkillItem);
        //                    SkillItem = null;
        //                }
        //            }
        //            objCamp.iSkillID = lSkillInfo;
        //            lCampaign.Add(objCamp);
        //            objCamp = null;
        //        }
        //    }
        //    return lCampaign;

        //}

        #endregion

        #region Process Wise Campaign list
        /// <summary>
        /// Gets the Process Wise Campaign list.
        /// </summary>
        /// <param name="iFormID">The form ID.</param>
        /// <param name="iLoggedinUserID">The loggedin user ID.</param>
        /// <param name="iProcessID">The process ID.</param>
        /// <returns></returns>
        public List<BECampaignInfo> GetProcessWiseCampaignList(int iFormID, int iLoggedinUserID, int iProcessID)
        {
            List<BECampaignInfo> lCampaignInfo = new List<BECampaignInfo>();

            Database db = DL_Shared.dbFactory(_oTenant);

            DbCommand dbCommand = db.GetStoredProcCommand(SQL_SP_PROCESSWISECAMPAIGN);
            db.AddInParameter(dbCommand, PARAM_PROCESSID, DbType.Int32, iProcessID);
            db.AddInParameter(dbCommand, PARAM_USERID, DbType.Int32, iLoggedinUserID);
            using (IDataReader rdr = db.ExecuteReader(dbCommand))
            {
                // Scroll through the results
                while (rdr.Read())
                {

                    BECampaignInfo objCamp = new BECampaignInfo
                    {
                        iCampaignID = int.Parse(rdr["CampaignID"].ToString()),
                        sCampaignName = rdr["CampaignName"].ToString().DecodeHtmlString() + " " + (Convert.ToBoolean(rdr["Disabled"]) ? "(Disabled)" : ""),
                        sCampaignDescription = rdr["Description"].ToString().DecodeHtmlString(),
                        dtEndDate = Convert.ToDateTime(rdr["EndDate"].ToString()),
                        iProcessID = string.IsNullOrEmpty(rdr["ProcessID"].ToString()) ? 0 : int.Parse(rdr["ProcessID"].ToString()),
                        iTimeZoneID = string.IsNullOrEmpty(rdr["TimeZoneID"].ToString()) ? 0 : int.Parse(rdr["TimeZoneID"].ToString()),
                        bDisabled = string.IsNullOrEmpty(rdr["Disabled"].ToString()) ? false : bool.Parse(rdr["Disabled"].ToString()),
                        iCreatedBy = string.IsNullOrEmpty(rdr["CreatedBy"].ToString()) ? 0 : int.Parse(rdr["CreatedBy"].ToString())
                    };
                    lCampaignInfo.Add(objCamp);
                }
            }
            return lCampaignInfo;
        }

        /// <summary>
        /// Gets the Process wise Campaign 
        /// </summary>
        /// <param name="CampaignID">The Campaign ID</param>
        /// <returns></returns>
        public DataSet GetCampaignAndRoleList(int iCampaignID)
        {
            DataSet dsCampaign = new DataSet();
            string[] dTables = { "Campaign" };
            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetSqlStringCommand(SQL_SP_PROCESSWISECAMPAIGNLISTANDROLELIST);
            db.AddInParameter(dbCommand, PARAM_CAMPAIGNID, DbType.Int32, iCampaignID);
            db.LoadDataSet(dbCommand, dsCampaign, dTables);

            return dsCampaign;

        }

        /// <summary>
        /// Get Process wise Campaign List Details
        /// </summary>
        /// <param name="ProcessID">The Process ID</param>
        /// <returns></returns>
        public DataSet GetCampaignDetails(int iProcessID)
        {
            DataSet dsCampaign = new DataSet();
            string[] dTables = { "Campaign" };
            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetSqlStringCommand(SQL_SP_PROCESSWISECAMPAIGNLIST);
            db.AddInParameter(dbCommand, PARAM_PROCESSID, DbType.Int32, iProcessID);
            db.LoadDataSet(dbCommand, dsCampaign, dTables);

            return dsCampaign;

        }

        /// <summary>
        /// Get Client wise Campaign and Role List
        /// </summary>
        /// <param name="ClientID">The Client ID</param>
        /// <returns></returns>
        public DataSet GetCampaignAndRoleDetails(int iClientID)
        {
            DataSet dsCampaign = new DataSet();
            string[] dTables = { "Campaign" };
            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetStoredProcCommand(SQL_SP_ClIENTWISECAMPAIGNANDROLELIST);
            db.AddInParameter(dbCommand, PARAM_CLIENTID, DbType.Int32, iClientID);
            db.LoadDataSet(dbCommand, dsCampaign, dTables);

            return dsCampaign;

        }

        /// <summary>
        /// Get Login Name wise Campaign List
        /// </summary>
        /// <param name="LoginName">The Login Name</param>
        /// <returns></returns>
        public DataSet GetCampaignDetailsByUserID(string sLoginName)
        {
            DataSet dsCampaign = new DataSet();
            string[] dTables = { "Campaign" };
            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetSqlStringCommand(SQL_SELECT_LOGINNAMEWISECAMPAIGNLIST);
            db.AddInParameter(dbCommand, PARAM_LOGINNAME, DbType.String, sLoginName);
            db.LoadDataSet(dbCommand, dsCampaign, dTables);
            return dsCampaign;
        }

        public int GetUserId(string sLoginName)
        {
            int UserID = 0;
            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetSqlStringCommand(SQL_SELECT_USERID);
            db.AddInParameter(dbCommand, PARAM_LOGINNAME, DbType.String, sLoginName);
            using (IDataReader rdr = db.ExecuteReader(dbCommand))
            {
                while (rdr.Read())
                {
                    UserID = Convert.ToInt32(rdr["UserID"]);
                }
            }
            return UserID;
        }

        public int GetRoleId()
        {
            int RoleID = 0;
            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetSqlStringCommand(SQL_SELECT_ROLEID);
            using (IDataReader rdr = db.ExecuteReader(dbCommand))
            {
                while (rdr.Read())
                {
                    RoleID = Convert.ToInt32(rdr["RoleID"]);
                }
            }
            return RoleID;
        }

        #endregion

        #region InsertData
        /// <summary>
        /// Inserts the data.
        /// </summary>
        /// <param name="ocampaign">campaign.</param>
        public void InsertData(BECampaignInfo ocampaign)
        {
            try
            {

                Database db = DL_Shared.dbFactory(_oTenant);
                DbCommand dbCommand = db.GetStoredProcCommand(SP_INSERT_CAMPAIGN);
                db.AddInParameter(dbCommand, PARAM_CAMPAIGNNAME, DbType.String, ocampaign.sCampaignName);
                db.AddInParameter(dbCommand, PARAM_CAMPAIGNDESC, DbType.String, ocampaign.sCampaignDescription);
                if (ocampaign.dtEndDate.Year != 0001)
                {
                    db.AddInParameter(dbCommand, PARAM_EndDate, DbType.DateTime, ocampaign.dtEndDate);
                }
                else
                { db.AddInParameter(dbCommand, PARAM_EndDate, DbType.DateTime); }

                db.AddInParameter(dbCommand, PARAM_PROCESSID, DbType.Int32, ocampaign.iProcessID);
                db.AddInParameter(dbCommand, PARAM_TIMEZONEID, DbType.Int32, ocampaign.iTimeZoneID);
                db.AddInParameter(dbCommand, PARAM_DISABLED, DbType.Boolean, ocampaign.bDisabled);
                db.AddInParameter(dbCommand, PARAM_FREEFIELD, DbType.Boolean, ocampaign.bFreeField);
                db.AddInParameter(dbCommand, PARAM_CREATEDBY, DbType.Int32, ocampaign.iCreatedBy);
                db.AddInParameter(dbCommand, PARAM_MODEIDS, DbType.String, ocampaign.sModeIds);
                db.AddInParameter(dbCommand, PARAM_BILLINGSYSTEM, DbType.Boolean, ocampaign.bBillingSystem);
                db.AddInParameter(dbCommand, PARAM_EMAILS, DbType.String, ocampaign.sEmail);
                db.AddInParameter(dbCommand, PARAM_THRESHOLDFORCOMPLETION, DbType.Int32, ocampaign.iThresholdForCompletion);
                db.AddInParameter(dbCommand, PARAM_THRESHOLDFORTOOPEN, DbType.Int32, ocampaign.iThresholdForToOpen);
                db.AddInParameter(dbCommand, PARAM_TARGETEFFICIENCY, DbType.Double, ocampaign.dTargetEfficiency);

                db.AddInParameter(dbCommand, PARAM_LOCATION, DbType.String, ocampaign.sLocations);
                db.AddInParameter(dbCommand, PARAM_SHIFTWINDOWS, DbType.String, ocampaign.sShiftwindows);
                db.AddInParameter(dbCommand, PARAM_ISWORKMANAGEMENT, DbType.Boolean, ocampaign.bPurposesWM);
                db.AddInParameter(dbCommand, PARAM_ISTRASACTIONMONTORING, DbType.Boolean, ocampaign.bPurposeTrans);
                db.AddInParameter(dbCommand, PARAM_ISTIMEMANAMENT, DbType.Boolean, ocampaign.bPurposeTime);
                db.AddInParameter(dbCommand, PARAM_BUSINESSJUSTIFICATION, DbType.String, ocampaign.sBusinessJustifications);
                db.AddInParameter(dbCommand, PARAM_TRAGENTUSERSQ1, DbType.String, ocampaign.sTargetq1);
                db.AddInParameter(dbCommand, PARAM_TRAGENTUSERSQ2, DbType.String, ocampaign.sTargetq2);
                db.AddInParameter(dbCommand, PARAM_TRAGENTUSERSQ3, DbType.String, ocampaign.sTargetq3);
                db.AddInParameter(dbCommand, PARAM_TRAGENTUSERSY1, DbType.String, ocampaign.sTargety1);
                db.AddInParameter(dbCommand, PARAM_TRAGENTUSERSY2, DbType.String, ocampaign.sTargety2);
                db.AddInParameter(dbCommand, PARAM_TRAGENTUSERSY3, DbType.String, ocampaign.sTargety3);
                db.AddInParameter(dbCommand, PARAM_KEYBENFITS, DbType.String, ocampaign.sKeyBenefits);
                db.AddInParameter(dbCommand, PARAM_BUSINESSAPPROVERID, DbType.String, ocampaign.iBuisnessID);
                db.AddInParameter(dbCommand, PARAM_TECHNOLOGYAPPROVERID, DbType.String, ocampaign.iTechID);
                db.AddInParameter(dbCommand, PARAM_STATUS, DbType.String, ocampaign.sStatus);
                db.AddInParameter(dbCommand, PARAM_APPROVALID, DbType.Int32, ocampaign.iApprovalId);



                DbCommand dbMaxCampaignID = db.GetSqlStringCommand(SQL_SELECT_MAXCAMPAIGN);

                DbCommand dbInsertCSkillCommand = db.GetSqlStringCommand(SQL_INSERT_CAMPAIGNSKILL);
                db.AddInParameter(dbInsertCSkillCommand, PARAM_CAMPAIGNID, DbType.Int32);
                db.AddInParameter(dbInsertCSkillCommand, PARAM_SKILLID, DbType.Int32);
                db.AddInParameter(dbInsertCSkillCommand, PARAM_CREATEDBY, DbType.Int32, ocampaign.iCreatedBy);


                //*************************************
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
                //if (ex.Message.Contains(BPA.GlobalResources.DataLayer.Resources.msg_CampaignAlready))
                //{
                //   // throw new ExceptionHandler.ExceptionType.DataAccessCustomException(BPA.GlobalResources.DataLayer.Resources.msg_CampaignAlready);
                //}
                throw;// new ExceptionHandler.ExceptionType.DataAccessException(ex.Message);
            }
            catch (Exception ex)
            {
                throw;// new ExceptionHandler.ExceptionType.DataAccessException(ex.Message);
            }


        }

        #endregion

        #region UpdateData
        /// <summary>
        /// Updates the data.
        /// </summary>
        /// <param name="ocampaign">campaign.</param>
        public void UpdateData(BECampaignInfo ocampaign)
        {
            try
            {
                Database db = DL_Shared.dbFactory(_oTenant);
                // DbCommand dbUpdateCommand = db.GetSqlStringCommand(SQL_UPDATE_CAMPAIGN);
                DbCommand dbUpdateCommand = db.GetStoredProcCommand(SP_UPDATE_CAMPAIGN);
                db.AddInParameter(dbUpdateCommand, PARAM_CAMPAIGNID, DbType.Int32, ocampaign.iCampaignID);
                db.AddInParameter(dbUpdateCommand, PARAM_CAMPAIGNNAME, DbType.String, ocampaign.sCampaignName);
                db.AddInParameter(dbUpdateCommand, PARAM_CAMPAIGNDESC, DbType.String, ocampaign.sCampaignDescription);
                if (ocampaign.dtEndDate.Year != 0001)
                {
                    db.AddInParameter(dbUpdateCommand, PARAM_EndDate, DbType.DateTime, ocampaign.dtEndDate);
                }
                else
                { db.AddInParameter(dbUpdateCommand, PARAM_EndDate, DbType.DateTime); }

                db.AddInParameter(dbUpdateCommand, PARAM_PROCESSID, DbType.Int32, ocampaign.iProcessID);
                db.AddInParameter(dbUpdateCommand, PARAM_TIMEZONEID, DbType.Int32, ocampaign.iTimeZoneID);
                db.AddInParameter(dbUpdateCommand, PARAM_BILLINGSYSTEM, DbType.Boolean, ocampaign.bBillingSystem);
                db.AddInParameter(dbUpdateCommand, PARAM_DISABLED, DbType.Boolean, ocampaign.bDisabled);
                db.AddInParameter(dbUpdateCommand, PARAM_FREEFIELD, DbType.Boolean, ocampaign.bFreeField);
                db.AddInParameter(dbUpdateCommand, PARAM_MODEIDS, DbType.String, ocampaign.sModeIds);
                db.AddInParameter(dbUpdateCommand, PARAM_MODIFIEDBY, DbType.Int32, ocampaign.iCreatedBy);
                db.AddInParameter(dbUpdateCommand, PARAM_THRESHOLDFORCOMPLETION, DbType.Int32, ocampaign.iThresholdForCompletion);
                db.AddInParameter(dbUpdateCommand, PARAM_THRESHOLDFORTOOPEN, DbType.Int32, ocampaign.iThresholdForToOpen);
                db.AddInParameter(dbUpdateCommand, PARAM_TARGETEFFICIENCY, DbType.Double, ocampaign.dTargetEfficiency);
                db.AddInParameter(dbUpdateCommand, PARAM_EMAILS, DbType.String, ocampaign.sEmail);

                db.AddInParameter(dbUpdateCommand, PARAM_LOCATION, DbType.String, ocampaign.sLocations);
                db.AddInParameter(dbUpdateCommand, PARAM_SHIFTWINDOWS, DbType.String, ocampaign.sShiftwindows);
                db.AddInParameter(dbUpdateCommand, PARAM_ISWORKMANAGEMENT, DbType.Boolean, ocampaign.bPurposesWM);
                db.AddInParameter(dbUpdateCommand, PARAM_ISTRASACTIONMONTORING, DbType.Boolean, ocampaign.bPurposeTime);
                db.AddInParameter(dbUpdateCommand, PARAM_ISTIMEMANAMENT, DbType.Boolean, ocampaign.bPurposeTrans);
                db.AddInParameter(dbUpdateCommand, PARAM_BUSINESSJUSTIFICATION, DbType.String, ocampaign.sBusinessJustifications);
                db.AddInParameter(dbUpdateCommand, PARAM_TRAGENTUSERSQ1, DbType.String, ocampaign.sTargetq1);
                db.AddInParameter(dbUpdateCommand, PARAM_TRAGENTUSERSQ2, DbType.String, ocampaign.sTargetq2);
                db.AddInParameter(dbUpdateCommand, PARAM_TRAGENTUSERSQ3, DbType.String, ocampaign.sTargetq3);
                db.AddInParameter(dbUpdateCommand, PARAM_TRAGENTUSERSY1, DbType.String, ocampaign.sTargety1);
                db.AddInParameter(dbUpdateCommand, PARAM_TRAGENTUSERSY2, DbType.String, ocampaign.sTargety2);
                db.AddInParameter(dbUpdateCommand, PARAM_TRAGENTUSERSY3, DbType.String, ocampaign.sTargety3);
                db.AddInParameter(dbUpdateCommand, PARAM_KEYBENFITS, DbType.String, ocampaign.sKeyBenefits);


                //*************************************
                DbCommand dbInsertCSkillCommand = db.GetSqlStringCommand(SQL_INSERT_CAMPAIGNSKILL);
                db.AddInParameter(dbInsertCSkillCommand, PARAM_CAMPAIGNID, DbType.Int32, ocampaign.iCampaignID);
                db.AddInParameter(dbInsertCSkillCommand, PARAM_SKILLID, DbType.Int32);
                db.AddInParameter(dbInsertCSkillCommand, PARAM_CREATEDBY, DbType.Int32, ocampaign.iCreatedBy);

                ////*********************************************************************************
                DbCommand dbCheckSkillCommand = db.GetSqlStringCommand(SQL_SELECT_CHECKSKILL);
                db.AddInParameter(dbCheckSkillCommand, PARAM_CAMPAIGNID, DbType.Int32, ocampaign.iCampaignID);
                db.AddInParameter(dbCheckSkillCommand, PARAM_SKILLID, DbType.Int32);

                DbCommand dbInactivateSkillCommand = db.GetSqlStringCommand(SQL_UPDATE_INACTIVATESKILL);
                db.AddInParameter(dbInactivateSkillCommand, PARAM_CAMPAIGNID, DbType.Int32, ocampaign.iCampaignID);

                DbCommand dbActivateSkillCommand = db.GetSqlStringCommand(SQL_UPDATE_ACTIVATESKILL);
                db.AddInParameter(dbActivateSkillCommand, PARAM_CAMPAIGNID, DbType.Int32, ocampaign.iCampaignID);
                db.AddInParameter(dbActivateSkillCommand, PARAM_SKILLID, DbType.Int32);

                //*********************************************************************************
                using (DbConnection conn = db.CreateConnection())
                {
                    conn.Open();// Connection Open
                    using (DbTransaction trans = conn.BeginTransaction()) //Start Transaction
                    {
                        try
                        {
                            db.ExecuteNonQuery(dbUpdateCommand, trans);
                            int MemberCount = ocampaign.iSkillID.Count;
                            db.ExecuteNonQuery(dbInactivateSkillCommand, trans);

                            for (int i = 0; i < MemberCount; i++)
                            {
                                db.SetParameterValue(dbCheckSkillCommand, PARAM_SKILLID, ocampaign.iSkillID[i].iSkillID);
                                object oRowCounter = db.ExecuteScalar(dbCheckSkillCommand, trans);
                                if (oRowCounter == null)
                                {
                                    db.SetParameterValue(dbInsertCSkillCommand, PARAM_SKILLID, ocampaign.iSkillID[i].iSkillID);
                                    db.ExecuteNonQuery(dbInsertCSkillCommand, trans);
                                }
                                else
                                {
                                    db.SetParameterValue(dbActivateSkillCommand, PARAM_SKILLID, ocampaign.iSkillID[i].iSkillID);
                                    db.ExecuteNonQuery(dbActivateSkillCommand, trans);
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
                  //  throw new ExceptionHandler.ExceptionType.DataAccessCustomException(BPA.GlobalResources.DataLayer.Resources.UniqueConstraints);
                }
                throw;// new ExceptionHandler.ExceptionType.DataAccessException(ex.Message);
            }
            catch (Exception ex)
            {
                throw;// new ExceptionHandler.ExceptionType.DataAccessException(ex.Message);
            }

        }
        #endregion

        #region DeleteData
        /// <summary>
        /// Deletes the data.
        /// </summary>
        /// <param name="ocampaign">The ocampaign.</param>
        public void DeleteData(BECampaignInfo ocampaign)
        {
            try
            {
                //*************************************
                Database db = DL_Shared.dbFactory(_oTenant);

                DbCommand dbDeleteCampCommand = db.GetSqlStringCommand(SQL_DELETE_CAMPAIGN);
                db.AddInParameter(dbDeleteCampCommand, PARAM_CAMPAIGNID, DbType.Int32, ocampaign.iCampaignID);

                //DbCommand dbDeleteCampParameterMapCommand = db.GetSqlStringCommand(SQL_DELETE_CAMPPARAMETERMAP);
                //db.AddInParameter(dbDeleteCampParameterMapCommand, PARAM_CAMPAIGNID, DbType.Int32, ocampaign.iCampaignID);

                DbCommand dbDeleteCampSkillMapCommand = db.GetSqlStringCommand(SQL_DELETE_CAMPSKILLMAP);
                db.AddInParameter(dbDeleteCampSkillMapCommand, PARAM_CAMPAIGNID, DbType.Int32, ocampaign.iCampaignID);

                using (DbConnection conn = db.CreateConnection())
                {
                    conn.Open();// Connection Open

                    using (DbTransaction trans = conn.BeginTransaction()) //Start Transaction
                    {
                        try
                        {
                            db.ExecuteNonQuery(dbDeleteCampSkillMapCommand, trans);
                            db.ExecuteNonQuery(dbDeleteCampCommand, trans);
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

        /// <summary>
        /// Gets the user approver list.
        /// </summary>
        /// <param name="UserId">The user id.</param>
        /// <param name="ClientId">The client id.</param>
        /// <param name="ProcessId">The process id.</param>
        /// <param name="Flag">The flag.</param>
        /// <returns></returns>
        public List<BEUserInfo> GetUserApproverListByProcess(int UserId, int iFormId, int ProcessId)
        {
            List<BEUserInfo> lApprovelInfo = new List<BEUserInfo>();
            DataSet ds = new DataSet();
            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetStoredProcCommand(SQL_USP_GETAPPROVERLIST);
            db.AddInParameter(dbCommand, PARAM_USERID, DbType.Int32, UserId);
            db.AddInParameter(dbCommand, PARAM_FORMID, DbType.Int32, iFormId);
            db.AddInParameter(dbCommand, PARAM_PROCESSID, DbType.Int32, ProcessId);
            db.LoadDataSet(dbCommand, ds, "Approvers");
            foreach (DataRow dr in ds.Tables["Approvers"].Rows)
            {
                BEUserInfo objCamp = new BEUserInfo
                {
                    iUserID = Convert.ToInt32(dr["UserID"].ToString()),
                    sUserName = dr["UserName"].ToString()
                };
                lApprovelInfo.Add(objCamp);
                objCamp = null;

            }
            return lApprovelInfo;
        }



        /// <summary>
        /// Gets the campaign list.
        /// </summary>
        /// <returns></returns>
        public List<BECampaignInfo> GetCampaignList()
        {
            List<BECampaignInfo> lCampaign = new List<BECampaignInfo>();

            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetSqlStringCommand(SQL_SELECT_ALLCAMPAIGN);

            using (IDataReader rdr = db.ExecuteReader(dbCommand))
            {
                while (rdr.Read())
                {
                    BECampaignInfo objCamp = new BECampaignInfo
                    {
                        iCampaignID = int.Parse(rdr["CampaignID"].ToString()),
                        sCampaignName = rdr["CampaignName"].ToString().DecodeHtmlString() + " " + (Convert.ToBoolean(rdr["Disabled"]) ? "(Disabled)" : ""),
                        sCampaignDescription = rdr["Description"].ToString().DecodeHtmlString(),
                        dtEndDate = Convert.ToDateTime(rdr["EndDate"].ToString()),
                        iProcessID = string.IsNullOrEmpty(rdr["ProcessID"].ToString()) ? 0 : int.Parse(rdr["ProcessID"].ToString()),
                        iTimeZoneID = string.IsNullOrEmpty(rdr["TimeZoneID"].ToString()) ? 0 : int.Parse(rdr["TimeZoneID"].ToString()),
                        bDisabled = string.IsNullOrEmpty(rdr["Disabled"].ToString()) ? false : bool.Parse(rdr["Disabled"].ToString()),
                        iCreatedBy = string.IsNullOrEmpty(rdr["CreatedBy"].ToString()) ? 0 : int.Parse(rdr["CreatedBy"].ToString())
                    };
                    lCampaign.Add(objCamp);
                    objCamp = null;
                }
            }
            return lCampaign;
        }

        #region Campaign Approval
        /// <summary>
        /// To get pending campaign approval
        /// </summary>
        /// <param name="iUserId"></param>
        /// <param name="sFromDate"></param>
        /// <param name="sToDate"></param>
        /// <returns></returns>
        public DataTable GetPandingCampaignApproval(int iUserId, DateTime sFromDate, DateTime sToDate)
        {
            DataSet ds = new DataSet();
            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetStoredProcCommand(SQL_SELECT_PANDINGCAMPAIGNAPPROVAL);
            db.AddInParameter(dbCommand, PARAM_USERID, DbType.Int32, iUserId);
            db.AddInParameter(dbCommand, PARAM_STARTDATE, DbType.DateTime, sFromDate);
            db.AddInParameter(dbCommand, PARAM_ENDDATE, DbType.DateTime, sToDate);
            db.LoadDataSet(dbCommand, ds, "PandingApproval");
            return ds.Tables[0];
        }

        /// <summary>
        /// Fetch Campaign Request Details
        /// </summary>
        /// <returns></returns>
        public DataTable FetchCampaignRequestDetails(int iApprovalId)
        {
            DataSet ds = new DataSet();
            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand;
            dbCommand = db.GetStoredProcCommand(SP_FETCH_CAMPAIGN_REQUEST_DETAILS);
            db.AddInParameter(dbCommand, PARAM_APPROVALID, DbType.Int32, iApprovalId);
            db.LoadDataSet(dbCommand, ds, "CampaignRequestDetails");
            return ds.Tables[0];
        }


        /// <summary>
        /// Insert campaign request change
        /// </summary>
        /// <param name="iApprovalId"></param>
        /// <param name="iUserId"></param>
        /// <param name="iUserLevel"></param>
        /// <param name="sChangeRequest"></param>
        public void InsertCampaignRequestChange(int iApprovalId, int iUserId, int iUserLevel, string sChangeRequest)
        {
            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetStoredProcCommand(SP_INSERT_CAMPAIGNCHNAGEREQUEST);
            db.AddInParameter(dbCommand, PARAM_APPROVALID, DbType.Int32, iApprovalId);
            db.AddInParameter(dbCommand, PARAM_USERID, DbType.Int32, iUserId);
            db.AddInParameter(dbCommand, PARAM_USERLEVEL, DbType.Int32, iUserLevel);
            db.AddInParameter(dbCommand, PARAM_CHANGEREQUEST, DbType.String, sChangeRequest);
            db.ExecuteNonQuery(dbCommand);
        }
        /// <summary>
        /// update campaign change request
        /// </summary>
        /// <param name="ocampaign"></param>
        public void UpdateCampaignRequestChange(BECampaignInfo ocampaign)
        {
            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbUpdateCommand = db.GetStoredProcCommand(SP_INSERT_CAMPAIGNCHNAGEREQUEST);
            db.AddInParameter(dbUpdateCommand, PARAM_LOCATION, DbType.String, ocampaign.sLocations);
            db.AddInParameter(dbUpdateCommand, PARAM_SHIFTWINDOWS, DbType.String, ocampaign.sShiftwindows);
            db.AddInParameter(dbUpdateCommand, PARAM_ISWORKMANAGEMENT, DbType.Boolean, ocampaign.bPurposesWM);
            db.AddInParameter(dbUpdateCommand, PARAM_ISTRASACTIONMONTORING, DbType.Boolean, ocampaign.bPurposeTrans);
            db.AddInParameter(dbUpdateCommand, PARAM_ISTIMEMANAMENT, DbType.Boolean, ocampaign.bPurposeTime);
            db.AddInParameter(dbUpdateCommand, PARAM_BUSINESSJUSTIFICATION, DbType.String, ocampaign.sBusinessJustifications);
            db.AddInParameter(dbUpdateCommand, PARAM_TRAGENTUSERSQ1, DbType.String, ocampaign.sTargetq1);
            db.AddInParameter(dbUpdateCommand, PARAM_TRAGENTUSERSQ2, DbType.String, ocampaign.sTargetq2);
            db.AddInParameter(dbUpdateCommand, PARAM_TRAGENTUSERSQ3, DbType.String, ocampaign.sTargetq3);
            db.AddInParameter(dbUpdateCommand, PARAM_TRAGENTUSERSY1, DbType.String, ocampaign.sTargety1);
            db.AddInParameter(dbUpdateCommand, PARAM_TRAGENTUSERSY2, DbType.String, ocampaign.sTargety2);
            db.AddInParameter(dbUpdateCommand, PARAM_TRAGENTUSERSY3, DbType.String, ocampaign.sTargety3);
            db.AddInParameter(dbUpdateCommand, PARAM_KEYBENFITS, DbType.String, ocampaign.sKeyBenefits);
            db.AddInParameter(dbUpdateCommand, PARAM_APPROVALID, DbType.Int32, ocampaign.iApprovalId);
            db.AddInParameter(dbUpdateCommand, PARAM_USERID, DbType.Int32, ocampaign.iModifiedBy);
            db.AddInParameter(dbUpdateCommand, PARAM_USERLEVEL, DbType.Int32, ocampaign.iModeId);
            db.ExecuteNonQuery(dbUpdateCommand);
        }



        #endregion

    }
}
