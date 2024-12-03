using PropertyChanged;
using System.Collections.ObjectModel;
using Tasker.Models;

namespace Tasker.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class MainViewModel
    {
        public ObservableCollection<Category> Categories { get; set; }
        public ObservableCollection<MyTask> Tasks { get; set; }

        public MainViewModel()
        {
            this.FillData();
            this.Tasks.CollectionChanged += Tasks_CollectionChanged;
        }

        private void Tasks_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            this.UpdateInformation();
        }

        private void FillData()
        {
            this.Categories = new ObservableCollection<Category>
               {
                    new Category
                    {
                         Id = 1,
                         Name = ".NET MAUI Course",
                         Color = "#CF14DF"
                    },
                    new Category
                    {
                         Id = 2,
                         Name = "Tutorials",
                         Color = "#df6f14"
                    },
                    new Category
                    {
                         Id = 3,
                         Name = "Shopping",
                         Color = "#14df80"
                    }
               };
            Tasks = new ObservableCollection<MyTask>
               {
                    new MyTask
                    {
                         Name = "Upload exercise files",
                         Completed = false,
                         CategoryId = 1
                    },
                    new MyTask
                    {
                         Name = "Plan next course",
                         Completed = false,
                         CategoryId = 1
                    },
                    new MyTask
                    {
                         Name = "Upload new ASP.NET video on YouTube",
                         Completed = false,
                         CategoryId = 2
                    },
                    new MyTask
                    {
                         Name = "Fix Settings.cs class of the project",
                         Completed = false,
                         CategoryId = 2
                    },
                    new MyTask
                    {
                         Name = "Update github repository",
                         Completed = true,
                         CategoryId = 2
                    },
                    new MyTask
                    {
                         Name = "Buy eggs",
                         Completed = false,
                         CategoryId = 3
                    },
                    new MyTask
                    {
                         Name = "Go for the pepperoni pizza",
                         Completed = false,
                         CategoryId = 3
                    },
               };

            this.UpdateInformation();
        }

        public void UpdateInformation()
        {
            foreach (var category in this.Categories)
            {
                var tasks = this.Tasks.Where(t => t.CategoryId == category.Id).ToList();

                var doneTasks = tasks.Where(t => t.Completed).ToList();
                var inProgress = tasks.Where(t => !t.Completed).ToList();

                category.PendingTasks = inProgress.Count;
                category.Percentage = (float)doneTasks.Count / (float)tasks.Count;

                foreach (var task in tasks)
                {
                    task.Color = category.Color;
                }
            }
        }
    }
}
