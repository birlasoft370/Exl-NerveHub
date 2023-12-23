using System.Net.Http.Headers;
using System.Net;
using System.Text;
using Newtonsoft.Json;

namespace MicUI.Configuration.Helper
{
    /// <summary>
    /// 
    /// </summary>
    internal static class CallAPI
    {
        internal static readonly string Bearer = "Bearer";
        internal static readonly string appJson = "application/json";

        private static void CreateHeaderInfo(HttpRequestMessage request, string token)
        {
            if (token != null)
                request.Headers.Authorization = new AuthenticationHeaderValue(Bearer, token);
        }

        internal static async Task<T> HttpPostTokenClientAsync<T, W>(string url, string token)
        {
            return await HttpClientAsync<T>(HttpMethod.Post, url, token, string.Empty);
        }

        internal static async Task<T> HttpClientAsync<T, W>(HttpMethod httptype, string url, W data, string token = "")
        {
            var jsonData = Convertor.Serialize<W>(data) ?? string.Empty;
            return await HttpClientAsync<T>(httptype, url, jsonData, token);
        }

        internal static async Task<T> HttpClientAsync<T>(HttpMethod httptype, string url, string jsonData = "", string token = "")
        {
            T? result = default(T);
            var request = new HttpRequestMessage(httptype, url);
            var client = new HttpClient();
            if (!string.IsNullOrWhiteSpace(jsonData))
            {
                request.Content = new StringContent(jsonData, Encoding.UTF8, appJson); //new StringContent(jsonData, Encoding.UTF8);
            }
            CreateHeaderInfo(request, token);

            //request.Content.Headers.ContentType = new MediaTypeHeaderValue(appJson);
            var response = await client.SendAsync(request);

            //if (response.StatusCode == HttpStatusCode.OK)
            //{
            var content = await response.Content.ReadAsStringAsync();
            //result = Convertor.DeserializeSpecial<T>(content);
            result = Convertor.DeserializeSpecial<T>(content);
            // var tokenDatam = JsonConvert.DeserializeObject<List<BELandingPageMenu>>(content);

            //}
            return result;
        }
        internal static async Task<T> HttpClientNormalAsync<T>(HttpMethod httptype, string url, string jsonData = "", string token = "")
        {
            T? result = default(T);
            var request = new HttpRequestMessage(httptype, url);
            var client = new HttpClient();
            if (!string.IsNullOrWhiteSpace(jsonData))
            {
                request.Content = new StringContent(jsonData, Encoding.UTF8, appJson); //new StringContent(jsonData, Encoding.UTF8);
            }
            CreateHeaderInfo(request, token);

            //request.Content.Headers.ContentType = new MediaTypeHeaderValue(appJson);
            var response = await client.SendAsync(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var content = await response.Content.ReadAsStringAsync();
                result = JsonConvert.DeserializeObject<T>(content);


            }
            return result;
        }
    }
}


