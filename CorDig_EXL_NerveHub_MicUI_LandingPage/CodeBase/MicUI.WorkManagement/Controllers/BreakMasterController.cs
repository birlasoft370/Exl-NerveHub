using BPA.GlobalResources;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using MicUI.WorkManagement.Models.ViewModels;
using MicUI.WorkManagement.Module.Administration;
using MicUI.WorkManagement.Services.ServiceModel;

namespace MicUI.WorkManagement.Controllers
{
    public class BreakMasterController : BaseController
    {
        private readonly IBreakService _repositoryBreak;
        public BreakMasterController(IBreakService repositoryBreak)
        {
            this._repositoryBreak = repositoryBreak;
        }
        public IActionResult Index()
        {
            if (!base.CheckViewPermission(int.Parse(Resources_FormID.BreakMaster)))
            {
                return RedirectToActionPermanent("AccessDenied", "Error");
            }
            
            BreakMasterViewModel objBreakMaster = new BreakMasterViewModel();

            if (TempData["BreakID"] != null)
            {
                var data = _repositoryBreak.GetBreakMasterById(int.Parse(TempData.Peek("BreakID").ToString()));
                objBreakMaster.mBreakID = data.BreakID;
                objBreakMaster.mBreakName = data.BreakName;
                objBreakMaster.mDescription = data.Description;
                objBreakMaster.mIsdisable = data.Disabled;
            }
            else
            {
                objBreakMaster = new BreakMasterViewModel();
                ModelState.Clear();
            }

            return View(objBreakMaster);
        }
        [HttpPost]
       
        public ActionResult Index(BreakMasterViewModel objBreakMaster)
        {
            try
            {
                if (objBreakMaster.mBreakID != 0)
                {
                    if (string.IsNullOrEmpty(objBreakMaster.mBreakName))
                    {
                        ModelState.AddModelError("mBreakName", BPA.GlobalResources.UI.WorkManagement.Resources_Break.reqired_BreakName);
                    }
                    else
                    {
                        Update(objBreakMaster);
                        ModelState.Clear();
                        ViewData["Message"] = BPA.GlobalResources.UI.WorkManagement.Resources_Break.display_Update_Message;
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(objBreakMaster.mBreakName))
                    {
                        ModelState.AddModelError("mBreakName", BPA.GlobalResources.UI.WorkManagement.Resources_Break.reqired_BreakName);
                    }
                    else
                    {
                        if (ModelState.IsValid)
                        {
                            _repositoryBreak.AddBreakMaster(CatchRecord(objBreakMaster));
                            ModelState.Clear();
                            ViewData["Message"] = BPA.GlobalResources.UI.WorkManagement.Resources_Break.display_Save_Message;
                        }

                    }

                }
            }
            catch (Exception ex)
            {
                ViewData["Message"] = ex.Message;
            }
            objBreakMaster = new BreakMasterViewModel();
            return View("Index", objBreakMaster);
        }
        [HttpPost]
        public JsonResult SetBreakID(string breakId)
        {
            int returnValue = 0;
            if (breakId != null)
            {
                TempData["BreakID"] = breakId;
                TempData.Keep("BreakID");
                returnValue = 1;
            }
            return Json(returnValue);
        }
        public void Update(BreakMasterViewModel objBreakMaster)
        {
            _repositoryBreak.UpdateBreakMaster(CatchRecord(objBreakMaster));

        }
        private BreakMasterModel CatchRecord(BreakMasterViewModel objBreakMaster)
        {
            BreakMasterModel objBEBreakInfo = new BreakMasterModel();
            
                objBEBreakInfo.BreakID = objBreakMaster.mBreakID;
                objBEBreakInfo.BreakName = objBreakMaster.mBreakName.ToString();
                objBEBreakInfo.Description = objBreakMaster.mDescription;
                objBEBreakInfo.Disabled = objBreakMaster.mIsdisable;
                objBEBreakInfo.UserId = base.oUser.iUserID;
                return objBEBreakInfo;
            
        }
        public ActionResult BreakMasterSearchView()
        {
            return View("BreakMasterSearchView", new BreakMasterViewModel());
        }
        public ActionResult BreakInfoList([DataSourceRequest] DataSourceRequest request, string mBreakName)
        {
            BreakMasterViewModel objBreakMasterViewModel = new BreakMasterViewModel();
            if (string.IsNullOrEmpty(mBreakName))
            {
                objBreakMasterViewModel.BEBreakInfoList = DisplayRecord(_repositoryBreak.GetBreakInfoList(string.Empty));
            }
            else
            {
                objBreakMasterViewModel.BEBreakInfoList = DisplayRecord(_repositoryBreak.GetBreakInfoList(mBreakName));
            }
            if (objBreakMasterViewModel.BEBreakInfoList.Count <= 0)
            {
                ViewData["Message"] = @BPA.GlobalResources.UI.Resources_common.display_msgNotFound;
            }

            return Json(objBreakMasterViewModel.BEBreakInfoList.ToDataSourceResult(request));
        }
        private List<BEBreakInfo> DisplayRecord(List<BreakMasterModel> breakMasterModelList)
        {
            List<BEBreakInfo> list = new();
            foreach (var item in breakMasterModelList)
            {
                BEBreakInfo bETerminationCodeInfo = new();
                bETerminationCodeInfo.bDisabled = item.Disabled;
                bETerminationCodeInfo.iBreakID = item.BreakID;
                bETerminationCodeInfo.sBreakName = item.BreakName;
                bETerminationCodeInfo.sBreakDescription = item.Description;
                bETerminationCodeInfo.iCreatedBy = item.UserId;
                bETerminationCodeInfo.iModifiedBy = item.UserId;
                list.Add(bETerminationCodeInfo);
            }
            return list;

        }
    }

}
