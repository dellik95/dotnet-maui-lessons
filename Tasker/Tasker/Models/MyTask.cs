using PropertyChanged;

namespace Tasker.Models
{
    [AddINotifyPropertyChangedInterface]
    public class MyTask
    {
        public string Name { get; set; }
        public string Color { get; set; }
        public int CategoryId { get; set; }
        public bool Completed { get; set; }
    }
}
