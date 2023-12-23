using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Security.Application;
using MicUI.Configuration.Helper;
using MicUI.Configuration.Models;
using MicUI.Configuration.Models.ViewModels;
using MicUI.Configuration.Module.Authentication;
using MicUI.Configuration.Module.Security;
using MicUI.Configuration.Services.ServiceModel;
using System.Data;

namespace MicUI.Configuration.Controllers
{
    public class PowerUserController : BaseController
    {
        private readonly IPermissionService _repositoryPermission;
        private readonly IAuthenticationTokenService _repositoryAuthenticate;
        public PowerUserController(IPermissionService repositoryPermission, IAuthenticationTokenService repositoryAuthenticate)
        {
            _repositoryPermission = repositoryPermission;
            _repositoryAuthenticate = repositoryAuthenticate;
        }


        public IActionResult Index()
        {
            PowerUserViewModel objPowerUserViewModel = new PowerUserViewModel();
            if (TempData["UserId"] != null)
            {
                int UserId = Convert.ToInt32(TempData["UserId"]);
                BEUserInfo objBEUserInfo = new();
                objBEUserInfo = _repositoryPermission.GetUserDetailsWithRole(UserId);

                objPowerUserViewModel.UserId = UserId;
                objPowerUserViewModel.LoginName = objBEUserInfo.sLoginName;
                objPowerUserViewModel.Email = objBEUserInfo.sEmail;
                objPowerUserViewModel.EmployeeId = objBEUserInfo.iEmployeeID;
                objPowerUserViewModel.FirstName = objBEUserInfo.sFirstName;
                objPowerUserViewModel.LastName = objBEUserInfo.sLastName;
                if (objBEUserInfo.oRoles.Count > 0)
                {
                    objPowerUserViewModel.Role = objBEUserInfo.oRoles[objBEUserInfo.oRoles.Count - 1].iRoleID;
                }
                else
                {
                    objPowerUserViewModel.Role = objBEUserInfo.oRoles[0].iRoleID;
                }
                objPowerUserViewModel.JobId = objBEUserInfo.iJobID;

                objPowerUserViewModel.PendingApproval = objBEUserInfo.iJobID;
                objPowerUserViewModel.MiddleName = objBEUserInfo.sMiddleName;
                objPowerUserViewModel.Disabled = objBEUserInfo.bDisabled;
                var dsList = _repositoryPermission.GetUserRoleApproverList(objPowerUserViewModel.Role);
                if (dsList.Count > 0)
                {
                    objPowerUserViewModel.ApproverList = DataTableToList(UISharedLayer.ToDataTable(dsList));
                }
                else
                {
                    objPowerUserViewModel.ApproverList = Enumerable.Empty<PowerUserViewModel.clsApprover>();
                }
            }
            return View(objPowerUserViewModel);
        }

        public List<PowerUserViewModel.clsApprover> DataTableToList(DataTable dt)
        {
            List<PowerUserViewModel.clsApprover> lstApprover = new List<PowerUserViewModel.clsApprover>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                PowerUserViewModel.clsApprover onjApprover = new();
                onjApprover.UserId = Int32.Parse(Convert.ToString(dt.Rows[i]["UserId"]) ?? "");
                onjApprover.ApproverName = Convert.ToString(dt.Rows[i]["Agent"]) ?? "";
                lstApprover.Add(onjApprover);
            }
            return lstApprover;
        }
        public JsonResult GetLdapUsers([DataSourceRequest] DataSourceRequest request, string lanId)
        {
            List<PowerUserViewModel.clsApprover> objLdapUserList = new List<PowerUserViewModel.clsApprover>();

            if (!String.IsNullOrEmpty(lanId))
            {
                string Domain = GlobalConstant.Domain;
                IList<BELdapUserInfo> ldapUserList = _repositoryAuthenticate.GetUserList(lanId, Domain);

                if (ldapUserList.Count > 0)
                {
                    for (int i = 0; i < ldapUserList.Count; i++)
                    {
                        if (ldapUserList[i].UserName != null)
                        {
                            string FirstName = string.Empty;
                            string LastName = string.Empty;
                            string MiddleName = string.Empty;
                            int EmployeeId = 0;
                            string[] arrUser = ldapUserList[i].UserName.Split('-');

                            FirstName = arrUser[0];
                            LastName = arrUser[1];

                            objLdapUserList.Add(new PowerUserViewModel.clsApprover
                            {
                                FirstName = FirstName,
                                LastName = LastName,
                                MiddleName = MiddleName,
                                UserName = ldapUserList[i].UserName,
                                LanId = arrUser[0].Trim(),
                                EmployeeId = EmployeeId,
                                Email = ldapUserList[i].EmailID,
                                IsDisabled = ldapUserList[i].bDisabled
                            });
                        }
                    }
                }
                return Json(objLdapUserList.ToDataSourceResult(request));
            }
            else
            {
                return Json(objLdapUserList.ToDataSourceResult(request));
            }
        }

