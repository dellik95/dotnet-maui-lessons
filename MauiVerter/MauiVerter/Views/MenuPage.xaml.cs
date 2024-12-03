using MauiVerter.ViewModels;

namespace MauiVerter.Views;

public partial class MenuPage : ContentPage
{

    private readonly Dictionary<string, ContentPage> items = new();

    public MenuPage()
    {
        InitializeComponent();
    }

    private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
    {
        string key = string.Empty;
        if (sender is Grid grid)
        {
            var element = grid.Children.OfType<Label>().LastOrDefault();
            if (element == null)
            {
                return;
            }

            key = element.Text;
        }

        if (!items.TryGetValue(key, out var page))
        {
            page = new ConverterView()
            {
                BindingContext = new ConverterViewModel(key)
            };

            this.items.Add(key, page);

        }
        this.Navigation.PushAsync(page);
    }
}