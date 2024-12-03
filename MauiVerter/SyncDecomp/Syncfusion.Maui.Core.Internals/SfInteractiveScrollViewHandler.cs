using System;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Windows.System;

namespace Syncfusion.Maui.Core.Internals;

internal class SfInteractiveScrollViewHandler : ViewHandler<SfInteractiveScrollView, ScrollViewer>
{
	internal static IPropertyMapper<SfInteractiveScrollView, SfInteractiveScrollViewHandler> Mapper = new PropertyMapper<SfInteractiveScrollView, SfInteractiveScrollViewHandler>(ViewHandler.ViewMapper)
	{
		["PresentedContent"] = MapContent,
		["HorizontalScrollBarVisibility"] = MapHorizontalScrollBarVisibility,
		["VerticalScrollBarVisibility"] = MapVerticalScrollBarVisibility,
		["Orientation"] = MapScrollOrientation,
		["ContentSize"] = MapContentSize
	};

	internal static CommandMapper<SfInteractiveScrollView, SfInteractiveScrollViewHandler> CommandMapper = new CommandMapper<SfInteractiveScrollView, SfInteractiveScrollViewHandler>(ViewHandler.ViewCommandMapper) { ["ScrollTo"] = MapScrollTo };

	private ScrollToParameters? m_scrollOffsetRequest;

	private FrameworkElement? m_content;

	private double m_dpi = 96.0;

	public SfInteractiveScrollViewHandler()
		: base((IPropertyMapper)Mapper, (CommandMapper?)CommandMapper)
	{
	}

	protected override ScrollViewer CreatePlatformView()
	{
		ScrollViewer scrollViewer = new ScrollViewer();
		scrollViewer.Loaded += OnLoaded;
		return scrollViewer;
	}

	protected override void ConnectHandler(ScrollViewer platformView)
	{
		base.ConnectHandler(platformView);
		platformView.ViewChanged += OnViewChanged;
		platformView.KeyDown += OnKeyDown;
		platformView.KeyUp += OnKeyUp;
		platformView.ManipulationInertiaStarting += OnManipulationInertiaStarting;
		platformView.EffectiveViewportChanged += OnEffectiveViewportChanged;
	}

	protected override void DisconnectHandler(ScrollViewer platformView)
	{
		if (m_content != null)
		{
			m_content.SizeChanged -= OnContentSizeChanged;
		}
		platformView.Loaded -= OnLoaded;
		platformView.KeyDown -= OnKeyDown;
		platformView.KeyUp -= OnKeyUp;
		platformView.ViewChanged -= OnViewChanged;
		platformView.EffectiveViewportChanged -= OnEffectiveViewportChanged;
		platformView.ManipulationInertiaStarting -= OnManipulationInertiaStarting;
		platformView.Content = null;
		base.DisconnectHandler(platformView);
	}

	public static void MapScrollOrientation(SfInteractiveScrollViewHandler handler, SfInteractiveScrollView scrollView)
	{
		handler.UpdateScrollOrientation();
	}

	public static void MapHorizontalScrollBarVisibility(SfInteractiveScrollViewHandler handler, SfInteractiveScrollView scrollView)
	{
		if (scrollView.Orientation != ScrollOrientation.Neither && scrollView.Orientation != 0)
		{
			handler.PlatformView.HorizontalScrollBarVisibility = handler.GetWScrollBarVisibility(scrollView.HorizontalScrollBarVisibility);
		}
	}

	public static void MapVerticalScrollBarVisibility(SfInteractiveScrollViewHandler handler, SfInteractiveScrollView scrollView)
	{
		if (scrollView.Orientation != ScrollOrientation.Neither && scrollView.Orientation != ScrollOrientation.Horizontal)
		{
			handler.PlatformView.VerticalScrollBarVisibility = handler.GetWScrollBarVisibility(scrollView.VerticalScrollBarVisibility);
		}
	}

	public static void MapScrollTo(SfInteractiveScrollViewHandler handler, SfInteractiveScrollView scrollView, object? args)
	{
		if (args is ScrollToParameters parameters)
		{
			handler.ScrollTo(parameters);
		}
	}

