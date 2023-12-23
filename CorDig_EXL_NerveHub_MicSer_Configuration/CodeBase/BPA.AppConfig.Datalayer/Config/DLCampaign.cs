using BPA.AppConfig.BusinessEntity.Application;
using BPA.AppConfig.BusinessEntity.Security;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BPA.AppConfig.BusinessEntity.Config;
using BPA.Utility;
using System.Data.SqlClient;

namespace BPA.AppConfig.Datalayer.Config
{
    public class DLCampaign : IDisposable
    {
        private BETenant _oTenant = null;

        public DLCampaign(BETenant oTenant)
        {
            _oTenant = oTenant;
        }

        public void Dispose()
        {
            _oTenant = null;
        }

        private const string SQL_USP_GETAPPROVERLIST = @"[Config].[Usp_GetApproverList]";
        private const string SQL_SP_CAMPAIGN = @"[WM].[Usp_GetCampaignList]";
        private const string SP_INSERT_CAMPAIGN = @"[Config].[USP_InsertCampaignMasterDetail]";
        private const string SQL_SELECT_MAXCAMPAIGN = @"SELECT MAX(CampaignID) FROM Config.tblCampaignMaster (NOLOCK)";
        private const string SQL_INSERT_CAMPAIGNSKILL = @"INSERT INTO Config.tblCampaignSkillMap(CampaignID, SkillID, CreatedBy) values(@CampaignID, @SkillID, @CreatedBy)";
        private const string SP_UPDATE_CAMPAIGN = @"[Config].[USP_UpdateCampaignMasterDetail]";
        private const string SQL_SELECT_CHECKSKILL = @"SELECT * FROM Config.tblCampaignSkillMap (NOLOCK) Where CampaignID=@CampaignID and SkillID =@SkillID";
        private const string SQL_UPDATE_INACTIVATESKILL = @"UPDATE Config.tblCampaignSkillMap SET Disabled=1 WHERE CampaignID=@CampaignID";
        private const string SQL_UPDATE_ACTIVATESKILL = @"UPDATE Config.tblCampaignSkillMap SET Disabled=0 Where CampaignID=@CampaignID and SkillID =@SkillID";
        private const string SQL_SELECT_PANDINGCAMPAIGNAPPROVAL = @"[Config].[USP_GetPendingCampaignApproval]";
        private const string SP_FETCH_CAMPAIGN_REQUEST_DETAILS = @"[Config].[USP_FetchCampaignRequestDetails]";
        private const string SP_INSERT_CAMPAIGNCHNAGEREQUEST = @"[Config].[USP_InsertCampaignChangeRequest]";
        private const string SP_SELECT_CAMPAIGN = @"[Config].[USP_GetCampaignDetail]";
        private const string SQL_SELECT_CAMPSKILLBYID = @"select Skill.SkillID, Skill.SkillName 
                                                        from Config.tblCampaignMaster CampMaster(NOLOCK)
                                                        inner join Config.tblCampaignSkillMap CampSkillMap (NOLOCK)
	                                                        on CampMaster.CampaignID=CampSkillMap.CampaignID 
                                                        INNER JOIN Config.tblSkillMaster Skill (NOLOCK) 
	                                                        on Skill.SkillID=CampSkillMap.SkillID 
                                                        where CampMaster.CampaignID=@CampaignID and CampSkillMap.Disabled=0";
        private const string SQL_SP_PROCESSWISECAMPAIGN = @"dbo.Usp_CDS_GetProcessWiseCampaignList";

        private const string PARAM_USERID = "@UserID";
        private const string PARAM_PROCESSID = "@ProcessID";
        private const string PARAM_AGENTID = "@AgentID";
        private const string PARAM_FLAG = "@Flag";
        private const string PARAM_FORMID = "@FormID";
        private const string PARAM_CAMPAIGNID = "@CampaignID";
        private const string PARAM_CAMPAIGNNAME = "@CampaignName";
        private const string PARAM_CAMPAIGNDESC = "@CampaignDescription";
        private const string PARAM_EndDate = "@EndDate";
        private const string PARAM_DASHBOARDREQUEST = "@DashboardRequest";
        private const string PARAM_ACTIVECAMPAIGNLIST = "@ACTIVECAMPAIGNLIST";
        private const string PARAM_TIMEZONEID = "@TimeZoneID";
        private const string PARAM_DISABLED = "@Disabled";

        private const string PARAM_FREEFIELD = "@FreeField";

        private const string PARAM_CREATEDBY = "@CreatedBy";
        private const string PARAM_MODIFIEDBY = "@ModifiedBy";
        private const string PARAM_SKILLID = "@SkillID";
        private const string PARAM_CLIENTID = "@ClientID";
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
                if (ex.Message.Contains(BPA.GlobalResources.DataLayer.Resources.msg_CampaignAlready))
                {
                    throw new ExceptionHandler.ExceptionType.DataAccessCustomException(BPA.GlobalResources.DataLayer.Resources.msg_CampaignAlready);
                }
                throw new ExceptionHandler.ExceptionType.DataAccessException(ex.Message);
            }*/
            catch (Exception ex)
            {
                throw;// new ExceptionHandler.ExceptionType.DataAccessException(ex.Message);
            }
        }

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
        }

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
    }
}
