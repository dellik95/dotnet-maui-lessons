using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace Syncfusion.Maui.Core.Converters;

public class EnumToIntConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (value != null)
		{
			if (value is Enum)
			{
				return System.Convert.ToInt32(value);
			}
			throw new ArgumentException("Value is not of enum type, it is of value");
		}
		throw new ArgumentException("Value is null");
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (value != null)
		{
			if (value is int num && Enum.IsDefined(targetType, num))
			{
				return Enum.ToObject(targetType, num);
			}
			throw new ArgumentException("Value is not of enummeration type");
		}
		throw new ArgumentException("Value is null");
	}
}
