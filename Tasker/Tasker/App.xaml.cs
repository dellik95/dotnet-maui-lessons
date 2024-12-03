using Tasker.Views;

namespace Tasker;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
        var navView = new NavigationPage(new MainView());
        MainPage = navView;
    }
}