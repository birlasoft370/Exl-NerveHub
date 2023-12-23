using BPA.EmailManagement.BusinessEntity.ExternalRef.Application;
using BPA.EmailManagement.BusinessEntity;
using BPA.EmailManagement.BusinessLayer.ExternalRef.WorkAllocation;
using BPA.EmailManagement.ServiceContract.ServiceContracts.WorkAllocation;
using BPA.EmailManagement.ServiceContract.ServiceContracts;
using BPA.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using BPA.EmailManagement.DataLayer;

namespace BPA.EmailManagement.BusinessLayer
{
    static class BLMailHelper
    {
        public static DataTable getUploadTable(IList<BEMailCampaignField> oMailFiled, bool IsSensitivity, bool NeedeTicket, bool DuringUploadeTicket)
        {

            DataTable datatable = new DataTable("UploadData");
            datatable.Columns.Add("MailConfigID", typeof(int));
            datatable.Columns.Add("MailFrom", typeof(string));
            datatable.Columns.Add("MailTo", typeof(string));
            datatable.Columns.Add("MailCC", typeof(string));
            datatable.Columns.Add("MailSubject", typeof(string));
            datatable.Columns.Add("MailReceivedDate", typeof(DateTime));
            datatable.Columns.Add("Getmaildetails", typeof(string));
            datatable.Columns.Add("MailFolderid", typeof(int));
            datatable.Columns.Add("MoveMailFolderID", typeof(int));
            datatable.Columns.Add("conversationid", typeof(string));
            datatable.Columns.Add("CategoriesVal", typeof(string));
            datatable.Columns.Add("ReceivedMail", typeof(string));
            datatable.Columns.Add("Importance", typeof(string));
            datatable.Columns.Add("AttachmentsCount", typeof(int));
            datatable.Columns.Add("ParentWorkId", typeof(int));
            datatable.Columns.Add("TOPID", typeof(int));
            if (NeedeTicket || DuringUploadeTicket)
            {
                datatable.Columns.Add("ReferenceNumber");
            }
            if (IsSensitivity)
            {
                datatable.Columns.Add("Sensitivity");
            }
            //  datatable.Columns.Add("EmployeeNumber", typeof(int));
            if (oMailFiled != null)
            {
                int icount = oMailFiled.Count;
                for (int i = 0; i < icount; i++)
                {
                    switch (oMailFiled[i].DataType)
                    {
                        case "BigInt":
                            datatable.Columns.Add(oMailFiled[i].ObjName, typeof(int));
                            break;
                        case "Boolean":
                            datatable.Columns.Add(oMailFiled[i].ObjName, typeof(bool));
                            break;
                        case "Character":
                            datatable.Columns.Add(oMailFiled[i].ObjName, typeof(string));
                            break;
                        case "DateTime":
                            datatable.Columns.Add(oMailFiled[i].ObjName, typeof(DateTime));
                            break;
                        case "Integer":
                            datatable.Columns.Add(oMailFiled[i].ObjName, typeof(int));
                            break;
                    }


                }
            }
            return datatable;
        }

