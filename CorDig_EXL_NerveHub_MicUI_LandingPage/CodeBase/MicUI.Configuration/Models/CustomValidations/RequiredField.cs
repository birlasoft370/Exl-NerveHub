using Kendo.Mvc.Infrastructure.Implementation;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace MicUI.Configuration.Models.CustomValidations
{
    /*
    /// <summary>
    ///To Validate Required 
    /// <para>Set strValidateOnControl to a string key seprated by coma for limited to some event check</para>
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true, Inherited = false)]
    sealed public class RequiredFieldAttribute : ValidationAttributeEx, IClientValidatable
    {
        public RequiredFieldAttribute() { }


        public override bool ValidateOnControl(string sControl)
        {
            return ValidationHelpMember.ValidateOnControl(sControl, sControlKeyCollection);
        }
        // Write your validation logic here in this function only
        public override bool IsValid(object value)
        {
            if (string.IsNullOrEmpty(Convert.ToString(value)))
                return false;
            return true;
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
            return String.Format(CultureInfo.CurrentCulture, ErrorMessageString, name);
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule();
            rule.ErrorMessage = this.ErrorMessageString;
            rule.ValidationType = "requiredfield";
            rule.ValidationParameters["buttonkeys"] = this.sControlKeyCollection;
            yield return rule;
        }

    }*/
}