using System.Net.Http;
using System.Threading.Tasks;

namespace SWApi
{
    /// <summary>
    /// Simple api service allowing to use a basic requests against an external service
    /// </summary>
    public class ApiService : IApiService
    {
        /// <inheritdoc />
        public async Task<string> GetRequestAsync(string url)
        {
            using (var client = new HttpClient())
            {
                using (var response = await client.GetAsync(url))
                {
                    // Throws exception if an error occurs
                    response.EnsureSuccessStatusCode();
                    using (var content = response.Content)
                    {
                        return await content.ReadAsStringAsync();
                    }
                }
            }
        }
    }
}
