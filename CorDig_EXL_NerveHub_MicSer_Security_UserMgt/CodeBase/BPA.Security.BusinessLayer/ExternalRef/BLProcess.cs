using BPA.Security.BusinessEntity.ExtrernalRefre;
using BPA.Security.BusinessEntity;
using BPA.Security.ServiceContract.ExternalRef;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using BPA.Security.DataLayer.ExternalRef;

namespace BPA.Security.BusinessLayer.ExternalRef
{/// <summary>
 /// BL Process
 /// </summary>
    //[ExceptionShielding("WCF Exception Shielding")]
    //[ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public class BLProcess : IProcessService, IDisposable
    {
        #region Constructor

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="BLProcess"/> class.
        /// </summary>
        public BLProcess()
        { }

        #endregion Constructor

        #region Get Process List

        /// <summary>
        /// Gets the process list.
        /// </summary>
        /// <param name="bActiveProcess">if set to <c>true</c> [active process].</param>
        /// <returns>
        /// If bStatus is true,retunr List of all the Active Process
        /// </returns>
        public List<BEProcessInfo> GetProcessList(int iLoggedinUserID, bool bActiveProcess, BETenant oTenant)
        {
            return GetProcessList(iLoggedinUserID, "", bActiveProcess, oTenant);
        }

        /// <summary>
        /// Gets the process list.
        /// </summary>
        /// <param name="iLoggedinUserID">The loggedin user ID.</param>
        /// <param name="ProcessName">Name of the process.</param>
        /// <param name="bActiveProcess">if set to <c>true</c> [active process].</param>
        /// <returns>
        /// If bStatus is true,retunr List of all the Active Process
        /// </returns>
        public List<BEProcessInfo> GetProcessList(int iLoggedinUserID, string ProcessName, bool bActiveProcess, BETenant oTenant)
        {
            using (DLProcess objProcess = new DLProcess(oTenant))
            {
                return objProcess.GetProcessList(iLoggedinUserID, ProcessName, bActiveProcess);
            }
        }

        /// <summary>
        /// To Get Access ProcessList
        /// </summary>
        /// <param name="iLoggedinUserID"></param>
        /// <param name="iAgentID"></param>
        /// <param name="bActiveProcess"></param>
        /// <returns></returns>
        public List<BEProcessInfo> GetProcessAccessList(int iLoggedinUserID, int iAgentID, bool bActiveProcess, BETenant oTenant)
        {
            using (DLProcess objProcess = new DLProcess(oTenant))
            {
                return objProcess.GetProcessAccessList(iLoggedinUserID, iAgentID, bActiveProcess);
            }
        }
        /// <summary>
        /// Gets the process list Search .
        /// </summary>
        /// <param name="iLoggedinUserID">The i loggedin user ID.</param>
        /// <param name="iClientID">The i client ID.</param>
        /// <param name="ProcessName">Name of the process.</param>
        /// <param name="bActiveProcess">if set to <c>true</c> [b active process].</param>
        /// <returns></returns>
        public List<BEProcessInfo> GetProcessListSearch(int iLoggedinUserID, int iClientID, string ProcessName, bool bActiveProcess, BETenant oTenant)
        {
            using (DLProcess objProcess = new DLProcess(oTenant))
            {
                return objProcess.GetProcessListSearch(iLoggedinUserID, iClientID, ProcessName, bActiveProcess);
            }
        }
        /// <summary>
        /// Gets the process list.
        /// </summary>
        /// <param name="iLoggedinUserID">The i loggedin user ID.</param>
        /// <param name="iClientID">The i client ID.</param>
        /// <param name="ProcessName">Name of the process.</param>
        /// <param name="bActiveProcess">if set to <c>true</c> [b active process].</param>
        /// <returns></returns>
        public List<BEProcessInfo> GetProcessList(int iLoggedinUserID, int iClientID, string ProcessName, bool bActiveProcess, BETenant oTenant)
        {
            using (DLProcess objProcess = new DLProcess(oTenant))
            {
                return objProcess.GetProcessList(iLoggedinUserID, iClientID, ProcessName, bActiveProcess);
            }
        }

        /// <summary>
        /// Gets the over rating process list.
        /// </summary>
        /// <param name="iLoggedinUserID">The i loggedin user ID.</param>
        /// <param name="ProcessName">Name of the process.</param>
        /// <param name="bActiveProcess">if set to <c>true</c> [b active process].</param>
        /// <returns></returns>
        public List<BEProcessInfo> GetOverRatingProcessList(int iLoggedinUserID, string ProcessName, bool bActiveProcess, BETenant oTenant)
        {
            using (DLProcess objProcess = new DLProcess(oTenant))
            {
                return objProcess.GetOverRatingProcessList(iLoggedinUserID, ProcessName, bActiveProcess);
            }
        }

        /// <summary>
        /// Gets the over rating process list.
        /// </summary>
        /// <param name="iLoggedinUserID">The i loggedin user ID.</param>
        /// <param name="iClientID">The i client ID.</param>
        /// <param name="ProcessName">Name of the process.</param>
        /// <param name="bActiveProcess">if set to <c>true</c> [b active process].</param>
        /// <returns></returns>
        public List<BEProcessInfo> GetHealthProcessList(int iLoggedinUserID, int iClientID, string ProcessName, bool bActiveProcess, BETenant oTenant)
        {
            using (DLProcess objProcess = new DLProcess(oTenant))
            {
                return objProcess.GetHealthProcessList(iLoggedinUserID, iClientID, ProcessName, bActiveProcess);
            }
        }

        /// <summary>
        /// Gets the process list.
        /// </summary>
        /// <param name="iProcessID">The i process ID.</param>
        /// <returns></returns>
        public List<BEProcessInfo> GetProcessList(int iProcessID, BETenant oTenant)
        {
            using (DLProcess objProcess = new DLProcess(oTenant))
            {
                return objProcess.GetProcessList(iProcessID);
            }
        }

        /// <summary>
        /// Gets the process list.
        /// </summary>
        /// <param name="ProcessID">The process ID.</param>
        /// <returns></returns>
        public BEProcessInfo GetProcessDetails(int ProcessID, BETenant oTenant)
        {
            using (DLProcess objProcess = new DLProcess(oTenant))
            {
                return objProcess.GetProcessDetails(ProcessID);
            }
        }

        /// <summary>
        /// Gets the process list.
        /// </summary>
        /// <param name="iLoggedinUserID">The i loggedin user ID.</param>
        /// <param name="ClientID">The client ID.</param>
        /// <returns></returns>
        public List<BEProcessInfo> GetClientWiseProcessList(int iLoggedinUserID, int ClientID, BETenant oTenant)
        {
            using (DLProcess objProcess = new DLProcess(oTenant))
            {
                return objProcess.GetClientWiseProcessList(iLoggedinUserID, ClientID);
            }
        }

        //added by Omkar to get Process Manager
        /// <summary>
        /// get Process Manager
        /// </summary>
        /// <param name="iProcessId"></param>
        /// <returns></returns>
        public DataSet GetProcessManager(int iProcessId, BETenant oTenant)
        {
            using (DLProcess objProcess = new DLProcess(oTenant))
            {
                return objProcess.GetProcessManager(iProcessId);
            }
        }

        //added by Omkar to get Campaign Batch list
        /// <summary>
        /// get Campaign Batch list
        /// </summary>
        /// <param name="iCampaignId"></param>
        /// <param name="sBatchCode"></param>
        /// <param name="sFromDate"></param>
        /// <param name="sToDate"></param>
        /// <returns></returns>
        public List<BEPendingApproval> GetCampaignBatchList(int iCampaignId, string sBatchCode, string sFromDate, string sToDate, BETenant oTenant)
        {
            List<BEPendingApproval> lstPendingApproval = new List<BEPendingApproval>();
            using (DLProcess objProcess = new DLProcess(oTenant))
            {
                lstPendingApproval = DataSetToList.ConvertTo<BEPendingApproval>(objProcess.GetCampaignBatchList(iCampaignId, sBatchCode, sFromDate, sToDate));
            }
            return lstPendingApproval;
        }
        public List<BEPendingApproval> GetCampaignBatchQMSList(int iCampaignId, string sBatchCode, string sFromDate, string sToDate, string UserID, BETenant oTenant)
        {
            List<BEPendingApproval> lstPendingApproval = new List<BEPendingApproval>();
            using (DLProcess objProcess = new DLProcess(oTenant))
            {
                lstPendingApproval = DataSetToList.ConvertTo<BEPendingApproval>(objProcess.GetCampaignBatchQMSList(iCampaignId, sBatchCode, sFromDate, sToDate, UserID));
            }
            return lstPendingApproval;
        }
        //added by Omkar for campaign batch delete request
        /// <summary>
        /// campaign batch delete request
        /// </summary>
        /// <param name="FormID"></param>
        /// <param name="oCampaign"></param>
        /// <param name="sBatchCode"></param>
        /// <param name="iNoOfRecords"></param>
        /// <param name="sFromDate"></param>
        /// <param name="sToDate"></param>
        /// <param name="iRequestBy"></param>
        /// <returns></returns>
        public int RequestCampaignBatchDelete(int FormID, BECampaignInfo oCampaign, string sBatchCode, int iNoOfRecords, string sFromDate, string sToDate, int iRequestBy, BETenant oTenant)
        {
            if (!CheckPermission.hasPermission(FormID, iRequestBy, PermissionSet.ADD))
            {
               // throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException(BPA.GlobalResources.BusinessLayer.Resources.InsertPermission);
            }
            using (DLProcess objProcess = new DLProcess(oTenant))
            {
                return objProcess.RequestCampaignBatchDelete(oCampaign, sBatchCode, iNoOfRecords, sFromDate, sToDate, iRequestBy);
            }
        }

        //added by Omkar to get pending campaign batch request
        /// <summary>
        /// get pending campaign batch request
        /// </summary>
        /// <param name="iUserID"></param>
        /// <param name="sFromDate"></param>
        /// <param name="sToDate"></param>
        /// <returns></returns>
        public List<BEPendingApproval> GetPendingCampaignBatchList(int iUserID, string sFromDate, string sToDate, BETenant oTenant)
        {
            List<BEPendingApproval> lstPendingApproval = new List<BEPendingApproval>();
            using (DLProcess objProcess = new DLProcess(oTenant))
            {
                lstPendingApproval = DataSetToList.ConvertTo<BEPendingApproval>(objProcess.GetPendingCampaignBatchList(iUserID, sFromDate, sToDate));
            }
            return lstPendingApproval;
        }

        //added by Omkar to approve campaign batch request
        /// <summary>
        /// approve campaign batch request
        /// </summary>
        /// <param name="iUserId"></param>
        /// <param name="FormID"></param>
        /// <param name="BatchApprovalID"></param>
        /// <param name="Action"></param>
        /// <returns></returns>
        public int StatusCampaignBatchReqest(int iUserId, int FormID, int BatchApprovalID, int Action, BETenant oTenant)
        {
            if (!CheckPermission.hasPermission(FormID, iUserId, PermissionSet.ADD))
            {
               // throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException(BPA.GlobalResources.BusinessLayer.Resources.InsertPermission);
            }
            using (DLProcess objProcess = new DLProcess(oTenant))
            {
                return objProcess.StatusCampaignBatchReqest(iUserId, BatchApprovalID, Action);
            }
        }

        //added by wasim to get Process, Multi client wise
        public List<BEProcessInfo> GetMultiClientWiseProcessList(int iLoggedinUserID, string ClientID, BETenant oTenant)
        {
            using (DLProcess objProcess = new DLProcess(oTenant))
            {
                return objProcess.GetMultiClientWiseProcessList(iLoggedinUserID, ClientID);
            }
        }

        public List<BEProcessInfo> GetClientListWiseProcessList(int iLoggedinUserID, string ClientID, BETenant oTenant)
        {
            using (DLProcess objProcess = new DLProcess(oTenant))
            {
                return objProcess.GetClientListWiseProcessList(iLoggedinUserID, ClientID);
            }
        }

        //public DataSet GetRoleDetails(BETenant oTenant)
        //{
        //    using (DLProcess objProcess = new DLProcess(oTenant))
        //    {
        //        return objProcess.GetRoleDetails();
        //    }
        //}

        /// <summary>
        /// Getps the ROCESSSLA.
        /// </summary>
        /// <returns></returns>
        public DataSet GetProcessSLA(int FiledId, BETenant oTenant)
        {
            using (DLProcess objProcess = new DLProcess(oTenant))
            {
                return objProcess.GetProcessSLA(FiledId);
            }
        }

        #endregion Get Process List

        #region Gets the process SLA list

        /// <summary>
        /// Gets the process SLA list.
        /// </summary>
        /// <param name="ProcessID">The process ID.</param>
        /// <returns></returns>
        public BEProcessSLA GetProcessSLAList(int ProcessSLAID, BETenant oTenant)
        {
            using (DLProcess objProcess = new DLProcess(oTenant))
            {
                return objProcess.GetProcessSLAList(ProcessSLAID);
            }
        }

        #endregion Gets the process SLA list

        #region Gets the ERP Process List

        /// <summary>
        /// Gets the ERP process list.
        /// </summary>
        /// <param name="ProcessID">The process ID.</param>
        /// <returns></returns>
        public List<BEERPProcess> GetERPProcessList(string sERPProcess, int iLocationID, int iProcessID, BETenant oTenant)
        {
            using (DLProcess objProcess = new DLProcess(oTenant))
            {
                return objProcess.GetERPProcessList(sERPProcess, iLocationID, iProcessID);
            }
        }

        /// <summary>
        /// GET ERPs  process list.
        /// </summary>
        /// <param name="aryDistinctERPProcessId">The ary distinct ERP process id.</param>
        /// <returns></returns>
        public List<BEERPProcess> GetERPProcessList(ArrayList aryDistinctERPProcessId, BETenant oTenant)
        {
            using (DLProcess objProcess = new DLProcess(oTenant))
            {
                return objProcess.GetERPProcessList(aryDistinctERPProcessId);
            }
        }

        #endregion Gets the ERP Process List

        #region Private Mentod

        /// <summary>
        /// Gets the QCAXML.
        /// </summary>
        /// <param name="oProcess">The object process.</param>
        /// <returns></returns>
        private string GetQCAXML(BEProcessInfo oProcess, BETenant oTenant)
        {

            var iProcessID = oProcess.iProcessID.ToString();
            var XMLQCA = new XElement("ROOT", from lstdata in oProcess.lProcessFTE
                                              select
                                                  new XElement("Column",
                                                  new XAttribute("PROCESSFTEID", lstdata.iProcessFTEID),
                                                  new XAttribute("LOCATIONID", lstdata.oLocation.iLocationID),
                                                  new XAttribute("FTE", lstdata.iFTE),
                                                  new XAttribute("QCACOUNT", lstdata.iQCACount),
                                                  new XAttribute("EFFECTIVESTARTDATE", lstdata.dtEffectiveStartDate),
                                                        new XAttribute("EFFECTIVEENDDATE", lstdata.dtEffectiveEndDate),
                                                           new XAttribute("CREATEDBY", lstdata.iCreatedBy),
                                                           new XAttribute("DISABLED", lstdata.bDisabled),
                                                  new XAttribute("STATUS", DisplayStatus(Convert.ToString(lstdata.oRowState)))
                                                  ));
            string QBMXML = Convert.ToString(XMLQCA);
            return QBMXML;


        }
        private string DisplayStatus(string _RowSate)
        {
            string Result = "";
            switch (_RowSate)
            {
                case "NEW":
                    Result = "New";
                    break;
                case "UPDATED":
                    Result = "Update";
                    break;
                case "DELETED":
                    Result = "Delete";
                    break;
            }
            return Result;
        }
        /// <summary>
        /// Gets the ERPP rocess XML.
        /// </summary>
        /// <param name="oProcess">The object process.</param>
        /// <returns></returns>
        private string GetERPPRocessXML(BEProcessInfo oProcess, BETenant oTenant)
        {

            //var iProcessID = oProcess.iProcessID.ToString();
            //var XMLQCA = new XElement("ROOT", from lstdata in oProcess.lProcessGroup
            //                                  select
            //                                      new XElement("Column",
            //                                      new XAttribute("PROCESSGROUPID", lstdata.iProcessGroupID),
            //                                      new XAttribute("PROCESSID", iProcessID),
            //                                      new XAttribute("ERPPROCESSID", lstdata.oERPProcess.iERPProcessID),
            //                                      new XAttribute("DISABLED", lstdata.oERPProcess.bDisabled),
            //                                      new XAttribute("CREATEDBY", lstdata.iCreatedBy),
            //                                      new XAttribute("STATUS", DisplayStatus(Convert.ToString(lstdata.oRowState)))
            //                                      ));
            //string QBMXML = Convert.ToString(XMLQCA);
            //return QBMXML;
            // return QBMXML;

            string XMLERPPROCESS = "";
            using (MemoryStream stream = new MemoryStream())
            {
                using (System.Xml.XmlTextWriter writer = new System.Xml.XmlTextWriter(stream, Encoding.UTF8))
                {
                    writer.WriteStartElement("ROOT");

                    for (int i = 0; i < oProcess.lProcessGroup.Count; i++)
                    {
                        writer.WriteStartElement("Column");
                        writer.WriteAttributeString("PROCESSGROUPID", oProcess.lProcessGroup[i].iProcessGroupID.ToString());
                        writer.WriteAttributeString("PROCESSID", oProcess.iProcessID.ToString());
                        writer.WriteAttributeString("ERPPROCESSID", oProcess.lProcessGroup[i].oERPProcess.iERPProcessID.ToString());
                        writer.WriteAttributeString("DISABLED", oProcess.lProcessGroup[i].oERPProcess.bDisabled.ToString());
                        writer.WriteAttributeString("CREATEDBY", oProcess.iCreatedBy.ToString());

                        switch (oProcess.lProcessGroup[i].oRowState)
                        {
                            case RowState.NEW:
                                writer.WriteAttributeString("STATUS", "New");
                                break;
                            case RowState.UPDATED:
                                writer.WriteAttributeString("STATUS", "Update");
                                break;
                            case RowState.DELETED:
                                writer.WriteAttributeString("STATUS", "Delete");
                                break;
                        }
                        writer.WriteEndElement();
                    }

                    writer.WriteEndElement();
                    writer.Flush();

                    stream.Seek(0, SeekOrigin.Begin);
                    int maxLength = 10000;
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        var sb = new StringBuilder();
                        while (true)
                        {
                            int ch = reader.Read();
                            if (ch == -1) break;
                            if (ch == '\r' || ch == '\n')
                            {
                                if (ch == '\r' && reader.Peek() == '\n') reader.Read();
                                XMLERPPROCESS = sb.ToString();
                            }
                            sb.Append((char)ch);

                            // Safety net here
                            //if (sb.Length > maxLength)
                            //throw new InvalidOperationException("Line is too long");
                        }
                        if (sb.Length > 0) XMLERPPROCESS = sb.ToString();



                        // XMLERPPROCESS = reader.ReadToEnd();
                    }
                }
            }

            return XMLERPPROCESS;
        }

