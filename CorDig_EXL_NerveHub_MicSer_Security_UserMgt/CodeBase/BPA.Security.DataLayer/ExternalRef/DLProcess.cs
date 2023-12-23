using BPA.Security.BusinessEntity;
using BPA.Security.BusinessEntity.ExtrernalRefre;
using BPA.Security.Datalayer;
using BPA.Security.DataLayer.ExternalRef.Utitlity;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPA.Security.DataLayer.ExternalRef
{
    /// <summary>
    ///  DL Process
    /// </summary>
    public class DLProcess : IDisposable
    {
        #region Field Process
        private BETenant _oTenant = null;
        private const string SQL_SELECT_CLIENTDETAILS = @"select PM.description,PM.Disabled, PM.ProcessName, PM.ProcessId,CM.ClientName,CM.ClientID from Config.tblProcessMaster(nolock) PM ,Config.tblClientMaster CM where PM.processid=@ProcessID and PM.ClientId=CM.ClientId";
        private const string SQL_SELECT_PROCESSSTASTAGE = @"select * from Config.tblMasterTable where FieldId=@FieldID";
        private const string SQL_SP_PROCESS = @"[Config].[Usp_GetProcessList]";
        private const string SQL_SP_PROCESSACCESSLIST = @"[Config].[Usp_GetAccessList]";

        private const string SQL_SP_OVERRATINGPROCESS = @"Usp_CDS_GetOverRatingProcessList";
        private const string SQL_SP_HEALTHPROCESS = @"Usp_CDS_GetHealthProcessList";


        private const string SQL_SP_PROCESSBYCLIENT = @"Usp_CDS_GetClientWiseProcessList";
        //added by wasim to get Multi client process
        //Dead Code: Unused Field
        //private const string SQL_SP_PROCESSBYMULTICLIENT = @"Usp_CDS_GetMultiClientWiseProcessList";

        private const string SQL_SP_PROCESSBYCLIENTLIST = @"Usp_CDS_GetClientWiseProcessList";
        private const string SQL_SELECT_PROCESSID = @"BEGIN SELECT CM.NAME,PM.PROCESSID,PM.CLIENTID,PM.PROCESSNAME, PM.DESCRIPTION,PM.CALENDARID,
                                                    PM.PROCESSTYPE,PM.PROCESSWORKTYPE,PM.CLIENTSBUID,PM.SCOPE,PM.PILOTSTARTDATE,PM.PILOTENDDATE,
                                                    PM.GOLIVEDATE,PM.DISABLED, STABILIZATIONSTARTDATE,STABILIZATIONENDDATE, PRODUCTIONSTARTDATE,
                                                    PRODUCTIONENDDATE,SUPERVISORFEEDBACKTARGETFREQUENCY,SUPERVISORFEEBACKTRAGETPERWEEK, QCAFEEBACKTRAGETPERWEEK,
                                                    TARGETAUDITPERMONTH, TARGETQCAHRS, PROCESSCOMPLEXITY,SM.NAME AS 'SBUNAME', MT.VALUE AS 'PROCESSCOMPLEXITYTEXT',MT1.VALUE AS 'PROCESSWORKTYPETEXT',isnull(CAPTYPE,0) CAPTYPE
                                                    FROM Config.tblProcessMaster (NOLOCK) PM
                                                    left JOIN Config.tblClientSBUMap (NOLOCK) CLSBU ON CLSBU.CLIENTSBUID=PM.CLIENTSBUID
                                                    LEFT OUTER JOIN Config.tblCalenderMaster (NOLOCK) CM ON PM.CALENDARID=CM.CALENDERID
                                                    LEFT OUTER JOIN Config.tblSBUMaster (NOLOCK) SM ON CLSBU.SBUID=SM.SBUID
                                                    LEFT OUTER JOIN Config.tblMasterTable (NOLOCK) MT ON PM.PROCESSCOMPLEXITY=MT.MASTERID AND MT.FIELDID=15
                                                    LEFT OUTER JOIN Config.tblMasterTable (NOLOCK) MT1 ON PM.PROCESSWORKTYPE=MT1.MASTERID AND MT1.FIELDID=48
                                                    WHERE PROCESSID= @PROCESSID

                                                    SELECT PROCESSSLAID,PROCESSID,STARTDATE,ENDDATE,STAGE,FILEDATA,
                                                    FILENAME FROM [Config].[tblProcessSLA] WHERE PROCESSID = @PROCESSID

                                                    SELECT PG.PROCESSGROUPID,EP.ERPPROCESSID,EP.NAME,isnull(EP.ERPCODE,0) as ERPCODE,EP.LOCATIONID
                                                    FROM [Config].[tblProcessGroup] PG INNER JOIN Config.tblERPProcess (NOLOCK) EP
                                                    ON EP.ERPPROCESSID=PG.ERPPROCESSID WHERE PROCESSID = @PROCESSID and PG.Disabled=0

                                                    SELECT PF.PROCESSFTEID,PF.LOCATIONID,LOCATIONNAME,PF.FTE,PF.QCACOUNT,PF.EFFECTIVESTARTDATE,PF.EFFECTIVEENDDATE
                                                    FROM Config.tblProcessFTE PF INNER JOIN Config.tblLocationMaster (NOLOCK) LM
                                                    ON PF.LOCATIONID=LM.LOCATIONID WHERE PROCESSID = @PROCESSID and PF.Disabled=0
                                                    END";
        private const string SQL_SELECT_PAS_PROCESSID = @"[Config].[USP_GetProcess_PASData]";
        //private const string SQL_SELECT_PAS_PROCESSID = @"BEGIN SELECT CM.NAME,PM.PROCESSID,PM.CLIENTID,PM.PROCESSNAME, PM.DESCRIPTION,PM.CALENDARID,
        //                                            PM.PROCESSTYPE,PM.PROCESSWORKTYPE,PM.CLIENTSBUID,PM.SCOPE,PM.PILOTSTARTDATE,PM.PILOTENDDATE,
        //                                            PM.GOLIVEDATE,PM.DISABLED, STABILIZATIONSTARTDATE,STABILIZATIONENDDATE, PRODUCTIONSTARTDATE,
        //                                            PRODUCTIONENDDATE,SUPERVISORFEEDBACKTARGETFREQUENCY,SUPERVISORFEEBACKTRAGETPERWEEK, QCAFEEBACKTRAGETPERWEEK,
        //                                            TARGETAUDITPERMONTH, TARGETQCAHRS, PROCESSCOMPLEXITY,SM.NAME AS 'SBUNAME', MT.VALUE AS 'PROCESSCOMPLEXITYTEXT', MT1.VALUE AS 'PROCESSWORKTYPETEXT', isnull(CAPTYPE, 0) CAPTYPE
        //                                               FROM Config.tblProcessMaster(NOLOCK) PM
        //                                            left JOIN Config.tblClientSBUMap(NOLOCK) CLSBU ON CLSBU.CLIENTSBUID=PM.CLIENTSBUID
        //                                           LEFT OUTER JOIN Config.tblCalenderMaster (NOLOCK) CM ON PM.CALENDARID= CM.CALENDERID

        //                                           LEFT OUTER JOIN Config.tblSBUMaster (NOLOCK) SM ON CLSBU.SBUID= SM.SBUID

        //                                           LEFT OUTER JOIN Config.tblMasterTable (NOLOCK) MT ON PM.PROCESSCOMPLEXITY= MT.MASTERID AND MT.FIELDID= 15

        //                                           LEFT OUTER JOIN Config.tblMasterTable (NOLOCK) MT1 ON PM.PROCESSWORKTYPE= MT1.MASTERID AND MT1.FIELDID= 48

        //                                           WHERE PROCESSID = @PROCESSID


        //                                           SELECT PROCESSSLAID, PROCESSID, STARTDATE, ENDDATE, STAGE, FILEDATA,
        //                                           FILENAME FROM [Config].[tblProcessSLA] WHERE PROCESSID = @PROCESSID


        //                                           SELECT PG.PROCESSGROUPID, EP.ERPPROCESSID, EP.NAME, isnull(EP.ERPCODE,0) as ERPCODE,EP.LOCATIONID
        //                                              FROM[Config].[tblProcessGroup] PG INNER JOIN Config.tblERPProcess(NOLOCK) EP
        //                                            ON EP.ERPPROCESSID=PG.ERPPROCESSID WHERE PROCESSID = @PROCESSID and PG.Disabled=0

        //                                            SELECT PF.PROCESSFTEID, PF.LOCATIONID, LOCATIONNAME, PF.FTE, PF.QCACOUNT, PF.EFFECTIVESTARTDATE, PF.EFFECTIVEENDDATE
        //                                            FROM Config.tblProcessFTE PF INNER JOIN Config.tblLocationMaster (NOLOCK) LM
        //                                            ON PF.LOCATIONID= LM.LOCATIONID WHERE PROCESSID = @PROCESSID and PF.Disabled= 0


        //                                            SELECT PASProcessDate,PASProcessType from Config.tblProcess_DateMaster where PROCESSID = @PROCESSID and Disabled = 0
        //                                            END";

        private const string SQL_SELECT_ROLENAME = @"SELECT ROLEID,ROLENAME FROM Config.tblRoleMaster WHERE DISABLED=0";
        private const string SQL_INSERT_PROCESS = @"Begin INSERT INTO Config.tblProcessMaster (CLIENTID,PROCESSNAME,DESCRIPTION,CALENDARID,PROCESSTYPE,PROCESSWORKTYPE,ClientSBUID,SCOPE,PilotStartDate,PilotEndDate,GOLIVEDATE,DISABLED,CREATEDBY, STABILIZATIONSTARTDATE,  STABILIZATIONENDDATE, PRODUCTIONSTARTDATE, PRODUCTIONENDDATE,SUPERVISORFEEDBACKTARGETFREQUENCY, SUPERVISORFEEBACKTRAGETPERWEEK, QCAFEEBACKTRAGETPERWEEK, TARGETAUDITPERMONTH, TARGETQCAHRS, PROCESSCOMPLEXITY,CAPTYPE) VALUES(@CLIENTID,@PROCESSNAME,@DESCRIPTION,@CALENDARID,@PROCESSTYPE,@PROCESSWORKTYPE,@SBUID,@SCOPE,@PilotStartdate,@PilotEnddate,@GOLIVEDATE,@DISABLED,@CREATEDBY, @STABILIZATIONSTARTDATE,  @STABILIZATIONENDDATE, @PRODUCTIONSTARTDATE, @PRODUCTIONENDDATE,@SUPERVISORFEEDBACKTARGETFREQUENCY, @SUPERVISORFEEBACKTRAGETPERWEEK, @QCAFEEBACKTRAGETPERWEEK, @TARGETAUDITPERMONTH, @TARGETQCAHRS, @PROCESSCOMPLEXITY,@CAPTYPE) SELECT SCOPE_IDENTITY() AS PROCESSID END";
        private const string SQL_UPDATE_PROCESS = @"UPDATE Config.tblProcessMaster SET CLIENTID=@CLIENTID,PROCESSNAME=@PROCESSNAME,DESCRIPTION=@DESCRIPTION,CALENDARID=@CALENDARID,
                                                    PROCESSTYPE=@PROCESSTYPE,PROCESSWORKTYPE=@PROCESSWORKTYPE,CLIENTSBUID=@SBUID,
                                                    SCOPE=@SCOPE,PilotStartDate=@PilotStartDate,PilotEndDate=@PilotEndDate,
                                                    GOLIVEDATE=@GOLIVEDATE,DISABLED=@DISABLED,MODIFIEDBY=@MODIFIEDBY, MODIFIEDON=GetDate() ,
                                                    STABILIZATIONSTARTDATE=@STABILIZATIONSTARTDATE,  STABILIZATIONENDDATE=@STABILIZATIONENDDATE, PRODUCTIONSTARTDATE=@PRODUCTIONSTARTDATE,
                                                    PRODUCTIONENDDATE=@PRODUCTIONENDDATE,SUPERVISORFEEDBACKTARGETFREQUENCY=@SUPERVISORFEEDBACKTARGETFREQUENCY, SUPERVISORFEEBACKTRAGETPERWEEK=@SUPERVISORFEEBACKTRAGETPERWEEK,
                                                    QCAFEEBACKTRAGETPERWEEK=@QCAFEEBACKTRAGETPERWEEK, TARGETAUDITPERMONTH=@TARGETAUDITPERMONTH, TARGETQCAHRS=@TARGETQCAHRS,
                                                    PROCESSCOMPLEXITY=@PROCESSCOMPLEXITY,CAPTYPE=@CAPTYPE WHERE PROCESSID=@PROCESSID";
        private const string SQL_DELETE_PROCESS = @"Delete from Config.tblSubProcess WHERE PROCESSID=@PROCESSID
                                                    DELETE Config.tblProcessMaster WHERE PROCESSID=@PROCESSID";

        private const string SQL_SELECT_PROCESSCOMPLEXITY = "select * from Config.tblMasterTable where FieldId=15";
        private const string SQL_SELECT_PROCESSDUPLICACY = @"select count(*) from Config.tblProcessMaster (nolock) where ProcessName=@ProcessName";
        private const string SQL_SELECT_PROCESSWITHCLINETDUPLICACY = @"select count(*) from Config.tblProcessMaster (nolock)  where ProcessName=@ProcessName and ClientID=@ClientID and Disabled=0";
        private const string SQL_SELECT_PROCESSWITHCLINETDUPLICACY_FORUPDATE = @"select count(*) from Config.tblProcessMaster (nolock)  where ProcessName=@ProcessName and ClientID=@ClientID and ProcessID not in (@ProcessID) and Disabled=0";
        private const string SQL_CHECK_CALENDEREXISTANCE = @"QMS.USP_CheckForCalenderExistance";
        private const string SQL_SELECT_PROCESSTYPE = @"select isnull(processtype,0) from Config.tblProcessMaster where processid=@processid";

        private const string SQL_INSERT_PROCESSOWNER = @"dbo.Usp_CDS_Insert_ProcessOwner";
        private const string SP_SELECT_GETPROCESSOWNER = @"[Config].[USP_GetProcessOwner]";
        //process request
        private const string SQL_INSERTUPDATE_PROCREQUEST = @"[Config].[USP_SendApproveProcessRequest]";
        //Get panding Request
        private const string SQL_Select_PandingApproval = @"[Config].[USP_GetPandingApproval]";
        //get user process list
        private const string SQL_Select_ProcessList = @"USP_CDS_GetUserAttached";

        //get existing user list
        private const string SQL_SELECT_EXISTINGUSERREQUEST = @"[Config].[USP_CheckExistingUserRequest]";

        private const string SP_SELECT_INSERTPROCESSINFORMATION = @"Config.USP_InsertProcessInformation";
        //private const string SP_SELECT_INSERTPROCESSINFORMATION = @"[Config].[USP_InsertProcessInformation_Backup]";

        private const string SQL_CHECK_RoleForOrgProcess = @"begin
                                                            if exists(SELECT * FROM Config.tblMasterTable (nolock) where FieldId=75 and value in (Select RoleId from Config.tblUserRoleMapping (nolock) where UserId=@UserId and Disabled=0))
	                                                            select 1
                                                            else
	                                                            select 0
                                                            end";
        //private const string SQL_SELECT_PROCESSAVPABOVE = @"Usp_CDS_GetProcessAVPAndAbove";
        private const string SQL_SELECT_PROCESSAVPABOVE = @"[Config].[Usp_GetApproverList]";

        private const string SQL_SELECT_CLIENTPROCBYCAMP = @"Select ClientID,ProcessID from Config.tblProcessMaster where ProcessID=(Select ProcessID from Config.tblCampaignMaster where CampaignID=@CampaignID)";
        private const string SP_GETPROCESSBYUSERID = @"Usp_CDS_GetProcessbyUserID";
        private const string SQL_CheckProcessOwnerApproverLevel = @"[Config].[USP_CheckProcessOwnerApproverLevel]";


        private const string PARAM_QCPARAMXML = "@QCParamXML";
        private const string PARAM_ERPPARAMXML = "@ERPParamXML";
        private const string PARAM_CALENDARID = "@CalendarID";
        private const string PARAM_PROCESSTYPE = "@ProcessType";
        private const string PARAM_PROCESSWORKTYPE = "@ProcessWorkType";
        private const string PARAM_SBUID = "@SBUID";
        private const string PARAM_SCOPE = "@Scope";
        private const string PARAM_GOLIVEDATE = "@GoLiveDate";
        private const string PARAM_USERID = "@UserID";
        private const string PARAM_CLIENTID = "@ClientID";
        private const string PARAM_PROCESSID = "@ProcessID";
        private const string PARAM_PROCESSNAME = "@ProcessName";
        private const string PARAM_PROCESSDESC = "@Description";
        private const string PARAM_DISABLED = "@Disabled";
        private const string PARAM_CREATEDBY = "@CreatedBy";
        private const string PARAM_MODIFIEDBY = "@ModifiedBy";
        private const string PARAM_ACTIVEPROCESSLIST = "@ActiveProcessList";
        private const string PARAM_FIELDid = "@FieldID";
        private const string PARAM_PROCESSOWNER = "@ProcessOwner";
        private const string PARAM_AGENTID = "@AgentID";
        private const string PARAM_FLAG = "@Flag";
        private const string PARAM_ISACTIVE = "@IsActive";
        //PROCESS REQUEST
        private const string APPROVER_ID = "@Approver_Id";
        private const string SProcRequest_Id = "@ProcRequest_Id";
        private const string SAction = "@Action";
        private const string PASProcessMonthYear = "@PASProcessMonthYear";
        private const string PARAM_StabilizationStartDate = "@StabilizationStartDate";

        private const string PARAM_StabilizationEndDate = "@StabilizationEndDate";
        private const string PARAM_ProductionStartDate = "@ProductionStartDate";
        private const string PARAM_ProductionEndDate = "@ProductionEndDate";
        private const string PARAM_SupervisorFeebackTragetPerWeek = "@SupervisorFeebackTragetPerWeek";
        private const string PARAM_SupervisorFeedbackTargetFrequency = "@SupervisorFeedbackTargetFrequency";

        private const string PARAM_QCAFeebackTragetPerWeek = "@QCAFeebackTragetPerWeek";
        private const string PARAM_TargetAuditPerMonth = "@TargetAuditPerMonth";
        private const string PARAM_TargetQCAHrs = "@TargetQCAHrs";
        private const string PARAM_ProcessComplexity = "@ProcessComplexity";

        private const string PARAM_TYPE = "@Type";
        private const string PARAM_CAPTYPE = "@CAPType";

        //added by Omkar
        private const string PARAM_CAMPAIGNID = "@CampaignID";
        private const string PARAM_BATCHCODE = "@BatchCode";
        private const string PARAM_FROMDATE = "@FromDate";
        private const string PARAM_TODATE = "@ToDate";
        private const string PARAM_NOOFRECORDS = "@NoOfRecords";
        private const string PARAM_REQUESTBYID = "@RequestByID";
        private const string PARAM_APPROVERID = "@ApproverID";
        private const string PARAM_BATCHAPPROVALID = "@BatchApprovalID";

        private const string PARAM_FORMID = "@FormID";
        private const string PARAM_PASPROCESSTYPE = "@PASProcessType";

        //        private const string SQL_SELECT_PROCESSMANAGER = @"Select Distinct UM.UserID, UM.FirstName + ' ' + coalesce(UM.MiddleName, '') + ' ' + coalesce(UM.LastName, '')  + ' (' + Convert(varchar, UM.EmpId) + ')' as Agent
        //                                                       From Config.tblJobDesc JD Join Config.tblUserMaster UM On JD.JobID = UM.JobID 
        //                                                       Join Config.tblUserProcessMap UPM On UM.UserID = UPM.UserID
        //                                                       Where JD.JobID = 5 And UPM.ProcessID = @ProcessID";
        private const string SQL_SELECT_PROCESSMANAGER = @"Usp_CDS_GetProcessMangerAndAbove";

        private const string SQL_INSERT_DELETECAMPAIGNBATCH = @"WM.USP_InsertDeleteCampaignBatch";//dbo.USP_CDS_InsertDeleteCampaignBatch";
        private const string SQL_SP_CAMPAIGNBATCH = @"QMS.USP_GetCampaignBatchList";//dbo.USP_CDS_GetCampaignBatchList"
        private const string SQL_SP_REQUESTCAMPAIGNBATCH = @"Config.USP_GetPendingCampaignBatchList";
        private const string SQL_APPROVE_DELETECAMPAIGNBATCH = @"dbo.USP_CDS_ApproveDeleteCampaignBatch";

        private const string PRAM_RequestID = "@RequestID";

        private const string SQL_SP_PROCESSLISTSEARCH = @"[Config].[Usp_GetProcessListSearch]";
        private const string SQL_SP_GETPROCESSMONTHSTAGE1 = @"[Config].[Usp_GetProcessMonthStage_One]";
        //////////////

        #endregion Field Process

        #region Field Process SLA

        private const string SQL_SELECT_PROCESSSLA = @"SELECT PROCESSSLAID,PROCESSID,STARTDATE,ENDDATE,STAGE,FILEDATA,FILENAME FROM [Config].[tblProcessSLA] WHERE PROCESSSLAID=@PROCESSSLAID";
        private const string SQL_INSERT_PROCESSSLA = @"INSERT INTO [Config].[tblProcessSLA] (PROCESSID,STARTDATE,ENDDATE,STAGE,FILEDATA,FILENAME,DISABLED,CREATEDBY) VALUES(@PROCESSID,@STARTDATE,@ENDDATE,@STAGE,@FILEDATA,@FILENAME,@DISABLED,@CREATEDBY)";
        private const string SQL_UPDATE_PROCESSSLA1 = @"UPDATE [Config].[tblProcessSLA] SET STARTDATE=@STARTDATE,ENDDATE=@ENDDATE,STAGE=@STAGE,FILEDATA=@FILEDATA,FILENAME=@FILENAME,DISABLED=@DISABLED,MODIFIEDBY=@MODIFIEDBY WHERE PROCESSSLAID=@PROCESSSLAID";
        private const string SQL_DELETE_PROCESSSLA = @"BEGIN DELETE [Config].[tblProcessFTE] WHERE PROCESSID=@PROCESSID DELETE [Config].[tblProcessGroup] WHERE PROCESSID=@PROCESSID DELETE [Config].[tblProcessSLA] WHERE PROCESSID=@PROCESSID END";
        private const string SQL_UPDATE_PROCESSSLA2 = @"UPDATE [Config].[tblProcessSLA] SET STARTDATE=@STARTDATE,ENDDATE=@ENDDATE,STAGE=@STAGE,DISABLED=@DISABLED,MODIFIEDBY=@MODIFIEDBY WHERE PROCESSSLAID=@PROCESSSLAID";

        private const string PARAM_PROCESSSLAID = "@ProcessSLAID";
        private const string PARAM_STAGE = "@Stage";
        private const string PARAM_FILEDATA = "@FileData";
        private const string PARAM_FILENAME = "@FileName";
        private const string PARAM_STARTDATE = "@Startdate";
        private const string PARAM_ENDDATE = "@EndDate";
        private const string PARAM_PILOTSTARTDATE = "@PilotStartdate";
        private const string PARAM_PILOTENDDATE = "@PilotEndDate";

        #endregion Field Process SLA

        #region Field Process ERP Map

        private const string SQL_INSERT_PROCESSERPMAP = @"INSERT INTO [Config].[tblProcessGroup] (PROCESSID,ERPPROCESSID,DISABLED,CREATEDBY) VALUES (@PROCESSID,@ERPPROCESSID,@DISABLED,@CREATEDBY)";
        private const string SQL_DELETE_PROCESSERPMAP = @"DELETE [Config].[tblProcessGroup] WHERE PROCESSGROUPID=@PROCESSGROUPID";

        private const string PARAM_PROCESSGROUPID = "@ProcessGroupID";
        private const string PARAM_ERPPROCESSID = "@ERPProcessID";

        #endregion Field Process ERP Map

        #region Field Process FTE

        private const string SQL_INSERT_PROCESSFTE = @"INSERT INTO [Config].[tblProcessFTE] (PROCESSID,LOCATIONID,FTE,QCACOUNT,EFFECTIVESTARTDATE,EFFECTIVEENDDATE,DISABLED,CREATEDBY) VALUES (@PROCESSID,@LOCATIONID,@FTE,@QCACOUNT,@EFFECTIVESTARTDATE,@EFFECTIVEENDDATE,@DISABLED,@CREATEDBY)";
        private const string SQL_DELETE_PROCESSFTE = @"DELETE [Config].[tblProcessFTE] WHERE PROCESSFTEID=@PROCESSFTEID";
        //private const string SQL_UPDATE_PROCESSFTE = @"UPDATE [Config].[tblProcessFTE]  SET FTE=@FTE,QCACOUNT=@QCACOUNT,DISABLED=@DISABLED,MODIFIEDBY=@CREATEDBY WHERE PROCESSFTEID=@PROCESSFTEID";
        private const string SQL_UPDATE_PROCESSFTE = @"UPDATE [Config].[tblProcessFTE]  SET FTE=@FTE,QCACOUNT=@QCACOUNT,EFFECTIVESTARTDATE=@EFFECTIVESTARTDATE,EFFECTIVEENDDATE=@EFFECTIVEENDDATE,DISABLED=@DISABLED,MODIFIEDBY=@ModifiedBy WHERE PROCESSFTEID=@PROCESSFTEID";

        private const string PARAM_PROCESSFTEID = "@ProcessFTEID";
        private const string PARAM_FTE = "@FTE";
        private const string PARAM_QCACOUNT = "@QCACount";
        private const string PARAM_EFFECTIVESTARTDATE = "@EffectiveStartDate";
        private const string PARAM_EFFECTIVEENDDATE = "@EffectiveEndDate";

        #endregion Field Process FTE

        #region Field Process ERP Process

        private const string SQL_SELECT_ERPPROCESSLIST = @"SELECT isnull(EP.ERPPROCESSID,0) as ERPPROCESSID,isnull(EP.ERPCODE,0) AS ERPCODE,EP.ClientName,EP.NAME,isnull(LM.LOCATIONID,0) AS LOCATIONID ,LM.LOCATIONNAME
                                                            FROM Config.tblERPProcess (NOLOCK) EP
                                                            INNER JOIN Config.tblLocationMaster (NOLOCK) LM ON EP.LOCATIONID=LM.LOCATIONID
                                                            WHERE EP.DISABLED=0 AND EP.ERPPROCESSID IN (SELECT ITEMS FROM FN_SPLIT (@ERPNAME, ',')) ORDER BY EP.ClientName,NAME";
        private const string SQL_SELECT_ERPPROCESS = @"IF (@PROCESSID IS NULL)
                                                        BEGIN
                                                            SELECT EP.ERPPROCESSID,EP.NAME,EP.ClientName,LM.LOCATIONNAME,0,1 AS 'DISABLED'
                                                            FROM Config.tblERPProcess (NOLOCK) EP
                                                            INNER JOIN Config.tblLocationMaster (NOLOCK) LM ON EP.LOCATIONID=LM.LOCATIONID WHERE EP.NAME LIKE @ERPNAME AND LM.LOCATIONID=ISNULL(@LOCATIONID,LM.LOCATIONID)  ORDER BY EP.ClientName,NAME
														END
														ELSE
														BEGIN
														    SELECT EP.ERPPROCESSID,EP.NAME,EP.ClientName,LM.LOCATIONNAME,PG.PROCESSGROUPID,CASE WHEN PG.PROCESSGROUPID IS NOT NULL AND PG.PROCESSID=@PROCESSID THEN 0 ELSE 1 END AS 'DISABLED'
															FROM Config.tblERPProcess (NOLOCK) EP
															INNER JOIN Config.tblLocationMaster (NOLOCK) LM ON EP.LOCATIONID=LM.LOCATIONID
															LEFT OUTER JOIN [Config].[tblProcessGroup] PG ON EP.ERPPROCESSID=PG.ERPPROCESSID WHERE EP.NAME LIKE @ERPNAME AND LM.LOCATIONID=ISNULL(@LOCATIONID,LM.LOCATIONID) ORDER BY EP.ClientName,NAME

														END
                                                        ";
        private const string PARAM_ERPPROCESSNAME = "@ERPName";
        private const string PARAM_LOCATION = "@LocationID";

        #endregion Field Process ERP Process

        #region Constructor and Dispose

        /// <summary>
        /// Initializes a new instance of the <see cref="DLClient"/> class.
        /// </summary>
        public DLProcess(BETenant oTenant)
        { _oTenant = oTenant; }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        { _oTenant = null; }

        #endregion Constructor and Dispose

        #region GetProcess List

        /// <summary>
        /// Gets the process list.
        /// </summary>
        /// <param name="iLoggedinUserID">The loggedin user ID.</param>
        /// <param name="bActiveProcess">if set to <c>true</c> [b active process].</param>
        /// <returns>
        /// If bStatus is true
        /// return List of all the Process (Active or Inactive)
        /// else List of all the Active Process
        /// </returns>
        public List<BEProcessInfo> GetProcessList(int iLoggedinUserID, bool bActiveProcess)
        {
            return GetProcessList(iLoggedinUserID, "", bActiveProcess);
        }

        /// <summary>
        /// Gets the process list.
        /// </summary>
        /// <param name="iLoggedinUserID">The loggedin user ID.</param>
        /// <param name="ProcessName">Name of the process.</param>
        /// <param name="bActiveProcess">if set to <c>true</c> [b active process].</param>
        /// <returns>
        /// If bStatus is true,retunr List of all the Active Process
        /// </returns>
        public List<BEProcessInfo> GetProcessList(int iLoggedinUserID, string ProcessName, bool bActiveProcess)
        {
            List<BEProcessInfo> lProcess = new List<BEProcessInfo>();

            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetStoredProcCommand(SQL_SP_PROCESS);
            db.AddInParameter(dbCommand, PARAM_PROCESSNAME, DbType.String, "" + ProcessName + "%");
            db.AddInParameter(dbCommand, PARAM_USERID, DbType.Int32, iLoggedinUserID);
            db.AddInParameter(dbCommand, PARAM_ACTIVEPROCESSLIST, DbType.Boolean, bActiveProcess);
            using (IDataReader rdr = db.ExecuteReader(dbCommand))
            {
                while (rdr.Read())
                {
                    BEProcessInfo objCamp = new BEProcessInfo(Convert.ToInt32(rdr["ProcessID"]), Convert.ToInt32(rdr["ClientID"]), rdr["ProcessName"].ToString().DecodeHtmlString() + " " + (Convert.ToBoolean(rdr["Disabled"]) ? "(Disabled)" : ""), rdr["Description"].ToString().DecodeHtmlString(), Convert.ToBoolean(rdr["Disabled"]), 0);
                    lProcess.Add(objCamp);
                    objCamp = null;
                }
            }
            return lProcess;
        }

        /// <summary>
        /// To Get Process Access List
        /// </summary>
        /// <param name="iLoggedinUserID"></param>
        /// <param name="iAgentID"></param>
        /// <param name="bActiveProcess"></param>
        /// <returns></returns>
        public List<BEProcessInfo> GetProcessAccessList(int iLoggedinUserID, int iAgentID, bool bActiveProcess)
        {
            List<BEProcessInfo> lProcess = new List<BEProcessInfo>();

            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetStoredProcCommand(SQL_SP_PROCESSACCESSLIST);
            db.AddInParameter(dbCommand, PARAM_USERID, DbType.Int32, iLoggedinUserID);
            db.AddInParameter(dbCommand, PARAM_ISACTIVE, DbType.Boolean, bActiveProcess);
            db.AddInParameter(dbCommand, PARAM_AGENTID, DbType.Int32, iAgentID);
            db.AddInParameter(dbCommand, PARAM_FLAG, DbType.Int32, 2);
            using (IDataReader rdr = db.ExecuteReader(dbCommand))
            {
                while (rdr.Read())
                {
                    BEProcessInfo objCamp = new BEProcessInfo(Convert.ToInt32(rdr["ProcessID"]), Convert.ToInt32(rdr["ClientID"]), rdr["ProcessName"].ToString().DecodeHtmlString() + " " + (Convert.ToBoolean(rdr["Disabled"]) ? "(Disabled)" : ""), rdr["Description"].ToString().DecodeHtmlString(), Convert.ToBoolean(rdr["Disabled"]), 0);
                    lProcess.Add(objCamp);
                    objCamp = null;
                }
            }
            return lProcess;
        }
        /// <summary>
        /// Gets the process list Search Master.
        /// </summary>
        /// <param name="iLoggedinUserID">The i loggedin user ID.</param>
        /// <param name="iClientID">The i client ID.</param>
        /// <param name="ProcessName">Name of the process.</param>
        /// <param name="bActiveProcess">if set to <c>true</c> [b active process].</param>
        /// <returns></returns>
        public List<BEProcessInfo> GetProcessListSearch(int iLoggedinUserID, int iClientID, string ProcessName, bool bActiveProcess)
        {
            List<BEProcessInfo> lProcess = new List<BEProcessInfo>();

            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetStoredProcCommand(SQL_SP_PROCESSLISTSEARCH);
            db.AddInParameter(dbCommand, PARAM_PROCESSNAME, DbType.String, "" + ProcessName + "%");
            db.AddInParameter(dbCommand, PARAM_USERID, DbType.Int32, iLoggedinUserID);
            db.AddInParameter(dbCommand, PARAM_CLIENTID, DbType.Int32, iClientID);
            db.AddInParameter(dbCommand, PARAM_ACTIVEPROCESSLIST, DbType.Boolean, bActiveProcess);
            using (IDataReader rdr = db.ExecuteReader(dbCommand))
            {
                while (rdr.Read())
                {
                    BEProcessInfo objCamp = new BEProcessInfo(Convert.ToInt32(rdr["ProcessID"]), Convert.ToInt32(rdr["ClientID"]), rdr["ProcessName"].ToString().DecodeHtmlString() + " " + (Convert.ToBoolean(rdr["Disabled"]) ? "(Disabled)" : ""), rdr["Description"].ToString().DecodeHtmlString(), Convert.ToBoolean(rdr["Disabled"]), 0);
                    lProcess.Add(objCamp);
                    objCamp = null;
                }
            }
            return lProcess;
        }
        /// <summary>
        /// Gets the process list.
        /// </summary>
        /// <param name="iLoggedinUserID">The i loggedin user ID.</param>
        /// <param name="iClientID">The i client ID.</param>
        /// <param name="ProcessName">Name of the process.</param>
        /// <param name="bActiveProcess">if set to <c>true</c> [b active process].</param>
        /// <returns></returns>
        public List<BEProcessInfo> GetProcessList(int iLoggedinUserID, int iClientID, string ProcessName, bool bActiveProcess)
        {
            List<BEProcessInfo> lProcess = new List<BEProcessInfo>();

            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetStoredProcCommand(SQL_SP_PROCESS);
            db.AddInParameter(dbCommand, PARAM_PROCESSNAME, DbType.String, "" + ProcessName + "%");
            db.AddInParameter(dbCommand, PARAM_USERID, DbType.Int32, iLoggedinUserID);
            db.AddInParameter(dbCommand, PARAM_CLIENTID, DbType.Int32, iClientID);
            db.AddInParameter(dbCommand, PARAM_ACTIVEPROCESSLIST, DbType.Boolean, bActiveProcess);
            using (IDataReader rdr = db.ExecuteReader(dbCommand))
            {
                while (rdr.Read())
                {
                    BEProcessInfo objCamp = new BEProcessInfo(Convert.ToInt32(rdr["ProcessID"]), Convert.ToInt32(rdr["ClientID"]), rdr["ProcessName"].ToString().DecodeHtmlString() + " " + (Convert.ToBoolean(rdr["Disabled"]) ? "(Disabled)" : ""), rdr["Description"].ToString().DecodeHtmlString(), Convert.ToBoolean(rdr["Disabled"]), 0);
                    lProcess.Add(objCamp);
                    objCamp = null;
                }
            }
            return lProcess;
        }

        /// <summary>
        /// Gets the over rating process list.
        /// </summary>
        /// <param name="iLoggedinUserID">The loggedin user ID.</param>
        /// <param name="ProcessName">Name of the process.</param>
        /// <param name="bActiveProcess">if set to <c>true</c> [b active process].</param>
        /// <returns></returns>
        public List<BEProcessInfo> GetOverRatingProcessList(int iLoggedinUserID, string ProcessName, bool bActiveProcess)
        {
            List<BEProcessInfo> lProcess = new List<BEProcessInfo>();

            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetStoredProcCommand(SQL_SP_OVERRATINGPROCESS);
            db.AddInParameter(dbCommand, PARAM_PROCESSNAME, DbType.String, "" + ProcessName + "%");
            db.AddInParameter(dbCommand, PARAM_USERID, DbType.Int32, iLoggedinUserID);
            db.AddInParameter(dbCommand, PARAM_ACTIVEPROCESSLIST, DbType.Boolean, bActiveProcess);
            using (IDataReader rdr = db.ExecuteReader(dbCommand))
            {
                while (rdr.Read())
                {
                    BEProcessInfo objCamp = new BEProcessInfo(Convert.ToInt32(rdr["ProcessID"]), Convert.ToInt32(rdr["ClientID"]), rdr["ProcessName"].ToString().DecodeHtmlString() + " " + (Convert.ToBoolean(rdr["Disabled"]) ? "(Disabled)" : ""), rdr["Description"].ToString().DecodeHtmlString(), Convert.ToBoolean(rdr["Disabled"]), 0);
                    lProcess.Add(objCamp);
                    objCamp = null;
                }
            }
            return lProcess;
        }

        /// <summary>
        /// Gets the health process list.
        /// </summary>
        /// <param name="iLoggedinUserID">The loggedin user ID.</param>
        /// <param name="iClientID">The i client ID.</param>
        /// <param name="ProcessName">Name of the process.</param>
        /// <param name="bActiveProcess">if set to <c>true</c> [b active process].</param>
        /// <returns></returns>
        public List<BEProcessInfo> GetHealthProcessList(int iLoggedinUserID, int iClientID, string ProcessName, bool bActiveProcess)
        {
            List<BEProcessInfo> lProcess = new List<BEProcessInfo>();

            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetStoredProcCommand(SQL_SP_HEALTHPROCESS);
            db.AddInParameter(dbCommand, PARAM_PROCESSNAME, DbType.String, "" + ProcessName + "%");
            db.AddInParameter(dbCommand, PARAM_CLIENTID, DbType.Int32, iClientID);
            db.AddInParameter(dbCommand, PARAM_USERID, DbType.Int32, iLoggedinUserID);
            db.AddInParameter(dbCommand, PARAM_ACTIVEPROCESSLIST, DbType.Boolean, bActiveProcess);
            using (IDataReader rdr = db.ExecuteReader(dbCommand))
            {
                while (rdr.Read())
                {
                    BEProcessInfo objCamp = new BEProcessInfo(Convert.ToInt32(rdr["ProcessID"]), Convert.ToInt32(rdr["ClientID"]), rdr["ProcessName"].ToString().DecodeHtmlString() + " " + (Convert.ToBoolean(rdr["Disabled"]) ? "(Disabled)" : ""), rdr["Description"].ToString().DecodeHtmlString(), Convert.ToBoolean(rdr["Disabled"]), 0);
                    lProcess.Add(objCamp);
                    objCamp = null;
                }
            }
            return lProcess;
        }

        /// <summary>
        /// Get The process Data
        /// </summary>
        public List<BEProcessInfo> GetProcessList(int iProcessID)
        {
            List<BEProcessInfo> lProcess = new List<BEProcessInfo>();

            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetSqlStringCommand(SQL_SELECT_PROCESSID);
            db.AddInParameter(dbCommand, PARAM_PROCESSID, DbType.Int32, iProcessID);
            using (IDataReader rdr = db.ExecuteReader(dbCommand))
            {
                while (rdr.Read())
                {
                    BEProcessInfo objCamp = new BEProcessInfo(Convert.ToInt32(rdr["ProcessID"]), Convert.ToInt32(rdr["ClientID"]), rdr["ProcessName"].ToString().DecodeHtmlString(), rdr["Description"].ToString().DecodeHtmlString(), Convert.ToBoolean(rdr["Disabled"]), 0);
                    lProcess.Add(objCamp);
                    objCamp = null;
                }
            }
            return lProcess;
        }

        /// <summary>
        /// Gets the client list.
        /// </summary>
        /// <param name="iProcessID">The i process ID.</param>
        /// <returns></returns>
        public List<BEProcessInfo> GetClientList(int iProcessID)
        {
            List<BEProcessInfo> lProcess = new List<BEProcessInfo>();

            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetSqlStringCommand(SQL_SELECT_CLIENTDETAILS);
            db.AddInParameter(dbCommand, PARAM_PROCESSID, DbType.Int32, iProcessID);
            using (IDataReader rdr = db.ExecuteReader(dbCommand))
            {
                while (rdr.Read())
                {
                    BEProcessInfo objCamp = new BEProcessInfo(Convert.ToInt32(rdr["ProcessID"]), Convert.ToInt32(rdr["ClientID"]), rdr["ProcessName"].ToString().DecodeHtmlString(), rdr["Description"].ToString().DecodeHtmlString(), Convert.ToBoolean(rdr["Disabled"]), 0);
                    lProcess.Add(objCamp);
                    objCamp = null;
                }
            }
            return lProcess;
        }

        /// <summary>
        /// Get The process Data
        /// </summary>
        public BEProcessInfo GetProcessDetails(int iProcessID)
        {
            BEProcessInfo objProcess = new BEProcessInfo();
            string tname = _oTenant.ClientName;
            string StrQuery = "";


            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand;
            if (tname == System.Configuration.ConfigurationManager.AppSettings["AppTenantName"].ToString().ToUpper())
            {
                StrQuery = SQL_SELECT_PAS_PROCESSID;
                dbCommand = db.GetStoredProcCommand(StrQuery);
            }
            else
            {


                StrQuery = SQL_SELECT_PROCESSID;
                dbCommand = db.GetSqlStringCommand(StrQuery);
            }


            db.AddInParameter(dbCommand, PARAM_PROCESSID, DbType.Int32, iProcessID);
            //Master Process Infromation
            using (IDataReader rdr = db.ExecuteReader(dbCommand))
            {
                while (rdr.Read())
                {
                    objProcess.iProcessID = int.Parse(rdr["ProcessID"].ToString());
                    objProcess.iClientID = int.Parse(rdr["ClientID"].ToString());
                    objProcess.sProcessName = rdr["ProcessName"].ToString().DecodeHtmlString();
                    objProcess.sProcessDescription = rdr["Description"].ToString().DecodeHtmlString();
                    objProcess.bDisabled = bool.Parse(rdr["Disabled"].ToString());
                    objProcess.iCalendarID = rdr["CalendarID"].ToString() == "" ? 0 : int.Parse(rdr["CalendarID"].ToString());
                    objProcess.iProcessType = rdr["ProcessType"].ToString() == "" ? 0 : int.Parse(rdr["ProcessType"].ToString());
                    objProcess.iProcessWorkType = rdr["ProcessWorkType"].ToString() == "" ? 0 : int.Parse(rdr["ProcessWorkType"].ToString());
                    objProcess.iSBUID = rdr["ClientSBUID"].ToString() == "" ? 0 : int.Parse(rdr["ClientSBUID"].ToString());
                    objProcess.sSBUName = rdr["SBUName"].ToString();
                    objProcess.sScope = rdr["Scope"].ToString();

                    objProcess.dProcessStartDate = rdr["PilotStartDate"].ToString() == "" ? DateTime.MinValue : DateTime.Parse(rdr["PilotStartDate"].ToString());
                    objProcess.dProcessEndDate = rdr["PilotEndDate"].ToString() == "" ? DateTime.MinValue : DateTime.Parse(rdr["PilotEndDate"].ToString());

                    objProcess.dGoLiveDate = rdr["GoLiveDate"].ToString() == "" ? DateTime.MinValue : DateTime.Parse(rdr["GoLiveDate"].ToString());
                    objProcess.sCalenderName = (rdr["Name"].ToString());
                    objProcess.dStabilizationStartDate = rdr["StabilizationStartDate"].ToString() == "" ? DateTime.MinValue : DateTime.Parse(rdr["StabilizationStartDate"].ToString());
                    objProcess.dStabilizationEndDate = rdr["StabilizationEndDate"].ToString() == "" ? DateTime.MinValue : DateTime.Parse(rdr["StabilizationEndDate"].ToString());
                    objProcess.dProductionStartDate = rdr["ProductionStartDate"].ToString() == "" ? DateTime.MinValue : DateTime.Parse(rdr["ProductionStartDate"].ToString());
                    objProcess.dProductionEndDate = rdr["ProductionEndDate"].ToString() == "" ? DateTime.MinValue : DateTime.Parse(rdr["ProductionEndDate"].ToString());
                    objProcess.iSupervisorFeebackTragetPerWeek = rdr["SupervisorFeebackTragetPerWeek"].ToString() == "" ? int.MinValue : int.Parse(rdr["SupervisorFeebackTragetPerWeek"].ToString());
                    objProcess.iSupervisorFeedbackTargetFrequency = rdr["SupervisorFeedbackTargetFrequency"].ToString() == "" ? int.MinValue : int.Parse(rdr["SupervisorFeedbackTargetFrequency"].ToString());
                    objProcess.iQCAFeebackTragetPerWeek = rdr["QCAFeebackTragetPerWeek"].ToString() == "" ? int.MinValue : int.Parse(rdr["QCAFeebackTragetPerWeek"].ToString());
                    objProcess.iTargetAuditPerMonth = rdr["TargetAuditPerMonth"].ToString() == "" ? int.MinValue : int.Parse(rdr["TargetAuditPerMonth"].ToString());
                    objProcess.iTargetQCAHrs = rdr["TargetQCAHrs"].ToString() == "" ? int.MinValue : int.Parse(rdr["TargetQCAHrs"].ToString());
                    objProcess.iProcessComplexity = rdr["ProcessComplexity"].ToString() == "" ? int.MinValue : int.Parse(rdr["ProcessComplexity"].ToString());
                    objProcess.sProcessComplexity = rdr["ProcessComplexityText"].ToString();
                    objProcess.sProcessWorkType = rdr["ProcessWorkTypeText"].ToString();
                    objProcess.iCAPType = rdr["CAPType"].ToString() == "" ? int.MinValue : int.Parse(rdr["CAPType"].ToString());
                }

                //Process SLA Data
                rdr.NextResult();
                using (BEProcessSLA objProcessSLA = new BEProcessSLA())
                {
                    while (rdr.Read())
                    {
                        objProcessSLA.iProcessSLAID = int.Parse(rdr["ProcessSLAID"].ToString());
                        objProcessSLA.sStage = rdr["Stage"].ToString();
                        objProcessSLA.sFileName = rdr["FileName"].ToString();
                        objProcessSLA.dStartDate = rdr["StartDate"].ToString() == "" ? DateTime.MinValue : DateTime.Parse(rdr["StartDate"].ToString());
                        objProcessSLA.dEndDate = rdr["EndDate"].ToString() == "" ? DateTime.MinValue : DateTime.Parse(rdr["EndDate"].ToString());
                        //objProcessSLA.dPilotStartDate = rdr["PilotStartDate"].ToString() == "" ? DateTime.MinValue : DateTime.Parse(rdr["PilotStartDate"].ToString());
                        //objProcessSLA.dPilotEndDate = rdr["PilotEndDate"].ToString() == "" ? DateTime.MinValue : DateTime.Parse(rdr["PilotEndDate"].ToString());
                    }
                    objProcess.oProcessSLA = objProcessSLA;
                }
                //Process group data
                rdr.NextResult();
                List<BEProcessGroup> lProcessGroup = new List<BEProcessGroup>();
                while (rdr.Read())
                {
                    BEProcessGroup objProcessGroup = new BEProcessGroup();
                    objProcessGroup.iProcessGroupID = int.Parse(rdr["ProcessGroupID"].ToString());
                    BEERPProcess objERPProcess = new BEERPProcess();
                    objERPProcess.iERPProcessID = int.Parse(rdr["ERPProcessID"].ToString());
                    objERPProcess.iERPCode = int.Parse(rdr["ERPCode"].ToString());
                    objERPProcess.sName = rdr["Name"].ToString();
                    objERPProcess.oLocation.iLocationID = int.Parse(rdr["LocationID"].ToString());
                    objProcessGroup.oERPProcess = objERPProcess;
                    objProcessGroup.oRowState = RowState.NONE;
                    lProcessGroup.Add(objProcessGroup);
                    objProcessGroup = null;
                }
                objProcess.lProcessGroup = lProcessGroup;
                //Process FTE Data
                rdr.NextResult();
                List<BEProcessFTE> lProcessFTE = new List<BEProcessFTE>();
                while (rdr.Read())
                {
                    BEProcessFTE objProcessFTE = new BEProcessFTE();
                    objProcessFTE.iProcessFTEID = int.Parse(rdr["ProcessFTEID"].ToString());
                    objProcessFTE.oLocation = new BELocation { iLocationID = int.Parse(rdr["LocationID"].ToString()), sLocationName = rdr["LocationName"].ToString() };
                    objProcessFTE.iFTE = float.Parse(rdr["FTE"].ToString());
                    objProcessFTE.iQCACount = float.Parse(rdr["QCACount"].ToString());
                    objProcessFTE.dtEffectiveStartDate = rdr["EffectiveStartDate"].ToString() == "" ? DateTime.Now : Convert.ToDateTime(rdr["EffectiveStartDate"]);
                    objProcessFTE.dtEffectiveEndDate = rdr["EffectiveEndDate"].ToString() == "" ? DateTime.Now : Convert.ToDateTime(rdr["EffectiveEndDate"]);
                    objProcessFTE.oRowState = RowState.NONE;
                    lProcessFTE.Add(objProcessFTE);
                    objProcessFTE = null;
                }
                if (tname == System.Configuration.ConfigurationManager.AppSettings["AppTenantName"].ToString().ToUpper())
                {
                    rdr.NextResult();
                    while (rdr.Read())
                    {
                        objProcess.sPASProcessMonth = rdr["PASProcessDate"].ToString();
                        objProcess.sPASProcessType = rdr["PASProcessType"].ToString();
                    }
                    rdr.NextResult();
                    while (rdr.Read())
                    {
                        objProcess.sPASProcessFlagActionType = rdr["cnt"].ToString();
                        //objProcess.sPASProcessU_ActionType= rdr["UAF"].ToString();

                    }
                }

                objProcess.lProcessFTE = lProcessFTE;
            }
            return objProcess;
        }

        /// <summary>
        /// Gets the client wise process list.
        /// </summary>
        /// <param name="iLoggedinUserID">The loggedin user ID.</param>
        /// <param name="iClientID">The client ID.</param>
        /// <returns></returns>
        public List<BEProcessInfo> GetClientWiseProcessList(int iLoggedinUserID, int iClientID)
        {
            List<BEProcessInfo> lProcess = new List<BEProcessInfo>();

            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetStoredProcCommand(SQL_SP_PROCESSBYCLIENT);
            db.AddInParameter(dbCommand, PARAM_CLIENTID, DbType.Int32, iClientID);
            db.AddInParameter(dbCommand, PARAM_USERID, DbType.Int32, iLoggedinUserID);
            using (IDataReader rdr = db.ExecuteReader(dbCommand))
            {
                while (rdr.Read())
                {
                    BEProcessInfo objCamp = new BEProcessInfo(Convert.ToInt32(rdr["ProcessID"]), Convert.ToInt32(rdr["ClientID"]), rdr["ProcessName"].ToString().DecodeHtmlString(), rdr["Description"].ToString().DecodeHtmlString(), Convert.ToBoolean(rdr["Disabled"]), 0);
                    lProcess.Add(objCamp);
                    objCamp = null;
                }
            }
            return lProcess;
        }

        //added by wasim to get Process, Multi client wise
        public List<BEProcessInfo> GetMultiClientWiseProcessList(int iLoggedinUserID, string iClientID)
        {
            List<BEProcessInfo> lProcess = new List<BEProcessInfo>();

            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetStoredProcCommand(SQL_SP_PROCESSBYCLIENT);
            db.AddInParameter(dbCommand, PARAM_CLIENTID, DbType.String, iClientID);
            db.AddInParameter(dbCommand, PARAM_USERID, DbType.Int32, iLoggedinUserID);
            using (IDataReader rdr = db.ExecuteReader(dbCommand))
            {
                while (rdr.Read())
                {
                    BEProcessInfo objCamp = new BEProcessInfo(Convert.ToInt32(rdr["ProcessID"]), Convert.ToInt32(rdr["ClientID"]), rdr["ProcessName"].ToString().DecodeHtmlString(), rdr["Description"].ToString().DecodeHtmlString(), Convert.ToBoolean(rdr["Disabled"]), 0);
                    lProcess.Add(objCamp);
                    objCamp = null;
                }
            }
            return lProcess;
        }

        /// <summary>
        /// Gets the client list wise process list.
        /// </summary>
        /// <param name="iLoggedinUserID">The loggedin user ID.</param>
        /// <param name="ClientID">The client ID.</param>
        /// <returns></returns>
        public List<BEProcessInfo> GetClientListWiseProcessList(int iLoggedinUserID, string ClientID)
        {
            List<BEProcessInfo> lProcess = new List<BEProcessInfo>();

            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetStoredProcCommand(SQL_SP_PROCESSBYCLIENTLIST);
            db.AddInParameter(dbCommand, PARAM_CLIENTID, DbType.String, ClientID);
            db.AddInParameter(dbCommand, PARAM_USERID, DbType.Int32, iLoggedinUserID);
            using (IDataReader rdr = db.ExecuteReader(dbCommand))
            {
                while (rdr.Read())
                {
                    BEProcessInfo objCamp = new BEProcessInfo(Convert.ToInt32(rdr["ProcessID"]), Convert.ToInt32(rdr["ClientID"]), rdr["ProcessName"].ToString().DecodeHtmlString(), rdr["Description"].ToString().DecodeHtmlString(), Convert.ToBoolean(rdr["Disabled"]), 0);
                    lProcess.Add(objCamp);
                    objCamp = null;
                }
            }
            return lProcess;
        }

        #endregion GetProcess List

        #region Get Process Manager

        //added by Omkar to get Process Manager
        /// <summary>
        /// Get Process Manager
        /// </summary>
        /// <param name="iProcessId"></param>
        /// <returns></returns>
        public DataSet GetProcessManager(int iProcessId)
        {
            DataSet ds = new DataSet();

            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetStoredProcCommand(SQL_SELECT_PROCESSMANAGER);
            db.AddInParameter(dbCommand, PARAM_PROCESSID, DbType.Int16, iProcessId);
            db.LoadDataSet(dbCommand, ds, "ProcessManager");
            return ds;
        }

        #endregion

        #region Campaign Batch List
        //added by Omkar
        /// <summary>
        /// Campaign Batch List
        /// </summary>
        /// <param name="iCampaignId"></param>
        /// <param name="sBatchCode"></param>
        /// <param name="sFromDate"></param>
        /// <param name="sToDate"></param>
        /// <returns></returns>
        public DataTable GetCampaignBatchList(int iCampaignId, string sBatchCode, string sFromDate, string sToDate)
        {
            DataSet ds = new DataSet();

            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetStoredProcCommand(SQL_SP_CAMPAIGNBATCH);
            db.AddInParameter(dbCommand, PARAM_CAMPAIGNID, DbType.Int32, iCampaignId);
            db.AddInParameter(dbCommand, PARAM_BATCHCODE, DbType.String, sBatchCode);
            db.AddInParameter(dbCommand, PARAM_FROMDATE, DbType.DateTime, sFromDate);
            db.AddInParameter(dbCommand, PARAM_TODATE, DbType.DateTime, sToDate);
            db.LoadDataSet(dbCommand, ds, "CampaignBatchList");
            return ds.Tables[0];
        }
        public DataTable GetCampaignBatchQMSList(int iCampaignId, string sBatchCode, string sFromDate, string sToDate, string UserID)
        {
            DataSet ds = new DataSet();

            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetStoredProcCommand(SQL_SP_CAMPAIGNBATCH);
            db.AddInParameter(dbCommand, PARAM_CAMPAIGNID, DbType.Int32, iCampaignId);
            db.AddInParameter(dbCommand, PARAM_BATCHCODE, DbType.String, sBatchCode);
            db.AddInParameter(dbCommand, PARAM_FROMDATE, DbType.DateTime, sFromDate);
            db.AddInParameter(dbCommand, PARAM_TODATE, DbType.DateTime, sToDate);
            db.AddInParameter(dbCommand, PRAM_RequestID, DbType.Int32, Convert.ToInt32(UserID));
            db.LoadDataSet(dbCommand, ds, "CampaignBatchList");
            return ds.Tables[0];
        }
        //added by Omkar
        /// <summary>
        /// get pending Campaign Batch list
        /// </summary>
        /// <param name="iUserID"></param>
        /// <param name="sFromDate"></param>
        /// <param name="sToDate"></param>
        /// <returns></returns>
        public DataTable GetPendingCampaignBatchList(int iUserID, string sFromDate, string sToDate)
        {
            DataSet ds = new DataSet();

            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetStoredProcCommand(SQL_SP_REQUESTCAMPAIGNBATCH);
            db.AddInParameter(dbCommand, PARAM_APPROVERID, DbType.Int32, iUserID);
            db.AddInParameter(dbCommand, PARAM_FROMDATE, DbType.DateTime, sFromDate);
            db.AddInParameter(dbCommand, PARAM_TODATE, DbType.DateTime, sToDate);
            db.LoadDataSet(dbCommand, ds, "RequestCampaignBatchList");
            return ds.Tables[0];
        }

        #endregion

        #region Request delete campaign batch
        //added by Omkar
        /// <summary>
        /// Request delete campaign batch
        /// </summary>
        /// <param name="oCampaign"></param>
        /// <param name="sBatchCode"></param>
        /// <param name="iNoOfRecords"></param>
        /// <param name="sFromDate"></param>
        /// <param name="sToDate"></param>
        /// <param name="iRequestBy"></param>
        /// <returns></returns>
        public int RequestCampaignBatchDelete(BECampaignInfo oCampaign, string sBatchCode, int iNoOfRecords, string sFromDate, string sToDate, int iRequestBy)
        {
            try
            {

                Database db = DL_Shared.dbFactory(_oTenant);
                DbCommand dbCommand = db.GetStoredProcCommand(SQL_INSERT_DELETECAMPAIGNBATCH);
                db.AddInParameter(dbCommand, PARAM_CLIENTID, DbType.Int32, oCampaign.iClientID);
                db.AddInParameter(dbCommand, PARAM_PROCESSID, DbType.Int32, oCampaign.iProcessID);
                db.AddInParameter(dbCommand, PARAM_CAMPAIGNID, DbType.Int32, oCampaign.iCampaignID);
                db.AddInParameter(dbCommand, PARAM_BATCHCODE, DbType.String, sBatchCode);
                db.AddInParameter(dbCommand, PARAM_FROMDATE, DbType.DateTime, sFromDate);
                db.AddInParameter(dbCommand, PARAM_TODATE, DbType.DateTime, sToDate);
                db.AddInParameter(dbCommand, PARAM_NOOFRECORDS, DbType.Int32, iNoOfRecords);
                db.AddInParameter(dbCommand, PARAM_REQUESTBYID, DbType.Int32, iRequestBy);
                db.AddInParameter(dbCommand, PARAM_APPROVERID, DbType.Int32, oCampaign.iApprovedAccess);
                int rowAffected = db.ExecuteNonQuery(dbCommand);
                return rowAffected;
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                if (ex.Number == 547)
                {
                 //   throw new ExceptionHandler.ExceptionType.DataAccessCustomException(BPA.GlobalResources.DataLayer.Resources.ReferenceConstraint);
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

        //added by Omkar
        /// <summary>
        /// approve campaign batch request
        /// </summary>
        /// <param name="iUserId"></param>
        /// <param name="BatchApprovalID"></param>
        /// <param name="Action"></param>
        public int StatusCampaignBatchReqest(int iUserId, int BatchApprovalID, int Action)
        {
            try
            {
                Database db = DL_Shared.dbFactory(_oTenant);
                DbCommand dbCommand = db.GetStoredProcCommand(SQL_APPROVE_DELETECAMPAIGNBATCH);
                db.AddInParameter(dbCommand, PARAM_USERID, DbType.Int32, iUserId);
                db.AddInParameter(dbCommand, PARAM_BATCHAPPROVALID, DbType.Int32, BatchApprovalID);
                db.AddInParameter(dbCommand, SAction, DbType.Int32, Action);
                int rowAffected = db.ExecuteNonQuery(dbCommand);
                return rowAffected;
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

        #region Get Role Details

        /// <summary>
        /// Gets the role details.
        /// </summary>
        /// <returns></returns>
        public DataSet GetRoleDetails()
        {
            DataSet ds = new DataSet();
            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetSqlStringCommand(SQL_SELECT_ROLENAME);
            db.LoadDataSet(dbCommand, ds, "RoleDetails");
            return ds;
        }

        /// <summary>
        /// Getps the ROCESSSLA.
        /// </summary>
        /// <param name="FIELDID">The FIELDID.</param>
        /// <returns></returns>
        public DataSet GetProcessSLA(int FIELDID)
        {
            DataSet ds = new DataSet();
            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetSqlStringCommand(SQL_SELECT_PROCESSSTASTAGE);
            db.AddInParameter(dbCommand, PARAM_FIELDid, DbType.Int16, FIELDID);
            db.LoadDataSet(dbCommand, ds, "ProcessSLA");
            return ds;
        }

        #endregion Get Role Details

        #region Get Process SLA

        /// <summary>
        /// Get The process Data
        /// </summary>
        public BEProcessSLA GetProcessSLAList(int iProcessSLAID)
        {
            BEProcessSLA objProcessSLA = new BEProcessSLA();
            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetSqlStringCommand(SQL_SELECT_PROCESSSLA);
            db.AddInParameter(dbCommand, PARAM_PROCESSSLAID, DbType.Int32, iProcessSLAID);

            MemoryStream ms;                          // Writes the BLOB to a file.
            BinaryWriter bw;                        // Streams the BLOB to the FileStream object.
            int bufferSize = 10000;                   // Size of the BLOB buffer.
            byte[] outbyte = new byte[bufferSize];  // The BLOB byte[] buffer to be filled by GetBytes.
            long retval;                            // The bytes returned from GetBytes.
            long startIndex = 0;                    // The starting position in the BLOB output.

            using (IDataReader rdr = db.ExecuteReader(dbCommand))
            {
                while (rdr.Read())
                {
                    // Create a file to hold the output.
                    ms = new MemoryStream();
                    bw = new BinaryWriter(ms);

                    // Reset the starting byte for the new BLOB.
                    startIndex = 0;

                    // Read the bytes into outbyte[] and retain the number of bytes returned.
                    retval = rdr.GetBytes(5, startIndex, outbyte, 0, bufferSize);

                    // Continue reading and writing while there are bytes beyond the size of the buffer.
                    while (retval == bufferSize)
                    {
                        bw.Write(outbyte);
                        bw.Flush();

                        // Reposition the start index to the end of the last buffer and fill the buffer.
                        startIndex += bufferSize;
                        retval = rdr.GetBytes(5, startIndex, outbyte, 0, bufferSize);
                    }

                    // Write the remaining buffer.
                    bw.Write(outbyte, 0, (int)retval);
                    bw.Flush();

                    // Close the output file.
                    bw.Close();
                    ms.Close();

                    objProcessSLA.iProcessSLAID = int.Parse(rdr["ProcessSLAID"].ToString());
                    objProcessSLA.sStage = rdr["Stage"].ToString();
                    objProcessSLA.sFileName = rdr["FileName"].ToString();
                    objProcessSLA.aryFileData = ms.ToArray();
                    objProcessSLA.dStartDate = rdr["StartDate"].ToString() == "" ? DateTime.MinValue : DateTime.Parse(rdr["StartDate"].ToString());
                    objProcessSLA.dEndDate = rdr["EndDate"].ToString() == "" ? DateTime.MinValue : DateTime.Parse(rdr["EndDate"].ToString());
                }
            }
            return objProcessSLA;
        }

        #endregion Get Process SLA

        #region Gets the ERP Process List

        /// <summary>
        /// GET ERPs  process list.
        /// </summary>
        /// <param name="aryDistinctERPProcessId">The ary distinct ERP process id.</param>
        /// <returns></returns>
        public List<BEERPProcess> GetERPProcessList(ArrayList aryDistinctERPProcessId)
        {
            StringBuilder sbERPProcess = new StringBuilder();
            foreach (object obj in aryDistinctERPProcessId)
            {
                sbERPProcess.Append(obj.ToString());
                sbERPProcess.Append(",");
            }
            List<BEERPProcess> lERPProcess = new List<BEERPProcess>();

            //Database db = DL_Shared.dbFactory(_oTenant);

            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand;

            dbCommand = db.GetSqlStringCommand(SQL_SELECT_ERPPROCESSLIST);

            db.AddInParameter(dbCommand, PARAM_ERPPROCESSNAME, DbType.String, sbERPProcess.ToString());

            using (IDataReader rdr = db.ExecuteReader(dbCommand))
            {
                while (rdr.Read())
                {
                    BEERPProcess oERPProcess = new BEERPProcess
                    {
                        iERPProcessID = int.Parse(rdr["ERPProcessID"].ToString()),
                        sClient = rdr["ClientName"].ToString(),
                        iERPCode = int.Parse(rdr["ERPCode"].ToString()),
                        sName = rdr["Name"].ToString(),
                        oLocation = new BELocation { iLocationID = int.Parse(rdr["LocationID"].ToString()), sLocationName = rdr["LocationName"].ToString() },
                    };
                    lERPProcess.Add(oERPProcess);
                    oERPProcess = null;
                }
            }

            return lERPProcess;
        }

        /// <summary>
        /// Gets the ERP process list.
        /// </summary>
        /// <param name="ProcessID">The process ID.</param>
        /// <returns></returns>
        public List<BEERPProcess> GetERPProcessList(string sERPProcess, int iLocationID, int iProcessID)
        {
            List<BEERPProcess> lERPProcess = new List<BEERPProcess>();
            //Database db = DL_Shared.dbFactory(_oTenant);

            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand;

            dbCommand = db.GetSqlStringCommand(SQL_SELECT_ERPPROCESS);

            db.AddInParameter(dbCommand, PARAM_ERPPROCESSNAME, DbType.String, "%" + sERPProcess + "%");
            db.AddInParameter(dbCommand, PARAM_LOCATION, DbType.Int32, iLocationID == 0 ? SqlInt32.Null : iLocationID);
            db.AddInParameter(dbCommand, PARAM_PROCESSID, DbType.Int32, iProcessID == 0 ? SqlInt32.Null : iProcessID);

            using (IDataReader rdr = db.ExecuteReader(dbCommand))
            {
                while (rdr.Read())
                {
                    BEERPProcess oERPProcess = new BEERPProcess
                    {
                        iERPProcessID = Convert.ToInt32(rdr["ERPProcessID"]),
                        sName = rdr["Name"].ToString(),
                        sClient = rdr["ClientName"].ToString(),
                        oLocation = new BELocation { sLocationName = rdr["LocationName"].ToString() },
                        bDisabled = Convert.ToBoolean(rdr["Disabled"])
                    };
                    lERPProcess.Add(oERPProcess);
                    oERPProcess = null;
                }
            }

            return lERPProcess;
        }

        #endregion Gets the ERP Process List

        public void InsertUpdate(BEProcessInfo oProcess, string QCAXML, string ERPProcessXML, string Actiontype)
        {
            Database db = DL_Shared.dbFactory(_oTenant);

            try
            {

                DbCommand dbCommand = db.GetStoredProcCommand(SP_SELECT_INSERTPROCESSINFORMATION);
                db.AddInParameter(dbCommand, PARAM_PROCESSID, DbType.Int32, oProcess.iProcessID);
                db.AddInParameter(dbCommand, PARAM_CLIENTID, DbType.Int32, oProcess.iClientID == 0 ? SqlInt32.Null : oProcess.iClientID);
                db.AddInParameter(dbCommand, PARAM_PROCESSNAME, DbType.String, oProcess.sProcessName);
                db.AddInParameter(dbCommand, PARAM_PROCESSDESC, DbType.String, oProcess.sProcessDescription);
                db.AddInParameter(dbCommand, PARAM_CALENDARID, DbType.Int32, oProcess.iCalendarID);
                db.AddInParameter(dbCommand, PARAM_SBUID, DbType.Int32, oProcess.iSBUID == 0 ? SqlInt32.Null : oProcess.iSBUID);
                db.AddInParameter(dbCommand, PARAM_PROCESSTYPE, DbType.Int32, oProcess.iProcessType == 0 ? SqlInt32.Null : oProcess.iProcessType);
                db.AddInParameter(dbCommand, PARAM_PROCESSWORKTYPE, DbType.Int32, oProcess.iProcessWorkType == 0 ? SqlInt32.Null : oProcess.iProcessWorkType);
                db.AddInParameter(dbCommand, PARAM_SCOPE, DbType.String, oProcess.sScope);

                db.AddInParameter(dbCommand, PARAM_PILOTSTARTDATE, DbType.DateTime, oProcess.oProcessSLA.dPilotStartDate.ToShortDateString() == "1/1/0001" ? SqlDateTime.Null : oProcess.oProcessSLA.dPilotStartDate);
                db.AddInParameter(dbCommand, PARAM_PILOTENDDATE, DbType.DateTime, oProcess.oProcessSLA.dPilotEndDate.ToShortDateString() == "1/1/0001" ? SqlDateTime.Null : oProcess.oProcessSLA.dPilotEndDate);

                db.AddInParameter(dbCommand, PARAM_GOLIVEDATE, DbType.DateTime, DateTime.Now);
                db.AddInParameter(dbCommand, PARAM_DISABLED, DbType.Boolean, oProcess.bDisabled);
                db.AddInParameter(dbCommand, PARAM_CREATEDBY, DbType.Int32, oProcess.iCreatedBy);

                db.AddInParameter(dbCommand, PARAM_StabilizationStartDate, DbType.DateTime, oProcess.dStabilizationStartDate.ToShortDateString() == "1/1/0001" ? SqlDateTime.Null : oProcess.dStabilizationStartDate);
                db.AddInParameter(dbCommand, PARAM_StabilizationEndDate, DbType.DateTime, oProcess.dStabilizationEndDate.ToShortDateString() == "1/1/0001" ? SqlDateTime.Null : oProcess.dStabilizationEndDate);
                db.AddInParameter(dbCommand, PARAM_ProductionStartDate, DbType.DateTime, oProcess.dProductionStartDate.ToShortDateString() == "1/1/0001" ? SqlDateTime.Null : oProcess.dProductionStartDate);
                db.AddInParameter(dbCommand, PARAM_ProductionEndDate, DbType.DateTime, oProcess.dProductionEndDate.ToShortDateString() == "1/1/0001" ? SqlDateTime.Null : oProcess.dProductionEndDate);

                db.AddInParameter(dbCommand, PARAM_SupervisorFeedbackTargetFrequency, DbType.Int32, oProcess.iSupervisorFeedbackTargetFrequency == 0 ? SqlInt32.Null : oProcess.iSupervisorFeedbackTargetFrequency);
                db.AddInParameter(dbCommand, PARAM_SupervisorFeebackTragetPerWeek, DbType.Int32, oProcess.iSupervisorFeebackTragetPerWeek == 0 ? SqlInt32.Null : oProcess.iSupervisorFeebackTragetPerWeek);
                db.AddInParameter(dbCommand, PARAM_QCAFeebackTragetPerWeek, DbType.Int32, oProcess.iQCAFeebackTragetPerWeek == 0 ? SqlInt32.Null : oProcess.iQCAFeebackTragetPerWeek);
                db.AddInParameter(dbCommand, PARAM_TargetAuditPerMonth, DbType.Int32, oProcess.iTargetAuditPerMonth == 0 ? SqlInt32.Null : oProcess.iTargetAuditPerMonth);
                db.AddInParameter(dbCommand, PARAM_TargetQCAHrs, DbType.Int32, oProcess.iTargetQCAHrs == 0 ? SqlInt32.Null : oProcess.iTargetQCAHrs);
                db.AddInParameter(dbCommand, PARAM_ProcessComplexity, DbType.Int32, oProcess.iProcessComplexity == 0 ? SqlInt32.Null : oProcess.iProcessComplexity);
                db.AddInParameter(dbCommand, PARAM_CAPTYPE, DbType.Int32, oProcess.iCAPType);
                db.AddInParameter(dbCommand, "@comStatus", DbType.String, Actiontype);

                db.AddInParameter(dbCommand, PARAM_STAGE, DbType.String, oProcess.oProcessSLA.sStage);
                db.AddInParameter(dbCommand, PARAM_FILENAME, DbType.String, oProcess.oProcessSLA.sFileName);
                db.AddInParameter(dbCommand, PARAM_FILEDATA, DbType.Binary, oProcess.oProcessSLA.aryFileData);
                db.AddInParameter(dbCommand, PARAM_STARTDATE, DbType.DateTime, DateTime.Now);
                db.AddInParameter(dbCommand, PARAM_ENDDATE, DbType.DateTime, DateTime.Now);

                db.AddInParameter(dbCommand, PARAM_QCPARAMXML, DbType.String, QCAXML);
                db.AddInParameter(dbCommand, PARAM_ERPPARAMXML, DbType.String, ERPProcessXML);
                db.AddInParameter(dbCommand, PASProcessMonthYear, DbType.String, oProcess.sPASProcessMonth);
                db.AddInParameter(dbCommand, PARAM_PASPROCESSTYPE, DbType.String, oProcess.sPASProcessType);


                db.ExecuteNonQuery(dbCommand);
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

        #region Insert Process

        /// <summary>
        /// Insert Process data.
        /// </summary>
        /// <param name="oProcess">process.</param>
        public void InsertData(BEProcessInfo oProcess)
        {
            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetSqlStringCommand(SQL_INSERT_PROCESS);
            db.AddInParameter(dbCommand, PARAM_PROCESSID, DbType.Int32, oProcess.iProcessID);
            db.AddInParameter(dbCommand, PARAM_CLIENTID, DbType.Int32, oProcess.iClientID == 0 ? SqlInt32.Null : oProcess.iClientID);
            db.AddInParameter(dbCommand, PARAM_PROCESSNAME, DbType.String, oProcess.sProcessName);
            db.AddInParameter(dbCommand, PARAM_PROCESSDESC, DbType.String, oProcess.sProcessDescription);
            db.AddInParameter(dbCommand, PARAM_CALENDARID, DbType.Int32, oProcess.iCalendarID);
            db.AddInParameter(dbCommand, PARAM_SBUID, DbType.Int32, oProcess.iSBUID == 0 ? SqlInt32.Null : oProcess.iSBUID);
            db.AddInParameter(dbCommand, PARAM_PROCESSTYPE, DbType.Int32, oProcess.iProcessType == 0 ? SqlInt32.Null : oProcess.iProcessType);
            db.AddInParameter(dbCommand, PARAM_PROCESSWORKTYPE, DbType.Int32, oProcess.iProcessWorkType == 0 ? SqlInt32.Null : oProcess.iProcessWorkType);
            db.AddInParameter(dbCommand, PARAM_SCOPE, DbType.String, oProcess.sScope);

            db.AddInParameter(dbCommand, PARAM_PILOTSTARTDATE, DbType.DateTime, oProcess.oProcessSLA.dPilotStartDate.ToShortDateString() == "1/1/0001" ? SqlDateTime.Null : oProcess.oProcessSLA.dPilotStartDate);
            db.AddInParameter(dbCommand, PARAM_GOLIVEDATE, DbType.DateTime, oProcess.dGoLiveDate.ToShortDateString() == "1/1/0001" ? SqlDateTime.Null : oProcess.dGoLiveDate);
            db.AddInParameter(dbCommand, PARAM_PILOTENDDATE, DbType.DateTime, oProcess.oProcessSLA.dPilotEndDate.ToShortDateString() == "1/1/0001" ? SqlDateTime.Null : oProcess.oProcessSLA.dPilotEndDate);

            db.AddInParameter(dbCommand, PARAM_StabilizationStartDate, DbType.DateTime, oProcess.dStabilizationStartDate.ToShortDateString() == "1/1/0001" ? SqlDateTime.Null : oProcess.dStabilizationStartDate);
            db.AddInParameter(dbCommand, PARAM_StabilizationEndDate, DbType.DateTime, oProcess.dStabilizationEndDate.ToShortDateString() == "1/1/0001" ? SqlDateTime.Null : oProcess.dStabilizationEndDate);
            db.AddInParameter(dbCommand, PARAM_ProductionStartDate, DbType.DateTime, oProcess.dProductionStartDate.ToShortDateString() == "1/1/0001" ? SqlDateTime.Null : oProcess.dProductionStartDate);
            db.AddInParameter(dbCommand, PARAM_ProductionEndDate, DbType.DateTime, oProcess.dProductionEndDate.ToShortDateString() == "1/1/0001" ? SqlDateTime.Null : oProcess.dProductionEndDate);
            db.AddInParameter(dbCommand, PARAM_SupervisorFeedbackTargetFrequency, DbType.Int32, oProcess.iSupervisorFeedbackTargetFrequency == 0 ? SqlInt32.Null : oProcess.iSupervisorFeedbackTargetFrequency);
            db.AddInParameter(dbCommand, PARAM_SupervisorFeebackTragetPerWeek, DbType.Int32, oProcess.iSupervisorFeebackTragetPerWeek == 0 ? SqlInt32.Null : oProcess.iSupervisorFeebackTragetPerWeek);
            db.AddInParameter(dbCommand, PARAM_QCAFeebackTragetPerWeek, DbType.Int32, oProcess.iQCAFeebackTragetPerWeek == 0 ? SqlInt32.Null : oProcess.iQCAFeebackTragetPerWeek);
            db.AddInParameter(dbCommand, PARAM_TargetAuditPerMonth, DbType.Int32, oProcess.iTargetAuditPerMonth == 0 ? SqlInt32.Null : oProcess.iTargetAuditPerMonth);
            db.AddInParameter(dbCommand, PARAM_TargetQCAHrs, DbType.Int32, oProcess.iTargetQCAHrs == 0 ? SqlInt32.Null : oProcess.iTargetQCAHrs);
            db.AddInParameter(dbCommand, PARAM_ProcessComplexity, DbType.Int32, oProcess.iProcessComplexity == 0 ? SqlInt32.Null : oProcess.iProcessComplexity);
            db.AddInParameter(dbCommand, PARAM_CAPTYPE, DbType.Int32, oProcess.iCAPType);

            db.AddInParameter(dbCommand, PARAM_DISABLED, DbType.Boolean, oProcess.bDisabled);
            db.AddInParameter(dbCommand, PARAM_CREATEDBY, DbType.Int32, oProcess.iCreatedBy);

            using (DbConnection conn = db.CreateConnection())
            {
                conn.Open();// Connection Open
                using (DbTransaction trans = conn.BeginTransaction()) //Start Transaction
                {
                    try
                    {
                        int iProcessID = Convert.ToInt32(((decimal)db.ExecuteScalar(dbCommand, trans)));
                        InsertDataSLA(db, trans, oProcess.oProcessSLA, iProcessID);
                        ManageDataProcessFTE(db, trans, oProcess, iProcessID);
                        ManageDataProcessGroup(db, trans, oProcess, iProcessID);
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
                            //throw new ExceptionHandler.ExceptionType.DataAccessCustomException(BPA.GlobalResources.DataLayer.Resources.UniqueConstraints);
                        }
                        throw;// new ExceptionHandler.ExceptionType.DataAccessException(ex.Message);
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        throw;// new ExceptionHandler.ExceptionType.DataAccessException(ex.Message);
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }
        }

        /// <summary>
        /// Manages the process owner.
        /// </summary>
        /// <param name="oProcess">The o process.</param>
        public void ManageProcessOwnerData(BEProcessInfo oProcess)
        {
            try
            {
                Database db = DL_Shared.dbFactory(_oTenant);
                DbCommand dbCommand = db.GetStoredProcCommand(SQL_INSERT_PROCESSOWNER);

                db.AddInParameter(dbCommand, PARAM_CLIENTID, DbType.Int32, oProcess.iClientID);
                db.AddInParameter(dbCommand, PARAM_PROCESSID, DbType.Int32, oProcess.iProcessID);
                db.AddInParameter(dbCommand, PARAM_PROCESSOWNER, DbType.String, oProcess.sProcessOwner);
                db.AddInParameter(dbCommand, PARAM_CREATEDBY, DbType.Int32, oProcess.iCreatedBy);
                db.ExecuteNonQuery(dbCommand);
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

        /// <summary>
        /// Send approve Process creation request
        /// </summary>
        ///

        public int SendApproveProcessReqest(BEProcessInfo oProcess, int ProcRequest_Id, int Action)
        {
            try
            {
                Database db = DL_Shared.dbFactory(_oTenant);
                DbCommand dbCommand = db.GetStoredProcCommand(SQL_INSERTUPDATE_PROCREQUEST);
                db.AddInParameter(dbCommand, PARAM_CLIENTID, DbType.Int32, oProcess.iClientID);
                db.AddInParameter(dbCommand, PARAM_PROCESSID, DbType.Int32, oProcess.iProcessID);
                db.AddInParameter(dbCommand, PARAM_PROCESSOWNER, DbType.String, oProcess.sProcessOwner);
                db.AddInParameter(dbCommand, PARAM_USERID, DbType.Int32, oProcess.iCreatedBy);
                db.AddInParameter(dbCommand, APPROVER_ID, DbType.Int32, oProcess.iApprover);
                db.AddInParameter(dbCommand, SProcRequest_Id, DbType.Int32, ProcRequest_Id);
                db.AddInParameter(dbCommand, SAction, DbType.Int32, Action);
                int rowEffected = db.ExecuteNonQuery(dbCommand);
                return rowEffected;
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

        #endregion Insert Process

        public DataTable GetPandingApproval(int iUserId, string sFromDate, string sToDate)
        {
            DataSet ds = new DataSet();
            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetStoredProcCommand(SQL_Select_PandingApproval);
            db.AddInParameter(dbCommand, "@Approver_Id", DbType.Int32, iUserId);
            db.AddInParameter(dbCommand, "@FromDate", DbType.DateTime, sFromDate);
            db.AddInParameter(dbCommand, "@ToDate", DbType.DateTime, sToDate);

            db.LoadDataSet(dbCommand, ds, "PandingApproval");
            return ds.Tables[0];
        }

        #region Update Process

        /// <summary>
        /// Update Process data.
        /// </summary>
        /// <param name="oProcess">process.</param>
        public void UpdateData(BEProcessInfo oProcess)
        {
            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetSqlStringCommand(SQL_UPDATE_PROCESS);
            db.AddInParameter(dbCommand, PARAM_PROCESSID, DbType.Int32, oProcess.iProcessID);
            db.AddInParameter(dbCommand, PARAM_CLIENTID, DbType.Int32, oProcess.iClientID == 0 ? SqlInt32.Null : oProcess.iClientID);
            db.AddInParameter(dbCommand, PARAM_PROCESSNAME, DbType.String, oProcess.sProcessName);
            db.AddInParameter(dbCommand, PARAM_PROCESSDESC, DbType.String, oProcess.sProcessDescription);
            db.AddInParameter(dbCommand, PARAM_CALENDARID, DbType.Int32, oProcess.iCalendarID);
            db.AddInParameter(dbCommand, PARAM_SBUID, DbType.Int32, oProcess.iSBUID == 0 ? SqlInt32.Null : oProcess.iSBUID);
            db.AddInParameter(dbCommand, PARAM_PROCESSTYPE, DbType.Int32, oProcess.iProcessType == 0 ? SqlInt32.Null : oProcess.iProcessType);
            db.AddInParameter(dbCommand, PARAM_PROCESSWORKTYPE, DbType.Int32, oProcess.iProcessWorkType == 0 ? SqlInt32.Null : oProcess.iProcessWorkType);
            db.AddInParameter(dbCommand, PARAM_SCOPE, DbType.String, oProcess.sScope);

            db.AddInParameter(dbCommand, PARAM_PILOTSTARTDATE, DbType.DateTime, oProcess.oProcessSLA.dPilotStartDate.ToShortDateString() == "1/1/0001" ? SqlDateTime.Null : oProcess.oProcessSLA.dPilotStartDate);
            db.AddInParameter(dbCommand, PARAM_PILOTENDDATE, DbType.DateTime, oProcess.oProcessSLA.dPilotEndDate.ToShortDateString() == "1/1/0001" ? SqlDateTime.Null : oProcess.oProcessSLA.dPilotEndDate);

            db.AddInParameter(dbCommand, PARAM_GOLIVEDATE, DbType.DateTime, oProcess.dGoLiveDate.ToShortDateString() == "1/1/0001" ? SqlDateTime.Null : oProcess.dGoLiveDate);
            db.AddInParameter(dbCommand, PARAM_DISABLED, DbType.Boolean, oProcess.bDisabled);
            db.AddInParameter(dbCommand, PARAM_MODIFIEDBY, DbType.Int32, oProcess.iCreatedBy);

            db.AddInParameter(dbCommand, PARAM_StabilizationStartDate, DbType.DateTime, oProcess.dStabilizationStartDate.ToShortDateString() == "1/1/0001" ? SqlDateTime.Null : oProcess.dStabilizationStartDate);
            db.AddInParameter(dbCommand, PARAM_StabilizationEndDate, DbType.DateTime, oProcess.dStabilizationEndDate.ToShortDateString() == "1/1/0001" ? SqlDateTime.Null : oProcess.dStabilizationEndDate);
            db.AddInParameter(dbCommand, PARAM_ProductionStartDate, DbType.DateTime, oProcess.dProductionStartDate.ToShortDateString() == "1/1/0001" ? SqlDateTime.Null : oProcess.dProductionStartDate);
            db.AddInParameter(dbCommand, PARAM_ProductionEndDate, DbType.DateTime, oProcess.dProductionEndDate.ToShortDateString() == "1/1/0001" ? SqlDateTime.Null : oProcess.dProductionEndDate);

            db.AddInParameter(dbCommand, PARAM_SupervisorFeedbackTargetFrequency, DbType.Int32, oProcess.iSupervisorFeedbackTargetFrequency == 0 ? SqlInt32.Null : oProcess.iSupervisorFeedbackTargetFrequency);
            db.AddInParameter(dbCommand, PARAM_SupervisorFeebackTragetPerWeek, DbType.Int32, oProcess.iSupervisorFeebackTragetPerWeek == 0 ? SqlInt32.Null : oProcess.iSupervisorFeebackTragetPerWeek);
            db.AddInParameter(dbCommand, PARAM_QCAFeebackTragetPerWeek, DbType.Int32, oProcess.iQCAFeebackTragetPerWeek == 0 ? SqlInt32.Null : oProcess.iQCAFeebackTragetPerWeek);
            db.AddInParameter(dbCommand, PARAM_TargetAuditPerMonth, DbType.Int32, oProcess.iTargetAuditPerMonth == 0 ? SqlInt32.Null : oProcess.iTargetAuditPerMonth);
            db.AddInParameter(dbCommand, PARAM_TargetQCAHrs, DbType.Int32, oProcess.iTargetQCAHrs == 0 ? SqlInt32.Null : oProcess.iTargetQCAHrs);
            db.AddInParameter(dbCommand, PARAM_ProcessComplexity, DbType.Int32, oProcess.iProcessComplexity == 0 ? SqlInt32.Null : oProcess.iProcessComplexity);
            db.AddInParameter(dbCommand, PARAM_CAPTYPE, DbType.Int32, oProcess.iCAPType);

            using (DbConnection conn = db.CreateConnection())
            {
                conn.Open();// Connection Open
                using (DbTransaction trans = conn.BeginTransaction()) //Start Transaction
                {
                    try
                    {
                        db.ExecuteNonQuery(dbCommand, trans);
                        if (oProcess.oProcessSLA.iProcessSLAID == 0)
                            InsertDataSLA(db, trans, oProcess.oProcessSLA, oProcess.iProcessID);
                        else
                            UpdateDataSLA(db, trans, oProcess.oProcessSLA);

                        ManageDataProcessFTE(db, trans, oProcess, oProcess.iProcessID);
                        ManageDataProcessGroup(db, trans, oProcess, oProcess.iProcessID);
                        trans.Commit(); //Commit Transaction
                    }
                    catch (System.Data.SqlClient.SqlException ex)
                    {
                        trans.Rollback();
                        if (ex.Number == 547)
                        {
                            //throw new ExceptionHandler.ExceptionType.DataAccessCustomException(BPA.GlobalResources.DataLayer.Resources.ReferenceConstraint);
                        }
                        if (ex.Number == 2627)
                        {
                          //  throw new ExceptionHandler.ExceptionType.DataAccessCustomException(BPA.GlobalResources.DataLayer.Resources.UniqueConstraints);
                        }
                        throw;// new ExceptionHandler.ExceptionType.DataAccessException(ex.Message);
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        throw;// new ExceptionHandler.ExceptionType.DataAccessException(ex.Message);
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }
        }

        #endregion Update Process

        #region Delete Process

        /// <summary>
        /// Delete Process data.
        /// </summary>
        /// <param name="oProcess">process.</param>
        public void DeleteData(BEProcessInfo oProcess)
        {

            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetSqlStringCommand(SQL_DELETE_PROCESS);
            db.AddInParameter(dbCommand, PARAM_PROCESSID, DbType.Int32, oProcess.iProcessID);
            using (DbConnection conn = db.CreateConnection())
            {
                conn.Open();// Connection Open
                using (DbTransaction trans = conn.BeginTransaction()) //Start Transaction
                {
                    try
                    {
                        DeleteDataSLA(db, trans, oProcess.iProcessID);
                        db.ExecuteNonQuery(dbCommand, trans);
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
                    finally
                    {
                        conn.Close();
                    }
                }
            }
        }

        #endregion Delete Process

        #region Insert Process SLA

        /// <summary>
        /// Insert Process data.
        /// </summary>
        /// <param name="oProcess">process.</param>
        public void InsertDataSLA(Database db, DbTransaction trans, BEProcessSLA oProcessSLA, int iProcessID)
        {
            try
            {
                DbCommand dbCommand = db.GetSqlStringCommand(SQL_INSERT_PROCESSSLA);
                db.AddInParameter(dbCommand, PARAM_PROCESSID, DbType.Int32, iProcessID);
                db.AddInParameter(dbCommand, PARAM_STAGE, DbType.String, oProcessSLA.sStage);
                db.AddInParameter(dbCommand, PARAM_FILENAME, DbType.String, oProcessSLA.sFileName);
                db.AddInParameter(dbCommand, PARAM_FILEDATA, DbType.Binary, oProcessSLA.aryFileData);
                db.AddInParameter(dbCommand, PARAM_STARTDATE, DbType.DateTime, oProcessSLA.dStartDate.ToShortDateString() == "1/1/0001" ? SqlDateTime.Null : oProcessSLA.dStartDate);
                db.AddInParameter(dbCommand, PARAM_ENDDATE, DbType.DateTime, oProcessSLA.dEndDate.ToShortDateString() == "1/1/0001" ? SqlDateTime.Null : oProcessSLA.dEndDate);
                //db.AddInParameter(dbCommand, PARAM_PILOTSTARTDATE, DbType.DateTime, oProcessSLA.dPilotStartDate.ToShortDateString() == "1/1/0001" ? SqlDateTime.Null : oProcessSLA.dPilotStartDate);
                //db.AddInParameter(dbCommand, PARAM_PILOTENDDATE, DbType.DateTime, oProcessSLA.dPilotEndDate.ToShortDateString() == "1/1/0001" ? SqlDateTime.Null : oProcessSLA.dPilotEndDate);
                db.AddInParameter(dbCommand, PARAM_DISABLED, DbType.Boolean, oProcessSLA.bDisabled);
                db.AddInParameter(dbCommand, PARAM_CREATEDBY, DbType.Int32, oProcessSLA.iCreatedBy);
                db.ExecuteNonQuery(dbCommand, trans);
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

        #endregion Insert Process SLA

        #region Update Process SLA

        /// <summary>
        /// Update Process data.
        /// </summary>
        /// <param name="oProcess">process.</param>
        public void UpdateDataSLA(Database db, DbTransaction trans, BEProcessSLA oProcessSLA)
        {
            try
            {
                string strUpdateQuery = oProcessSLA.sFileName != "" ? SQL_UPDATE_PROCESSSLA1 : SQL_UPDATE_PROCESSSLA2;
                DbCommand dbCommand = db.GetSqlStringCommand(strUpdateQuery);
                db.AddInParameter(dbCommand, PARAM_PROCESSSLAID, DbType.Int32, oProcessSLA.iProcessSLAID);
                db.AddInParameter(dbCommand, PARAM_STAGE, DbType.String, oProcessSLA.sStage);
                if (oProcessSLA.sFileName != "")
                {
                    db.AddInParameter(dbCommand, PARAM_FILENAME, DbType.String, oProcessSLA.sFileName);
                    db.AddInParameter(dbCommand, PARAM_FILEDATA, DbType.Binary, oProcessSLA.aryFileData);
                }
                db.AddInParameter(dbCommand, PARAM_STARTDATE, DbType.DateTime, oProcessSLA.dStartDate.ToShortDateString() == "1/1/0001" ? SqlDateTime.Null : oProcessSLA.dStartDate);
                db.AddInParameter(dbCommand, PARAM_ENDDATE, DbType.DateTime, oProcessSLA.dEndDate.ToShortDateString() == "1/1/0001" ? SqlDateTime.Null : oProcessSLA.dEndDate);
                //db.AddInParameter(dbCommand, PARAM_PILOTSTARTDATE, DbType.DateTime, oProcessSLA.dPilotStartDate.ToShortDateString() == "1/1/0001" ? SqlDateTime.Null : oProcessSLA.dPilotStartDate);
                //db.AddInParameter(dbCommand, PARAM_PILOTENDDATE, DbType.DateTime, oProcessSLA.dPilotEndDate.ToShortDateString() == "1/1/0001" ? SqlDateTime.Null : oProcessSLA.dPilotEndDate);
                db.AddInParameter(dbCommand, PARAM_DISABLED, DbType.Boolean, oProcessSLA.bDisabled);
                db.AddInParameter(dbCommand, PARAM_MODIFIEDBY, DbType.Int32, oProcessSLA.iCreatedBy);
                db.ExecuteNonQuery(dbCommand, trans);
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

        #endregion Update Process SLA

        #region Delete Process SLA

        /// <summary>
        /// Delete Process data.
        /// </summary>
        /// <param name="db">The db.</param>
        /// <param name="trans">The trans.</param>
        /// <param name="iProcessID">The i process ID.</param>
        public void DeleteDataSLA(Database db, DbTransaction trans, int iProcessID)
        {
            try
            {
                DbCommand dbCommand = db.GetSqlStringCommand(SQL_DELETE_PROCESSSLA);
                db.AddInParameter(dbCommand, PARAM_PROCESSID, DbType.Int32, iProcessID);
                db.ExecuteNonQuery(dbCommand, trans);
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

        #endregion Delete Process SLA

        #region Manage Process Group

        /// <summary>
        /// Manage Process data.
        /// </summary>
        /// <param name="db">The db.</param>
        /// <param name="trans">The trans.</param>
        /// <param name="oProcess">process.</param>
        /// <param name="iProcessID">The process ID.</param>
        public void ManageDataProcessGroup(Database db, DbTransaction trans, BEProcessInfo oProcess, int iProcessID)
        {
            try
            {
                List<BEProcessGroup> lProcessGroup = oProcess.lProcessGroup;
                DbCommand dbCommand = null;
                foreach (BEProcessGroup item in lProcessGroup)
                {
                    if (item.oRowState == RowState.NEW)
                    {
                        dbCommand = db.GetSqlStringCommand(SQL_INSERT_PROCESSERPMAP);
                        db.AddInParameter(dbCommand, PARAM_PROCESSID, DbType.Int32, iProcessID);
                        db.AddInParameter(dbCommand, PARAM_ERPPROCESSID, DbType.Int32, item.oERPProcess.iERPProcessID);
                        db.AddInParameter(dbCommand, PARAM_DISABLED, DbType.Boolean, oProcess.bDisabled);
                        db.AddInParameter(dbCommand, PARAM_CREATEDBY, DbType.Int32, oProcess.iCreatedBy);
                        db.ExecuteNonQuery(dbCommand, trans);
                    }
                    else if (item.oRowState == RowState.DELETED)
                    {
                        dbCommand = db.GetSqlStringCommand(SQL_DELETE_PROCESSERPMAP);
                        if (item.oRowState == RowState.DELETED)
                        {
                            db.AddInParameter(dbCommand, PARAM_PROCESSGROUPID, DbType.Int32, item.iProcessGroupID);
                            db.ExecuteNonQuery(dbCommand, trans);
                        }
                    }
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

        #endregion Manage Process Group

        #region Manage Process FTE

        /// <summary>
        /// Manages the data process FTE.
        /// </summary>
        /// <param name="db">The db.</param>
        /// <param name="trans">The trans.</param>
        /// <param name="oProcess">The process.</param>
        /// <param name="iProcessID">The process ID.</param>
        public void ManageDataProcessFTE(Database db, DbTransaction trans, BEProcessInfo oProcess, int iProcessID)
        {
            try
            {
                List<BEProcessFTE> lProcessFTE = oProcess.lProcessFTE;
                DbCommand dbCommand = null;
                foreach (BEProcessFTE item in lProcessFTE)
                {
                    if (item.oRowState == RowState.NEW)
                    {
                        dbCommand = db.GetSqlStringCommand(SQL_INSERT_PROCESSFTE);
                        db.AddInParameter(dbCommand, PARAM_PROCESSID, DbType.Int32, iProcessID);
                        db.AddInParameter(dbCommand, PARAM_LOCATION, DbType.Int32, item.oLocation.iLocationID);
                        db.AddInParameter(dbCommand, PARAM_FTE, DbType.Double, item.iFTE);
                        db.AddInParameter(dbCommand, PARAM_QCACOUNT, DbType.Double, item.iQCACount);
                        db.AddInParameter(dbCommand, PARAM_EFFECTIVESTARTDATE, DbType.DateTime, item.dtEffectiveStartDate);
                        db.AddInParameter(dbCommand, PARAM_EFFECTIVEENDDATE, DbType.DateTime, item.dtEffectiveEndDate);
                        db.AddInParameter(dbCommand, PARAM_DISABLED, DbType.Boolean, oProcess.bDisabled);
                        db.AddInParameter(dbCommand, PARAM_CREATEDBY, DbType.Int32, oProcess.iCreatedBy);
                        db.ExecuteNonQuery(dbCommand, trans);
                    }
                    else if (item.oRowState == RowState.UPDATED)
                    {
                        dbCommand = db.GetSqlStringCommand(SQL_UPDATE_PROCESSFTE);
                        db.AddInParameter(dbCommand, PARAM_PROCESSFTEID, DbType.Int32, item.iProcessFTEID);
                        db.AddInParameter(dbCommand, PARAM_FTE, DbType.Double, item.iFTE);
                        db.AddInParameter(dbCommand, PARAM_QCACOUNT, DbType.Double, item.iQCACount);
                        db.AddInParameter(dbCommand, PARAM_EFFECTIVESTARTDATE, DbType.DateTime, item.dtEffectiveStartDate);
                        db.AddInParameter(dbCommand, PARAM_EFFECTIVEENDDATE, DbType.DateTime, item.dtEffectiveEndDate);
                        db.AddInParameter(dbCommand, PARAM_DISABLED, DbType.Boolean, oProcess.bDisabled);
                        db.AddInParameter(dbCommand, PARAM_MODIFIEDBY, DbType.Int32, oProcess.iModifiedBy);
                        db.ExecuteNonQuery(dbCommand, trans);
                    }
                    else if (item.oRowState == RowState.DELETED)
                    {
                        dbCommand = db.GetSqlStringCommand(SQL_DELETE_PROCESSFTE);
                        if (item.oRowState == RowState.DELETED)
                        {
                            db.AddInParameter(dbCommand, PARAM_PROCESSFTEID, DbType.Int32, item.iProcessFTEID);
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

        #endregion Manage Process FTE

        /// <summary>
        /// Gets the process complexity.
        /// </summary>
        /// <returns></returns>
        public DataSet GetProcessComplexity()
        {
            DataSet ds = new DataSet();


            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetSqlStringCommand(SQL_SELECT_PROCESSCOMPLEXITY);
            db.LoadDataSet(dbCommand, ds, "Complexity");
            return ds;
        }

        /// <summary>
        /// Checks the process for uniqueness.
        /// </summary>
        /// <param name="ClientId">The client id.</param>
        /// <param name="SBUId">The SBU id.</param>
        /// <returns></returns>
        public bool CheckProcessForUniqueness(string ProcessName)
        {

            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetSqlStringCommand(SQL_SELECT_PROCESSDUPLICACY);

            db.AddInParameter(dbCommand, PARAM_PROCESSNAME, DbType.String, ProcessName);
            if (Convert.ToInt32(db.ExecuteScalar(dbCommand)) > 0)
                return true;
            else
                return false;
        }
        // Added by ManishDwivedi-20-Jan-2022
        /// <summary>
        /// Checks the process for uniqueness with Client
        /// </summary>
        /// <param name="ProcessName"></param>
        /// <param name="iClientId"></param>
        /// <returns></returns>
        public bool CheckProcessByClientForUniqueness(string ProcessName, int iClientId)
        {

            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetSqlStringCommand(SQL_SELECT_PROCESSWITHCLINETDUPLICACY);

            db.AddInParameter(dbCommand, PARAM_PROCESSNAME, DbType.String, ProcessName);
            db.AddInParameter(dbCommand, PARAM_CLIENTID, DbType.Int32, iClientId);
            if (Convert.ToInt32(db.ExecuteScalar(dbCommand)) > 0)
                return true;
            else
                return false;
        }


        public bool CheckProcessByClientForUniqueness(string ProcessName, int iClientId, int iProcessID)
        {

            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetSqlStringCommand(SQL_SELECT_PROCESSWITHCLINETDUPLICACY_FORUPDATE);

            db.AddInParameter(dbCommand, PARAM_PROCESSNAME, DbType.String, ProcessName);
            db.AddInParameter(dbCommand, PARAM_CLIENTID, DbType.Int32, iClientId);
            db.AddInParameter(dbCommand, PARAM_PROCESSID, DbType.Int32, iProcessID);
            if (Convert.ToInt32(db.ExecuteScalar(dbCommand)) > 0)
                return true;
            else
                return false;
        }
        //end
        /// <summary>
        /// Checks the calender existance.
        /// </summary>
        /// <param name="ProcessId">The process id.</param>
        /// <param name="StartDate">The start date.</param>
        /// <param name="EndDate">The end date.</param>
        /// <param name="Type">The type.</param>
        /// <returns></returns>
        public int CheckCalenderExistance(int ProcessId, DateTime StartDate, DateTime EndDate, int Type)
        {


            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetStoredProcCommand(SQL_CHECK_CALENDEREXISTANCE);
            db.AddInParameter(dbCommand, PARAM_PROCESSID, DbType.Int32, ProcessId);
            db.AddInParameter(dbCommand, PARAM_STARTDATE, DbType.DateTime, StartDate);
            db.AddInParameter(dbCommand, PARAM_ENDDATE, DbType.DateTime, EndDate);
            db.AddInParameter(dbCommand, PARAM_TYPE, DbType.Int32, Type);
            return Convert.ToInt32(db.ExecuteScalar(dbCommand));
        }

        /// <summary>
        /// Checks the role for org process.
        /// </summary>
        /// <param name="UserId">The user id.</param>
        /// <returns></returns>
        public int CheckRoleForOrgProcess(int UserId)
        {

            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetSqlStringCommand(SQL_CHECK_RoleForOrgProcess);

            db.AddInParameter(dbCommand, PARAM_USERID, DbType.Int32, UserId);
            return Convert.ToInt32(db.ExecuteScalar(dbCommand));
        }

        /// <summary>
        /// Gets the type of the process.
        /// </summary>
        /// <param name="ProcessId">The process id.</param>
        /// <returns></returns>
        public int GetProcessType(int ProcessId)
        {

            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetSqlStringCommand(SQL_SELECT_PROCESSTYPE);

            db.AddInParameter(dbCommand, PARAM_PROCESSID, DbType.Int32, ProcessId);

            return Convert.ToInt32(db.ExecuteScalar(dbCommand));
        }

        /// <summary>
        /// Gets the user process owner.
        /// </summary>
        /// <param name="ProcessId">The process id.</param>
        /// <returns></returns>
        public DataSet GetUserProcessOwner(int ProcessId)
        {
            string[] aryTables = { "UserList", "ProcessOwner" };
            DataSet ds = new DataSet();

            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetStoredProcCommand(SP_SELECT_GETPROCESSOWNER);
            db.AddInParameter(dbCommand, PARAM_PROCESSID, DbType.Int32, ProcessId);
            db.LoadDataSet(dbCommand, ds, aryTables);
            return ds;
        }

        public DataTable GetUserProcessList(int ClientId, int ProcessId)
        {
            DataSet ds = new DataSet();

            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetStoredProcCommand(SQL_Select_ProcessList);
            db.AddInParameter(dbCommand, PARAM_CLIENTID, DbType.Int32, ClientId);
            db.AddInParameter(dbCommand, PARAM_PROCESSID, DbType.Int32, ProcessId);
            db.LoadDataSet(dbCommand, ds, "process");
            return ds.Tables[0];
        }

        /// <summary>
        /// Gets list of duplicate process owner creation request
        /// </summary>
        /// <returns></returns>
        public string ExistingUserRequest(int ProcessId, string ProcessOwner)
        {

            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetStoredProcCommand(SQL_SELECT_EXISTINGUSERREQUEST);

            db.AddInParameter(dbCommand, PARAM_PROCESSID, DbType.Int32, ProcessId);
            db.AddInParameter(dbCommand, PARAM_PROCESSOWNER, DbType.String, ProcessOwner);

            string existValue = db.ExecuteScalar(dbCommand).ToString();
            return existValue;
        }

        /// <summary>
        /// Get Process AVP Above
        /// </summary>
        /// <param name="iProcessId"></param>
        /// <returns></returns>
        public DataSet GetProcessAVPAbove(int iUserId, int iFormId, int iProcessId)
        {
            DataSet ds = new DataSet();

            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetStoredProcCommand(SQL_SELECT_PROCESSAVPABOVE);
            db.AddInParameter(dbCommand, PARAM_USERID, DbType.Int32, iUserId);
            db.AddInParameter(dbCommand, PARAM_FORMID, DbType.Int32, iFormId);
            db.AddInParameter(dbCommand, PARAM_PROCESSID, DbType.Int32, iProcessId);
            db.LoadDataSet(dbCommand, ds, "ProcessManager");
            return ds;
        }

        public List<BEProcessInfo> GetClientProcByCampID(int iCampaignId)
        {
            List<BEProcessInfo> lstReport = new List<BEProcessInfo>();
            try
            {

                Database db = DL_Shared.dbFactory(_oTenant);
                DbCommand dbCommand = db.GetSqlStringCommand(SQL_SELECT_CLIENTPROCBYCAMP);
                db.AddInParameter(dbCommand, PARAM_CAMPAIGNID, DbType.Int32, iCampaignId);
                IDataReader rdr = db.ExecuteReader(dbCommand);
                while (rdr.Read())
                {
                    lstReport.Add(new BEProcessInfo()
                    {
                        iClientID = int.Parse(rdr["ClientID"].ToString()),
                        iProcessID = int.Parse(rdr["ProcessID"].ToString())
                    });
                }
            }
            catch (Exception ex)
            {
                throw new NotImplementedException(ex.Message);
            }
            return lstReport;
        }

        public List<BEProcessInfo> GetProcessListByUserID(int iLoggedinUserID)
        {
            List<BEProcessInfo> lProcess = new List<BEProcessInfo>();

            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetStoredProcCommand(SP_GETPROCESSBYUSERID);
            db.AddInParameter(dbCommand, PARAM_USERID, DbType.Int32, iLoggedinUserID);
            using (IDataReader rdr = db.ExecuteReader(dbCommand))
            {
                while (rdr.Read())
                {

                    BEProcessInfo objCamp = new BEProcessInfo();
                    objCamp.iProcessID = Convert.ToInt32(rdr["ProcessID"]);
                    objCamp.iClientID = Convert.ToInt32(rdr["ClientID"]);
                    lProcess.Add(objCamp);
                    objCamp = null;
                }
            }
            return lProcess;
        }

        public string CheckProcessOwnerApproverLevel(BEProcessInfo oProcess)
        {
            try
            {

                Database db = DL_Shared.dbFactory(_oTenant);
                DbCommand dbCommand = db.GetStoredProcCommand(SQL_CheckProcessOwnerApproverLevel);
                db.AddInParameter(dbCommand, PARAM_PROCESSID, DbType.Int32, oProcess.iProcessID);
                db.AddInParameter(dbCommand, PARAM_PROCESSOWNER, DbType.String, oProcess.sProcessOwner);
                db.AddInParameter(dbCommand, APPROVER_ID, DbType.Int32, oProcess.iApprover);
                string str = db.ExecuteScalar(dbCommand).ToString();
                return str;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetProcessMonthStage1(int iProcessId)
        {
            DataSet ds = new DataSet();

            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetStoredProcCommand(SQL_SP_GETPROCESSMONTHSTAGE1);
            db.AddInParameter(dbCommand, PARAM_PROCESSID, DbType.Int32, iProcessId);
            db.LoadDataSet(dbCommand, ds, "ProcessManager");
            return ds;
        }

    }
}