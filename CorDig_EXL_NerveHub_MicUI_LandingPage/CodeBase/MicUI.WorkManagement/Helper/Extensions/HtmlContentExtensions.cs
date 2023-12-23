using Microsoft.AspNetCore.Html;
using System.IO;

namespace MicUI.WorkManagement.Helper.Extensions
{
    public static class HtmlContentExtensions
    {
        public static string ToHtmlString(this IHtmlContent htmlContent)
        {
            if (htmlContent is HtmlString htmlString)
            {
                return htmlString.Value;
            }

            using (var writer = new StringWriter())
            {
                htmlContent.WriteTo(writer, System.Text.Encodings.Web.HtmlEncoder.Default);
                return writer.ToString();
            }
        }

        public static string FormatWith(this string value, params object[] args)
        {
            return String.Format(value, args);
        }
    }
}
