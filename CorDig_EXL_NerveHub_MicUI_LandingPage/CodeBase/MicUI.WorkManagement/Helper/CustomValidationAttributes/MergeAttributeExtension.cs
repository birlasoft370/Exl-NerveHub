namespace MicUI.WorkManagement.Helper.CustomValidationAttributes
{
    public static class MergeAttributeExtension
    {
        public static bool MergeAttribute(this IDictionary<string, string> attributes, string key, string value)
        {
            if (attributes.ContainsKey(key))
            {
                return false;
            }
            attributes.Add(key, value);
            return true;
        }
    }
}
