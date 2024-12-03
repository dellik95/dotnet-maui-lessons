using Microsoft.Maui;
using Microsoft.Maui.Graphics;

namespace Syncfusion.Maui.Core.Internals;

public class PanEventArgs
{
	private readonly Point _touchPoint;

	private readonly Point _translatePoint;

	private readonly GestureStatus _status;

	private readonly Point _velocity;

	public GestureStatus Status => _status;

	public Point TranslatePoint => _translatePoint;

	public Point TouchPoint => _touchPoint;

	public Point Velocity => _velocity;

	public PanEventArgs(GestureStatus status, Point touchPoint, Point translatePoint, Point velocity)
	{
		_status = status;
		_touchPoint = touchPoint;
		_translatePoint = translatePoint;
		_velocity = velocity;
	}
}