        [HttpPost]
        public JsonResult GetApprover(int iRoleId)
        {
            if (iRoleId != 0)
            {
                DataTable ds = UISharedLayer.ToDataTable(_repositoryPermission.GetUserRoleApproverList(iRoleId));
                return Json(DataTableToList(ds));
            }
            else
            {
                return Json(new List<PowerUserViewModel.clsApprover>());
            }
        }

        public ActionResult SearchView(string userName)
        {
            PowerUserViewModel objPowerUserViewModel = new PowerUserViewModel();
            return View(objPowerUserViewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SearchView(PowerUserViewModel oModel)
        {
            oModel.ApproverList = GetUserList(oModel.UserName);
            return View(oModel);
        }
        [NonAction]
        public IList<PowerUserViewModel.clsApprover> GetUserList(string userName)
        {
            List<PowerUserViewModel.clsApprover> objuserList = new List<PowerUserViewModel.clsApprover>();

            IList<BEUserInfo> objBEUserInfoList = new List<BEUserInfo>();

            if (userName == null)
            {
                objBEUserInfoList = _repositoryPermission.GetUserList("", true, string.Empty);
                objuserList = new List<PowerUserViewModel.clsApprover>();
            }
            else
            {
                objBEUserInfoList = _repositoryPermission.GetUserList(userName, true, string.Empty);
                objuserList = new List<PowerUserViewModel.clsApprover>();
            }

            for (int i = 0; i < objBEUserInfoList.Count; i++)
            {
                objuserList.Add(new PowerUserViewModel.clsApprover
                {
                    ApproverName = objBEUserInfoList[i].Name,
                    UserId = objBEUserInfoList[i].iUserID
                });

            }

            return objuserList;

        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public JsonResult SaveUpdatePowerUser(PowerUserViewModel objPowerUserViewModel)
        {
            try
            {

                if (objPowerUserViewModel.UserId == 0)
                {
                    if (objPowerUserViewModel.UserId == base.oUser.iUserID)
                    {
                        return Json("notallowed");
                    }
                    else
                    {
                        _repositoryPermission.InsertUserRoleData(CatchRecord(objPowerUserViewModel), 1, int.Parse(BPA.GlobalResources.Resources_FormID.PowerUser));
                        return Json(1);
                    }
                }
                else
                {
                    if (objPowerUserViewModel.JobId == 1)
                    {
                        return Json("pending");
                    }
                    else
                    {
                        if (objPowerUserViewModel.UserId == base.oUser.iUserID)
                        {
                            return Json("notallowed");
                        }
                        else
                        {
                            _repositoryPermission.InsertUserRoleData(CatchRecord(objPowerUserViewModel), 2, int.Parse(BPA.GlobalResources.Resources_FormID.PowerUser));
                            return Json(2);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        [NonAction]
        private PowerUserInfo CatchRecord(PowerUserViewModel objPowerUserViewModel)
        {
            PowerUserInfo objPowerUserInfo = new PowerUserInfo();
            if (objPowerUserViewModel.UserId > 0)
            {
                objPowerUserInfo.ModifyDate = DateTimeTimeZoneConversion.GetCurrentDateTime(true, base.oUser.sUserTimeZone, base.oUser.sServerTimeZone);
                objPowerUserInfo.ModifiedBy = base.oUser.iUserID;
            }
            else
            {
                objPowerUserInfo.CreatedBy = base.oUser.iUserID;
                // oUser.iCreatedBy = base.oUser.iUserID;
            }
            objPowerUserInfo.LoginName = Encoder.HtmlEncode(objPowerUserViewModel.LoginName, false);
            objPowerUserInfo.Email = Encoder.HtmlEncode(objPowerUserViewModel.Email, false);
            objPowerUserInfo.FirstName = Encoder.HtmlEncode(objPowerUserViewModel.FirstName, false);
            objPowerUserInfo.MiddleName = Encoder.HtmlEncode(objPowerUserViewModel.MiddleName, false);
            objPowerUserInfo.LastName = Encoder.HtmlEncode(objPowerUserViewModel.LastName, false);
            objPowerUserInfo.Disabled = Convert.ToBoolean(objPowerUserViewModel.Disabled);
            objPowerUserInfo.EmployeeID = objPowerUserViewModel.EmployeeId;
            objPowerUserInfo.LanID = objPowerUserViewModel.LanId;
            PowerRoleInfo oRoles = new PowerRoleInfo();
            oRoles.RoleID = objPowerUserViewModel.Role;
            oRoles.CreatedBy = objPowerUserViewModel.Approver;
            objPowerUserInfo.Roles.Add(oRoles);

            objPowerUserInfo.RoleApprover = objPowerUserViewModel.Role;//276
            objPowerUserInfo.Approver = objPowerUserViewModel.Approver;//116877

            return objPowerUserInfo;
        }

        [HttpPost]
        //[ValidateAntiForgeryToken] // Cross-Site Request Forgery fixed
        public JsonResult SetEditableUserId(string tempUserId)
        {
            TempData["UserId"] = tempUserId;
            TempData.Keep("UserId");
            return Json(BPA.GlobalResources.UI.Resources_common.display_Ok);
        }
    }
}