        #endregion Private Mentod

        public void InsertUpdate(BEProcessInfo oProcess, string ActionType, BETenant oTenant)
        {
            using (DLProcess objProcess = new DLProcess(oTenant))
            {
                objProcess.InsertUpdate(oProcess, GetQCAXML(oProcess, oTenant), GetERPPRocessXML(oProcess, oTenant), ActionType);
            }
        }

        #region Insert Process Details

        /// <summary>
        /// Inserts the data.
        /// </summary>
        /// <param name="oProcess">process.</param>
        /// <param name="FormID">The form ID.</param>
        public void InsertData(BEProcessInfo oProcess, int FormID, BETenant oTenant)
        {
            if (!CheckPermission.hasPermission(FormID, oProcess.iCreatedBy, PermissionSet.ADD))
            {
               // throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException(BPA.GlobalResources.BusinessLayer.Resources.InsertPermission);
            }
            if (oProcess.sProcessName == string.Empty || oProcess.sProcessName == "")
            {
               // throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException("Process Name " + BPA.GlobalResources.BusinessLayer.Resources.RequiredField);
            }
            InsertUpdate(oProcess, "A", oTenant);

        }

        /// <summary>
        /// Manages the process owner.
        /// </summary>
        /// <param name="oProcess">The o process.</param>
        /// <param name="FormID">The form ID.</param>
        public void ManageProcessOwnerData(BEProcessInfo oProcess, int FormID, BETenant oTenant)
        {
            if (!CheckPermission.hasPermission(FormID, oProcess.iCreatedBy, PermissionSet.ADD))
            {
               // throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException(BPA.GlobalResources.BusinessLayer.Resources.InsertPermission);
            }
            using (DLProcess objProcess = new DLProcess(oTenant))
            {
                objProcess.ManageProcessOwnerData(oProcess);
            }
        }

