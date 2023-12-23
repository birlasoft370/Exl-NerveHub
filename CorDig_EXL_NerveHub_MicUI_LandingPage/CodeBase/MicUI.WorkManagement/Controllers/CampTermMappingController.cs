using BPA.GlobalResources;
using Microsoft.AspNetCore.Mvc;
using MicUI.WorkManagement.Models.ViewModels;

namespace MicUI.WorkManagement.Controllers
{
    public class CampTermMappingController : BaseController
    {
        public ActionResult Index()
        {
            //DataAnnotationsModelValidatorProvider.AddImplicitRequiredAttributeForValueTypes = false;
            CampTermMappingViewModel oCampTermMappingViewModel = new CampTermMappingViewModel();
            if (!base.CheckViewPermission(int.Parse(Resources_FormID.CampaignTerminationCodeMappingFormID)))
            {
                return RedirectToActionPermanent("AccessDenied", "Error");
            }
            return View(oCampTermMappingViewModel);
        }
    }
}
