using BPA.GlobalResources.UI.AppConfiguration;
using MicUI.WorkManagement.Helper.CustomValidationAttributes;
using MicUI.WorkManagement.Services.ServiceModel;
using System.ComponentModel.DataAnnotations;
using System.Security.AccessControl;
using System.Xml.Linq;

namespace MicUI.WorkManagement.Models.ViewModels
{
    [Serializable]
    public class CalendarMasterModel : ViewModelValidationHelper
    {


        public IList<BECalendarInfo> CalendarList;

        public int mCalenderID
        {
            get;
            set;
        }
        //Calendar Name For Search

        [Display(ResourceType = typeof(Resources_Calender), Name = "displayCalendarName")]
        public string CalendarSearchName
        { get; set; }

        //Calendar Name
        [Required(ErrorMessageResourceType = typeof(Resources_Calender), ErrorMessageResourceName = "requiredCalName")]
        [Display(ResourceType = typeof(Resources_Calender), Name = "displayCalendarName")]
        public string mCalenderName
        { get; set; }

        //Description
        [Display(ResourceType = typeof(Resources_Calender), Name = "displayDescription")]
        public string mDescription
        { get; set; }

        //Disable
        [Display(ResourceType = typeof(Resources_Calender), Name = "displayDisable")]
        public bool mIsdisable
        {
            get; set;
        }

    }
}