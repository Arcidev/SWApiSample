using SWApi.Tests.Unit.Mocks;
using System.Threading.Tasks;
using Xunit;

namespace SWApi.Tests.Unit
{
    public class UnitTests
    {
        private static readonly SWApiService service = new SWApiService(new ApiServiceMock());

        [Fact]
        public async Task TestGetStarshipsSequentially()
        {
            var result = await service.GetAllStarships();
            Assert.NotEmpty(result);
            Assert.Equal(result.Count, ApiServiceMock.Starships.Count);
        }

        [Fact]
        public async Task TestGetStarshipsParallelly()
        {
            var result = await service.GetAllStarshipsParallelly();
            Assert.NotEmpty(result);
            Assert.Equal(result.Count, ApiServiceMock.Starships.Count);
        }
    }
}