        public int SendApproveProcessReqest(BEProcessInfo oProcess, int FormID, int ProcRequest_Id, int Action, BETenant oTenant)
        {
            if (!CheckPermission.hasPermission(FormID, oProcess.iCreatedBy, PermissionSet.ADD))
            {
              //  throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException(BPA.GlobalResources.BusinessLayer.Resources.InsertPermission);
            }
            using (DLProcess objProcess = new DLProcess(oTenant))
            {
                return objProcess.SendApproveProcessReqest(oProcess, ProcRequest_Id, Action);
            }
        }

        public DataTable GetPandingApproval(int iUserId, string sFromDate, string sToDate, BETenant oTenant)
        {
            using (DLProcess objProcess = new DLProcess(oTenant))
            {
                return objProcess.GetPandingApproval(iUserId, sFromDate, sToDate);
            }
        }

        #endregion Insert Process Details

        #region Update Process Details

        /// <summary>
        /// Updates the data.
        /// </summary>
        /// <param name="oProcess">process.</param>
        /// <param name="FormID">The form ID.</param>
        public void UpdateData(BEProcessInfo oProcess, int FormID, BETenant oTenant)
        {
            if (!CheckPermission.hasPermission(FormID, oProcess.iCreatedBy, PermissionSet.UPDATE))
            {
              //  throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException(BPA.GlobalResources.BusinessLayer.Resources.UpdatePermission);
            }
            if (oProcess.sProcessName == string.Empty || oProcess.sProcessName == "")
            {
               // throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException("Process Name " + BPA.GlobalResources.BusinessLayer.Resources.RequiredField);
            }

            InsertUpdate(oProcess, "M", oTenant);
        }