        public static string GetBatchCodeName(BatchFrequencyType batchFrequency)
        {
            string batchName = "MailUploaded_";
            switch (batchFrequency)
            {
                case BatchFrequencyType.Daily:
                    batchName += DateTime.Now.Day.ToString() + "_" + DateTime.Now.ToString("MMM") + "_" + DateTime.Now.ToString("yyyy");
                    break;
                case BatchFrequencyType.Weekly:
                    var day = (int)CultureInfo.CurrentCulture.Calendar.GetDayOfWeek(DateTime.Now);

                    batchName += "wk" + CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(DateTime.Now.AddDays(4 - (day == 0 ? 7 : day)), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + "_" + DateTime.Now.ToString("MMM") + "_" + DateTime.Now.ToString("yyyy");
                    break;
                case BatchFrequencyType.Fortnightly:
                    if (DateTime.Now.Day < 15)
                    { batchName += "FirstHalf_" + DateTime.Now.ToString("MMM") + "_" + DateTime.Now.ToString("yyyy"); }
                    else
                    { batchName += "SecondHalf_" + DateTime.Now.ToString("MMM") + "_" + DateTime.Now.ToString("yyyy"); }

                    break;
                case BatchFrequencyType.Monthly:
                    batchName += DateTime.Now.ToString("MMM") + "_" + DateTime.Now.ToString("yyyy");
                    break;
                case BatchFrequencyType.Quarterly:
                    double quarter = Math.Ceiling((double.Parse(DateTime.Today.Month.ToString()) + 2) / 3);
                    batchName += "Q" + quarter.ToString() + "_" + DateTime.Now.ToString("yyyy");
                    break;
                case BatchFrequencyType.HalfYearly:
                    if (DateTime.Now.Month < 6)
                    { batchName += "H1_" + DateTime.Now.ToString("yyyy"); }
                    else
                    { batchName += "H2_" + DateTime.Now.ToString("yyyy"); }

                    break;
                case BatchFrequencyType.Yearly:
                    batchName += DateTime.Now.ToString("yyyy");
                    break;
                default:
                    batchName += DateTime.Now.ToString("MMM") + "_" + DateTime.Now.ToString("yyyy");
                    break;

            }
            return batchName;
        }

        public static bool isExistWorkDetails(string Subject)
        {
            bool checkValue = false;

            string strActualImgTag = "";
            int start = 0;
            start = Subject.IndexOf("[", start);
            while (start >= 0)
            {
                int count = Subject.Substring(start).IndexOf(']');
                if (count > 1)
                {
                    strActualImgTag = Subject.Substring(start).Substring(1, count - 1);
                    break;
                }
            }
            try
            {
                if (!string.IsNullOrWhiteSpace(strActualImgTag))
                {
                    checkValue = strActualImgTag.Contains("####");
                    string[] str = strActualImgTag.Replace("#", "").Split('|');
                    int iWorkID = int.Parse(str[0]);
                }
            }
            catch (Exception ex)
            {
                checkValue = isExistWorkDetails(Subject.Substring(Subject.IndexOf(']') + 1));
            }
            return checkValue;
        }

        public static emaildetail GetWorkIDFromSubject(string Subject)
        {
            emaildetail oemp = new emaildetail();

            string strActualImgTag = "";
            int start = 0;
            start = Subject.IndexOf("[", start);
            while (start >= 0)
            {
                int count = Subject.Substring(start).IndexOf(']');
                if (count > 1)
                {
                    strActualImgTag = Subject.Substring(start).Substring(1, count - 1);
                    break;
                }
            }
            try
            {
                if (!string.IsNullOrWhiteSpace(strActualImgTag))
                {
                    string[] str = strActualImgTag.Replace("#", "").Split('|');
                    oemp.iWorkID = int.Parse(str[0]);
                    if (str.Length > 1) oemp.iEmployeeID = int.Parse(str[1]);
                    if (str.Length > 2) oemp.iDStoreID = int.Parse(str[2]);
                }
            }
            catch
            {
                oemp = GetWorkIDFromSubject(Subject.Substring(Subject.IndexOf(']') + 1));
            }
            return oemp;
        }

        public static DataSet GetVendorColumnDetails(string CampId)
        {
            System.Reflection.Assembly ass = System.Reflection.Assembly.GetExecutingAssembly();
            string codeBase = System.IO.Path.GetDirectoryName(ass.CodeBase);
            System.Uri uri = new Uri(codeBase);

            var DirectoryPath = uri.LocalPath.Replace(Path.DirectorySeparatorChar.ToString() + "bin", "").Replace(Path.DirectorySeparatorChar.ToString() + "Debug", "") + Path.DirectorySeparatorChar.ToString() + "VendorDetails.xml";
            DataSet ds = new DataSet();
            XDocument xdoc = XDocument.Load(DirectoryPath);
            foreach (var row in xdoc.Descendants("VendorCampDetails").Where(p => p.Attribute("CampId").Value.Equals(CampId)))
            {
                foreach (XElement element in row.Elements())
                {
                    ds.Tables.Add(CreateTableFrom(element));

                }
            }
            return ds;
        }

        private static DataTable CreateTableFrom(XElement report)
        {
            DataTable table = new DataTable(report.Name.LocalName);
            table.Columns.AddRange(GetColumnsOf(report));

            foreach (var row in report.Descendants("Fields"))
            {
                DataRow dr = table.NewRow();
                foreach (var field in row.Elements())
                    dr[field.Name.LocalName] = (string)field;
                table.Rows.Add(dr);
            }

            return table;
        }

        private static DataColumn[] GetColumnsOf(XElement report)
        {
            return report.Descendants("Fields")
                         .SelectMany(row => row.Elements().Select(e => e.Name.LocalName))
                         .Distinct()
                         .Select(field => new DataColumn(field))
                         .ToArray();
        }

        public static DataTable AddData(DataTable ds, int iCreatedBy, int iBatchID, string sBatchName, bool IsFreshRequired, bool NeedeTicket, bool DuringUploadeTicket)
        {
            DataTable myDataSet = ds;
            try
            {

                if (!myDataSet.Columns.Contains("BatchCode") && !string.IsNullOrEmpty(sBatchName)) myDataSet.Columns.Add("BatchCode");
                if (!myDataSet.Columns.Contains("BatchID")) myDataSet.Columns.Add("BatchID");
                if (!myDataSet.Columns.Contains("Version")) myDataSet.Columns.Add("Version");
                if (!myDataSet.Columns.Contains("CreatedBy")) myDataSet.Columns.Add("CreatedBy");
                int mailcount = myDataSet.Rows.Count;
                for (int i = 0; i < mailcount; i++)
                {
                    myDataSet.Rows[i]["CreatedBy"] = iCreatedBy.ToString();
                    if (NeedeTicket || DuringUploadeTicket)
                    {
                        if (myDataSet.Rows[i]["TOPID"] == null || string.IsNullOrEmpty(myDataSet.Rows[i]["TOPID"].ToString()))
                        {
                            if (!string.IsNullOrEmpty(sBatchName)) { myDataSet.Rows[i]["BatchCode"] = sBatchName; }
                            myDataSet.Rows[i]["BatchID"] = iBatchID;
                        }
                        else if (myDataSet.Rows[i]["TOPID"] != null)
                        {
                            if (IsFreshRequired == true)//IsAssignLast == false && 
                            {
                                if (!string.IsNullOrEmpty(sBatchName)) { myDataSet.Rows[i]["BatchCode"] = sBatchName; }
                                myDataSet.Rows[i]["BatchID"] = iBatchID;
                            }
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(sBatchName)) { myDataSet.Rows[i]["BatchCode"] = sBatchName; }
                        myDataSet.Rows[i]["BatchID"] = iBatchID;
                    }

                    myDataSet.Rows[i]["Version"] = "0";

                }
                myDataSet.AcceptChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ds;
        }

        public static DataTable GetWorkID(DataTable dtWork, string sTableName, BETenant oTenant, out string sWorkIDList, IList<BEMailCampaignField> additionColumnName)
        {

            sWorkIDList = "";
            string sColnameList = "";
            DataTable dttemp;
            if (dtWork.Rows.Count > 0)
            {
                int icount = 0;
                if (additionColumnName != null)
                {
                    icount = additionColumnName.Count;
                    for (int i = 0; i < icount; i++)
                    {
                        sColnameList += "," + additionColumnName[i].ObjName;
                    }
                }
                foreach (DataRow dr in dtWork.Rows)
                {
                    if (dr["ParentWorkID"] == DBNull.Value) continue;
                    sWorkIDList += dr["ParentWorkID"] + ",";
                }
                sWorkIDList += "0";
                using (DLMailScheduler oSchedular = new DLMailScheduler(oTenant))
                {
                    dttemp = oSchedular.GetWorkDetails(sWorkIDList, sTableName, sColnameList);
                }
                if (dttemp == null)
                {
                    return dtWork;
                }
                if (dttemp != null && dttemp.Rows.Count == 0)
                {
                    return dtWork;
                }
                foreach (DataRow dr in dtWork.Rows)
                {
                    if (dr["ParentWorkID"] == DBNull.Value) continue;
                    DataRow[] drt = dttemp.Select("oldWorkID =" + dr["ParentWorkID"]);
                    if (drt.Count() > 0)
                    {
                        dr["ParentWorkID"] = drt[0]["ParentID"].ToString();
                        dr["TOPID"] = drt[0]["TOPID"].ToString();
                        dr["EmployeeNumber"] = Convert.ToString(drt[0]["EmployeeNumber"]);

                        for (int i = 0; i < icount; i++)
                        {
                            switch (additionColumnName[i].DataType)
                            {
                                case "BigInt":
                                    if (!string.IsNullOrEmpty(drt[0][additionColumnName[i].ObjName].ToString()))
                                    {
                                        dr[additionColumnName[i].ObjName] = int.Parse(drt[0][additionColumnName[i].ObjName].ToString());
                                    }
                                    break;
                                case "Boolean":
                                    if (!string.IsNullOrEmpty(drt[0][additionColumnName[i].ObjName].ToString()))
                                    {
                                        dr[additionColumnName[i].ObjName] = bool.Parse(drt[0][additionColumnName[i].ObjName].ToString());
                                    }
                                    break;
                                case "Character":
                                    dr[additionColumnName[i].ObjName] = drt[0][additionColumnName[i].ObjName].ToString();
                                    break;
                                case "DateTime":
                                    if (!string.IsNullOrEmpty(drt[0][additionColumnName[i].ObjName].ToString()))
                                    {
                                        dr[additionColumnName[i].ObjName] = DateTime.Parse(drt[0][additionColumnName[i].ObjName].ToString());
                                    }
                                    break;
                                case "Integer":
                                    if (!string.IsNullOrEmpty(drt[0][additionColumnName[i].ObjName].ToString()))
                                    {
                                        dr[additionColumnName[i].ObjName] = int.Parse(drt[0][additionColumnName[i].ObjName].ToString());
                                    }
                                    break;
                            }


                        }

                    }
                }

            }
            return dtWork;
        }

        public static void BulkCopyDataUpload(BEMailConfiguration listConfigDetails, DataTable mydt, int iFolderID, string TableName, BETenant oTenant, string sWorkIDList)
        {
            string strmsg = "";
            try
            {
                if (mydt.Rows.Count > 0)
                {

                    using (IWorkUploadService oWorkUpload = new BLWorkUpload())
                    {
                        TimeZoneInfo tz = TimeZoneInfo.Local;
                        if (!string.IsNullOrEmpty(listConfigDetails.oTimeZone.sTimeZoneID))
                        {

                            tz = TimeZoneInfo.FindSystemTimeZoneById(listConfigDetails.oTimeZone.sTimeZoneID);
                            mydt = DateTimeTimeZoneConversion.AdjustTimeZone(mydt, tz.StandardName, listConfigDetails.sServerTimeZone);

                        }

                        strmsg = oWorkUpload.BulkUpload(listConfigDetails.iCampaignID, mydt, TableName, oTenant);

                        if (!string.IsNullOrEmpty(strmsg))
                        {
                            //throw new BPA.ExceptionHandler.ExceptionType.BusinessLogicException(" Column not match" + strmsg);//issue
                        }
                        if (!string.IsNullOrEmpty(sWorkIDList))
                        {
                            using (DLMailScheduler oSchedular = new DLMailScheduler(oTenant))
                            {
                                oSchedular.UpdateWorkTable(sWorkIDList, TableName);
                            }
                        }
                        InsertMaxRecieveDatetime(mydt, listConfigDetails, iFolderID, oTenant);
                    }


                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public static DataTable GetWorkAutoMailID(string sTableName, BETenant oTenant, string sWorkIDList)
        {
            DataTable dttemp;
            using (DLMailScheduler oSchedular = new DLMailScheduler(oTenant))
            {
                dttemp = oSchedular.GetWorkAutoMailID(sWorkIDList, sTableName);
            }
            return dttemp;
        }

        public static string AddHeadersToMailBody(BEMailHelper objEmail, string trimmedBody)
        {
            bool isSendmail = true;

            string headers = "";
            if (!string.IsNullOrEmpty(objEmail.FromAddress))
            {
                if (objEmail.MsgSender != null)
                {
                    if (objEmail.MsgSender.Equals(objEmail.FromAddress))
                    {
                        headers = "<b>From: </b>" + (string.IsNullOrEmpty(objEmail.FromAddressWithDisplayName) ? objEmail.FromAddress : objEmail.FromAddressWithDisplayName) + "<br/>";
                    }
                    else
                    {
                        headers = "<b>From: </b>" + (string.IsNullOrEmpty(objEmail.MsgSenderDisplayName) ? objEmail.MsgSender : objEmail.MsgSenderDisplayName) + "  on behalf of " + (string.IsNullOrEmpty(objEmail.FromAddressWithDisplayName) ? objEmail.FromAddress : objEmail.FromAddressWithDisplayName) + "<br/>";
                    }
                }
                else
                {
                    headers = "<b>From: </b>" + (string.IsNullOrEmpty(objEmail.FromAddressWithDisplayName) ? objEmail.FromAddress : objEmail.FromAddressWithDisplayName) + "<br/>";
                }

            }
            if (objEmail.Msg_DateReceived != DateTime.MinValue)
            {
                headers += "<b>Sent: </b>" + string.Format(new CustomDateProvider(), "{0}", objEmail.Msg_DateReceived) + "<br/>";
            }
            if (objEmail.MailToRecipients != null && objEmail.MailToRecipients.Count > 0)
            {
                headers += "<b>To: </b>";
                foreach (string address in objEmail.MailToRecipients)
                {
                    headers += address + "; ";
                }

                headers += "<br />";
            }

            if (objEmail.MailCCRecipients != null && objEmail.MailCCRecipients.Count > 0)
            {
                headers += "<b>CC: </b>";
                foreach (string address in objEmail.MailCCRecipients)
                {
                    headers += address + "; ";
                }
                headers += "<br />";
            }

            if (!string.IsNullOrEmpty(objEmail.MsgSubject))
            {
                headers += "<b>Subject: </b>" + objEmail.MsgSubject + "<br/>";
            }
            if (objEmail.sSensitivity != null)
            {
                switch (objEmail.sSensitivity.ToUpper())
                {
                    case "PERSONAL":
                        headers += isSendmail ? @"<b>Sensitivity:</b> Personal <br/>" : @"Please treat this as Personal. <br/>";
                        break;
                    case "PRIVATE":
                        headers += isSendmail ? @"<b>Sensitivity:</b> Private <br/>" : @"Please treat this as Private. <br/>";
                        break;
                    case "CONFIDENTIAL":
                        headers += isSendmail ? @"<b>Sensitivity:</b> Confidential <br/>" : @"Please treat this as Confidential. <br/>";
                        break;
                    default:
                        break;
                }
            }
            if (objEmail.sImportant != null)
            {
                if (objEmail.sImportant.ToUpper() == "HIGH")
                    headers += isSendmail ? @"<b>Importance:</b> High <br/>" : @"This message was sent with High importance. <br/>";
                else if (objEmail.sImportant.ToUpper() == "LOW")
                    headers += isSendmail ? @"<b>Importance:</b> Low <br/>" : @"This message was sent with Low importance. <br/>";
            }


            int start = 0;
            string strActualImgTag = "<body>";
            start = trimmedBody.IndexOf("<body", start);
            while (start > 0)
            {
                string str = trimmedBody.Substring(start);
                strActualImgTag = str.Substring(0, str.IndexOf(">") + 1);
                start = 0;
            }
            trimmedBody = trimmedBody.Replace(strActualImgTag, strActualImgTag + headers + "<br/>");

            return trimmedBody;

        }

        private static void InsertMaxRecieveDatetime(DataTable uploadtable, BEMailConfiguration mailConfig, int iFolderID, BETenant oTenant)
        {
            DateTime maxDatetime = DateTime.Now;
            DataView dv = uploadtable.DefaultView;
            dv.RowFilter = "MailReceivedDate=Max(MailReceivedDate)";
            using (IMailConfigurationService IMailConfig = new BLMailConfiguration())
            {
                IMailConfig.InsertRecieveDateTime(mailConfig.iCampaignID, mailConfig.iMailConfigID, iFolderID, dv[0][4].ToString(), DateTime.Parse(dv[0][5].ToString()), dv[0][6].ToString(), oTenant);
            }

        }
    }

    class emaildetail
    {
        public int iWorkID { get; set; }
        public int iEmployeeID { get; set; }

        public int iDStoreID { get; set; }
    }

    public class CustomDateProvider : IFormatProvider, ICustomFormatter
    {
        public object GetFormat(Type formatType)
        {
            if (formatType == typeof(ICustomFormatter))
                return this;

            return null;
        }

        public string Format(string format, object arg, IFormatProvider formatProvider)
        {
            if (!(arg is DateTime)) throw new NotSupportedException();

            var dt = (DateTime)arg;

            string suffix = "";

            //if (dt.Day == 11 | dt.Day == 12 | dt.Day == 13) 
            //{
            //    suffix = "th";
            //}
            //else if (dt.Day % 10 == 1)
            //{
            //    suffix = "st";
            //}
            //else if (dt.Day % 10 == 2)
            //{
            //    suffix = "nd";
            //}
            //else if (dt.Day % 10 == 3)
            //{
            //    suffix = "rd";
            //}
            //else
            //{
            //    suffix = "th";
            //}


            return string.Format("{0:dddd}, {0:MMMM} {1}{2}, {0:yyyy hh:mm tt}", arg, dt.Day, suffix);
        }
    }
}
