using CollectionViewPractice.Models;
using CollectionViewPractice.ViewModels;
using CollectionViewPractice.Views;

namespace CollectionViewPractice;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });
        builder.Services.AddTransient<IProductRepository, ProductRepository>();
        builder.Services.AddTransient<DataViewModel>();
        builder.Services.AddTransient<DataViewPresenter>();


        return builder.Build();
    }
}
