using System;
using System.Globalization;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace Syncfusion.Maui.Core;

internal class ToggleColorConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (value != null && parameter != null)
		{
			bool flag = (bool)value;
			if (parameter is SfShapeView sfShapeView && sfShapeView.BindingContext is LegendItem legendItem)
			{
				return flag ? legendItem.DisableBrush : legendItem.IconBrush;
			}
			if (parameter is Label label && label.BindingContext is LegendItem legendItem2)
			{
				SolidColorBrush solidColorBrush = (SolidColorBrush)legendItem2.DisableBrush;
				return flag ? solidColorBrush.Color : legendItem2.TextColor;
			}
		}
		return Colors.Transparent;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		return value;
	}
}
