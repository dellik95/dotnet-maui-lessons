using PropertyChanged;
using System.Collections.ObjectModel;
using Tasker.Models;

namespace Tasker.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class AddTaskViewModel
    {
        public string Task { get; set; }

        public ObservableCollection<Category> Categories { get; set; }
        public ObservableCollection<MyTask> Tasks { get; set; }
    }
}
