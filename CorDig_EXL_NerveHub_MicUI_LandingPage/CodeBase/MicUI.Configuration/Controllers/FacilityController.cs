using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MicUI.Configuration.Models.ViewModels;
using MicUI.Configuration.Module.Administration.Facility;
using MicUI.Configuration.Module.Configuration.ClientInfoSetup;
using MicUI.Configuration.Module.Configuration.ProcessInfoSetup;
using MicUI.Configuration.Services.ServiceModel;

namespace MicUI.Configuration.Controllers
{
    public class FacilityController : BaseController
    {

        private readonly IFacilityService _repositoryFacility;
        private readonly ISBUInfoService _repositorySBU;
        private readonly ILocationService _repositoryLocation;
        public FacilityController
          (
            IFacilityService repositoryFacility,
            ISBUInfoService repositorySBU,
            ILocationService repositoryLocation
          )
        {
            _repositoryFacility = repositoryFacility;
            _repositorySBU = repositorySBU;
            _repositoryLocation = repositoryLocation;

        }

        /// <summary>
        /// Loads the Index View
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            if (TempData["iFacilityID"] != null)
            {
                if (TempData["iFacilityID"].ToString() != "0")
                {
                    return View("Index", FillSearch());
                }
                else
                {
                    return View(new FacilityViewModel());
                }

            }
            else
            {
                return View(new FacilityViewModel());
            }
        }
        [HttpGet]
        public ActionResult FacilitySearchView()
        {
            var objFacility = new FacilityViewModel();
            return View(objFacility);
        }
        public JsonResult JsonGetLocations()
        {
            return Json(GetLocations());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FacilitySearchView(FacilityViewModel objFacility)
        {
            try
            {
                objFacility.lstFacilities = _repositoryFacility.GetFacilityList((objFacility.sSearchText == null ? "" : objFacility.sSearchText.Trim()), false);
            }
            catch (Exception ex)
            { throw ex; }

            return View(objFacility);
        }
        [HttpPost]

        public JsonResult SetTempFacility(int iFacilityID)
        {
            TempData["iFacilityID"] = iFacilityID;
            TempData.Keep("iFacilityID");

            return Json(1);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(FacilityViewModel oFacility)
        {

            try
            {
                FacilityModel model = new FacilityModel();
                model.FacilityID = oFacility.iFacilityID;
                model.FacilityName = oFacility.sFacilityName;
                model.FacilityDescription = oFacility.sFacilityDescription;
                model.LocationID = oFacility.iLocationID;

                if (oFacility.iFacilityID > 0)
                {
                    _repositoryFacility.UpdateData(model);
                    ViewData["Message"] = @BPA.GlobalResources.UI.AppConfiguration.Resources_Facility.displayFacilityUpdated;
                }
                else
                {
                    _repositoryFacility.InsertData(model);
                    ViewData["Message"] = @BPA.GlobalResources.UI.AppConfiguration.Resources_Facility.displayFacilitySaved;
                }

                ModelState.Clear();
                oFacility = new FacilityViewModel();

            }

            catch (Exception ex)
            {
                throw ex;
            }

            return View("Index", oFacility);

        }
        public SelectList GetLocations()
        {
            var tempLstLocation = _repositoryLocation.GetLocationList("");
            IList<SelectListItem> Locations = new List<SelectListItem>();
            foreach (var item in tempLstLocation)
            {
                Locations.Add(new SelectListItem { Text = item.LocationName, Value = item.LocationID.ToString() });
            }
            return new SelectList(Locations, "Value", "Text");

        }
        private FacilityViewModel FillSearch()
        {
            FacilityViewModel objFacility = new FacilityViewModel();
            try
            {
                var SelectedFacility = _repositoryFacility.GetFacilityById(int.Parse(TempData["iFacilityID"].ToString()));
                if (SelectedFacility != null)
                {
                    objFacility.iFacilityID = SelectedFacility.FacilityID;
                    objFacility.iLocationID = SelectedFacility.LocationID;
                    objFacility.sFacilityName = SelectedFacility.FacilityName;
                    objFacility.sFacilityDescription = SelectedFacility.FacilityDescription;
                    objFacility.bDisabled = SelectedFacility.Disabled;
                }
                TempData["iFacilityID"] = 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return objFacility;
        }
    }
}