using System;
using System.Collections.Specialized;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace Syncfusion.Maui.Core.Internals;

internal class LegendLayout : AbsoluteLayout
{
	private ILegend? legend;

	private IPlotArea plotArea;

	private SfLegend? legendItemsView;

	internal readonly AreaBase AreaBase;

	public ILegend? Legend
	{
		get
		{
			return legend;
		}
		internal set
		{
			if (legend != value)
			{
				legend = value;
				plotArea.Legend = value;
				CreateLegendView();
			}
		}
	}

	public LegendLayout(AreaBase? area)
	{
		if (area == null)
		{
			throw new ArgumentNullException("Chart area cannot be null");
		}
		AreaBase = area;
		IView areaBase = AreaBase;
		if (areaBase == null)
		{
			throw new ArgumentException("Chart area should be a view");
		}
		plotArea = AreaBase.PlotArea;
		INotifyCollectionChanged legendItems = plotArea.LegendItems;
		if (legendItems != null)
		{
			legendItems.CollectionChanged += OnLegendItemsCollectionChanged;
		}
		plotArea.LegendItemsUpdated += OnLegendItemsUpdated;
		Add(areaBase);
	}

	protected override Size ArrangeOverride(Rect bounds)
	{
		plotArea.UpdateLegendItems();
		Rect bounds2 = new Rect(0.0, 0.0, bounds.Width, bounds.Height);
		if (legend != null)
		{
			if (legendItemsView != null)
			{
				Rect legendRectangle = SfLegend.GetLegendRectangle(legendItemsView, new Rect(0.0, 0.0, bounds.Width, bounds.Height), 0.25);
				if (legendItemsView.Placement == LegendPlacement.Top)
				{
					AbsoluteLayout.SetLayoutBounds((BindableObject)legendItemsView, new Rect(0.0, 0.0, bounds.Width, legendRectangle.Height));
					bounds2 = new Rect(0.0, legendRectangle.Height, bounds.Width, bounds.Height - legendRectangle.Height);
				}
				else if (legendItemsView.Placement == LegendPlacement.Bottom)
				{
					AbsoluteLayout.SetLayoutBounds((BindableObject)legendItemsView, new Rect(0.0, bounds.Height - legendRectangle.Height, bounds.Width, legendRectangle.Height));
					bounds2 = new Rect(0.0, 0.0, bounds.Width, bounds.Height - legendRectangle.Height);
				}
				else if (legendItemsView.Placement == LegendPlacement.Left)
				{
					AbsoluteLayout.SetLayoutBounds((BindableObject)legendItemsView, new Rect(0.0, 0.0, legendRectangle.Width, bounds.Height));
					bounds2 = new Rect(legendRectangle.Width, 0.0, bounds.Width - legendRectangle.Width, bounds.Height);
				}
				else if (legendItemsView.Placement == LegendPlacement.Right)
				{
					AbsoluteLayout.SetLayoutBounds((BindableObject)legendItemsView, new Rect(bounds.Width - legendRectangle.Width, 0.0, legendRectangle.Width, bounds.Height));
					bounds2 = new Rect(0.0, 0.0, bounds.Width - legendRectangle.Width, bounds.Height);
				}
				AbsoluteLayout.SetLayoutBounds((BindableObject)AreaBase, bounds2);
			}
		}
		else
		{
			AbsoluteLayout.SetLayoutBounds((BindableObject)AreaBase, bounds2);
		}
		return base.ArrangeOverride(bounds);
	}

	private void OnLegendItemsCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
	{
	}

	private void OnLegendItemsUpdated(object? sender, EventArgs e)
	{
		if (Legend == null)
		{
		}
	}

	private void CreateLegendView()
	{
		if (legendItemsView != null)
		{
			Remove(legendItemsView);
		}
		if (Legend != null)
		{
			SfLegend sfLegend = Legend.CreateLegendView();
			legendItemsView = sfLegend ?? new SfLegend();
			legendItemsView.ToggleVisibility = Legend.ToggleVisibility;
			legendItemsView.BindingContext = Legend;
			legendItemsView.SetBinding(SfLegend.ItemTemplateProperty, "ItemTemplate");
			legendItemsView.SetBinding(SfLegend.ItemsLayoutProperty, "ItemsLayout");
			legendItemsView.SetBinding(SfLegend.PlacementProperty, "Placement");
			legendItemsView.ItemsSource = plotArea.LegendItems;
			legendItemsView.ItemClicked += OnLegendItemToggled;
			Add(legendItemsView);
		}
	}

	private void OnLegendItemToggled(object? sender, LegendItemClickedEventArgs e)
	{
		ToggleLegendItem(e.LegendItem);
	}

	private void ToggleLegendItem(ILegendItem? legendItem)
	{
		if (legendItem != null)
		{
			plotArea.LegendItemToggleHandler(legendItem);
		}
	}
}
