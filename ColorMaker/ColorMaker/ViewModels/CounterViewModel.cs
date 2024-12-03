using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace ColorMaker.ViewModels
{
    internal class CounterViewModel : INotifyPropertyChanged
    {
        private Color bgColor;

        public Color BgColor
        {
            get
            {
                return this.bgColor;
            }
            set
            {
                this.bgColor = value;
                this.OnPropertyChanged(nameof(RedChanel));
                this.OnPropertyChanged(nameof(GreenChanel));
                this.OnPropertyChanged(nameof(BlueChanel));
                this.OnPropertyChanged();
            }
        }

        private int redChanel;
        public int RedChanel
        {
            get
            {
                return redChanel;
            }

            set
            {
                this.redChanel = value;
                this.BgColor = Color.FromRgb(value, this.GreenChanel, this.BlueChanel);
            }
        }

        private int greenChanel;
        public int GreenChanel
        {
            get
            {
                return greenChanel;
            }
            set
            {
                this.greenChanel = value;
                this.BgColor = Color.FromRgb(this.RedChanel, value, this.BlueChanel);
            }
        }

        private int blueChanel;
        public int BlueChanel
        {
            get => blueChanel; set
            {
                this.blueChanel = value;
                this.BgColor = Color.FromRgb(this.RedChanel, this.GreenChanel, value);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
