using System;
using Microsoft.Maui.Controls;

namespace Syncfusion.Maui.Core;

internal class SfDropdownView : ContentView
{
	private View? popupListView;

	public static readonly BindableProperty AnchorViewProperty = BindableProperty.Create(nameof(AnchorView), typeof(View), typeof(SfDropdownView), null, BindingMode.OneWay, null, OnAnchorViewChanged);

	public static readonly BindableProperty PopupHeightProperty = BindableProperty.Create(nameof(PopupHeight), typeof(double), typeof(SfDropdownView), null, BindingMode.OneWay, null, OnPopupHeightChanged);

	public static readonly BindableProperty PopupWidthProperty = BindableProperty.Create(nameof(PopupWidth), typeof(double), typeof(SfDropdownView), null, BindingMode.OneWay, null, OnPopupWidthChanged);

	public static readonly BindableProperty PopupXProperty = BindableProperty.Create(nameof(PopupX), typeof(int), typeof(SfDropdownView), 0, BindingMode.OneWay, null, OnPopupXChanged);

	public static readonly BindableProperty PopupYProperty = BindableProperty.Create(nameof(PopupY), typeof(int), typeof(SfDropdownView), 0, BindingMode.OneWay, null, OnPopupYChanged);

	public static readonly BindableProperty IsOpenProperty = BindableProperty.Create(nameof(IsOpen), typeof(bool), typeof(SfDropdownView), false, BindingMode.OneWay, null, OnPopupStateChanged);

	internal static readonly BindableProperty IsListViewClickedProperty = BindableProperty.Create(nameof(IsListViewClicked), typeof(bool), typeof(SfDropdownView), false);

	internal View AnchorView
	{
		get
		{
			return (View)GetValue(AnchorViewProperty);
		}
		set
		{
			SetValue(AnchorViewProperty, value);
		}
	}

	internal double PopupHeight
	{
		get
		{
			return (double)GetValue(PopupHeightProperty);
		}
		set
		{
			SetValue(PopupHeightProperty, value);
		}
	}

	internal double PopupWidth
	{
		get
		{
			return (double)GetValue(PopupWidthProperty);
		}
		set
		{
			SetValue(PopupWidthProperty, value);
		}
	}

	internal int PopupX
	{
		get
		{
			return (int)GetValue(PopupXProperty);
		}
		set
		{
			SetValue(PopupXProperty, value);
		}
	}

	internal int PopupY
	{
		get
		{
			return (int)GetValue(PopupYProperty);
		}
		set
		{
			SetValue(PopupYProperty, value);
		}
	}

	internal bool IsOpen
	{
		get
		{
			return (bool)GetValue(IsOpenProperty);
		}
		set
		{
			SetValue(IsOpenProperty, value);
		}
	}

	internal bool IsDropDownCicked { get; set; }

	internal bool IsListViewClicked
	{
		get
		{
			return (bool)GetValue(IsListViewClickedProperty);
		}
		set
		{
			SetValue(IsListViewClickedProperty, value);
		}
	}

	internal event EventHandler<EventArgs>? PopupClosed;

	internal event EventHandler<EventArgs>? PopupOpened;

	private static void OnPopupHeightChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (bindable is SfDropdownView sfDropdownView && sfDropdownView.Handler is SfDropdownViewHandler sfDropdownViewHandler)
		{
			sfDropdownViewHandler?.UpdatePopupHeight((double)newValue);
		}
	}

	private static void OnPopupWidthChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (bindable is SfDropdownView sfDropdownView && sfDropdownView.Handler is SfDropdownViewHandler sfDropdownViewHandler)
		{
			sfDropdownViewHandler?.UpdatePopupWidth((double)newValue);
		}
	}

	private static void OnPopupXChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (bindable is SfDropdownView sfDropdownView && sfDropdownView.Handler is SfDropdownViewHandler sfDropdownViewHandler)
		{
			sfDropdownViewHandler?.UpdatePopupX((int)newValue);
		}
	}

	private static void OnPopupYChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (bindable is SfDropdownView sfDropdownView && sfDropdownView.Handler is SfDropdownViewHandler sfDropdownViewHandler)
		{
			sfDropdownViewHandler?.UpdatePopupY((int)newValue);
		}
	}

	private static void OnAnchorViewChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (bindable is SfDropdownView sfDropdownView && sfDropdownView.Handler is SfDropdownViewHandler sfDropdownViewHandler)
		{
			sfDropdownViewHandler?.UpdateAnchorView((View)newValue);
		}
	}

	private static void OnPopupStateChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (!(bool)newValue)
		{
			(bindable as SfDropdownView)?.HidePopup();
		}
		else
		{
			(bindable as SfDropdownView)?.ShowPopup();
		}
	}

	public SfDropdownView()
	{
		base.IsClippedToBounds = true;
	}

	internal void HidePopup()
	{
		if (base.Handler is SfDropdownViewHandler sfDropdownViewHandler)
		{
			sfDropdownViewHandler?.HidePopup();
		}
		RaisePopupClosedEvent(EventArgs.Empty);
	}

	internal void UpdatePopupContent(View listView)
	{
		popupListView = listView;
		if (base.Handler is SfDropdownViewHandler sfDropdownViewHandler)
		{
			sfDropdownViewHandler?.UpdatePopupContent(listView);
		}
	}

	internal void ShowPopup()
	{
		if (base.Handler is SfDropdownViewHandler sfDropdownViewHandler)
		{
			sfDropdownViewHandler?.ShowPopup();
		}
		RaisePopupOpenedEvent(EventArgs.Empty);
	}

	internal void RaisePopupClosedEvent(EventArgs args)
	{
		this.PopupClosed?.Invoke(this, args);
	}

	internal void RaisePopupOpenedEvent(EventArgs args)
	{
		this.PopupOpened?.Invoke(this, args);
	}

	protected override void OnHandlerChanged()
	{
		base.OnHandlerChanged();
		if (base.Handler != null && base.Handler is SfDropdownViewHandler sfDropdownViewHandler)
		{
			sfDropdownViewHandler.UpdatePopupHeight(PopupHeight);
			sfDropdownViewHandler.UpdatePopupWidth(PopupWidth);
			sfDropdownViewHandler.UpdatePopupX(PopupX);
			sfDropdownViewHandler.UpdatePopupY(PopupY);
			if (popupListView != null)
			{
				sfDropdownViewHandler.UpdatePopupContent(popupListView);
			}
			sfDropdownViewHandler.UpdateAnchorView(AnchorView);
			if (IsOpen)
			{
				sfDropdownViewHandler.ShowPopup();
			}
		}
	}
}
