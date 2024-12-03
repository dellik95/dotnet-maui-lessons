using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.Maui.Controls;

namespace Syncfusion.Maui.Core.Converters;

public class ListToStringConverter : IValueConverter
{
	public string Separator { get; set; } = string.Empty;


	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (value != null)
		{
			if (value is ICollection source)
			{
				IEnumerable<string> values = from val in source.OfType<object>()
					select val.ToString();
				return string.Join(Separator, values);
			}
			throw new ArgumentException($"Value is of {new Func<Type>(value.GetType)}");
		}
		return string.Empty;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotSupportedException("ConvertBack to original value is not possible. BindingMode must be set as OneWay.");
	}
}
