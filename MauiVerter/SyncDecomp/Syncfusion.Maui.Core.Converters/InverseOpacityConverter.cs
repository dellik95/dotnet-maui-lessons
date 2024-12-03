using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace Syncfusion.Maui.Core.Converters;

public class InverseOpacityConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		double num = ReturnOpacityValue(value);
		return num;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		double num = ReturnOpacityValue(value);
		return num;
	}

	private double ReturnOpacityValue(object value)
	{
		if (value != null)
		{
			double num = System.Convert.ToDouble(value);
			double value2 = 1.0 - num;
			if (num >= 0.0 && num <= 1.0)
			{
				return Math.Round(value2, 1);
			}
			if (num < 0.0)
			{
				return 0.0;
			}
			return 1.0;
		}
		throw new ArgumentException("Value is null");
	}
}
