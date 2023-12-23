using BPA.GlobalResources;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MicUI.Configuration.Helper;
using MicUI.Configuration.Models.ViewModels;
using MicUI.Configuration.Module.Configuration.ClientInfoSetup;
using MicUI.Configuration.Module.Configuration.ProcessInfoSetup;
using MicUI.Configuration.Module.Reports;
using MicUI.Configuration.Module.Security;
using MicUI.Configuration.Services.ServiceModel;
using System.Data;

namespace MicUI.Configuration.Controllers
{
    public class ReportsController : BaseController
    {
        private readonly IERPJobRoleMapService _repositoryERPJobRoleMap;
        private readonly IReportsService _repositoryReports;
        private readonly IClientService _repositoryClient;
        private readonly IProcessService _repositoryProcess;
        private readonly IRolesService _repositoryRole;
        private readonly IPermissionService _repositoryPermission;

        public ReportsController(IERPJobRoleMapService repositoryERPJobRoleMap, IReportsService repositoryReports, IClientService repositoryClient, IProcessService repositoryProcess, IRolesService repositoryRole, IPermissionService repositoryPermission)
        {
            _repositoryERPJobRoleMap = repositoryERPJobRoleMap;
            _repositoryReports = repositoryReports;
            _repositoryClient = repositoryClient;
            _repositoryProcess = repositoryProcess;
            _repositoryRole = repositoryRole;
            _repositoryPermission = repositoryPermission;
        }

        public IActionResult Index()
        {
            return View();
        }

