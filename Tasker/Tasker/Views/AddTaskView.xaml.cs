using System.Diagnostics;
using Tasker.Models;
using Tasker.ViewModels;
using Tasker.Extensions;

namespace Tasker.Views;

public partial class AddTaskView : ContentPage
{
    private Category selectedCategory = null;

    private readonly AddTaskViewModel viewModel;
    public AddTaskView() : this(new AddTaskViewModel())
    {

    }

    public AddTaskView(AddTaskViewModel viewModel)
    {
        InitializeComponent();
        this.viewModel = viewModel;
        this.BindingContext = this.viewModel;
    }

    private void RadioButton_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (sender is not RadioButton rb)
        {
            return;
        }

        if (rb.Value is Category selectedCategory)
        {

            this.selectedCategory = selectedCategory;
        }
    }

    private async void OnAddTask(object sender, EventArgs e)
    {
        if (this.selectedCategory != null)
        {
            this.viewModel.Tasks.Add(new MyTask()
            {
                CategoryId = selectedCategory.Id,
                Name = viewModel.Task,
            });

            await this.Navigation.PopAsync();
            return;
        }
        await this.DisplayAlert("No category", "Select one category for task", "Ok");
        return;
    }

    private async void OnAddCategory(object sender, EventArgs e)
    {
        var category = await this.DisplayPromptAsync("New category!", "Write new category of tasks", keyboard: Keyboard.Text, maxLength: 15);

        if (string.IsNullOrEmpty(category))
        {
            return;
        }


        var color = new Color().GetRandomColor();

        var id = this.viewModel.Categories.Max(x => x.Id) + 1;

        this.viewModel.Categories.Add(new Category()
        {
            Id = id,
            Color = color.ToHex(),
            Name = category,
        });

        return;
    }
}