        #endregion Update Process Details

        #region Delete Process Details

        /// <summary>
        /// Deletes the data.
        /// </summary>
        /// <param name="oProcess">process.</param>
        /// <param name="FormID">The form ID.</param>
        public void DeleteData(BEProcessInfo oProcess, int FormID, BETenant oTenant)
        {
            if (!CheckPermission.hasPermission(FormID, oProcess.iCreatedBy, PermissionSet.DELETE))
            {
               // throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException(BPA.GlobalResources.BusinessLayer.Resources.DeletePermission);
            }
            if (oProcess.sProcessName == string.Empty || oProcess.sProcessName == "")
            {
               // throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException("Process Name " + BPA.GlobalResources.BusinessLayer.Resources.RequiredField);
            }
            using (DLProcess objProcess = new DLProcess(oTenant))
            {
                objProcess.DeleteData(oProcess);
            }
        }

        #endregion Delete Process Details

        #region Gets the process complexity.

        /// <summary>
        /// Gets the process complexity.
        /// </summary>
        /// <returns></returns>
        public DataSet GetProcessComplexity(BETenant oTenant)
        {
            using (DLProcess oProcess = new DLProcess(oTenant))
            {
                return oProcess.GetProcessComplexity();
            }
        }

        #endregion Gets the process complexity.

