using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace MicUI.Configuration.Helper.CustomValidationAttributes
{
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
}
