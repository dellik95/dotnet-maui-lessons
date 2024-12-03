using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace Syncfusion.Maui.Core.Converters;

public class DoubleToIntConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (value != null && value is double)
		{
			return System.Convert.ToInt32(value);
		}
		throw new ArgumentException("Value is not a valid double", nameof(value));
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (value != null && value is int)
		{
			return System.Convert.ToDouble(value);
		}
		throw new ArgumentException("Value is not a valid integer", nameof(value));
	}
}