        #region Checks the process for uniqueness.

        /// <summary>
        /// Checks the process for uniqueness.
        /// </summary>
        /// <param name="ClientId">The client id.</param>
        /// <param name="SBUId">The SBU id.</param>
        /// <returns></returns>
        public bool CheckProcessForUniqueness(string ProcessName, BETenant oTenant)
        {
            using (DLProcess oProcess = new DLProcess(oTenant))
            {
                return oProcess.CheckProcessForUniqueness(ProcessName);
            }
        }

        #endregion Checks the process for uniqueness.

        #region Checks the calender existance.

        /// <summary>
        /// Checks the calender existance.
        /// </summary>
        /// <param name="ProcessId">The process id.</param>
        /// <param name="StartDate">The start date.</param>
        /// <param name="EndDate">The end date.</param>
        /// <param name="Type">The type.</param>
        /// <returns></returns>
        public int CheckCalenderExistance(int ProcessId, DateTime StartDate, DateTime EndDate, int Type, BETenant oTenant)
        {
            using (DLProcess oProcess = new DLProcess(oTenant))
            {
                return oProcess.CheckCalenderExistance(ProcessId, StartDate, EndDate, Type);
            }
        }

        #endregion Checks the calender existance.

        #region CheckRoleForOrgProcess

        /// <summary>
        /// Checks the role for org process.
        /// </summary>
        /// <param name="UserId">The user id.</param>
        /// <returns></returns>
        public int CheckRoleForOrgProcess(int UserId, BETenant oTenant)
        {
            using (DLProcess oProcess = new DLProcess(oTenant))
            {
                return oProcess.CheckRoleForOrgProcess(UserId);
            }
        }

