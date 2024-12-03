using System;
using System.Collections;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace Syncfusion.Maui.Core.Converters;

public class IsListNotNullOrEmptyConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (value == null)
		{
			return false;
		}
		if (!(value is ICollection))
		{
			throw new ArgumentException("Value cannot be casted to ICollection", nameof(value));
		}
		ICollection collection = (ICollection)value;
		return collection.Count != 0;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotSupportedException("ConvertBack to original value is not possible. BindingMode must be set as OneWay.");
	}
}
