using System;
using System.Threading;
using System.Threading.Tasks;

namespace SWApi.Tests.Unit.Mocks
{
    /// <inheritdoc />
    public class StarshipSlowApiServiceMock : StarshipApiServiceMock
    {
        private static readonly Random random = new Random();

        /// <summary>
        /// Mocks get request by providing data from file while using Thread.Sleep
        /// </summary>
        /// <param name="url">Url to be used. Only query param page is being used</param>
        /// <returns>Stringified starship response object</returns>
        public override Task<string> GetRequestAsync(string url)
        {
            Thread.Sleep(random.Next(100, 400));
            return base.GetRequestAsync(url);
        }
    }
}
