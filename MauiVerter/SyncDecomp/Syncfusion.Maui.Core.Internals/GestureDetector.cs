using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Microsoft.UI.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Input;
using Windows.Foundation;

namespace Syncfusion.Maui.Core.Internals;

public class GestureDetector : IDisposable
{
	private List<ITapGestureListener>? tapGestureListeners;

	private List<IDoubleTapGestureListener>? doubleTapGestureListeners;

	private List<IPinchGestureListener>? pinchGestureListeners;

	private List<IPanGestureListener>? panGestureListeners;

	private List<ILongPressGestureListener>? longPressGestureListeners;

	private bool _disposed;

	private bool isViewListenerAdded;

	internal readonly View MauiView;

	private bool wasPinchStarted;

	private bool wasPanStarted;

	private bool isPinching;

	private bool isPanning;

	private Windows.Foundation.Point touchMovePoint;

	private readonly Dictionary<uint, Windows.Foundation.Point> touchPointers = new Dictionary<uint, Windows.Foundation.Point>();

	private ManipulationModes defaultManipulationMode = ManipulationModes.System;

	private Microsoft.Maui.Graphics.Point panVelocity;

	public GestureDetector(View mauiView)
	{
		MauiView = mauiView;
		if (mauiView.Handler != null)
		{
			SubscribeNativeGestureEvents(mauiView);
			return;
		}
		mauiView.HandlerChanged += MauiView_HandlerChanged;
		mauiView.HandlerChanging += MauiView_HandlerChanging;
	}

	private void MauiView_HandlerChanged(object? sender, EventArgs e)
	{
		if (sender is View view && view.Handler != null)
		{
			SubscribeNativeGestureEvents(view);
		}
	}

	private void MauiView_HandlerChanging(object? sender, HandlerChangingEventArgs e)
	{
		UnsubscribeNativeGestureEvents(e.OldHandler);
	}

	public void AddListener(IGestureListener listener)
	{
		if (listener is IPanGestureListener item)
		{
			panGestureListeners ??= new List<IPanGestureListener>();
			panGestureListeners.Add(item);
		}
		if (listener is IPinchGestureListener item2)
		{
			pinchGestureListeners ??= new List<IPinchGestureListener>();
			pinchGestureListeners.Add(item2);
		}
		if (listener is ILongPressGestureListener item3)
		{
			longPressGestureListeners ??= new List<ILongPressGestureListener>();
			longPressGestureListeners.Add(item3);
		}
		if (listener is ITapGestureListener item4)
		{
			tapGestureListeners ??= new List<ITapGestureListener>();
			tapGestureListeners.Add(item4);
		}
		if (listener is IDoubleTapGestureListener item5)
		{
			doubleTapGestureListeners ??= new List<IDoubleTapGestureListener>();
			doubleTapGestureListeners.Add(item5);
		}
		if (!isViewListenerAdded)
		{
			CreateNativeListener();
		}
		isViewListenerAdded = true;
	}

	public void RemoveListener(IGestureListener listener)
	{
		if (listener is IPanGestureListener item && panGestureListeners != null && panGestureListeners.Contains(item))
		{
			panGestureListeners.Remove(item);
		}
		if (listener is IPinchGestureListener item2 && pinchGestureListeners != null && pinchGestureListeners.Contains(item2))
		{
			pinchGestureListeners.Remove(item2);
		}
		if (listener is ILongPressGestureListener item3 && longPressGestureListeners != null && longPressGestureListeners.Contains(item3))
		{
			longPressGestureListeners.Remove(item3);
		}
		if (listener is ITapGestureListener item4 && tapGestureListeners != null && tapGestureListeners.Contains(item4))
		{
			tapGestureListeners.Remove(item4);
		}
		if (listener is IDoubleTapGestureListener item5 && doubleTapGestureListeners != null && doubleTapGestureListeners.Contains(item5))
		{
			doubleTapGestureListeners.Remove(item5);
		}
	}

	public void ClearListeners()
	{
		tapGestureListeners?.Clear();
		doubleTapGestureListeners?.Clear();
		pinchGestureListeners?.Clear();
		panGestureListeners?.Clear();
		longPressGestureListeners?.Clear();
	}

