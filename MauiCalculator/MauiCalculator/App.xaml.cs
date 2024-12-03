using MauiCalculator.Views;

namespace MauiCalculator;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        MainPage = new CalculatorView();
    }
}
