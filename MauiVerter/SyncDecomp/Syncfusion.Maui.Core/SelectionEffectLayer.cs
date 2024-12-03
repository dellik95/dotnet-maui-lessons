using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace Syncfusion.Maui.Core;

internal class SelectionEffectLayer
{
	private const float SelectionTransparencyFactor = 0.12f;

	private Rect selectionBounds;

	private Brush selectionColor = new SolidColorBrush(Colors.Black);

	private readonly IDrawableLayout drawable;

	internal double Width { get; set; }

	internal double Height { get; set; }

	public SelectionEffectLayer(Brush selectionColor, IDrawableLayout drawable)
	{
		this.selectionColor = selectionColor;
		this.drawable = drawable;
	}

	internal void DrawSelection(ICanvas canvas)
	{
		if (selectionColor != null)
		{
			canvas.Alpha = 0.12f;
			canvas.SetFillPaint(selectionColor, selectionBounds);
			canvas.FillRectangle(selectionBounds);
		}
	}

	internal void UpdateSelectionBounds(double width = 0.0, double height = 0.0, Brush? selectionColor = null)
	{
		selectionColor ??= new SolidColorBrush(Colors.Transparent);
		this.selectionColor = selectionColor;
		selectionBounds = new Rect(0.0, 0.0, width, height);
		drawable.InvalidateDrawable();
	}
}