	public bool HasListener()
	{
		List<ITapGestureListener>? list = tapGestureListeners;
		int result;
		if (list == null || list.Count <= 0)
		{
			List<IDoubleTapGestureListener>? list2 = doubleTapGestureListeners;
			if (list2 == null || list2.Count <= 0)
			{
				List<ILongPressGestureListener>? list3 = longPressGestureListeners;
				if (list3 == null || list3.Count <= 0)
				{
					List<IPinchGestureListener>? list4 = pinchGestureListeners;
					if (list4 == null || list4.Count <= 0)
					{
						List<IPanGestureListener>? list5 = panGestureListeners;
						result = ((list5 != null && list5.Count > 0) ? 1 : 0);
						goto IL_0075;
					}
				}
			}
		}
		result = 1;
		goto IL_0075;
		IL_0075:
		return (byte)result != 0;
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
				isViewListenerAdded = false;
				ClearListeners();
				Unsubscribe(MauiView);
			}
		}
	}

	internal virtual void OnPinch(GestureStatus state, Microsoft.Maui.Graphics.Point point, double pinchAngle, float scale)
	{
		if (pinchGestureListeners == null)
		{
			return;
		}
		PinchEventArgs e = new PinchEventArgs(state, point, pinchAngle, scale);
		foreach (IPinchGestureListener pinchGestureListener in pinchGestureListeners)
		{
			pinchGestureListener.OnPinch(e);
		}
	}

	internal virtual void OnScroll(GestureStatus state, Microsoft.Maui.Graphics.Point startPoint, Microsoft.Maui.Graphics.Point scalePoint, Microsoft.Maui.Graphics.Point velocity)
	{
		if (panGestureListeners == null)
		{
			return;
		}
		PanEventArgs e = new PanEventArgs(state, startPoint, scalePoint, velocity);
		foreach (IPanGestureListener panGestureListener in panGestureListeners)
		{
			panGestureListener.OnPan(e);
		}
	}

	internal virtual void OnTapped(Microsoft.Maui.Graphics.Point touchPoint, int tapCount)
	{
		TapEventArgs e;
		if (tapCount == 1 && tapGestureListeners != null)
		{
			e = new TapEventArgs(touchPoint, tapCount);
			foreach (ITapGestureListener tapGestureListener in tapGestureListeners)
			{
				tapGestureListener.OnTap(MauiView, e);
				tapGestureListener.OnTap(e);
			}
		}
		if (tapCount != 2 || doubleTapGestureListeners == null)
		{
			return;
		}
		e = new TapEventArgs(touchPoint, tapCount);
		foreach (IDoubleTapGestureListener doubleTapGestureListener in doubleTapGestureListeners)
		{
			doubleTapGestureListener.OnDoubleTap(e);
		}
	}

	internal virtual void OnLongPress(Microsoft.Maui.Graphics.Point touchPoint)
	{
		if (longPressGestureListeners == null)
		{
			return;
		}
		LongPressEventArgs e = new LongPressEventArgs(touchPoint);
		foreach (ILongPressGestureListener longPressGestureListener in longPressGestureListeners)
		{
			longPressGestureListener.OnLongPress(e);
		}
	}

	private void Unsubscribe(View? mauiView)
	{
		if (mauiView != null)
		{
			UnsubscribeNativeGestureEvents(mauiView.Handler);
			mauiView.HandlerChanged -= MauiView_HandlerChanged;
			mauiView.HandlerChanging -= MauiView_HandlerChanging;
			mauiView = null;
		}
	}

	internal void SubscribeNativeGestureEvents(View? mauiView)
	{
		if (mauiView == null)
		{
			return;
		}
		UIElement uIElement = mauiView.Handler?.PlatformView as UIElement;
		if (uIElement != null)
		{
			defaultManipulationMode = uIElement.ManipulationMode;
			if (tapGestureListeners != null && tapGestureListeners.Count > 0)
			{
				uIElement.Tapped += PlatformView_Tapped;
			}
			if (doubleTapGestureListeners != null && doubleTapGestureListeners.Count > 0)
			{
				uIElement.DoubleTapped += PlatformView_DoubleTapped;
			}
			if (longPressGestureListeners != null && longPressGestureListeners.Count > 0)
			{
				uIElement.Holding += PlatformView_Holding;
			}
			if ((panGestureListeners != null && panGestureListeners.Count > 0) || (pinchGestureListeners != null && pinchGestureListeners.Count > 0))
			{
				uIElement.PointerPressed += PlatformView_PointerPressed;
				uIElement.PointerMoved += PlatformView_PointerMoved;
				uIElement.PointerReleased += PlatformView_PointerReleased;
				uIElement.PointerCanceled += PlatformView_PointerCanceled;
				uIElement.PointerExited += PlatformView_PointerExited;
				uIElement.PointerCaptureLost += PlatformView_PointerCaptureLost;
			}
			if (panGestureListeners != null && panGestureListeners.Count > 0)
			{
				uIElement.ManipulationInertiaStarting += OnNativeViewManipulationInertiaStarting;
			}
		}
	}

	internal void CreateNativeListener()
	{
		SubscribeNativeGestureEvents(MauiView);
	}

	private void AddPointer(Pointer pointer, Windows.Foundation.Point position)
	{
		if (!touchPointers.ContainsKey(pointer.PointerId))
		{
			touchPointers.Add(pointer.PointerId, position);
		}
	}

	private void RemovePointer(Pointer pointer)
	{
		if (touchPointers.ContainsKey(pointer.PointerId))
		{
			touchPointers.Remove(pointer.PointerId);
		}
	}

	private void PlatformView_PointerCaptureLost(object sender, PointerRoutedEventArgs e)
	{
		OnPointerEnd(sender, e, isReleasedProperly: false);
	}

	private void OnPointerEnd(object sender, PointerRoutedEventArgs e, bool isReleasedProperly)
	{
		RemovePointer(e.Pointer);
		PinchCompleted(isReleasedProperly);
		PanCompleted(isReleasedProperly);
		if (sender is UIElement uIElement)
		{
			uIElement.ManipulationMode = defaultManipulationMode;
		}
	}

	private void PlatformView_PointerMoved(object sender, PointerRoutedEventArgs e)
	{
		if (MauiView.IsEnabled && !MauiView.InputTransparent)
		{
			touchMovePoint = e.GetCurrentPoint(sender as UIElement).Position;
			OnPinch(e.Pointer.PointerId, touchMovePoint);
			OnPan(e.Pointer.PointerId, touchMovePoint);
		}
	}

	private void PlatformView_PointerExited(object sender, PointerRoutedEventArgs e)
	{
		if (MauiView.IsEnabled && !MauiView.InputTransparent)
		{
			OnPointerEnd(sender, e, isReleasedProperly: true);
		}
	}

	private void PlatformView_PointerCanceled(object sender, PointerRoutedEventArgs e)
	{
		if (MauiView.IsEnabled && !MauiView.InputTransparent)
		{
			OnPointerEnd(sender, e, isReleasedProperly: false);
		}
	}

	private void PlatformView_PointerReleased(object sender, PointerRoutedEventArgs e)
	{
		if (MauiView.IsEnabled && !MauiView.InputTransparent)
		{
			OnPointerEnd(sender, e, isReleasedProperly: true);
		}
	}

	private void PlatformView_PointerPressed(object sender, PointerRoutedEventArgs e)
	{
		if (!MauiView.IsEnabled || MauiView.InputTransparent)
		{
			return;
		}
		AddPointer(e.Pointer, e.GetCurrentPoint(sender as UIElement).Position);
		UIElement uIElement = sender as UIElement;
		if (uIElement != null)
		{
			if (touchPointers.Count == 1 && panGestureListeners != null && panGestureListeners.Count > 0)
			{
				uIElement.ManipulationMode = ManipulationModes.TranslateX | ManipulationModes.TranslateY | ManipulationModes.TranslateInertia;
			}
			if (touchPointers.Count == 2 && pinchGestureListeners != null && pinchGestureListeners.Count > 0)
			{
				uIElement.ManipulationMode = ManipulationModes.Scale;
			}
			if (touchPointers.Count > 2)
			{
				uIElement.ManipulationMode = defaultManipulationMode;
			}
		}
	}

	private void OnNativeViewManipulationInertiaStarting(object sender, ManipulationInertiaStartingRoutedEventArgs e)
	{
		if (panGestureListeners != null && panGestureListeners.Count != 0)
		{
			panVelocity = new Microsoft.Maui.Graphics.Point(e.Velocities.Linear.X * 1000.0, e.Velocities.Linear.Y * 1000.0);
		}
	}

	private void OnPan(uint pointerID, Windows.Foundation.Point position)
	{
		if (panGestureListeners == null || panGestureListeners.Count == 0 || touchPointers.Count != 1 || !touchPointers.ContainsKey(pointerID))
		{
			return;
		}
		Microsoft.Maui.Graphics.Point point = new Microsoft.Maui.Graphics.Point(touchPointers[pointerID].X, touchPointers[pointerID].Y);
		touchPointers[pointerID] = position;
		Microsoft.Maui.Graphics.Point touchPoint = new Microsoft.Maui.Graphics.Point(touchMovePoint.X, touchMovePoint.Y);
		Microsoft.Maui.Graphics.Point translatePoint = new Microsoft.Maui.Graphics.Point(touchPoint.X - point.X, touchPoint.Y - point.Y);
		GestureStatus status = GestureStatus.Started;
		isPanning = true;
		foreach (IPanGestureListener panGestureListener in panGestureListeners)
		{
			if (wasPanStarted)
			{
				status = GestureStatus.Running;
			}
			PanEventArgs e = new PanEventArgs(status, touchPoint, translatePoint, Microsoft.Maui.Graphics.Point.Zero);
			panGestureListener.OnPan(e);
		}
		wasPanStarted = true;
	}

	private void OnPinch(uint pointerID, Windows.Foundation.Point position)
	{
		if (pinchGestureListeners != null && pinchGestureListeners.Count != 0 && touchPointers.Count == 2 && touchPointers.ContainsKey(pointerID))
		{
			if (wasPanStarted)
			{
				PanCompleted(isCompleted: false);
			}
			List<uint> list = touchPointers.Keys.ToList();
			Windows.Foundation.Point point = touchPointers[list[0]];
			Windows.Foundation.Point point2 = touchPointers[list[1]];
			touchPointers[pointerID] = position;
			Windows.Foundation.Point point3 = touchPointers[list[0]];
			Windows.Foundation.Point point4 = touchPointers[list[1]];
			list.Clear();
			Microsoft.Maui.Graphics.Point point5 = new Microsoft.Maui.Graphics.Point(point.X + point2.X, point.Y + point2.Y);
			double distance = MathUtils.GetDistance(point.X, point2.X, point.Y, point2.Y);
			if (!wasPinchStarted)
			{
				PerformPinch(new Microsoft.Maui.Graphics.Point(point5.X / 2.0, point5.Y / 2.0), Microsoft.Maui.Graphics.Point.Zero, 1f);
			}
			Microsoft.Maui.Graphics.Point point6 = new Microsoft.Maui.Graphics.Point(point3.X + point4.X, point3.Y + point4.Y);
			double distance2 = MathUtils.GetDistance(point3.X, point4.X, point3.Y, point4.Y);
			Microsoft.Maui.Graphics.Point scalePoint = new Microsoft.Maui.Graphics.Point(point6.X / 2.0, point6.Y / 2.0);
			Microsoft.Maui.Graphics.Point translationPoint = new Microsoft.Maui.Graphics.Point(point6.X - point5.X, point6.Y - point5.Y);
			float scale = (float)(distance2 / distance);
			PerformPinch(scalePoint, translationPoint, scale);
		}
	}

	private void PerformPinch(Microsoft.Maui.Graphics.Point scalePoint, Microsoft.Maui.Graphics.Point translationPoint, float scale)
	{
		if (pinchGestureListeners == null)
		{
			return;
		}
		GestureStatus status = GestureStatus.Started;
		double angle = MathUtils.GetAngle(scalePoint.X, translationPoint.X, scalePoint.Y, translationPoint.Y);
		isPinching = true;
		foreach (IPinchGestureListener pinchGestureListener in pinchGestureListeners)
		{
			if (wasPinchStarted)
			{
				status = GestureStatus.Running;
			}
			PinchEventArgs e = new PinchEventArgs(status, scalePoint, angle, scale);
			pinchGestureListener.OnPinch(e);
		}
		wasPinchStarted = true;
	}

	private void PinchCompleted(bool isCompleted, Microsoft.Maui.Graphics.Point scalePoint = default(Microsoft.Maui.Graphics.Point), double angle = double.NaN, float scale = 1f)
	{
		if (pinchGestureListeners == null || pinchGestureListeners.Count == 0 || !isPinching)
		{
			return;
		}
		foreach (IPinchGestureListener pinchGestureListener in pinchGestureListeners)
		{
			PinchEventArgs e = new PinchEventArgs(isCompleted ? GestureStatus.Completed : GestureStatus.Canceled, scalePoint, angle, scale);
			pinchGestureListener.OnPinch(e);
		}
		isPinching = false;
		touchPointers.Clear();
		wasPinchStarted = false;
	}

	private void PanCompleted(bool isCompleted)
	{
		if (panGestureListeners == null || panGestureListeners.Count == 0 || !isPanning)
		{
			return;
		}
		foreach (IPanGestureListener panGestureListener in panGestureListeners)
		{
			PanEventArgs e = new PanEventArgs(isCompleted ? GestureStatus.Completed : GestureStatus.Canceled, new Microsoft.Maui.Graphics.Point(touchMovePoint.X, touchMovePoint.Y), Microsoft.Maui.Graphics.Point.Zero, panVelocity);
			panGestureListener.OnPan(e);
		}
		isPanning = false;
		wasPanStarted = false;
		panVelocity = Microsoft.Maui.Graphics.Point.Zero;
	}

	private void PlatformView_Holding(object sender, HoldingRoutedEventArgs e)
	{
		if (!MauiView.IsEnabled || MauiView.InputTransparent || longPressGestureListeners == null || longPressGestureListeners.Count == 0)
		{
			return;
		}
		Windows.Foundation.Point position = e.GetPosition(sender as UIElement);
		foreach (ILongPressGestureListener longPressGestureListener in longPressGestureListeners)
		{
			if (e.HoldingState == HoldingState.Started)
			{
				longPressGestureListener.OnLongPress(new LongPressEventArgs(new Microsoft.Maui.Graphics.Point(position.X, position.Y)));
			}
		}
		if (longPressGestureListeners[0].IsTouchHandled)
		{
			e.Handled = true;
		}
	}

	private void PlatformView_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
	{
		if (!MauiView.IsEnabled || MauiView.InputTransparent || doubleTapGestureListeners == null || doubleTapGestureListeners.Count == 0)
		{
			return;
		}
		Windows.Foundation.Point position = e.GetPosition(sender as UIElement);
		foreach (IDoubleTapGestureListener doubleTapGestureListener in doubleTapGestureListeners)
		{
			doubleTapGestureListener.OnDoubleTap(new TapEventArgs(new Microsoft.Maui.Graphics.Point(position.X, position.Y), 2));
		}
		if (doubleTapGestureListeners[0].IsTouchHandled)
		{
			e.Handled = true;
		}
	}

	private void PlatformView_Tapped(object sender, TappedRoutedEventArgs e)
	{
		if (!MauiView.IsEnabled || MauiView.InputTransparent || tapGestureListeners == null || tapGestureListeners.Count == 0)
		{
			return;
		}
		Windows.Foundation.Point position = e.GetPosition(sender as UIElement);
		foreach (ITapGestureListener tapGestureListener in tapGestureListeners)
		{
			tapGestureListener.OnTap(MauiView, new TapEventArgs(new Microsoft.Maui.Graphics.Point(position.X, position.Y), 1));
			tapGestureListener.OnTap(new TapEventArgs(new Microsoft.Maui.Graphics.Point(position.X, position.Y), 1));
		}
		if (tapGestureListeners[0].IsTouchHandled)
		{
			e.Handled = true;
		}
	}

	internal void UnsubscribeNativeGestureEvents(IElementHandler handler)
	{
		if (handler != null)
		{
			UIElement uIElement = handler.PlatformView as UIElement;
			if (uIElement != null)
			{
				uIElement.ManipulationMode = defaultManipulationMode;
				uIElement.Tapped -= PlatformView_Tapped;
				uIElement.DoubleTapped -= PlatformView_DoubleTapped;
				uIElement.Holding -= PlatformView_Holding;
				uIElement.PointerPressed -= PlatformView_PointerPressed;
				uIElement.PointerMoved -= PlatformView_PointerMoved;
				uIElement.PointerReleased -= PlatformView_PointerReleased;
				uIElement.PointerCanceled -= PlatformView_PointerCanceled;
				uIElement.PointerExited -= PlatformView_PointerExited;
				uIElement.PointerCaptureLost -= PlatformView_PointerCaptureLost;
				uIElement.ManipulationInertiaStarting -= OnNativeViewManipulationInertiaStarting;
			}
		}
		touchPointers.Clear();
	}
}
