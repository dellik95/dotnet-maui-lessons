using Microsoft.Maui.Graphics;

namespace Syncfusion.Maui.Core.Internals;

public class LongPressEventArgs
{
	private readonly Point _touchPoint;

	public Point TouchPoint => _touchPoint;

	public LongPressEventArgs(Point touchPoint)
	{
		_touchPoint = touchPoint;
	}
}
