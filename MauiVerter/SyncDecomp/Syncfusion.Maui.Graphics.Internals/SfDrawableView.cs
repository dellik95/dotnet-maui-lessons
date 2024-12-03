using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace Syncfusion.Maui.Graphics.Internals;

public class SfDrawableView : View, IDrawableView, IView, IElement, ITransform, IDrawable
{
	void IDrawable.Draw(ICanvas canvas, RectF dirtyRect)
	{
		OnDraw(canvas, dirtyRect);
	}

	protected virtual void OnDraw(ICanvas canvas, RectF dirtyRect)
	{
	}

	public void InvalidateDrawable()
	{
		if (base.Handler is SfDrawableViewHandler sfDrawableViewHandler)
		{
			sfDrawableViewHandler.Invalidate();
		}
	}
}
