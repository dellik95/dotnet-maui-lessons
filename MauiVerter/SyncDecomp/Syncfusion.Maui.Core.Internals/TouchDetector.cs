using System;
using System.Collections.Generic;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Microsoft.UI.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Input;

namespace Syncfusion.Maui.Core.Internals;

public class TouchDetector : IDisposable
{
	private readonly List<ITouchListener> touchListeners;

	private bool _disposed;

	internal readonly View MauiView;

	public TouchDetector(View mauiView)
	{
		MauiView = mauiView;
		touchListeners = new List<ITouchListener>();
		if (mauiView.Handler != null)
		{
			SubscribeNativeTouchEvents(mauiView);
			return;
		}
		mauiView.HandlerChanged += MauiView_HandlerChanged;
		mauiView.HandlerChanging += MauiView_HandlerChanging;
	}

	private void MauiView_HandlerChanged(object? sender, EventArgs e)
	{
		if (sender is View view && view.Handler != null)
		{
			SubscribeNativeTouchEvents(view);
		}
	}

	private void MauiView_HandlerChanging(object? sender, HandlerChangingEventArgs e)
	{
		UnsubscribeNativeTouchEvents(e.OldHandler);
	}

	public void AddListener(ITouchListener listener)
	{
		if (!touchListeners.Contains(listener))
		{
			touchListeners.Add(listener);
		}
	}

	public void RemoveListener(ITouchListener listener)
	{
		if (touchListeners.Contains(listener))
		{
			touchListeners.Remove(listener);
		}
	}

	public bool HasListener()
	{
		return touchListeners.Count > 0;
	}

	public void ClearListeners()
	{
		touchListeners.Clear();
	}

	public void Dispose()
	{
		Dispose(disposing: true);
	}

	protected virtual void Dispose(bool disposing)
	{
		if (!_disposed)
		{
			_disposed = true;
			if (disposing)
			{
				ClearListeners();
				Unsubscribe(MauiView);
			}
		}
	}

	internal void OnTouchAction(long pointerId, PointerActions action, Point point)
	{
		PointerEventArgs eventArgs = new PointerEventArgs(pointerId, action, point);
		OnTouchAction(eventArgs);
	}

	internal void OnTouchAction(long pointerId, PointerActions action, PointerDeviceType deviceType, Point point)
	{
		PointerEventArgs eventArgs = new PointerEventArgs(pointerId, action, deviceType, point);
		OnTouchAction(eventArgs);
	}

	internal void OnTouchAction(PointerEventArgs eventArgs)
	{
		foreach (ITouchListener touchListener in touchListeners)
		{
			touchListener.OnTouch(eventArgs);
		}
	}

	internal bool OnScrollAction(long pointerId, Point origin, double direction, bool? handled = null)
	{
		ScrollEventArgs scrollEventArgs = new ScrollEventArgs(pointerId, origin, direction);
		if (handled.HasValue)
		{
			scrollEventArgs.Handled = handled.Value;
		}
		foreach (ITouchListener touchListener in touchListeners)
		{
			touchListener.OnScrollWheel(scrollEventArgs);
		}
		return scrollEventArgs.Handled;
	}

	private void Unsubscribe(View? mauiView)
	{
		if (mauiView != null)
		{
			UnsubscribeNativeTouchEvents(mauiView.Handler);
			mauiView.HandlerChanged -= MauiView_HandlerChanged;
			mauiView.HandlerChanging -= MauiView_HandlerChanging;
			mauiView = null;
		}
	}

	internal void SubscribeNativeTouchEvents(View? mauiView)
	{
		if (mauiView != null)
		{
			UIElement uIElement = mauiView.Handler?.PlatformView as UIElement;
			if (uIElement != null)
			{
				uIElement.PointerPressed += PlatformView_PointerPressed;
				uIElement.PointerMoved += PlatformView_PointerMoved;
				uIElement.PointerReleased += PlatformView_PointerReleased;
				uIElement.PointerCanceled += PlatformView_PointerCanceled;
				uIElement.PointerWheelChanged += PlatformView_PointerWheelChanged;
				uIElement.PointerEntered += PlatformView_PointerEntered;
				uIElement.PointerExited += PlatformView_PointerExited;
			}
		}
	}

	private void PlatformView_PointerExited(object sender, PointerRoutedEventArgs e)
	{
		if (MauiView.IsEnabled && !MauiView.InputTransparent)
		{
			UIElement uIElement = sender as UIElement;
			if (uIElement != null)
			{
				PointerPoint currentPoint = e.GetCurrentPoint(uIElement);
				PointerPointProperties properties = currentPoint.Properties;
				PointerEventArgs eventArgs = new PointerEventArgs(currentPoint.PointerId, PointerActions.Exited, GetDeviceType(currentPoint.PointerDeviceType), new Point(currentPoint.Position.X, currentPoint.Position.Y))
				{
					IsLeftButtonPressed = properties.IsLeftButtonPressed,
					IsRightButtonPressed = properties.IsRightButtonPressed
				};
				OnTouchAction(eventArgs);
			}
		}
	}

