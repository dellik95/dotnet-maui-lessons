using Microsoft.Maui.Graphics;

namespace Syncfusion.Maui.Core.Internals;

public class ScrollEventArgs
{
	public long PointerID { get; private set; }

	public double ScrollDelta { get; private set; }

	public Point TouchPoint { get; private set; }

	internal bool Handled { get; set; }

	public ScrollEventArgs(long id, Point origin, double direction)
	{
		PointerID = id;
		TouchPoint = origin;
		ScrollDelta = direction;
	}
}