	public static void MapContentSize(SfInteractiveScrollViewHandler handler, SfInteractiveScrollView scrollView)
	{
		if (!(handler.m_content == null))
		{
			if (scrollView.ContentSize.IsZero || (scrollView.ContentSize.Width == handler.m_content.ActualWidth && scrollView.ContentSize.Height == handler.m_content.ActualHeight))
			{
				scrollView.IsContentLayoutRequested = false;
			}
			else
			{
				scrollView.IsContentLayoutRequested = true;
			}
		}
	}

	public static void MapContent(SfInteractiveScrollViewHandler handler, SfInteractiveScrollView scrollView)
	{
		if (!(handler.PlatformView == null) && handler.MauiContext != null)
		{
			View presentedContent = scrollView.PresentedContent;
			if (presentedContent != null)
			{
				FrameworkElement frameworkElement = (handler.m_content = presentedContent.ToPlatform(handler.MauiContext));
				frameworkElement.IsTabStop = true;
				frameworkElement.SizeChanged += handler.OnContentSizeChanged;
				handler.PlatformView.Content = frameworkElement;
			}
		}
	}

	private void UpdateScrollOrientation()
	{
		switch (base.VirtualView.Orientation)
		{
		case ScrollOrientation.Neither:
			SetScrollBarVisibility(Microsoft.UI.Xaml.Controls.ScrollBarVisibility.Hidden, Microsoft.UI.Xaml.Controls.ScrollBarVisibility.Hidden);
			SetScrollMode(Microsoft.UI.Xaml.Controls.ScrollMode.Disabled, Microsoft.UI.Xaml.Controls.ScrollMode.Disabled);
			break;
		case ScrollOrientation.Vertical:
			SetScrollBarVisibility(Microsoft.UI.Xaml.Controls.ScrollBarVisibility.Hidden, GetWScrollBarVisibility(base.VirtualView.VerticalScrollBarVisibility));
			SetScrollMode(Microsoft.UI.Xaml.Controls.ScrollMode.Disabled, Microsoft.UI.Xaml.Controls.ScrollMode.Enabled);
			break;
		case ScrollOrientation.Horizontal:
			SetScrollBarVisibility(GetWScrollBarVisibility(base.VirtualView.HorizontalScrollBarVisibility), Microsoft.UI.Xaml.Controls.ScrollBarVisibility.Hidden);
			SetScrollMode(Microsoft.UI.Xaml.Controls.ScrollMode.Enabled, Microsoft.UI.Xaml.Controls.ScrollMode.Disabled);
			break;
		case ScrollOrientation.Both:
			SetScrollBarVisibility(GetWScrollBarVisibility(base.VirtualView.HorizontalScrollBarVisibility), GetWScrollBarVisibility(base.VirtualView.VerticalScrollBarVisibility));
			SetScrollMode(Microsoft.UI.Xaml.Controls.ScrollMode.Enabled, Microsoft.UI.Xaml.Controls.ScrollMode.Enabled);
			break;
		}
	}

	private void SetScrollBarVisibility(Microsoft.UI.Xaml.Controls.ScrollBarVisibility hScrollBarVisibility, Microsoft.UI.Xaml.Controls.ScrollBarVisibility vScrollBarVisibility)
	{
		base.PlatformView.HorizontalScrollBarVisibility = hScrollBarVisibility;
		base.PlatformView.VerticalScrollBarVisibility = vScrollBarVisibility;
	}

	private void SetScrollMode(Microsoft.UI.Xaml.Controls.ScrollMode horizontalScrollMode, Microsoft.UI.Xaml.Controls.ScrollMode verticalScrollMode)
	{
		base.PlatformView.HorizontalScrollMode = horizontalScrollMode;
		base.PlatformView.VerticalScrollMode = verticalScrollMode;
	}

	private Microsoft.UI.Xaml.Controls.ScrollBarVisibility GetWScrollBarVisibility(Microsoft.Maui.ScrollBarVisibility scrollBarVisibility)
	{
		return scrollBarVisibility switch
		{
			Microsoft.Maui.ScrollBarVisibility.Always => Microsoft.UI.Xaml.Controls.ScrollBarVisibility.Visible, 
			Microsoft.Maui.ScrollBarVisibility.Never => Microsoft.UI.Xaml.Controls.ScrollBarVisibility.Hidden, 
			_ => Microsoft.UI.Xaml.Controls.ScrollBarVisibility.Auto, 
		};
	}

