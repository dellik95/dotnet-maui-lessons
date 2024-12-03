using System.Runtime.Versioning;
using Microsoft.Maui.Graphics;

namespace Syncfusion.Maui.Core.Internals;

public class PointerEventArgs
{
	public long Id { get; private set; }

	public PointerActions Action { get; private set; }

	public Point TouchPoint { get; private set; }

	public PointerDeviceType PointerDeviceType { get; internal set; } = PointerDeviceType.Touch;


	public bool IsLeftButtonPressed { get; internal set; } = false;


	[UnsupportedOSPlatform("MACCATALYST")]
	public bool IsRightButtonPressed { get; internal set; } = false;


	public PointerEventArgs(long id, PointerActions action, Point touchPoint)
	{
		Id = id;
		Action = action;
		TouchPoint = touchPoint;
	}

	public PointerEventArgs(long id, PointerActions action, PointerDeviceType deviceType, Point touchPoint)
	{
		Id = id;
		Action = action;
		TouchPoint = touchPoint;
		PointerDeviceType = deviceType;
	}
}
