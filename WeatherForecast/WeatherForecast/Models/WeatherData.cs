using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace WeatherForecast.Models
{

    public class WeatherData
    {
        public float latitude { get; set; }
        public float longitude { get; set; }
        public float generationtime_ms { get; set; }
        public int utc_offset_seconds { get; set; }
        public string timezone { get; set; }
        public string timezone_abbreviation { get; set; }
        public float elevation { get; set; }
        public Current_Weather current_weather { get; set; }
        public Daily_Units daily_units { get; set; }
        public Daily daily { get; set; }

        [JsonIgnore]
        public ObservableCollection<DailyWeather> dailyWeather { get; set; } = new ObservableCollection<DailyWeather>();
    }
}
