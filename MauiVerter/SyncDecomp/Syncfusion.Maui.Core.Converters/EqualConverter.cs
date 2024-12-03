using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace Syncfusion.Maui.Core.Converters;

public class EqualConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		bool flag = (value != null && value.Equals(parameter)) || (value == null && parameter == null);
		return flag;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotSupportedException("ConvertBack to original value is not possible. BindingMode must be set as OneWay.");
	}
}
