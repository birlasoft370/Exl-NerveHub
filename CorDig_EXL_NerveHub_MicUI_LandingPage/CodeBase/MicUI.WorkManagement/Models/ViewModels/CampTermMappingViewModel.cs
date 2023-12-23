using BPA.GlobalResources.UI;
using MicUI.WorkManagement.Helper.CustomValidationAttributes;
using MicUI.WorkManagement.Services.ServiceModel;
using System.ComponentModel.DataAnnotations;
using System.Security.AccessControl;
using System.Xml.Linq;

namespace MicUI.WorkManagement.Models.ViewModels
{
    [Serializable]
    public class CampTermMappingViewModel : ViewModelValidationHelper
    {
        public BECampTermCodeMapping BECampTermCodeMapping = new BECampTermCodeMapping();

        public List<BETerminationCodeInfo> GetTermCodeList = new List<BETerminationCodeInfo>();
        public List<BECampaignClientProcess> ClientProcess = new List<BECampaignClientProcess>();

        public List<BECampaignInfo> BECampaignList = new List<BECampaignInfo>();

        [Display(ResourceType = typeof(BPA.GlobalResources.UI.WorkManagement.Resources_CampTermMapping), Name = "display_Campaign")]
        public string mcampainName
        { get; set; }


        [Required(ErrorMessageResourceType = typeof(BPA.GlobalResources.UI.WorkManagement.Resources_CampTermMapping), ErrorMessageResourceName = "required_Client")]
        [Display(ResourceType = typeof(Resources_common), Name = "display_Client")]
        public int iClientID
        {
            get;
            set;
        }

        [Required(ErrorMessageResourceType = typeof(BPA.GlobalResources.UI.WorkManagement.Resources_CampTermMapping), ErrorMessageResourceName = "required_Process")]
        [Display(ResourceType = typeof(Resources_common), Name = "display_Process")]
        public int iProcessID
        {
            get;
            set;
        }

        [Required(ErrorMessageResourceType = typeof(BPA.GlobalResources.UI.WorkManagement.Resources_CampTermMapping), ErrorMessageResourceName = "required_Campaign")]
        [Display(ResourceType = typeof(Resources_common), Name = "display_Campaign")]
        public int sCampaignName
        {
            get;
            set;
        }


        [Required(ErrorMessage = "Please Enter Termination Code Name!")]
        [Display(Name = "Campaign")]

        public int miCampaignTermMapID
        {
            get;
            set;
        }

        [Display(ResourceType = typeof(BPA.GlobalResources.UI.WorkManagement.Resources_CampTermMapping), Name = "display_Campaign")]
        public int miCampaignID
        {
            get;
            set;
        }
        public string mClientSearchID
        { get; set; }

        public string mProcessSearchID
        { get; set; }
        public string sTerminationName
        {
            get;
            set;
        }
        public List<mTermCodeList> GridTermCodeMap = new List<mTermCodeList>();
        public IList<BETerminationCodeInfo> GetTerminationList
        {
            get;
            set;
        }
        public string sltTerminationID { get; set; }
        public string sltTerminationNameID { get; set; }
    }
    [Serializable]
    public class mTermCodeList
    {
        public int TerminationID { get; set; }
        public int Selected { get; set; }
        public string TerminatioName { get; set; }
        public int IsProductive { get; set; }
        public int IsEnd { get; set; }
        public int Disabled { get; set; }

    }

}