	private void PlatformView_PointerEntered(object sender, PointerRoutedEventArgs e)
	{
		if (MauiView.IsEnabled && !MauiView.InputTransparent)
		{
			UIElement uIElement = sender as UIElement;
			if (uIElement != null)
			{
				PointerPoint currentPoint = e.GetCurrentPoint(uIElement);
				PointerPointProperties properties = currentPoint.Properties;
				PointerEventArgs eventArgs = new PointerEventArgs(currentPoint.PointerId, PointerActions.Entered, GetDeviceType(currentPoint.PointerDeviceType), new Point(currentPoint.Position.X, currentPoint.Position.Y))
				{
					IsLeftButtonPressed = properties.IsLeftButtonPressed,
					IsRightButtonPressed = properties.IsRightButtonPressed
				};
				OnTouchAction(eventArgs);
			}
		}
	}

	private void PlatformView_PointerWheelChanged(object sender, PointerRoutedEventArgs e)
	{
		if (MauiView.IsEnabled && !MauiView.InputTransparent)
		{
			UIElement uIElement = sender as UIElement;
			if (uIElement != null)
			{
				PointerPoint currentPoint = e.GetCurrentPoint(uIElement);
				e.Handled = OnScrollAction(currentPoint.PointerId, new Point(currentPoint.Position.X, currentPoint.Position.Y), currentPoint.Properties.MouseWheelDelta, e.Handled);
			}
		}
	}

	private void PlatformView_PointerPressed(object sender, PointerRoutedEventArgs e)
	{
		if (!MauiView.IsEnabled || MauiView.InputTransparent)
		{
			return;
		}
		UIElement uIElement = sender as UIElement;
		if (uIElement != null)
		{
			uIElement.CapturePointer(e.Pointer);
			PointerPoint currentPoint = e.GetCurrentPoint(uIElement);
			PointerPointProperties properties = currentPoint.Properties;
			PointerEventArgs eventArgs = new PointerEventArgs(currentPoint.PointerId, PointerActions.Pressed, GetDeviceType(currentPoint.PointerDeviceType), new Point(currentPoint.Position.X, currentPoint.Position.Y))
			{
				IsLeftButtonPressed = properties.IsLeftButtonPressed,
				IsRightButtonPressed = properties.IsRightButtonPressed
			};
			OnTouchAction(eventArgs);
			if (touchListeners[0].IsTouchHandled)
			{
				uIElement.ManipulationMode = ManipulationModes.None;
			}
		}
	}

	private void PlatformView_PointerMoved(object sender, PointerRoutedEventArgs e)
	{
		if (MauiView.IsEnabled && !MauiView.InputTransparent)
		{
			UIElement uIElement = sender as UIElement;
			if (uIElement != null)
			{
				PointerPoint currentPoint = e.GetCurrentPoint(uIElement);
				OnTouchAction(currentPoint.PointerId, PointerActions.Moved, GetDeviceType(currentPoint.PointerDeviceType), new Point(currentPoint.Position.X, currentPoint.Position.Y));
			}
		}
	}

	private void PlatformView_PointerCanceled(object sender, PointerRoutedEventArgs e)
	{
		if (!MauiView.IsEnabled || MauiView.InputTransparent)
		{
			return;
		}
		UIElement uIElement = sender as UIElement;
		if (uIElement != null)
		{
			uIElement.ReleasePointerCapture(e.Pointer);
			PointerPoint currentPoint = e.GetCurrentPoint(uIElement);
			OnTouchAction(currentPoint.PointerId, PointerActions.Cancelled, GetDeviceType(currentPoint.PointerDeviceType), new Point(currentPoint.Position.X, currentPoint.Position.Y));
			if (uIElement.ManipulationMode == ManipulationModes.None)
			{
				uIElement.ManipulationMode = ManipulationModes.System;
			}
		}
	}

	private void PlatformView_PointerReleased(object sender, PointerRoutedEventArgs e)
	{
		if (!MauiView.IsEnabled || MauiView.InputTransparent)
		{
			return;
		}
		UIElement uIElement = sender as UIElement;
		if (uIElement != null)
		{
			uIElement.ReleasePointerCapture(e.Pointer);
			PointerPoint currentPoint = e.GetCurrentPoint(uIElement);
			OnTouchAction(currentPoint.PointerId, PointerActions.Released, GetDeviceType(currentPoint.PointerDeviceType), new Point(currentPoint.Position.X, currentPoint.Position.Y));
			if (uIElement.ManipulationMode == ManipulationModes.None)
			{
				uIElement.ManipulationMode = ManipulationModes.System;
			}
		}
	}

	private static PointerDeviceType GetDeviceType(Microsoft.UI.Input.PointerDeviceType deviceType)
	{
		return (deviceType == Microsoft.UI.Input.PointerDeviceType.Mouse) ? PointerDeviceType.Mouse : PointerDeviceType.Touch;
	}

	internal void UnsubscribeNativeTouchEvents(IElementHandler handler)
	{
		if (handler != null)
		{
			UIElement uIElement = handler.PlatformView as UIElement;
			if (uIElement != null)
			{
				uIElement.PointerPressed -= PlatformView_PointerPressed;
				uIElement.PointerMoved -= PlatformView_PointerMoved;
				uIElement.PointerReleased -= PlatformView_PointerReleased;
				uIElement.PointerCanceled -= PlatformView_PointerCanceled;
				uIElement.PointerWheelChanged -= PlatformView_PointerWheelChanged;
				uIElement.PointerEntered -= PlatformView_PointerEntered;
				uIElement.PointerExited -= PlatformView_PointerExited;
			}
		}
	}
}
