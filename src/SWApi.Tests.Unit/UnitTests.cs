using SWApi.Enums;
using SWApi.Models;
using SWApi.Tests.Unit.Mocks;
using System;
using System.Threading.Tasks;
using Xunit;

namespace SWApi.Tests.Unit
{
    public class UnitTests
    {
        private const int numberOfStops = 1_000_000;
        private const string mglt = "20";
        private static readonly string consumables = $"2 {ConsumableTime.Months}";

        private static readonly SWApiService service = new SWApiService(new StarshipApiServiceMock());

        [Fact]
        public async Task TestGetStarshipsSequentially()
        {
            var result = await service.GetAllStarships();
            Assert.NotEmpty(result);
            Assert.Equal(result.Count, StarshipApiServiceMock.Starships.Count);
        }

        [Fact]
        public async Task TestGetStarshipsParallelly()
        {
            var result = await service.GetAllStarshipsParallelly();
            Assert.NotEmpty(result);
            Assert.Equal(result.Count, StarshipApiServiceMock.Starships.Count);
        }

        [Fact]
        public void TestStopsCalculation()
        {
            var random = new Random();
            var randomMglt = random.Next(10, 100);
            var randomConsumables = random.Next(1, 5);

            var consumableTimeValues = Enum.GetValues(typeof(ConsumableTime));
            var randomTime = (ConsumableTime)consumableTimeValues.GetValue(random.Next(consumableTimeValues.Length));

            var starship = new Starship()
            {
                MGLT = randomMglt.ToString(),
                Consumables = $"{randomConsumables} {randomTime}"
            };

            var hoursToRefill = randomMglt * randomConsumables * (int)randomTime;
            Assert.Equal(numberOfStops / hoursToRefill, starship.CalculateStops(numberOfStops));
            Assert.Equal(0, starship.CalculateStops(hoursToRefill));
            Assert.Equal(0, starship.CalculateStops(hoursToRefill - 1));
        }

        [Fact]
        public void TestUknownStopsCalculation()
        {
            Assert.Null(new Starship() { MGLT = mglt }.CalculateStops(numberOfStops));
            Assert.Null(new Starship() { Consumables = consumables}.CalculateStops(numberOfStops));
            Assert.Null(new Starship() { MGLT = string.Empty, Consumables = string.Empty }.CalculateStops(numberOfStops));
            Assert.Null(new Starship() { MGLT = "unknown", Consumables = consumables }.CalculateStops(numberOfStops));
            Assert.Null(new Starship() { MGLT = mglt, Consumables = "unknown" }.CalculateStops(numberOfStops));
        }
    }
}
