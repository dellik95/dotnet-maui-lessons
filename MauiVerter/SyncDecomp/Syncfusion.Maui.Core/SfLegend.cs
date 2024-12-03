using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace Syncfusion.Maui.Core;

internal class SfLegend : SfView, ILegend
{
	private const double maxSize = 8388607.5;

	private readonly CollectionView legendView;

	public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(nameof(ItemsSource), typeof(IEnumerable), typeof(SfLegend), null, BindingMode.Default);

	public static readonly BindableProperty ToggleVisibilityProperty = BindableProperty.Create(nameof(ToggleVisibility), typeof(bool), typeof(SfLegend), false, BindingMode.Default, null, OnToggleVisibilityChanged);

	internal static readonly BindableProperty ItemTemplateProperty = BindableProperty.Create(nameof(ItemTemplate), typeof(Microsoft.Maui.Controls.DataTemplate), typeof(SfLegend), null, BindingMode.Default, null, OnItemTemplateChanged);

	internal static readonly BindableProperty PlacementProperty = BindableProperty.Create(nameof(Placement), typeof(LegendPlacement), typeof(SfLegend), LegendPlacement.Top, BindingMode.Default, null, OnPlacementChanged);

	internal static readonly BindableProperty ItemsLayoutProperty = BindableProperty.Create(nameof(ItemsLayout), typeof(IItemsLayout), typeof(SfLegend), null, BindingMode.Default, null, OnOrientationChanged);

	public IEnumerable ItemsSource
	{
		get
		{
			return (IEnumerable)GetValue(ItemsSourceProperty);
		}
		set
		{
			SetValue(ItemsSourceProperty, value);
		}
	}

	public bool ToggleVisibility
	{
		get
		{
			return (bool)GetValue(ToggleVisibilityProperty);
		}
		set
		{
			SetValue(ToggleVisibilityProperty, value);
		}
	}

	internal Microsoft.Maui.Controls.DataTemplate ItemTemplate
	{
		get
		{
			return (Microsoft.Maui.Controls.DataTemplate)GetValue(ItemTemplateProperty);
		}
		set
		{
			SetValue(ItemTemplateProperty, value);
		}
	}

	internal LegendPlacement Placement
	{
		get
		{
			return (LegendPlacement)GetValue(PlacementProperty);
		}
		set
		{
			SetValue(PlacementProperty, value);
		}
	}

	internal IItemsLayout ItemsLayout
	{
		get
		{
			return (IItemsLayout)GetValue(ItemsLayoutProperty);
		}
		set
		{
			SetValue(ItemsLayoutProperty, value);
		}
	}

	LegendPlacement ILegend.Placement
	{
		get
		{
			return Placement;
		}
		set
		{
		}
	}

	IItemsLayout ILegend.ItemsLayout
	{
		get
		{
			return ItemsLayout;
		}
		set
		{
		}
	}

	Microsoft.Maui.Controls.DataTemplate ILegend.ItemTemplate
	{
		get
		{
			return ItemTemplate;
		}
		set
		{
		}
	}

	bool ILegend.IsVisible { get; set; }

	internal event EventHandler<LegendItemClickedEventArgs>? ItemClicked;

	public SfLegend()
	{
		legendView = new CollectionView();
		legendView.HorizontalOptions = LayoutOptions.Center;
		legendView.VerticalOptions = LayoutOptions.Center;
		legendView.BindingContext = this;
		legendView.SetBinding(ItemsView.ItemsSourceProperty, "ItemsSource");
		legendView.SelectionMode = Microsoft.Maui.Controls.SelectionMode.None;
		legendView.HorizontalScrollBarVisibility = Microsoft.Maui.ScrollBarVisibility.Never;
		legendView.VerticalScrollBarVisibility = Microsoft.Maui.ScrollBarVisibility.Never;
		legendView.ItemsLayout = LinearItemsLayout.Horizontal;
		legendView.HandlerChanged += CollectionView_HandlerChanged;
		UpdateLegendTemplate();
		base.Children.Add(legendView);
	}

