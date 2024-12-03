using Microsoft.Maui.Graphics;

namespace Syncfusion.Maui.Core.Internals;

public class TapEventArgs
{
	private readonly int _noOfTapCount;

	private readonly Point _tapPoint;

	public Point TapPoint => _tapPoint;

	public int TapCount => _noOfTapCount;

	public TapEventArgs(Point touchPoint, int tapCount)
	{
		_tapPoint = touchPoint;
		_noOfTapCount = tapCount;
	}
}
