using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Security.Application;
using MicUI.Configuration.Helper;
using MicUI.Configuration.Models;
using MicUI.Configuration.Models.ViewModels;
using MicUI.Configuration.Module.Administration.Facility;
using MicUI.Configuration.Module.Administration.LOB;
using MicUI.Configuration.Module.Authentication;
using MicUI.Configuration.Module.Configuration.CampaignInfoSetup;
using MicUI.Configuration.Module.Configuration.ClientInfoSetup;
using MicUI.Configuration.Module.Configuration.ProcessInfoSetup;
using MicUI.Configuration.Module.Security;
using MicUI.Configuration.Module.Security.UserMaster;
using MicUI.Configuration.Services.ServiceModel;
using Newtonsoft.Json;
using System.Data;

namespace MicUI.Configuration.Controllers
{
    public class UserManagementController : BaseController
    {
        private readonly IRolesService _repositoryRole;
        private readonly IFacilityService _repositoryFacility;
        private readonly ILOBService _repositoryLOB;
        private readonly ISBUInfoService _repositorySBUInfo;
        private readonly IPermissionService _repositoryPermission;
        private readonly IAuthenticationTokenService _repositoryAuthenticate;
        private readonly IUserAccessRequestService _repositoryUserAccess;
        private readonly IProcessService _repositoryProcess;
        private readonly IClientService _repositoryClient;
        private readonly ICampaignService _repositoryCampaign;

        public UserManagementController(IRolesService repositoryRole, IFacilityService repositoryFacility, ILOBService repositoryLOB, ISBUInfoService repositorySBUInfo, IPermissionService repositoryPermission, IAuthenticationTokenService repositoryAuthenticate, IUserAccessRequestService repositoryUserAccess, IProcessService repositoryProcess, IClientService repositoryClient, ICampaignService repositoryCampaign)
        {
            _repositoryRole = repositoryRole;
            _repositoryFacility = repositoryFacility;
            _repositoryLOB = repositoryLOB;
            _repositorySBUInfo = repositorySBUInfo;
            _repositoryPermission = repositoryPermission;
            _repositoryAuthenticate = repositoryAuthenticate;
            _repositoryUserAccess = repositoryUserAccess;
            _repositoryProcess = repositoryProcess;
            _repositoryClient = repositoryClient;
            _repositoryCampaign = repositoryCampaign;
        }

        public ActionResult Index()
        {
            return View(new UserManagementViewModel());
        }
        public JsonResult JsonGetJobCode()
        {
            return Json(_repositoryRole.GetUserJobCode());
        }
        public JsonResult JsonGetFacilities()
        {
            return Json(_repositoryFacility.GetFacilityList("", true));
        }
        public JsonResult JsonGetLOB()
        {
            return Json(_repositoryLOB.GetLOBList("", true));
        }
        public JsonResult JsonGetSBU()
        {
            return Json(_repositorySBUInfo.GetSBUList(true));
        }
        public JsonResult JsonGetRoleApprover()
        {
            IList<KeyValuePair<string, string>> lstApprover = new List<KeyValuePair<string, string>>();
            List<BEUserInfo> lRoleInfo = new List<BEUserInfo>();
            lRoleInfo = _repositoryPermission.GetUserRoleListUserRole();
            if (lRoleInfo != null && lRoleInfo.Count > 0)
            {
                for (int i = 0; i < lRoleInfo.Count; i++)
                {
                    lstApprover.Add(new KeyValuePair<string, string>(lRoleInfo[i].iUserID.ToString(), lRoleInfo[i].sUserName.ToString()));
                }
            }
            return Json(lstApprover);
        }

