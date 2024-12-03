using Microsoft.Maui;
using Microsoft.Maui.Graphics;

namespace Syncfusion.Maui.Core.Internals;

public class PinchEventArgs
{
	private readonly Point _touchPoint;

	private readonly double _angle;

	private readonly float _scale;

	private readonly GestureStatus _status;

	public GestureStatus Status => _status;

	public float Scale => _scale;

	public Point TouchPoint => _touchPoint;

	public double Angle => _angle;

	public PinchEventArgs(GestureStatus status, Point touchPoint, double angle, float scale)
	{
		_status = status;
		_touchPoint = touchPoint;
		_scale = scale;
		_angle = angle;
	}
}
