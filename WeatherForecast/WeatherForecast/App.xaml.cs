using WeatherForecast.Views;

namespace WeatherForecast;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

		MainPage = new WeatherPage();
    }
}
