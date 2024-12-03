using System;
using System.Collections.Generic;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Input;

namespace Syncfusion.Maui.Core.Internals;

public class KeyboardDetector : IDisposable
{
	private List<IKeyboardListener> keyboardListeners;

	internal readonly View MauiView;

	private bool _disposed;

	private bool isViewListenerAdded;

	public KeyboardDetector(View mauiView)
	{
		MauiView = mauiView;
		keyboardListeners = new List<IKeyboardListener>();
		if (mauiView.Handler != null)
		{
			SubscribeNativeKeyEvents(mauiView);
			return;
		}
		mauiView.HandlerChanged += MauiView_HandlerChanged;
		mauiView.HandlerChanging += MauiView_HandlerChanging;
	}

	private void MauiView_HandlerChanged(object? sender, EventArgs e)
	{
		if (sender is View view && view.Handler != null)
		{
			SubscribeNativeKeyEvents(view);
		}
	}

	private void MauiView_HandlerChanging(object? sender, HandlerChangingEventArgs e)
	{
		UnsubscribeNativeKeyEvents(e.OldHandler);
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

	public void AddListener(IKeyboardListener listener)
	{
		keyboardListeners ??= new List<IKeyboardListener>();
		if (!keyboardListeners.Contains(listener))
		{
			keyboardListeners.Add(listener);
		}
		if (!isViewListenerAdded)
		{
			CreateNativeListener();
		}
		isViewListenerAdded = true;
	}

	public void ClearListeners()
	{
		keyboardListeners.Clear();
	}

	public bool HasListener()
	{
		List<IKeyboardListener> list = keyboardListeners;
		return list != null && list.Count > 0;
	}

	public void RemoveListener(IKeyboardListener listener)
	{
		if (listener != null)
		{
			if (keyboardListeners != null && keyboardListeners.Contains(listener))
			{
				keyboardListeners.Remove(listener);
			}
		}
	}

	internal void OnKeyAction(KeyEventArgs args, bool isKeyDown)
	{
		if (keyboardListeners.Count == 0)
		{
			return;
		}
		if (isKeyDown)
		{
			foreach (IKeyboardListener keyboardListener in keyboardListeners)
			{
				keyboardListener.OnKeyDown(args);
			}
			return;
		}
		foreach (IKeyboardListener keyboardListener2 in keyboardListeners)
		{
			keyboardListener2.OnKeyUp(args);
		}
	}

	private void Unsubscribe(View? mauiView)
	{
		if (mauiView != null)
		{
			UnsubscribeNativeKeyEvents(mauiView.Handler);
			mauiView.HandlerChanged -= MauiView_HandlerChanged;
			mauiView.HandlerChanging -= MauiView_HandlerChanging;
			mauiView = null;
		}
	}

	internal void SubscribeNativeKeyEvents(View? mauiView)
	{
		if (mauiView != null)
		{
			UIElement uIElement = mauiView.Handler?.PlatformView as UIElement;
			if (uIElement != null && keyboardListeners.Count > 0)
			{
				uIElement.KeyDown += PlatformView_KeyDown;
				uIElement.KeyUp += PlatformView_KeyUp;
			}
		}
	}

	internal void CreateNativeListener()
	{
		SubscribeNativeKeyEvents(MauiView);
	}

	private void HandleKeyActions(KeyRoutedEventArgs e, bool isKeyDown)
	{
		KeyboardKey keyboardKey = KeyboardListenerExtension.ConvertToKeyboardKey(e.Key);
		KeyEventArgs keyEventArgs = new KeyEventArgs(keyboardKey)
		{
			IsShiftKeyPressed = KeyboardListenerExtension.CheckShiftKeyPressed(),
			IsCtrlKeyPressed = KeyboardListenerExtension.CheckControlKeyPressed(),
			IsAltKeyPressed = KeyboardListenerExtension.CheckAltKeyPressed(),
			IsCommandKeyPressed = false
		};
		OnKeyAction(keyEventArgs, isKeyDown);
		e.Handled = keyEventArgs.Handled;
	}

	private void PlatformView_KeyUp(object sender, KeyRoutedEventArgs e)
	{
		HandleKeyActions(e, isKeyDown: false);
	}

	private void PlatformView_KeyDown(object sender, KeyRoutedEventArgs e)
	{
		HandleKeyActions(e, isKeyDown: true);
	}

	internal void UnsubscribeNativeKeyEvents(IElementHandler handler)
	{
		if (handler != null)
		{
			UIElement uIElement = handler.PlatformView as UIElement;
			if (uIElement != null)
			{
				uIElement.KeyDown -= PlatformView_KeyDown;
				uIElement.KeyUp -= PlatformView_KeyUp;
			}
		}
	}
}