        #endregion CheckRoleForOrgProcess

        #region Gets the type of the process.

        /// <summary>
        /// Gets the type of the process.
        /// </summary>
        /// <param name="ProcessId">The process id.</param>
        /// <returns></returns>
        public int GetProcessType(int ProcessId, BETenant oTenant)
        {
            using (DLProcess oProcess = new DLProcess(oTenant))
            {
                return oProcess.GetProcessType(ProcessId);
            }
        }

        #endregion Gets the type of the process.

        #region Gets the user process owner.

        /// <summary>
        /// Gets the user process owner.
        /// </summary>
        /// <param name="ProcessId">The process id.</param>
        /// <returns></returns>
        public DataSet GetUserProcessOwner(int ProcessId, BETenant oTenant)
        {
            using (DLProcess objProcess = new DLProcess(oTenant))
            {
                return objProcess.GetUserProcessOwner(ProcessId);
            }
        }

        public DataTable GetUserProcessList(int ClientId, int ProcessId, BETenant oTenant)
        {
            using (DLProcess objProcess = new DLProcess(oTenant))
            {
                return objProcess.GetUserProcessList(ClientId, ProcessId);
            }
        }

        #endregion Gets the user process owner.

        #region Gets duplicatelist of processownercreation request
        /// <summary>
        /// Gets duplicatelist of processownercreation request
        /// </summary>
        /// <param name="ProcessId"></param>
        /// <param name="ProcessOwner"></param>
        /// <returns></returns>
        public string ExistingUserRequest(int ProcessId, string ProcessOwner, BETenant oTenant)
        {
            using (DLProcess objProcess = new DLProcess(oTenant))
            {
                return objProcess.ExistingUserRequest(ProcessId, ProcessOwner);
            }
        }
        #endregion

