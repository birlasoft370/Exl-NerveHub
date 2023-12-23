using BPA.GlobalResources;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MicUI.Configuration.Helper;
using MicUI.Configuration.Models.ViewModels;
using MicUI.Configuration.Module.Configuration.ClientInfoSetup;
using MicUI.Configuration.Module.Security;
using MicUI.Configuration.Services.ServiceModel;
using System.Data;

namespace MicUI.Configuration.Controllers
{
    public class ProcessOwnerController : BaseController
    {
        private readonly IRolesService _repositoryRole;
        private readonly IClientService _repositoryClient;
        private readonly IProcessOwnerService _processOwnerService;
        public ProcessOwnerController(IRolesService repositoryRole, IClientService repositoryClient, IProcessOwnerService processOwnerService)
        {

            _repositoryRole = repositoryRole;
            _repositoryClient = repositoryClient;
            _processOwnerService = processOwnerService;
        }
        public IActionResult Index()
        {
            return View(new ProcessOwnerViewModel());
        }
        public JsonResult GetUserRoleApproverList()
        {
            var dsApprover = _repositoryRole.PopulateRoleApproverDropDownList(int.Parse(BPA.GlobalResources.UI.AppConfiguration.Resources_ProcessOwner.display_RoleID));
            return Json(dsApprover);
        }
        public JsonResult GetCascadeClient()
        {
            return Json(_repositoryClient.GetClientList(true));
        }
        /// <summary>
        /// This is used to fetch the process list by the client id     
        /// </summary>
        /// <param name="ClientID">Client Id</param>
        /// <returns>Resturn the list of the Process</returns>
        [HttpGet]
        public JsonResult GetClientWiseProcessList(int iClientID)
        {
            List<BEProcessInfo> lProcessInfo;
            lProcessInfo = _processOwnerService.GetClientWiseProcessList(base.oUser.iUserID, iClientID);
            return Json(lProcessInfo);
        }
        public JsonResult GetUserProcessOwner(int? iProcessID)
        {

            IList<KeyValuePair<string, string>> lstApprover = new List<KeyValuePair<string, string>>();
            Object lstConsolidatedData = null;
            var dsApprover = UISharedLayer.ValidateDate(UISharedLayer.ToDataTable(_processOwnerService.GetUserProcessOwner(iProcessID.GetValueOrDefault())));
            if (dsApprover != null && dsApprover.Rows.Count > 0)
            {
                lstConsolidatedData = dsApprover.AsEnumerable().Select(r => r.Table.Columns.Cast<DataColumn>()
               .Select(c => new KeyValuePair<object, object>(c.ColumnName.Replace("[^a-zA-Z0-9]+", "").Replace(" ", ""), r[c.Ordinal])).
               ToDictionary(z => z.Key, z => z.Value)).ToList();
            }
            return Json(lstConsolidatedData);

        }
        private string ExistingUserRequest(int ProcessId, string ProcessOwner)
        {
            return _processOwnerService.ExistingUserRequest(ProcessId, ProcessOwner);
        }
        [HttpPost]
        public ActionResult Index(ProcessOwnerViewModel oProcessOwnerViewModel)
        {
            string ExistingUser = "";
            ExistingUser = ExistingUserRequest(Convert.ToInt32(oProcessOwnerViewModel.ProcessName), String.Join(",", oProcessOwnerViewModel.ProcessOwnerName).Trim());
            if (ExistingUser == "" || ExistingUser == null)
            {
                if (String.Join(",", oProcessOwnerViewModel.ProcessOwnerName).Trim() != "")
                {
                    int Roweffected = 0;
                    BEProcessInfo oProcessInfo = CatchRecord(oProcessOwnerViewModel);
                    oProcessOwnerViewModel.UserId = base.oUser.iUserID.ToString();
                    var str = _processOwnerService.CheckProcessOwnerApproverLevel(oProcessOwnerViewModel);
                    if (string.IsNullOrEmpty(Convert.ToString(str)))
                    {
                        Roweffected = _processOwnerService.SendApproveProcessReqest(oProcessOwnerViewModel, int.Parse(Resources_FormID.ProcessOwner), 0, 0);

                    }
                    else
                    {
                        ViewData["Message"] = str + BPA.GlobalResources.UI.AppConfiguration.Resources_ProcessOwner.display_Approverdesignationshouldbehigher;
                        return View(oProcessOwnerViewModel);
                    }
                }
                else
                {
                    ViewData["Message"] = BPA.GlobalResources.UI.AppConfiguration.Resources_ProcessOwner.display_PleaseSpecifyProcessOwner;
                    return View(oProcessOwnerViewModel);
                }
            }

            else
            {
                ViewData["Message"] = BPA.GlobalResources.UI.AppConfiguration.Resources_ProcessOwner.display_ProcessOwnerRequest + ExistingUser + BPA.GlobalResources.UI.AppConfiguration.Resources_ProcessOwner.display_stillPending;
                return View(oProcessOwnerViewModel);
            }
            return View(new ProcessOwnerViewModel());
        }
        private BEProcessInfo CatchRecord(ProcessOwnerViewModel oProcessOwnerViewModel)
        {
            BEProcessInfo objProcess = new BEProcessInfo();
            objProcess.iClientID = Convert.ToInt32(oProcessOwnerViewModel.ClientName);
            objProcess.iProcessID = Convert.ToInt32(oProcessOwnerViewModel.ProcessName);
            objProcess.sProcessOwner = Convert.ToString(oProcessOwnerViewModel.ProcessOwnerName);
            objProcess.iCreatedBy = base.oUser.iUserID;
           // objProcess.iApprover = Convert.ToInt32(oProcessOwnerViewModel.Approver);
            // objProcess.iApprover = Convert.ToInt32(oProcessOwnerViewModel.Approver);
            return objProcess;
        }
        [HttpGet]
        public ActionResult _ProcessOwnerApproval()
        {
            ProcessOwnerApproval oProcess = new ProcessOwnerApproval();
            oProcess.oTenant = base.oTenant;
            return View(oProcess);
        }
        public ActionResult ProcessApproval_ReadP([DataSourceRequest] DataSourceRequest request, string dFrom, string dTo)
        {
            List<ProcessOwnerApproval> lstProcessApp = null;

            List<ProcessApproval> lstProcessAppreport = _processOwnerService.GetPandingApproval(base.oUser.iUserID, dFrom, dTo);
             lstProcessApp = lstProcessAppreport.AsEnumerable().Select(x => new ProcessOwnerApproval()
            {
                RequestId = x.RequestId,
                ClientName = x.ClientName,
                ProcessName = x.ProcessName,
                Creater = x.Creater,
                Approver = x.Approver,
                CreateDate = Convert.ToDateTime(x.CreateDate),
                ForUser = x.ForUser,
                TransStatus = x.TransStatus,
            }).ToList();

            return Json(lstProcessApp.ToDataSourceResult(request));
        }
        public JsonResult GetUserProcess_Owner(int? iProcessID)
        {
            IList<string> lstApprover = new List<string>();
            Object lstConsolidatedData = null;
            var ApprovarList = _processOwnerService.GetUserProcessOwnerUserList(iProcessID.GetValueOrDefault());
            var dsApprover = UISharedLayer.ValidateDate(UISharedLayer.ToDataTable(ApprovarList.UserList));
            if (dsApprover != null && dsApprover.Rows.Count > 0)
            {
                lstConsolidatedData = dsApprover.AsEnumerable().Select(r => r.Table.Columns.Cast<DataColumn>()
             .Select(c => new KeyValuePair<object, object>(c.ColumnName.Replace("[^a-zA-Z0-9]+", "").Replace(" ", ""), r[c.Ordinal])).
             ToDictionary(z => z.Key, z => z.Value)).ToList();
                List<string> lst = new List<string>();
                // oCampaignViewModel.Mode.AddRange(oCamp.sModeIds.TrimEnd(',').Split(','));
                if (ApprovarList.UserIdList.Count > 0)
                {
                    foreach (var dr in ApprovarList.UserIdList)
                    {
                        lstApprover.Add(dr.ToString());
                    }


                }
            }
            return Json(lstApprover);

        }
    }
}
