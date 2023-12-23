using BPA.AppConfig.BusinessEntity;
using BPA.AppConfig.BusinessEntity.Application;
using BPA.AppConfig.BusinessEntity.Config;
using BPA.AppConfig.Datalayer.Config;
using BPA.AppConfig.ServiceContracts.ServiceContracts.Config;
using System.Collections;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace BPA.AppConfig.BusinessLayer.Config
{
    //[ExceptionShielding("WCF Exception Shielding")]
    // [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
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

        public List<BEERPProcess> GetERPProcessList(string sERPProcess, int iLocationID, int iProcessID, BETenant oTenant)
        {
            using (DLProcess objProcess = new DLProcess(oTenant))
            {
                return objProcess.GetERPProcessList(sERPProcess, iLocationID, iProcessID);
            }
        }

        public List<BEProcessInfo> GetProcessListSearch(int iLoggedinUserID, int iClientID, string ProcessName, bool bActiveProcess, BETenant oTenant)
        {
            using (DLProcess objProcess = new DLProcess(oTenant))
            {
                return objProcess.GetProcessListSearch(iLoggedinUserID, iClientID, ProcessName, bActiveProcess);
            }
        }

        public BEProcessInfo GetProcessDetails(int ProcessID, BETenant oTenant)
        {
            using (DLProcess objProcess = new DLProcess(oTenant))
            {
                return objProcess.GetProcessDetails(ProcessID);
            }
        }

        public void InsertData(BEProcessInfo oProcess, int FormID, BETenant oTenant)
        {
            /*
            if (!CheckPermission.hasPermission(FormID, oProcess.iCreatedBy, PermissionSet.ADD))
            {
                throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException(BPA.GlobalResources.BusinessLayer.Resources.InsertPermission);
            }
            if (oProcess.sProcessName == string.Empty || oProcess.sProcessName == "")
            {
                throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException("Process Name " + BPA.GlobalResources.BusinessLayer.Resources.RequiredField);
            }*/
            InsertUpdate(oProcess, "A", oTenant);

        }

        public void UpdateData(BEProcessInfo oProcess, int FormID, BETenant oTenant)
        {
            /*
            if (!CheckPermission.hasPermission(FormID, oProcess.iCreatedBy, PermissionSet.UPDATE))
            {
                throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException(BPA.GlobalResources.BusinessLayer.Resources.UpdatePermission);
            }
            if (oProcess.sProcessName == string.Empty || oProcess.sProcessName == "")
            {
                throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException("Process Name " + BPA.GlobalResources.BusinessLayer.Resources.RequiredField);
            }*/

            InsertUpdate(oProcess, "M", oTenant);
        }

        public void InsertUpdate(BEProcessInfo oProcess, string ActionType, BETenant oTenant)
        {
            using (DLProcess objProcess = new DLProcess(oTenant))
            {
                objProcess.InsertUpdate(oProcess, GetQCAXML(oProcess, oTenant), GetERPPRocessXML(oProcess, oTenant), ActionType);
            }
        }

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
                using (XmlTextWriter writer = new XmlTextWriter(stream, Encoding.UTF8))
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

        public int CheckCalenderExistance(int ProcessId, DateTime StartDate, DateTime EndDate, int Type, BETenant oTenant)
        {
            using (DLProcess oProcess = new DLProcess(oTenant))
            {
                return oProcess.CheckCalenderExistance(ProcessId, StartDate, EndDate, Type);
            }
        }

        public List<BEProcessInfo> GetProcessList(int iProcessID, BETenant oTenant)
        {
            using (DLProcess objProcess = new DLProcess(oTenant))
            {
                return objProcess.GetProcessList(iProcessID);
            }
        }

        public List<BEProcessInfo> GetProcessList(int iLoggedinUserID, int iClientID, string ProcessName, bool bActiveProcess, BETenant oTenant)
        {
            using (DLProcess objProcess = new DLProcess(oTenant))
            {
                return objProcess.GetProcessList(iLoggedinUserID, iClientID, ProcessName, bActiveProcess);
            }
        }
        public List<BEProcessInfo> GetProcessList(int iLoggedinUserID, bool bActiveProcess, BETenant oTenant)
        {
            using (DLProcess objProcess = new DLProcess(oTenant))
            {
                return objProcess.GetProcessList(iLoggedinUserID, bActiveProcess);
            }
        }
        public List<BEERPProcess> GetERPProcessList(ArrayList aryDistinctERPProcessId, BETenant oTenant)
        {
            using (DLProcess objProcess = new DLProcess(oTenant))
            {
                return objProcess.GetERPProcessList(aryDistinctERPProcessId);
            }
        }
        public int CheckRoleForOrgProcess(int UserId, BETenant oTenant)
        {
            using (DLProcess oProcess = new DLProcess(oTenant))
            {
                return oProcess.CheckRoleForOrgProcess(UserId);
            }
        }
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