        [HttpPost]
        [ValidateAntiForgeryToken] // Cross-Site Request Forgery fixed
        public JsonResult GetLanUser(string Text)
        {
            IList<BELdapUserInfo> LanUsers = null;

            Errlog.ErrorLogFile("UserManagementController", "GetLanUser Entry", Text);

            LanUsers = _repositoryAuthenticate.GetUserList(Encoder.HtmlEncode(Text, false), "Corp.exlservice.com");

            Errlog.ErrorLogFile("UserManagementController", "GetLanUser", LanUsers.Count.ToString());

            if (LanUsers.Count > 0)
            {
                Errlog.ErrorLogFile("UserManagementController", "GetLanUser", $"User Count name Like {Encoder.HtmlEncode(Text, false)} = {LanUsers.Count}");

                for (int i = 0; i < LanUsers.Count; i++)
                {
                    if (LanUsers[i].UserName != null)
                    {
                        string[] UserData = LanUsers[i].UserName.Split('-');
                        LanUsers[i].UserName = string.IsNullOrEmpty(UserData[1].ToString().Trim()) ? LanUsers[i].FirstName + " " + LanUsers[i].LastName : UserData[1].ToString().Trim() + " - (" + UserData[0].ToString() + ")";
                        LanUsers[i].EmailID = UserData[0].ToString().Trim() + ":" + LanUsers[i].EmailID + ":" + LanUsers[i].LastName + ":" + LanUsers[i].MiddleName + ":" + LanUsers[i].FirstName;
                    }
                }

                Errlog.ErrorLogFile("UserManagementController", "GetLanUser", $"After Model binding in For Loop");

            }
            else
            {
                Errlog.ErrorLogFile("UserManagementController", "GetLanUser", "else block");

                ViewData["Message"] = BPA.GlobalResources.UI.Resources_common.dispNoRecordFound;
            }
            Errlog.ErrorLogFile("UserManagementController", "GetLanUser", $"before json Return user id {base.oUser.iUserID}");

            return Json(new { LanUsers, base.oUser.iUserID });
        }
        public JsonResult GetSuperwiser(string Text, string SearchCondition)
        {
            List<BEUserInfo> lstSuperwiser = null;
            //bFlag = _FormID == 188 ? false : true;
            //for this list update the SP to get isclient
            lstSuperwiser = _repositoryPermission.GetUserList(Encoder.HtmlEncode(Text.Trim(), false), true, SearchCondition);
            SelectList lst = new SelectList(lstSuperwiser, "iUserID", "Name");
            return Json(new { lstSuperwiser, base.oUser.iUserID });
        }

        //[HttpParamAction]
        [HttpPost]
        public ActionResult CreateUser(UserManagementViewModel objUserManagementViewModel)
        {
            try
            {
                ClientUserInfoModel ObjUserInfo = CatchRecordUser(objUserManagementViewModel);

                if (objUserManagementViewModel.iUserID == 0)
                {
                    _repositoryPermission.InsertClientUserRecord(ObjUserInfo, int.Parse(BPA.GlobalResources.Resources_FormID.UserManagement));
                    ViewData["Message"] = BPA.GlobalResources.UI.AppConfiguration.Resources_UserManagement.display_UserSavedSucessFully + ",OK";
                }
                else
                {
                    _repositoryPermission.UpdateClientUserRecord(ObjUserInfo, int.Parse(BPA.GlobalResources.Resources_FormID.UserManagement));
                    ViewData["Message"] = BPA.GlobalResources.UI.AppConfiguration.Resources_UserManagement.display_UserUpdatedSucessFully + ",OK";
                }
                ObjUserInfo = null;
            }
            catch (Exception ex)
            {
                ViewData["Message"] = ex.Message.ToString();

            }
            return Json(ViewData["Message"].ToString());
        }

        private ClientUserInfoModel CatchRecordUser(UserManagementViewModel oModel)
        {
            DateTime fdate = string.IsNullOrEmpty(oModel.dDOJ) ? DateTimeTimeZoneConversion.GetCurrentDateTime(false, base.oUser.sUserTimeZone, base.oUser.sServerTimeZone) : Convert.ToDateTime(oModel.dDOJ);
            //BEUserInfo user = oModel.UserInfo;
            ClientUserInfoModel oUser = new ClientUserInfoModel()
            {
                bIsBot = oModel.bIsBot,
                ClientID = oModel.iClientID,
                ClientUser = oModel.bClientUser,
                DeletedProcess = string.Empty,
                Disabled = oModel.bDisabled,
                DOJ = fdate,
                Email = oModel.sEmail,
                EmployeeID = Convert.ToInt32(oModel.iEmployeeID),
                FacilityId = oModel.iFacilityId,
                FirstName = oModel.sFirstName,
                JobID = oModel.iJobID,
                LanID = oModel.bLanID,
                LastName = oModel.sLastName,
                LOBID = oModel.iLOBID,
                LoginName = oModel.sLoginName,
                MiddleName = oModel.sMiddleName,
                Process = string.Empty,
                RoleApprover = Convert.ToInt32(oModel.iRoleApprover),
                RoleID = Convert.ToInt32(oModel.iRoleID),
                SBUID = oModel.iSBUID,
                SupervisorID = oModel.iSupervisorID,
                UserId = oModel.iUserID
            };
            // oModel.UserInfo;
            oUser.ClientUser = oModel.bClientUser;
            if (oModel.iExistingRoleID == int.Parse(oModel.iRoleID))
            {
                oUser.RoleApprover = 0;
            }
            string sProcess = string.Empty;
            string sDeletedProcess = string.Empty;

            oUser.DeletedProcess = sDeletedProcess;
            oUser.Process = sProcess;
            oUser.DOJ = fdate;
            return oUser;
        }