	private void ScrollTo(ScrollToParameters parameters)
	{
		if (!base.VirtualView.IsContentLayoutRequested)
		{
			if (parameters.ScrollX != base.PlatformView.HorizontalOffset || parameters.ScrollY != base.PlatformView.VerticalOffset)
			{
				base.PlatformView.ChangeView(parameters.ScrollX, parameters.ScrollY, null, !parameters.Animated);
			}
			else
			{
				ScrollChangedEventArgs scrolledEventArgs = new ScrollChangedEventArgs(base.PlatformView.HorizontalOffset, base.PlatformView.VerticalOffset, base.VirtualView.ScrollX, base.VirtualView.ScrollY);
				base.VirtualView.OnScrollChanged(scrolledEventArgs);
			}
			m_scrollOffsetRequest = null;
		}
		else
		{
			m_scrollOffsetRequest = parameters;
		}
	}

	private void OnContentSizeChanged(object sender, SizeChangedEventArgs e)
	{
		if (base.VirtualView.IsContentLayoutRequested)
		{
			base.VirtualView.IsContentLayoutRequested = false;
		}
		if (m_scrollOffsetRequest != null)
		{
			ScrollTo(m_scrollOffsetRequest);
			m_scrollOffsetRequest = null;
		}
	}

	private void OnEffectiveViewportChanged(FrameworkElement sender, EffectiveViewportChangedEventArgs args)
	{
		base.VirtualView.ViewportWidth = base.PlatformView.ViewportWidth;
		base.VirtualView.ViewportHeight = base.PlatformView.ViewportHeight;
	}

	private void OnManipulationInertiaStarting(object sender, ManipulationInertiaStartingRoutedEventArgs e)
	{
		if (base.VirtualView == null || base.VirtualView.Orientation == ScrollOrientation.Neither)
		{
			return;
		}
		double num = 8.0;
		double num2 = 0.25;
		double num3 = base.PlatformView.HorizontalOffset;
		double num4 = base.PlatformView.VerticalOffset;
		double num5 = Math.Pow(e.Velocities.Linear.X, 2.0);
		double num6 = Math.Pow(e.Velocities.Linear.Y, 2.0);
		double num7 = e.Velocities.Linear.X / Math.Abs(e.Velocities.Linear.X);
		double num8 = e.Velocities.Linear.Y / Math.Abs(e.Velocities.Linear.Y);
		if (!(num5 < num2) || !(num6 < num2))
		{
			if (base.VirtualView.Orientation != 0 && num5 >= num2)
			{
				num3 += (0.0 - num7) * num5 * m_dpi * num;
			}
			if (base.VirtualView.Orientation != ScrollOrientation.Horizontal && num6 >= num2)
			{
				num4 += (0.0 - num8) * num6 * m_dpi * num;
			}
			base.PlatformView.ChangeView(num3, num4, null);
			base.VirtualView.IsScrolling = true;
			e.Handled = true;
		}
	}

	private void OnKeyUp(object sender, KeyRoutedEventArgs e)
	{
		if (e.Key == VirtualKey.Shift)
		{
			UpdateScrollOrientation();
		}
	}

	private void OnKeyDown(object sender, KeyRoutedEventArgs e)
	{
		if (e.Key == VirtualKey.Shift)
		{
			base.PlatformView.VerticalScrollMode = Microsoft.UI.Xaml.Controls.ScrollMode.Disabled;
			base.PlatformView.VerticalScrollBarVisibility = Microsoft.UI.Xaml.Controls.ScrollBarVisibility.Hidden;
		}
	}

	private void OnLoaded(object sender, RoutedEventArgs e)
	{
		if (sender is ScrollViewer scrollViewer)
		{
			m_dpi = scrollViewer.XamlRoot.RasterizationScale * 96.0;
		}
	}

	private void OnViewChanged(object? sender, ScrollViewerViewChangedEventArgs e)
	{
		if (!base.VirtualView.IsContentLayoutRequested)
		{
			ScrollChangedEventArgs scrolledEventArgs = new ScrollChangedEventArgs(base.PlatformView.HorizontalOffset, base.PlatformView.VerticalOffset, base.VirtualView.ScrollX, base.VirtualView.ScrollY);
			base.VirtualView.OnScrollChanged(scrolledEventArgs);
			if (!e.IsIntermediate)
			{
				base.VirtualView.SendScrollFinished();
				base.VirtualView.IsScrolling = false;
			}
			else
			{
				base.VirtualView.IsScrolling = true;
			}
		}
	}
}
