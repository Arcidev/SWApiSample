using SWApi;
using System;
using System.Threading.Tasks;

namespace SWApiSample
{
    class Program
    {
        static async Task Main(string[] args)
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
            var result = await swApi.GetAllStarshipsParallelly();

            foreach (var starship in result)
                Console.WriteLine($"{starship.Name}: {starship.CalculateStops(distance)?.ToString() ?? "unknown"}");
        }
    }
}
