using BPA.GlobalResources;
using BPA.GlobalResources.UI.AppConfiguration;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using MicUI.Configuration.Helper;
using MicUI.Configuration.Models.ViewModels;
using MicUI.Configuration.Module.Security;
using MicUI.Configuration.Services.ServiceModel;
using Newtonsoft.Json;
using System.Data;
using System.Text;

namespace MicUI.Configuration.Controllers
{
    public class RolesController : BaseController
    {
        private readonly IRolesService _repositoryRole;
        public RolesController(IRolesService repositoryRole)
        {
            _repositoryRole = repositoryRole;
        }
        public IActionResult Index()
        {

            if (!base.CheckViewPermission(int.Parse(Resources_FormID.Client)))
            {
                return RedirectToActionPermanent("AccessDenied", "Error");
            }
            //DataAnnotationsModelValidatorProvider.AddImplicitRequiredAttributeForValueTypes = false;
            RoleViewModel oRoleModel = new();
            //TempData["sRoleID"] = 2349;
            if (TempData["sRoleID"] == null)
            {
                GetFrmAction(oRoleModel, null);
            }
            else
            {
                // Update Case
                // get the form action control along with Role List
                oRoleModel.RoleId = int.Parse(TempData.Peek("sRoleID").ToString());
                oRoleModel = DisplayRecord(_repositoryRole.GetRoleList(int.Parse(TempData.Peek("sRoleID").ToString()))[0]);
            }
            return View(oRoleModel);
        }

        private RoleViewModel DisplayRecord(RoleInfoModel oBERole)
        {
            RoleViewModel oRoleViewModel = new();
            // Assign the value to RoleViewmodel object base on BERoleInfo object value
            oRoleViewModel.RoleId = oBERole.RoleID;
            oRoleViewModel.RoleName = oBERole.RoleName ?? "";
            oRoleViewModel.Description = oBERole.RoleDescription ?? "";
            oRoleViewModel.IsClientUserRole = oBERole.IsClientRole;
            oRoleViewModel.Disable = oBERole.Disabled;
            //oRoleViewModel.SecurityGroup = oBERole.iSecurityGroup.ToString();
            oRoleViewModel.RoleLevel = oBERole.LevelID.ToString();
            //oRoleViewModel.Approver = oBERole.iLevelID.ToString();

            GetFrmAction(oRoleViewModel, oBERole);  // Get the form action which check box checked by the users

            return oRoleViewModel; // Return RoleViewModel object

        }

        /// <summary>
        /// this method check the datatable row value if row value is DBNull then set the Null else set row value
        /// </summary>
        /// <param name="oRoleViewModel">RoleViewModel</param>
        /// <param name="oBERole">BERoleInfo</param>

