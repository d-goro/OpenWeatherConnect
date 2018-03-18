using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace OpenWeather
{
    class Program
    {
        static void Main(string[] args)
        {
            bool flip = false;
            while (true)
            {
                Console.WriteLine("Specify city or type 'Q' for exit: ");
                var code = Console.ReadLine();
                if (code == "Q")
                    break;

                var query = $"http://api.openweathermap.org/data/2.5/find?q={code}&units=metric&appid={Properties.Settings.Default.APIKey}";

                DisplayDataFromQuery(code, query, flip);

                flip = !flip;
            }

        }

        static async void DisplayDataFromQuery(string city, string query, bool flip)
        {
            if (flip)
            {
                var response = await QueryWeatherWithDynamic(query);
                Console.WriteLine($"{city}: Temperature: {response.Temp}, Wind speed: {response.WindSpeed}, Humidity: {response.Humidity}");
            }
            else
            {
                var response = await QueryWeatherWithDeSerialization(query);
                Console.WriteLine($"{city}: Temperature: {response.Temp}, Wind speed: {response.WindSpeed}, Humidity: {response.Humidity}");
            }
        }

        static async Task<(string Temp, string WindSpeed, string Humidity)> QueryWeatherWithDynamic(string query)
        {
            return await Task.Run(() =>
            {
                string temp = "", speed = "", humidity = "";
                using (var client = new WebClient())
                {
                    try
                    {
                        var response = client.DownloadString(query);

                        dynamic queryDictionary = JsonConvert.DeserializeObject(response);
                        temp = queryDictionary.list[0].main.temp;
                        humidity = queryDictionary.list[0].main.humidity;
                        speed = queryDictionary.list[0].wind.speed;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }
                return (temp, speed, humidity);
            });
        }

        static async Task<(string Temp, string WindSpeed, string Humidity)> QueryWeatherWithDeSerialization(string query)
        {
            return await Task.Run(() =>
            {
                string temp = "", speed = "", humidity = "";
                using (var client = new WebClient())
                {
                    try
                    {
                        var response = client.DownloadString(query);
                        var weatherData = JsonConvert.DeserializeObject<WeatherData>(response);

                        temp = weatherData.Temperature;
                        humidity = weatherData.Humidity;
                        speed = weatherData.WindSpeed;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }
                return (temp, speed, humidity);
            });
        }
    }
}
