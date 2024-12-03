using System.Collections.Generic;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Windows.Foundation;

namespace Syncfusion.Maui.Core.Internals;

internal class SfWindowOverlay
{
	private IWindow? window;

	private bool hasOverlayStackInRoot = false;

	private readonly Dictionary<FrameworkElement, PositionDetails> positionDetails;

	private Panel? rootView;

	private WindowOverlayStack? overlayStack;

	internal SfWindowOverlay()
	{
		positionDetails = new Dictionary<FrameworkElement, PositionDetails>();
		AddToWindow();
	}

	public void AddToWindow()
	{
		if (!hasOverlayStackInRoot)
		{
			IWindow window = Microsoft.Maui.Controls.Application.Current?.MainPage?.Window;
			if (window != null)
			{
				this.window = window;
			}
			Initialize();
		}
	}

	private void AlignPosition(WindowOverlayHorizontalAlignment horizontalAlignment, WindowOverlayVerticalAlignment verticalAlignment, float width, float height, ref float x, ref float y)
	{
		switch (horizontalAlignment)
		{
		case WindowOverlayHorizontalAlignment.Right:
			x -= width;
			break;
		case WindowOverlayHorizontalAlignment.Center:
			x -= width / 2f;
			break;
		}
		switch (verticalAlignment)
		{
		case WindowOverlayVerticalAlignment.Bottom:
			y -= height;
			break;
		case WindowOverlayVerticalAlignment.Center:
			y -= height / 2f;
			break;
		}
	}

	private void AlignPositionToRelative(WindowOverlayHorizontalAlignment horizontalAlignment, WindowOverlayVerticalAlignment verticalAlignment, float childWidth, float childHeight, float relatveViewWidth, float relatveViewHeight, ref float x, ref float y)
	{
		switch (horizontalAlignment)
		{
		case WindowOverlayHorizontalAlignment.Right:
			x += relatveViewWidth - childWidth;
			break;
		case WindowOverlayHorizontalAlignment.Center:
			x += relatveViewWidth / 2f - childWidth / 2f;
			break;
		}
		switch (verticalAlignment)
		{
		case WindowOverlayVerticalAlignment.Bottom:
			y += relatveViewHeight;
			break;
		case WindowOverlayVerticalAlignment.Center:
			y += relatveViewHeight / 2f - childHeight / 2f;
			break;
		case WindowOverlayVerticalAlignment.Top:
			y += 0f - childHeight;
			break;
		}
	}

	public virtual WindowOverlayStack CreateStack()
	{
		return new WindowOverlayStack();
	}

	public void AddOrUpdate(View child, double x, double y, WindowOverlayHorizontalAlignment horizontalAlignment = WindowOverlayHorizontalAlignment.Left, WindowOverlayVerticalAlignment verticalAlignment = WindowOverlayVerticalAlignment.Top)
	{
		AddToWindow();
		if (!hasOverlayStackInRoot || overlayStack == null || child == null)
		{
			return;
		}
		IMauiContext mauiContext = window?.Handler?.MauiContext;
		if (mauiContext != null)
		{
			FrameworkElement frameworkElement = child.ToPlatform(mauiContext);
			PositionDetails positionDetails;
			if (this.positionDetails.ContainsKey(frameworkElement))
			{
				positionDetails = this.positionDetails[frameworkElement];
			}
			else
			{
				positionDetails = new PositionDetails();
				this.positionDetails.Add(frameworkElement, positionDetails);
			}
			float x2 = (float)x;
			float y2 = (float)y;
			positionDetails.X = x2;
			positionDetails.Y = y2;
			positionDetails.HorizontalAlignment = horizontalAlignment;
			positionDetails.VerticalAlignment = verticalAlignment;
			if (!overlayStack.Children.Contains(frameworkElement))
			{
				overlayStack.Children.Add(frameworkElement);
				frameworkElement.LayoutUpdated += OnChildLayoutChanged;
			}
			if (frameworkElement.DesiredSize.Width <= 0.0 || frameworkElement.DesiredSize.Height <= 0.0)
			{
				frameworkElement.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
			}
			AlignPosition(horizontalAlignment, verticalAlignment, (float)frameworkElement.DesiredSize.Width, (float)frameworkElement.DesiredSize.Height, ref x2, ref y2);
			Canvas.SetLeft(frameworkElement, x2);
			Canvas.SetTop(frameworkElement, y2);
		}
	}

