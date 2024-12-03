using Microsoft.Maui;
using Microsoft.Maui.Graphics.Win2D;
using Microsoft.Maui.Handlers;

namespace Syncfusion.Maui.Graphics.Internals;

public class SfDrawableViewHandler : ViewHandler<IDrawableView, W2DGraphicsView>
{
	public SfDrawableViewHandler()
		: base((IPropertyMapper)ViewHandler.ViewMapper, (CommandMapper?)null)
	{
	}

	public SfDrawableViewHandler(PropertyMapper mapper)
		: base((IPropertyMapper)mapper, (CommandMapper?)null)
	{
	}

	protected override W2DGraphicsView CreatePlatformView()
	{
		W2DGraphicsView w2DGraphicsView = new W2DGraphicsView();
		w2DGraphicsView.Drawable = base.VirtualView;
		w2DGraphicsView.UseSystemFocusVisuals = true;
		w2DGraphicsView.IsTabStop = true;
		return w2DGraphicsView;
	}

	public void Invalidate()
	{
		base.PlatformView?.Invalidate();
	}
}
