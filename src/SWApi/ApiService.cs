using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace SWApi
{
    /// <summary>
    /// Simple api service allowing to use a basic requests against an external service
    /// </summary>
    public class ApiService : IApiService
    {
        /// <summary>
        /// Makes get request with defined url
        /// </summary>
        /// <param name="url">Url against which a request will be made</param>
        /// <returns>Content as string from the requested url</returns>
        /// <exception cref="HttpRequestException">Thrown when unsuccessful status code</exception>
        public async Task<string> GetRequestAsync(string url)
        {
            using var client = new HttpClient();
            using var response = await client.GetAsync(url);
            // Throws exception if an error occurs
            response.EnsureSuccessStatusCode();
            using var content = response.Content;
            return await content.ReadAsStringAsync();
        }

        /// <summary>
        /// Makes get request with defined url
        /// </summary>
        /// <param name="url">Url against which a request will be made</param>
        /// <returns>Content as object from the requested url</returns>
        /// <exception cref="HttpRequestException">Thrown when unsuccessful status code</exception>
        public async Task<T> GetRequestAsync<T>(string url) where T : class
        {
            var content = await GetRequestAsync(url).ConfigureAwait(false);
            if (string.IsNullOrEmpty(content))
                return null;

            return JsonConvert.DeserializeObject<T>(content);
        }
    }
}