        #region Client User Creation search Logic
        public ActionResult UserManagementSearchView()
        {
            return View(new UserManagementViewModel());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UserManagementSearchView(UserManagementViewModel ObjClient)
        {
            ObjClient.ValidationFilter(ModelState, "btnSearch");
            if (ObjClient.UserEditList == null) { ObjClient.UserEditList = new List<BEUserInfo>(); }
            if (ObjClient.sSearchText != null)
            {
                if (ObjClient.sSearchText.Length >= 3)
                {
                    ObjClient.UserEditList = _repositoryPermission.GetUserList(Encoder.HtmlEncode(ObjClient.sSearchText, false), false, "");
                    if (ObjClient.UserEditList.Count <= 0)
                    {
                        ViewData["result"] = @BPA.GlobalResources.UI.Resources_common.display_msgNotFound;
                    }
                    var jsonResult = Json(ObjClient.UserEditList);
                    return View(ObjClient);
                }
                else
                {
                    return View(new UserManagementViewModel());
                }
            }
            else
            {
                return View(new UserManagementViewModel());
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult GetSearchUserList(string Text, string SearchCondition)
        {
            var userList = _repositoryPermission.GetUserList(Encoder.HtmlEncode(Text, false), false, SearchCondition);
            var jsonResult = Json(userList);
            return jsonResult;
        }

        #endregion

        public JsonResult SetUserID(int UserID = 0)
        {
            TempData["UserID"] = UserID;
            TempData.Keep("UserID");
            return Json("1");
        }

        public ActionResult UserDetailsView(int UserId)
        {
            UserManagementViewModel model = new UserManagementViewModel();
            model.ValidationFilter(ModelState, "SavUserDetail");
            if (UserId > 0)
            {
                DataSet dsUserDetails = JsonConvert.DeserializeObject<DataSet>(_repositoryUserAccess.GetUserDetails(UserId)) ?? new DataSet();
                TempData["UserDetailsViewTable"] = JsonConvert.SerializeObject(dsUserDetails);
                if (dsUserDetails != null && dsUserDetails.Tables.Count > 0)
                {
                    DataTable dtUserDetails = dsUserDetails.Tables["UserDetails"];
                    if (dtUserDetails.Rows.Count > 0)
                    {
                        model.iUserID = Convert.ToInt32(dtUserDetails.Rows[0]["UserId"].ToString());
                        model.sFirstName = dtUserDetails.Rows[0]["FirstName"].ToString();

                        model.iEmployeeID = dtUserDetails.Rows[0]["EmpId"].ToString();

                        model.sMiddleName = dtUserDetails.Rows[0]["MiddleName"].ToString();

                        model.sLastName = dtUserDetails.Rows[0]["LastName"].ToString();

                        model.sLoginName = dtUserDetails.Rows[0]["LoginName"].ToString();

                        model.sEmail = dtUserDetails.Rows[0]["Email"].ToString();

                        model.sSupervisorName = dtUserDetails.Rows[0]["SupervisorName"].ToString();
                        model.sTeamName = dtUserDetails.Rows[0]["TeamName"].ToString();

                        model.dDOJ = dtUserDetails.Rows[0]["DOJ"] != null ? dtUserDetails.Rows[0]["DOJ"].ToString() : Convert.ToString(DateTime.Today);
                        model.DOL = dtUserDetails.Rows[0]["Enddate"].ToString();
                        model.sFacilityName = dtUserDetails.Rows[0]["FacilityName"].ToString();
                        model.sLocationName = dtUserDetails.Rows[0]["LocationName"].ToString();

                        model.RoleInfo.sRoleName = dtUserDetails.Rows[0]["ERPRole"].ToString();
                        model.sErpJob = dtUserDetails.Rows[0]["JobDesc"].ToString();

                        model.bClientUser = Convert.ToBoolean(dtUserDetails.Rows[0]["IsClient"]);
                        model.bDisabled = Convert.ToBoolean(dtUserDetails.Rows[0]["Disabled"]);
                        dtUserDetails = null;

                    }
                    if (dsUserDetails.Tables["ShiftDetails"] != null)
                    {
                        DataTable dtShift = dsUserDetails.Tables["ShiftDetails"].Clone();
                        foreach (DataColumn c in dtShift.Columns)
                        {
                            string cName = "";
                            c.Caption = c.Caption;
                            var cTempName = c.ColumnName.Split(' ');
                            foreach (var item in cTempName)
                                cName += item;
                            c.ColumnName = cName;
                        }
                        dtShift.AcceptChanges();
                        for (int i = 0; i < dsUserDetails.Tables["ShiftDetails"].Rows.Count; i++)
                            dtShift.Rows.Add(dsUserDetails.Tables["ShiftDetails"].Rows[i].ItemArray);
                        model.dtShift = dtShift;
                        dtShift = null;
                    }
                    if (dsUserDetails.Tables["ProjectDetails"] != null)
                        model.dtClientProcess = dsUserDetails.Tables["ProjectDetails"];
                    if (dsUserDetails.Tables["QuotaDetails"] != null)
                        model.dtQuota = dsUserDetails.Tables["QuotaDetails"];
                    if (dsUserDetails.Tables["ERPDetails"] != null)
                        model.dtERPChanges = dsUserDetails.Tables["ERPDetails"];
                    dsUserDetails = null;
                }
                else
                    ViewData["Message"] = "No Detail Found.";
            }
            // DataTable products = model.dtClientProcess;

            //  return View(products);
            return View(model);
            // return View("UserDetailsView2", products);

        }

        public ActionResult GetShiftDetailsDataTable([DataSourceRequest] DataSourceRequest request)
        {
            DataTable dt = new();
            DataSet dsTables = JsonConvert.DeserializeObject<DataSet>(Convert.ToString(TempData["UserDetailsViewTable"]) ?? "") ?? new DataSet();
            dt = dsTables.Tables["ShiftDetails"] ?? dt;
            return Json(dt.ToDataSourceResult(request));
        }
        public ActionResult GetProjectDetailsDataTable([DataSourceRequest] DataSourceRequest request)
        {
            DataTable dt = new();
            DataSet dsTables = JsonConvert.DeserializeObject<DataSet>(Convert.ToString(TempData["UserDetailsViewTable"]) ?? "") ?? new DataSet();
            dt = dsTables.Tables["ProjectDetails"] ?? dt;
            return Json(dt.ToDataSourceResult(request));
        }
        public ActionResult GetQuotaDetailsDataTable([DataSourceRequest] DataSourceRequest request)
        {
            DataTable dt = new();
            DataSet dsTables = JsonConvert.DeserializeObject<DataSet>(Convert.ToString(TempData["UserDetailsViewTable"]) ?? "") ?? new DataSet();
            dt = dsTables.Tables["QuotaDetails"] ?? dt;
            return Json(dt.ToDataSourceResult(request));
        }
        public ActionResult GetERPDetailsDataTable([DataSourceRequest] DataSourceRequest request)
        {
            DataTable dt = new();
            DataSet dsTables = JsonConvert.DeserializeObject<DataSet>(Convert.ToString(TempData["UserDetailsViewTable"]) ?? "") ?? new DataSet();
            dt = dsTables.Tables["ERPDetails"] ?? dt;
            return Json(dt.ToDataSourceResult(request));
        }

        public ActionResult FillSelectedUserDetail()
        {
            UserManagementViewModel UserDetail = new UserManagementViewModel();
            BEUserInfo oBEUserInfo = _repositoryPermission.GetClientUserDetails(int.Parse(TempData["UserID"].ToString()));
            UserDetail.UserInfo = oBEUserInfo;
            if (oBEUserInfo.oRoles != null && oBEUserInfo.oRoles.Count > 0)
            {
                UserDetail.iRoleID = oBEUserInfo.oRoles.FirstOrDefault().iRoleID.ToString();
                UserDetail.iExistingRoleID = int.Parse(UserDetail.iRoleID);
            }
            oBEUserInfo = null;
            var lstGridProcess = getGridProcess(UserDetail.iClientID);
            string[] aryProcess = UserDetail.UserInfo.sProcess.Split(',');
            if (aryProcess != null && aryProcess.Length > 0)
            {
                for (int i = 0; i < lstGridProcess.Count; i++)
                {
                    if (aryProcess.Contains(lstGridProcess[i].iProcessID.ToString()))
                    {
                        lstGridProcess[i].bSelected = true;
                        lstGridProcess[i].iProcessMapID = lstGridProcess[i].iProcessID;
                    }
                }
            }
            UserDetail.lstProcessToMap = lstGridProcess;
            //}
            return View("Index", UserDetail);
        }
        private IList<ProcessAndApprover> getGridProcess(int iClientID)
        {
            IList<ProcessAndApprover> objProApp = new List<ProcessAndApprover>();
            IList<BEProcessInfo> lProcInfo = null;
            lProcInfo = _repositoryProcess.GetProcessListSearch(iClientID, "");
            if (lProcInfo != null && lProcInfo.Count > 0)
            {
                for (int i = 0; i < lProcInfo.Count; i++)
                {
                    objProApp.Add(new ProcessAndApprover() { objProcessInfo = lProcInfo[i] });
                }
            }
            lProcInfo = null;

            return objProApp;
        }

        #region User Access Status/Approval/Update Logic
        public ActionResult UserAccessApprovalView()
        {
            return View(new UserManagementViewModel());
        }
        [HttpPost]
        [ValidateAntiForgeryToken] // Cross-Site Request Forgery fixed
        public ActionResult UserAccessApprovalView(UserManagementViewModel model)
        {
            DateTime fdate = string.IsNullOrEmpty(model.FromDate) ? DateTimeTimeZoneConversion.GetCurrentDateTime(false, base.oUser.sUserTimeZone, base.oUser.sServerTimeZone) : Convert.ToDateTime(model.FromDate);
            DateTime Tdate = string.IsNullOrEmpty(model.ToDate) ? DateTimeTimeZoneConversion.GetCurrentDateTime(false, base.oUser.sUserTimeZone, base.oUser.sServerTimeZone) : Convert.ToDateTime(model.ToDate);
            var listdsReqStatus = _repositoryPermission.GetUserRequestStatus(fdate, Tdate);
            if (listdsReqStatus != null && listdsReqStatus.Count > 0)
            {
                model.dtRequestStatus = UISharedLayer.ValidateDate(UISharedLayer.ToDataTable(listdsReqStatus));
                listdsReqStatus = null;
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken] // Cross-Site Request Forgery fixed
        public JsonResult JsonGetRole(int? iAgentID, bool IsclientUser)
        {
            string ERPRole = "";
            if (iAgentID == null)
            {
                return Json(_repositoryRole.GetClientRoleList());
            }
            else
            {
                DataSet dsUserDetails = JsonConvert.DeserializeObject<DataSet>(_repositoryUserAccess.GetUserDetails(iAgentID.Value)) ?? new DataSet();
                if (dsUserDetails.Tables[0].Rows.Count > 0)
                {
                    ERPRole = dsUserDetails.Tables[0].Rows[0]["ERPRole"].ToString();
                }
                IList<BEUserSetting> oUser = null;

                oUser = _repositoryPermission.GetUserSetting(iAgentID.Value);

                var oUserSetting = oUser.Where(a => a.IsClientRole == IsclientUser).ToList();
                return Json(new { oUserSetting, ERPRole, });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken] // Cross-Site Request Forgery fixed
        public JsonResult MakeTree(int iAgentID, int iRoleID, bool IsCLientUser)
        {
            DataSet dsUserDetails = JsonConvert.DeserializeObject<DataSet>(_repositoryUserAccess.GetUserDetails(iAgentID)) ?? new DataSet();
            IList<TreeStructure> treeData = new List<TreeStructure>();
            IList<BEClientInfo> lClient = null;
            IList<BEProcessInfo> lProcess = null;
            IList<BECampaignInfo> lCamp = null;
            bool isClient = false;
            bool isProcess = false;
            bool isCampaign = false;
            int _MappedOn = 0;


            IList<BEUserSetting> oUserSetting = _repositoryPermission.GetUserSetting(iAgentID);
            IList<BEUserSetting> temp = oUserSetting.Where(p => p.RoleID == iRoleID).ToList();
            if (temp.Count > 0) { _MappedOn = temp[0].MappedOn; }
            for (int i = 0; i < temp.Count(); i++)
            {

                switch (temp[i].MappedOn)
                {
                    case 1:
                        isClient = true;
                        //lblTreeHeader.Text = "Client Mapping";
                        break;
                    case 2:
                        isProcess = true;
                        //lblTreeHeader.Text = "Process Mapping";
                        break;
                    case 3:
                        isCampaign = true;
                        //lblTreeHeader.Text = "Campaign Mapping";
                        break;
                }
            }

            oUserSetting = null; temp = null;
            lClient = _repositoryClient.GetClientAccessList(iAgentID, true);
            lProcess = _repositoryProcess.GetProcessAccessList(iAgentID, true);
            lCamp = _repositoryCampaign.GetCampaignAccessList(iAgentID, true);

            for (int i = 0; i < lClient.Count; i++)
            {
                TreeStructure t = new TreeStructure() { text = lClient[i].sClientName, id = lClient[i].iClientID.ToString() + "|1", NodeIdentity = "1", MappedOn = _MappedOn };
                if (isClient)
                {
                    t.Checked = t.AllReadyChecked = lClient[i].bDisabled;
                    treeData.Add(t);
                    continue;
                }

                IList<BEProcessInfo> leProcess = lProcess.Where(p => p.iClientID == lClient[i].iClientID).ToList();
                for (int j = 0; j < leProcess.Count(); j++)
                {
                    TreeStructure tProcess = new TreeStructure() { text = leProcess[j].sProcessName, id = leProcess[j].iProcessID.ToString() + "|2", NodeIdentity = "2" };
                    if (isProcess)
                    {
                        tProcess.Checked = tProcess.AllReadyChecked = leProcess[j].bDisabled;
                        if (t.items == null)
                            t.items = new List<TreeStructure>();
                        t.items.Add(tProcess);
                        if (tProcess.Checked)
                        {
                            t.expanded = true;
                        }
                        tProcess = null;
                        continue;
                    }

                    IList<BECampaignInfo> leCamp = lCamp.Where(p => p.iProcessID == leProcess[j].iProcessID).ToList();
                    for (var z = 0; z < leCamp.Count; z++)
                    {
                        TreeStructure tCampaign = new TreeStructure() { text = leCamp[z].sCampaignName, id = leCamp[z].iCampaignID.ToString() + "|3", NodeIdentity = "3" };
                        if (isCampaign)
                        {
                            tCampaign.Checked = tCampaign.AllReadyChecked = leCamp[z].bDisabled;
                            if (tProcess.items == null) tProcess.items = new List<TreeStructure>();
                            tProcess.items.Add(tCampaign);
                            if (tCampaign.Checked)
                            {
                                t.expanded = true;
                                tProcess.expanded = true;
                            }
                        }

                        tCampaign = null;
                    }
                    leCamp = null;
                    if (t.items == null) t.items = new List<TreeStructure>();
                    t.items.Add(tProcess);
                    tProcess = null;
                }
                treeData.Add(t);
                t = null;
                leProcess = null;
            }

            lClient = null; lProcess = null; lCamp = null;

            return Json(new { treeData = treeData, UserMapData = dsUserDetails.Tables[2].ConvertToList<ProcessMap>() });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult InsertUserMapping(int iAgentID, int iRoleID, string EffectiveDate, bool Disable, string JsonString)
        {
            string MSG = "";
            List<TreeStructure> lstTreeData = JsonConvert.DeserializeObject<IEnumerable<TreeStructure>>(JsonString).ToList() ?? new List<TreeStructure>();
            BEUserMapping oUserMap = new BEUserMapping() { oClient = new List<BEClientInfo>(), oProcess = new List<BEProcessInfo>(), oCampaign = new List<BECampaignInfo>() };
            string sAllSelectedID = ""; int ProcessId = 0;
            foreach (var item in lstTreeData)
            {
                switch (item.MappedOn.ToString())
                {
                    case "1":
                        if (item.Checked && !item.AllReadyChecked)
                        {
                            oUserMap.oClient.Add(new BEClientInfo() { iClientID = Convert.ToInt32(item.id.Split('|')[0]) });
                            sAllSelectedID += item.id.Split('|')[0] + ",";
                        }
                        break;
                    case "2":

                        if (item.items != null)
                        {
                            foreach (var pItem in item.items)
                            {
                                if (pItem.Checked && !pItem.AllReadyChecked)
                                {
                                    oUserMap.oProcess.Add(new BEProcessInfo() { iProcessID = Convert.ToInt32(pItem.id.Split('|')[0]) });
                                    sAllSelectedID += pItem.id.Split('|')[0] + ",";
                                    ProcessId = Convert.ToInt32(pItem.id.Split('|')[0]);
                                }
                            }
                        }
                        break;
                    case "3":

                        if (item.items != null)
                        {
                            foreach (var pitem in item.items)
                            {
                                if (pitem.items != null)
                                {
                                    foreach (var cItem in pitem.items)
                                    {
                                        if (cItem.Checked && !cItem.AllReadyChecked)
                                        {
                                            oUserMap.oCampaign.Add(new BECampaignInfo() { iCampaignID = Convert.ToInt32(cItem.id.Split('|')[0]) });
                                            sAllSelectedID += cItem.id.Split('|')[0] + ",";
                                        }
                                    }
                                }
                            }
                        }
                        break;
                }
            }

            if (sAllSelectedID != "")
                sAllSelectedID = sAllSelectedID.Substring(0, sAllSelectedID.Length - 1);
            oUserMap.oRole = new BERoleInfo { iRoleID = iRoleID };
            oUserMap.oUser = new BEUserInfo { iUserID = iAgentID };
            oUserMap.iCreatedBy = base.oUser.iUserID;
            oUserMap.dtEffectiveDate = Convert.ToDateTime(EffectiveDate);
            oUserMap.bDisabled = Disable;
            oUserMap.MappedOn = lstTreeData[0].MappedOn;
            object lstUserApproval = null;
            try
            {
                int RequestId = _repositoryPermission.InsertUserMappingForApproval(CatchRecord(oUserMap, Encoder.HtmlEncode(sAllSelectedID, false)), int.Parse(BPA.GlobalResources.Resources_FormID.UserManagement));
                var dsReqApproval = UISharedLayer.ToDataTable(_repositoryPermission.GetUserRequestType(oUserMap.oUser.iUserID, RequestId, oUserMap.MappedOn));
                if (dsReqApproval != null)
                {
                    if (dsReqApproval.Rows.Count > 0)
                    {
                        lstUserApproval = from o in dsReqApproval.AsEnumerable()
                                          select new UserApproval()
                                          {
                                              RequestId = RequestId,
                                              Id = o.Field<int>("Id"),
                                              RequestName = o.Field<string>("RequestName"),
                                              RequestType = o.Field<int>("RequestType"),
                                              RequestTypeDetails = o.Field<string>("RequestTypeDetails"),
                                              RequestTypeId = o.Field<int>("RequestTypeId"),
                                              ProcessId = ProcessId
                                          };
                    }
                    else
                    {
                        ViewData["Message"] = BPA.GlobalResources.UI.AppConfiguration.Resources_UserManagement.display_ChangesDoneSuccessfully;
                        MSG = BPA.GlobalResources.UI.AppConfiguration.Resources_UserManagement.display_ChangesDoneSuccessfully;
                    }
                }
                oUserMap = null;

            }
            catch (Exception ex)
            {
                var result_ = new { lstUserApproval = (lstUserApproval == null ? "" : lstUserApproval), success = false, message = ex.Message.ToString() };
                return Json(result_);

            }

            var result = new { lstUserApproval = (lstUserApproval == null ? "" : lstUserApproval), success = true, message = "" };
            return Json(result);

        }


        private InsertUserMappingForApprovalModel CatchRecord(BEUserMapping mapping, string DeletedNodes)
        {
            InsertUserMappingForApprovalModel model = new InsertUserMappingForApprovalModel()
            {
                RoleInfo = new UserMappingForApprovalRoleInfo()
                {
                    RoleID = mapping.oRole.iRoleID
                },
                UserInfo = new UserMappingForApprovalUserInfo()
                {
                    UserID = mapping.oUser.iUserID
                },
                Disabled = mapping.bDisabled,
                MappedOn = mapping.MappedOn,
                EffectiveDate = mapping.dtEffectiveDate,
                ClientInfo = mapping.oClient.Select(x => new UserMappingForApprovalClientInfo()
                {
                    ClientID = x.iClientID
                }).ToList(),
                ProcessInfo = mapping.oProcess.Select(x => new UserMappingForApprovalProcessInfo()
                {
                    ProcessID = x.iProcessID
                }).ToList(),
                CampaignInfo = mapping.oCampaign.Select(x => new UserMappingForApprovalCampaignInfo()
                {
                    CampaignID = x.iCampaignID
                }).ToList(),
                DeletedNodes = DeletedNodes
            };
            return model;
        }

        public JsonResult FillApproverList(int UserId, int ClientId, int ProcessId, int Flag)
        {
            object lstApprover = null;
            var approverList = _repositoryPermission.GetUserApproverList(UserId, ClientId, ProcessId, Flag, int.Parse(BPA.GlobalResources.Resources_FormID.UserManagement));
            approverList.Add(new UserApproverDetail() { ApproverId = 124097, ApproverName = "Sowmya Chowdary" });
            var ds = UISharedLayer.ToDataTable(approverList);
            lstApprover = (from o in ds.AsEnumerable()
                           where o.Field<int>("ApproverId") != base.oUser.iUserID
                           select new ApproverDetail()
                           {
                               iApproverId = o.Field<int>("ApproverId"),
                               sApproverName = o.Field<string>("ApproverName") ?? ""
                           }).ToList();
            ds = null;
            return Json(lstApprover);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult CancelMappingRequest(int RequestId)
        {
            _repositoryPermission.CancelRequestInBetween(RequestId, int.Parse(BPA.GlobalResources.Resources_FormID.UserManagement));
            ViewData["Message"] = BPA.GlobalResources.UI.AppConfiguration.Resources_UserManagement.display_RequestCanceledSuccessfully;
            return Json(ViewData["Message"].ToString());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult GetAndCancelRequestStatus(string FromDate, string ToDate, int? RequestID)
        {
            if (RequestID != null)
            {
                _repositoryPermission.CancelAccessRequest(RequestID.Value, int.Parse(BPA.GlobalResources.Resources_FormID.UserManagement));
            }
            object lstReqStatus = null;
            var dsReqStatus = UISharedLayer.ValidateDate(UISharedLayer.ToDataTable(_repositoryPermission.GetUserRequestStatus(Convert.ToDateTime(FromDate), Convert.ToDateTime(ToDate))));

            lstReqStatus = dsReqStatus.AsEnumerable().Select(r => r.Table.Columns.Cast<DataColumn>()
            .Select(c => new KeyValuePair<string, string>(c.ColumnName, r[c.Ordinal].ToString())).ToDictionary(z => z.Key, z => z.Value)).ToList();

            dsReqStatus = null;
            return Json(new { lstReqStatus = lstReqStatus, ID = RequestID });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult UserMappingapproval(string JsonString)
        {
            List<UserApproval> lstTreeData = JsonConvert.DeserializeObject<IEnumerable<UserApproval>>(JsonString).ToList();
            UserMappingApproverModel model = new UserMappingApproverModel();
            List<UserMappingApprover> approvers = new List<UserMappingApprover>();

            for (int i = 0; i < lstTreeData.Count; i++)
            {
                UserMappingApprover user = new UserMappingApprover();

                user.RequestId = lstTreeData[i].RequestId;
                user.RequestType = lstTreeData[i].RequestType;
                user.RequestTypeId = lstTreeData[i].RequestTypeId;
                user.Approver1Id = lstTreeData[i].SelectedApproverL1.iApproverId;
                user.Approver2Id = "";
                approvers.Add(user);
            }
            model.UserMappingApprovers = approvers;
            _repositoryPermission.InsertUserMappingApprovers(model, int.Parse(BPA.GlobalResources.Resources_FormID.UserManagement));
            ViewData["Message"] = BPA.GlobalResources.UI.AppConfiguration.Resources_UserManagement.display_RequestRaisedSuccessfully;
            model = null;
            lstTreeData = null;
            approvers = null;
            return Json(ViewData["Message"].ToString());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult GetRequestForApproval(string RequestIds, string RequestTypeIDs, string RequestTypes, string ApprovalLevels, bool? isApproved)
        {
            string Meassage = string.Empty;
            if (RequestIds != null && RequestIds != "")
            {
                var partRequestIds = RequestIds.Split(',');
                var partRequestTypeIDs = RequestTypeIDs.Split(',');
                var partRequestTypes = RequestTypes.Split(',');
                var partApprovalLevels = ApprovalLevels.Split(',');
                if (partRequestIds.Length > 0)
                {
                    try
                    {
                        if (isApproved.Value)
                        {
                            for (int i = 0; i < partRequestIds.Length; i++)
                            {
                                if (partRequestIds[i] != "" && partRequestIds[i] != ",")
                                    _repositoryPermission.ApproveAccessRequest(Convert.ToInt32(Encoder.HtmlEncode(partRequestIds[i], false)), Convert.ToInt32(Encoder.HtmlEncode(partRequestTypeIDs[i], false)), Convert.ToInt32(Encoder.HtmlEncode(partRequestTypes[i], false)), Convert.ToInt32(Encoder.HtmlEncode(partApprovalLevels[i], false)), int.Parse(BPA.GlobalResources.Resources_FormID.UserManagement));
                            }
                            Meassage = BPA.GlobalResources.UI.AppConfiguration.Resources_UserManagement.display_RequestApprovedSuccessfully;
                        }
                        else
                        {
                            for (int i = 0; i < partRequestIds.Length; i++)
                            {
                                if (partRequestIds[i] != "" && partRequestIds[i] != ",")
                                    _repositoryPermission.RejectAccessRequest(Convert.ToInt32(Encoder.HtmlEncode(partRequestIds[i], false)), Convert.ToInt32(Encoder.HtmlEncode(partRequestTypeIDs[i], false)), Convert.ToInt32(Encoder.HtmlEncode(partRequestTypes[i], false)), Convert.ToInt32(Encoder.HtmlEncode(partApprovalLevels[i], false)), int.Parse(BPA.GlobalResources.Resources_FormID.UserManagement));
                            }
                            Meassage = BPA.GlobalResources.UI.AppConfiguration.Resources_UserManagement.display_RequestRejectedSuccessfully;
                        }
                    }
                    catch (Exception ex)
                    {
                        ViewData["Message"] = ex.Message.ToString();
                    }
                }
                partRequestIds = null;
                partRequestTypeIDs = null;
                partRequestTypes = null;
                partApprovalLevels = null;
            }
            object lstReqApproval = null;

            var dsReqApproval = UISharedLayer.ValidateDate(UISharedLayer.ToDataTable(_repositoryPermission.GetUserApprovalList()));

            dsReqApproval.Columns.Add("bSelected", typeof(bool));
            lstReqApproval = dsReqApproval.AsEnumerable().Select(r => r.Table.Columns.Cast<DataColumn>()
          .Select(c => new KeyValuePair<string, string>(c.ColumnName, r[c.Ordinal].ToString())).ToDictionary(z => z.Key, z => z.Value)).ToList();


            dsReqApproval = null;

            return Json(new { lstReqApproval = lstReqApproval, Meassage = Meassage });
        }

        #endregion
    }
}
