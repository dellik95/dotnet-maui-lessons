using System;
using Microsoft.Maui;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using Syncfusion.Maui.Core.Platform;

namespace Syncfusion.Maui.Core;

public class SfViewHandler : LayoutHandler
{
	private LayoutPanelExt? layoutPanelExt;

	public SfViewHandler()
		: base(ViewHandler.ViewMapper)
	{
	}

	public SfViewHandler(PropertyMapper mapper)
		: base(mapper)
	{
	}

	protected override LayoutPanel CreatePlatformView()
	{
		if (base.VirtualView == null)
		{
			throw new InvalidOperationException("VirtualView must be set to create a LayoutViewGroup");
		}
		layoutPanelExt = new LayoutPanelExt((IDrawableLayout)base.VirtualView)
		{
			CrossPlatformMeasure = base.VirtualView.CrossPlatformMeasure,
			CrossPlatformArrange = base.VirtualView.CrossPlatformArrange
		};
		return layoutPanelExt;
	}

	public override void SetVirtualView(IView view)
	{
		base.SetVirtualView(view);
		if (base.VirtualView == null)
		{
			throw new InvalidOperationException("VirtualView should have been set by base class.");
		}
		if (layoutPanelExt != null)
		{
			layoutPanelExt.CrossPlatformMeasure = base.VirtualView.CrossPlatformMeasure;
			layoutPanelExt.CrossPlatformArrange = base.VirtualView.CrossPlatformArrange;
		}
	}

	public void Invalidate()
	{
		layoutPanelExt?.Invalidate();
	}

	public void SetDrawingOrder(DrawingOrder drawingOrder = DrawingOrder.NoDraw)
	{
		if (layoutPanelExt != null)
		{
			layoutPanelExt.DrawingOrder = drawingOrder;
		}
	}

	public void UpdateClipToBounds(bool clipToBounds)
	{
		if (layoutPanelExt != null)
		{
			layoutPanelExt.ClipsToBounds = clipToBounds;
		}
	}

	public new void Add(IView child)
	{
		if ((object)base.PlatformView == null)
		{
			throw new InvalidOperationException("PlatformView should have been set by base class.");
		}
		if (base.VirtualView == null)
		{
			throw new InvalidOperationException("VirtualView should have been set by base class.");
		}
		if (base.MauiContext == null)
		{
			throw new InvalidOperationException("MauiContext should have been set by base class.");
		}
		int count = base.PlatformView.Children.Count;
		if (layoutPanelExt != null)
		{
			LayoutPanelExt? obj = layoutPanelExt;
			if ((object)obj != null && obj.DrawingOrder == DrawingOrder.AboveContent)
			{
				base.PlatformView.Children.Insert(count - 1, child.ToPlatform(base.MauiContext));
			}
			else
			{
				base.PlatformView.Children.Insert(count, child.ToPlatform(base.MauiContext));
			}
		}
	}

	public new void Insert(int index, IView child)
	{
		if ((object)base.PlatformView == null)
		{
			throw new InvalidOperationException("PlatformView should have been set by base class.");
		}
		if (base.VirtualView == null)
		{
			throw new InvalidOperationException("VirtualView should have been set by base class.");
		}
		if (base.MauiContext == null)
		{
			throw new InvalidOperationException("MauiContext should have been set by base class.");
		}
		if (layoutPanelExt != null)
		{
			LayoutPanelExt? obj = layoutPanelExt;
			if ((object)obj != null && obj.DrawingOrder == DrawingOrder.BelowContent)
			{
				base.PlatformView.Children.Insert(index + 1, child.ToPlatform(base.MauiContext));
			}
			else
			{
				base.PlatformView.Children.Insert(index, child.ToPlatform(base.MauiContext));
			}
		}
	}

	protected override void DisconnectHandler(LayoutPanel platformView)
	{
		base.DisconnectHandler(platformView);
		foreach (IView item in base.VirtualView)
		{
			item.Handler?.DisconnectHandler();
		}
		if (layoutPanelExt != null)
		{
			layoutPanelExt.Dispose();
			layoutPanelExt = null;
		}
	}
}
