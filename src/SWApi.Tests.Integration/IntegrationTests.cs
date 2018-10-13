using System.Threading.Tasks;
using Xunit;

namespace SWApi.Tests.Integration
{
    public class IntegrationTests
    {
        private const int numberOfStops = 1_000_000;

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

        [Fact]
        public async Task TestStarshipStopsCalculation()
        {
            var result = await service.GetAllStarshipsParallelly();
            foreach (var item in result)
            {
                // Check that all values within consumables are handled
                if (item.Consumables?.Split().Length == 2 && int.TryParse(item.MGLT, out var _))
                {
                    Assert.NotNull(item.CalculateStops(numberOfStops));
                    continue;
                }

                Assert.Null(item.CalculateStops(numberOfStops));
            }
        }
    }
}
