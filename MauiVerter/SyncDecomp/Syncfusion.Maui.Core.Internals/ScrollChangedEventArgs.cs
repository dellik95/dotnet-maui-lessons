using System;

namespace Syncfusion.Maui.Core.Internals;

public class ScrollChangedEventArgs : EventArgs
{
	internal double OldScrollX { get; }

	internal double OldScrollY { get; }

	internal double ScrollX { get; }

	internal double ScrollY { get; }

	public double VerticalChange { get; private set; }

	public double HorizontalChange { get; private set; }

	internal ScrollChangedEventArgs(double scrollX, double scrollY, double oldScrollX, double oldScrollY)
	{
		OldScrollX = oldScrollX;
		OldScrollY = oldScrollY;
		ScrollX = scrollX;
		ScrollY = scrollY;
		HorizontalChange = scrollX - oldScrollX;
		VerticalChange = scrollY - oldScrollY;
	}
}
