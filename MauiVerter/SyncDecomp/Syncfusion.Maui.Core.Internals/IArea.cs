using Microsoft.Maui.Graphics;

namespace Syncfusion.Maui.Core.Internals;

internal interface IArea
{
	Rect AreaBounds { get; set; }

	bool NeedsRelayout { get; set; }

	IPlotArea PlotArea { get; }

	void ScheduleUpdateArea();
}
