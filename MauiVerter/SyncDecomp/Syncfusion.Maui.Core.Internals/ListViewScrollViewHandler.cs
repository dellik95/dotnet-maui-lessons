using System;
using System.ComponentModel;
using Microsoft.Maui;
using Microsoft.Maui.Handlers;
using Microsoft.UI.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;

namespace Syncfusion.Maui.Core.Internals;

internal class ListViewScrollViewHandler : ScrollViewHandler
{
	private FrameworkElement? content;

	private double previousOffset = 0.0;

	private ListViewScrollViewExt? ScrollView => base.VirtualView as ListViewScrollViewExt;

	public override void SetVirtualView(IView view)
	{
		base.SetVirtualView(view);
		if (ScrollView != null && ScrollView.ScrollingEnabled)
		{
			ScrollView.PropertyChanged += OnListViewScrollViewPropertyChanged;
			if (base.PlatformView.Content != null)
			{
				content = base.PlatformView.Content as FrameworkElement;
				content.ManipulationMode = ManipulationModes.All;
				WireEvents();
			}
		}
	}

	protected override void DisconnectHandler(ScrollViewer nativeView)
	{
		if (ScrollView != null && ScrollView.ScrollingEnabled)
		{
			ScrollView.PropertyChanged -= OnListViewScrollViewPropertyChanged;
			UnWireEvents();
		}
		base.DisconnectHandler(nativeView);
	}

	private void ScrollViewer_ViewChanging(object? sender, ScrollViewerViewChangingEventArgs e)
	{
		if (ScrollView.IsProgrammaticScrolling)
		{
			ScrollView.SetScrollState("Programmatic");
		}
		ScrollView.ScrollEndPosition = ((ScrollView.Orientation == ScrollOrientation.Vertical) ? e.FinalView.VerticalOffset : e.FinalView.HorizontalOffset);
	}

	private void ScrollViewer_ViewChanged(object? sender, ScrollViewerViewChangedEventArgs e)
	{
		ScrollView.SetIsHorizontalRTLViewLoaded(IsHorizontalRTLViewLoaded: true, 0.0);
		if (ScrollView.GetScrollState() == "Programmatic")
		{
			if (ScrollView.Orientation == ScrollOrientation.Vertical && ScrollView.ScrollEndPosition == base.PlatformView.VerticalOffset)
			{
				ScrollView.IsProgrammaticScrolling = false;
				ScrollView.SetScrollState("Idle");
			}
			else if (ScrollView.Orientation == ScrollOrientation.Horizontal && ScrollView.ScrollEndPosition == base.PlatformView.HorizontalOffset)
			{
				ScrollView.IsProgrammaticScrolling = false;
				ScrollView.SetScrollState("Idle");
			}
		}
	}

	private void Content_PointerWheelChanged(object sender, PointerRoutedEventArgs e)
	{
		bool flag = ScrollView.Orientation == ScrollOrientation.Vertical;
		bool flag2 = ScrollView.Height == ScrollView.GetContainerTotalExtent();
		bool flag3 = ScrollView.Width == ScrollView.GetContainerTotalExtent();
		if ((flag2 && flag) || (flag3 && !flag))
		{
			return;
		}
		int mouseWheelDelta = e.GetCurrentPoint(content).Properties.MouseWheelDelta;
		double num = (flag ? base.PlatformView.VerticalOffset : base.PlatformView.HorizontalOffset);
		double num2 = num - (double)mouseWheelDelta;
		double num3 = ScrollView.GetContainerTotalExtent() - (flag ? ScrollView.Height : ScrollView.Width);
		num2 = ((num2 < 0.0) ? 0.0 : num2);
		num2 = ((num2 > num3) ? num3 : num2);
		if (num2 == num)
		{
			e.Handled = true;
			return;
		}
		ScrollView.SetScrollState("Programmatic");
		ScrollView.ProcessAutoOnScroll();
		if (ScrollView.Orientation == ScrollOrientation.Vertical)
		{
			base.PlatformView.ScrollToVerticalOffset(num2);
		}
		else
		{
			base.PlatformView.ScrollToHorizontalOffset(num2);
		}
		ScrollView.ScrollEndPosition = num2;
		e.Handled = true;
	}

