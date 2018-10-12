using System.Threading.Tasks;
using Xunit;

namespace SWApi.Tests.Integration
{
    public class IntegrationTests
    {
        private static readonly SWApiService service = new SWApiService(new ApiService());

        [Fact]
        public async Task TestGetStarshipsSequentially()
        {
            var result = await service.GetAllStarships();
            Assert.NotEmpty(result);
        }

        [Fact]
        public async Task TestGetStarshipsParallelly()
        {
            var result = await service.GetAllStarshipsParallelly();
            Assert.NotEmpty(result);
        }
    }
}
