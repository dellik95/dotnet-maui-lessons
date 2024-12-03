using CollectionViewPractice.ViewModels;

namespace CollectionViewPractice.Views;

public partial class DataViewPresenter : ContentPage
{
    public DataViewPresenter(DataViewModel dataViewModel)
    {
        InitializeComponent();
        this.BindingContext = dataViewModel;
    }
}