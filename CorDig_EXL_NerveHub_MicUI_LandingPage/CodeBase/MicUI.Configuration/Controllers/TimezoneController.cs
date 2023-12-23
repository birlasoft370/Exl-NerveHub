using Microsoft.AspNetCore.Mvc;
using MicUI.Configuration.Models;
using MicUI.Configuration.Models.ViewModels;
using MicUI.Configuration.Module.Configuration.TimeZone;
using MicUI.Configuration.Services.ServiceModel;

namespace MicUI.Configuration.Controllers
{
    public class TimezoneController : BaseController
    {
        //
        // GET: /AppConfiguration/TimeZone/
        #region private Properties

        private readonly ITimeZoneService _repositoryTimeZone;

        #endregion
        #region Constructors

        public TimezoneController(ITimeZoneService repositoryTimeZone)
        {
            this._repositoryTimeZone = repositoryTimeZone;
        }
        #endregion
        public IActionResult Index()
        {
            TimeZoneViewModel objTimeZoneViewModel = new();
            if (TempData["TimeZoneID"] == null)
            {
                TempData["TimeZoneID"] = 0;
            }

            string id = (TempData["TimeZoneID"]).ToString();
            if (int.Parse(id) != 0)
            {

                TimeZoneModel objBETimeZoneInfo = _repositoryTimeZone.GetTimeZoneById(int.Parse(id));

                objTimeZoneViewModel = DisplayRecord(objBETimeZoneInfo);

            }

            else
            {
                objTimeZoneViewModel = new();
            }



            return View(objTimeZoneViewModel);

        }
        public JsonResult GetTimeZone()
        {
            return Json(_repositoryTimeZone.GetTimeZoneList(""));
        }
        [HttpPost]

        public ActionResult TimeZoneSearchView(TimeZoneViewModel ObjTimeZobe)
        {
            ObjTimeZobe.ValidationFilter(ModelState, "btnSearch");

            if (ObjTimeZobe.lstTimeZoneview == null) { ObjTimeZobe.lstTimeZoneview = new List<BETimeZoneInfo>(); }

            ObjTimeZobe.SearchTimeZone = ObjTimeZobe.SearchTimeZone == null ? null : ObjTimeZobe.SearchTimeZone.ToString().Trim();


            ObjTimeZobe.lstTimeZoneview = _repositoryTimeZone.GetTimeZoneList(ObjTimeZobe.SearchTimeZone.ToString().Trim());

            if (ObjTimeZobe.lstTimeZoneview.Count == 0)
            {
                ViewData["Message"] = @BPA.GlobalResources.UI.Resources_common.display_msgNotFound;
            }
            return View("TimeZoneSearchView", ObjTimeZobe);
        }
        public ActionResult TimeZoneSearchView()
        {
            return View(new TimeZoneViewModel());
        }
        #region 
        [HttpPost]
        public JsonResult SetTimeZoneID(string Timneid)
        {

            int returnValue = 0;
            if (Timneid != null)
            {
                TempData["TimeZoneID"] = Timneid;
                TempData.Keep("TimeZoneID");
                returnValue = 1;

            }
            return Json(returnValue);
        }
        private TimeZoneViewModel DisplayRecord(TimeZoneModel oTimeZone)
        {
            TimeZoneViewModel objTimeZoneViewModel = new TimeZoneViewModel();
            objTimeZoneViewModel.TimeZoneID = int.Parse(oTimeZone.TimeZoneID.ToString());
            objTimeZoneViewModel.TimeZoneName = oTimeZone.TimeZoneName;
            objTimeZoneViewModel.Description = oTimeZone.Description;
            objTimeZoneViewModel.OffSetGMT = oTimeZone.OffSetGMT;
            objTimeZoneViewModel.Disabled = oTimeZone.Disabled;

            return objTimeZoneViewModel;
        }

        #endregion
    }
}
