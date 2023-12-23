using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using MicUI.Configuration.Models.ViewModels;
using MicUI.Configuration.Module.Security;
using MicUI.Configuration.Services.ServiceModel;

namespace MicUI.Configuration.Controllers
{
    public class ERPJobRoleMapController : BaseController
    {
        private readonly IERPJobUserRoleMapService _ERPJobRoleMapService;
        private readonly IRolesService _repositoryRole;
        public ERPJobRoleMapController(IERPJobUserRoleMapService ERPJobRoleMapService, IRolesService repositoryRole)
        {
            _ERPJobRoleMapService = ERPJobRoleMapService;
            _repositoryRole = repositoryRole;
        }
        public IActionResult Index()
        {
            ERPJobRoleMapViewModel objeRPJobRoleMapViewModel = new();

            int filterId = 0;
            if (TempData["FilterId"] != null)
            {
                filterId = Convert.ToInt32(TempData["FilterId"]);

                TempData["iMode"] = "2";
                TempData.Keep("iMode");
                // ViewBag.iMode = "2";
            }
            else
            {
                TempData["iMode"] = "1";
                TempData.Keep("iMode");
               
            }
            if (filterId > 0)
            {
                List<BEErpJobRoleMap> objBEerpjobrolemap = _ERPJobRoleMapService.GetJobRoleMap(filterId);

                objeRPJobRoleMapViewModel.iJobCode = objBEerpjobrolemap[0].oJob.iJobCode;
                objeRPJobRoleMapViewModel.sRoleName = objBEerpjobrolemap[0].oRole.iRoleID.ToString();
                objeRPJobRoleMapViewModel.iMappedOn = Convert.ToInt32(objBEerpjobrolemap[0].iMappedOn);
                objeRPJobRoleMapViewModel.bDefaultRole = objBEerpjobrolemap[0].bDefaultRole;
                objeRPJobRoleMapViewModel.bDisable = objBEerpjobrolemap[0].bDisable;
                objeRPJobRoleMapViewModel.iApprover = objBEerpjobrolemap[0].iApprover;
                objeRPJobRoleMapViewModel.iMode = "2";
                TempData["iMode"] = "2";
                TempData.Keep("iMode");
                
            }
            else
            {
                objeRPJobRoleMapViewModel.iMode = "1";
                TempData["iMode"] = "1";
                TempData.Keep("iMode");
                //ViewBag.iMode = "1";
            }

            return View(objeRPJobRoleMapViewModel);
        }
        /// <summary>
        /// SaveERPJobRoleMappingData
        /// </summary>
        /// <param name="modelData"></param>
        /// <returns></returns>
        [HttpPost]
       // [ValidateAntiForgeryToken]
        public ActionResult SaveERPJobRoleMappingData([Bind(Prefix = "modelData")] ERPJobRoleMapViewModel modelData)
        {        
            try
            {
                _ERPJobRoleMapService.AddERPJobData(CatchRecordSave(modelData), int.Parse(BPA.GlobalResources.Resources_FormID.ERPJobRoleMapping));
            }
            catch (ApplicationException ex)
            {
                return Json(new { success = false, responseText = ex.Message.ToString() });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, responseText = ex.Message.ToString() });
            }

            return Json(new { success = true, responseText = "message successfuly sent!" });



        }
        private ErpJobRoleMap CatchRecordSave(ERPJobRoleMapViewModel objERPJobRoleMapping)
        {

            ErpJobRoleMap objRoleJob = new ErpJobRoleMap();
            objRoleJob.RoleName = objERPJobRoleMapping.sRoleName;
            objRoleJob.jobDescription = objERPJobRoleMapping.sJobDesc;
            objRoleJob.iMappedOn = objERPJobRoleMapping.iMappedOn;
            objRoleJob.bDisabled = objERPJobRoleMapping.bDisable;
            objRoleJob.bDefaultRole = objERPJobRoleMapping.bDefaultRole;
            objRoleJob.CreatedBy = base.oUser.iUserID;
            objRoleJob.ApproverId = objERPJobRoleMapping.iApprover;
            objRoleJob.iMode = TempData["iMode"].ToString();
            objRoleJob.RoleId = objERPJobRoleMapping.iRoleID;
            objRoleJob.jobId = objERPJobRoleMapping.iJobCode;
            return objRoleJob;
        }

        /// <summary>
        /// GetJob
        /// </summary>
        /// <returns></returns>
        public JsonResult JsonGetJobCode()
        {
            var tempLstJobCode = _ERPJobRoleMapService.GetJob();
            return Json(tempLstJobCode);
        }
        /// <summary>
        /// GetRoleList
        /// </summary>
        /// <returns></returns>
        public JsonResult JsonGetRoleList()
        {
            var tempLstRoleList = _ERPJobRoleMapService.GetRoleList();

            return Json(tempLstRoleList);
        }
        /// <summary>
        /// GetUserRoleApproverList
        /// </summary>
        /// <returns></returns>
        public JsonResult JsonGetUserRoleApproverList(int iRoleID)
        {
            var list = _repositoryRole.PopulateRoleApproverDropDownList(iRoleID);
            if (list !=null)
            {
                return Json(list);
            }
            else
            {
                return Json((""));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Search()
        {
            ERPJobRoleMapViewModel ObjeRPJobRoleMapViewModel = new();
            return View(ObjeRPJobRoleMapViewModel);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="objERPJobRoleMapViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Read_ERPJobRoleMapSearch([DataSourceRequest] DataSourceRequest request, ERPJobRoleMapViewModel objERPJobRoleMapViewModel)
        {
            List<BEErpJobRoleMap> listFilter = new List<BEErpJobRoleMap>();
            listFilter = _ERPJobRoleMapService.GetJobRoleMapByName(objERPJobRoleMapViewModel.SearchJobRoleMapping);
            objERPJobRoleMapViewModel.lstERPJobRoleMapViewModel = new List<ERPJobRoleMapViewModel>();
            for (int i = 0; i < listFilter.Count; i++)
            {
                objERPJobRoleMapViewModel.lstERPJobRoleMapViewModel.Add(new ERPJobRoleMapViewModel
                {
                    RoleJobID = listFilter[i].iRoleJobID,
                    RoleJob = listFilter[i].RoleJob
                });
            }
            return Json(objERPJobRoleMapViewModel.lstERPJobRoleMapViewModel.ToDataSourceResult(request));
        }

        [HttpPost]       
        public JsonResult SetEditableId(string tempFilterId)
        {
            TempData["FilterId"] = tempFilterId;
            TempData.Keep("FilterId");
            return Json("1");
        }
    }
}