        private void GetFrmAction(RoleViewModel oRoleViewModel, RoleInfoModel oBERole)
        {
            oRoleViewModel.lstFromActionMap = new List<GetFromAction>();
            oRoleViewModel.lstFromModuleActionMap = new List<GetFromModuleAction>();
            DataTable dt;
            if (oBERole == null)
            {
                dt = UISharedLayer.ValidateDate(UISharedLayer.ToDataTable(_repositoryRole.PopulateFormAction(0)));
            }
            else
            {
                dt = UISharedLayer.ValidateDate(UISharedLayer.ToDataTable(oBERole.FormActions));
            }
            TempData["Gd"] = JsonConvert.SerializeObject(dt);
            // TempData["Gd"] = dt;
            TempData.Keep("Gd");
            string ModuleNameTemp = "";
            foreach (DataRow row in dt.Rows)
            {
                var obj = new GetFromAction();
                var objModule = new GetFromModuleAction();
                obj.FormName = row["FormName"].ToString();
                obj.FormId = int.Parse(row["FormID"].ToString());
                if (row["ModuleName"].ToString() != "")
                {


                    if (ModuleNameTemp == "")
                    {
                        ModuleNameTemp = row["ModuleName"].ToString();
                        objModule.ModuleName = ModuleNameTemp;
                        if (!string.IsNullOrEmpty(row["View"].ToString()))
                        {
                            objModule.SelectedModelView = int.Parse(row["View"].ToString());
                        }
                        if (!string.IsNullOrEmpty(row["Add"].ToString()))
                        {
                            objModule.SelectedModelAdd = int.Parse(row["Add"].ToString());
                        }
                        else
                        {
                            objModule.SelectedModelAdd = null;
                        }
                        oRoleViewModel.lstFromModuleActionMap.Add(objModule);
                    }
                    else
                    {
                        if (ModuleNameTemp != row["ModuleName"].ToString())
                        {
                            ModuleNameTemp = row["ModuleName"].ToString();
                            objModule.ModuleName = ModuleNameTemp;
                            if (!string.IsNullOrEmpty(row["View"].ToString()))
                            {
                                objModule.SelectedModelView = int.Parse(row["View"].ToString());
                            }
                            oRoleViewModel.lstFromModuleActionMap.Add(objModule);
                        }
                    }

                }
                if (!string.IsNullOrEmpty(row["View"].ToString()))
                {
                    obj.SelectedView = int.Parse(row["View"].ToString());
                }
                else
                {
                    obj.SelectedView = null;
                }
                if (!string.IsNullOrEmpty(row["Add"].ToString()))
                {
                    obj.SelectedAdd = int.Parse(row["Add"].ToString());
                }
                else
                {
                    obj.SelectedAdd = null;
                }
                if (!string.IsNullOrEmpty(row["Modify"].ToString()))
                {
                    obj.SelectedModify = int.Parse(row["Modify"].ToString());
                }
                else
                {
                    obj.SelectedModify = null;
                }
                if (!string.IsNullOrEmpty(row["Delete"].ToString()))
                {
                    obj.SelectedDelete = int.Parse(row["Delete"].ToString());
                }
                else
                {
                    obj.SelectedDelete = null;
                }
                if (!string.IsNullOrEmpty(row["Approve"].ToString()))
                {
                    obj.SelectedApprove = int.Parse(row["Approve"].ToString());
                }
                else
                {
                    obj.SelectedApprove = null;
                }
                oRoleViewModel.lstFromActionMap.Add(obj);

            }
        }

        public JsonResult FillRoleLevel()
        {
            return Json(_repositoryRole.FillRoleLevel());
        }

        public JsonResult GetRoleApprover()
        {
            return Json(_repositoryRole.PopulateRoleApproverDropDownList(1));
        }

        public ActionResult HierarchyBinding_SearchAssessment([DataSourceRequest] DataSourceRequest request, string ModuleName)
        {
            if (!string.IsNullOrWhiteSpace(ModuleName))
            {
                return Json(GetGridModuleDataItem(ModuleName).lstFromActionMap.ToDataSourceResult(request));
            }
            else
            {
                return Json(0);
            }
        }

