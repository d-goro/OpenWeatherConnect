using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenWeather
{
    public class WeatherData
    {
        public class Weather
        {
            public Dictionary<string, string> Main { get; set; }

            public Dictionary<string, string> Wind { get; set; }

        }

        public int Count { get; set; }

        public Weather[] List { get; set; }

        public string Temperature => List.First().Main["temp"];

        public string Humidity => List.First().Main["humidity"];

        public string WindSpeed => List.First().Wind["speed"];
    }

}
