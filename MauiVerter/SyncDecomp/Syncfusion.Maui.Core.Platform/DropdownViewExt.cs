using System.Numerics;
using Microsoft.Maui.Platform;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Windows.UI;

namespace Syncfusion.Maui.Core.Platform;

public class DropdownViewExt : ContentPanel
{
	private double popupHeight = 400.0;

	private double popupWidth = 0.0;

	private int popupX = 0;

	private int popupY = 0;

	internal Grid? parentGrid;

	private FrameworkElement? anchorView;

	private bool isInitialLoad;

	internal Popup? Popup { get; set; }

	internal double PopupHeight
	{
		get
		{
			return popupHeight;
		}
		set
		{
			popupHeight = value;
			if (parentGrid != null && !double.IsNaN(value))
			{
				parentGrid.MaxHeight = value;
			}
		}
	}

	internal double PopupWidth
	{
		get
		{
			return popupWidth;
		}
		set
		{
			popupWidth = value;
			if (parentGrid != null && !double.IsNaN(value) && value > 0.0)
			{
				parentGrid.MaxWidth = value;
			}
		}
	}

	internal int PopupX
	{
		get
		{
			return popupX;
		}
		set
		{
			popupX = value;
			if (Popup != null)
			{
				Popup.HorizontalOffset = value;
			}
		}
	}

	internal int PopupY
	{
		get
		{
			return popupY;
		}
		set
		{
			popupY = value;
			if (Popup != null)
			{
				Popup.VerticalOffset = value;
			}
		}
	}

	internal FrameworkElement? AnchorView
	{
		get
		{
			return anchorView;
		}
		set
		{
			anchorView = value;
			if (AnchorView != null && Popup != null)
			{
				Popup.PlacementTarget = AnchorView;
			}
		}
	}

	public DropdownViewExt()
	{
		Initialize();
	}

	internal void Initialize()
	{
		parentGrid = new Grid
		{
			CornerRadius = new CornerRadius(4.0),
			Background = new SolidColorBrush(Colors.White),
			BorderBrush = new SolidColorBrush(Color.FromArgb(byte.MaxValue, 220, 220, 220)),
			BorderThickness = new Thickness(1.0),
			Translation = new Vector3(0f, 0f, 32f),
			Shadow = new ThemeShadow(),
			MaxHeight = PopupHeight
		};
		Popup = new Popup
		{
			Child = parentGrid,
			DesiredPlacement = PopupPlacementMode.BottomEdgeAlignedLeft,
			ShouldConstrainToRootBounds = false
		};
		base.Children.Add(Popup);
		base.Loaded += PopupViewExt_Loaded;
	}

	private void PopupViewExt_Loaded(object sender, RoutedEventArgs e)
	{
		if (base.XamlRoot != null && base.XamlRoot.Content != null)
		{
			if (Popup != null)
			{
				if (Popup.XamlRoot == null)
				{
					Popup.XamlRoot = base.XamlRoot;
				}
				if (isInitialLoad)
				{
					Popup.IsOpen = true;
				}
			}
			base.XamlRoot.Content.PointerPressed += Content_PointerPressed;
		}
		isInitialLoad = false;
	}

	private void Content_PointerPressed(object sender, PointerRoutedEventArgs e)
	{
		if (Popup != null && Popup.IsOpen)
		{
			Popup.IsOpen = false;
		}
	}

	internal void ShowPopup()
	{
		if (!(Popup != null) || Popup.IsOpen)
		{
			return;
		}
		if (Popup.XamlRoot == null)
		{
			Popup.XamlRoot = base.XamlRoot;
			if (AnchorView != null && Popup.PlacementTarget == null)
			{
				Popup.PlacementTarget = AnchorView;
			}
		}
		if (Popup.XamlRoot != null)
		{
			Popup.IsOpen = true;
		}
		else
		{
			isInitialLoad = true;
		}
	}

	internal void HidePopup()
	{
		if (Popup != null && Popup.IsOpen)
		{
			Popup.IsOpen = false;
		}
	}

	internal void UpdatePopupContent(FrameworkElement view)
	{
		if (parentGrid != null && !parentGrid.Children.Contains(view))
		{
			parentGrid.Children.Add(view);
		}
	}

	internal void Dispose()
	{
		base.Loaded -= PopupViewExt_Loaded;
		if (AnchorView != null)
		{
			AnchorView = null;
		}
		if (base.XamlRoot != null && base.XamlRoot.Content != null)
		{
			base.XamlRoot.Content.PointerPressed -= Content_PointerPressed;
		}
	}
}
