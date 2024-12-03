using System;
using System.Globalization;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace Syncfusion.Maui.Core.Converters;

public class BrushToColorConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (!Brush.IsNullOrEmpty(value as SolidColorBrush))
		{
			SolidColorBrush solidColorBrush = (SolidColorBrush)value;
			return solidColorBrush.Color;
		}
		throw new ArgumentException("Expected value to be a type of brush", nameof(value));
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (value != null && value is Color color)
		{
			return new SolidColorBrush(color);
		}
		throw new ArgumentException("Expected value to be a type of color", nameof(value));
	}
}
