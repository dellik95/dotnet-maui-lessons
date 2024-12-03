using CollectionViewPractice.Views;

namespace CollectionViewPractice;

public partial class App : Application
{
    public App(DataViewPresenter dataView)
    {
        InitializeComponent();

        MainPage = dataView;

    }
}
