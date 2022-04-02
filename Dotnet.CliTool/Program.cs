using System;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Dotnet.CliTool
{
	class Program
	{
		static async Task Main(string[] args)
		{
			var city = args.AsQueryable().FirstOrDefault();

			if (city == null)
			{
				Console.Write("Enter City name: ");
				city = Console.ReadLine().Trim();
			}
			
			var client = new HttpClient();
			var request = new HttpRequestMessage
			{
				Method = HttpMethod.Get,
				RequestUri = new Uri($"https://community-open-weather-map.p.rapidapi.com/weather?q={city}&lat=0&lon=0&id=2172797&lang=null&units=imperial"),
				Headers =
				{
					{ "X-RapidAPI-Host", "community-open-weather-map.p.rapidapi.com" },
					{ "X-RapidAPI-Key", "493b91739fmshbd17cf455e808b8p16d44ejsn1789fd0b38eb" },
				},
			};
			using (var response = await client.SendAsync(request))
			{
				try
				{
					response.EnsureSuccessStatusCode();
					var body = await response.Content.ReadAsStringAsync();
					var data = JsonSerializer.Deserialize<Root>(body);
					Console.WriteLine($"--- CURRENT WEATHER OF {city.ToUpper()} ---");
					Console.WriteLine($"City: {data.name}");
					Console.WriteLine($"Country: {data.sys.country}");
					Console.WriteLine($"Weather {data.weather[0].description}: {data.weather[0].main}");
					Console.WriteLine($"Current Temperature: {data.main.temp}°C");
					Console.WriteLine($"Maximum Temperature: {data.main.temp_max}°C");
					Console.WriteLine($"Clouds: {data.wind.speed}mph");
					Console.WriteLine($"Clouds: {data.clouds.all}%");
				}
				catch (Exception e)
				{
					Console.WriteLine(e);
					throw;
				}
			}
			Console.WriteLine();
			Console.Write("Press any key to exit...");
			Console.ReadLine();
		}
	}
}