	public void AddOrUpdate(View child, View relative, double x = 0.0, double y = 0.0, WindowOverlayHorizontalAlignment horizontalAlignment = WindowOverlayHorizontalAlignment.Left, WindowOverlayVerticalAlignment verticalAlignment = WindowOverlayVerticalAlignment.Top)
	{
		AddToWindow();
		if (!hasOverlayStackInRoot || overlayStack == null || child == null || relative == null || relative.Width < 0.0 || relative.Height < 0.0)
		{
			return;
		}
		IMauiContext mauiContext = window?.Handler?.MauiContext;
		if (mauiContext != null && relative.Handler != null && relative.Handler.MauiContext != null)
		{
			FrameworkElement frameworkElement = child.ToPlatform(mauiContext);
			FrameworkElement frameworkElement2 = relative.ToPlatform(relative.Handler.MauiContext);
			PositionDetails positionDetails;
			if (this.positionDetails.ContainsKey(frameworkElement))
			{
				positionDetails = this.positionDetails[frameworkElement];
			}
			else
			{
				positionDetails = new PositionDetails();
				this.positionDetails.Add(frameworkElement, positionDetails);
			}
			float x2 = (float)x;
			float y2 = (float)y;
			positionDetails.X = x2;
			positionDetails.Y = y2;
			positionDetails.HorizontalAlignment = horizontalAlignment;
			positionDetails.VerticalAlignment = verticalAlignment;
			positionDetails.Relative = frameworkElement2;
			if (!overlayStack.Children.Contains(frameworkElement))
			{
				overlayStack.Children.Add(frameworkElement);
				frameworkElement.LayoutUpdated += OnChildLayoutChanged;
			}
			if (frameworkElement.DesiredSize.Width <= 0.0 && frameworkElement.DesiredSize.Height <= 0.0)
			{
				frameworkElement.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
			}
			AlignPositionToRelative(horizontalAlignment, verticalAlignment, (float)frameworkElement.DesiredSize.Width, (float)frameworkElement.DesiredSize.Height, frameworkElement2.ActualSize.X, frameworkElement2.ActualSize.Y, ref x2, ref y2);
			GeneralTransform generalTransform = frameworkElement2.TransformToVisual(rootView);
			Point point = generalTransform.TransformPoint(new Point(0f, 0f));
			Canvas.SetLeft(frameworkElement, (double)x2 + point.X);
			Canvas.SetTop(frameworkElement, (double)y2 + point.Y);
		}
	}

	public void Remove(View view)
	{
		if (hasOverlayStackInRoot && view != null && view.Handler != null && view.Handler.MauiContext != null)
		{
			FrameworkElement frameworkElement = view.ToPlatform(view.Handler.MauiContext);
			frameworkElement.LayoutUpdated -= OnChildLayoutChanged;
			overlayStack?.Children.Remove(frameworkElement);
			positionDetails.Remove(frameworkElement);
		}
	}

	public void RemoveFromWindow()
	{
		if (overlayStack != null)
		{
			ClearChildren();
			rootView?.Children.Remove(overlayStack);
			overlayStack = null;
		}
		hasOverlayStackInRoot = false;
	}

	private void Initialize()
	{
		if (hasOverlayStackInRoot || window == null || window.Content == null)
		{
			return;
		}
		FrameworkElement frameworkElement = window.Content.ToPlatform();
		if (frameworkElement == null || !(window.Handler is WindowHandler windowHandler))
		{
			return;
		}
		Microsoft.UI.Xaml.Window platformView = windowHandler.PlatformView;
		if ((object)platformView == null)
		{
			return;
		}
		rootView = platformView.Content as Panel;
		if (!(rootView == null))
		{
			if ((object)overlayStack == null)
			{
				overlayStack = CreateStack();
			}
			if (!rootView.Children.Contains(overlayStack))
			{
				rootView.Children.Add(overlayStack);
			}
			overlayStack.SetValue(Canvas.ZIndexProperty, 99);
			hasOverlayStackInRoot = true;
		}
	}

	private void OnChildLayoutChanged(object? sender, object e)
	{
		if (sender == null)
		{
			return;
		}
		FrameworkElement frameworkElement = (FrameworkElement)sender;
		if (positionDetails.TryGetValue(frameworkElement, out PositionDetails value) && value != null)
		{
			FrameworkElement relative = value.Relative;
			float x = value.X;
			float y = value.Y;
			if (relative == null && frameworkElement.Width > 0.0 && frameworkElement.Height > 0.0)
			{
				AlignPosition(value.HorizontalAlignment, value.VerticalAlignment, (float)frameworkElement.DesiredSize.Width, (float)frameworkElement.DesiredSize.Height, ref x, ref y);
				Canvas.SetLeft(frameworkElement, x);
				Canvas.SetTop(frameworkElement, y);
			}
			else if (relative != null && relative.Width > 0.0 && relative.Height > 0.0)
			{
				AlignPositionToRelative(value.HorizontalAlignment, value.VerticalAlignment, (float)frameworkElement.DesiredSize.Width, (float)frameworkElement.DesiredSize.Height, relative.ActualSize.X, relative.ActualSize.Y, ref x, ref y);
				GeneralTransform generalTransform = relative.TransformToVisual(rootView);
				Point point = generalTransform.TransformPoint(new Point(0f, 0f));
				Canvas.SetLeft(frameworkElement, (double)x + point.X);
				Canvas.SetTop(frameworkElement, (double)y + point.Y);
			}
		}
	}

	private void ClearChildren()
	{
		if (!(overlayStack != null) || positionDetails.Count <= 0)
		{
			return;
		}
		foreach (FrameworkElement key in positionDetails.Keys)
		{
			key.LayoutUpdated -= OnChildLayoutChanged;
			overlayStack.Children.Remove(key);
		}
		positionDetails.Clear();
	}
}
