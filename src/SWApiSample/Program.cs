using SWApi;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace SWApiSample
{
    class Program
    {
        static async Task Main()
        {
            int distance = 0;
            while (true)
            {
                Console.WriteLine("Set distance: ");
                if (int.TryParse(Console.ReadLine(), out distance) && distance > 0)
                    break;

                Console.WriteLine("Distance must be a positive number");
            }

            var swApi = new SWApiService(new ApiService());
            try
            {
                var result = await swApi.GetAllStarshipsParallelly();
                foreach (var starship in result.OrderBy(x => x.Name))
                    Console.WriteLine($"{starship.Name}: {starship.CalculateStops(distance)?.ToString() ?? "unknown"}");
            }
            catch(HttpRequestException)
            {
                Console.WriteLine("There was an error while communicating with SW API");
            }
        }
    }
}
