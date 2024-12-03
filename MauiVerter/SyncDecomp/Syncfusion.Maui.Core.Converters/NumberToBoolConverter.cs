using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace Syncfusion.Maui.Core.Converters;

public class NumberToBoolConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (value == null)
		{
			throw new ArgumentNullException("Value should not be null", "value");
		}
		return System.Convert.ToBoolean(value);
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (value == null)
		{
			throw new ArgumentNullException("Value should not be null", "value");
		}
		return ((bool)value) ? 1 : 0;
	}
}