	public static Rect GetLegendRectangle(SfLegend legend, Rect availableSize, double maxSizePercentage)
	{
		if (legend != null)
		{
			Size zero = Size.Zero;
			double num = 8388607.5;
			double num2 = 0.0;
			double num3 = 0.0;
			switch (legend.Placement)
			{
			case LegendPlacement.Top:
				zero = legend.Measure(availableSize.Width, double.PositiveInfinity, MeasureFlags.IncludeMargins);
				num3 = ((availableSize.Height * maxSizePercentage < zero.Height) ? (availableSize.Height * maxSizePercentage) : zero.Height);
				num2 = ((availableSize.Height != num) ? (availableSize.Height - num3) : 0.0);
				return new Rect(availableSize.X, availableSize.Y, availableSize.Width, num3);
			case LegendPlacement.Bottom:
				zero = legend.Measure(availableSize.Width, double.PositiveInfinity, MeasureFlags.IncludeMargins);
				num3 = ((availableSize.Height * maxSizePercentage < zero.Height) ? (availableSize.Height * maxSizePercentage) : zero.Height);
				num2 = ((availableSize.Height != num) ? (availableSize.Height - num3) : 0.0);
				return new Rect(availableSize.X, availableSize.Y + num2, availableSize.Width, num3);
			case LegendPlacement.Left:
				zero = legend.Measure(double.PositiveInfinity, availableSize.Height, MeasureFlags.IncludeMargins);
				num3 = ((availableSize.Width * maxSizePercentage < zero.Width) ? (availableSize.Width * maxSizePercentage) : zero.Width);
				return new Rect(availableSize.X, availableSize.Y, num3, availableSize.Height);
			case LegendPlacement.Right:
				zero = legend.Measure(double.PositiveInfinity, availableSize.Height, MeasureFlags.IncludeMargins);
				num3 = ((availableSize.Width * maxSizePercentage < zero.Width) ? (availableSize.Width * maxSizePercentage) : zero.Width);
				num2 = ((availableSize.Width != num) ? (availableSize.Width - num3) : 0.0);
				return new Rect(availableSize.X + num2, availableSize.Y, num3, availableSize.Height);
			}
		}
		return Rect.Zero;
	}

	protected virtual SfShapeView CreateShapeView()
	{
		return new SfShapeView();
	}

	protected virtual Label CreateLabelView()
	{
		return new Label();
	}

