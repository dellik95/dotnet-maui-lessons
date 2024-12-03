using System.Collections;
using Microsoft.Maui.Controls;

namespace Syncfusion.Maui.Core;

internal interface ILegend
{
	IEnumerable ItemsSource { get; set; }

	bool ToggleVisibility { get; set; }

	LegendPlacement Placement { get; set; }

	IItemsLayout ItemsLayout { get; set; }

	bool IsVisible { get; set; }

	DataTemplate ItemTemplate { get; set; }

	SfLegend? CreateLegendView()
	{
		return null;
	}
}
