using BPA.GlobalResources.UI.AppConfiguration;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using MicUI.Configuration.Models.ViewModels;
using MicUI.Configuration.Module.Administration.LOB;
using MicUI.Configuration.Services.ServiceModel;

namespace MicUI.Configuration.Controllers
{
    public class LOBController : BaseController
    {
        private readonly ILOBService _repositoryLOB;

        public LOBController(ILOBService repositoryLOB)
        {
            _repositoryLOB = repositoryLOB;
        }

        // [HttpGet("LOB/Index")]
        public IActionResult Index()
        {
            LOBViewModel oLOBViewModel = new();
            if (TempData["iLOBId"] != null)
            {
                oLOBViewModel = GetModel(this._repositoryLOB.GetLOBById(int.Parse(TempData["iLOBId"].ToString())));
            }
            return View(oLOBViewModel);
        }
        [HttpPost]
        public ActionResult Index(LOBViewModel LOB)
        {
            try
            {

                if (LOB.LOBID == 0)
                {
                    this._repositoryLOB.InsertData(BindModel(LOB));
                    ViewData["Message"] = ResourceLOB.display_Save;
                }
                else
                {
                    this._repositoryLOB.UpdateData(BindModel(LOB));
                    ViewData["Message"] = ResourceLOB.display_Update;
                }
                ModelState.Clear();
                LOB = new();
            }
            catch (Exception)
            {
                throw;
            }
            return View("Index", LOB);
        }

        public ActionResult LOBSearchView()
        {
            LOBViewModel LOB = new();
            return View(LOB);
        }

        public ActionResult LOB_Read([DataSourceRequest] DataSourceRequest request, string LOBname)
        {
            if (!string.IsNullOrEmpty(LOBname))
            {
                var list = this._repositoryLOB.GetLOBList(LOBname, false);
                return Json(list.ToDataSourceResult(request));
            }
            else
            {
                return Json("");
            }
        }

        [NonAction]
        private LOBModel BindModel(LOBViewModel lobModel)
        {
            LOBModel objLOB = new();
            objLOB.ERPID = lobModel.ERPID;
            objLOB.LOBID = lobModel.LOBID;
            objLOB.Description = lobModel.Description;
            objLOB.LOBName = lobModel.LOBName;
            objLOB.Disabled = lobModel.IsDisable;
            return objLOB;
        }

        [NonAction]
        private LOBViewModel GetModel(LOBModel info)
        {
            LOBViewModel lobObj = new();
            lobObj.LOBID = info.LOBID;
            lobObj.ERPID = info.ERPID;
            lobObj.IsDisable = info.Disabled;
            lobObj.LOBName = info.LOBName;
            lobObj.Description = info.Description;
            return lobObj;
        }

        [HttpPost]
        public JsonResult EditingCustom_Edit(string iLOBId)
        {
            TempData["iLOBId"] = iLOBId;
            TempData.Keep("iLOBId");
            return Json(BPA.GlobalResources.UI.Resources_common.display_Ok);
        }
    }
}