        #region Access Control Report
        /// <summary>
        /// Access  Report Action Method
        /// </summary>
        /// <returns></returns>
        public ActionResult Rpt_AccessReport()
        {
            //it's Check the User Permission
            if (!base.CheckViewPermission(int.Parse(Resources_FormID.AccessReport)))
                return RedirectToActionPermanent("AccessDenied", "Error");
            else
                return View(new ReportsConfigViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult GetAccessControlReport(string[] ClientID, string[] ProcessID, string[] RoleID)
        {
            //if (ClientID == "") ClientID = "0"; if (ProcessID == "") ProcessID = "0";
            //if (RoleID == "") RoleID = "0";
            string ClientIDs = string.Join(",", ClientID);
            string ProcessIDs = string.Join(",", ProcessID);
            string RoleIDs = string.Join(",", RoleID);
            Object ObjAccessReport = 0;
            List<object> lstObject = new List<object>();
            DataSet ds = new DataSet();
            var dsListAccessCntlRpt = _repositoryReports.GetMultiACLReport(ClientIDs, ProcessIDs, "0", RoleIDs);
            ds.Tables.Add(UISharedLayer.ValidateDate(UISharedLayer.ToDataTable(dsListAccessCntlRpt)));
            ds = GetDataSetHeader_Local(ds);
            //SA: Changes to compensate time zone differences                            
            ds = DateTimeTimeZoneConversion.MakeClientData(ds);
            foreach (DataTable dt in ds.Tables)
            {
                if (dt.Rows.Count > 0)
                {
                    ObjAccessReport = dt.AsEnumerable().Select(r => r.Table.Columns.Cast<DataColumn>()
                    .Select(c => new KeyValuePair<string, string>(c.ColumnName, r[c.Ordinal].ToString())).ToDictionary(z => z.Key, z => z.Value)).ToList();
                    string[] columnNames = dt.Columns.Cast<DataColumn>().Select(x => x.Caption).ToArray();
                }
            }

            return Json(ObjAccessReport);
        }

        #endregion

        #region ERPRoleJobMappingReport

        /// <summary>
        /// ERP Role Job Mapping Report Action Method
        /// </summary>
        /// <returns></returns>
        public ActionResult Rpt_ERPRoleJobMappingReport()
        {
            //it's Check the User Permission

            if (!base.CheckViewPermission(int.Parse(Resources_FormID.ERPRoleJobMappingReport)))
                return RedirectToActionPermanent("AccessDenied", "Error");
            else
                return View(new ReportsConfigViewModel());
        }

        public JsonResult GetJob()
        {
            return Json(_repositoryERPJobRoleMap.GetJob());
        }

        public JsonResult GetERPJobRoleMappingReport([DataSourceRequest] DataSourceRequest request, string ERPJob)
        {
            Object ObjERPJobRoleMappingReport = null;
            List<object> lstObject = new List<object>();
            if (!string.IsNullOrEmpty(ERPJob))
            {
                DataSet ds = new DataSet();
                List<BEErpJobRoleMap> List = _repositoryERPJobRoleMap.GetJobRoleMap(int.Parse(ERPJob));
                ds.Tables.Add(UISharedLayer.ValidateDate(UISharedLayer.ToDataTable(List)));
                ds = GetDataSetHeader_Local(ds);
                foreach (DataTable dt in ds.Tables)
                {
                    ObjERPJobRoleMappingReport = dt.AsEnumerable().Select(r => r.Table.Columns.Cast<DataColumn>()
                    .Select(c => new KeyValuePair<string, string>(c.ColumnName, r[c.Ordinal].ToString())).ToDictionary(z => z.Key, z => z.Value)).ToList();
                    string[] columnNames = dt.Columns.Cast<DataColumn>().Select(x => x.Caption).ToArray();
                }

                return Json(ObjERPJobRoleMappingReport);
            }
            else
            {
                return Json(lstObject);
            }
        }
        #endregion

        public DataSet GetDataSetHeader_Local(DataSet ds)
        {
            foreach (DataTable dtable in ds.Tables)
            {
                foreach (DataColumn item in dtable.Columns)
                {
                    string columnTittle = item.ColumnName;
                    // item.ColumnName = Regex.Replace(item.ColumnName, "[^a-zA-Z]+", " ").Trim().Replace(" ", "_");
                    item.ColumnName = item.ColumnName.Trim().Replace(" ", "_");
                    item.Caption = columnTittle;
                }
            }
            return ds;

        }

        #region Process Owner Request Status Report

        /// <summary>
        /// Process Owner Request Status   Report Action Method
        /// </summary>
        /// <returns></returns>
        public ActionResult Rpt_ProcessOwnerRequestStatusReport()
        {
            //it's Check the User Permission
            if (!base.CheckViewPermission(int.Parse(Resources_FormID.ProcessOwnerRequestStatusReport)))
                return RedirectToActionPermanent("AccessDenied", "Error");
            else
                return View(new ReportsConfigViewModel());
        }

        [HttpPost]
        public JsonResult GetProcessOwnerApprovalReport([DataSourceRequest] DataSourceRequest request, string StartDate, string EndDate, string Status)
        {

            Object ObjProcessOwnerApprovalReport = null;
            DataSet ds = new DataSet();
            var listProcessOwnerApproval = _repositoryReports.GetProcessOwnerApprovalReport(StartDate, EndDate, Status);
            ds.Tables.Add(UISharedLayer.ValidateDate(UISharedLayer.ToDataTable(listProcessOwnerApproval)));
            ds = GetDataSetHeader_Local(ds);
            foreach (DataTable dt in ds.Tables)
            {
                ObjProcessOwnerApprovalReport = dt.AsEnumerable().Select(r => r.Table.Columns.Cast<DataColumn>()
                .Select(c => new KeyValuePair<string, string>(c.ColumnName, r[c.Ordinal].ToString())).ToDictionary(z => z.Key, z => z.Value)).ToList();
                string[] columnNames = dt.Columns.Cast<DataColumn>().Select(x => x.Caption).ToArray();
            }

            return Json(ObjProcessOwnerApprovalReport);
        }

        #endregion

        #region ProcessOwnerReport

        /// <summary>
        /// Process Owner Report Action Method
        /// </summary>
        /// <returns></returns>
        public ActionResult Rpt_ProcessOwnerReport()
        {
            //it's Check the User Permission
            if (!base.CheckViewPermission(int.Parse(Resources_FormID.ProcessOwnerReport)))
                return RedirectToActionPermanent("AccessDenied", "Error");
            else
                return View(new ReportsConfigViewModel());
        }

    
       


        [HttpPost]
        public JsonResult GetProcessOwnerReport([DataSourceRequest] DataSourceRequest request, string ClientID, string ProcessID)
        {
            Object ObjProcessOwnerReport = null;
            //List<ProcessOwnerReport> lstObject = new List<ProcessOwnerReport>();
            DataSet ds = new DataSet();
            var lstObject = _repositoryReports.GetProcessOwnerReport(Int32.Parse(ClientID), Int32.Parse(ProcessID));
            ds.Tables.Add(UISharedLayer.ValidateDate(UISharedLayer.ToDataTable(lstObject)));
            ds = GetDataSetHeader_Local(ds);
            foreach (DataTable dt in ds.Tables)
            {
                ObjProcessOwnerReport = dt.AsEnumerable().Select(r => r.Table.Columns.Cast<DataColumn>()
                .Select(c => new KeyValuePair<string, string>(c.ColumnName, r[c.Ordinal].ToString())).ToDictionary(z => z.Key, z => z.Value)).ToList();
                string[] columnNames = dt.Columns.Cast<DataColumn>().Select(x => x.Caption).ToArray();
            }
            return Json(ObjProcessOwnerReport);
        }

        #endregion

        [HttpGet]
        public JsonResult GetCascadeClient()
        {
            return Json(_repositoryClient.GetClientList(true, null));
        }
        public IActionResult GetClient()
        {
            try
            {
                List<BEClientInfo> clientList = _repositoryClient.GetClientList(true, null);
                return Json(clientList);
            }
            catch (Exception)
            {



                throw;
            }

        }

        [HttpGet]
        public JsonResult GetProcess()
        {
            List<BEProcessInfo> lProcessInfo;
            lProcessInfo = _repositoryProcess.GetProcessList();
            return Json(lProcessInfo);
        }

        public JsonResult GetRole()
        {
            return Json(_repositoryRole.GetRoleList());
        }


        #region LogReport

        public ActionResult Rpt_LogReport()
        {
            //it's Check the User Permission
            //if (!base.CheckViewPermission(int.Parse(Resources_FormID.ProcessOwnerReport)))
            //    return RedirectToActionPermanent("AccessDenied", "Error");
            //else
            return View(new ReportsConfigViewModel());
        }

        public JsonResult GetExceptionHandlerReport([DataSourceRequest] DataSourceRequest request, string StartDate, string EndDate, string SeverityFlag)
        {
            List<ReportsConfigViewModel> lstval = new List<ReportsConfigViewModel>();

            if (!string.IsNullOrWhiteSpace(SeverityFlag))
            {
                var listds = _repositoryReports.GetLogDetailsReport(StartDate, EndDate, SeverityFlag);

                lstval = listds.Select(dr => new ReportsConfigViewModel()
                {
                    LogID = dr.LogID.ToString(),
                    Severity = dr.Severity.ToString(),
                    MachineName = dr.MachineName.ToString(),
                    Timestamp = dr.Timestamp.ToString(),
                    Message_cut = dr.Message_cut.ToString(),
                    AppDomainName = dr.AppDomainName.ToString(),
                    ProcessID = dr.ProcessID.ToString(),
                    ThreadName = dr.ThreadName ?? "",
                    Win32ThreadId = dr.Win32ThreadId.ToString(),
                    FormattedMessage = dr.FormattedMessage.ToString()
                }).ToList();
            }
            return Json(lstval.ToDataSourceResult(request));
        }

        public JsonResult JsonGetSeverity()
        {
            return Json(GetSeverity());
        }
        public SelectList GetSeverity()
        {

            IList<SelectListItem> Severity = new List<SelectListItem>();

            Severity.Add(new SelectListItem { Text = "All", Value = "ALL" });
            Severity.Add(new SelectListItem { Text = "Error", Value = "Error" });
            Severity.Add(new SelectListItem { Text = "Information", Value = "Information" });

            return new SelectList(Severity, "Value", "Text");

        }
        public JsonResult GetExceptionHandlerReport_Message(int LogID)
        {
            string MsgDetails = "";
            var detailedLogMsg = _repositoryReports.GetMessageDetails(LogID);
            MsgDetails = detailedLogMsg.Message.ToString();
            return Json(MsgDetails);
        }

        #endregion


        #region RoleAccess

        //public JsonResult GetRole()
        //{

        //    return Json(_repositoryERPJobRoleMap.GetRoleList(true));
        //}

        public ActionResult Rpt_RoleFormAccessReport()
        {
            //it's Check the User Permission
            if (!base.CheckViewPermission(int.Parse(Resources_FormID.RoleFormAccessReport)))
                return RedirectToActionPermanent("AccessDenied", "Error");
            else
                return View(new ReportsConfigViewModel());
        }

        public ActionResult Rpt_UserAccessRequestStatusReport()
        {
            //it's Check the User Permission
            if (!base.CheckViewPermission(int.Parse(Resources_FormID.UserAccessRequestStatusReport)))
                return RedirectToActionPermanent("AccessDenied", "Error");
            else
                return View(new ReportsConfigViewModel());
        }


        [HttpPost]
     
        public JsonResult GetRoleFormAccessReport(string[] Role)
        {

            Object ObjERPJobRoleMappingReport = 0;
            if (Role.Length > 0)
            {
                string RoleIDs = string.Join(",", Role);
                List<object> lstObject = new List<object>();
                DataSet ds = new DataSet();
                List<RoleFormAccessModel> List = _repositoryERPJobRoleMap.GetRoleFormMap(RoleIDs);
                ds.Tables.Add(UISharedLayer.ValidateDate(UISharedLayer.ToDataTable(List)));
                ds = GetDataSetHeader_Local(ds);
                //SA: Changes to compensate time zone differences                            
                ds = DateTimeTimeZoneConversion.MakeClientData(ds);
                foreach (DataTable dt in ds.Tables)
                {
                    ObjERPJobRoleMappingReport = dt.AsEnumerable().Select(r => r.Table.Columns.Cast<DataColumn>()
                    .Select(c => new KeyValuePair<string, string>(c.ColumnName, r[c.Ordinal].ToString())).ToDictionary(z => z.Key, z => z.Value)).ToList();
                    string[] columnNames = dt.Columns.Cast<DataColumn>().Select(x => x.Caption).ToArray();
                }
            }
            return Json(ObjERPJobRoleMappingReport);
        }


        [HttpGet]
        public JsonResult GetRequestedFor(string SearchText)
        {
           // SearchText = SearchText == "" ? "***" : SearchText;
            if (SearchText != "")
            {
                return Json(_repositoryPermission.GetRequestUserList(SearchText, true, base.oUser.iUserID, ""));
            }
            return null;
        }
        [HttpGet]
        public JsonResult GetRequestedBy(string SearchText = "")
        {
            //SearchText = SearchText == "" ? "***" : SearchText;
            if (SearchText != "")
            {
                return Json(_repositoryPermission.GetRequestUserList(SearchText, true, base.oUser.iUserID, null));
            }
            return null;
        }

        [HttpPost]
        public JsonResult GetUserAccessRequestStatusReport(string StartDate, string EndDate, string RequestedFor, string RequestedBy, string RequestedStatus)
        {
            RequestedFor = RequestedFor == "" ? "0" : RequestedFor;
            RequestedBy = RequestedBy == "" ? "0" : RequestedBy;

            Object ObjProcessOwnerReport = null;
            List<object> lstObject = new List<object>();
            DataSet ds = new DataSet();
            ds.Tables.Add(UISharedLayer.ValidateDate(UISharedLayer.ToDataTable(_repositoryReports.GetUserAccessRequestStatusReport(base.oUser.iUserID, StartDate, EndDate, Int32.Parse(RequestedFor), Int32.Parse(RequestedBy), Int32.Parse(RequestedStatus == "" ? "0" : RequestedStatus)))));
            ds = GetDataSetHeader_Local(ds);
            //SA: Changes to compensate time zone differences                            
            ds = DateTimeTimeZoneConversion.MakeClientData(ds);
            foreach (DataTable dt in ds.Tables)
            {
                ObjProcessOwnerReport = dt.AsEnumerable().Select(r => r.Table.Columns.Cast<DataColumn>()
                .Select(c => new KeyValuePair<string, string>(c.ColumnName, r[c.Ordinal].ToString())).ToDictionary(z => z.Key, z => z.Value)).ToList();
                string[] columnNames = dt.Columns.Cast<DataColumn>().Select(x => x.Caption).ToArray();
            }
            return Json(ObjProcessOwnerReport);
        }

        #endregion
    }
}
