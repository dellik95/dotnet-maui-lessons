using System;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Input;
using Syncfusion.Maui.Core.Platform;

namespace Syncfusion.Maui.Core;

public class SfDropdownViewHandler : ContentViewHandler
{
	private DropdownViewExt? popupViewExt;

	private SfDropdownView? popupView;

	public SfDropdownViewHandler()
	{
	}

	public SfDropdownViewHandler(PropertyMapper mapper)
		: base(mapper)
	{
	}

	public SfDropdownViewHandler(IPropertyMapper? mapper = null)
		: base(mapper)
	{
	}

	protected SfDropdownViewHandler(IPropertyMapper mapper, CommandMapper? commandMapper = null)
		: base(mapper, commandMapper)
	{
	}

	protected override ContentPanel CreatePlatformView()
	{
		if (base.VirtualView == null)
		{
			throw new InvalidOperationException("VirtualView must be set to create a LayoutViewGroup");
		}
		popupView = (SfDropdownView)base.VirtualView;
		popupViewExt = new DropdownViewExt();
		if (popupViewExt != null)
		{
			popupViewExt.Loaded += PopupViewExt_Loaded;
		}
		if (popupViewExt?.Popup != null)
		{
			popupViewExt.Popup.Closed += Popup_Closed;
		}
		if (popupViewExt != null)
		{
			return popupViewExt;
		}
		return new DropdownViewExt();
	}

	private void PopupViewExt_Loaded(object sender, RoutedEventArgs e)
	{
		if (popupViewExt?.parentGrid != null && popupViewExt.parentGrid.Children[0] != null)
		{
			popupViewExt.parentGrid.Children[0].PointerPressed += SfPopupViewHandler_PointerPressed;
		}
	}

	private void SfPopupViewHandler_PointerPressed(object sender, PointerRoutedEventArgs e)
	{
		if (popupView != null)
		{
			popupView.IsListViewClicked = true;
		}
	}

	protected override void DisconnectHandler(ContentPanel platformView)
	{
		base.DisconnectHandler(platformView);
		if (popupViewExt != null)
		{
			if (popupViewExt.Popup != null)
			{
				popupViewExt.Popup.Closed -= Popup_Closed;
			}
			popupViewExt.Dispose();
			popupViewExt = null;
		}
	}

	private void Popup_Closed(object? sender, object e)
	{
		if (popupView != null)
		{
			popupView.IsOpen = false;
		}
	}

	public void UpdatePopupContent(View listView)
	{
		if (base.MauiContext == null)
		{
			throw new InvalidOperationException("MauiContext should have been set by base class.");
		}
		popupViewExt?.UpdatePopupContent(listView.ToPlatform(base.MauiContext));
	}

	public void HidePopup()
	{
		popupViewExt?.HidePopup();
	}

	public void ShowPopup()
	{
		if (popupView != null)
		{
			popupView.IsOpen = true;
		}
		popupViewExt?.ShowPopup();
	}

	public void UpdatePopupHeight(double height)
	{
		if (popupViewExt != null)
		{
			popupViewExt.PopupHeight = height;
		}
	}

	public void UpdatePopupWidth(double width)
	{
		if (popupViewExt != null)
		{
			popupViewExt.PopupWidth = width;
		}
	}

	public void UpdatePopupX(int x)
	{
		if (popupViewExt != null)
		{
			popupViewExt.PopupX = x;
		}
	}

	public void UpdatePopupY(int y)
	{
		if (popupViewExt != null)
		{
			popupViewExt.PopupY = y;
		}
	}

	public void UpdateAnchorView(View view)
	{
		if (base.MauiContext == null)
		{
			throw new InvalidOperationException("MauiContext should have been set by base class.");
		}
		if (popupViewExt != null)
		{
			popupViewExt.AnchorView = view.ToPlatform(base.MauiContext);
		}
	}
}
