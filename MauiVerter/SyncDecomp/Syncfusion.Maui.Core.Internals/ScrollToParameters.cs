namespace Syncfusion.Maui.Core.Internals;

internal class ScrollToParameters
{
	internal double ScrollY { get; }

	internal double ScrollX { get; }

	internal bool Animated { get; }

	internal ScrollToParameters(double scrollX, double scrollY, bool animated = false)
	{
		ScrollX = scrollX;
		ScrollY = scrollY;
		Animated = animated;
	}
}
