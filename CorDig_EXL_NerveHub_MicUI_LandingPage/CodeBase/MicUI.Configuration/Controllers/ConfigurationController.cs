using Microsoft.AspNetCore.Mvc;
using MicUI.Configuration.Helper.Sessions;
using MicUI.Configuration.Models.ViewModels;
using MicUI.Configuration.Module.Configuration.Languages;
using MicUI.Configuration.Module.Configuration.TimeZone;
using MicUI.Configuration.Module.UserPreference;
using MicUI.Configuration.Services.ServiceModel;

namespace MicUI.Configuration.Controllers
{
    // [Route("Configuration")]
    public class ConfigurationController : BaseController
    {
        private readonly IGetSetSessionValues _getSetSessionValues;
        private readonly IUserPreferenceService _repositoryUserPreference;
        private readonly ITimeZoneService _repositoryTimeZone;
        private readonly ILanguagesService _repositoryLanguage;
        public ConfigurationController(IGetSetSessionValues getSetSessionValues, IUserPreferenceService repositoryUserPreference, ITimeZoneService repositoryTimeZone, ILanguagesService repositoryLanguage)
        {
            _getSetSessionValues = getSetSessionValues;
            _repositoryUserPreference = repositoryUserPreference;
            _repositoryTimeZone = repositoryTimeZone;
            _repositoryLanguage = repositoryLanguage;
        }

        //[Route("Configuration")]
        //[Route("Configuration/Index")]
        //[Route("/Configuration/Index")]
        // [HttpGet("Index")]
        public IActionResult Index()
        {
            return View();
        }

        public JsonResult GetTimeZone()
        {
            IList<BETimeZoneInfo> tempLstTimeZone = null;

            try
            {
                tempLstTimeZone = _repositoryTimeZone.GetTimeZoneList();
                return Json(tempLstTimeZone);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            { tempLstTimeZone = null; }
        }

        public JsonResult GetLanguages()
        {
            IList<BELanguages> tempLstLanguages = null;
            try
            {
                tempLstLanguages = _repositoryLanguage.GetLanguageList(true);
                return Json(tempLstLanguages);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            { tempLstLanguages = null; }
        }

        [HttpGet]
        public IActionResult UserPreference()
        {
            UserPreferenceViewModel objUserPreferenceViewModel = new();
            var UserPerefernceDetail = _repositoryUserPreference.GetUserPreferenceDetail();
            if (UserPerefernceDetail != null)
            {
                objUserPreferenceViewModel.TimeZoneID = UserPerefernceDetail.iTimeZoneID;
                objUserPreferenceViewModel.Disable = UserPerefernceDetail.bDisable;
                objUserPreferenceViewModel.Language = UserPerefernceDetail.sLanguage;
            }
            return View(objUserPreferenceViewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SaveUpdateUserPreference(UserPreferenceViewModel objUserPreferenceViewModel)
        {
            try
            {
                _repositoryUserPreference.SaveUpdateUserPreference(objUserPreferenceViewModel);
                var _oUser = _getSetSessionValues.GetSessionUserInfo();
                _oUser.sLanguage = objUserPreferenceViewModel.Language;
                _oUser.sUserTimeZone = objUserPreferenceViewModel.sTimeZone;
                _getSetSessionValues.SetSessionUserInfo(_oUser);
                return Json(1);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
            finally
            {
                //objBETimeZoneInfo = null;
                //objBEUserPreference = null;
            }
        }
    }
}
