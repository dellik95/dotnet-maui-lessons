using PropertyChanged;
using System.Text.Json;
using System.Windows.Input;
using WeatherForecast.Models;

namespace WeatherForecast.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class WeatherPageViewModel
    {
        public bool IsLoading { get; set; }
        public WeatherData WeatherData { get; set; }

        public string PlaceName { get; set; }

        public DateTime Date { get; set; } = DateTime.Now;


        private HttpClient _httpClient;

        public WeatherPageViewModel()
        {
            this._httpClient = new HttpClient();
        }

        public ICommand SearchCommand => new Command(async (searchTerm) =>
        {
            this.IsLoading = true;
            this.PlaceName = searchTerm.ToString();
            var location = await this.GetLocationAsync(searchTerm.ToString());
            await this.GetWeather(location);
        });

        public async Task<Location> GetLocationAsync(string address)
        {
            IEnumerable<Location> locations = await Geocoding.Default.GetLocationsAsync(address);
            var location = locations.FirstOrDefault();
            return location;
        }

        private async Task GetWeather(Location location)
        {
            var url = $"https://api.open-meteo.com/v1/forecast?latitude={location.Latitude}&longitude={location.Longitude}&daily=weathercode,temperature_2m_max,temperature_2m_min&current_weather=true&windspeed_unit=ms&timezone=Europe%2FKyiv";

            var responseMessage = await _httpClient.GetAsync(url);
            if (responseMessage.IsSuccessStatusCode)
            {
                using var reader = await responseMessage.Content.ReadAsStreamAsync();
                this.WeatherData = JsonSerializer.Deserialize<WeatherData>(reader);

                for (var i = 0; i < WeatherData.daily.time.Length; i++)
                {
                    var daily = new DailyWeather()
                    {
                        time = WeatherData.daily.time[i],
                        weathercode = WeatherData.daily.weathercode[i],
                        temperature_2m_max = WeatherData.daily.temperature_2m_max[i],
                        temperature_2m_min = WeatherData.daily.temperature_2m_min[i]
                    };
                    this.WeatherData.dailyWeather.Add(daily);
                }
            }
            this.IsLoading = false;
        }
    }
}
