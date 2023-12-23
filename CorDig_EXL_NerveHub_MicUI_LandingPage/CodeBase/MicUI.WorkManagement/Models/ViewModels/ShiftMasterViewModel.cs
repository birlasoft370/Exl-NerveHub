using BPA.GlobalResources.UI.WorkManagement;
using MicUI.WorkManagement.Helper.CustomValidationAttributes;
using System.ComponentModel.DataAnnotations;

namespace MicUI.WorkManagement.Models.ViewModels
{
    [Serializable]
    public class ShiftMasterViewModel : ViewModelValidationHelper
    {

        [ScaffoldColumn(false)]
        public int ShiftID { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources_Shift), ErrorMessageResourceName = "required_Shift")]
        [Display(ResourceType = typeof(BPA.GlobalResources.UI.WorkManagement.Resources_Shift), Name = "display_ShiftName")]
        public string ShiftName { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources_Shift), ErrorMessageResourceName = "requiredDescription")]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Description:")]

        public string Description { get; set; }
        [ScaffoldColumn(false)]

        //[Required(ErrorMessageResourceType = typeof(Resources_Shift), ErrorMessageResourceName = "requiredShiftStartTime", sControlKeyCollection = "btnShiftMasterSave")]
        [Display(ResourceType = typeof(BPA.GlobalResources.UI.WorkManagement.Resources_Shift), Name = "display_ShiftStartTime")]
        public string ShiftStartTime { get; set; }

        [Display(ResourceType = typeof(BPA.GlobalResources.UI.WorkManagement.Resources_Shift), Name = "display_Hrs")]
        public string ShiftStartHrTime { get; set; }

        [Display(ResourceType = typeof(BPA.GlobalResources.UI.WorkManagement.Resources_Shift), Name = "display_Min")]
        public string ShiftStartMinTime { get; set; }
        [ScaffoldColumn(false)]

        [Display(ResourceType = typeof(BPA.GlobalResources.UI.WorkManagement.Resources_Shift), Name = "display_ShiftEndTime")]
        public string ShiftEndTime { get; set; }

        [Display(ResourceType = typeof(BPA.GlobalResources.UI.WorkManagement.Resources_Shift), Name = "display_Hrs")]
        public string ShiftEndHrTime { get; set; }

        [Display(ResourceType = typeof(BPA.GlobalResources.UI.WorkManagement.Resources_Shift), Name = "display_Min")]
        public string ShiftEndMinTime { get; set; }

        private List<ShiftMasterViewModel> _ShiftList = new List<ShiftMasterViewModel>();
        public List<ShiftMasterViewModel> ShiftList { get { return _ShiftList; } set { _ShiftList = value; } }

        [Display(ResourceType = typeof(BPA.GlobalResources.UI.WorkManagement.Resources_Shift), Name = "display_Disable")]
        public bool Disable { get; set; }


        [Display(ResourceType = typeof(BPA.GlobalResources.UI.WorkManagement.Resources_Shift), Name = "display_ShiftName")]
        public string SearchShiftName { get; set; }
    }
}