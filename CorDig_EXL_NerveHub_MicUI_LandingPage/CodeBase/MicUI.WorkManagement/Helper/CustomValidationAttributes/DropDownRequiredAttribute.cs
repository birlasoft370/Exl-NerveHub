using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace MicUI.WorkManagement.Helper.CustomValidationAttributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    sealed public class DropDownRequiredAttribute : ValidationAttributeEx, IClientModelValidator
    {
        bool _isFirstDefault = true;

        public DropDownRequiredAttribute()
        {
        }
        public DropDownRequiredAttribute(bool isFirstDefault)
        {
            _isFirstDefault = isFirstDefault;
        }


        public override bool ValidateOnControl(string sControl)
        {
            return ValidationHelpMember.ValidateOnControl(sControl, sControlKeyCollection);
        }

        // Write your validation logic here in this function only
        public override bool IsValid(object value)
        {
            int ddlSelectedItemId = 0;
            if (value != null)
            {
                switch (value.GetType().Name)
                {

                    case "Int32":
                        ddlSelectedItemId = (int)value;
                        break;
                    case "String":
                        if ((string)value == "--Select--")
                            return false;
                        else
                            return true;
                        //ddlSelectedItemId =Convert.ToInt32((string)value);
                        break;

                        ////add more Case to validate another drop downlist
                        //case "EntityName":
                        //    break;
                }

            }
            bool result = true;
            if ((_isFirstDefault == true && ddlSelectedItemId > 0) || (_isFirstDefault == false && ddlSelectedItemId >= 0))
                result = true;
            else
                result = false;
            return result;
        }

        //Do not Change this function 
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            bool isRequired = ValidationHelpMember.IsRequired(validationContext, PropertyCampareValues, PropertyName);

            if (isRequired && !IsValid(value))
                return new ValidationResult(this.ErrorMessage);
            return ValidationResult.Success;
        }

        public override string FormatErrorMessage(string name)
        {
            return String.Format(CultureInfo.CurrentCulture,
           ErrorMessageString, name);
        }

        public void AddValidation(ClientModelValidationContext context)
        {
            context.Attributes.MergeAttribute("data-val-dropdownrequired", this.ErrorMessageString);
            context.Attributes.MergeAttribute("data-val-dropdownrequired-isfirstdefault", this._isFirstDefault.ToString());//Bool
            context.Attributes.MergeAttribute("data-val-dropdownrequired-buttonkeys", this.sControlKeyCollection);
        }
    }
}
