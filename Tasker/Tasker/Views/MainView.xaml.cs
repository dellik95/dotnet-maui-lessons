using Tasker.ViewModels;

namespace Tasker.Views;

public partial class MainView : ContentPage
{
    private readonly MainViewModel viewModel = new();
    public MainView()
    {
        InitializeComponent();
        this.BindingContext = viewModel;
    }

    private void IsCompleted_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        this.viewModel.UpdateInformation();
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        var vm = new AddTaskViewModel()
        {
            Tasks = this.viewModel.Tasks,
            Categories = this.viewModel.Categories,
        };

        await this.Navigation.PushAsync(new AddTaskView(vm));
    }
}