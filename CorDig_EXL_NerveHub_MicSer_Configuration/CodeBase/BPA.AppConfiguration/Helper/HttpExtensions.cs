using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using Newtonsoft.Json;

namespace BPA.AppConfiguration.Helper
{
    public static class HttpExtensions
    {
        private static readonly JsonSerializer Serializer = new()
        {
            NullValueHandling = NullValueHandling.Ignore
        };

        public static async Task WriteJson<T>(this HttpResponse response, T obj, string contentType = null)
        {
            response.ContentType = contentType ?? "application/json";
            await using var writer = new HttpResponseStreamWriter(response.Body, Encoding.UTF8);
            using var jsonWriter = new JsonTextWriter(writer);
            jsonWriter.CloseOutput = false;
            jsonWriter.AutoCompleteOnClose = false;

            Serializer.Serialize(jsonWriter, obj);
        }
    }
}
