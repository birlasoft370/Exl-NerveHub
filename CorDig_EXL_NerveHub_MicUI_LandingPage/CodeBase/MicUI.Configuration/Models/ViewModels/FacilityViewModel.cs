using MicUI.Configuration.Services.ServiceModel;
using System.ComponentModel.DataAnnotations;
using System.Security.AccessControl;
using System.Xml.Linq;

namespace MicUI.Configuration.Models.ViewModels
{
    [Serializable]
    public class FacilityViewModel : ViewModelValidationHelper
    {
        private IEnumerable<BEFacility> _lstFacilities = new List<BEFacility>();

        public IEnumerable<BEFacility> lstFacilities
        {
            get { return _lstFacilities; }
            set { _lstFacilities = value; }
        }
        // [RequiredField(sControlKeyCollection = "Search", ErrorMessageResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.Resources_Facility), ErrorMessageResourceName = "required_SearchText")]
        public string sSearchText { get; set; }
        public FacilityViewModel()
        {

        }
        public BEFacility objFacility = new BEFacility();
        #region BEFacility
        [ScaffoldColumn(false)]
        public int iFacilityID
        {
            get
            {
                return objFacility.iFacilityID;
            }
            set
            {
                objFacility.iFacilityID = value;
            }
        }
        [Required(ErrorMessageResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.Resources_Facility), ErrorMessageResourceName = "required_LocationName")]
        [Display(ResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.Resources_Facility), Name = "display_LocationName")]
        public int iLocationID
        {
            get
            {
                return objFacility.iLocationID;
            }
            set
            {
                objFacility.iLocationID = value;
            }
        }


        [Required(ErrorMessageResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.Resources_Facility), ErrorMessageResourceName = "required_FacilityName")]
        [Display(ResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.Resources_Facility), Name = "display_FacilityName")]
        public string sFacilityName
        {
            get
            {
                return objFacility.sFacilityName;
            }
            set
            {
                objFacility.sFacilityName = value;
            }
        }

        //[RequiredField(sControlKeyCollection = "Create,Search", ErrorMessage = "Enter Description.")]
        //[Formate("33-222-44", ErrorMessage = "Please correct formate as 33-222-44.")]
        //[FilterExpression("alphanum", "@.", "", ErrorMessage = "Please enter numbers only.")]
        //[MaxLengthCheck(3, ErrorMessage = "Three digit allowed.")]
        [Display(ResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.Resources_Facility), Name = "display_FacilityDescription")]
        public string sFacilityDescription
        {
            get
            {
                return objFacility.sFacilityDescription;
            }
            set
            {
                objFacility.sFacilityDescription = value;
            }
        }

        //[CheckBoxRequired(ErrorMessage = "Please Check Disabled.", PropertyName = "bDisabled,bDisabled2", PropertyCampareValues = false)]
        [Display(ResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.Resources_Facility), Name = "display_Disabled")]
        public bool? bDisabled
        {
            get { return objFacility.bDisabled; }
            set { objFacility.bDisabled = Convert.ToBoolean(value); }
        }

        #endregion
    }

}
