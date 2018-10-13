using Newtonsoft.Json;
using SWApi.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWApi
{
    /// <summary>
    /// Star Wars API Service
    /// </summary>
    public class SWApiService
    {
        private readonly IApiService service;

        private const string baseUrl = "https://swapi.co/api";
        private static readonly string starshipsUrl = $"{baseUrl}/starships";

        /// <summary>
        /// Creates new instance of SWApiService
        /// </summary>
        /// <param name="service">API service to be used to perform GET requests</param>
        public SWApiService(IApiService service)
        {
            this.service = service;
        }

        /// <summary>
        /// Gets all starships from SWApi endpoint by calling <see cref="IApiService"/>
        /// </summary>
        /// <returns>All starships contained in SWApi endpoint</returns>
        public async Task<List<Starship>> GetAllStarships()
        {
            var response = new StarshipsResponse() { NextPageUrl = starshipsUrl };
            var result = new List<Starship>();
            do
            {
                var responseStr = await service.GetRequestAsync(response.NextPageUrl);
                response = JsonConvert.DeserializeObject<StarshipsResponse>(responseStr);
                result.AddRange(response.Starships);

            } while (!string.IsNullOrEmpty(response?.NextPageUrl));

            return result;
        }

        /// <summary>
        /// Gets all starships from SWApi endpoint by calling <see cref="IApiService"/> parallelly
        /// </summary>
        /// <returns>All starships contained in SWApi endpoint</returns>
        public async Task<List<Starship>> GetAllStarshipsParallelly()
        {
            var responseStr = await service.GetRequestAsync(starshipsUrl);
            var response = JsonConvert.DeserializeObject<StarshipsResponse>(responseStr);

            // Create enumartion of tasks for every page (skipping first) so we can create async request for every page without sequentional wait
            // We will use an equation for pageCount as: (records - 1) / recordsPerPage + 1;
            // In this case records: response.Count, recordsPerPage: response.Starships.Count without adding 1 as we already have the first page
            var tasks = Enumerable.Range(2, (response.Count - 1) / response.Starships.Count).Select(i => service.GetRequestAsync($"{starshipsUrl}/?page={i}"));
            var allStarships = (await Task.WhenAll(tasks)).Select(x => JsonConvert.DeserializeObject<StarshipsResponse>(x).Starships).SelectMany(x => x);

            response.Starships.AddRange(allStarships);
            return response.Starships;
        }
    }
}
