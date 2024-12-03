using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Syncfusion.Maui.Graphics.Internals;

namespace Syncfusion.Maui.Core;

internal class HighlightEffectLayer
{
	private const float HighlightTransparencyFactor = 0.04f;

	private Rect highlightBounds;

	private Brush highlightColor = new SolidColorBrush(Colors.Black);

	private readonly IDrawable drawable;

	internal double Width { get; set; }

	internal double Height { get; set; }

	public HighlightEffectLayer(Brush highlightColor, IDrawable drawable)
	{
		this.highlightColor = highlightColor;
		this.drawable = drawable;
	}

	internal void DrawHighlight(ICanvas canvas, RectF rectF, Brush highlightColorValue)
	{
		if (highlightColor != null)
		{
			canvas.SetFillPaint(highlightColorValue, rectF);
			canvas.FillRectangle(rectF);
		}
	}

	internal void DrawHighlight(ICanvas canvas)
	{
		if (highlightColor != null)
		{
			canvas.Alpha = 0.04f;
			DrawHighlight(canvas, highlightBounds, highlightColor);
		}
	}

	internal void UpdateHighlightBounds(double width = 0.0, double height = 0.0, Brush? highlightColor = null)
	{
		highlightColor ??= new SolidColorBrush(Colors.Transparent);
		this.highlightColor = highlightColor;
		highlightBounds = new Rect(0.0, 0.0, width, height);
		if (drawable is IDrawableLayout drawableLayout)
		{
			drawableLayout.InvalidateDrawable();
		}
		else if (drawable is IDrawableView drawableView)
		{
			drawableView.InvalidateDrawable();
		}
	}
}
