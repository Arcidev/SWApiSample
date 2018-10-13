using Moq;
using Newtonsoft.Json;
using SWApi.Enums;
using SWApi.Models;
using SWApi.Tests.Unit.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SWApi.Tests.Unit
{
    public class UnitTests
    {
        private const int testIterations = 100;
        private const int numberOfStops = 1_000_000;
        private const string mglt = "20";
        private static readonly string consumables = $"2 {ConsumableTime.Months}";

        private static readonly SWApiService service = new SWApiService(new StarshipApiServiceMock());

        [Fact]
        public async Task TestGetStarships()
        {
            var mock = new Mock<StarshipApiServiceMock>();
            mock.Setup(x => x.GetRequestAsync(It.IsNotNull<string>())).Returns(Task.FromResult(JsonConvert.SerializeObject(new StarshipsResponse()
            {
                Count = StarshipApiServiceMock.Starships.Count,
                Starships = StarshipApiServiceMock.Starships
            })));

            var mockService = new SWApiService(mock.Object);
            Assert.NotEmpty(await mockService.GetAllStarships());
            Assert.NotEmpty(await mockService.GetAllStarshipsParallelly());
        }

        [Fact]
        public async Task TestGetStarships2()
        {
            var secondPageUrl = "Second_Page_Url";
            var pageCount = StarshipApiServiceMock.Starships.Count / 2 + 1;

            var mock = new Mock<StarshipApiServiceMock>();
            mock.Setup(x => x.GetRequestAsync(secondPageUrl)).Returns(Task.FromResult(JsonConvert.SerializeObject(new StarshipsResponse()
            {
                Count = StarshipApiServiceMock.Starships.Count,
                Starships = StarshipApiServiceMock.Starships.Skip(pageCount).ToList()
            })));
            mock.Setup(x => x.GetRequestAsync(It.IsNotIn(secondPageUrl))).Returns(Task.FromResult(JsonConvert.SerializeObject(new StarshipsResponse()
            {
                Count = StarshipApiServiceMock.Starships.Count,
                Starships = StarshipApiServiceMock.Starships.Take(pageCount).ToList(),
                NextPageUrl = secondPageUrl
            })));

            var mockService = new SWApiService(mock.Object);
            var starships = await mockService.GetAllStarships();

            Assert.NotEmpty(starships);
            Assert.NotEmpty(await mockService.GetAllStarshipsParallelly());

            Assert.Equal(StarshipApiServiceMock.Starships.Count, starships.Count);
            for (int i = 0; i < StarshipApiServiceMock.Starships.Count; i++)
                Assert.True(StarshipsEqual(starships[i], StarshipApiServiceMock.Starships[i]));
        }

        [Fact]
        public async Task TestApiMockService()
        {
            Assert.NotNull(StarshipApiServiceMock.Starships);
            Assert.NotEmpty(StarshipApiServiceMock.Starships);

            var mockService = new StarshipApiServiceMock();
            var items = new List<Starship>();
            var response = new StarshipsResponse() { NextPageUrl = "localhost" };

            do
            {
                var result = await mockService.GetRequestAsync(response.NextPageUrl);
                Assert.NotNull(result);

                response = JsonConvert.DeserializeObject<StarshipsResponse>(result);
                Assert.NotNull(response);
                Assert.NotNull(response.Starships);

                items.AddRange(response.Starships);
            } while (response.NextPageUrl != null);

            Assert.Equal(StarshipApiServiceMock.Starships.Count, items.Count);
            for (int i = 0; i < StarshipApiServiceMock.Starships.Count; i++)
                Assert.True(StarshipsEqual(items[i], StarshipApiServiceMock.Starships[i]));
        }

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
            for (int i = 0; i < testIterations; i++)
            {
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
                var stopsExpected = numberOfStops / hoursToRefill - (numberOfStops % hoursToRefill == 0 ? 1 : 0);

                Assert.Equal(stopsExpected, starship.CalculateStops(numberOfStops));
                Assert.Equal(0, starship.CalculateStops(hoursToRefill));
                Assert.Equal(0, starship.CalculateStops(hoursToRefill - 1));
                Assert.Equal(1, starship.CalculateStops(hoursToRefill + 1));
            }
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

        private static bool StarshipsEqual(Starship starship, Starship starship2)
        {
            if (starship == null)
                return starship2 == null;

            if (starship2 == null)
                return false; // no need to check again

            return starship.Name == starship2.Name && starship.Consumables == starship2.Consumables && starship.MGLT == starship2.MGLT;
        }
    }
}