	private static void OnItemTemplateChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (bindable is SfLegend sfLegend)
		{
			sfLegend.OnItemTemplateChanged(oldValue, newValue);
		}
	}

	private static void OnPlacementChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (bindable is SfLegend sfLegend)
		{
			sfLegend.UpdateLegendPlacement();
		}
	}

	private static void OnToggleVisibilityChanged(BindableObject bindable, object oldValue, object newValue)
	{
	}

	private static void OnOrientationChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (bindable is SfLegend sfLegend)
		{
			sfLegend.legendView.ItemsLayout = newValue as LinearItemsLayout;
		}
	}

	private void CollectionView_HandlerChanged(object? sender, EventArgs e)
	{
		if (sender is CollectionView collectionView)
		{
			IViewHandler handler = collectionView.Handler;
			if (handler != null && handler.PlatformView is Microsoft.UI.Xaml.Controls.ListView listView)
			{
				listView.ItemContainerStyle?.Setters.Add(new Microsoft.UI.Xaml.Setter(FrameworkElement.MinWidthProperty, 5));
				listView.ItemContainerTransitions = null;
			}
		}
	}

	internal void UpdateLegendPlacement()
	{
		if (ItemsLayout == null)
		{
			if (Placement == LegendPlacement.Top || Placement == LegendPlacement.Bottom)
			{
				ItemsLayout = LinearItemsLayout.Horizontal;
			}
			else
			{
				ItemsLayout = LinearItemsLayout.Vertical;
			}
			legendView.ItemsLayout = ItemsLayout;
		}
	}

	public void LegendTappedAction(LegendItem legendItem)
	{
		if (ToggleVisibility && legendItem != null)
		{
			legendItem.IsToggled = !legendItem.IsToggled;
			if (this.ItemClicked != null)
			{
				this.ItemClicked(this, new LegendItemClickedEventArgs
				{
					LegendItem = legendItem
				});
			}
		}
	}

	private Microsoft.Maui.Controls.DataTemplate GetDefaultLegendTemplate()
	{
		return new Microsoft.Maui.Controls.DataTemplate(delegate
		{
			HorizontalStackLayout horizontalStackLayout = new HorizontalStackLayout
			{
				Spacing = 6.0,
				Padding = new Microsoft.Maui.Thickness(8.0, 10.0)
			};
			ToggleColorConverter converter = new ToggleColorConverter();
			SfShapeView sfShapeView = CreateShapeView();
			if (sfShapeView != null)
			{
				sfShapeView.HorizontalOptions = LayoutOptions.Start;
				sfShapeView.VerticalOptions = LayoutOptions.Center;
				Binding item = new Binding("IsToggled")
				{
					Converter = converter,
					ConverterParameter = sfShapeView
				};
				Binding item2 = new Binding("IconBrush");
				MultiBinding binding = new MultiBinding
				{
					Bindings = new List<BindingBase> { item, item2 },
					Converter = new MultiBindingIconBrushConverter(),
					ConverterParameter = sfShapeView
				};
				sfShapeView.SetBinding(SfShapeView.IconBrushProperty, binding);
				sfShapeView.SetBinding(SfShapeView.ShapeTypeProperty, "IconType");
				sfShapeView.SetBinding(VisualElement.HeightRequestProperty, "IconHeight");
				sfShapeView.SetBinding(VisualElement.WidthRequestProperty, "IconWidth");
				horizontalStackLayout.Children.Add(sfShapeView);
			}
			Label label = CreateLabelView();
			if (label != null)
			{
				label.VerticalTextAlignment = Microsoft.Maui.TextAlignment.Center;
				label.SetBinding(Label.TextProperty, "Text");
				Binding item = new Binding("IsToggled")
				{
					Converter = converter,
					ConverterParameter = label
				};
				label.SetBinding(Label.TextColorProperty, item);
				label.SetBinding(View.MarginProperty, "TextMargin");
				label.SetBinding(Label.FontSizeProperty, "FontSize");
				label.SetBinding(Label.FontFamilyProperty, "FontFamily");
				label.SetBinding(Label.FontAttributesProperty, "FontAttributes");
				horizontalStackLayout.Children.Add(label);
			}
			return horizontalStackLayout;
		});
	}

	private void UpdateLegendTemplate()
	{
		Microsoft.Maui.Controls.DataTemplate itemTemplate = new Microsoft.Maui.Controls.DataTemplate(() => new LegendItemView(LegendTappedAction)
		{
			ItemTemplate = GetDefaultLegendTemplate()
		});
		legendView.ItemTemplate = itemTemplate;
	}

	private void OnItemTemplateChanged(object oldValue, object newValue)
	{
		if (object.Equals(oldValue, newValue) || legendView == null)
		{
			return;
		}
		if (newValue != null)
		{
			Microsoft.Maui.Controls.DataTemplate dataTemplate = newValue as Microsoft.Maui.Controls.DataTemplate;
			if (dataTemplate != null)
			{
				Microsoft.Maui.Controls.DataTemplate itemTemplate = new Microsoft.Maui.Controls.DataTemplate(() => new LegendItemView(LegendTappedAction)
				{
					ItemTemplate = dataTemplate
				});
				legendView.ItemTemplate = itemTemplate;
				return;
			}
		}
		UpdateLegendTemplate();
	}
}
