using Newtonsoft.Json;
using SWApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;

namespace SWApi.Tests.Unit.Mocks
{
    /// <summary>
    /// Mock of the API Service used to make basic requests against an external service
    /// </summary>
    internal class ApiServiceMock : IApiService
    {
        private static readonly object locker = new object();
        private static List<Starship> starships;

        private const int pageCount = 10;

        /// <summary>
        /// Collection of starships loaded from file
        /// </summary>
        public static List<Starship> Starships
        {
            get
            {
                if (starships != null)
                    return starships;

                lock (locker)
                {
                    if (starships != null)
                        return starships;

                    starships = new List<Starship>();
                    var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                    starships.AddRange(JsonConvert.DeserializeObject<List<Starship>>(File.ReadAllText($@"{path}\MOCK_DATA.json")));
                    return starships;
                }
            }
        }
        
        /// <summary>
        /// Mocks get request by providing data from file
        /// </summary>
        /// <param name="url">Url to be used. Only query param page is being used</param>
        /// <returns>Stringified starship response object</returns>
        public virtual Task<string> GetRequestAsync(string url)
        {
            var uri = new UriBuilder(url);
            var query = HttpUtility.ParseQueryString(uri.Query);
            if (!int.TryParse(query.Get("page"), out var page))
                page = 1;

            var starships = Starships.Skip((page - 1) * pageCount).Take(pageCount).ToList();
            query["page"] = (page + 1).ToString();
            uri.Query = query.ToString();

            return Task.FromResult(JsonConvert.SerializeObject(
                new StarshipsResponse()
                {
                    Count = Starships.Count,
                    Starships = starships,
                    NextPageUrl = page * pageCount < Starships.Count ? uri.ToString() : null
                }));
        }
    }
}
