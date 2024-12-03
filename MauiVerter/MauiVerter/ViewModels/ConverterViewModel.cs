using PropertyChanged;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using UnitsNet;

namespace MauiVerter.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class ConverterViewModel
    {
        public string QuantityName { get; set; }

        public ObservableCollection<string> FromMeasures { get; set; }

        public ObservableCollection<string> ToMeasures { get; set; }

        public ICommand ReturnCommand { get; private set; }

        public string SelectedFromMeasure { get; set; }

        public double EnteredValue { get; set; }
        public double Result { get; set; }

        public string SelectedToMeasure { get; set; }

        public ConverterViewModel(string quantityName)
        {
            this.ReturnCommand = new Command(() =>
            {
                this.Convert();
            });
            this.QuantityName = quantityName;
            this.FromMeasures = this.LoadMeasures();
            this.ToMeasures = this.LoadMeasures();
            this.InitDefaultValues();
            this.Convert();
        }

        private void InitDefaultValues()
        {
            this.SelectedFromMeasure = this.FromMeasures[0];
            this.SelectedToMeasure = this.FromMeasures[1];
        }

        private ObservableCollection<string> LoadMeasures()
        {
            var types = Quantity
                .Infos.
                FirstOrDefault(x => x.Name == QuantityName)
                .UnitInfos.Select(x => x.Name).ToList();

            return new ObservableCollection<string>(types);
        }

        public void Convert()
        {
            var res = UnitConverter.ConvertByName(this.EnteredValue, this.QuantityName, this.SelectedFromMeasure, this.SelectedToMeasure);
            this.Result = res;
        }
    }
}
