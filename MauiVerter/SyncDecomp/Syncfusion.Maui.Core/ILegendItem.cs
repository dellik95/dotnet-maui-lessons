using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace Syncfusion.Maui.Core;

public interface ILegendItem
{
	string Text { get; set; }

	string FontFamily { get; set; }

	FontAttributes FontAttributes { get; set; }

	Brush IconBrush { get; set; }

	Color TextColor { get; set; }

	double IconWidth { get; set; }

	double IconHeight { get; set; }

	float FontSize { get; set; }

	Thickness TextMargin { get; set; }

	ShapeType IconType { get; set; }

	bool IsToggled { get; set; }

	Brush DisableBrush { get; set; }

	bool IsIconVisible { get; set; }

	int Index { get; }

	object? Item { get; }
}
