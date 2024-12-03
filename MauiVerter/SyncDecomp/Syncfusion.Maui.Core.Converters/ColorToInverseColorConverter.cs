using System;
using System.Globalization;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace Syncfusion.Maui.Core.Converters;

public class ColorToInverseColorConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		return ConvertColorToInverseColor(value);
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		return ConvertColorToInverseColor(value);
	}

	private Color ConvertColorToInverseColor(object value)
	{
		if (value != null && value is Color color)
		{
			return new Color(1f - color.Red, 1f - color.Green, 1f - color.Blue);
		}
		throw new ArgumentException("Expected value to be a type of color", nameof(value));
	}
}
