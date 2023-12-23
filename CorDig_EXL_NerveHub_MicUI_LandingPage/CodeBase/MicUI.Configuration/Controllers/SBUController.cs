using BPA.GlobalResources.UI.AppConfiguration;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using MicUI.Configuration.Models.ViewModels;
using MicUI.Configuration.Module.Configuration.ClientInfoSetup;
using MicUI.Configuration.Services.ServiceModel;

namespace MicUI.Configuration.Controllers
{
    public class SBUController : BaseController
    {

        #region private Properties
        private readonly ISBUInfoService _repositorySBU;
        #endregion
        #region Constructors
        public SBUController(ISBUInfoService repositorySBU)
        {
            this._repositorySBU = repositorySBU;
        }
        #endregion
        public ActionResult Index(string SBUid)
        {
            SBUViewModel oSBUViewModel = new SBUViewModel();
            if (TempData["iSBUID"] != null)
            {
                oSBUViewModel = GetModel(_repositorySBU.GetSBUById(int.Parse(TempData["iSBUID"].ToString())));
            }
            return View(oSBUViewModel);
        }
        [HttpPost]

        public ActionResult Index(SBUViewModel SBU)
        {
            string OutPut = "";


            if (SBU.SBUID == 0)
            {
                OutPut = _repositorySBU.InsertData(BindModel(SBU));
                if (OutPut != "")
                {
                    ViewData["Message"] = ResourceSBU.msgSave;
                    ModelState.Clear();
                    SBU = new SBUViewModel();
                }
                else
                {
                    ViewData["Message"] = OutPut;
                }
            }
            else
            {
                _repositorySBU.UpdateData(BindModel(SBU));
                ViewData["Message"] = ResourceSBU.msgUpdated;
                ModelState.Clear();
                SBU = new SBUViewModel();
            }

            return View("Index", SBU);
        }
        public ActionResult SearchView()
        {
            SBUViewModel SBU = new SBUViewModel();
            return View(SBU);
        }
        public ActionResult SBU_Read([DataSourceRequest] DataSourceRequest request, string sSBUName)
        {
            List<BESBUInfo> bELOBs = new List<BESBUInfo>();
             bELOBs = _repositorySBU.GetSBUList(sSBUName);
            return Json(bELOBs.ToDataSourceResult(request));
        }
        [HttpPost]

        public JsonResult EditingCustom_Edit(string iSBUID)
        {
            TempData["iSBUID"] = iSBUID;
            TempData.Keep("iSBUID");
            return Json("OK");
        }


        /// <summary>
        /// To Get SBU model value from entity , to bind the view strongly type
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>

        private SBUModel BindModel(SBUViewModel SBUModel)
        {
            SBUModel objSBU = new();
            objSBU.ERPID = SBUModel.ERPID;
            objSBU.SBUID = SBUModel.SBUID;
            objSBU.Description = SBUModel.Description;
            objSBU.SBUName = SBUModel.SBUName;
            //objSBU.sDescription = AntiXssEncoder.HtmlEncode(SBUModel.Description, false);
            //objSBU.sName = AntiXssEncoder.HtmlEncode(SBUModel.SBUName, false);
            //  objSBU.iCreatedBy = User.iUserID;
            objSBU.IsClientSBU = SBUModel.IsClientSBU;
            objSBU.Disabled = SBUModel.Disable;
            return objSBU;
        }

        /// <summary>
        /// To Get SBU model value from entity , to bind the view strongly type
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        private SBUViewModel GetModel(SBUModel info)
        {
            SBUViewModel SBUObj = new SBUViewModel();
            SBUObj.SBUID = info.SBUID;
            SBUObj.ERPID = info.ERPID;
            SBUObj.SBUName = info.SBUName;
            SBUObj.IsClientSBU = info.IsClientSBU;
            SBUObj.Disable = info.Disabled;
            SBUObj.Description = info.Description;
            return SBUObj;
        }
    }
}
