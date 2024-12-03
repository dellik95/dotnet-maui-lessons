using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace Syncfusion.Maui.Core.Converters;

public class IsStringNotNullOrEmptyConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (string.IsNullOrEmpty(value as string))
		{
			return false;
		}
		return true;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotSupportedException("ConvertBack to original value is not possible. BindingMode must be set as OneWay.");
	}
}