        private RoleViewModel GetGridModuleDataItem(string ModuleName)
        {
            RoleViewModel oRoleViewModel = new();
            oRoleViewModel.lstFromActionMap = new List<GetFromAction>();
            if (TempData["Gd"] != null)
            {
                DataTable GridDT = JsonConvert.DeserializeObject<DataTable>(TempData["Gd"].ToString()) ?? new DataTable();
                TempData.Keep("Gd");
                string searchTerm = ModuleName.ToString();
                string expression = String.Format("TRIM(ModuleName) = '{0}'", searchTerm.Trim());
                DataRow[] row = GridDT.Select(expression);
                //dr[0]["Form Name"].ToString()
                int i = 0;
                while (i < row.Length)
                {
                    var obj = new GetFromAction();
                    var objModule = new GetFromModuleAction();
                    obj.FormName = row[i]["FormName"].ToString();
                    obj.FormId = int.Parse(row[i]["FormID"].ToString());

                    if (row[i]["View"] != DBNull.Value)
                    {
                        obj.SelectedView = int.Parse(row[i]["View"].ToString());
                    }
                    else
                    {
                        obj.SelectedView = null;
                    }
                    if (!string.IsNullOrWhiteSpace(row[i]["Add"].ToString()))// != DBNull.Value
                    {
                        obj.SelectedAdd = int.Parse(row[i]["Add"].ToString());
                    }
                    else
                    {
                        obj.SelectedAdd = null;
                    }
                    if (!string.IsNullOrWhiteSpace(row[i]["Modify"].ToString()))// != DBNull.Value
                    {
                        obj.SelectedModify = int.Parse(row[i]["Modify"].ToString());
                    }
                    else
                    {
                        obj.SelectedModify = null;
                    }
                    if (!string.IsNullOrWhiteSpace(row[i]["Delete"].ToString()))
                    {
                        obj.SelectedDelete = int.Parse(row[i]["Delete"].ToString());
                    }
                    else
                    {
                        obj.SelectedDelete = null;
                    }
                    if (!string.IsNullOrWhiteSpace(row[i]["Approve"].ToString()))
                    {
                        obj.SelectedApprove = int.Parse(row[i]["Approve"].ToString());
                    }
                    else
                    {
                        obj.SelectedApprove = null;
                    }
                    oRoleViewModel.lstFromActionMap.Add(obj);
                    i++;
                }
            }
            return oRoleViewModel;

        }
        //
        // POST: /AppConfiguration/Roles/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Index(RoleViewModel JsonAllowActionData, string iApprover)
        {

            try
            {
                // RoleViewModel oRoleViewModel = JsonConvert.DeserializeObject<RoleViewModel>(objRoleModel);
                RoleViewModel oRoleViewModel = new();
                oRoleViewModel = JsonAllowActionData;


                if (JsonAllowActionData.lstFromActionMap != null)
                {

                    oRoleViewModel.lstFromActionMap = new List<GetFromAction>(JsonAllowActionData.lstFromActionMap.OrderBy(x => x.FormId));

                    if (oRoleViewModel.RoleId == 0)
                    {
                        _repositoryRole.InsertData(CatchRecord(oRoleViewModel, 1), Convert.ToInt32(iApprover), int.Parse(Resources_FormID.Roles));
                        ViewData["Message"] = Resources_Roles.display_Save;
                    }
                    else
                    {
                        // CatchRecord(oRoleViewModel, 2);
                        _repositoryRole.UpdateData(CatchRecord(oRoleViewModel, 2), Convert.ToInt32(iApprover), int.Parse(Resources_FormID.Roles));
                        ViewData["Message"] = Resources_Roles.display_Update;
                    }
                    ModelState.Clear();
                    oRoleViewModel = new RoleViewModel();
                    GetFrmAction(oRoleViewModel, null);
                }
                else
                {
                    var result_In = new { strMsg = "0", success = false, message = "" };
                    return Json(result_In);
                }
            }
            catch (Exception ex)
            {
                ViewData["Message"] = ex.Message.ToString();
                var result_ = new { strMsg = "0", success = false, message = ex.Message.ToString() };
                return Json(result_);

            }
            var result = new { strMsg = "1", success = true, message = "" };
            return Json(result);
            //return Json("1", JsonRequestBehavior.AllowGet);
        }

