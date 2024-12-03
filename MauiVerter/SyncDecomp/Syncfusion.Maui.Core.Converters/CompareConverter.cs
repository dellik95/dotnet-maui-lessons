using System;
using System.Globalization;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Xaml;

namespace Syncfusion.Maui.Core.Converters;

public class CompareConverter : IMarkupExtension<IValueConverter>, IMarkupExtension, IValueConverter
{
	public enum OperatorType
	{
		NotEqual,
		Smaller,
		SmallerOrEqual,
		Equal,
		Greater,
		GreaterOrEqual
	}

	private enum ModeOptions
	{
		Boolean,
		Object
	}

	private ModeOptions modeOption;

	public IComparable? ValueForComparing { get; set; }

	public OperatorType ComparisonOperator { get; set; }

	public object? TrueValueObject { get; set; }

	public object? FalseValueObject { get; set; }

	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (ValueForComparing == null)
		{
			throw new ArgumentNullException("Comparing Value should not be null", "ValueForComparing");
		}
		if (!(value is IComparable))
		{
			throw new ArgumentException("is expected to implement IComparable interface.", nameof(value));
		}
		if (TrueValueObject != null)
		{
			modeOption = ModeOptions.Object;
		}
		IComparable comparable = (IComparable)value;
		int num = comparable.CompareTo(ValueForComparing);
		return ComparisonOperator switch
		{
			OperatorType.Smaller => CheckCondition(num < 0), 
			OperatorType.SmallerOrEqual => CheckCondition(num <= 0), 
			OperatorType.Equal => CheckCondition(num == 0), 
			OperatorType.NotEqual => CheckCondition(num != 0), 
			OperatorType.GreaterOrEqual => CheckCondition(num >= 0), 
			OperatorType.Greater => CheckCondition(num > 0), 
			_ => throw new ArgumentOutOfRangeException("Comparison Operator is not suppoerted", "ComparisonOperator"), 
		};
	}

	private object CheckCondition(bool comparisonResult)
	{
		if (modeOption == ModeOptions.Object)
		{
			return comparisonResult ? TrueValueObject : FalseValueObject;
		}
		return comparisonResult;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotSupportedException("ConvertBack to original value is not possible. BindingMode must be set as OneWay.");
	}

	public IValueConverter ProvideValue(IServiceProvider serviceProvider)
	{
		return this;
	}

	object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider)
	{
		return ((IMarkupExtension<IValueConverter>)this).ProvideValue(serviceProvider);
	}
}
