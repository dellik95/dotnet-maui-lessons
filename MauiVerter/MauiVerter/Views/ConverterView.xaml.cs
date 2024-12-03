using MauiVerter.ViewModels;

namespace MauiVerter.Views;

public partial class ConverterView : ContentPage
{
    public ConverterView()
    {
        InitializeComponent();
    }

    private void Picker_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (this.BindingContext is ConverterViewModel ctx)
        {
            ctx.Convert();
        }
    }
}