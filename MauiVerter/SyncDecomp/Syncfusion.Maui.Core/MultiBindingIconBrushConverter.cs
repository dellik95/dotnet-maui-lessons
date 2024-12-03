using System;
using System.Globalization;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace Syncfusion.Maui.Core;

internal class MultiBindingIconBrushConverter : IMultiValueConverter
{
	public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
	{
		if (values != null && targetType.IsAssignableFrom(typeof(Brush)) && parameter is SfShapeView sfShapeView && sfShapeView.BindingContext is LegendItem legendItem)
		{
			return legendItem.IsToggled ? legendItem.DisableBrush : legendItem.IconBrush;
		}
		return new SolidColorBrush(Colors.Transparent);
	}

	public object[]? ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
	{
		return null;
	}
}
