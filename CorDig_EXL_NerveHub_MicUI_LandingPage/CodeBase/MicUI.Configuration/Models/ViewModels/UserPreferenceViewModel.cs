using BPA.GlobalResources.UI;
using MicUI.Configuration.Helper.CustomValidationAttributes;
using System.ComponentModel.DataAnnotations;

namespace MicUI.Configuration.Models.ViewModels
{
    public class UserPreferenceViewModel
    {
        [RequiredField(ErrorMessageResourceType = typeof(Resources_UserPreference), ErrorMessageResourceName = "req_TimeZone")]
        [Display(ResourceType = typeof(Resources_UserPreference), Name = "disp_TimeZone")]
        public int TimeZoneID { get; set; }

        [RequiredField(ErrorMessageResourceType = typeof(Resources_UserPreference), ErrorMessageResourceName = "req_Language")]
        [Display(ResourceType = typeof(Resources_UserPreference), Name = "disp_Language")]
        public string Language { get; set; }

        [Display(ResourceType = typeof(Resources_UserPreference), Name = "disp_Disable")]
        public bool Disable { get; set; }

        public string sTimeZone { get; set; }
    }
}
