namespace BPA.AppConfiguration.Helper;

internal static class Common
{
    internal static T GetValue<T>(HttpContext httpContext, string key)
    {
        if (httpContext != null)
        {
            if (httpContext.Items.ContainsKey(key))
            {
                httpContext.Items.TryGetValue(key, out var tokenValue);
                if (tokenValue != null)
                {
                    return (T)tokenValue;
                }
            }
        }
        return default(T);
    }
}
