using System;
using System.Globalization;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace Syncfusion.Maui.Core.Converters;

public class ColorToOnColorConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (value != null && value is Color color)
		{
			return GetColorForText(color);
		}
		throw new ArgumentException("Expected value to be a type of color", nameof(value));
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotSupportedException("ConvertBack to original value is not possible. BindingMode must be set as OneWay.");
	}

	private Color GetColorForText(Color color)
	{
		return ((double)color.GetLuminosity() > 0.72) ? Colors.Black : Colors.White;
	}
}
