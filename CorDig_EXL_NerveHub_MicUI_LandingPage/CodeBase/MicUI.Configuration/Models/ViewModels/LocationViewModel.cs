using BPA.GlobalResources.UI.AppConfiguration;
using System.ComponentModel.DataAnnotations;
using System.Security.AccessControl;
using System.Xml.Linq;

namespace MicUI.Configuration.Models.ViewModels
{
    [Serializable]
    public class LocationViewModel
    {
        [Required(ErrorMessageResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.Resource_Location), ErrorMessageResourceName = "requied_Location")]
        [Display(ResourceType = typeof(Resource_Location), Name = "display_Location")]
        public string LocationName
        {
            get;
            set;

        }

        [Display(ResourceType = typeof(Resource_Location), Name = "display_Location")]
        public string SearchLocationName
        {
            get;
            set;
        }

        public int iLocationID
        {
            get;
            set;
        }


        [Display(ResourceType = typeof(Resource_Location), Name = "display_Description")]
        [DataType(DataType.MultilineText)]
        public string LocationDesc
        {
            get;
            set;
        }


        [Display(ResourceType = typeof(Resource_Location), Name = "displayDisable")]
        public bool IsDisable
        {
            get;
            set;
        }

        List<BELocation> _Olocation = new List<BELocation>();
        public List<BELocation> Olocation { get { return _Olocation; } set { _Olocation = value; } }


    }
}

