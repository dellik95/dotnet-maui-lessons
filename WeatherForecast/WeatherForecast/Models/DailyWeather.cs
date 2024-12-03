namespace WeatherForecast.Models
{
    public class DailyWeather
    {
        public string time { get; set; }
        public int weathercode { get; set; }
        public float temperature_2m_max { get; set; }
        public float temperature_2m_min { get; set; }
    }
}
