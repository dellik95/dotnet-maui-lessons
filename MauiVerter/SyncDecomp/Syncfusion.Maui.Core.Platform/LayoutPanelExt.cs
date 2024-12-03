using System;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Graphics.Win2D;
using Microsoft.Maui.Platform;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using Windows.Foundation;

namespace Syncfusion.Maui.Core.Platform;

internal class LayoutPanelExt : LayoutPanel
{
	private W2DGraphicsView? nativeGraphicsView;

	private DrawingOrder drawingOrder = DrawingOrder.NoDraw;

	internal Func<double, double, Microsoft.Maui.Graphics.Size>? CrossPlatformMeasure { get; set; }

	internal Func<Microsoft.Maui.Graphics.Rect, Microsoft.Maui.Graphics.Size>? CrossPlatformArrange { get; set; }

	public DrawingOrder DrawingOrder
	{
		get
		{
			return drawingOrder;
		}
		set
		{
			drawingOrder = value;
			if (DrawingOrder == DrawingOrder.NoDraw)
			{
				RemoveDrawableView();
				return;
			}
			InitializeNativeGraphicsView();
			ArrangeNativeGraphicsView();
		}
	}

	public IDrawable Drawable { get; set; }

	public LayoutPanelExt(IDrawableLayout layout)
	{
		Drawable = layout;
		base.AllowFocusOnInteraction = true;
		base.UseSystemFocusVisuals = true;
		base.SizeChanged += ContentPanelExt_SizeChanged;
	}

	private void ContentPanelExt_SizeChanged(object sender, SizeChangedEventArgs e)
	{
		nativeGraphicsView?.Invalidate();
	}

	internal void InitializeNativeGraphicsView()
	{
		if (!base.Children.Contains(nativeGraphicsView))
		{
			nativeGraphicsView = new W2DGraphicsView
			{
				Drawable = Drawable
			};
		}
		if (nativeGraphicsView != null)
		{
			if (DrawingOrder == DrawingOrder.AboveContentWithTouch || DrawingOrder == DrawingOrder.BelowContent)
			{
				nativeGraphicsView.IsHitTestVisible = true;
			}
			else
			{
				nativeGraphicsView.IsHitTestVisible = false;
			}
		}
	}

	internal void RemoveDrawableView()
	{
		if (nativeGraphicsView != null && base.Children.Contains(nativeGraphicsView))
		{
			base.Children.Remove(nativeGraphicsView);
		}
	}

	internal void ArrangeNativeGraphicsView()
	{
		if (nativeGraphicsView != null)
		{
			if (base.Children.Contains(nativeGraphicsView))
			{
				base.Children.Remove(nativeGraphicsView);
			}
			if (DrawingOrder == DrawingOrder.AboveContentWithTouch || DrawingOrder == DrawingOrder.AboveContent)
			{
				base.Children.Add(nativeGraphicsView);
			}
			else
			{
				base.Children.Insert(0, nativeGraphicsView);
			}
		}
	}

	internal void Invalidate()
	{
		nativeGraphicsView?.Invalidate();
	}

	protected override Windows.Foundation.Size ArrangeOverride(Windows.Foundation.Size finalSize)
	{
		if (CrossPlatformArrange == null)
		{
			return base.ArrangeOverride(finalSize);
		}
		double width = finalSize.Width;
		double height = finalSize.Height;
		CrossPlatformArrange(new Microsoft.Maui.Graphics.Rect(0.0, 0.0, width, height));
		if (base.ClipsToBounds && base.Clip != null && (base.Clip.Bounds.Width != finalSize.Width || base.Clip.Bounds.Height != finalSize.Height))
		{
			base.Clip = new RectangleGeometry
			{
				Rect = new Windows.Foundation.Rect(0.0, 0.0, finalSize.Width, finalSize.Height)
			};
		}
		nativeGraphicsView?.Arrange(new Windows.Foundation.Rect(0.0, 0.0, width, height));
		return finalSize;
	}

	protected override Windows.Foundation.Size MeasureOverride(Windows.Foundation.Size availableSize)
	{
		if (CrossPlatformMeasure == null)
		{
			return base.MeasureOverride(availableSize);
		}
		double width = availableSize.Width;
		double height = availableSize.Height;
		Microsoft.Maui.Graphics.Size size = CrossPlatformMeasure(width, height);
		width = size.Width;
		height = size.Height;
		nativeGraphicsView?.Measure(availableSize);
		return new Windows.Foundation.Size(width, height);
	}

	internal void Dispose()
	{
		if (nativeGraphicsView != null)
		{
			nativeGraphicsView = null;
		}
	}
}
