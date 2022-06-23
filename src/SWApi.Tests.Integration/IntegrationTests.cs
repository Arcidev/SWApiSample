using SWApi.Enums;
using System;
using System.Threading.Tasks;
using Xunit;

namespace SWApi.Tests.Integration
{
    public class IntegrationTests
    {
        private static readonly SWApiService service = new(new ApiService());

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
        public async Task TestStarshipConsumables()
        {
            var result = await service.GetAllStarshipsParallelly();
            foreach (var item in result)
            {
                // Check that all values within consumables are handled
                var consumables = item.Consumables?.Split();
                if (consumables?.Length == 2)
                {
                    // Assuming this is always number so in case it's not it would be nice to know
                    Assert.True(int.TryParse(consumables[0], out var _), $"Invalid number for consumable: {item.Consumables}");

                    // Assuming this is always time period and is defined within enum so in case it's not it would be nice to know
                    Assert.True(Enum.TryParse(consumables[1], true, out ConsumableTime _), $"Invalid time period for consumable: {item.Consumables}");
                }
            }
        }

        [Fact]
        public async Task TestStarshipParallelGetOrder()
        {
            var starships = await service.GetAllStarships();
            var starships2 = await service.GetAllStarshipsParallelly();

            Assert.Equal(starships.Count, starships2.Count);
            for (int i = 0; i < starships.Count; i++)
            {
                Assert.Equal(starships[i].Name, starships2[i].Name);
                Assert.Equal(starships[i].MGLT, starships2[i].MGLT);
                Assert.Equal(starships[i].Consumables, starships2[i].Consumables);
            }
        }
    }
}
