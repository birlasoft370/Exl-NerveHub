using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BPA.Security.BusinessEntity.ExtrernalRefre.Uitility
{
    public static class ValidationRegex
    {
        private static string EmailValidation
        {
            get
            {
                return @"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?";
            }
        }

        public static bool IsEmail(string emailAddress)
        {
            return Regex.IsMatch(emailAddress, EmailValidation, RegexOptions.IgnoreCase);
        }
        public static string whitelist(string txtValues)
        {
            string RValues = "";
            string whitelist = @"^[a-zA-Z\-\.']$";
            Regex pattern = new Regex(whitelist);
            if (!pattern.IsMatch(txtValues))
            {
                RValues = txtValues;
            }
            return RValues;
        }
    }
}