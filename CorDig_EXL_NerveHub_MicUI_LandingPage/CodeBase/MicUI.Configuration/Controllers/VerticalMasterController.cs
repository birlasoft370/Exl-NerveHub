using BPA.GlobalResources.UI.AppConfiguration;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using MicUI.Configuration.Models.ViewModels;
using MicUI.Configuration.Module.Configuration.ClientInfoSetup;
using MicUI.Configuration.Services.ServiceModel;

namespace MicUI.Configuration.Controllers
{

    public class VerticalMasterController : BaseController
    {
        private readonly IVerticalService _repositaryVertical;

        public VerticalMasterController(IVerticalService VerticalService)
        {
            this._repositaryVertical = VerticalService;
        }
        //
        // GET: /AppConfiguration/VErticalMaster/
        public ActionResult Index(string iVerticalMasterID)
        {
            VerticalMasterViewModel ObjVertical = new VerticalMasterViewModel();
            if (iVerticalMasterID != null)
            {
                VerticalMasterModel ObjListBE = null;

                ObjListBE = _repositaryVertical.GetVerticalMasterById(Convert.ToInt32(iVerticalMasterID));

                if ( ObjListBE!=null)
                {
                    ObjVertical.VerticalID = ObjListBE.VerticalID;
                    ObjVertical.ERPID = ObjListBE.ERPID;
                    ObjVertical.VerticaName = ObjListBE.VerticaName;
                    ObjVertical.VerticaDescription = ObjListBE.VerticalDescription;
                    ObjVertical.Disable = ObjListBE.Disabled;
                }
          
            }
            return View(ObjVertical);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ObjVertical"></param>
        /// <returns></returns>
        [HttpPost]
        // [ValidateAntiForgeryToken]
        public ActionResult Index(VerticalMasterViewModel ObjVertical)
        {
            try
            {
                if (ObjVertical.VerticalID == 0)
                {
                    this._repositaryVertical.InsertData(CatchRecords(ObjVertical));
                    ViewData["Message"] = Resources_Vertical.display_Save_msg;
                }
                else
                {
                    this._repositaryVertical.UpdateData(CatchRecords(ObjVertical));
                    ViewData["Message"] = Resources_Vertical.display_update_msg;
                }
                ModelState.Clear();
                ObjVertical = new VerticalMasterViewModel();
            }

            catch (Exception ex)
            {
                throw ex;
            }
            return View(ObjVertical);
        }

        public VerticalMasterModel CatchRecords(VerticalMasterViewModel ObjVertical)
        {
            VerticalMasterModel objBEVerticalMaster = new VerticalMasterModel();

            objBEVerticalMaster.VerticalID = ObjVertical.VerticalID;
            objBEVerticalMaster.ERPID = ObjVertical.ERPID.GetValueOrDefault();
            objBEVerticalMaster.VerticaName = ObjVertical.VerticaName;
            objBEVerticalMaster.VerticalDescription = ObjVertical.VerticaDescription;
            objBEVerticalMaster.Disabled = Convert.ToBoolean(ObjVertical.Disable);


            return objBEVerticalMaster;

        }
        [HttpGet]
        public ActionResult VerticalSearchView()
        {
            VerticalMasterViewModel ObjTMaster = new VerticalMasterViewModel();

            return View(ObjTMaster);
        }
        private VerticalMasterViewModel GetList(string VName)
        {
            VerticalMasterViewModel ObjTMaster = new VerticalMasterViewModel();

            IList<BEVerticalInfo> ObjListBE = null;
        
            if (VName == null)
            {
                ObjListBE = GetModel(this._repositaryVertical.GetVerticalMasterList(""));
                ObjTMaster.VerticaMasterList = new List<VerticalMasterViewModel>();
            }
            else
            {
                ObjListBE = GetModel(this._repositaryVertical.GetVerticalMasterList(VName));
                ObjTMaster.VerticaMasterList = new List<VerticalMasterViewModel>();
            }
            // }
            foreach (var item in ObjListBE)
            {
                ObjTMaster.VerticaMasterList.Add(new VerticalMasterViewModel { ERPID = item.iERPID, VerticaName = item.sVerticalName + " " + (item.bDisabled == true ? "(Disabled)" : ""), VerticaDescription = item.sDescription, EncryptVerticaMasterID = item.iVerticalID.ToString() });
            }
            ObjListBE = null;
            return ObjTMaster;
        }
        public ActionResult Vertical_ReadP([DataSourceRequest] DataSourceRequest request)
        {

            return Json(GetList(null).VerticaMasterList.ToDataSourceResult(request));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult VerticalSearchView(VerticalMasterViewModel ObjTMaster)
        {
            VerticalMasterViewModel ObjTMaster_ = GetList(ObjTMaster.VerticaName);
            if (ObjTMaster_.VerticaMasterList.Count <= 0)
            {
                ViewData["Message"] = @BPA.GlobalResources.UI.Resources_common.dispNoRecordFound;
            }
            return View(ObjTMaster_);
        }
        [NonAction]
        private IList<BEVerticalInfo> GetModel(List<VerticalMasterModel> list)
        {
            IList<BEVerticalInfo> verticalInfos = new List<BEVerticalInfo>();
            foreach (var info in list)
            {
                BEVerticalInfo lobObj = new BEVerticalInfo();
                lobObj.iVerticalID = info.VerticalID;
                lobObj.iERPID = int.Parse(info.ERPID.ToString());
                lobObj.bDisabled = Convert.ToBoolean(info.Disabled);
                lobObj.sVerticalName = info.VerticaName;
                lobObj.sDescription = info.VerticalDescription;
                verticalInfos.Add(lobObj);
            }
            return verticalInfos;
        }





    }
}