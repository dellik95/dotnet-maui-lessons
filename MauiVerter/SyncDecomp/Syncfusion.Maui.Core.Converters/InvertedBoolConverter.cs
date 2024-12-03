using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace Syncfusion.Maui.Core.Converters;

public class InvertedBoolConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		return InvertBool(value);
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		return InvertBool(value);
	}

	private static object InvertBool(object value)
	{
		if (value is bool)
		{
			return !(bool)value;
		}
		return value;
	}
}
