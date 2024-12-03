using System;
using System.Collections.ObjectModel;
using Microsoft.Maui.Graphics;

namespace Syncfusion.Maui.Core.Internals;

internal interface IPlotArea
{
	ILegend? Legend { get; set; }

	ReadOnlyObservableCollection<ILegendItem> LegendItems { get; }

	Rect PlotAreaBounds { get; set; }

	bool ShouldPopulateLegendItems { get; set; }

	LegendHandler LegendItemToggleHandler
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	event EventHandler<LegendItemEventArgs> LegendItemToggled
	{
		add
		{
			throw new NotImplementedException();
		}
		remove
		{
			throw new NotImplementedException();
		}
	}

	event EventHandler<EventArgs> LegendItemsUpdated
	{
		add
		{
			throw new NotImplementedException();
		}
		remove
		{
			throw new NotImplementedException();
		}
	}

	void UpdateLegendItems()
	{
	}
}
