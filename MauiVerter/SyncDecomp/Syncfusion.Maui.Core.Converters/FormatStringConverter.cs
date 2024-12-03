using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace Syncfusion.Maui.Core.Converters;

public class FormatStringConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (value == null)
		{
			throw new ArgumentException("Value is Null");
		}
		if (!(value is IFormattable formattable) || !(parameter is string text))
		{
			throw new ArgumentException("value is not of type Iformattable or Format is null", nameof(value));
		}
		return formattable.ToString(text, null);
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotSupportedException("ConvertBack to original value is not possible. BindingMode must be set as OneWay.");
	}
}