        private RoleInfoModel CatchRecord(RoleViewModel oRoleViewModel, int iFlag)
        {

            RoleInfoModel oRoles = new();
            oRoles.RoleID = oRoleViewModel.RoleId;
            oRoles.RoleName = Microsoft.Security.Application.Encoder.HtmlEncode(oRoleViewModel.RoleName, false);
            oRoles.RoleDescription = Microsoft.Security.Application.Encoder.HtmlEncode(oRoleViewModel.Description, false);
            oRoles.Disabled = oRoleViewModel.Disable;
            oRoles.IsClientRole = oRoleViewModel.IsClientUserRole;
            //oRoles.iSecurityGroup = int.Parse(oRoleViewModel.SecurityGroup);
            oRoles.LevelID = int.Parse(oRoleViewModel.RoleLevel);
            //oRoles.iLevelID = 1;
            string sClientRole = oRoleViewModel.IsClientUserRole ? Resources_Roles.display_Yes : Resources_Roles.display_No;
            string sStatus = oRoleViewModel.Disable ? Resources_Roles.display_Disabled : Resources_Roles.display_Active;
            oRoleViewModel.sbMailBody.Append("<b>" + Resources_Roles.display_Role_Details + "</b><br><br>");
            // iFlag value check role is update or create new role.If iFlag value = 2 > Roles Update 
            if (oRoleViewModel.RoleId > 0 && iFlag == 2)
            {
                // Create a Mail body in case of Role Update
                oRoleViewModel.sbMailBody.Append("<table border='1'>");
                oRoleViewModel.sbMailBody.Append("<tr><th>" + Resources_Roles.display_Parameter + "</th><th>" + Resources_Roles.display_Existing_Value + "</th><th>" + Resources_Roles.display_Changed_Value + "</th></tr>");
                oRoleViewModel.sbMailBody.Append("<tr><td>" + Resources_Roles.display_RoleName + "</td><td>" + Resources_Roles.display_RoleName + "</td><td>" + oRoleViewModel.RoleName + "</td></tr>");
                oRoleViewModel.sbMailBody.Append("<tr><td>" + Resources_Roles.display_IsClientUserRole + "</td><td>" + Resources_Roles.display_IsClientUserRole + "</td><td>" + sClientRole + "</td></tr>");
                oRoleViewModel.sbMailBody.Append("<tr><td>" + Resources_Roles.display_SecurityGroup + "</td><td>" + Resources_Roles.display_SecurityGroup + "</td><td>" + oRoleViewModel.SecurityGroup + "</td></tr>");
                oRoleViewModel.sbMailBody.Append("<tr><td>" + Resources_Roles.display_Status + "</td><td>" + Resources_Roles.display_Disable + "</td><td>" + sStatus + "</td></tr>");
                oRoleViewModel.sbMailBody.Append("</table><br>");
            }
            else
            {
                // Create a Mail body in case of new Role Create 
                oRoleViewModel.sbMailBody.Append("<b>" + Resources_Roles.display_RoleName + "</b> " + oRoleViewModel.RoleName + "<br>");
                oRoleViewModel.sbMailBody.Append("<b>" + Resources_Roles.display_IsClientUserRole + "</b> " + sClientRole + "<br>");
                oRoleViewModel.sbMailBody.Append("<b>" + Resources_Roles.display_SecurityGroup + "</b> " + oRoleViewModel.SecurityGroup + "<br>");
                oRoleViewModel.sbMailBody.Append("<b>" + Resources_Roles.display_Status + "</b> " + sStatus + "<br>");
            }
            DataTable dt = GetGridDataItem(oRoleViewModel);
            oRoles.MailBodyContent = oRoleViewModel.sbMailBody.ToString();
            oRoles.FormActions = dt.AsEnumerable().Select(x => new FormAction()
            {
                Add = (string)x["Add"],
                Approve = (string)x["Approve"],
                ChildID = (string)x["ChildID"],
                Delete = (string)x["Delete"],
                Description = (string)x["Description"],
                FormID = (long)x["FormID"],
                FormName = (string)x["FormName"],
                Modify = (string)x["Modify"],
                ModuleName = (string)x["ModuleName"],
                View = (string)x["View"]
            }).ToList();
            return oRoles;
        }

