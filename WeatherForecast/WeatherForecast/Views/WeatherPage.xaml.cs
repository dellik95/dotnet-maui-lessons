using WeatherForecast.ViewModels;

namespace WeatherForecast.Views;

public partial class WeatherPage : ContentPage
{
    public WeatherPage()
    {
        InitializeComponent();
        this.BindingContext = new WeatherPageViewModel();
    }
}