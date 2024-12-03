using Dangl.Calculator;
using PropertyChanged;
using System.Windows.Input;

namespace MauiCalculator.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    class CalculatorViewModel
    {
        public string Formula { get; set; }

        public string Result { get; set; } = "0";

        public ICommand ResetCommand => new Command(() =>
        {
            this.Result = "0";
            this.Formula = string.Empty;
        });

        public ICommand BackspaceCommand => new Command(() =>
        {
            if (!string.IsNullOrEmpty(this.Formula))
            {
                this.Formula = this.Formula[..^1];
            }
            return;
        });


        public ICommand CalculateCommand => new Command(() =>
        {
            if (string.IsNullOrEmpty(this.Formula))
            {
                return;
            }
            var calculation = Calculator.Calculate(this.Formula);
            if (calculation.IsValid)
            {
                this.Result = calculation.Result.ToString();
            }
        });


        public ICommand OperationCommand => new Command((number) =>
        {
            this.Formula += number;
        });
    }
}