        private DataTable GetGridDataItem(RoleViewModel oRoleViewModel)
        {
            StringBuilder sbRow = null;
            if (TempData["Gd"] != null)
            {
                DataTable GridDT = JsonConvert.DeserializeObject<DataTable>(TempData["Gd"].ToString() ?? "") ?? new DataTable();
                TempData.Keep("Gd");
                GridDT.Select().OrderBy(u => u["FormId"]).ToList<DataRow>()
                       .ForEach(r =>
                       {
                           int _FormId = Convert.ToInt32(r["FormId"]);
                           var oRole = oRoleViewModel.lstFromActionMap.Where(x => x.FormId == _FormId).FirstOrDefault();

                           r["View"] = oRole == null ? Convert.ToInt32(r["View"].ToString() == "" ? 0 : r["View"]) : oRole.SelectedView;
                           r["Modify"] = oRole == null ? Convert.ToInt32(r["Modify"].ToString() == "" ? 0 : r["Modify"]) : oRole.SelectedModify;
                           r["Delete"] = oRole == null ? Convert.ToInt32(r["Delete"].ToString() == "" ? 0 : r["Delete"]) : oRole.SelectedDelete;
                           r["Add"] = oRole == null ? Convert.ToInt32(r["Add"].ToString() == "" ? 0 : r["Add"]) : oRole.SelectedAdd;
                           r["Approve"] = oRole == null ? Convert.ToInt32(r["Approve"].ToString() == "" ? 0 : r["Approve"]) : oRole.SelectedApprove;

                       });

                int rowCount = GridDT.Rows.Count;
                int colCount = GridDT.Columns.Count;
                DataTable dt = new();
                dt = GridDT.Clone();  // Create a clone of datatable because it will not effect on original datatable
                oRoleViewModel.sbMailBody.Append("<b>" + Resources_Roles.display_Access_Details + "</b><br><br>");
                oRoleViewModel.sbMailBody.Append("<table border='1'>");
                oRoleViewModel.sbMailBody.Append("<tr><th>" + Resources_Roles.display_Form + "</th><th>" + Resources_Roles.display_View + "</th><th>" + Resources_Roles.display_Modify + "</th><th>" + Resources_Roles.display_Delete + "</th><th>" + Resources_Roles.display_Approver_Chk + "</th><th>" + Resources_Roles.display_Add + "</th></tr>");
                for (int i = 0; i < rowCount; i++)
                {
                    object[] tempobject = new object[colCount];
                    sbRow = new StringBuilder();
                    for (int j = 0; j < colCount; j++)
                    {
                        tempobject[j] = GridDT.Rows[i][j];
                        if (tempobject[j] == null) continue;
                        if (j == 2) sbRow.Append("<tr><td>" + GridDT.Rows[i][j].ToString() + "</td>");
                        if (j < 3) continue;
                        if (tempobject[j].ToString() == "1") tempobject[j] = 0;
                        // if user does not the form action control in that case else will be call.
                        if (Request.Form.Count > 8)
                        {
                            // check which check box checked by the user and get the value.
                            if (Convert.ToString(Request.Form[$"chk{GridDT.Columns[j].ColumnName}"]) != null)
                            {
                                if (Request.Form["chk" + GridDT.Columns[j].ColumnName].Contains(tempobject[0].ToString()))
                                    tempobject[j] = 1;
                                if (GridDT.Rows[i][j] != null && tempobject[j] != null)//remove null def
                                    GridColor(GridDT.Rows[i][j].ToString(), tempobject[j].ToString(), sbRow);
                            }
                            else
                            {
                                // check if Datatable column name equal to ChildID then continue in for loop
                                if (GridDT.Columns[j].ColumnName == "ChildID") continue;
                                sbRow.Append("<td>&nbsp;</td>");
                            }
                        }
                        else
                        {
                            if (GridDT.Rows[i][j] != null && tempobject[j] != null)//remove null def
                                GridColor(GridDT.Rows[i][j].ToString(), tempobject[j].ToString(), sbRow);
                        }
                    }
                    oRoleViewModel.sbMailBody.Append(sbRow.ToString() + "</tr>");
                    dt.LoadDataRow(tempobject, true);
                    tempobject = null;
                }
                oRoleViewModel.sbMailBody.Append("</table>");
                oRoleViewModel.sbMailBody.Append("<table border='0'>");
                oRoleViewModel.sbMailBody.Append("<tr><td bgcolor='Red' width='25'><div style='border: solid 1 black'>&nbsp;</div></td><td>" + Resources_Roles.display_Revoke_Access_Request + "</td><td bgcolor='yellow' width='25'><div style='border: solid 1 black'>&nbsp;</div></td><td>Grant Access Request</td><td width='25' bgcolor='Green'><div style='border: solid 1 black'>&nbsp;</div></td><td>Already having Access</td><td width='25'><div style='border: solid 1 black'>&nbsp;</div></td><td>No Access Applicable</td></tr>");
                oRoleViewModel.sbMailBody.Append("</table>");
                return GridDT;
            }
            else
            {
                return null;
            }

        }

        private void GridColor(string GridDT, string tempobject, StringBuilder sbRow)
        {
            if (!string.IsNullOrEmpty(GridDT))
            {
                string prevItemValue = GridDT == "1" ? "on" : "off";// check if GridDt value equals to 1 then set the value on else off
                string itemValue = "";
                if (tempobject == "0" || tempobject.ToString() == null)
                    itemValue = "off";
                else
                    itemValue = "on";
                if (prevItemValue != itemValue && itemValue == "on")
                    sbRow.Append("<td bgcolor='yellow'></td>");
                else if (prevItemValue != itemValue && itemValue != "on")
                    sbRow.Append("<td bgcolor='red'></td>");
                else if (prevItemValue == itemValue && itemValue == "on")
                    sbRow.Append("<td bgcolor='green'></td>");
                else
                    sbRow.Append("<td>&nbsp;</td>");
            }
            else
            {
                if (GridDT == "ChildID") return;
                sbRow.Append("<td>&nbsp;</td>");
            }
        }

        public ActionResult RolesSearchView()
        {
            RoleViewModel oRole = new RoleViewModel();
            return View(oRole);
        }

