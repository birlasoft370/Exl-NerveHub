using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using MicUI.Configuration.Models;
using MicUI.Configuration.Models.ViewModels;
using MicUI.Configuration.Module.Configuration.ProcessInfoSetup;
using MicUI.Configuration.Services.ServiceModel;

namespace MicUI.Configuration.Controllers
{
   
        public class LocationController : BaseController
        {
            #region private Properties
            private readonly ILocationService _repositoryLocation;
            #endregion


            #region Constructors
            /// <summary>
            /// 
            /// </summary>
            /// <param name="repositoryLocation"></param>
            public LocationController(ILocationService repositoryLocation)
            {
                this._repositoryLocation = repositoryLocation;
            }
            #endregion
            public IActionResult Index()
            {
                LocationViewModel ObLocation = new LocationViewModel();
                if (Convert.ToString(TempData["iLocationID"]) != "")
                {
                    ObLocation.iLocationID = Convert.ToInt32(TempData["iLocationID"]);
                    ObLocation.LocationName = Convert.ToString(TempData["LocationName"]);
                    ObLocation.LocationDesc = Convert.ToString(TempData["LocationDescription"]);
                    ObLocation.IsDisable = Convert.ToBoolean(TempData["Disabled"]);
                }
                return View(ObLocation);
            }
            [HttpGet]
            public ActionResult LocationSearchView()
            {
                LocationViewModel objLocation = new LocationViewModel();
                return View(objLocation);
            }
            private LocationModel CatchRecord(LocationViewModel location)
            {
                LocationModel objLocationInfo = new LocationModel();
                objLocationInfo.LocationID = location.iLocationID;
                objLocationInfo.LocationName = location.LocationName;
                objLocationInfo.LocationDesc = location.LocationDesc;
                objLocationInfo.Disabled = location.IsDisable;
                return objLocationInfo;
            }
            [HttpPost]

            public ActionResult Index(LocationViewModel location)
            {
                //To validate model before insert/update
                try
                {
                    if (location.iLocationID != 0)
                    {
                        _repositoryLocation.UpdateData(CatchRecord(location));
                        ViewData["Message"] = BPA.GlobalResources.UI.AppConfiguration.Resource_Location.display_LocationUpdated;
                    }
                    else
                    {
                        _repositoryLocation.InsertData(CatchRecord(location));
                        ViewData["Message"] = BPA.GlobalResources.UI.AppConfiguration.Resource_Location.display_LocationSaved;
                    }

                    ModelState.Clear();
                    location = new LocationViewModel();

                }

                catch (Exception ex)
                {
                    throw ex;
                }
                return View("Index", location);
            }
            public ActionResult GetLocationList([DataSourceRequest] DataSourceRequest request, string LocationName)
            {
                LocationViewModel location = new LocationViewModel();
                List<LocationModel> locationList = new List<LocationModel>();
                locationList = _repositoryLocation.GetLocationList(LocationName.Trim());
                location.Olocation = locationList.Select(x => new BELocation
                {
                    iLocationID = x.LocationID,
                    sLocationName = x.LocationName,
                    sLocationDescription = x.LocationDesc,
                    bDisabled = x.Disabled
                }).ToList();
                return Json(location.Olocation.ToDataSourceResult(request));

            }
            [HttpPost]
            public ActionResult LocationSearchView(LocationViewModel objLocation)
            {
                List<LocationModel> location = new List<LocationModel>();
                location = _repositoryLocation.GetLocationList(objLocation.SearchLocationName);
                objLocation.Olocation = location.Select(x => new BELocation
                {
                    iLocationID = x.LocationID,
                    sLocationName = x.LocationName,
                    sLocationDescription = x.LocationDesc,
                    bDisabled = x.Disabled
                }).ToList();

                if (objLocation.Olocation.Count == 0)
                {
                    ViewData["Message"] = @BPA.GlobalResources.UI.Resources_common.dispNoRecordFound;
                }
                return View("LocationSearchView", objLocation);
            }
            [HttpPost]
            public JsonResult editLocation(string iLocationID, string LocationName, string LocationDescription, string Disabled)
            {
                TempData["iLocationID"] = iLocationID;
                TempData.Keep("iLocationID");
                TempData["LocationName"] = LocationName.Replace("(Disabled)", "");
                TempData.Keep("LocationName");
                TempData["LocationDescription"] = LocationDescription;
                TempData.Keep("LocationDescription");
                TempData["Disabled"] = Disabled;
                TempData.Keep("Disabled");
                return Json(@BPA.GlobalResources.UI.Resources_common.display_Ok);
            }
        }
    }
