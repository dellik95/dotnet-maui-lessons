using System;
using System.Globalization;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace Syncfusion.Maui.Core.Converters;

public class ColorToGrayScaleColorConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (value != null && value is Color color)
		{
			return GetGrayScaleColor(color);
		}
		throw new ArgumentException("Expected value to be a type of color", nameof(value));
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotSupportedException("Converting back to the original value is not possible. Binding mode must be set as one way.");
	}

	private Color GetGrayScaleColor(Color color)
	{
		float gray = (color.Red + color.Green + color.Blue) / 3f;
		return new Color(gray);
	}
}
