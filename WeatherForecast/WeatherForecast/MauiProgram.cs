using SkiaSharp.Views.Maui.Controls.Hosting;
using WeatherForecast.ViewModels;
using WeatherForecast.Views;

namespace WeatherForecast;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseSkiaSharp()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                fonts.AddFont("Rubik-Light.ttf", "RubikLight");
                fonts.AddFont("Rubik-Regular.ttf", "Rubik");
            });


        builder.Services.AddTransient<WeatherPageViewModel>();
        builder.Services.AddTransient<WeatherPage>();

        return builder.Build();
    }
}
