using System.Threading.Tasks;

namespace SWApi
{
    /// <summary>
    /// Interface for service providing basic requests
    /// </summary>
    public interface IApiService
    {
        /// <summary>
        /// Makes get request with defiend url
        /// </summary>
        /// <param name="url">Url against which a request will be made</param>
        /// <returns>Result in string from the requested url</returns>
        Task<string> GetRequestAsync(string url);
    }
}
