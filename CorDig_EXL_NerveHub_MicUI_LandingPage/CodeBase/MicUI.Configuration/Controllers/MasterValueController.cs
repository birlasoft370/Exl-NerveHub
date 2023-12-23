using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using MicUI.Configuration.Models.ViewModels;
using MicUI.Configuration.Module.Administration.MasterValue;
using Newtonsoft.Json;

namespace MicUI.Configuration.Controllers
{
    public class MasterValueController : BaseController
    {
        private readonly IMasterValueService _masterValueService;
        public MasterValueController(IMasterValueService masterValueService)
        {
            _masterValueService = masterValueService;
        }

        public IActionResult Index()
        {
            MasterValueViewModel objMasterViewModel;
            TempData["MasterValueID"] = "1050";
            if (TempData["MasterValueID"] == null)
            {
                TempData["MasterValueID"] = 0;
            }

            string id = TempData["MasterValueID"].ToString();
            if (id == "0")
            {
                objMasterViewModel = new MasterValueViewModel();
            }
            else
            {
                objMasterViewModel = new MasterValueViewModel();
                objMasterViewModel = this.DisplayRecord(this._masterValueService.GetMasterDetails(Convert.ToInt32(id))); // tonken?
            }
            return View(objMasterViewModel);
        }
        [NonAction]
        private MasterValueViewModel DisplayRecord(MasterValueModel oBEMasterType)
        {
            MasterValueViewModel oMasterValueModel = new()
            {
                MasterValueID = oBEMasterType.MasterValueID,
                MasterType = oBEMasterType.MasterType,
                bDisable = oBEMasterType.Disabled             
                
            };
            foreach (var item in oBEMasterType.ValueList)
            {
                oMasterValueModel.ValueList.Add(new MasterValueViewModel { FieldID = item.FieldID, Values = item.Values, Disable = item.Disabled });
            }
            TempData["cal"] = JsonConvert.SerializeObject(oMasterValueModel);
            //var result = JsonConvert.DeserializeObject<MasterValueViewModel>(TempData["cal"].ToString());
            return oMasterValueModel;
        }    

        public ActionResult MasterValue_Create([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")] IEnumerable<MasterValueList> MasterValue, FormCollection frm)
        {
            var results = new List<MasterValueList>();

            if (MasterValue != null && ModelState.IsValid)
            {
                foreach (var values in MasterValue)
                {
                    // productService.Create(product);
                    results.Add(values);
                }
            }

            return Json(results.ToDataSourceResult(request, ModelState, p => new MasterValueList { Values = results[0].Values, Disable = results[0].Disable }));
        }

        public ActionResult MasterValue_Update([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")] IEnumerable<MasterValueList> MasterValue, FormCollection frm)
        {


            if (MasterValue != null && ModelState.IsValid)
            {
                foreach (var week in MasterValue)
                {
                    //productService.Update(product);
                }
            }

            return Json(MasterValue.ToDataSourceResult(request, ModelState));
        }
        public ActionResult MasterValue_Destroy([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")] IEnumerable<MasterValueList> MasterValue)
        {

            foreach (var week in MasterValue)
            {
                // productService.Destroy(product);
            }


            return Json(MasterValue.ToDataSourceResult(request, ModelState));
        }
        public ActionResult MasterValue_Read([DataSourceRequest] DataSourceRequest request)
        {
            if (TempData["cal"] != null)
            {
                //mpData.Keep("cal");
                var result= JsonConvert.DeserializeObject<MasterValueViewModel>(TempData["cal"].ToString());
                return Json(GetList(result).ValueList.ToDataSourceResult(request));
            }
            else
            {
                return Json(0);
            }
        }
        private MasterValueViewModel GetList(MasterValueViewModel objMasterValue)
        {
            //foreach (var item in objMasterValue.ValueList)
            //{
            //    var obj = new MasterValueList();
            //    obj.Values = item.Values.ToString();
            //    obj.Disable = item.Disable;
            //    objMasterValue.ValueList.Add(obj);                
            //}
            return objMasterValue;
        }
        public ActionResult MasterValueSearchView()
        {

            return View(new MasterValueViewModel());
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult MasterValueSearchView(MasterValueViewModel oMasterValueModel)
        {
            if (oMasterValueModel == null) oMasterValueModel = new MasterValueViewModel();
            //if (string.IsNullOrEmpty(oDefectModel.SearchDefectName)) return View(oDefectModel);
            //IList<BEMasterType> lstBEMasterType = null;
            var lstBEMasterType = this._masterValueService.GetMasterList(oMasterValueModel.MasterValueSearchName, false);
            oMasterValueModel.MasterValueList = new List<MasterValueViewModel>();
            foreach (var item in lstBEMasterType)
            {

                oMasterValueModel.MasterValueList.Add(new MasterValueViewModel { MasterValueID = item.MasterValueID, MasterValueSearchName = item.MasterType });
            }
            if (oMasterValueModel.MasterValueList.Count <= 0)
            {
                ViewData["Message"] = @BPA.GlobalResources.UI.Resources_common.display_msgNotFound;
            }
            return View(oMasterValueModel);
        }

        #region Method to Set MasterValue for delete and update

        // Method To set Client Id For Update and Delete 
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public JsonResult SetMasterValueID(string id)
        {
            int returnValue = 0;
            if (id != null)
            {
                TempData["MasterValueID"] = id;
                TempData.Keep("MasterValueID");
                returnValue = 1;

            }
            return Json(returnValue);
        }

        #endregion
        [HttpPost]
      //  [ValidateAntiForgeryToken]
        public JsonResult InsertData(string strParameter, string FieldID, string Values, string Disable)
        {
            string result = string.Empty;
            try
            {

                string[] cstrParameter = strParameter.Split(';');
               
                if (Convert.ToInt32(cstrParameter[0].ToString()) == 0)
                {
                    this._masterValueService.InsertData(CatchRecordNew(strParameter, FieldID, Values, Disable), 159);
                    result = @BPA.GlobalResources.UI.AppConfiguration.Resources_MasterValue.disp_SaveMasterValue;
                }
                else
                {
                    this._masterValueService.UpdateData(CatchRecordNew(strParameter, FieldID, Values, Disable), 159);
                    result = BPA.GlobalResources.UI.AppConfiguration.Resources_MasterValue.disp_UpdatedMasterValue;
                }
            }
            catch (Exception ex)
            {
                var result_ = new { strResult = "", success = false, message = ex.Message.ToString() };
                return Json(result_);

            }

            var vresult = new { strResult = result, success = true, message = "" };
            return Json(vresult);
        }
        private MasterValueModel CatchRecordNew(string strParameter, string FieldID, string Values, string Disable)
        {
            string[] cstrParameter = strParameter.Split(';');
            string[] cstrValues = Values.Split(';');
            string[] cstrFieldID = FieldID.Split(';');
            string[] cstrDisable = Disable.Split(';');
            var model = new MasterValueModel()
            {
                MasterValueID = Convert.ToInt32(cstrParameter[0].ToString()),
                MasterType = cstrParameter[1].ToString(),
                Disabled = Convert.ToBoolean(cstrParameter[2] == "0" ? false : true),
                ValueList = new List<MasterValueLists> { new MasterValueLists() { Disabled = Convert.ToBoolean(cstrDisable[0] == "0" ? false : true),FieldID= Convert.ToInt32(cstrFieldID[1].ToString()), Values= cstrValues[1].ToString() } }
            };
            return model;
        }
    }
}
