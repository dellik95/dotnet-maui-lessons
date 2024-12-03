using System;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Layouts;

namespace Syncfusion.Maui.Core.Internals;

internal abstract class AreaBase : AbsoluteLayout, IArea
{
	private const double maxSize = 8388607.5;

	private readonly CoreScheduler coreScheduler;

	private Rect areaBounds;

	public Rect AreaBounds
	{
		get
		{
			return areaBounds;
		}
		set
		{
			if (areaBounds != value)
			{
				if (PlotArea != null)
				{
					PlotArea.PlotAreaBounds = value;
				}
				areaBounds = value;
				NeedsRelayout = true;
			}
		}
	}

	public abstract IPlotArea PlotArea { get; }

	public bool NeedsRelayout { get; set; } = false;


	IPlotArea IArea.PlotArea => PlotArea;

	public AreaBase()
	{
		coreScheduler = CoreScheduler.CreateScheduler();
	}

	protected virtual void UpdateAreaCore()
	{
	}

	protected override Size MeasureOverride(double widthConstraint, double heightConstraint)
	{
		Size desiredSize = this.ComputeDesiredSize(widthConstraint, heightConstraint);
		bool flag = double.IsPositiveInfinity(heightConstraint) && Math.Round(desiredSize.Height) <= 1.0;
		bool flag2 = double.IsPositiveInfinity(widthConstraint) && Math.Round(desiredSize.Width) <= 1.0;
		if (flag || flag2)
		{
			base.DesiredSize = new Size(flag2 ? 300.0 : desiredSize.Width, flag ? 300.0 : desiredSize.Height);
		}
		else
		{
			base.DesiredSize = desiredSize;
		}
		return base.DesiredSize;
	}

	protected override Size ArrangeOverride(Rect bounds)
	{
		if (!AreaBounds.Equals(bounds) && bounds.Width != 8388607.5 && bounds.Height != 8388607.5)
		{
			AreaBounds = bounds;
			if (bounds.Width > 0.0 && bounds.Height > 0.0)
			{
				ScheduleUpdateArea();
			}
		}
		return base.ArrangeOverride(bounds);
	}

	public void ScheduleUpdateArea()
	{
		if (NeedsRelayout && !areaBounds.IsEmpty)
		{
			coreScheduler.ScheduleCallback(UpdateArea);
		}
	}

	private void UpdateArea()
	{
		PlotArea.UpdateLegendItems();
		UpdateAreaCore();
		NeedsRelayout = false;
	}
}
