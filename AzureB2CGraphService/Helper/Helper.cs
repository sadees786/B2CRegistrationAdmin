using AzureB2CGraphService.Model;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace AzureB2CGraphService.Helper
{
    public class Helper
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="httpClient">HttpClient used to call the protected API</param>
        public Helper(HttpClient httpClient)
        {
            HttpClient = httpClient;
        }
        protected HttpClient HttpClient { get; private set; }

        public async Task<HttpResponseMessage> CallWebApiPost(string webApiUrl, string accessToken, string userToAdd)
        {
            if (string.IsNullOrEmpty(accessToken)) return null;
               HttpRequestMessage  request = new HttpRequestMessage(HttpMethod.Post, webApiUrl);
               request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
               request.Content = new StringContent(userToAdd, Encoding.UTF8, "application/json");            
               HttpResponseMessage response = await HttpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode) return null;
            return response;
        }

        public async Task<string> CallWebApiGet(string webApiUrl, string accessToken)
        {
            if (string.IsNullOrEmpty(accessToken)) return null;
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, webApiUrl);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            HttpResponseMessage response = await HttpClient.SendAsync(request);
            var result = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode) return null;
            return result;
        }
    }
}
