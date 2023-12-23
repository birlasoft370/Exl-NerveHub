using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace MicUI.WorkManagement.Helper.CustomValidationAttributes
{
    public abstract class ValidationAttributeEx : ValidationAttribute
    {
        public abstract bool ValidateOnControl(string sControl);
        public int Sequence { get; set; }
        public string sControlKeyCollection { get; set; }
        public string PropertyName { get; set; }
        public object PropertyCampareValues { get; set; }
    }
    static class ValidationHelpMember
    {
        public static bool ValidateOnControl(string sControlKey, string sControlKeyCollection)
        {
            if (sControlKey != null && sControlKeyCollection != null && sControlKeyCollection != "")
            {
                bool bMeet = false;
                string[] sDiffControlKey = sControlKeyCollection.Split(',');
                if (sDiffControlKey.Length > 0)
                {
                    foreach (var Key in sDiffControlKey)
                    {
                        if (Key.ToUpper() == sControlKey.ToUpper())
                            bMeet = true;
                    }
                    return bMeet;
                }
                else return true;
            }
            else return true;
        }

        /// <summary>
        /// Determines whether or not validation should occur.
        /// </summary>
        /// <param name="validationContext">The validation context which contains the instance that owns the member which we are validating.</param>
        /// <returns>Returns true if validation should occur, false otherwise.</returns>     
        public static bool IsRequired(ValidationContext validationContext, object Values, string PropertyName)
        {
            if (PropertyName != null && Values != null)
            {
                var property = validationContext.ObjectType.GetProperty(PropertyName);
                var currentValue = property.GetValue(validationContext.ObjectInstance, null);
                var typ = currentValue.GetType();
                //foreach (var val in Values)
                if (object.Equals(currentValue, Values))
                    return true;
                return false;
            }
            return true;
        }
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true, Inherited = false)]
    sealed public class RequiredFieldAttribute : ValidationAttributeEx, IClientModelValidator
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
            return String.Format(CultureInfo.CurrentCulture,
           ErrorMessageString, name);
        }

        public void AddValidation(ClientModelValidationContext context)
        {
            context.Attributes.MergeAttribute("data-val-requiredfield", this.ErrorMessageString);
            context.Attributes.MergeAttribute("data-val-requiredfield-buttonkeys", this.sControlKeyCollection);
        }
    }
}