	private void Content_ManipulationInertiaStarting(object sender, ManipulationInertiaStartingRoutedEventArgs e)
	{
		e.Handled = true;
	}

	private void Content_ManipulationStarting(object sender, ManipulationStartingRoutedEventArgs e)
	{
		previousOffset = ((ScrollView.Orientation == ScrollOrientation.Vertical) ? base.PlatformView.VerticalOffset : base.PlatformView.HorizontalOffset);
	}

	private void Content_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
	{
		bool flag = ScrollView.Orientation == ScrollOrientation.Vertical;
		if (e.PointerDeviceType == Microsoft.UI.Input.PointerDeviceType.Mouse)
		{
			e.Handled = true;
			return;
		}
		if (ScrollView.IsSwipingEnabled())
		{
			if (ScrollView.IsListViewInSwiping())
			{
				return;
			}
			double value = (flag ? e.Cumulative.Translation.X : e.Cumulative.Translation.Y);
			double value2 = (flag ? e.Cumulative.Translation.Y : e.Cumulative.Translation.X);
			if (Math.Abs(value2) <= Math.Abs(value))
			{
				return;
			}
		}
		if (e.IsInertial && Math.Abs(e.Velocities.Linear.X) > 0.2)
		{
			ScrollView.SetScrollState("Fling");
		}
		else if (ScrollView.GetScrollState() != "Fling")
		{
			ScrollView.SetScrollState("Dragging");
		}
		if (flag)
		{
			double offset = previousOffset - ((base.PlatformView.FlowDirection == Microsoft.UI.Xaml.FlowDirection.RightToLeft && !flag) ? (e.Cumulative.Translation.Y * -1.0) : e.Cumulative.Translation.Y);
			base.PlatformView.ScrollToVerticalOffset(offset);
		}
		else
		{
			double offset2 = previousOffset - ((base.PlatformView.FlowDirection == Microsoft.UI.Xaml.FlowDirection.RightToLeft && !flag) ? (e.Cumulative.Translation.X * -1.0) : e.Cumulative.Translation.X);
			base.PlatformView.ScrollToHorizontalOffset(offset2);
		}
	}

	private void Content_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
	{
		ScrollView.SetScrollState("Idle");
	}

	private void OnListViewScrollViewPropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		if (e.PropertyName == "ScrollPosition")
		{
			if (ScrollView.Orientation == ScrollOrientation.Vertical)
			{
				base.PlatformView.ChangeView(0.0, ScrollView.ScrollPosition, null, ScrollView.DisableAnimation);
			}
			else
			{
				base.PlatformView.ChangeView(ScrollView.ScrollPosition, 0.0, null, ScrollView.DisableAnimation);
			}
		}
	}

	private void WireEvents()
	{
		if (content != null)
		{
			base.PlatformView.ViewChanging += ScrollViewer_ViewChanging;
			base.PlatformView.ViewChanged += ScrollViewer_ViewChanged;
			content.ManipulationStarting += Content_ManipulationStarting;
			content.ManipulationDelta += Content_ManipulationDelta;
			content.ManipulationCompleted += Content_ManipulationCompleted;
			content.PointerWheelChanged += Content_PointerWheelChanged;
			content.ManipulationInertiaStarting += Content_ManipulationInertiaStarting;
		}
	}

	private void UnWireEvents()
	{
		if (content != null)
		{
			base.PlatformView.ViewChanging -= ScrollViewer_ViewChanging;
			base.PlatformView.ViewChanged -= ScrollViewer_ViewChanged;
			content.ManipulationStarting -= Content_ManipulationStarting;
			content.ManipulationDelta -= Content_ManipulationDelta;
			content.ManipulationCompleted -= Content_ManipulationCompleted;
			content.PointerWheelChanged -= Content_PointerWheelChanged;
			content.ManipulationInertiaStarting -= Content_ManipulationInertiaStarting;
		}
	}
}
