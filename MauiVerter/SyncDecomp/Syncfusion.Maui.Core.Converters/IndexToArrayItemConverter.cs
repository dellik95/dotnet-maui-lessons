using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.Maui.Controls;

namespace Syncfusion.Maui.Core.Converters;

public class IndexToArrayItemConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (value == null)
		{
			throw new ArgumentNullException("Value should not null", "value");
		}
		if (parameter == null)
		{
			throw new ArgumentNullException("Parameter should not null", "parameter");
		}
		int num;
		if (value is int)
		{
			num = (int)value;
			if (true)
			{
				goto IL_007b;
			}
		}
		if (!(value is double))
		{
			throw new ArgumentException("Value is not a valid integer", nameof(value));
		}
		num = System.Convert.ToInt32(value);
		goto IL_007b;
		IL_007b:
		if (!(parameter is ICollection<object> source))
		{
			throw new ArgumentException("Parameter is not a valid array", nameof(parameter));
		}
		List<object> list = source.ToList();
		if (num < 0 || num >= list.Count)
		{
			throw new ArgumentOutOfRangeException("Index was out of range", "value");
		}
		return list[num];
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (value == null)
		{
			throw new ArgumentNullException("Value should not be null", "value");
		}
		if (parameter == null)
		{
			throw new ArgumentNullException("Parameter should not null", "parameter");
		}
		if (!(parameter is ICollection<object> source))
		{
			throw new ArgumentException("Parameter is not a valid array", nameof(parameter));
		}
		List<object> list = source.ToList();
		if (list.Contains(value))
		{
			return list.IndexOf(value);
		}
		throw new ArgumentException("Value does not exist in the array", nameof(value));
	}
}