        #region Get ProcessAVP Approval
        public List<BEApproval> GetProcessAVPAbove(int iUserId, int iFormId, int iProcessId, BETenant oTenant)
        {
            List<BEApproval> lstApproval = new List<BEApproval>();
            using (DLProcess objProcess = new DLProcess(oTenant))
            {
                lstApproval = DataSetToList.ConvertTo<BEApproval>(objProcess.GetProcessAVPAbove(iUserId, iFormId, iProcessId).Tables[0]);
            }
            return lstApproval;
        }
        #endregion

        public List<BEProcessInfo> GetClientProcByCampID(int iCampaignId, BETenant oTenant)
        {
            using (DLProcess objProcess = new DLProcess(oTenant))
            {
                return objProcess.GetClientProcByCampID(iCampaignId);
            }
        }

        public List<BEProcessInfo> GetProcessListByUserID(int iLoggedinUserID, BETenant oTenant)
        {
            using (DLProcess objProcess = new DLProcess(oTenant))
            {
                return objProcess.GetProcessList(iLoggedinUserID);
            }
        }
        public string CheckProcessOwnerApproverLevel(BEProcessInfo oProcess, BETenant oTenant)
        {
            using (DLProcess objProcess = new DLProcess(oTenant))
            {
                return objProcess.CheckProcessOwnerApproverLevel(oProcess);
            }
        }
        // Added by ManishDwivedi-20-Jan-2022
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ProcessName"></param>
        /// <param name="iClientId"></param>
        /// <param name="iProcessID"></param>
        /// <param name="oTenant"></param>
        /// <returns></returns>
        public bool CheckProcessByClientForUniqueness(string ProcessName, int iClientId, int iProcessID, BETenant oTenant)
        {
            using (DLProcess oProcess = new DLProcess(oTenant))
            {
                if (iProcessID > 0)
                {
                    return oProcess.CheckProcessByClientForUniqueness(ProcessName, iClientId, iProcessID);
                }
                else { return oProcess.CheckProcessByClientForUniqueness(ProcessName, iClientId); }

            }
        }
    }
}