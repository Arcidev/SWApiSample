using System.Threading.Tasks;

namespace SWApi
{
    /// <summary>
    /// Interface for service providing basic requests
    /// </summary>
    public interface IApiService
    {
        /// <summary>
        /// Makes get request with defined url
        /// </summary>
        /// <param name="url">Url against which a request will be made</param>
        /// <returns>Content as string from the requested url</returns>
        Task<string> GetRequestAsync(string url);

        /// <summary>
        /// Makes get request with defined url
        /// </summary>
        /// <param name="url">Url against which a request will be made</param>
        /// <returns>Content as object from the requested url</returns>
        Task<T> GetRequestAsync<T>(string url) where T : class;
    }
}