        [HttpPost]
        public ActionResult RolesSearchView(RoleViewModel oRoleViewModel)
        {
            ModelState.Remove("RoleName");
            ModelState.Remove("SecurityGroup");
            ModelState.Remove("RoleLevel");
            ModelState.Remove("ClientName");
            ModelState.Remove("ProcessName");
            ModelState.Remove("CampaignName");
            ModelState.Remove("SearchName");
            ModelState.Remove("StartDate");
            ModelState.Remove("EndDate");
            ModelState.Remove("Approver");
            if (oRoleViewModel == null) oRoleViewModel = new RoleViewModel();
            oRoleViewModel.SearchViewList = new List<RoleViewModel>();
            oRoleViewModel.SearchViewList.AddRange(_repositoryRole.GetRoleList(oRoleViewModel.SearchName, false).Select(x =>
            new RoleViewModel { RoleId = x.iRoleID, SearchRoleName = x.sRoleName }));
            return View(oRoleViewModel);
        }

        public ActionResult RolesRequestView()
        {
            RoleViewModel oRoleModel = new RoleViewModel();
            return PartialView(oRoleModel);
        }


        /// <summary>
        /// Read a record of Role request Status and fill the grid
        /// </summary>
        /// <param name="request">DataSourceRequest</param>
        /// <param name="startDate">string</param>
        /// <param name="endDate">string</param>
        /// <returns></returns>
        public ActionResult RoleRequest_ReadP([DataSourceRequest] DataSourceRequest request, string startDate, string endDate)
        {
            RoleViewModel oRoleModel = new RoleViewModel();
            oRoleModel.RoleRequestList = new List<RoleViewModel>();
            // Get the Rercord from BLRoles Class method
            var listRoleRequest = _repositoryRole.GetRoleRequestStatus(Convert.ToDateTime(startDate), Convert.ToDateTime(endDate));
            oRoleModel.RoleRequestList.AddRange(from row in listRoleRequest
                                                select new RoleViewModel
                                                {
                                                    RequestId = row.RequestId,//  row.Field<int>("RequestId"),
                                                    RequestBy = row.RequestedBy,
                                                    RequestDesc = row.RequestDesc,
                                                    RequestDate = row.RequestedOn,
                                                    RequestApprover = row.Approver,
                                                    RequestStatus = row.RequestStatus
                                                });
            // Return the list as Json format
            return Json(oRoleModel.RoleRequestList.ToDataSourceResult(request));

        }

        [HttpPost]
        [ValidateAntiForgeryToken] // Cross-Site Request Forgery fixed
        public ActionResult CancelRequest(int RequestId)
        {
            _repositoryRole.CancelRoleRequest(RequestId, int.Parse(Resources_FormID.Roles));
            return Json(BPA.GlobalResources.UI.Resources_common.display_Ok);
        }

        [HttpGet]
        public PartialViewResult RolesApprovalView()
        {
            return PartialView();
        }

        public JsonResult EditingCustom_Edit(string sRoleID)
        {
            TempData["sRoleID"] = sRoleID;
            TempData.Keep("sRoleID");
            return Json(1);
        }

        public JsonResult RoleApproval_ReadP([DataSourceRequest] DataSourceRequest request)
        {
            RoleViewModel oRoleModel = new();
            oRoleModel.RoleApprovalList = new List<RoleViewModel>();
            var approverlist = _repositoryRole.GetRoleApprovalList(base.oUser.iUserID);// UISharedLayer.ValidateDate(UISharedLayer.ToDataTable(_repositoryRole.GetRoleApprovalList(base.oUser.iUserID))); // Get the Role Approval List

            oRoleModel.RoleApprovalList.AddRange(from row in approverlist.AsEnumerable()
                                                 select new RoleViewModel
                                                 {
                                                     RequestId = row.RequestId,
                                                     RequestBy = row.RequestedBy,
                                                     RequestDesc = row.RequestDesc,
                                                     RequestDate = Convert.ToDateTime(row.RequestedOn)
                                                 });
            return Json(oRoleModel.RoleApprovalList.ToDataSourceResult(request));
        }

        public ActionResult RoleApprove(int RequestId)
        {
            // Call a BLRoles Class Method "ApproveRoleRequest"
            _repositoryRole.ApproveRoleRequest(RequestId, int.Parse(Resources_FormID.Roles));
            return Json(BPA.GlobalResources.UI.Resources_common.display_Ok);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RoleReject(int RequestId)
        {
            _repositoryRole.RejectRoleRequest(RequestId, int.Parse(Resources_FormID.Roles));
            return Json(BPA.GlobalResources.UI.Resources_common.display_Ok);
        }
    }
}
