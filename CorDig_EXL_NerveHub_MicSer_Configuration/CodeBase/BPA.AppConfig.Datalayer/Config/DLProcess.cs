using BPA.AppConfig.BusinessEntity.Application;
using BPA.AppConfig.BusinessEntity.Config;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlTypes;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BPA.Utility;
using BPA.AppConfig.BusinessEntity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Collections;

namespace BPA.AppConfig.Datalayer.Config
{
    public class DLProcess : IDisposable
    {

        private BETenant _oTenant = null;
        public DLProcess(BETenant oTenant)
        {
            _oTenant = oTenant;
        }
        public void Dispose()
        { _oTenant = null; }

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
        private const string SQL_SP_PROCESSLISTSEARCH = @"[Config].[Usp_GetProcessListSearch]";
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
        private const string SQL_SELECT_ERPPROCESSLIST = @"SELECT isnull(EP.ERPPROCESSID,0) as ERPPROCESSID,isnull(EP.ERPCODE,0) AS ERPCODE,EP.ClientName,EP.NAME,isnull(LM.LOCATIONID,0) AS LOCATIONID ,LM.LOCATIONNAME
                                                            FROM Config.tblERPProcess (NOLOCK) EP
                                                            INNER JOIN Config.tblLocationMaster (NOLOCK) LM ON EP.LOCATIONID=LM.LOCATIONID
                                                            WHERE EP.DISABLED=0 AND EP.ERPPROCESSID IN (SELECT ITEMS FROM FN_SPLIT (@ERPNAME, ',')) ORDER BY EP.ClientName,NAME";

        private const string SQL_CHECK_RoleForOrgProcess = @"BEGIN
                                                            IF EXISTS(SELECT * FROM Config.tblMasterTable (nolock) WHERE FieldId=75 AND value IN (SELECT RoleId FROM Config.tblUserRoleMapping (NOLOCK) WHERE UserId=@UserId AND Disabled=0))
	                                                            SELECT 1
                                                            ELSE
	                                                            SELECT 0
                                                            END";
        private const string SQL_SELECT_PROCESSWITHCLINETDUPLICACY = @"select count(*) from Config.tblProcessMaster (nolock)  where ProcessName=@ProcessName and ClientID=@ClientID and Disabled=0";
        private const string SQL_SELECT_PROCESSWITHCLINETDUPLICACY_FORUPDATE = @"select count(*) from Config.tblProcessMaster (nolock)  where ProcessName=@ProcessName and ClientID=@ClientID and ProcessID not in (@ProcessID) and Disabled=0";


        private const string SP_SELECT_INSERTPROCESSINFORMATION = @"Config.USP_InsertProcessInformation";
        private const string SQL_SELECT_PAS_PROCESSID = @"[Config].[USP_GetProcess_PASData]";
        private const string SQL_CHECK_CALENDEREXISTANCE = @"QMS.USP_CheckForCalenderExistance";
        private const string SQL_SP_PROCESS = @"[Config].[Usp_GetProcessList]";

        private const string PARAM_ERPPROCESSNAME = "@ERPName";
        private const string PARAM_LOCATION = "@LocationID";
        private const string PARAM_PROCESSID = "@ProcessID";
        private const string PARAM_PROCESSNAME = "@ProcessName";
        private const string PARAM_USERID = "@UserID";
        private const string PARAM_CLIENTID = "@ClientID";
        private const string PARAM_ACTIVEPROCESSLIST = "@ActiveProcessList";
        private const string PARAM_QCPARAMXML = "@QCParamXML";
        private const string PARAM_ERPPARAMXML = "@ERPParamXML";
        private const string PARAM_CALENDARID = "@CalendarID";
        private const string PARAM_PROCESSTYPE = "@ProcessType";
        private const string PARAM_PROCESSWORKTYPE = "@ProcessWorkType";
        private const string PARAM_SBUID = "@SBUID";
        private const string PARAM_SCOPE = "@Scope";
        private const string PARAM_GOLIVEDATE = "@GoLiveDate";
        private const string PARAM_PROCESSDESC = "@Description";
        private const string PARAM_DISABLED = "@Disabled";
        private const string PARAM_CREATEDBY = "@CreatedBy";
        private const string PARAM_MODIFIEDBY = "@ModifiedBy";
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
        private const string PARAM_PILOTSTARTDATE = "@PilotStartdate";
        private const string PARAM_PILOTENDDATE = "@PilotEndDate";
        private const string PARAM_PROCESSSLAID = "@ProcessSLAID";
        private const string PARAM_STAGE = "@Stage";
        private const string PARAM_FILEDATA = "@FileData";
        private const string PARAM_FILENAME = "@FileName";
        private const string PARAM_STARTDATE = "@Startdate";
        private const string PARAM_ENDDATE = "@EndDate";

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
        public BEProcessInfo GetProcessDetails(int iProcessID)
        {
            BEProcessInfo objProcess = new BEProcessInfo();
            string tname = _oTenant.ClientName;
            string StrQuery = "";


            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand;
            if (tname == _oTenant.AppTenantName)//  "EXLPAS" System.Configuration.AppSettings["AppTenantName"].ToString().ToUpper())
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
                //if (tname == System.Configuration.ConfigurationManager.AppSettings["AppTenantName"].ToString().ToUpper())
                //{
                //    rdr.NextResult();
                //    while (rdr.Read())
                //    {
                //        objProcess.sPASProcessMonth = rdr["PASProcessDate"].ToString();
                //        objProcess.sPASProcessType = rdr["PASProcessType"].ToString();
                //    }
                //    rdr.NextResult();
                //    while (rdr.Read())
                //    {
                //        objProcess.sPASProcessFlagActionType = rdr["cnt"].ToString();
                //        //objProcess.sPASProcessU_ActionType= rdr["UAF"].ToString();

                //    }
                //}

                objProcess.lProcessFTE = lProcessFTE;
            }
            return objProcess;
        }

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

        public List<BEProcessInfo> GetProcessList(int iLoggedinUserID, bool bActiveProcess)
        {
            List<BEProcessInfo> lProcess = new List<BEProcessInfo>();

            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetStoredProcCommand(SQL_SP_PROCESS);
            db.AddInParameter(dbCommand, PARAM_USERID, DbType.Int32, iLoggedinUserID);
            db.AddInParameter(dbCommand, PARAM_PROCESSNAME, DbType.String, string.Empty);
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

        public int CheckRoleForOrgProcess(int UserId)
        {

            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetSqlStringCommand(SQL_CHECK_RoleForOrgProcess);

            db.AddInParameter(dbCommand, PARAM_USERID, DbType.Int32, UserId);
            return Convert.ToInt32(db.ExecuteScalar(dbCommand));
        }

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

    }
}
