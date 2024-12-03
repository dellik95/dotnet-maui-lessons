using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace Syncfusion.Maui.Graphics.Internals;

public interface ILineDrawing
{
	Color Stroke { get; set; }

	double StrokeWidth { get; set; }

	bool EnableAntiAliasing { get; set; }

	float Opacity { get; set; }

	DoubleCollection? StrokeDashArray { get; set; }
}
