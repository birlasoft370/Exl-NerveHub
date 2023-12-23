using BPA.GlobalResources.UI.AppConfiguration;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using MicUI.Configuration.Services.ServiceModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Security.AccessControl;
using System.Xml.Linq;

namespace MicUI.Configuration.Models.ViewModels
{
    [Serializable]
    public class TimeZoneViewModel : ViewModelValidationHelper
    {
        public TimeZoneViewModel()
        {
            lstTimeZoneview = new List<BETimeZoneInfo>();
        }
        [ScaffoldColumn(false)]
        public int TimeZoneID { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources_TimeZone), ErrorMessageResourceName = "requiredTimeZoneName")]
        [Display(ResourceType = typeof(Resources_TimeZone), Name = "dispTimeZoneName")]
        public string TimeZoneName { get; set; }

        [Display(ResourceType = typeof(Resources_TimeZone), Name = "dispDescription")]
        public string Description { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources_TimeZone), ErrorMessageResourceName = "requiredOffsetGMT")]
        [Display(ResourceType = typeof(Resources_TimeZone), Name = "dispOffsetGMT")]
        public string OffSetGMT { get; set; }
        public IEnumerable<SelectListItem> lstTimeZone { get; set; }

        [Display(ResourceType = typeof(Resources_TimeZone), Name = "dispDisable")]
        public bool Disabled { get; set; }

        [Display(ResourceType = typeof(Resources_TimeZone), Name = "dispTimeZoneName")]
        public string SearchTimeZone
        {
            get;
            set;
        }

        public IList<BETimeZoneInfo> lstTimeZoneview { get; set; }
    }
}
public class ViewModelValidationHelper
{
    private ValidationContext @ValidationContext { get; set; }
    public void ValidationFilter(ModelStateDictionary ModelState, string SearchKey)
    {
        this.ValidationContext = new ValidationContext(this, null, null);
        Type type = ValidationContext.ObjectType;
        foreach (var property in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            var validationAttributes = (ValidationAttributeEx[])property.GetCustomAttributes(typeof(ValidationAttributeEx), true);
            foreach (var attr in validationAttributes)
            {
                if (attr.sControlKeyCollection != null && attr.sControlKeyCollection != "" && !attr.sControlKeyCollection.Contains(SearchKey))
                {
                    ModelState.Remove(property.Name);
                }
            }
        }
    }
}
public abstract class ValidationAttributeEx : ValidationAttribute
{
    public abstract bool ValidateOnControl(string sControl);
    public int Sequence { get; set; }
    public string sControlKeyCollection { get; set; }
    public string PropertyName { get; set; }
    public object PropertyCampareValues { get; set; }
}